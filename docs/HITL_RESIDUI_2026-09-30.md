# HITL residui — 2026-09-30 (post APPLY-HITL)

**Stato:** decision-ready · **nessun fix in questa sessione**  
**HEAD base:** `ddea5ee`  
**Evidenza render:** `.local/smoke/verify_ep13_render.png`, `audit_ep12.png`, `audit_ep18.png`  
(+ log desktop `verify_ep_render_log.txt`: 8× PictureBox `HAS_IMG`, Ep13 `keysV4=piccolo_gregge`, left start≈157 centrato)

---

## SEZIONE 1 — EP 12 (Filippo e l’Eunuco)

| # | info | valore |
|---|------|--------|
| titolo | | Filippo e l’Eunuco Etiope |
| index `2753` | | **V[1]** (non V[4]) |
| key attuale | | `2753` |
| caption attuale | | `Una domanda: cosa significa?` |
| visibleKeys | | `1F47C`, **`2753`**, `1F30A`, `1F4D6`, `Hackney-100` |
| screenshot | | `.local/smoke/audit_ep12.png` |
| semantica | | Caption = domanda esplicita → «?» **coerente** col tema (eunuco che non capisce Isaia). Non è caption tipo «Qualcosa manca…». File PNG presente (non missing). |
| key alt proposte (già in assets) | | `1F4AC` (speech/bubble, presente); `203C` (già in hidden+hint — **evitare** dup); nessun altro “domanda” dedicato oltre `2753` (`photo_concepts.question`) |
| conclusione audit | | **OK intenzionale** (placeholder didattico) · opz. fix solo se UX «?» dà fastidio |
| opzioni | | **A)** fix `2753`→`1F4AC` (+ caption soft) · **B)** accetta · **C)** ridefinisci slot/tema |
| proposta | | **B** — caption e tema giustificano il «?» |
| decisione utente | | [ ] A  [ ] B  [ ] C |

---

## SEZIONE 2 — EP 18 (Buon Samaritano)

| # | info | valore |
|---|------|--------|
| titolo | | Il Buon Samaritano |
| index `2753` | | **V[4]** |
| key attuale | | `2753` |
| caption attuale | | `Chi si fermerà ad aiutare?` |
| visibleKeys | | `1F6B6-…`, `1F4AA-1F3FD`, `Hackney-100`, `1F4B0`, **`2753`** |
| screenshot | | `.local/smoke/audit_ep18.png` |
| semantica | | Caption retorica anti-spoiler (chi aiuta?) → «?» **plausibile**. PNG presente. |
| key alt proposte (già in assets) | | `270B-1F3FD` (mano/gesto, presente); `1F440` (già hint); `1F498` (già hidden); `1F6B6-200D-2640-FE0F` (walk woman, presente) |
| conclusione audit | | **ambiguo / OK intenzionale** — tollerabile; A se si vuole evitare pattern Ep13 UX |
| opzioni | | **A)** fix `2753`→`270B-1F3FD` (o altra) · **B)** accetta · **C)** ridefinisci |
| proposta | | **B** (o **A**=`270B-1F3FD` se uniformare “no 2753 in visible”) |
| decisione utente | | [ ] A  [ ] B  [ ] C |

---

## SEZIONE 3 — W16 / D7 (Ep7 theater)

| info | valore |
|------|--------|
| Campo | **stesso:** `immersive.theaterQuote.it` (Ep 7) |
| W16 (HITL Sez.2) | before lungo Ester 4:14 → after «accorciare (stringa in D7)» |
| D7 (HITL Sez.3) | **A** keep · **B** `«…per un tempo come questo?» — Ester 4:14` · **C** custom; scripture 4:16 resta |
| Decisione utente (APPLY) | W1–W26 **tutte ok**; D7 **defer** |
| Cosa ha fatto il apply (`729a5f7`) | Ha applicato **testo = D7-B** (non ha inventato una terza stringa) |
| Diff applicato | before: `«Chi sa se non sei giunta… proprio per un tempo come questo?» — Ester 4:14` (len≈74) → after: `«…per un tempo come questo?» — Ester 4:14` (len **41**) |
| Scripture | invariata: `E se devo perire, perirò! - Ester 4:16 (TNM)` (len **44**) → tier theater≤scripture **OK** |
| Relazione | W16 e D7 = **stessa voce**; applicare W16 con after “accorcia” **richiedeva** una stringa: usata D7-B. Anomalia di processo (D7 defer vs W16 ok), non di campo. |
| proposta | **keep** (allinea D7-B + fix tier) · altrimenti **revert** al before lungo · o **defer** finché non rileggi D7 |
| decisione utente | [ ] keep  [ ] revert  [ ] defer |

---

## SEZIONE 4 — Summary one-line

Incolla in chat:

```text
EP12: B
EP18: B
W16: keep
# alternative: EP12:A key=1F4AC | EP18:A key=270B-1F3FD | W16:revert
```

| Codice | Scelta | Se A/modifica, testo o key |
|--------|--------|----------------------------|
| EP12 | **B** (applicata) | — |
| EP18 | **B** (applicata) | — |
| W16/D7 | **keep** (applicata) | D7-B in tree |

---

## Decisioni applicate 2026-09-30

- **EP12: B** — `2753` intenzionale (domanda eunuco: «Una domanda: cosa significa?»)
- **EP18: B** — `2753` intenzionale (retorica: «Chi si fermerà ad aiutare?»)
- **W16: keep** — D7-B applicato correttamente (`«…per un tempo come questo?» — Ester 4:14`)

### Fix desktop asset (stesso giorno)

- Causa: `piccolo_gregge` (e `1F411`) **assenti** da `Properties/Resources.resx` + `Resources.Designer.cs` → `StoryResources.GetImage` → fallback `2753`.
- Fix: registrazione ResXFileRef + property Designer; csproj invariato (embed via resx).
- Evidenza: `ResourceManager.GetObject(piccolo_gregge)=True`; pixel_dist V4 vs 2753 = **277.53**

## Fase A — Verify Ep13 (evidenza)

| check | risultato |
|-------|-----------|
| MSBuild exit | **0** |
| exe TS | `2026-09-30T10:14:26+02:00` |
| screenshot | `.local/smoke/verify_ep13_render.png` |
| V[4] immagine | **OK** — `piccolo_gregge` (gregge, non «?») |
| V[4] caption | **OK** — `Un gregge sul colle` |
| V[1] vs V[4] | **OK** — `1F411` pecora singola ≠ gregge |
| layout centrato | **OK** — smoke desktop `left=157,347,537,727` (Bug3) + griglia compose |
| regressione «?» in V[4] | **No** |

Desktop smoke: `VerifyEpRenderSmoke` exit 0; tutti i PB `HAS_IMG`; `keysV4=piccolo_gregge`.
