# Semantic image audit — 2026-09-30 (READ ONLY)

**Scope:** caption↔key + stile PNG (non solo esistenza file).  
**Canon:** `data/episodes.json` (23 episodi).  
**Metodo:** `.local/semantic_audit_v2.py` + verifica visuale Ep 11 (PIL screenshot + browser grid).  
**Invarianti rispettate:** nessun fix; non toccati `data/episodes.json`, `webapp/`, `StoryLibrary.cs`, testi versetti; nessun deploy.

**Differenza vs** `docs/IMAGES_AUDIT_2026-09-30.md`: quello verificava solo esistenza + placeholder `2753` + duplicati. Questo audit aggiunge semantica caption↔soggetto e classificazione stile.

**Classi stile (evidenza file):**
| Classe | Criterio misurato |
|--------|-------------------|
| `photorealistic` | tipicamente ≥ ~1.2 MB, bassa trasparenza, alta varianza colore |
| `emoji-derived` | tipicamente &lt; 500 KB e/o alta trasparenza / silhouette |
| `icon-svg` | PNG piatto piccolo / SVG |
| `placeholder` | stem `2753` |

**Match semantico hard:** solo lemma animali/oggetti espliciti in caption (word-boundary) vs soggetto Unicode della key. Altrimenti `needs_human_review` (metafora/rebus — non contato come MISMATCH).

---

## 1. Ep 11 detail

### 1.1 Estratto `data/episodes.json` (id=11)

| Campo | Valore |
|-------|--------|
| `visibleKeys[5]` | `1F6A2`, `1F404`, `1F413`, `1F327`, `1F308` |
| `hiddenKeys[2]` | `1F410`, `1F411` |
| `hintKey` | `1F30A` |
| `imageCaptionsIt[8]` | Vedi tabella sotto |
| `rebus.titleIt` | Noè e il Diluvio |
| `rebus.keywordIt` | Salvezza |
| `immersive.themeIt` / `themeEn` | Salvezza / Salvation |
| `immersive.intro.it` | Obbedienza paziente… e una porta che Dio chiuse. |
| `immersive.moral.it` | La salvezza passa dall’ascoltare Dio anche quando il mondo non lo fa. |

### 1.2 Tabella slot (evidenza)

| slot | role | key | caption | Resources/ | webapp/assets/ | bytes | stile PNG | soggetto Unicode key | soggetto **visivo PNG** (verificato) | match caption↔visual | screenshot |
|------|------|-----|---------|------------|----------------|-------|-----------|----------------------|--------------------------------------|----------------------|------------|
| 1 | visible | `1F6A2` | Una grande imbarcazione | ✓ | ✓ | 1971967 | photorealistic | ship | nave/arca | yes | `.local/smoke/audit_ep11_slot1.png` |
| 2 | visible | `1F404` | Le mucche a coppie | ✓ | ✓ | 1693445 | photorealistic | cow | mucca | yes | `.local/smoke/audit_ep11_slot2.png` |
| 3 | visible | `1F413` | I polli a coppie | ✓ | ✓ | 1660371 | photorealistic | rooster (U+1F413) | **capra di montagna** | **no** | `.local/smoke/audit_ep11_slot3.png` |
| 4 | visible | `1F327` | La pioggia torrenziale per 40 giorni | ✓ | ✓ | 1895466 | photorealistic | cloud_with_rain | pioggia | yes | `.local/smoke/audit_ep11_slot4.png` |
| 5 | visible | `1F308` | L’arcobaleno del patto | ✓ | ✓ | 1520708 | photorealistic | rainbow | arcobaleno | yes | `.local/smoke/audit_ep11_slot5.png` |
| 6 | hidden | `1F410` | Altre mucche (coppia per coppia) | ✓ | ✓ | **13844** | **emoji-derived** | goat | **capra icona/silhouette** | **no** | `.local/smoke/audit_ep11_slot6.png` |
| 7 | hidden | `1F411` | Altri polli (coppia per coppia) | ✓ | ✓ | 1642233 | photorealistic | sheep | pecora | **no** | `.local/smoke/audit_ep11_slot7.png` |
| 8 | hint | `1F30A` | Le acque che coprono la terra | ✓ | ✓ | 2269640 | photorealistic | water_wave | onda | needs_human_review (lemma soft) | `.local/smoke/audit_ep11_slot8.png` |

**Board completa (PNG + caption overlay):** `.local/smoke/audit_ep11_full.png`  
**Board web (grid 8 slot + caption):** `.local/smoke/audit_ep11_web_full.png`  
**Labeled per slot:** `.local/smoke/audit_ep11_slot{N}_labeled.png`

### 1.3 Anomalie Ep 11 (lista)

| id | slot | key | caption | image file | stile | anomalia | evidenza |
|----|------|-----|---------|------------|-------|----------|----------|
| E11-A | 3 | `1F413` | I polli a coppie | `Resources/1F413.png` | photorealistic | **MISMATCH** caption “polli” vs visual **capra**; inoltre key Unicode = rooster ma PNG = capra | `audit_ep11_slot3.png` + `audit_ep11_web_full.png` |
| E11-B | 6 | `1F410` | Altre mucche (coppia per coppia) | `Resources/1F410.png` | emoji-derived | **MISMATCH** caption “mucche” vs visual **capra** | `audit_ep11_slot6.png` |
| E11-C | 6 | `1F410` | Altre mucche (coppia per coppia) | `Resources/1F410.png` | emoji-derived (13844 B, ~72% trasparenza) | **STYLE** emoji/silhouette in mezzo a 7 slot photorealistic (~1.5–2.3 MB) | `audit_ep11_slot6.png` vs slot1–5,7–8 |
| E11-D | 7 | `1F411` | Altri polli (coppia per coppia) | `Resources/1F411.png` | photorealistic | **MISMATCH** caption “polli” vs visual **pecora** | `audit_ep11_slot7.png` |

**Nota deterministica:** non esistono asset `1F414.png` (chicken Unicode) in `Resources/` né `webapp/assets/`. Unica capra photorealistica nel set usato: contenuto di `1F413.png` (nonostante il nome key).

---

## 2. Audit semantico cross 23 (visible + hidden + hint dove hard-lemma)

### 2.1 Stile PNG su tutte le key usate (88 stem unici)

| stile | conteggio stem |
|-------|----------------|
| photorealistic | 85 |
| emoji-derived | **1** (`1F410`) |
| placeholder | **1** (`2753`) |
| icon-svg | 0 |
| missing | 0 |

### 2.2 Tabella anomalie (solo hard evidence)

| id | slot | role | key | caption | anomalia | severità | note |
|----|------|------|-----|---------|----------|----------|------|
| 11 | 3 | visible | `1F413` | I polli a coppie | MISMATCH | high | visual = capra (non pollo); key Unicode rooster |
| 11 | 6 | hidden | `1F410` | Altre mucche (coppia per coppia) | MISMATCH | high | visual = capra |
| 11 | 6 | hidden | `1F410` | Altre mucche (coppia per coppia) | STYLE | high | emoji-derived 13844 B |
| 11 | 7 | hidden | `1F411` | Altri polli (coppia per coppia) | MISMATCH | high | visual = pecora |
| 12 | 2 | visible | `2753` | Una domanda: cosa significa? | PLACEHOLDER | high | stem `2753` in visible |
| 18 | 5 | visible | `2753` | Chi si fermerà ad aiutare? | PLACEHOLDER | high | stem `2753` in visible |

### 2.3 Conteggio per categoria

| categoria | count |
|-----------|-------|
| MISMATCH | 3 (tutti Ep 11) |
| STYLE | 1 (Ep 11 slot 6) |
| PLACEHOLDER | 2 (Ep 12, Ep 18) |
| **Totale anomalie hard** | **6** |

### 2.4 Heatmap anomalie per episodio

| id | mismatch | style | placeholder | totale |
|----|----------|-------|-------------|--------|
| 11 | 3 | 1 | 0 | 4 |
| 12 | 0 | 0 | 1 | 1 |
| 18 | 0 | 0 | 1 | 1 |
| altri (1–10,13–17,19–23) | 0 | 0 | 0 | 0 |

### 2.5 Episodi clean (0 anomalie hard)

`1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 13, 14, 15, 16, 17, 19, 20, 21, 22, 23`  
(**20** episodi clean; **3** con anomalie: 11, 12, 18)

### 2.6 Pattern ricorrenti

1. **STYLE emoji-legacy isolato:** una sola key `1F410` emoji-derived convivente con pack photorealistic 2026-07-21 — solo Ep 11.
2. **MISMATCH animal captions Ep 11:** le caption “polli/mucche” sono sfasate rispetto agli animali nelle PNG (capra/pecora/mucca). Lo slot visible 3 (`1F413`) ha **nome Unicode rooster** ma **contenuto PNG = capra**.
3. **PLACEHOLDER residui:** `2753` ancora in visible su Ep **12** e **18** (come segnalato post-Ep13 fix).
4. **Captions generiche/metafora:** maggioranza degli episodi non usa lemma hard-animal → `needs_human_review` per caption↔key (non elevati a MISMATCH senza evidenza visuale). Audit visuale full-catalog **non** eseguito fuori Ep 11.

### 2.7 needs_human_review (esempi, non anomalia)

- Ep 2 slot 3: caption “La donna che taglia i capelli” + key `1F487-…-2642` (ZWJ male) — possibile mismatch di genere Unicode; **needs human review** (non classificato hard senza policy genere).
- Ep 8 slot 5: “L’ariete provveduto” + `1f4112` (sheep) — hard-family `ariete`↔sheep → match yes.
- Ep 10 hidden: “Il leone… con l’agnello” + `1F981` (lion) — match sul lemma leone.

---

## 3. Artefatti

| path | ruolo |
|------|--------|
| `.local/semantic_audit_v2.py` | script read-only |
| `.local/smoke/semantic_audit_v2.json` | dump machine-readable |
| `.local/smoke/audit_ep11_*.png` | evidenza slot / full / labeled / web |
| `docs/SEMANTIC_FIX_HITL_2026-09-30.md` | decisioni HITL (Fase 3) |

**STOP:** nessun fix applicato in questo turno.
