# Spec F — Streak no-hint + Badge (2026-09-30)

**Stato:** design only — **nessun runtime**.  
**Prerequisiti chiusi:** E2 i18n; stars policy A; multiplayer MVP (`jwquiz_challenge_v1` session-only).  
**Superfici in scope:** web HUD (`webapp/index.html`) + desktop `ProgressTracker` / `ProgressPanel`.  
**Fuori scope:** `classic.html`, `app.js`, `jwquiz_challenge_v1`, `data/episodes.json`, sync cloud.

Evidenza storage attuale:
- Web: `PROGRESS_KEY = "jwquiz_immersive_progress_v1"` (`webapp/index.html` ~1318–1464) — shape `{ xp, episodes: { [id]: { stars, mode, at } } }`.
- Desktop: `UserProgress.dat` via `ProgressTracker.cs` (linee testo + badge completion-count: PrimiPassi / Studioso / Esperto / StudioDiligente).
- Challenge: `jwquiz_challenge_v1` — **non** usare per streak persistente.

---

## 1. Definizioni

| Termine | Definizione |
|---------|-------------|
| **Hint usato** | `hintEverUsed === true` (web) / equivalente desktop peak-help: hint attivato almeno una volta nell’episodio (policy A — hide non cancella la penalità). Reveal immagini **non** azzera lo streak (solo hint). |
| **Episodio “pulito” (no-hint)** | Completamento con `hintEverUsed === false` (e analogo desktop). |
| **Streak no-hint** | Numero di episodi **completati consecutivamente** (ordine cronologico di completamento, non ID crescente) senza hint usato. |
| **Cross-sessione** | Persistente in storage locale; sopravvive a reload / riavvio app. |
| **Reset** | Al completamento di un episodio con hint usato → `noHintStreak = 0` (max storico invariato). |
| **Badge streak** | Sblocco one-shot quando `maxNoHintStreak` (o streak corrente, HITL F4) raggiunge una soglia. |

**Nota policy A:** `docs/STARS_POLICY_DECISION.md` — peak help; hide reveal/hint non ripristina stelle. Streak allinea la stessa semantica su `hintEverUsed`.

---

## 2. Storage schema (web) — estensione backward-compat

**Chiave reale:** `jwquiz_immersive_progress_v1` (non `jwquiz_progress_v1`; documentato qui per evitare drift).

### 2.1 Shape attuale (evidenza)

```json
{
  "xp": 0,
  "episodes": {
    "11": { "stars": 2, "mode": "journey", "at": 1720000000000 }
  }
}
```

(+ mirror legacy `jwquiz_web_xp` per XP).

### 2.2 Shape proposta (v1 estesa, stesso `version` implicito / campo opzionale)

```json
{
  "xp": 0,
  "episodes": {
    "11": {
      "stars": 2,
      "mode": "journey",
      "at": 1720000000000,
      "hintUsed": false
    }
  },
  "noHintStreak": 0,
  "maxNoHintStreak": 0,
  "badges": [],
  "lastPlayedAt": "2026-09-30T12:00:00.000Z",
  "completionOrder": []
}
```

| Campo | Tipo | Note |
|-------|------|------|
| `episodes[id].hintUsed` | bool | NUOVO opzionale; default `false` se assente (migrazione). Impostato a `true` se in quella run `hintEverUsed`. |
| `noHintStreak` | int | NUOVO; default `0`. |
| `maxNoHintStreak` | int | NUOVO; default `0`. |
| `badges` | string[] | NUOVO; id badge streak (`streak_attento`, …). |
| `lastPlayedAt` | ISO string | NUOVO opzionale. |
| `completionOrder` | number[] | NUOVO opzionale; coda cronologica id episodio per audit “consecutivo”. |

### 2.3 Migrazione lettura

```text
loadProgress():
  raw = JSON.parse(localStorage[PROGRESS_KEY] || "{}")
  return {
    xp: Number(raw.xp || …),
    episodes: raw.episodes || {},
    noHintStreak: Number(raw.noHintStreak || 0),
    maxNoHintStreak: Number(raw.maxNoHintStreak || 0),
    badges: Array.isArray(raw.badges) ? raw.badges : [],
    lastPlayedAt: raw.lastPlayedAt || null,
    completionOrder: Array.isArray(raw.completionOrder) ? raw.completionOrder : []
  }
```

File utenti esistenti senza i nuovi campi restano validi (default 0 / []).

### 2.4 Aggiornamento a completamento (logica)

```text
on completeEpisode(storyId):
  hintUsed = state.rebus.hintEverUsed  // policy A
  if hintUsed:
    noHintStreak = 0
  else:
    noHintStreak += 1
    maxNoHintStreak = max(maxNoHintStreak, noHintStreak)
  episodes[id] = { …prev, stars, mode, at, hintUsed }
  completionOrder.push(storyId)
  lastPlayedAt = now ISO
  unlock badges if thresholds met (see §3)
  saveProgress()
```

**Replay:** se l’utente ricompleta lo stesso id, conta comunque come evento cronologico (streak su run, non su unique id). HITL F4 se badge solo su “prima volta pulita”.

---

## 3. Badge soglie (proposta — HITL F1/F2)

| id | Nome IT | Nome EN | Soglia (`maxNoHintStreak` ≥) |
|----|---------|---------|------------------------------|
| `streak_attento` | Attento | Attentive | 5 |
| `streak_diligente` | Diligente | Diligent | 10 |
| `streak_esperto` | Esperto | Expert | 20 |

**Collisione desktop:** `ProgressTracker` ha già badge id `Esperto` (“Esperto Biblico”, ≥12 completamenti). Usare id prefissati `streak_*` e label UI distinte (es. “Esperto (senza indizi)”) per evitare overlap.

**Evitare** nomi dottrinali (Saggio / Profeta / Apostolo) — KB storico; sostituiti da questa proposta.

---

## 4. UI

### 4.1 Web HUD (`index.html`)

- Chip streak accanto a `#hudProgress`: es. `7 no-hint` / i18n `streak_noHint`.
- Al raggiungimento soglia: toast/pop-up breve (palette brand oro/blu `#E8C547` / `#0B1220`), non nel hero.
- Nessun badge nel first viewport marketing (brand rules).

### 4.2 Desktop

- `ProgressPanel`: riga “Streak senza indizio: N (max M)” + lista badge `streak_*` oltre ai badge completion esistenti.
- Persistenza: estendere formato `UserProgress.dat` (linee aggiuntive) **oppure** future JSON (G5) — **MVP:** aggiungere campi al Save/Load testo attuale senza BinaryFormatter rewrite forzato.

### 4.3 i18n (chiavi UI.it / UI.en)

| key | it (proposta) | en (proposta) |
|-----|---------------|---------------|
| `streak_noHint` | {n} senza indizio | {n} no-hint |
| `streak_max` | Record: {n} | Best: {n} |
| `badge_attento` | Attento | Attentive |
| `badge_diligente` | Diligente | Diligent |
| `badge_esperto` | Esperto (senza indizi) | Expert (no hints) |
| `badge_unlocked` | Nuovo badge: {name} | New badge: {name} |

---

## 5. Cross-surface

| Superficie | Storage | Note |
|------------|---------|------|
| Web immersive | `jwquiz_immersive_progress_v1` | Source MVP web |
| Desktop | `UserProgress.dat` / `ProgressTracker` | Campi streak paralleli; **no sync** web↔desktop in MVP |
| Classic / challenge | — | Esclusi |

Migrazione BinaryFormatter → JSON (G5): **future work**, non bloccante per F MVP se Save/Load testo attuale può appendere campi.

---

## 6. Non-goals

- Nessun backend / cloud sync.
- Nessuna modifica a `data/episodes.json` / versetti.
- Nessuna modifica a `jwquiz_challenge_v1`.
- Nessun tocco a logica stelle (policy A resta).
- Nessun tocco a `app.js` / analytics / wrangler.
- Nessun rename badge completion desktop esistenti.

---

## 7. Implementazione (post-HITL) — outline

1. Web: estendere `loadProgress` / `completeEpisode` / `refreshHud` + toast badge.  
2. Desktop: `CompleteStory` riceve flag `hintUsed`; aggiorna streak; `CheckStreakBadges()`.  
3. Test: 5 completamenti no-hint → badge Attento; 1 con hint → streak 0; reload conserva max.  
4. KB §10/§13 dopo apply.

---

## 8. HITL F1–F5 (one-line)

```text
F1:  [ ] Attento/Diligente/Esperto  [ ] altri nomi (incolla)  [ ] defer
F2:  [ ] soglie 5/10/20  [ ] altre (incolla)  [ ] defer
F3:  [ ] reset streak su hint usato = sì  [ ] no  [ ] defer
F4:  [ ] badge su maxNoHintStreak (qualsiasi run)  [ ] solo prima run pulita per episodio  [ ] defer
F5:  [ ] UI web + desktop nel MVP  [ ] solo web  [ ] solo desktop  [ ] defer
```

**Proposte assistente:** `F1=Attento/Diligente/Esperto` · `F2=5/10/20` · `F3=sì` · `F4=maxNoHintStreak` · `F5=web+desktop`

Shortcut:

```text
F1a F2a F3yes F4max F5both
```

---

## 9. Riferimenti

| Doc / file | Perché |
|------------|--------|
| `docs/STARS_POLICY_DECISION.md` | Policy A peak-help / `hintEverUsed` |
| `webapp/index.html` `PROGRESS_KEY` | Storage web reale |
| `ProgressTracker.cs` / `ProgressPanel.cs` | Badge e progress desktop |
| `docs/TECHNICAL_REVIEW_REPORT.md` Prompt F | Origin request |
| `.github/KB.md` §11 | Next: F dopo E2 chiuso |
