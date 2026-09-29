# Classic flat rebus — deprecation decision (design only)

**Stato:** design in review — **nessuna implementazione**.  
**Data:** 2026-09-29  
**Superficie:** `webapp/classic.html` + `webapp/app.js`  
**Vincolo:** `app.js` off-limits finché OK esplicito + scope; questo doc non autorizza tocchi.

---

## 1. Contesto

| Fatto | Dettaglio |
|-------|-----------|
| Player primario | `webapp/index.html` (Quiz / Rebus 3D / Avventura) |
| Docs prodotto | `docs/ARCHITECTURE.md` / `AGENTS.md`: *classic = editor + admin* |
| Realtà UI | `classic.html` apre ancora sul **rebus flat** (slot + reveal/hint/soluzione); editor e admin sono pannelli `hidden` |
| Coerenza recente | **F3C-A** (2026-09-29): toggle simmetrico + stars policy **A** (peak) allineati a desktop/immersive |
| Link da immersivo | CTA «Editor / Admin» e «Apri editor classico» → `classic.html` |
| Titolo pagina | `JW Quiz — Editor / Admin` (già orientato ops, non player) |

**Domanda:** deprecare il rebus flat in classic, lasciando solo editor + admin?

---

## 2. Inventario `classic.html` (~184 righe markup)

| Blocco | DOM | Ruolo | Visibile di default |
|--------|-----|-------|---------------------|
| Topbar | brand, lingua, `storySelect`, Crea episodio, online, Login, XP | Navigazione condivisa play+ops | Sì |
| Link immersivo | `href=index.html` | Escape hatch verso player | Sì |
| Header play | `#storyTitle`, `#storyRef`, XP previsti, `#starsBox` | Chrome rebus flat | Sì |
| Caption + slots | `#caption`, `#slots` | Board 8 PNG | Sì |
| Actions play | `#btnReveal`, `#btnHint`, `#btnSolution`, `#btnNext` | Gameplay flat | Sì |
| Soluzione | `#solutionPanel` | Testo post-reveal | No (`hidden`) |
| Editor | `#editorPanel` + campi + slot editor | Creazione episodi KV/local | No |
| Picker asset | `#pickerPanel` | Galleria/upload PNG | No |
| Admin | `#adminPanel` | Stats `/api/analytics` + `ADMIN_SECRET` | No |
| Scripts | `stories.js`, `assets.js`, `story-i18n.js`, `app.js` | Runtime unico | — |

**Nota:** non esiste oggi una “home editor-only”; il flat *è* la vista di riposo.

---

## 3. Inventario `app.js` (~1138 LOC) — stime per decisione

Scomposizione **indicativa** (funzioni / listener, non split file):

| Area | LOC stimate | Esempi |
|------|-------------|--------|
| Shared asset/story/i18n/caption | ~350–400 | `sanitizeStory`, `getDisplayCaption`, `imageUrl`, `refreshSharedData` |
| **Gameplay flat** | ~220–280 | `renderSlots`, `renderButtons`, `calcStars`, listener reveal/hint/solution/next, XP/stars UI |
| Editor + picker | ~280–320 | `openEditor`, `saveEditorStory`, `renderAssetGrid`, `uploadCustomAsset` |
| Admin + analytics heartbeat | ~120–150 | `openAdmin`, `sendAnalyticsEvent`, `startHeartbeat` |
| Init / wiring | ~80–100 | `initialize`, selector change |

**Accoppiamento critico:** dopo «Salva episodio» l’editor chiama `loadStory(...)` e ripopola la board flat — il flat funge da **preview QA** post-edit. Una rimozione grezza del play rompe quel loop se non si sostituisce con preview editor dedicata.

---

## 4. Opzioni

### A — Mantieni (status quo post–F3C-A)

| | |
|--|--|
| **Guadagna** | Zero effort; policy stelle/toggle già allineate; preview post-save; superficie di smoke/debug senza WebGL; rollback = N/A |
| **Perde** | Divergenza docs («solo editor») vs UI reale; rischio confusione utente che apre classic pensando al gioco; debito triplice (desktop / immersive / flat) se in futuro le policy divergono di nuovo |
| **Impatto `app.js`** | **0** righe |
| **Editor / admin** | Nessuna regressione |
| **Utente** | Chi usa flat oggi: (1) ops/editor che verifica PNG dopo save, (2) bookmark legacy `/classic.html` come “gioco semplice”, (3) smoke agent/QA su file:// senza Three.js. Volume analytics sconosciuto (heartbeat su classic, non distinto “play vs edit”) |
| **Effort / rischio / rollback** | Effort **0** · Rischio **basso** · Rollback N/A |

### B — Deprecare: classic = solo editor + admin

| | |
|--|--|
| **Guadagna** | Allinea UI a ARCHITECTURE/AGENTS; riduce superficie bug gameplay; messaggio prodotto chiaro (play = immersivo) |
| **Perde** | Preview flat post-save; smoke “no-WebGL”; bookmark legacy; possibile attrito per chi preferisce board 2D |
| **Impatto `app.js`** | Rimuovere/guardare **~220–280** LOC gameplay + semplificare `render`/`initialize`; **+40–80** LOC se serve preview slot in-editor; tocco **alto**, richiede OK esplicito |
| **Editor / admin** | Rischio **medio**: regressione se `loadStory`/`renderSlots` restano accoppiati al play; admin/heartbeat restano |
| **Utente** | Redirect soft da “play” a `index.html`; CTA immersivo già presente |
| **Effort / rischio / rollback** | Effort **M** (0.5–1.5 g) · Rischio **medio-alto** · Rollback: revert commit + feature flag |

### C — Mantieni solo per debug editor (nascosto in UI)

| | |
|--|--|
| **Guadagna** | Messaggio prodotto “editor/admin”; flat resta per QA (`?play=1` / `localStorage` / tasto “Anteprima rebus”); meno confusione casual user |
| **Perde** | Un pezzo di UX “gioco” pubblico; serve discovery del flag |
| **Impatto `app.js`** | **~25–60** LOC (hide default + toggle); markup class/`hidden` su sezioni play |
| **Editor / admin** | Basso rischio se default = editor aperto o landing “scegli: crea / admin / anteprima” |
| **Utente** | Visitatori vedono ops; power user sblocca flat |
| **Effort / rischio / rollback** | Effort **S** (2–6 h) · Rischio **basso-medio** · Rollback: togliere flag / unhide |

### D — Altro emerso

| Variante | Idee |
|----------|------|
| **D1 Split file** | `classic-play.js` vs `classic-ops.js` prima di deprecare — abilita B con rischio minore; effort **M+**, non depreca ancora |
| **D2 Redirect** | `classic.html` → solo se `?editor=1`, altrimenti 302/`location` a `index.html` — simile a C ma più aggressivo |
| **D3 Desktop-only flat** | Web abbandona flat; WinForms resta unico 2D — coerente con proiezione, perde preview web |

Nessuna richiede schema dati / `episodes.json`.

---

## 5. Raccomandazione

**Raccomandazione: A — Mantieni as-is (post–F3C-A), con trigger espliciti di rivalutazione.**

### Motivazione

1. **Costo già pagato:** F3C-A ha chiuso la divergenza stelle/toggle; deprecare ora spreca quel lavoro senza beneficio utente misurato.
2. **Preview editor:** il flat non è solo “legacy play” — è il percorso naturale di verifica dopo `saveEditorStory` → `loadStory`. B senza preview sostitutiva è un downgrade ops.
3. **Docs aspirazionali ≠ prodotto:** ARCHITECTURE dice “editor only”, ma il markup e i link (anche «Apri editor classico» dal theater) ancora trattano classic come hub. Meglio **allineare i docs** o fare **C** in un secondo momento, non tagliare codice a caldo.
4. **Rischio B alto su file off-limits:** `app.js` è monolitico; rimuovere 200+ LOC senza split aumenta probabilità di regressione editor/API.
5. **Player primario già chiaro:** `index.html` è l’esperienza pubblica; flat non compete sul marketing se i link immersivi restano in evidenza.

### Se in futuro si vuole muovere

| Priorità | Percorso |
|----------|----------|
| 1º | **C** (nascondi + anteprima esplicita) — basso rischio, allinea messaggio |
| 2º | **D1** split moduli in `app.js` (solo con OK) |
| 3º | **B** rimozione gameplay quando preview editor è indipendente |

### Trigger di rivalutazione (uscire da A)

Rivalutare entro **30 giorni** o alla prima di queste condizioni:

1. Analytics / feedback: traffico play su `classic.html` ≈ 0 **e** gli editor non usano la board post-save.  
2. Nuova divergenza policy (stelle/toggle) richiederebbe di nuovo OK su `app.js` flat.  
3. Bug flat ricorrenti che non esistono su immersive/desktop.  
4. Decisione prodotto di fare landing classic = editor-first (allora preferire **C**, non B subito).  
5. Split `app.js` già fatto → costo marginale di B crolla.

---

## 6. Piano step-by-step (solo se si sceglie B o C)

*Non eseguire finché OK umano su questa decisione.*

### Piano C (consigliato se si abbandona A)

1. Design UI: default nasconde `#slots`, `.actions`, chrome stelle play; mostra CTA «Crea episodio» / «Admin» / «Anteprima rebus».  
2. Flag: `?preview=1` **o** bottone che setta `localStorage.jwquiz_classic_preview=1`.  
3. Patch minima `classic.html` (class/`hidden`) + `app.js` (~25–60 LOC) — **OK esplicito scope**.  
4. Aggiornare i18n CTA da immersivo se il copy «Apri editor classico» confonde.  
5. Smoke: editor save → anteprima; admin login; immersivo link.  
6. KB + AGENTS: “classic default = ops; flat = preview opt-in”.  
7. **Rollback:** revert commit; flag default off.

### Piano B (dopo C o D1)

1. Prerequisito: preview slot **dentro** `#editorPanel` (non dipende da play chrome).  
2. Rimuovere sezioni play da `classic.html`.  
3. Rimuovere listener/render gameplay da `app.js` (~220–280 LOC); tenere shared.  
4. `initialize()`: non `loadStory(0)` in vista play; aprire editor vuoto o lista episodi read-only.  
5. Redirect bookmark: banner “Il gioco è su /” → `index.html`.  
6. Parity smoke editor/admin; nessun tocco `functions/api/*` salvo necessità.  
7. **Rollback:** revert; o feature flag `ENABLE_CLASSIC_FLAT` se si preferisce soft-delete.

---

## 7. Decisioni aperte per l’umano

- [ ] Approvare **A** (keep) — chiudere ticket design.  
- [ ] Preferire **C** — autorizzare scope `app.js` &lt; 60 LOC + markup.  
- [ ] Preferire **B** — solo dopo preview editor dedicata / OK rischio.  
- [ ] Aggiornare subito ARCHITECTURE/AGENTS al comportamento reale (flat ancora presente) **senza** deprecare codice.  
- [ ] Misurare usage (evento analytics `classic_play` vs `classic_edit`) prima della prossima rivalutazione.

---

## 8. Commit proposto (docs only)

```text
git add docs/CLASSIC_REBUS_DEPRECATION.md .github/KB.md
git commit -m "docs(design): classic flat deprecation decision"
```

**Non eseguire** finché conferma umana.  
Nessun tocco a `app.js` / `classic.html` in questo task.
