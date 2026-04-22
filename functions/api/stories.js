const STORIES_KEY = "user_stories";
const STORY_COUNTER_KEY = "story_counter";
const MIN_STORY_ID = 18;

function json(data, status) {
  return new Response(JSON.stringify(data), {
    status: status || 200,
    headers: { "content-type": "application/json; charset=utf-8" }
  });
}

async function readStories(env) {
  const raw = await env.JWQUIZ_DATA.get(STORIES_KEY);
  if (!raw) {
    return [];
  }

  try {
    return JSON.parse(raw);
  } catch (error) {
    return [];
  }
}

function isAssetKeyValid(key) {
  return typeof key === "string" && /^(custom:[A-Za-z0-9._-]+|[A-Za-z0-9._-]+)$/.test(key);
}

function normalizeStoryInput(raw) {
  const visibleKeys = Array.isArray(raw.visibleKeys) ? raw.visibleKeys.slice(0, 5) : [];
  const hiddenKeys = Array.isArray(raw.hiddenKeys) ? raw.hiddenKeys.slice(0, 2) : [];
  const imageCaptions = Array.isArray(raw.imageCaptions) ? raw.imageCaptions.slice(0, 8) : [];

  if (!raw.title || !raw.scriptureReference || !raw.keyword || !raw.hint || !raw.solution) {
    throw new Error("Campi testuali obbligatori mancanti.");
  }
  if (visibleKeys.length !== 5 || hiddenKeys.length !== 2 || !raw.hintKey) {
    throw new Error("Servono 5 immagini visibili, 2 immagini nascoste e 1 immagine indizio.");
  }

  visibleKeys.concat(hiddenKeys).concat([raw.hintKey]).forEach((key) => {
    if (!isAssetKeyValid(key)) {
      throw new Error("Chiave immagine non valida: " + key);
    }
  });

  while (imageCaptions.length < 8) {
    imageCaptions.push("");
  }

  return {
    title: String(raw.title).trim(),
    scriptureReference: String(raw.scriptureReference).trim(),
    keyword: String(raw.keyword).trim(),
    hint: String(raw.hint).trim(),
    solution: String(raw.solution).trim(),
    scriptureQuote: String(raw.scriptureQuote || "").trim(),
    engagementNote: String(raw.engagementNote || "").trim(),
    visibleKeys,
    hiddenKeys,
    hintKey: String(raw.hintKey).trim(),
    imageCaptions,
    isUserCreated: true
  };
}

export async function onRequestGet(context) {
  if (!context.env.JWQUIZ_DATA) {
    return json({ error: "Binding KV JWQUIZ_DATA mancante." }, 500);
  }

  const stories = await readStories(context.env);
  return json({ stories });
}

export async function onRequestPost(context) {
  if (!context.env.JWQUIZ_DATA) {
    return json({ error: "Binding KV JWQUIZ_DATA mancante." }, 500);
  }

  try {
    const input = normalizeStoryInput(await context.request.json());
    const stories = await readStories(context.env);
    const currentCounter = Number((await context.env.JWQUIZ_DATA.get(STORY_COUNTER_KEY)) || MIN_STORY_ID);
    const nextId = Math.max(currentCounter, MIN_STORY_ID, ...stories.map((item) => Number(item.id || 0))) + 1;
    const story = Object.assign({ id: nextId }, input);

    stories.push(story);
    await context.env.JWQUIZ_DATA.put(STORIES_KEY, JSON.stringify(stories));
    await context.env.JWQUIZ_DATA.put(STORY_COUNTER_KEY, String(nextId));

    return json({ story }, 201);
  } catch (error) {
    return json({ error: error.message || "Errore salvataggio storia." }, 400);
  }
}