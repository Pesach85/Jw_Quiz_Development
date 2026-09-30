# HITL decisions — i18n §2 wording + D1–D10

**Data:** 2026-09-30  
**Fonte:** `docs/I18N_QA_FIX_PROPOSAL.md` (§2 + §3)  
**Stato:** **nessun fix applicato** in questa sessione (§1 già in `74b61c7`).  
**Come rispondere:** una riga, es.  
`W1a W2a W3a W4 defer W5a … D1b D2a D3b D4a D5a D6:"…" D7:"…" D8a D9b D10a`

Legenda scelta: **a** = proposta primaria | **b** = alternativa | **ok** = approva as-is | **defer** = salta | **keep** = lascia before invariato.

---

## TABELLA 1 — Fix wording §2

| # | id | campo | before | after proposto | motivazione | OK? (scegli) |
|---|----|-------|--------|----------------|-------------|--------------|
| W1 | 1 | `rebus.imageCaptionsIt[6]` | La morte entrò nel mondo | **a)** Una conseguenza grave · **b)** Un mondo cambiato · **keep** | Anti-spoiler esito | |
| W2 | 1 | `rebus.imageCaptionsIt[7]` | Il serpente ingannatore | **a)** Un inganno sottile · **keep** | Anti-spoiler animale | |
| W3 | 2 | `rebus.imageCaptionsIt[4]` | Il tempio di Dagon | **a)** Un tempio nemico · **b)** Un edificio di culto ostile · **keep** | Anti-spoiler culto | |
| W4 | 2 | `rebus.imageCaptionsIt[6]` | La fine della forza | **a)** Una forza che viene meno · **keep** | Anti-spoiler esito | |
| W5 | 3 | `rebus.imageCaptionsIt[3]` | La grande città di Ninive | **a)** Una grande città · **keep** | Anti-spoiler luogo | |
| W6 | 3 | `immersive.intro.it` | Fuggire da Dio non funziona. La misericordia, sì. | **a)** Fuggire dalla missione non funziona. (misericordia → morale) · **b)** (scrivi tu) · **keep** · vedi anche **D6** | Soften morale in intro | |
| W7 | 4 | `rebus.imageCaptionsIt[0]` | Le pecore alla destra del Re | **a)** Due gruppi davanti al Re · **keep** · vedi **D8** | Mt25 reveal | |
| W8 | 4 | `rebus.imageCaptionsIt[1]` | Le capre alla sinistra | **a)** Un altro gruppo in attesa · **keep** | Mt25 reveal | |
| W9 | 4 | `rebus.imageCaptionsIt[2]` | Il canto della vittoria | **a)** Un canto solenne · **keep** | Tone | |
| W10 | 4 | `rebus.imageCaptionsIt[3]` | La bandiera d'Israele | **a)** Uno stendardo alzato · **keep** | Ancora | |
| W11 | 4 | `rebus.imageCaptionsIt[4]` | La corona del Regno | **a)** Un segno di autorità · **keep** | Reveal | |
| W12 | 4 | `rebus.imageCaptionsIt[5]` | Il Figlio dell'uomo glorificato | **a)** Una figura glorificata · **keep** | Titolo | |
| W13 | 4 | `rebus.imageCaptionsIt[6]` | L'arpa del giudizio | **a)** Uno strumento solenne · **keep** | Reveal | |
| W14 | 4 | `rebus.imageCaptionsIt[7]` | Il Giudice giusto | **a)** Chi decide con giustizia · **keep** | Ruolo | |
| W15 | 5 | `rebus.imageCaptionsIt[3]` | La morte dei primogeniti | **a)** Una notte terribile in Egitto · **keep** | Anti-spoiler esito | |
| W16 | 7 | `immersive.theaterQuote.it` | «Chi sa se non sei giunta… proprio per un tempo come questo?» — Ester 4:14 | **a)** accorcia (stringa in **D7**) · **keep** · **HITL citazione** | Tier theater>scripture | |
| W17 | 7 | `rebus.imageCaptionsIt[0]` | La regina Ester | **a)** Una regina al bivio · **keep** | Nome | |
| W18 | 7 | `rebus.imageCaptionsIt[6]` | Lo shock di Aman smascherato | **a)** Uno shock a corte · **keep** | Nome+esito | |
| W19 | 8 | `rebus.imageCaptionsIt[0]` | Il patriarca Abramo | **a)** Un patriarca in cammino · **keep** | Nome | |
| W20 | 8 | `rebus.imageCaptionsIt[6]` | Isacco, il figlio promesso | **a)** Il figlio promesso · **b)** Un figlio amato · **keep** | Nome | |
| W21 | 8 | `immersive.titleEn` | Abraham and Isaac | **a)** Abraham and Isaac on Mount Moriah · **keep** · = **D5** | Parity IT | |
| W22 | 11 | `rebus.imageCaptionsIt[0]` | L'arca di salvezza | **a)** Una grande imbarcazione · **keep** | Anti-spoiler | |
| W23 | 12 | `rebus.imageCaptionsIt[0]` | Un angelo guida Filippo | **a)** Una guida improvvisa · **b)** Un inviato che guida · **keep** | Nome | |
| W24 | 19 | `rebus.imageCaptionsIt[5]` | Una torre che punta in alto | **a)** Un edificio che punta in alto · **keep** | Eco titolo | |
| W25 | 20 | `rebus.imageCaptionsIt[5]` | Un leone potente | **a)** Una belva potente · **keep** | Soft titolo | |
| W26 | 21 | `immersive.moral.it` | Il perdono di Dio apre una nuova missione. … | **a)** La conversione apre una nuova missione. Il passato non blocca chi risponde alla chiamata. · **keep** · **defer** | Tema Conversione | |

**Shortcut batch (opzionale):**  
- `WALL-a` = approva tutte le **a** di W1–W26 tranne W6/W16 (citazioni/intro → D6/D7)  
- `W4-all-a` = approva W7–W14 tutte **a** (Ep4 pack)  
- `Wkeep-scripture` = keep su ogni theater/scripture (W16)

---

## TABELLA 2 — Ambigui D1–D10

| # | D# | domanda | contesto | risposta proposta | alternative | scelta |
|---|----|---------|----------|-------------------|-------------|--------|
| 1 | D1 | Soften TNM `positivamente morirai` in UI Ep1? | `rebus.scriptureQuoteIt` | **a)** keep fedele TNM | **b)** soft-parafrasi UI (scrivi testo) | |
| 2 | D2 | ASCII→Unicode anche in `scriptureQuoteIt` / dialoghi `solutionIt` 13–18? | §1 già fatto su caption/hint/note | **a)** no — solo §1 (già applicato) | **b)** sì anche scripture · **c)** sì solo `solutionIt` (non scripture) | |
| 3 | D3 | Zero nomi propri anche in hidden/hint, o solo visible pre-G1? | Ep4 + pattern 1–12 | **a)** solo visible/caption UI pre-reveal | **b)** anche hidden/hint | |
| 4 | D4 | Quiz prompt possono nominare (es. Babele)? | Ep19 q0 | **a)** sì, OK in quiz | **b)** ciechi fino a morale | |
| 5 | D5 | Allineare `titleEn` Ep8 a Moriah? | = W21 | **a)** sì (W21a) | **b)** keep | |
| 6 | D6 | Testo esatto intro Ep3 after? | W6 | **a)** `Fuggire dalla missione non funziona.` | **b)** keep · **c)** (incolla tua frase) | |
| 7 | D7 | Stringa breve theater Ep7? Scripture 4:16 resta? | W16; len 74→breve | **a)** keep theater · **b)** `«…per un tempo come questo?» — Ester 4:14` · **c)** (incolla) · scripture **resta** sì/no | |
| 8 | D8 | Ep4: pack soft W7–W14? | 8 caption major | **a)** tutte W*a* · **b)** set alternativo (incolla) · **c)** defer Ep4 | |
| 9 | D9 | Morale Ep19: nominare «orgoglio»? | keywordIt=Orgoglio | **a)** keep «pieno di sé / umiltà» | **b)** apri con «L'orgoglio…» | |
| 10 | D10 | `solutionIt` 13–18: solo ASCII accenti, no retouch? | se D2≠b | **a)** sì passata ortografica solution only | **b)** no tocco solution · **c)** includere in D2b | |

---

## Risposta (incolla qui / in chat)

```text
# esempio:
W1a W2a W3a W4a W5a W6a W7a W8a W9a W10a W11a W12a W13a W14a W15a W16 defer W17a W18a W19a W20a W21a W22a W23a W24a W25a W26defer
D1a D2a D3a D4a D5a D6a D7a D8a D9a D10b
```

Dopo la tua riga → prompt **E2-APPLY-§2** (nessuna applicazione finché non rispondi).

---

## Commit

```text
git add docs/I18N_HITL_DECISIONS.md .github/KB.md
git commit -m "docs(i18n): HITL decisions per §2 + D1-D10"
```
