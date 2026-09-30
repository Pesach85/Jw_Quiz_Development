# Menu layout design — Bug 2 (23 episodi)

**Data:** 2026-09-30  
**Stato:** design only — **nessun codice runtime modificato**  
**Contesto post:** FIX-DESKTOP-1-3 (`1390bd3`) — Bug 1/3 chiusi; Bug 2 aperto  
**Evidenza attuale:**

| Range | Dove | Forma |
|-------|------|--------|
| 1–12 | `Form1.Designer.cs` `storieToolStripMenuItem.DropDownItems` (~L81–93) | 12 `ToolStripMenuItem` flat; naming legacy (`storia2ToolStripMenuItem1` = Storia 3; `storia10ToolStripMenuItem` = Storia **12**) |
| 13–23 | `Form1.cs` `BuildDynamicMenus` via `StoryEngine.GetDynamicStories()` | sottomenu «Nuovi Episodi» (dinamico post-Bug1) |
| Utente / Crea | `Form1.cs` runtime | sotto «Storie» dopo separator |

`StoryEngine.GetDynamicStories()` = `StoryLibrary.Stories.Where(s => s.IsDynamic)` → id **13–23**.  
`TotalStories` = `StoryLibrary.Stories.Count` (= 23).

Problema UX: tre+ dropdown affollati (12 flat + 11 «Nuovi» + Crea/Utente); nessun raggruppamento per fascia; manutenzione Designer fragile.

Opzioni valutate (da `docs/DESKTOP_BUGS_2026-09-30.md`): **A**, **C**, **D**. (Opzione B temi: fuori scope di questo design.)

---

## Opzione A — Fasce 1–12 / 13–18 / 19–23 in sottomenu

### Descrizione visiva (layout ipotetico)

```text
Menu → Storie
         ├─ Episodi 1–12      ▶  Storia 1 … Storia 12
         ├─ Episodi 13–18     ▶  Storia 13 … Storia 18
         ├─ Episodi 19–23     ▶  Storia 19 … Storia 23
         ├─ ──────────────
         ├─ Crea storia
         └─ Storie utente     ▶  …
```

«Nuovi Episodi» come etichetta unica sparisce; le tre fasce sostituiscono sia i 12 item Designer sia il sottomenu dinamico attuale.

### Pro

- Chiarezza: l’utente vede subito le bande catalogo (classici / mid / K2).
- Scalabilità moderata: aggiungere 24–28 = nuova fascia o estendere 19–23 senza allungare un unico flat.
- Compatibile WinForms (`ToolStripMenuItem` nested) — pattern già usato per «Nuovi Episodi».
- Tempo utente: max **2 click** (fascia → id), vs 1 click oggi su 1–12 ma lista caotica.

### Contro

- Un click in più rispetto a Storia 1–12 flat.
- Perdita scorciatoia mentale «Nuovi Episodi» (se gli utenti già la conoscono post-onboarding).
- Fasce fisse 12/6/5: se il catalogo cresce asimmetrico vanno ricalibrate a mano (a meno di helper).

### Effort stimato

| Voce | Stima |
|------|--------|
| File | `Form1.cs` (build fasce), `Form1.Designer.cs` (rimuovere/nascondere 12 item + handler stub), eventualmente `AppText` (3 label fascia) |
| Righe | ~80–150 (Designer cleanup più pesante dei loop) |
| Tempo | **S** — ½–1 giorno |

### Rischio regressione

- **Medio-basso** se si lasciano i Click handler Designer orfani vs **medio** se si cancellano item Designer senza ripuntare `OpenStory(id)`.
- Runtime: basso se le fasce usano lo stesso `OpenStory` di Bug1.
- Designer: rischio di `.resx` / InitializeComponent desync se si editano item a mano.

### Rollback

Ripristinare DropDownItems Designer 1–12 + sottomenu «Nuovi Episodi» da `GetDynamicStories()` (stato `1390bd3`).

---

## Opzione C — Dialog selector al posto del dropdown lungo

### Descrizione visiva (layout ipotetico)

```text
Menu → Storie
         ├─ Scegli episodio…     →  [Dialog]
         │                           ListBox / ListView:
         │                             1  ·  (tema o solo id)
         │                             …
         │                             23
         │                           [Apri] [Annulla]
         ├─ Crea storia
         └─ Storie utente
```

Niente lista 1–23 nel menu; un solo entry point. Opzionale: filtro testo, colonna tema (`keywordIt`), anti-spoiler (solo id + tema).

### Pro

- Scalabile a N episodi (30+ senza colonna WinForms infinita).
- Anti-spoiler opzionale: niente titoli rivelatori nel menu.
- UX da “catalogo”: ricerca/scroll nativo ListBox migliore di ToolStrip multi-column.
- Unifica 1–12 e 13–23 in un’unica superficie.

### Contro

- Cambio UX più netto (non è più “menu classico Windows”).
- Accessibilità: focus dialog, Esc, Enter da testare; screen reader diverso dal menu.
- Tempo utente: click menu + selezione + Apri (spesso **3 azioni**).
- Nuovo form/dialog da localizzare (`AppText`).

### Effort stimato

| Voce | Stima |
|------|--------|
| File | nuovo `EpisodePickerForm.cs` (+ Designer), `Form1.cs`, `AppText`, test smoke |
| Righe | ~200–350 |
| Tempo | **M** — 1–2 giorni |

### Rischio regressione

- **Medio**: nuova superficie UI; regressione apertura storie se binding id sbagliato.
- Designer Form1: basso se si sostituisce solo il contenuto di `storieToolStripMenuItem`.
- Nessun rischio layout rebus (Bug3 già chiuso) se il dialog è isolato.

### Rollback

Rimuovere dialog; ripristinare menu A o stato `1390bd3`.

---

## Opzione D — Menu 100% dinamico da `StoryEngine`

### Descrizione visiva (layout ipotetico)

```text
Menu → Storie
         ├─ Storia 1
         ├─ Storia 2
         ├─ …
         ├─ Storia 23          ← tutti da GetAllStories() / TotalStories
         ├─ ──────────────
         ├─ Crea storia
         └─ Storie utente
```

O, variante **D+fasce** (A implementata in chiave D): stesse tre fasce, ma **zero** item hardcoded in Designer — solo container vuoti o un unico `storieToolStripMenuItem` popolato a runtime.

### Pro

- Una sola fonte di verità: `StoryLibrary` / `StoryEngine` (allineato a web `episodes.json` via generator).
- Elimina naming legacy Designer e drift 1–12 vs catalogo.
- Scalabilità: nuovo episodio in library = voce menu senza tocco Designer.
- Coerente con Bug1 (`GetDynamicStories` già pattern runtime).

### Contro

- Refactor: rimuovere 12 Click handler Designer + campi `storia*ToolStripMenuItem`.
- Flat 1–23 senza fasce = lista ancora lunga (peggio di A sulla chiarezza).
- Perdita eventuale di scorciatoie future se qualcuno lega hotkey agli item Designer.

### Effort stimato

| Voce | Stima |
|------|--------|
| File | `Form1.cs`, `Form1.Designer.cs` (pulizia ampia), smoke |
| Righe | ~120–250 (Designer delete + un loop `GetAllStories`) |
| Tempo | **M** — 1 giorno (meno di C; più di A pura se si ripulisce Designer) |

### Rischio regressione

- **Medio**: InitializeComponent e wire Click; errore tipico = doppie voci se non si `Clear` i DropDownItems Designer.
- Runtime: basso se si riusa `OpenStory`.
- Designer: **alto** se edit parziale lascia handler morti / NullReference su campi rimossi.

### Rollback

Git revert del commit menu; Designer 1–12 + Nuovi dinamici.

---

## Confronto sintetico

| Criterio | A fasce | C dialog | D 100% dinamico |
|----------|---------|----------|-----------------|
| Click tipici ad aprire | 2 | 2–3 | 1 (lista lunga) |
| Chiarezza bande | Alta | Alta (se colonne) | Bassa se flat |
| Scalabilità N→30+ | Media | Alta | Media (flat) / Alta (D+fasce) |
| Effort | S | M | M |
| Rischio Designer | Medio | Basso–medio | Alto se cleanup |
| Allineamento StoryEngine | Parziale | Totale | Totale |

---

## Raccomandazione

**A implementata come D leggero (fasce dinamiche; Designer 1–12 deprecati).**

Motivazione (evidenza + trade-off):

1. Post-Bug1 il pattern runtime (`GetDynamicStories` / `OpenStory`) è già collaudato — estenderlo a tre fasce evita un terzo stile (dialog) e chiude il debito Designer.
2. Le bande **1–12 / 13–18 / 19–23** rispecchiano come il team già parla del catalogo (classici, mid, K2) e tengono i dropdown ≤12 voci.
3. C (dialog) resta piano B se il catalogo supera ~30 o serve ricerca/anti-spoiler titoli; oggi effort M non giustificato per solo Bug 2.
4. D flat-only senza fasce **non** risolve l’affollamento — solo la manutenzione.

### Piano implementativo suggerito (NON in questo deliverable)

1. In `BuildDynamicMenus`: `Clear` voci storia numeriche; creare 3 parent `ToolStripMenuItem`; popolare da `GetAllStories()` filtrando id ranges (o helper `GetStoriesInRange`).
2. Rimuovere dal Designer i 12 item + handler (o, passo intermedio: `Visible = false` / non aggiungerli al DropDown — poi delete).
3. Localizzare label fasce in `AppText`.
4. Smoke: aprire Ep 1, 12, 13, 18, 19, 23; Crea + Utente invariati.
5. Prompt dedicato: **FIX-BUG-2** dopo OK umano su questa raccomandazione.

### Rollback

Revert commit FIX-BUG-2 → stato menu `1390bd3`.

---

## Commit proposto (NON eseguire finché OK umano)

```text
git add docs/MENU_LAYOUT_DESIGN.md .github/KB.md
git commit -m "docs(design): menu layout 23 episodi (bug 2)"
```

**STOP** — attendere approvazione opzione (A+D leggero vs C vs deferire) prima di codice.
