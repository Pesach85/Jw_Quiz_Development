export async function onRequestGet(context) {
  const objectKey = context.params.key;
  if (!context.env.JWQUIZ_UPLOADS) {
    return new Response("Binding R2 JWQUIZ_UPLOADS mancante.", { status: 500 });
  }
  if (!objectKey) {
    return new Response("Chiave asset mancante.", { status: 400 });
  }

  const object = await context.env.JWQUIZ_UPLOADS.get(objectKey);
  if (!object) {
    return new Response("Asset non trovato.", { status: 404 });
  }

  const headers = new Headers();
  object.writeHttpMetadata(headers);
  headers.set("etag", object.httpEtag);
  headers.set("cache-control", "public, max-age=31536000, immutable");

  return new Response(object.body, { headers });
}