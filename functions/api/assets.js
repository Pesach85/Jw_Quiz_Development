const ASSETS_KEY = "custom_assets";
const MAX_FILE_SIZE = 2 * 1024 * 1024;

function json(data, status) {
  return new Response(JSON.stringify(data), {
    status: status || 200,
    headers: { "content-type": "application/json; charset=utf-8" }
  });
}

async function readAssets(env) {
  const raw = await env.JWQUIZ_DATA.get(ASSETS_KEY);
  if (!raw) {
    return [];
  }

  try {
    return JSON.parse(raw);
  } catch (error) {
    return [];
  }
}

function safeSlug(value) {
  return String(value || "asset")
    .toLowerCase()
    .replace(/\.png$/i, "")
    .replace(/[^a-z0-9]+/g, "-")
    .replace(/^-+|-+$/g, "")
    .slice(0, 48) || "asset";
}

export async function onRequestGet(context) {
  if (!context.env.JWQUIZ_DATA) {
    return json({ error: "Binding KV JWQUIZ_DATA mancante." }, 500);
  }

  const assets = await readAssets(context.env);
  return json({ assets });
}

export async function onRequestPost(context) {
  if (!context.env.JWQUIZ_DATA || !context.env.JWQUIZ_UPLOADS) {
    return json({ error: "Binding Cloudflare mancante: servono KV JWQUIZ_DATA e R2 JWQUIZ_UPLOADS." }, 500);
  }

  try {
    const formData = await context.request.formData();
    const file = formData.get("file");
    const providedName = String(formData.get("name") || "").trim();
    let objectKey;
    let asset;
    let assets;

    if (!(file instanceof File)) {
      throw new Error("File PNG mancante.");
    }
    if (file.type !== "image/png") {
      throw new Error("Sono permessi solo file PNG.");
    }
    if (file.size > MAX_FILE_SIZE) {
      throw new Error("Il PNG supera 2 MB.");
    }

    objectKey = Date.now() + "-" + safeSlug(providedName || file.name) + ".png";
    await context.env.JWQUIZ_UPLOADS.put(objectKey, await file.arrayBuffer(), {
      httpMetadata: { contentType: "image/png" }
    });

    asset = {
      key: "custom:" + objectKey,
      label: providedName || file.name.replace(/\.png$/i, ""),
      uploadedAt: new Date().toISOString(),
      builtIn: false
    };

    assets = await readAssets(context.env);
    assets.unshift(asset);
    await context.env.JWQUIZ_DATA.put(ASSETS_KEY, JSON.stringify(assets));

    return json({ asset }, 201);
  } catch (error) {
    return json({ error: error.message || "Errore upload PNG." }, 400);
  }
}