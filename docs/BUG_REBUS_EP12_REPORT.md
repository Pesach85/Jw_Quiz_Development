# Bug report — Ep 12 reveal / hide / hint (desktop + web)

**Data:** 2026-09-29  
**Scope:** indagine sola lettura (nessuna patch applicata)  
**Commit Step 2:** `aa8791f` — `feat(tools): generator stories.js + StoryLibrary.cs (step 2 G1)`  
**Parent pre-Step2 (catalogo C#):** `46767e9` (Step 1) / contenuto StoryLibrary pre-generazione  
**HEAD al report:** `470acdb` (Step 3)

---

## 1. Sintesi manifestazione

| Superficie | Sintomo riportato |
|------------|-------------------|
| **Desktop WinForms Ep 12** | Dopo “Rivela 2 immagini”, il pulsante etichettato **“Nascondi 2 immagini”** non nasconde. Click “Indizio” **prima** del reveal → form percepito come **bloccato**. |
| **Webapp** | Sospetto stesso comportamento (non confermato da smoke browser in questa sessione). |

**Ordine click tipico (KO):**

1. Scenario A: Rivela → (testo diventa Nascondi) → click Nascondi → **nessun hide** (o seconda reveal / pulsante disabilitato).  
2. Scenario B: Indizio prima di reveal → indizio si apre → pulsante Indizio **disabilitato** con testo “Nascondi indizio” → nessun percorso reverse.

---

## 2. Fase 0 — Diff Ep 12 (campo → pre-Step2 → post-Step2 → episodes.json)

Fonte pre-Step2 C#: `git show 46767e9:StoryLibrary.cs` (Id=12).  
Post-Step2: `aa8791f` / HEAD attuale (= JSON).

| Campo | Pre-Step2 (`StoryLibrary.cs`) | Post-Step2 / `episodes.json` | Δ |
|-------|-------------------------------|------------------------------|---|
| Title | Filippo e l'Eunuco Etiope | stesso | — |
| Keyword | Buona Novella | stesso | — |
| ScriptureReference | Atti 8:26-40 | stesso | — |
| Hint / Solution / EngagementNote | IT (C# apostrofi ASCII) | IT da stories.js (apostrofi tipografici) | wording tipografico, non strutturale |
| ScriptureQuote | ASCII `Gesu'` | `Gesù` (stories.js) | tipografico |
| **VisibleEmojis** | `1F47C, 2753, 1F30A, 1F4D6, Hackney-100` | **stesso** | — |
| **HiddenEmojis** | **`203C, 1F4E3`** | **`1F4D6, Hackney_100`** | **REGRESSIONE** |
| **HintEmoji** | **`203C`** | **`1F4AC`** | **REGRESSIONE** |
| ImageCaptions | 8 voci (C#) | 8 voci (da C# in Step 1) | allineate |
| IsDynamic | false | false | — |
| ImageResourceName | Philip | Philip | — |

**`stories.js` JW_STORIES Ep 12:**

| Campo | Pre-Step2 | Post-Step2 |
|-------|-----------|------------|
| visibleKeys | **3** chiavi (`1F47C, 2753, 1F30A`) | **5** (da C#) |
| hiddenKeys | `1F4D6, Hackney_100` | **invariato** (e questo è il problema) |
| hintKey | `1F4AC` | **invariato** |
| imageCaptions | 6 | 8 (da C#) |

**Causa merge (Step 1):** `visibleKeys`/`captions` presi da C# (completi); `hiddenKeys`/`hintKey` tenuti da `stories.js` incompleto → **sovrapposizione** `1F4D6` in visible[3] e hidden[0]; `Hackney-100` (visibile) vs `Hackney_100` (nascosto, underscore).

**STORIES inline `index.html` Ep 12:** solo Q&A/theater (titolo corto “Filippo e l’Eunuco”); **non** contiene PNG keys — irrilevante per reveal desktop.

**PNG su disco:** tutte le chiavi citate (`Hackney-100`, `Hackney_100`, `1F4AC`, `203C`, `1F4E3`) esistono in `Resources/` e `webapp/assets/`. Non è un 404 PNG.

---

## 3. Fase 1 — State machine `DynamicStoryForm.cs`

### Modello stato

```text
bool[3] revealed
  [0] = slot 5 (prima nascosta)
  [1] = slot 6 (seconda nascosta)
  [2] = slot 7 (indizio)
Timer hintPulseTimer @ 300ms → pulsa BackColor slot 7 finché !revealed[2]
```

### Pseudo-codice attuale (rilevante)

```text
RevealButton_Click:
  if !revealed[0]:
      show image Tag→slot5; revealed[0]=true
      button.Text = "Nascondi 2 immagini"
  else if !revealed[1]:
      show image Tag→slot6; revealed[1]=true
      button.Enabled = false          // ← dopo 2 reveal: click impossibile
      button.Text = "Nascondi 2 immagini"
  // NON ESISTE ramo hide / reset

HintButton_Click:
  if !revealed[2]:
      show Tag→slot7; revealed[2]=true
      button.Enabled = false          // ← "Nascondi indizio" ma click morto
      button.Text = "Nascondi indizio"
  // NON ESISTE ramo hide

HintPulseTimer_Tick:
  if revealed[2]: Stop; reset color; return
  toggle amber BackColor on picBoxes[7]
  // UI thread WinForms Timer — no Invoke, no DoEvents
```

### Punti sospetti (riga → atteso vs osservato)

| Riga | Condizione | Atteso (UX label) | Osservato |
|------|------------|-------------------|-----------|
| **L343 / L352** | Text = `HideImages` | Click successivo nasconde | Click rivela ancora (2ª volta) o **no-op** se `Enabled=false` |
| **L351** | `revealButton.Enabled = false` dopo 2 reveal | Nascondi possibile | Pulsante morto → “non nasconde” |
| **L364–365** | Hint: `Enabled=false` + testo HideHint | Toggle hide | Pulsante morto → percepito come blocco |
| **L336–355** | Nessun `HideReveal()` | Esiste hide collegato | **Assente** (blame: da 2026-04-22, pre-Step2) |
| Timer L207–220 | Hint prima del reveal | Pulse stoppa dopo reveal hint | OK; **nessun deadlock** nel codice |

**Conclusione state machine:** bug **pre-esistente** di etichette “Nascondi*” senza implementazione hide + disable del bottone. Non è un freeze CPU/thread; è UI one-way.

---

## 4. Fase 2 — Comparativa strutturale (post-Step2 / HEAD)

| Ep | vis/hid/hint | Dup vis∩hid | Note |
|----|--------------|-------------|------|
| 1 | 5/2/1 | no | OK |
| 10 | 5/2/1 | **`1F451` in vis e hid** | **Regressione merge** (pre: hid `1F981,1F5FA`) |
| 11 | 5/2/1 | no | OK |
| **12** | 5/2/1 | **`1F4D6` in vis e hid** | **Regressione merge**; hint `1F4AC` vs pre `203C` |
| 13 | 5/2/1 | no | OK |
| 18 | 5/2/1 | no | OK |

Tipi C#: sempre `string[]` via `new[] { ... }` — nessun null strutturale.

**Ep 12 non è l’unico cambiato:** anche Ep 10 ha hidden/hint drift. Il sintomo “Nascondi” colpisce **tutti** gli episodi via DynamicStoryForm; Ep 12/10 hanno in più **chiavi rebus errate/duplicate** (qualità puzzle, non freeze).

---

## 5. Fase 3 / 4 — Riproduzione

### Desktop (analisi statica + checklist umana)

L’agent non ha automatizzato click WinForms. Esito **codice-deterministico**:

| Scenario | Ep 12 | Ep 10 | Ep 13 |
|----------|-------|-------|-------|
| A: Rivela×1 → click “Nascondi…” | **KO hide** (2ª reveal) | stesso (bug form) | stesso |
| A: Rivela×2 → “Nascondi…” | **KO** (Enabled=false) | stesso | stesso |
| B: Indizio prima di reveal | Indizio OK; pulsante **disabilitato**; **no deadlock nel codice** | stesso | stesso |

**Checklist per l’utente (conferma “freeze”):**

1. Aprire `bin\Debug\Jw_Quiz_Development.exe` → Ep 12.  
2. Scenario A: 1× Rivela → 1× Nascondi → annotare se rivela la 2ª o non fa nulla.  
3. Scenario A2: fino a 2 reveal → verificare se Nascondi è grigio.  
4. Scenario B: Indizio subito → Task Manager: CPU ~0%? App risponde ad altri click (Soluzione / Prossima)?  
5. Ripetere Ep 10 e Ep 13.  
6. Annotare se “blocco” = freeze vero o solo pulsante morto.

### Webapp (statica + preview non smoke-click)

| Path | Reveal hide | Hint pre-reveal |
|------|-------------|-----------------|
| **Immersive** rebus 3D (`index.html`) | Reveal solo incrementa; button `disabled` a count≥2; **nessuna label Nascondi immagini** | Hint one-way; solution toggle sì |
| **Classic** (`app.js` L1037–1048) | Stesso pattern one-way + `revealBtn.disabled` a ≥2 | Hint one-way + disable |
| Quiz / Journey | Rebus act come sopra se journey include rebus | n/a per quiz puro |

**Verdetto web:** stesso modello **one-way** del desktop (classic). Non è un bug nuovo di Step 2 sul theater; Step 2 ha però corrotto i **dati** Ep 12/10 in `stories.js` (dup keys).

---

## 6. Causa deterministica (combinazione)

| ID | Tipo | Evidenza |
|----|------|----------|
| **(b) primaria UX** | Bug pre-esistente `DynamicStoryForm` (e mirror `app.js`) | Label Hide* senza ramo hide; `Enabled=false` dopo reveal completo / hint (dal 2026-04-22) |
| **(a) regressione dati Step 1→2** | Ep 12 (e 10) hidden/hint sbagliati nel JSON/generator | Tabella Fase 0; merge ha privilegiato JS incompleto su hidden/hint |
| **(c) interazione** | Dup keys peggiorano il puzzle Ep 12 | Reveal “nasconde” contenuto già visibile → sensazione di “non funziona” su Ep 12 |

**Il “freeze” da Indizio prima del reveal non è spiegato da deadlock:** nessun `Invoke` ricorsivo / `DoEvents`. Ipotesi forte: pulsante disabilitato + testo “Nascondi indizio” = UI percepita bloccata. Da confermare con checklist §5.

---

## 7. Proposta fix (NON applicata — attende OK)

### Opzione F1 — Fix minimo state machine desktop (consigliata per sintomo A/B)

**File:** `DynamicStoryForm.cs`  
**Idea:** implementare toggle hide quando il testo è Hide*, e **non** disabilitare i pulsanti (o riabilitarli).

Pseudo-patch:

```csharp
// RevealButton_Click — se entrambi rivelati, Hide ripristina placeholder 2753 e revealed[0/1]=false
// HintButton_Click — se revealed[2], Hide ripristina KeyHint placeholder e revealed[2]=false; restart timer
// Rimuovere revealButton.Enabled = false e hintButton.Enabled = false
```

**Rollback:** revert commit su `DynamicStoryForm.cs`.  
**Impatto Step 4:** nessuno (solo desktop).

### Opzione F2 — Correggere dati Ep 12 (e 10) in `data/episodes.json`

Ripristinare hidden/hint **pre-Step2 C#**:

| Ep | hiddenKeys | hintKey |
|----|------------|---------|
| 12 | `203C`, `1F4E3` | `203C` *(o hint dedicato se si vuole evitare dup con hidden[0] — pre aveva hint=203C = stesso di hidden[0])* |
| 10 | `1F981`, `1F5FA` | `1F5FA` |

Poi: `python tools/generate_story_artifacts.py` (+ `sync_all` se serve).

**Nota:** pre-Step2 Ep 12 aveva già `HintEmoji == HiddenEmojis[0]` (`203C`) — accettabile storicamente; l’importante è **non** riusare le visible keys come hidden.

**Rollback:** revert JSON + regenerate.  
**Impatto Step 4:** migliora rebus web; non sblocca hide.

### Opzione F3 — Mirror fix in `app.js` / immersive (opzionale, dopo F1)

Classic/immersive: aggiungere hide toggle o non promettere “Nascondi” (oggi immersive non etichetta Nascondi su reveal).  
**Off-limits attuali:** `app.js` — richiede OK esplicito.

### Raccomandazione

1. **F1** subito (chiude sintomo Nascondi / Indizio “bloccato” su **tutti** gli episodi).  
2. **F2** subito dopo (chiude regressione puzzle Ep 12/10).  
3. Step 4 adapter può procedere **dopo F1+F2** (o almeno F2 se si accetta hide come debito UX noto).

---

## 8. Test di regressione (post-fix)

1. Desktop Ep 12: Rivela×1 → Nascondi → slot 5 torna `?`.  
2. Desktop Ep 12: Rivela×2 → Nascondi → entrambi `?`; Rivela di nuovo OK.  
3. Desktop Ep 12: Indizio prima di reveal → nascondi indizio → pulse riparte; form resta responsivo.  
4. Desktop Ep 10 e Ep 1: stessi 3 scenari.  
5. Classic + immersive rebus Ep 12: texture 5+2+1 coerenti, no dup vis/hid; console senza 404.

---

## 9. Impatto su Step 4

| Fix | Blocca Step 4? |
|-----|----------------|
| Solo F1 (`DynamicStoryForm`) | No |
| F2 (`episodes.json` + regenerate) | No — anzi pulisce dati per adapter |
| F3 (`app.js` / index rebus UI) | Può intrecciare Step 4 → fare **dopo** o insieme con OK |

**Step 4 resta BLOCCATO** finché umano non approva almeno F1+F2 (o dichiara debito hide accettabile e approva solo F2).

---

## 10. Riferimenti

- `DynamicStoryForm.cs` L11, L202–220, L336–368  
- `AppText.cs` HideImages / HideHint  
- `data/episodes.json` id 12  
- Merge notes: `data/episodes_migration_report.json` (id 12 visible da C#, hint da JS)  
- Commit Step 2: `aa8791f`
