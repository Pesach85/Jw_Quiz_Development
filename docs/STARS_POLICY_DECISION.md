# Stars policy — decisione unica cross-surface

**Data:** 2026-09-29  
**Stato:** Proposta (design only — non implementata)  
**Contesto:** post H3-bis (F1 desktop, F3 immersive, F3C-A classic)

---

## 1. Problema

Dopo l’introduzione del toggle reveal/hint, le tre superfici rebus non condividono la stessa regola sulle stelle / XP previsti:

| Superficie | File | Policy attuale | Comportamento su hide |
|------------|------|----------------|------------------------|
| Desktop | `DynamicStoryForm.cs` (F1) | **B** — ricalcolo | `HelpsUsed() = revealState + hint` → hide ripristina stelle |
| Immersive | `webapp/index.html` (F3) | **B** — ricalcolo | `used = helps + revealCount + hint` → hide azzera conteggio reveal/hint |
| Classic flat | `webapp/app.js` (F3C-A) | **A** — peak | `maxRevealUsed` / `hintEverUsed` → hide **non** ripristina |

Il giocatore che passa da classic a desktop (o immersive) percepisce regole diverse sullo stesso gesto “Nascondi”. Va scelta **una** policy e allineate le tre superfici.

---

## 2. Definizioni

### Policy A — peak help (penalità permanente per episodio)

- Ogni avanzamento di reveal (slot 5, poi 6) alza un picco `maxRevealUsed` (0…2).
- Il primo show dell’indizio setta `hintEverUsed = true`.
- Hide rivela/nasconde solo la **vista**; stelle e XP previsti restano sul picco.
- Reset picchi solo al cambio episodio / nuovo load.

### Policy B — ricalcolo sullo stato corrente

- Stelle / XP = funzione di ciò che è **ora** rivelato.
- Hide → conteggio aiuti scende → stelle risalgono.

*(Invariato in entrambe: reset a inizio episodio; solution toggle non è un “help” di slot; immersive tiene `state.helps` separato per errori quiz.)*

---

## 3. Criteri UX (letteratura applicata)

| Criterio | Implicazione | Favorisce |
|----------|--------------|-----------|
| **Loss aversion** (Kahneman & Tversky) | Una stella persa “pesa” più di una recuperata; ripristinarla dopo hide rende la perdita **reversibile e poco credibile** | **A** |
| **Perceived fairness** | Fair = “pago per quello che ho visto”, non “pago per quello che è ancora in schermo”. L’informazione già vista non si può disimparare | **A** |
| **Anti-gaming** | Con B: reveal → peek → hide → stelle piene → soluzione = score falso | **A** |
| **Retrieval practice / desirable difficulties** (Bjork) | Il rebus premia il richiamo senza aiuto; una volta usato l’aiuto, la prova “senza aiuto” è già compromessa | **A** |
| **Board-state metaphor** | Stelle = “quanto del puzzle è aperto adesso” (come pezzi sul tavolo) | B |
| **Feedback immediato leggibile** | B è più “fisico” (nascondo → riprendo); può confondere il significato di XP “previsti” | B (superficiale) |

Il prodotto è **didattico** (anti-spoiler, XP come segnale di sforzo), non un puzzle sandbox dove “nascondi” equivale a “annulla mossa”. La metafora board-state (B) è secondaria rispetto a fairness informativa e anti-cheat leggero.

---

## 4. Raccomandazione

### **Policy A (peak help) come unica policy cross-surface.**

**Perché:**

1. Allinea stelle/XP al fatto cognitivo: *hai già visto* l’immagine / l’indizio.
2. Impedisce il pattern peek-and-restore (B).
3. Classic F3C-A è già su A — due superfici (desktop + immersive) vanno aggiornate, una sola resta.
4. Coerente con il messaggio UI “XP previsti” come stima del premio a fine episodio, non come riflettore del DOM.

**Non raccomandato (ora): B** — semplifica il codice ma indebolisce il segnale di apprendimento e la fairness tra giocatori.

**Ibrido scartato:** A solo a fine episodio / B in live UI → doppia verità (stelle sul chip ≠ XP guadagnato); confonde più di quanto aiuti.

---

## 5. Piano di implementazione (post-approvazione)

### 5.1 Desktop → A (`DynamicStoryForm.cs`)

- Aggiungere `maxRevealUsed` (int) e `hintEverUsed` (bool), reset in costruttore / eventuale nuovo episodio se lo stesso form viene riusato (oggi form nuovo per storia).
- `HelpsUsed()` → `maxRevealUsed + (hintEverUsed ? 1 : 0)`.
- In `RevealButton_Click` / `UpdateRevealUi`: dopo `revealState++`, `maxRevealUsed = Math.Max(maxRevealUsed, revealState)`; su hide (`revealState = 0`) **non** azzerare il picco.
- In `HintButton_Click`: se `hintRevealed` diventa true, `hintEverUsed = true`.
- Test manuale: Ep 1/12 — reveal×2 → hide → stelle restano basse; hint on/off → stella hint non torna.

**Rollback:** revert commit desktop.

### 5.2 Immersive → A (`webapp/index.html`)

- Estendere `state.rebus` (o top-level) con `maxRevealUsed`, `hintEverUsed` (reset in `openTheater` / dove si fa `state.rebus = {…}`).
- `recomputeStars`:  
  `used = state.helps + state.rebus.maxRevealUsed + (state.rebus.hintEverUsed ? 1 : 0)`  
  (tenere `state.helps` per errori quiz).
- Nel click reveal: aggiornare picco prima/dopo increment; su reset a 0 non toccare picco.
- Nel click hint: se on → `hintEverUsed = true`.
- Smoke: Ep 1 rebus — stesso scenario A del classic.

**Rollback:** revert commit immersive.  
**Nota Step 4:** se l’adapter STORIES tocca `index.html`, sequenziare **dopo** o nello stesso PR con review esplicita del hunk stelle.

### 5.3 Classic — nessuna modifica se si sceglie A

`app.js` F3C-A resta riferimento. Nessun nuovo OK su `app.js`.

### 5.4 Se (invece) si scegliesse B — solo per completezza

- **Richiede OK umano esplicito** (secondo tocco `app.js`).
- Patch &lt; 15 righe: rimuovere `maxRevealUsed` / `hintEverUsed`; `calcStars` / `calcExpectedXp` su `revealCount` + `hintRevealed`; reset semplificato in `loadStory`.
- Desktop + immersive già B → no change.
- **Non raccomandato** da questo documento.

---

## 6. Commit / test / KB (quando si implementa)

| Ordine | Commit suggerito |
|--------|------------------|
| 1 | `fix(desktop): stars policy A peak-help (DynamicStoryForm)` |
| 2 | `fix(web): stars policy A peak-help immersive` |
| 3 | `kb: stars policy A cross-surface` (§10/§13/§17) |

**Checklist smoke (tutte e 3 le superfici, Ep 1 + 12):**

1. Reveal 1 → stelle −1; hide non disponibile finché non a 2 (label); a 2 → Nascondi → vista nascosta, **stelle invariate**.
2. Hint on → −1; hint off → stelle **invariate**.
3. Reveal+hint → hide tutto → stelle = picco; XP previsti coerenti.
4. Nuovo episodio → stelle di nuovo ★★★.

---

## 7. Impatto su Step 4 (G1 adapter)

- Step 4 (adapter `index.html` STORIES) **non dipende** da questa policy, ma conviene **chiudere l’allineamento A su immersive prima o insieme allo Step 4** per evitare doppio tocco su `index.html`.
- Classic / desktop fuori dallo Step 4.

---

## 8. Decisione richiesta all’umano

- [ ] **Approvare Policy A** → procedere con piano §5.1 + §5.2 (nessun tocco `app.js`).
- [ ] Rifiutare e chiedere Policy B → STOP finché OK esplicito su `app.js` (&lt; 15 righe) come §5.4.

**Questo file è solo design. Nessuna patch applicata in questo commit.**
