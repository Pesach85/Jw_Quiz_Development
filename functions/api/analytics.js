/**
 * /api/analytics — Cloudflare Pages Function
 * Handles: presence heartbeat, story events, and admin stats.
 *
 * KV keys (in JWQUIZ_DATA):
 *   presence_{safeClientId}     → {ts, storyId, language}  TTL=120s
 *   session24_{safeClientId}    → "1"                       TTL=86400s (unique daily session)
 *   analytics_total_views       → number (string)
 *   analytics_total_completions → number (string)
 *   analytics_total_sessions    → number (string)
 *
 * Env variable:
 *   ADMIN_SECRET  → shared secret required for admin_stats queries
 *                   Set in Cloudflare Pages → Settings → Environment Variables
 */

const PRESENCE_PREFIX = "presence_";
const PRESENCE_TTL_SECONDS = 120;

function json(data, status) {
  return new Response(JSON.stringify(data), {
    status: status || 200,
    headers: {
      "content-type": "application/json; charset=utf-8",
      "Access-Control-Allow-Origin": "*"
    }
  });
}

async function countOnlineUsers(env) {
  try {
    const list = await env.JWQUIZ_DATA.list({ prefix: PRESENCE_PREFIX });
    return list.keys.length;
  } catch (_e) {
    return 0;
  }
}

async function getStat(env, key) {
  const val = await env.JWQUIZ_DATA.get(key);
  return Number(val || "0");
}

async function incrementStat(env, key) {
  const current = await getStat(env, key);
  await env.JWQUIZ_DATA.put(key, String(current + 1));
}

export async function onRequestPost(context) {
  const { env, request } = context;
  if (!env.JWQUIZ_DATA) {
    return json({ error: "KV non disponibile." }, 503);
  }

  let body;
  try {
    body = await request.json();
  } catch (_e) {
    return json({ error: "Payload non valido." }, 400);
  }

  const { type, clientId, storyId, language, secret } = body || {};

  // ── Admin stats query ────────────────────────────────────────────────────
  if (type === "admin_stats") {
    const adminSecret = env.ADMIN_SECRET || "";
    if (!adminSecret || String(secret || "") !== adminSecret) {
      return json({ error: "Non autorizzato." }, 403);
    }
    const [online, views, completions, sessions] = await Promise.all([
      countOnlineUsers(env),
      getStat(env, "analytics_total_views"),
      getStat(env, "analytics_total_completions"),
      getStat(env, "analytics_total_sessions")
    ]);
    return json({ online, views, completions, sessions });
  }

  // ── Validate clientId for all event types ────────────────────────────────
  if (!clientId || typeof clientId !== "string") {
    return json({ error: "clientId mancante." }, 400);
  }
  const safeId = String(clientId).replace(/[^a-z0-9_]/g, "").slice(0, 80);
  if (safeId.length < 6) {
    return json({ error: "clientId non valido." }, 400);
  }

  // ── Heartbeat / presence ─────────────────────────────────────────────────
  if (type === "heartbeat") {
    const presenceKey = PRESENCE_PREFIX + safeId;
    const payload = {
      ts: Date.now(),
      storyId: Number(storyId) || 0,
      language: String(language || "Italian").replace(/[^A-Za-z]/g, "").slice(0, 20)
    };
    await env.JWQUIZ_DATA.put(presenceKey, JSON.stringify(payload), {
      expirationTtl: PRESENCE_TTL_SECONDS
    });

    // Count unique sessions in 24h window (one increment per client per day)
    const sessionKey = "session24_" + safeId;
    const existingSession = await env.JWQUIZ_DATA.get(sessionKey);
    if (!existingSession) {
      await env.JWQUIZ_DATA.put(sessionKey, "1", { expirationTtl: 86400 });
      await incrementStat(env, "analytics_total_sessions");
    }

    const online = await countOnlineUsers(env);
    return json({ online });
  }

  // ── Story view event ─────────────────────────────────────────────────────
  if (type === "story_view") {
    await incrementStat(env, "analytics_total_views");
    return json({ ok: true });
  }

  // ── Story complete event ─────────────────────────────────────────────────
  if (type === "story_complete") {
    await incrementStat(env, "analytics_total_completions");
    return json({ ok: true });
  }

  return json({ error: "Tipo evento non riconosciuto." }, 400);
}

export async function onRequestOptions() {
  return new Response(null, {
    headers: {
      "Access-Control-Allow-Origin": "*",
      "Access-Control-Allow-Methods": "POST, OPTIONS",
      "Access-Control-Allow-Headers": "Content-Type"
    }
  });
}
