# Proposta fix i18n — da QA report (Prompt E2 analisi)

**Data:** 2026-09-30  
**Fonte:** `docs/I18N_QA_REPORT.md`  
**Canon verificato:** `data/episodes.json` (23 record)  
**Verifica stale:** `.local/verify_qa_stale.py` → **FAILS 0** (testi QA = record JSON).  
**Vincolo:** **nessun fix applicato** a `data/episodes.json` né ad altri artefact. Solo proposta.

Severità in scope: **≥ minor** (+ **typo** ortografici trattati come fix ovvi dove applicabile).  
Issue con severità `—` (nessun fix) omesse.

---

## 1. Fix ovvi (before → after)

Residui tecnici / ortografia ASCII→Unicode su **caption, hint, engagementNote** (non su `scriptureQuoteIt` / dialoghi in `solutionIt` — vedi §3 D2).

| id | campo | before (verificato JSON) | after proposto | note |
|----|-------|--------------------------|----------------|------|
| 5 | `rebus.imageCaptionsIt[4]` | `GRATUITO: la liberazione di Dio` | `Una liberazione donata` | Rimuove prefisso editoriale `GRATUITO:` (QA major; cleanup ovvio del prefisso; wording short-list OK se preferisci solo strip → `la liberazione di Dio`) |
| 10 | `rebus.imageCaptionsIt[3]` | `OK - tutto va bene nel nuovo mondo` | `Pace nel mondo nuovo` | Rimuove prefisso `OK -`; alternativa QA: `Un mondo senza violenza` |
| 13 | `rebus.imageCaptionsIt[7]` | `Una forza piu' grande delle apparenze` | `Una forza più grande delle apparenze` | ASCII `piu'` → `più` |
| 14 | `rebus.imageCaptionsIt[0]` | `Un uomo con grande autorita'` | `Un uomo con grande autorità` | `autorita'` → `autorità` |
| 14 | `rebus.hintIt` | `Anni di schiavitu' e prigione nascondevano un piano di Dio.` | `Anni di schiavitù e prigione nascondevano un piano di Dio.` | `schiavitu'` → `schiavitù` |
| 14 | `rebus.engagementNoteIt` | `Anche le ingiustizie piu' dure possono far parte del piano di Dio.` | `Anche le ingiustizie più dure possono far parte del piano di Dio.` | `piu'` → `più` |
| 15 | `rebus.engagementNoteIt` | `La lealta' a Geova va oltre le frontiere etniche e culturali.` | `La lealtà a Geova va oltre le frontiere etniche e culturali.` | `lealta'` → `lealtà` |
| 16 | `rebus.engagementNoteIt` | `Geova usa anche le circostanze piu' disperate per proteggere i Suoi servitori.` | `Geova usa anche le circostanze più disperate per proteggere i Suoi servitori.` | `piu'` → `più` |
| 18 | `rebus.imageCaptionsIt[4]` | `Chi si fermera' ad aiutare?` | `Chi si fermerà ad aiutare?` | `fermera'` → `fermerà` |

**Esclusi volutamente da questa sezione (non sono errori):** elisioni italiane corrette (`dall'alto`, `L'amore`, `Va'` / `fa'` imperativi).

---

## 2. Fix wording (before → after + motivazione)

Toccano caption anti-spoiler, tier theater/scripture, titoli o intro. **Richiedono OK esplicito** prima di E2-APPLY.

| id | campo | before (verificato JSON) | after proposto (da QA) | motivazione | classificazione |
|----|-------|--------------------------|------------------------|-------------|------------------|
| 1 | `rebus.imageCaptionsIt[6]` | `La morte entrò nel mondo` | `Una conseguenza grave` (alt. `Un mondo cambiato`) | Anti-spoiler esito pre-G1 | fix wording |
| 1 | `rebus.imageCaptionsIt[7]` | `Il serpente ingannatore` | `Un inganno sottile` | Anti-spoiler personaggio/animale | fix wording |
| 2 | `rebus.imageCaptionsIt[4]` | `Il tempio di Dagon` | `Un tempio nemico` (alt. `Un edificio di culto ostile`) | Anti-spoiler luogo/culto | fix wording |
| 2 | `rebus.imageCaptionsIt[6]` | `La fine della forza` | `Una forza che viene meno` | Anti-spoiler esito | fix wording |
| 3 | `rebus.imageCaptionsIt[3]` | `La grande città di Ninive` | `Una grande città` | Anti-spoiler luogo | fix wording |
| 3 | `immersive.intro.it` | `Fuggire da Dio non funziona. La misericordia, sì.` | Soften: enfasi fuga/missione; misericordia in morale (testo esatto da scegliere in APPLY) | Intro anticipa morale | fix wording |
| 4 | `rebus.imageCaptionsIt[0]` | `Le pecore alla destra del Re` | Neutralizzare (es. `Due gruppi davanti al Re`) | Slot rivelatorio Mt 25 | fix wording |
| 4 | `rebus.imageCaptionsIt[1]` | `Le capre alla sinistra` | Neutralizzare (es. `Un altro gruppo in attesa`) | Idem | fix wording |
| 4 | `rebus.imageCaptionsIt[2]` | `Il canto della vittoria` | Soft (es. `Un canto solenne`) | Tone reveal | fix wording |
| 4 | `rebus.imageCaptionsIt[3]` | `La bandiera d'Israele` | Soft (es. `Uno stendardo alzato`) | Ancora nazionale | fix wording |
| 4 | `rebus.imageCaptionsIt[4]` | `La corona del Regno` | Soft (es. `Un segno di autorità`) | Reveal Regno | fix wording |
| 4 | `rebus.imageCaptionsIt[5]` | `Il Figlio dell'uomo glorificato` | Soft (es. `Una figura glorificata`) | Titolo cristologico | fix wording |
| 4 | `rebus.imageCaptionsIt[6]` | `L'arpa del giudizio` | Soft (es. `Uno strumento solenne`) | Reveal giudizio | fix wording |
| 4 | `rebus.imageCaptionsIt[7]` | `Il Giudice giusto` | Soft (es. `Chi decide con giustizia`) | Reveal ruolo | fix wording |
| 5 | `rebus.imageCaptionsIt[3]` | `La morte dei primogeniti` | `Una notte terribile in Egitto` | Anti-spoiler esito | fix wording |
| 7 | `immersive.theaterQuote.it` | `«Chi sa se non sei giunta… proprio per un tempo come questo?» — Ester 4:14` (len 74) | Accorciare a citazione breve (es. rif. corto / ellissi più secca); tenere Ester 4:16 in scripture | Tier invertiti: theater > scripture | fix wording |
| 7 | `rebus.imageCaptionsIt[0]` | `La regina Ester` | `Una regina al bivio` | Anti-spoiler nome | fix wording |
| 7 | `rebus.imageCaptionsIt[6]` | `Lo shock di Aman smascherato` | `Uno shock a corte` | Anti-spoiler nome + esito | fix wording |
| 8 | `rebus.imageCaptionsIt[0]` | `Il patriarca Abramo` | `Un patriarca in cammino` | Anti-spoiler nome | fix wording |
| 8 | `rebus.imageCaptionsIt[6]` | `Isacco, il figlio promesso` | `Il figlio promesso` (alt. `Un figlio amato`) | Anti-spoiler nome | fix wording |
| 8 | `immersive.titleEn` | `Abraham and Isaac` | `Abraham and Isaac on Mount Moriah` | Allinea a `rebus.titleIt` = `Abramo e Isacco al Monte Moria` | fix wording |
| 11 | `rebus.imageCaptionsIt[0]` | `L'arca di salvezza` | `Una grande imbarcazione` | Anti-spoiler arca | fix wording |
| 12 | `rebus.imageCaptionsIt[0]` | `Un angelo guida Filippo` | `Una guida improvvisa` (alt. `Un inviato che guida`) | Anti-spoiler nome | fix wording |
| 19 | `rebus.imageCaptionsIt[5]` | `Una torre che punta in alto` | `Un edificio che punta in alto` | Coerente con cap[0] `Un edificio imponente`; meno eco «Torre di Babele» | fix wording |
| 20 | `rebus.imageCaptionsIt[5]` | `Un leone potente` | `Una belva potente` | Soft vicino al titolo Daniele | fix wording |
| 21 | `immersive.moral.it` | `Il perdono di Dio apre una nuova missione. Il passato non blocca chi risponde alla chiamata.` | Opzionale: ancorare tema `Conversione` in apertura (es. `La conversione apre…`) | Allineamento tema/morale | fix wording (opzionale) |

**Ep5 caption[4]** e **Ep10 caption[3]** compaiono anche in §1 (prefisso tecnico). Se in APPLY si sceglie solo strip del prefisso senza rewrite, restano ovvi; se si adotta il testo QA completo, trattarli come wording OK-unica-volta.

---

## 3. Ambigui (domanda all’umano)

| # | Domanda | Evidenza | Impatto se sì/no |
|---|---------|----------|------------------|
| D1 | Ammorbidire in UI il calco TNM `positivamente morirai` (Ep1 `rebus.scriptureQuoteIt`), o lasciare citazione fedele? | QA minor; testo attuale verificato in JSON | Soft = parafrasi UI; No = nessun tocco scripture |
| D2 | Normalizzare apostrofi ASCII **anche** in `scriptureQuoteIt` (e dialoghi in `solutionIt`) Ep13–18, o **solo** caption/hint/note (§1)? | Hit verificati: Ep13 `sapra'`/`perche'`/`consegna'`; Ep14–18 scripture/solution con `e'`, `sara'`, `Gesu'`, `Mose'`, ecc. | Sì = ortografia+fedeltà citazione; No = §1 only |
| D3 | Policy anti-spoiler: zero nomi propri anche negli slot **hidden/hint**, o solo visible pre-G1? | Pattern QA 1–12; Ep4 ha 8 caption tutte forti | Scope rewrite ampio vs mirato |
| D4 | I prompt quiz possono nominare luoghi/personaggi (Ep19 `…a Babele?`, ecc.) o restano ciechi fino alla morale? | QA: Ep19 q0 nominare Babele = minor accettabile | UX quiz vs spoiler |
| D5 | Confermi allineamento `titleEn` Ep8 a Moriah (§2)? | `titleEn` corto vs `titleIt` Monte Moria | Cosmetico i18n |
| D6 | Ep3 intro: testo after esatto? (QA chiede soften senza stringa unica) | `Fuggire da Dio non funziona. La misericordia, sì.` | Serve una frase approvata |
| D7 | Ep7 theater: quale stringa breve sostituisce Ester 4:14 lunga? (scripture 4:16 resta?) | theater len 74 > scripture len 44 | Tier + testo citazione |
| D8 | Ep4: approvi i soft di §2 slot-per-slot o preferisci un set alternativo? | 8 caption tutte major | Blocco APPLY finché non c’è lista chiusa |
| D9 | Ep19 morale: inserire esplicitamente «orgoglio» (`keywordIt=Orgoglio`) o lasciare «pieno di sé / umiltà»? | QA minor opzionale | Editoriale |
| D10 | `solutionIt` Ep13–18: passata solo ASCII accenti (`dichiaro'`, `sposo'`, …) senza retouch narrativo? | Non in §1 per non mescolare con citazioni | Se D2=no ma solution sì → lista dedicata in APPLY |

---

## Esito verifica stale

| Check | Risultato |
|-------|-----------|
| Needle QA vs `data/episodes.json` | **FAILS 0** |
| QA report stale? | **No** — si può procedere a review umana |

---

## STOP — approvazione umana obbligatoria

**Non applicare nessun fix.** Nessun tocco a `data/episodes.json`, generator, `StoryLibrary`, webapp copy.

Attendi OK esplicito per:

1. **E2-APPLY** — quali righe di §1 / §2 / risposte D1–D10 includere.
2. Nessun merge wording anti-spoiler senza conferma slot-per-slot (specie Ep4, Ep7 theater).

### Commit proposto (NON eseguire finché OK umano)

```text
git add docs/I18N_QA_FIX_PROPOSAL.md .github/KB.md
git commit -m "docs(i18n): proposta fix da QA report"
```
