# Dataset unification design (G1) — JW Quiz

**Date:** 2026-09-29  
**Phase:** DESIGN only (no `webapp/` changes in this deliverable)  
**Audience:** product owner / revisore tecnico / agent build (Prompt C)  
**Goal:** eliminare il drift tra Q&A immersivo (`STORIES` in `index.html`), rebus web (`webapp/stories.js`) e desktop (`StoryLibrary.cs`).

---

## 1. Context & invariants (must hold)

| Invariant | Implication for unification |
|-----------|------------------------------|
| 3 modalità (`quiz` / `rebus` / `journey`) | Schema must expose `questions[]` + rebus keys; theater `buildFlow()` unchanged in behavior |
| Anti-spoiler rebus | Grid/tiles: episodio + tema, not title; captions policy on rebus fields only |
| IT source of truth + EN parallel | Canonical record carries `it` + `en` for immersive copy; rebus long text can use `story-i18n` shape |
| No npm bundler | Ship generated `stories.js`; `index.html` loads script tag (CDN only for Three.js) |
| Desktop net472 | No new NuGet; prefer **generated** `StoryLibrary.cs` or partial from Python |
| Rebus 3D | Continues `window.JW_STORIES` + `buildRebusLookup()` — keys stay PNG, never Unicode in slot data |
| `app.js` / `classic.html` | Out of scope for merge; classic keeps using `stories.js` + API i18n |
| `sync_all.py` | Becomes orchestrator: optional photo apply + **generate story artifacts** + android www |

**Off-limits in build:** `analytics.js`, `wrangler.toml`, `functions/api/*`, `app.js` patches.

---

## 2. Step 1 — Drift map (current state)

### 2.1 Field-level matrix

| Field | `index.html` `STORIES` | `webapp/stories.js` | `StoryLibrary.cs` | Aligned? |
|-------|------------------------|---------------------|-------------------|----------|
| **id** | `story(id, …)` | `id` | `Id` | Yes (1–18) |
| **Titolo IT** | `title.it` | `title` (IT only) | `Title` | Mostly; see ep. 8 |
| **Titolo EN** | `title.en` | — | — (desktop via `StoryLocalizationService` rule-based) | **No** — EN only in index |
| **Tema IT** | `theme.it` | `keyword` | `Keyword` | Mostly (accents/apostrophe) |
| **Tema EN** | `theme.en` | — | — | **No** |
| **Scrittura (ref breve)** | `scripture` | `scriptureReference` | `ScriptureReference` | Mostly same string |
| **Intro IT/EN** | `intro {it,en}` | — | — | **Immersive only** |
| **2× MCQ IT/EN** | `questions[]` | — | — | **Immersive only** |
| **Morale IT/EN** | `moral {it,en}` | — | — | **Immersive only** |
| **Citazione theater IT/EN** | `quote {it,en}` (short) | — | — | **Immersive only** |
| **Citazione TNM lunga** | — | `scriptureQuote` | `ScriptureQuote` | **Rebus/desktop only** |
| **Hint / solution / engagement** | — | `hint`, `solution`, `engagementNote` | same | Aligned IT prose |
| **5+2+1 PNG keys** | — (via `JW_STORIES`) | `visibleKeys`, `hiddenKeys`, `hintKey` | `VisibleEmojis`, `HiddenEmojis`, `HintEmoji` | Aligned keys |
| **8 caption** | — | `imageCaptions[]` | `ImageCaptions[]` | Aligned IT |
| **Intro symbols** | `symbols[]` (Unicode emoji decor) | — | — | Theater-only; not in rebus data |
| **i18n API shape** | — | no `translations` | `SourceLanguage` + `Translations` on model | User stories only today |

### 2.2 Sample concrete drifts

| Ep | Issue | `index.html` | `stories.js` / `StoryLibrary.cs` |
|----|--------|--------------|-----------------------------------|
| **8** | Titolo | `"Abramo e Isacco"` | `"Abramo e Isacco al Monte Moria"` |
| **2** | Tema/keyword spelling | `"Fedeltà"` (theme) | `"Fedeltà"` / `"Fedelta'"` (ASCII apostrophe in C#) |
| **1** | Citazione | Short theater quote (`Genesi 2:17` snippet) | Full TNM `scriptureQuote` multi-sentence |
| **All** | Q&A bundle | 2 MCQ × 18 episodi inline (~500 lines) | Absent |
| **All** | EN immersive | Full parallel in `STORIES` | Absent in `stories.js`; desktop EN via translation engine, not episode copy |

### 2.3 Runtime coupling today

```
stories.js ──► window.JW_STORIES ──► REBUS_LOOKUP ──► Rebus 3D textures
                     ▲
                     │ (same ids/titles/themes intended)
index.html STORIES ──┴──► theater intro/quiz/moral (independent array)
StoryLibrary.cs ───────► desktop DynamicStoryForm (rebus fields)
```

**G1:** two authoring surfaces for metadata (title/theme/scripture) + immersive content only in `index.html`.

---

## 3. Step 2 — Unification options

### Option A — `stories.js` as single hand-edited source

**Idea:** Extend each object in `webapp/stories.js` with immersive blocks (`titleEn`, `themeEn`, `intro`, `questions`, `moral`, `quote`, optional `symbols`). `index.html` removes inline `STORIES` array; after `<script src="stories.js">`, an adapter builds `const STORIES = JW_STORIES.map(toImmersiveStory)`. `StoryLibrary.cs` updated manually or via one-off script when rebus fields change.

| Pro | Contro |
|-----|--------|
| No new canonical file; one web file for agents to edit | **Two targets** remain if C# still manual (drift returns) |
| Minimal pipeline change | Large monolithic JS; hard to validate JSON schema |
| Fastest path to drop duplicate Q&A in HTML | Poor fit for `story-i18n.js` `translations` pattern |

| Impact | Rating |
|--------|--------|
| Theater | Low — adapter only |
| Desktop | **High drift risk** unless C# sync disciplined |
| `sync_all.py` | Unchanged |
| Regression risk | Medium |
| Effort | **S** (3–5 days) |

---

### Option B — Canonical `data/episodes.json` + Python generators (recommended)

**Idea:** One UTF-8 JSON file (18 episodes) with nested structure:

```json
{
  "id": 1,
  "scriptureReference": "Genesi 2-3",
  "rebus": {
    "titleIt": "...",
    "keywordIt": "...",
    "hintIt": "...",
    "solutionIt": "...",
    "scriptureQuoteIt": "...",
    "engagementNoteIt": "...",
    "visibleKeys": ["..."],
    "hiddenKeys": ["..."],
    "hintKey": "...",
    "imageCaptionsIt": ["×8"]
  },
  "immersive": {
    "titleEn": "...",
    "themeIt": "...",
    "themeEn": "...",
    "intro": { "it": "...", "en": "..." },
    "questions": [ { "prompt": { "it", "en" }, "answers": [...] } ],
    "moral": { "it", "en" },
    "theaterQuote": { "it", "en" },
    "symbols": ["optional decorative — not PNG keys"]
  }
}
```

Generators (new `tools/generate_story_artifacts.py`):

1. **`webapp/stories.js`** — `window.JW_STORIES = [...]` (rebus + optional embedded immersive for adapter, or separate `window.JW_IMMERSIVE`).
2. **`StoryLibrary.cs`** — generated region `#region GeneratedStoryCatalog` (or whole file) from same JSON.
3. Optional: **`webapp/stories.meta.json`** for validation only (not loaded in prod).

`sync_all.py` invokes generator before `sync_android_www.py`.

| Pro | Contro |
|-----|--------|
| **Single edit point**; G1 closed structurally | Up-front schema + migration effort |
| Validates with Python (keys exist in assets, 8 captions, 2 questions) | Generated C# must be committed or generated in CI (repo chooses commit generated output) |
| Aligns with KB pipeline philosophy | First migration needs careful merge of index Q&A + stories.js |
| Can emit alignment test: JSON vs Resources PNG keys | New tool to maintain |

| Impact | Rating |
|--------|--------|
| Theater | Low — consume generated data |
| Desktop | **Low** — same generator |
| `sync_all.py` | **+1 step** (document in `tools/README.md`) |
| Regression risk | Low–medium (with validator) |
| Effort | **M** (5–8 days) |

---

### Option C — Runtime merge (`stories.js` + `stories-immersive.js`)

**Idea:** Keep rebus in `stories.js`; move Q&A to `stories-immersive.js` (or JSON fetched). At load, `mergeById(JW_STORIES, JW_IMMERSIVE)`. `StoryLibrary.cs` stays separate; add `tools/verify_story_alignment.py` failing CI if id/title/theme/scripture diverge.

| Pro | Contro |
|-----|--------|
| Smallest change to rebus file | **G1 half-fixed** — still 2–3 sources |
| Clear separation rebus vs quiz copy | Merge bugs at runtime; order of script tags |
| Good incremental step | Desktop still manual duplicate |

| Impact | Rating |
|--------|--------|
| Theater | Medium (merge logic) |
| Desktop | Unchanged drift |
| `sync_all.py` | Optional copy only |
| Regression risk | Medium |
| Effort | **S–M** |

---

### Option D (variant) — Extend `story-i18n.js` schema only

**Idea:** Store full episode in shared `{ sourceLanguage, translations: { Italian, English } }` including `questions` arrays; generators emit three targets. Heavier schema design; best long-term for classic + immersive + API.

Consider as **phase 2** of Option B, not separate first ship.

---

## 4. Step 3 — Recommendation

**Choose Option B** (canonical JSON + Python generators), with a **thin adapter** in `index.html` (build phase) that replaces the inline `STORIES` literal.

**Why not A alone:** desktop `StoryLibrary.cs` will drift again without generation.  
**Why not C alone:** leaves G1 partially open; merge is harder to test than generate.  
**Why B fits repo:** existing `tools/sync_all.py`, no npm, net472 safe, `story-i18n.js` can later consume the same JSON for user-story parity.

**Reuse `story-i18n.js`:** generator outputs rebus text compatible with `ensureStoryTranslations()` for classic/API; immersive MCQ stays in JSON fields not needed by classic flat rebus UI.

---

## 5. Rollout plan (build phase — after explicit OK)

| Step | Action | Verification (must stay green) | Rollback |
|------|--------|--------------------------------|----------|
| **0** | Define JSON schema + `tools/validate_episodes.py` | Validator passes on empty/minimal fixture | Delete tool |
| **1** | Migrate content: merge `stories.js` + `STORIES` block → `data/episodes.json` | Validator: 18 ids, PNG keys, 2 questions, it+en | Revert JSON |
| **2** | Implement `generate_story_artifacts.py` → `stories.js` + `StoryLibrary.cs` | Diff only generated sections; MSBuild Debug **0** | Revert generator output |
| **3** | Wire `sync_all.py` to run generator | `python tools/sync_all.py` idempotent | Revert sync_all one line |
| **4** | `index.html`: remove inline `STORIES`; adapter from `JW_STORIES` / companion global | Preview: 18 tiles, 3 modes, IT/EN, rebus Continua gated | Restore inline array (git) |
| **5** | Remove duplicate manual edits; add `tools/verify_episode_parity.py` in agent checklist | Prompt H subset + desktop smoke one episode | Revert step 4–5 |

**Checkpoints after steps 2, 4, 5:**

- 18 episodi apribili in theater (quiz + rebus + journey)
- IT/EN switch updates grid + theater
- Anti-spoiler: tile shows guessStory + theme, not title
- Rebus 3D loads textures from keys
- `MSBuild Jw_Quiz_Development.csproj /p:Configuration=Debug` exit 0 if C# touched
- `python tools/sync_all.py` after any `webapp/` player change

**Scripture / doctrinal text:** migration copies existing strings only; **no wording edits** without human approval (AGENTS.md stop rule).

---

## 6. Build-phase commit policy (proposed)

| Commit | When |
|--------|------|
| `docs(kb): design unificazione dataset` | After design OK (this doc + KB) |
| `feat(tools): schema episodi + generator StoryLibrary/stories.js` | Step 0–2 |
| `refactor(dataset): index.html consume generated STORIES` | Step 4 |
| `refactor(dataset): allinea StoryLibrary a source unica` | If split from generator commit |
| `kb: dataset unificato G1 chiuso` | Final KB §10/§11/§17 |

One logical commit per step; never squash; no deploy in Prompt C.

---

## 7. Open questions for human before build

1. **Generated C#:** commit generated `StoryLibrary.cs` to git (recommended for Wrangler/desktop parity without build-time Python) vs generate in pre-commit hook?
2. **Theater `symbols`:** keep Unicode emoji for intro only, or replace with PNG keys / remove?
3. **Ep 8 title:** standardize on `"Abramo e Isacco al Monte Moria"` everywhere?
4. **Theater quote vs `scriptureQuote`:** keep two tiers (short theater + long rebus) explicitly in schema?

---

## 8. References

- `docs/AGENTS.md` — where to edit episodes today (split across 3 files)
- `docs/ARCHITECTURE.md` — source of truth table
- `.github/KB.md` §11 G1, §13 drift row
- `webapp/story-i18n.js` — shared translation normalization for API/classic
