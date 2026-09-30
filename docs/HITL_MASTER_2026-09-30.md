# HITL Master — 2026-09-30

**Stato:** decision-ready · **nessun fix applicato**  
**HEAD base:** `36cbe87` (+ questo commit docs)  
**Fonti autonome:** questo file + `docs/IMAGES_AUDIT_2026-09-30.md` (tabella 184)  
**Come rispondere:** compila SEZIONE 4 (one-line) oppure spunta le caselle nelle sezioni 1–3.

---

## SEZIONE 1 — Ep 13 (Davide e Golia) — immagini

### Cosa mostra l'app (evidenza)

| Evidenza | Dettaglio |
|----------|-----------|
| Segnalazione | Utente: «alcune immagini sembrano non essere presenti» (8 slot, 2 con «?») |
| Screenshot path | Segnalazione in chat utente (2026-09-30); smoke repo non ha `bug2_ep13` dedicato |
| Audit file | `docs/IMAGES_AUDIT_2026-09-30.md` § Ep 13 |
| Missing file? | **No** — 0 missing su Resources/ e webapp/assets/ per le 8 key |

| slot | role | key | Resources | size | assets | size | captionIt |
|------|------|-----|-----------|------|--------|------|-----------|
| 0 | visible | `038-boy-1` | ✓ | 1449763 | ✓ | (sync) | Un giovane pastore |
| 1 | visible | `1F411` | ✓ | 1642233 | ✓ | | Un piccolo gregge |
| 2 | visible | `2694` | ✓ | 1496563 | ✓ | | Armi e combattimento |
| 3 | visible | `1F632` | ✓ | 1517031 | ✓ | | Paura nel campo |
| 4 | visible | `2753` | ✓ | 1599304 | ✓ | | Qualcosa manca in questa storia... |
| 0 | hidden | `1F480` | ✓ | 1722098 | ✓ | | La caduta del nemico |
| 1 | hidden | `1F451` | ✓ | 1592421 | ✓ | | Una vittoria inattesa |
| 0 | hint | `1F4AA-1F3FD` | ✓ | 1330462 | ✓ | | Una forza più grande delle apparenze |

**Causa percepita «immagini assenti» (deterministica):**  
`visibleKeys[4] = 2753` è il glifo Unicode **«?»**. Il PNG **esiste** (1.6 MB) ma *rappresenta* un punto interrogativo → l'utente legge «slot vuoto/mancante». I 2 «?» su hidden sono **attesi** pre-reveal (non file missing).

### Cosa dovrebbe mostrare (semantica)

Tema: pastore giovane, gregge, sfida in armi, paura, strumento di fede (fionda/pietra), caduta gigante, vittoria/re.  
Catalogo PNG tematici disponibili in `Resources/` (sottoinsieme rilevante): `038-boy-1`, `1F411`/`1f411-w`/`1f4112`, `2694`, `1F632`, `1F480`, `1F451`, `1F4AA-1F3FD`, `2753`. **Non** risultano key dedicate «fionda/pietra/gigante» oltre a queste.

### Key sospette

| key | ruolo | sospetto | alternative in assets (stesso catalogo) |
|-----|-------|----------|----------------------------------------|
| `2753` | visible[4] | placeholder «?» intenzionale + caption esplicita | sostituire con altra key esistente (es. riuso `1F632` / `2694` crea dup) — **nessuna key fionda dedicata** |
| `2694` | visible[2] | spade incrociate vs pastore | keep (armi campo) o swap con altra |
| `1F480` | hidden[0] | teschio = caduta | keep metafora |
| `1F451` | hidden[1] | corona = vittoria | keep metafora |

### Opzioni

| Opzione | Azione | Impatto | Rollback |
|---------|--------|---------|----------|
| **A** | Fix key: sostituire `2753` in Ep13 visible[4] con altra key (da scegliere) + adeguare caption | UX: niente più «?» visibile; serve key/caption approved | ripristina `2753` + caption attuale |
| **B** | Accetta: placeholder intenzionale (caption già spiega «Qualcosa manca…») | Nessun codice/dati; educa utente | n/a |
| **C** | Ridefinisci tema/visivo Ep13 (set 8 key + caption) | Effort M; rischio spoiler | revert JSON keys/captions |

**Proposta assistente:** **B** (accetta) se l'intento didattico del «?» resta; altrimenti **A** solo con key+caption espliciti dall'umano (catalogo senza fionda dedicata).

**Decisione one-line Ep13:** `EP13: A|B|C` (+ se A: `key=... caption=...`)

Decisione utente: [ ] A  [ ] B  [ ] C  [ ] defer

---

## SEZIONE 2 — E2 §2 fix wording (HITL)

Regola: **non** applicare finché non OK. Impatto tipico: anti-spoiler pre-G1 / coerenza i18n. Rollback: ripristinare `before` in `data/episodes.json` + sync.

| # | id | campo | before | after proposto | motivazione | impatto | rollback | Decisione utente |
|---|----|-------|--------|----------------|-------------|---------|----------|------------------|
| W1 | 1 | `rebus.imageCaptionsIt[6]` | La morte entrò nel mondo | Una conseguenza grave *(alt: Un mondo cambiato)* | Anti-spoiler esito | ↓ spoiler Eden | restore before | [ ] ok [ ] modifica [ ] defer |
| W2 | 1 | `rebus.imageCaptionsIt[7]` | Il serpente ingannatore | Un inganno sottile | Anti-spoiler animale | ↓ spoiler | restore | [ ] ok [ ] modifica [ ] defer |
| W3 | 2 | `rebus.imageCaptionsIt[4]` | Il tempio di Dagon | Un tempio nemico *(alt: Un edificio di culto ostile)* | Anti-spoiler culto | ↓ spoiler | restore | [ ] ok [ ] modifica [ ] defer |
| W4 | 2 | `rebus.imageCaptionsIt[6]` | La fine della forza | Una forza che viene meno | Anti-spoiler esito | ↓ spoiler | restore | [ ] ok [ ] modifica [ ] defer |
| W5 | 3 | `rebus.imageCaptionsIt[3]` | La grande città di Ninive | Una grande città | Anti-spoiler luogo | ↓ spoiler | restore | [ ] ok [ ] modifica [ ] defer |
| W6 | 3 | `immersive.intro.it` | Fuggire da Dio non funziona. La misericordia, sì. | **Proposta:** `Fuggire dalla missione non funziona.` *(misericordia resta in morale; o testo tuo)* | Soften morale in intro | chiarezza tier intro≠morale | restore | [ ] ok [ ] modifica [ ] defer |
| W7 | 4 | `rebus.imageCaptionsIt[0]` | Le pecore alla destra del Re | Due gruppi davanti al Re | Mt25 reveal | ↓ spoiler | restore | [ ] ok [ ] modifica [ ] defer |
| W8 | 4 | `rebus.imageCaptionsIt[1]` | Le capre alla sinistra | Un altro gruppo in attesa | Mt25 reveal | ↓ spoiler | restore | [ ] ok [ ] modifica [ ] defer |
| W9 | 4 | `rebus.imageCaptionsIt[2]` | Il canto della vittoria | Un canto solenne | Tone | soft | restore | [ ] ok [ ] modifica [ ] defer |
| W10 | 4 | `rebus.imageCaptionsIt[3]` | La bandiera d'Israele | Uno stendardo alzato | Ancora nazionale | soft | restore | [ ] ok [ ] modifica [ ] defer |
| W11 | 4 | `rebus.imageCaptionsIt[4]` | La corona del Regno | Un segno di autorità | Reveal Regno | ↓ spoiler | restore | [ ] ok [ ] modifica [ ] defer |
| W12 | 4 | `rebus.imageCaptionsIt[5]` | Il Figlio dell'uomo glorificato | Una figura glorificata | Titolo cristologico | ↓ spoiler | restore | [ ] ok [ ] modifica [ ] defer |
| W13 | 4 | `rebus.imageCaptionsIt[6]` | L'arpa del giudizio | Uno strumento solenne | Reveal | soft | restore | [ ] ok [ ] modifica [ ] defer |
| W14 | 4 | `rebus.imageCaptionsIt[7]` | Il Giudice giusto | Chi decide con giustizia | Ruolo | ↓ spoiler | restore | [ ] ok [ ] modifica [ ] defer |
| W15 | 5 | `rebus.imageCaptionsIt[3]` | La morte dei primogeniti | Una notte terribile in Egitto | Anti-spoiler esito | ↓ spoiler | restore | [ ] ok [ ] modifica [ ] defer |
| W16 | 7 | `immersive.theaterQuote.it` | «Chi sa se non sei giunta… proprio per un tempo come questo?» — Ester 4:14 | Accorciare (stringa esatta in **D7**); scripture Ester 4:16 resta | Tier theater>scripture | coerenza tier; **tocca citazione** | restore theater | [ ] ok [ ] modifica [ ] defer |
| W17 | 7 | `rebus.imageCaptionsIt[0]` | La regina Ester | Una regina al bivio | Anti-spoiler nome | ↓ spoiler | restore | [ ] ok [ ] modifica [ ] defer |
| W18 | 7 | `rebus.imageCaptionsIt[6]` | Lo shock di Aman smascherato | Uno shock a corte | Nome+esito | ↓ spoiler | restore | [ ] ok [ ] modifica [ ] defer |
| W19 | 8 | `rebus.imageCaptionsIt[0]` | Il patriarca Abramo | Un patriarca in cammino | Anti-spoiler nome | ↓ spoiler | restore | [ ] ok [ ] modifica [ ] defer |
| W20 | 8 | `rebus.imageCaptionsIt[6]` | Isacco, il figlio promesso | Il figlio promesso *(alt: Un figlio amato)* | Anti-spoiler nome | ↓ spoiler | restore | [ ] ok [ ] modifica [ ] defer |
| W21 | 8 | `immersive.titleEn` | Abraham and Isaac | Abraham and Isaac on Mount Moriah | Parity vs titleIt Monte Moria | i18n | restore | [ ] ok [ ] modifica [ ] defer |
| W22 | 11 | `rebus.imageCaptionsIt[0]` | L'arca di salvezza | Una grande imbarcazione | Anti-spoiler arca | ↓ spoiler | restore | [ ] ok [ ] modifica [ ] defer |
| W23 | 12 | `rebus.imageCaptionsIt[0]` | Un angelo guida Filippo | Una guida improvvisa *(alt: Un inviato che guida)* | Anti-spoiler nome | ↓ spoiler | restore | [ ] ok [ ] modifica [ ] defer |
| W24 | 19 | `rebus.imageCaptionsIt[5]` | Una torre che punta in alto | Un edificio che punta in alto | Eco «Torre di Babele» | soft | restore | [ ] ok [ ] modifica [ ] defer |
| W25 | 20 | `rebus.imageCaptionsIt[5]` | Un leone potente | Una belva potente | Soft titolo Daniele | soft | restore | [ ] ok [ ] modifica [ ] defer |
| W26 | 21 | `immersive.moral.it` | Il perdono di Dio apre una nuova missione. Il passato non blocca chi risponde alla chiamata. | La conversione apre una nuova missione. Il passato non blocca chi risponde alla chiamata. | Ancora tema Conversione | coerenza tema *(opzionale)* | restore | [ ] ok [ ] modifica [ ] defer |

**Batch shortcut:** `WALL-a` = ok su W1–W26 tranne W6/W16 (dipendono da D6/D7).

---

## SEZIONE 3 — D1–D10 ambigui editoriali

| # | D# | domanda | contesto (testo attuale) | alternative | proposta | rollback | Decisione |
|---|----|---------|--------------------------|-------------|----------|----------|-----------|
| 1 | D1 | Soften calco TNM `positivamente morirai`? | Ep1 `scriptureQuoteIt`: `...positivamente morirai.' - Genesi 2:16-17 (TNM)` | **A** keep fedele · **B** soft UI (incolla testo) · **defer** | **A** — non toccare citazioni senza necessità | restore quote | [ ] A [ ] B [ ] C [ ] defer |
| 2 | D2 | ASCII→Unicode anche in scripture/solution 13–18? | §1 già su caption/hint/note (`74b61c7`). Ep13 scripture ha ancora `sapra'`/`perche'` | **A** no (solo §1) · **B** sì scripture+solution · **C** solo solutionIt | **A** finché non OK citazioni; **C** se vuoi ortografia narrative | revert campi | [ ] A [ ] B [ ] C [ ] defer |
| 3 | D3 | Zero nomi anche in hidden/hint? | Ep4+ pattern 1–12 | **A** solo visible/caption pre-G1 · **B** anche hidden/hint | **A** — scope mirato | n/a policy | [ ] A [ ] B [ ] C [ ] defer |
| 4 | D4 | Quiz possono nominare (Babele…)? | Ep19 q0: `Perché Geova intervenne a Babele?` | **A** sì OK in quiz · **B** ciechi fino a morale | **A** | restore prompt | [ ] A [ ] B [ ] C [ ] defer |
| 5 | D5 | Allineare titleEn Ep8 a Moriah? | `Abraham and Isaac` vs IT Monte Moria | **A** sì (=W21a) · **B** keep | **A** | restore titleEn | [ ] A [ ] B [ ] C [ ] defer |
| 6 | D6 | Testo esatto intro Ep3? | `Fuggire da Dio non funziona. La misericordia, sì.` | **A** `Fuggire dalla missione non funziona.` · **B** keep · **C** (incolla) | **A** | restore intro | [ ] A [ ] B [ ] C [ ] defer |
| 7 | D7 | Theater Ep7 breve + scripture 4:16? | theater len74 Ester 4:14; scripture `E se devo perire, perirò! - Ester 4:16 (TNM)` | **A** keep theater · **B** `«…per un tempo come questo?» — Ester 4:14` · **C** (incolla); scripture resta **sì/no** | **A** o **B** se vuoi bilanciare tier; scripture **resta** | restore theater | [ ] A [ ] B [ ] C [ ] defer |
| 8 | D8 | Ep4 pack soft W7–W14? | 8 caption major | **A** tutte W*a* · **B** set alternativo (incolla) · **C** defer Ep4 | **A** se anti-spoiler priorità | restore 8 caps | [ ] A [ ] B [ ] C [ ] defer |
| 9 | D9 | Morale Ep19 nominare «orgoglio»? | morale: `...pieno di sé. L'umiltà apre...`; keyword `Orgoglio` | **A** keep · **B** apri con «L'orgoglio…» | **A** | restore morale | [ ] A [ ] B [ ] C [ ] defer |
| 10 | D10 | solutionIt 13–18 solo ASCII accenti? | se D2≠B | **A** sì passata ortografica solution · **B** no tocco · **C** includere in D2B | **B** se D2=A; **A** se vuoi cleanup narrative | restore solution | [ ] A [ ] B [ ] C [ ] defer |

### Anomalie immagini correlate (non wording)

| ID | Tipo | Ep | Dettaglio | Decisione suggerita |
|----|------|----|-----------|---------------------|
| IMG-2753 | placeholder visible | 12, **13**, 18 | key `2753` presente come PNG «?» | Ep13 → SEZIONE 1; 12/18: [ ] keep [ ] fix later |
| IMG-hint-dup | hint==hidden | 10,12,19–23 | pattern documentato H4b (warn Ep10 accettato) | [ ] keep policy [ ] review |
| IMG-miss | missing file | — | **0** | n/a |

---

## SEZIONE 4 — Riepilogo decisioni (≤5 min)

Incolla in chat:

```text
EP13: B
W: W1ok W2ok W3ok W4ok W5ok W6ok W7ok W8ok W9ok W10ok W11ok W12ok W13ok W14ok W15ok W16defer W17ok W18ok W19ok W20ok W21ok W22ok W23ok W24ok W25ok W26defer
D: D1A D2A D3A D4A D5A D6A D7A D8A D9A D10B
# se modifica: Wx:"testo esatto"  Dx:"testo esatto"
```

| Codice | Scelta | Se modifica, testo esatto |
|--------|--------|---------------------------|
| EP13 | | |
| W1–W26 | | |
| D1 | | |
| D2 | | |
| D3 | | |
| D4 | | |
| D5 | | |
| D6 | | |
| D7 | | |
| D8 | | |
| D9 | | |
| D10 | | |
| IMG-2753-12/18 | | |
| IMG-hint-dup | | |

---

## Residui post-applicazione 2026-09-30

| Voce | Stato | Nota |
|------|-------|------|
| EP13 | **APPLICATO** | `visibleKeys[4]=piccolo_gregge`; caption `Un gregge sul colle` (evita dup con V[1]) |
| W1–W26 | **APPLICATI** | after primario HITL Sez.2; W16 stringa breve tipo D7-B |
| D2 | **APPLICATO** | ASCII `'` → accenti Unicode / U+2019 su campi editoriali |
| D3 | **APPLICATO** | policy B (hidden/hint) via W1/W2/W4/W12–W14/W18/W20/W24/W25 |
| EP12/18 | **defer** | placeholder `2753` in visible — da rivedere |
| D1 | **defer** | consultare I_tuoi_versetti API (jw.org / wol.jw.org) per stile editoriale/dottrinale |
| D4–D10 | **defer** | review utente a fine sessione |

## Vincoli

- Apply eseguito secondo decisioni utente 2026-09-30.
- Residui sopra: nessun tocco finché non OK esplicito.
