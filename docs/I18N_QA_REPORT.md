# QA i18n episodi 1–23 (Prompt E)

**Data:** 2026-09-30  
**Canon:** `data/episodes.json` (schema v1, 23 record)  
**Metodo:** lettura integrale + scan automatico anti-spoiler / parity IT–EN / tier theater vs scripture  
**Vincolo:** nessun fix applicato; citazioni/parafrasi dottrinali non modificate senza OK umano  

Priorità di copertura:

| Banda | Id | Copertura |
|-------|----|-----------|
| Full | 1–12 | QA completa |
| Spot | 13–18 | spot-check |
| Full | 19–23 | QA completa (nuovi K2) |

Severità: `blocker` | `major` | `minor` | `typo`

---

## Tabella issue

| id | campo | issue (IT/EN) | severità | fix proposto |
|----|-------|---------------|----------|--------------|
| 1 | `imageCaptionsIt[6]` | Anti-spoiler esito: «La morte entrò nel mondo» | major | Es. «Una conseguenza grave» / «Un mondo cambiato» |
| 1 | `imageCaptionsIt[7]` | Anti-spoiler personaggio: «Il serpente ingannatore» | major | Es. «Un inganno sottile» (niente animale rivelatore) |
| 1 | `scriptureQuoteIt` | Glossario TNM: «positivamente morirai» (calco letterale) | minor | **Decisione aperta** — lasciare citazione TNM o soft-parafrasi UI (richiede OK) |
| 2 | `imageCaptionsIt[4]` | Anti-spoiler: «Il tempio di Dagon» | major | «Un tempio nemico» / «Un edificio di culto ostile» |
| 2 | `imageCaptionsIt[6]` | Anti-spoiler esito: «La fine della forza» | major | «Una forza che viene meno» |
| 2 | `theaterQuote.it` | Theater cita già «Sansone» (ok post-reveal; coerente) | — | Nessun fix; nota: theater può nominare dopo soluzione |
| 3 | `imageCaptionsIt[3]` | Anti-spoiler luogo: «La grande città di Ninive» | major | «Una grande città» |
| 3 | `intro.it` | Intro anticipa tema/morale («misericordia») | minor | Soften: enfasi su fuga/missione; lasciare misericordia alla morale |
| 4 | `imageCaptionsIt[0–7]` | Caption molto rivelatorie (pecore/capre, Figlio dell'uomo, Giudice) | major | Neutralizzare slot visibili; spostare indizi forti solo su hidden/hint |
| 5 | `imageCaptionsIt[3]` | Anti-spoiler esito: «La morte dei primogeniti» | major | «Una notte terribile in Egitto» (senza esito esplicito) |
| 5 | `imageCaptionsIt[4]` | Residuo goffo: «GRATUITO: la liberazione di Dio» | major | Rimuovere prefisso «GRATUITO:»; es. «Una liberazione immersiva» → meglio «Una liberazione donata» |
| 7 | `theater vs scripture` | Theater IT più lungo (74) della scripture (44) — tier invertiti | minor | Accorciare theater a citazione breve; tenere Ester 4:16 in scripture |
| 7 | `imageCaptionsIt[0]` | Anti-spoiler: «La regina Ester» | major | «Una regina al bivio» |
| 7 | `imageCaptionsIt[6]` | Anti-spoiler: «Lo shock di Aman smascherato» | major | «Uno shock a corte» (niente nome) |
| 8 | `imageCaptionsIt[0]` | Anti-spoiler: «Il patriarca Abramo» | major | «Un patriarca in cammino» |
| 8 | `imageCaptionsIt[6]` | Anti-spoiler: «Isacco, il figlio promesso» | major | «Il figlio promesso» / «Un figlio amato» |
| 8 | `immersive.titleEn` | EN più corto del titolo IT («Abraham and Isaac» vs «…al Monte Moria») | minor | Allineare: «Abraham and Isaac on Mount Moriah» |
| 10 | `imageCaptionsIt[3]` | Caption goffa: «OK - tutto va bene nel nuovo mondo» | major | «Pace nel mondo nuovo» / «Un mondo senza violenza» |
| 11 | `imageCaptionsIt[0]` | Anti-spoiler: «L'arca di salvezza» | major | «Una grande imbarcazione» |
| 12 | `imageCaptionsIt[0]` | Anti-spoiler: «Un angelo guida Filippo» | major | «Una guida improvvisa» / «Un inviato che guida» |
| 13 | `scriptureQuoteIt` | Apostrofi ASCII rule-based: `sapra'`, `perche'` | typo | Normalizzare a `saprà` / `perché` (solo ortografia; **OK umano** se si tocca citazione) |
| 13–18 | `imageCaptionsIt` / note | Pattern `piu'`, `fermara'`, `sapra'` in vari campi spot | typo | Passata ortografica ASCII→Unicode su caption/note (non su citazioni se bloccate) |
| 18 | `questions[0].prompt` | Quiz nomina «samaritano» (atteso in MCQ; non caption) | — | Nessun fix caption; OK in quiz |
| 19 | `imageCaptionsIt[5]` | «Una torre che punta in alto» — vicino al titolo «Torre di Babele» | minor | «Un edificio che punta in alto» (coerente con cap[0]) |
| 19 | `questions[0].prompt` | «…a Babele?» spoiler nome in MCQ | minor | Accettabile in quiz; alternativa: «in quella città?» |
| 19 | `moral.it` vs `keywordIt` | Tema «Orgoglio» non nominato; usa «pieno di sé / umiltà» | minor | Opzionale: «L'orgoglio…» in apertura morale |
| 20 | `imageCaptionsIt[5]` | «Un leone potente» — vicino al titolo | minor | «Una belva potente» (slot hidden ok-ish; preferibile soft) |
| 21 | `moral.it` vs tema | Tema «Conversione»; morale parla di perdono/missione | minor | Opzionale ancoraggio: «La conversione apre…» |
| 21 | `imageCaptionsIt` | Buone (nessun Saul/Paolo/Damasco) | — | OK |
| 22 | `imageCaptionsIt` | Nessun Giosuè/Gerico nei caption — OK | — | OK |
| 23 | `imageCaptionsIt` | Nessun Marta/Maria nei caption — OK | — | OK |
| 23 | `themeEn` | «Priorities» vs IT «Priorità» — parity OK | — | OK |

Nessun **blocker** strutturale (campi IT/EN vuoti, IT==EN identici, answers incomplete) trovato su 1–23.

---

## Pattern ricorrenti

1. **Caption pre-G1 troppo “soluzione”** (1–12): nomi propri, luoghi, esiti narrativi negli slot visibili/hint.
2. **Residui editoriali** in caption: prefissi tipo `GRATUITO:`, `OK -` (Ep 5, 10).
3. **Apostrofi ASCII** (`piu'`, `perche'`, `sapra'`) tipici del motore rule-based / paste legacy — più evidenti in 13–18 scripture/caption.
4. **Ep 19–23** (post-K2): caption complessivamente più anti-spoiler; theater “Riferimento …” vs scripture citazione — tier distinti OK; piccole ancore tema/morale opzionali.
5. **Theater vs scripture:** nella maggioranza theater breve + scripture lunga (corretto); anomalia Ep **7** (theater > scripture).

---

## Decisioni aperte (umano)

| # | Domanda | Impatto |
|---|---------|---------|
| D1 | Ammorbidire citazioni TNM con calchi letterali (`positivamente morirai`) nelle UI, o lasciare quote fedeli? | Glossario / Ep1 scripture |
| D2 | Normalizzare apostrofi ASCII anche dentro `scriptureQuoteIt`, o solo caption/note? | Tipografia vs fedeltà citazione |
| D3 | Anti-spoiler caption: policy “zero nomi propri” anche negli slot hidden/hint, o solo visible[0–4]? | Scope rewrite 1–12 |
| D4 | Quiz prompt possono nominare personaggi (Ep19 Babele, Ep20 Daniele) o devono restare ciechi fino alla morale? | UX quiz vs anti-spoiler |
| D5 | Allineare `titleEn` Ep8 al titolo IT completo (Monte Moriah)? | Cosmetico i18n |

---

## Esito

- Report consegnato; **nessun fix applicato**.
- Prossimi passi: review umana della tabella → eventuale Prompt E2 (patch caption only, citazioni bloccate finché D1/D2).

Commit proposto (NON eseguire):

```text
git add docs/I18N_QA_REPORT.md .github/KB.md
git commit -m "docs(i18n): QA report 1-23"
```
