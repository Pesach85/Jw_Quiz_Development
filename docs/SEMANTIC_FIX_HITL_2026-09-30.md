# HITL — Semantic image fix decisions — 2026-09-30

**Input:** `docs/SEMANTIC_AUDIT_2026-09-30.md`  
**Stato:** **APPLICATO** 2026-09-30 (Fase 1–6).  
**Decisioni utente:** E11 caption A1+A4 + regen B1; Ep12 `1F5FA` / Ep18 `1F932-1F3FC` (no-dup HITL).  
**Vincoli:** versetti intatti; `app.js` intatto.

---

## SEZIONE 1 — Ep 11 fix

### Contesto breve
Ep 11 (Noè): caption animalistiche disallineate dalle PNG; slot 6 è unico emoji-legacy nel catalogo; `1F413.png` è visualmente una **capra** nonostante il codepoint Unicode rooster.

### Anomalie e opzioni

| # | slot | key attuale | caption attuale | problema | opzioni |
|---|------|-------------|-----------------|----------|---------|
| **E11-1** | 6 | `1F410` | Altre mucche (coppia per coppia) | STYLE: emoji/silhouette (13844 B) vs resto photorealistic | **A)** sostituisci PNG con asset fotorealistico; **B)** accetta stile misto; **C)** rigenera PNG via Cursor GenerateImage (capra o mucca, stile pack 2026-07-21) |
| **E11-2** | 6 | `1F410` | Altre mucche (coppia per coppia) | MISMATCH: caption “mucche”, PNG = capra | **A)** fix caption → es. “Altre capre…”; **B)** fix key/PNG → mucca (`1F404` già usato in slot 2; duplicare contenuto o nuovo asset); **C)** ridefinisci tema animali arca (allinea tutte le caption agli animali reali) |
| **E11-3** | 7 | `1F411` | Altri polli (coppia per coppia) | MISMATCH: caption “polli”, PNG = pecora | **A)** fix caption → es. “Altre pecore…”; **B)** fix key → pollo/gallo (asset `1F414` **assente**; servirebbe nuovo PNG); **C)** ridefinisci set animali (con E11-2/E11-4) |
| **E11-4** | 3 | `1F413` | I polli a coppie | MISMATCH: caption “polli”; PNG visual = **capra di montagna**; key Unicode = rooster | **A)** fix caption → “Le capre a coppie”; **B)** sostituisci PNG con gallo/pollo fotorealistico (nuovo asset; rinominare o ripuntare key); **C)** rinomina key+file a codepoint coerente + aggiorna JSON (impatto Resources + webapp + ResourceManager) |

### Dettaglio opzioni (costo / impatto / rollback)

#### E11-1 STYLE (`1F410`)

| opt | costo | impatto visivo | rollback |
|-----|-------|----------------|----------|
| A | Medio: trovare o creare PNG photoreal ~1.5–2 MB; aggiornare `Resources/1F410.png` + `webapp/assets/1F410.png` (+ .resx se embedded) | Alto positivo: omogeneità pack | Ripristinare PNG emoji da git |
| B | Zero | Nessuno (inconsistenza resta) | N/A |
| C | Medio–alto: GenerateImage + QA stile/licenza + copia dual path | Alto positivo se match pack | Come A |

**Alternative assets esistenti (evidenza audit):**  
- Photoreal goat già presente come **contenuto** di `1F413.png` (ma key nome = rooster; già in slot 3).  
- Nessun altro `*410*` photoreal.  
- `1f4111.png` = emoji-derived (13374 B) — **non** candidata STYLE fix.  
- Mucca photoreal: `1F404.png` (già slot 2).

#### E11-2 MISMATCH slot 6 (mucche vs capra)

| opt | costo | impatto visivo | rollback |
|-----|-------|----------------|----------|
| A | Basso: 1 stringa in `imageCaptionsIt[5]` | Basso (testo); stile emoji resta se E11-1=B | Revert JSON |
| B | Medio–alto: nuovo o riuso PNG mucca su `1F410` | Alto (coerenza caption); rischio “due mucche” con slot 2 | Revert PNG+JSON |
| C | Alto: riesame 4 caption animali Ep 11 | Alto (narrazione rebus) | Revert JSON |

#### E11-3 MISMATCH slot 7 (polli vs pecora)

| opt | costo | impatto visivo | rollback |
|-----|-------|----------------|----------|
| A | Basso: caption → pecore | Basso | Revert JSON |
| B | Alto: creare `1F414` o altro pollo photoreal + aggiornare `hiddenKeys[1]` | Alto | Revert key+PNG |
| C | Alto: bundle con E11-2/4 | Alto | Revert JSON |

#### E11-4 MISMATCH slot 3 (`1F413` polli vs capra visual)

| opt | costo | impatto visivo | rollback |
|-----|-------|----------------|----------|
| A | Basso: caption → capre | Basso; **allinea** caption al PNG attuale | Revert JSON |
| B | Alto: nuovo PNG gallo/pollo; sostituire contenuto `1F413.png` (o nuova key) | Alto; sistema key Unicode torna coerente | Revert PNG |
| C | Molto alto: rename key across desktop+web+stories | Alto; rischio ResourceManager | Revert multi-file |

### Pacchetto consigliato (non applicato — solo proposta)

**Minimo coerente (basso costo):** E11-2-A + E11-3-A + E11-4-A + E11-1-C  
→ caption allineate a ciò che le PNG mostrano (capra/capra/pecora) + regen photoreal per `1F410`.  
**Alternativa “tema polli”:** richiederebbe E11-4-B + E11-3-B (nuovi asset polli) — costo alto; `1F414` assente.

---

## SEZIONE 2 — Cross 23 anomalie (raggruppate)

| gruppo | episodi / slot | opzioni globali | costo |
|--------|----------------|-----------------|-------|
| **STYLE emoji-legacy** | Solo Ep **11** slot 6 (`1F410`). Unica key emoji-derived tra le 88 usate. | **A)** regen via Cursor GenerateImage (stile pack); **B)** accetta; **C)** N/A top-N (già N=1) | A medio; B zero |
| **MISMATCH caption↔visual** | Solo Ep **11** slot 3,6,7 (evidenza visuale). Nessun altro hard-lemma mismatch cross-23. | **A)** fix caption; **B)** fix key/PNG; **C)** caso per caso (Ep 11 = caso unico) | A basso; B–C medio/alto |
| **PLACEHOLDER residui** | Ep **12** slot 2 visible `2753`; Ep **18** slot 5 visible `2753` | **A)** fix come Ep 13 (`piccolo_gregge` / asset tematico); **B)** accetta placeholder come “domanda” rebus | A medio (scelta asset+caption); B zero |
| **needs_human_review (non bloccanti)** | Molti episodi: caption metaforiche senza lemma hard; Ep 2 genere Unicode ZWJ male vs “donna” | **A)** audit visuale spot; **B)** ignora finché non segnalato utente; **C)** policy genere Unicode | A variabile |

### PLACEHOLDER — dettaglio

| ep | slot | key | caption | nota |
|----|------|-----|---------|------|
| 12 | 2 | `2753` | Una domanda: cosa significa? | Caption coerente col punto interrogativo; anomalia = uso placeholder pack, non semantic animal |
| 18 | 5 | `2753` | Chi si fermerà ad aiutare? | Idem |

Opzioni Ep 12/18:  
- **A1** (12): asset “domanda/scroll/angelo” photoreal dedicato, nuova key, update `visibleKeys[1]`.  
- **A2** (18): asset “soccorritore / punto interrogativo stilizzato photoreal”, update `visibleKeys[4]`.  
- **B:** lasciare `2753` se si accetta il linguaggio “?” come indizio intenzionale.

---

## SEZIONE 3 — Summary one-line

**Ep 11: 3 MISMATCH animali + 1 STYLE (`1F410`); cross-23: solo altri hard = PLACEHOLDER su Ep 12 e 18; 20 episodi clean — fix HITL: allinea caption Ep 11 agli animali PNG e/o regen `1F410`, poi decidi placeholder 12/18 come Ep 13.**

---

## Checklist decisione founder (compilare)

| ID | Scelta (A/B/C) | Note |
|----|----------------|------|
| E11-1 | **C** (regen GenerateImage) | `ep11-goat-photo` → `1F410` 1.58 MB |
| E11-2 | **A** (caption) | “Ecco altre capre” |
| E11-3 | **A** (caption) | “Pecore in coppia” |
| E11-4 | **A** (caption) | “Una coppia di capre” |
| STYLE globale | = E11-1 C | unico emoji legacy nel catalogo |
| PLACEHOLDER 12 | **D2** → `1F5FA` | “Il cammino verso Gaza” (no-dup vs preferenza 1F4D6) |
| PLACEHOLDER 18 | **D2** → `1F932-1F3FC` | “Chi tende la mano?” (no-dup vs preferenza 1F6B6) |
| NHR / genere Ep2 | defer | fuori scope |

**Applicato 2026-09-30.** Pipeline validate/sync/parity/MSBuild exit 0.
