# Desktop bugs — indagine 2026-09-30 (sola lettura)

**Scope:** post onboarding I (`ebf63ed`). Nessun fix applicato in questo documento.  
**Segnalazioni:** screenshot utente (menu Storie / DynamicStoryForm).

## Sintesi

| # | Bug | Severità | Superficie |
|---|-----|----------|------------|
| 1 | Menu «Nuovi Episodi» mostra solo 13–18 (mancano 19–23) | **major** | Form1 runtime menu |
| 2 | Layout menu Storie caotico / lungo con 23 episodi | **minor→major** UX | Designer + Form1 |
| 3 | Griglia 8 PictureBox non centrata orizzontalmente | **major** visual | DynamicStoryForm |

---

## Bug 1 — Nuovi Episodi senza 19–23

### Diagnosi (righe esatte)

`Form1.cs` — costruzione runtime (non Designer):

```190:196:Form1.cs
            for (int id = 13; id <= 18; id++)
            {
                int capturedId = id;
                var item = new ToolStripMenuItem(AppText.Get("StoryPrefix") + " " + capturedId);
                item.Click += (s, e) => OpenStory(capturedId);
                nuoviEpisodiMenuItem.DropDownItems.Add(item);
            }
```

Localizzazione testi (stesso range hardcoded):

```178:180:Form1.cs
            for (int id = 13; id <= 18 && id - 13 < nuoviEpisodiMenuItem.DropDownItems.Count; id++)
            {
                nuoviEpisodiMenuItem.DropDownItems[id - 13].Text = AppText.Get("StoryPrefix") + " " + id;
```

Episodi 1–12: items **Designer** in `Form1.Designer.cs` (`storieToolStripMenuItem.DropDownItems`, ~L81–93).

### StoryLibrary

`StoryLibrary.cs` contiene `Id = 19` … `Id = 23` (generato K2). **Dati OK.**

### Causa root

**Hardcode range `13..18`** in `BuildDynamicMenus` / `ApplyLocalization`.  
Non è StoryLibrary incompleto né filtro su `StoryEngine`. Il sottomenu «Nuovi Episodi» non è derivato da `TotalStories`.

### Fix proposto (NON applicato)

1. Sostituire il loop con `for (int id = 13; id <= StoryEngine.TotalStories; id++)` (o costante `FirstDynamicId = 13`).
2. Allineare il loop di `ApplyLocalization` allo stesso upper bound.
3. Smoke: aprire Menu → Storie → Nuovi Episodi → verificare voci 13–23 e apertura Ep 19/23.

### Rollback

Ripristinare `id <= 18` nei due loop.

---

## Bug 2 — Layout menu caotico

### Diagnosi

Struttura attuale (ibrida):

| Range | Dove | Forma |
|-------|------|--------|
| 1–12 | `Form1.Designer.cs` flat sotto «Storie» | 12 `ToolStripMenuItem` + naming legacy (`storia2ToolStripMenuItem1` = Storia 3, …; `storia10` = Storia **12**) |
| 13–18 (+futuri) | runtime sotto «Nuovi Episodi» | dropdown annidato |
| Utente / Crea | runtime | sotto «Storie» |

Effetti con catalogo 23:

- Lista «Storie» già lunga (12 flat + separator + Nuovi + Crea + Utente).
- «Nuovi Episodi» con 11 voci (13–23 dopo Bug1) = dropdown lungo, **nessuno scroll dedicato** oltre al comportamento nativo WinForms (può diventare colonna / multi-column su schermi piccoli).
- Naming Designer confuso (manutenzione).

Non c’è raggruppamento per tema né dialog selector.

### Opzioni design (NON implementare)

| Opzione | Idea | Pro | Contro | Effort |
|---------|------|-----|--------|--------|
| **A** | Sottomenu `1–12` / `13–18` / `19–23` | Chiaro, compatibile WinForms | Un click in più | **S** (½–1 gg) |
| **B** | Sottomenu per tema (`keyword`) | Pedagogico | Temi duplicati (es. Coraggio); i18n titoli | **M** |
| **C** | Dialog selector (ListBox + Apri) al posto del dropdown lungo | Scalabile a N episodi; anti-spoiler opzionale (solo id/tema) | Cambio UX; accessibilità | **M** |
| **D** | Unificare tutto in un solo menu dinamico da `StoryEngine` (elimina Designer 1–12) | Una sola fonte di verità | Refactor click handlers Designer | **M** |

**Raccomandazione:** **A + D leggero** — popolare dinamicamente tre fasce da `StoryEngine.TotalStories`, deprecare items Designer 1–12 in un secondo passo.

### Rollback

Ripristinare struttura menu precedente (Designer + Nuovi 13–18).

---

## Bug 3 — PictureBox non centrati orizzontalmente

### Diagnosi (righe esatte)

`DynamicStoryForm.cs` — layout **manuale** (non TableLayoutPanel):

```111:150:DynamicStoryForm.cs
            var imagePanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 410,
                ...
            };
            ...
                pb.Left = 40 + col * 190;
                pb.Top = 58 + row * 170;
```

Griglia 4×2: `Width=150`, passo orizzontale `190`, offset sinistro fisso `40`.

Larghezza occupata ≈ `40 + 3*190 + 150 = 760` px.  
Con form tipica ≥ 1000–1200 px (header già usa `Left=780` per XP), rimane **spazio a destra** non bilanciato → griglia percepita “a sinistra”.

Nessun handler `Resize` per ricalcolare `Left`. `Anchor` default (Top|Left).

### Causa root

**Posizionamento assoluto con offset sinistro fisso**; container `Dock=Top` a piena larghezza senza centering della griglia.

### Fix proposto (NON applicato)

Preferito (effort S):

```csharp
// su Layout/Resize di imagePanel:
int gridW = 40 + 3 * 190 + 150; // o 4*150 + 3*gap
int startX = Math.Max(16, (imagePanel.ClientSize.Width - gridW) / 2);
pb.Left = startX + col * 190;
```

Alternative:

- `TableLayoutPanel` 4 colonne, `Dock=None`, centrato nel parent.
- Padding simmetrico calcolato: `Padding = new Padding(startX, …)`.

Smoke: ridimensionare finestra; verificare margini L≈R.

### Rollback

Ripristinare `pb.Left = 40 + col * 190`.

---

## Priorità consigliata

| Ordine | Bug | Perché |
|--------|-----|--------|
| **1** | Bug 1 (19–23 menu) | Blocca accesso desktop agli episodi K2; fix 2 loop |
| **2** | Bug 3 (centratura) | Visibile subito in DynamicStoryForm; fix locale Resize |
| **3** | Bug 2 (layout menu) | UX; meglio dopo Bug1 (range noto) con opzione A/D |

---

## Commit proposto (NON eseguire)

```text
git add docs/DESKTOP_BUGS_2026-09-30.md .github/KB.md
git commit -m "docs(bug): indagine 3 bug desktop (menu/layout/centratura)"
```
