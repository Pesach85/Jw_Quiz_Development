# JW Quiz — Technical Review Report

**Audience:** revisore tecnico / product owner tecnico  
**Data:** 2026-09-29  
**Repo:** `Jw_Quiz_Development` · branch `main` · live [jwquiz.pages.dev](https://jwquiz.pages.dev/)  
**Scopo:** stato del prodotto, inventario feature FE/BE, rischi, **Next Best Decisions** e **Next Best Prompts** (model **Auto**; **Grok 4.7** solo quando disponibile e utile).

---

## 1. Executive summary

JW Quiz è un prodotto didattico originale (non ufficiale Watch Tower) con tre modalità selezionabili — **Quiz**, **Rebus 3D**, **Avventura** — distribuite su **quattro superfici** allineate sullo stesso catalogo di 18 episodi:

| Superficie | Ruolo | Maturità |
|------------|--------|----------|
| Web immersive (`webapp/index.html`) | Player primario (Three.js + fallback 2D) | Produzione |
| Web classic (`webapp/classic.html`) | Editor episodi + admin analytics | Produzione (ops parziale) |
| Desktop WinForms (.NET 4.7.2) | Proiezione / host + editor locale | Produzione |
| Android WebView | Bundle generato di `webapp/` | Codice pronto; APK non ancora smoke-testato in handoff |

**Verdetto:** architettura coerente e shippata; il debito principale è **duplicazione dataset** (rebus vs Q&A immersivo), **ops Cloudflare** (`ADMIN_SECRET`, smoke Android), e **gamification** ancora a livello proposta.

**Git note (2026-09-29):** `main` locale risulta **behind origin/main di 7 commit** — allineare prima di decisioni di deploy.

---

## 2. Product invariants (non negoziabili)

1. Tre modalità distinte; non fonderle in un unico path forzato.
2. Anti-spoiler rebus: titolo/scrittura nascosti fino alla soluzione; tile = episodio + tema.
3. Chiavi PNG nei dati storia, mai emoji Unicode in slot/hint.
4. IT source of truth; EN parallelo completo.
5. Contenuti didattici originali; niente dump articoli/PDF/artwork JW.org.
6. Desktop resta WinForms **net472**.
7. Player = `index.html`; editor/admin = `classic.html`.
8. `android/.../assets/www` è **generato** via `python tools/sync_all.py` dal root.

Riferimenti: `docs/AGENTS.md`, `docs/ARCHITECTURE.md`, `.github/KB.md`.

---

## 3. Stack

| Layer | Tecnologia |
|-------|------------|
| Desktop | C# · WinForms · .NET Framework 4.7.2 · SDK-style csproj |
| Web player | HTML/CSS/JS monolitico · Three.js 0.160 CDN · Web Audio |
| Web editor | `classic.html` + `app.js` + `story-i18n.js` + `stories.js` |
| Backend | Cloudflare Pages Functions · KV `JWQUIZ_DATA` · R2 `JWQUIZ_UPLOADS` |
| Mobile | Android WebView (offline file:// + CDN Three.js se online) |
| Asset pipeline | Python `tools/sync_all.py` · photo masters → Resources + webapp/assets |

---

## 4. Architettura a 4 superfici

```
                    ┌─────────────────────────────┐
                    │  Catalogo 18 episodi + PNG  │
                    │  StoryLibrary.cs ↔ stories.js│
                    └─────────────┬───────────────┘
          ┌───────────────────────┼───────────────────────┐
          ▼                       ▼                       ▼
   Desktop DynamicStoryForm   Web index.html         classic.html
   + StoryEditor + XP         Quiz/Rebus/Journey     editor + admin
          │                       │                       │
          │                       ▼                       ▼
          │                 Android WebView         Pages Functions
          │                 (www sync)              KV + R2 + analytics
          └──────────── menu → jwquiz.pages.dev ──────────┘
```

**Source of truth**

| Dato | Canonico | Copia / rischio |
|------|----------|-----------------|
| PNG rebus | `Resources/*.png` | `webapp/assets/` via sync |
| Campi rebus | `StoryLibrary.cs` | `webapp/stories.js` (sync manuale) |
| Q&A immersivo | `STORIES` in `index.html` | **Duplicato** vs stories.js |
| Masters foto | `tools/photo_masters/` (gitignored) | apply → Resources/webapp |

---

## 5. Feature inventory — Frontend (Web immersive)

File: `webapp/index.html` (~113 KB) + `stories.js` + assets.

| Feature | Dettaglio |
|---------|-----------|
| Mode switch | `quiz` / `rebus` / `journey` · persistenza `jwquiz_immersive_mode_v1` |
| Landing hero | Particelle/solidi Three.js · parallax · scroll reveal |
| Theater flow | Intro → (Rebus) → (Quiz MCQ) → Morale — dipende dalla modalità |
| Rebus 3D | 8 lastre PNG fluttuanti · raycast · `fitRebusCamera` tavola 4×2 |
| Fallback 2D | reduced-motion / touch stretto / low memory / WebGL fail |
| Stelle | Formula 3→2→1 in base agli aiuti (allineata desktop) |
| HUD progresso | XP + episodi in `localStorage` |
| Audio | SFX/ambient Web Audio · toggle persistente |
| i18n | `UI.it` / `UI.en` + testi storie paralleli |
| Anti-spoiler | Intro senza titolo; titolo in soluzione/morale |
| Performance | DPR cap 1.75 · particelle adattive · dispose geometry su close |

**Non fa:** persistenza cloud progresso giocatore; editor episodi; upload asset.

---

## 6. Feature inventory — Frontend (Web classic / editor)

File: `webapp/classic.html`, `app.js`, `story-i18n.js`, `assets.js`.

| Feature | Dettaglio |
|---------|-----------|
| Gameplay rebus flat | Reveal 2 immagini, hint, soluzione, stelle live |
| Catalogo 1–18 | Da `stories.js` (anti-spoiler nel selettore) |
| Editor episodi | Creazione locale + merge selettore |
| Persistenza locale | `localStorage` `jwquiz_web_user_stories_v1` |
| Shared cloud | POST/GET `/api/stories` → KV |
| Upload PNG custom | POST `/api/assets` → R2 · chiavi `custom:…` |
| i18n shared | `story-i18n.js` (UI + auto-traduzione + normalizzazione JSON) |
| Presence badge | Heartbeat `/api/analytics` |
| Admin panel | Login con `ADMIN_SECRET` · online/views/completions/sessions |

Fallback: se API assenti (preview `http.server`), continua solo browser-side.

---

## 7. Feature inventory — Backend (Cloudflare)

| Endpoint | Metodi | Binding | Funzione |
|----------|--------|---------|----------|
| `/api/stories` | GET, POST | KV | Lista / crea episodi utente (ID > 18) + auto-translations |
| `/api/assets` | GET, POST | KV + R2 | Lista / upload PNG ≤ 2 MB |
| `/api/assets/[key]` | GET | R2 | Stream PNG custom (cache immutable) |
| `/api/analytics` | POST, OPTIONS | KV | Heartbeat (TTL 120s), story_view, story_complete, admin_stats |

**Sicurezza / ops**

- Admin gated da `ADMIN_SECRET` (env Pages) — **da configurare** se non già fatto.
- Analytics events non autenticati (clientId sanitizzato); conteggi aggregati incrementali.
- Validazione chiavi asset e campi obbligatori su POST stories.
- `wrangler.toml`: progetto `jwquiz`, KV + R2 binding, `JWQUIZ_ENV=production`.

**Assente (by design oggi):** auth utente end-user, CRUD delete/update storie, rate limit espliciti, export analytics CSV.

---

## 8. Feature inventory — Desktop (WinForms)

| Modulo | Ruolo |
|--------|-------|
| `Form1` + `Forms_list` | Menu, navigazione, fullscreen, bridge “Apri JW Quiz Web” |
| `DynamicStoryForm` | Runtime unico episodi 1–18 + user (8 slot PNG, reveal, hint animato, stelle) |
| `StoryLibrary` / `StoryEngine` | Catalogo e progressione |
| `StoryEditorForm` + `UserStoryLibrary` | Editor + `UserStories.dat` |
| `ProgressTracker` / `ProgressPanel` | XP, badge, stelle, `UserProgress.dat` |
| `LanguageManager` / `AppText` / `StoryLocalizationService` / `StoryTranslationEngine` | i18n it/en |
| `StoryCaptionPolicy` | Anti-spoiler didascalie |
| `StoryResources` | Loader PNG centralizzato |

Legacy Form2–Form13 **rimossi**; tutto passa da `DynamicStoryForm`.

---

## 9. Feature inventory — Android & tooling

| Pezzo | Stato |
|-------|--------|
| Shell WebView | Presente sotto `android/` |
| Sync | `python tools/sync_all.py` (photo apply + www copy + `.gitkeep`) |
| Offline | `file:///android_asset/www/index.html`; WebGL CDN richiede rete |
| APK smoke | **Pending** (Android Studio + Gradle wrapper) |

Tooling: `photo_concepts.py`, `apply_photo_assets.py`, `sync_android_www.py`, `sync_all.py`.

---

## 10. Catalogo contenuti

18 episodi (Eden → Buon Samaritano). Temi: Obbedienza, Fedeltà, Misericordia, Giudizio, Potere, Preghiera, Coraggio, Fede, Perdono, Profezia, Salvezza, Buona Novella, Devozione, Protezione, Amore per il Prossimo.

Ogni episodio immersivo: intro + 2 MCQ tipiche + morale + citazione.  
Rebus: 5 visibili + 2 nascosti + 1 hint · XP base 100 (−20 per aiuto, min 20).

---

## 11. Rischi e gap tecnici

| ID | Severità | Gap | Impatto |
|----|----------|-----|---------|
| G1 | Alta | Q&A `STORIES` in `index.html` ≠ unica source con `stories.js` | Drift titoli/temi/testi |
| G2 | Alta | `ADMIN_SECRET` / smoke admin non confermati in handoff | Analytics admin inutilizzabile |
| G3 | Alta | Android APK non smoke-testato | Superficie “pronta” non validata |
| G4 | Media | Branch locale behind origin (−7) | Deploy/decisioni su codice non allineato |
| G5 | Media | BinaryFormatter su desktop | Debito migrazione JSON |
| G6 | Media | Traduzioni rule-based lunghe | QA linguistico / dottrinale |
| G7 | Bassa | Analytics senza auth client | Inflazione conteggi possibile |
| G8 | Bassa | Three.js CDN dependency | Offline Android WebGL fragile |

---

## 12. Next Best Decisions (prioritizzate)

### P0 — Ops / allineamento (questa settimana)

| # | Decisione | Perché | Criterio done |
|---|-----------|--------|---------------|
| D1 | `git pull` / allinea `main` a origin | Evitare review su snapshot stale | `git status` sync |
| D2 | Verificare/configurare `ADMIN_SECRET` su Pages | Sblocca admin stats | Login admin OK in prod |
| D3 | Smoke Android dopo `sync_all.py` | Chiude superficie mobile | APK apre 3 modalità |

### P1 — Integrità contenuti (prossimo sprint)

| # | Decisione | Perché | Criterio done |
|---|-----------|--------|---------------|
| D4 | Unificare Q&A immersivo con dataset condiviso (es. `stories.js` o JSON unico importato) | Elimina G1 | Un solo file per titolo/tema/Q&A/rebus keys |
| D5 | QA linguistico episodi 1–12 (IT source + EN review) | Post-rollout dynamic | Checklist 18 episodi firmata |

### P2 — Prodotto (backlog attivo)

| # | Decisione | Note |
|---|-----------|------|
| D6 | Streak + badge (no-hint consecutive) | Gamification media KB |
| D7 | Classifica sessione locale 2–8 giocatori | Uso gruppo/proiezione |
| D8 | Percorsi tematici (Fede/Amore/Coraggio) | Retention |
| D9 | FR/ES riusando schema `{ it, en, … }` | Espansione i18n |
| D10 | Episodio 19+ solo su richiesta umana | Invariante AGENTS |

### P3 — Debito tecnico

| # | Decisione |
|---|-----------|
| D11 | Sostituire BinaryFormatter → JSON |
| D12 | Rate limit / soft auth analytics |
| D13 | ProgressPanel: grafico XP + lista completati |

---

## 13. Policy modelli per i prompt

| Modello | Quando usarlo |
|---------|----------------|
| **Auto** (default) | Implementazione, fix, pipeline, Cloudflare Functions, WinForms net472, sync, refactor mirati, PR/doc piccole |
| **Grok 4.7** (solo se disponibile in Cursor) | Sintesi architetturale multi-file, brainstorming gamification/UX, audit drift contenuti su molti episodi, “revisore” creativo di prompt/NBD — **non** sostituire Auto per patch chirurgiche |

Se Grok 4.7 non è in elenco modelli nella sessione: usare **solo Auto**. Non sostituire con altri modelli a caso.

---

## 14. Next Best Prompts (copy-paste)

### Prompt A — Allineamento repo + smoke deploy readiness  
**Model: Auto**

```
Leggi docs/AGENTS.md, docs/ARCHITECTURE.md e .github/KB.md §1 §14 §16 §17.
Allinea il branch main a origin senza force. Poi: git status --short, verifica wrangler.toml,
e produci una checklist pre-deploy Pages (tree pulito, binding KV/R2, assenza --commit-dirty).
Non fare deploy finché non confermo. Non toccare testi versetti.
```

### Prompt B — Verifica ADMIN_SECRET e pannello admin  
**Model: Auto**

```
Analizza functions/api/analytics.js e il pannello admin in webapp/classic.html + app.js.
Documenta esattamente quali env var servono e lo smoke test post-config (heartbeat + admin_stats).
Non stampare o inventare secret. Se ADMIN_SECRET manca, elenca i passi Cloudflare Pages Settings
senza eseguirli. Aggiorna KB §11 e §17.
```

### Prompt C — Unificazione dataset Q&A + rebus (design + implementazione a fasi)  
**Model: Auto** (implementazione) · se disponibile **Grok 4.7** solo per la fase “design options”

```
[FASE DESIGN — Grok 4.7 se disponibile, altrimenti Auto]
Propuni 2-3 opzioni per unificare STORIES (Q&A in webapp/index.html) con webapp/stories.js
e StoryLibrary.cs, rispettando: 3 modalità, anti-spoiler, IT source, zero bundler npm.
Tabella pro/contro + fase rollout senza regressioni theater.

[FASE BUILD — sempre Auto]
Implementa l’opzione approvata in passi piccoli. Dopo ogni passo: python tools/sync_all.py
se tocchi webapp/. Non fondere editor in index.html. Aggiorna KB §10/§11.
```

### Prompt D — Smoke Android APK  
**Model: Auto**

```
Segui android/README.md e docs/AGENTS.md. Dal root esegui python tools/sync_all.py,
verifica che www/.gitkeep sia presente e git status non mostri www/** sporchi.
Prepara istruzioni Android Studio (aprire android/, Gradle sync, assembleDebug)
e checklist smoke: 3 modalità, fallback 2D, un episodio rebus. Non committare www/**.
```

### Prompt E — QA i18n episodi 1–12  
**Model: Auto** · **Grok 4.7** se disponibile per review qualitativa EN

```
Confronta testi IT/EN di StoryLibrary + story-i18n + UI immersiva per episodi 1–12.
Segnala mismatch, caption anti-spoiler deboli, e glossario rule-based da rifinire.
Non modificare citazioni scritturale senza mia approvazione esplicita. Output: tabella ID → issue → fix proposto.
```

### Prompt F — Streak + badge (feature gamification)  
**Model: Auto** · kickoff design con **Grok 4.7** se disponibile

```
[DESIGN — Grok 4.7 se disponibile]
Specifica streak no-hint e badge Saggio/Profeta/Apostolo per desktop ProgressTracker
e HUD web immersive, con persistenza e senza spezzare la formula stelle 3/2/1.

[BUILD — Auto]
Implementa solo dopo mia OK sulla spec. Desktop net472 compatible. Web: localStorage.
Aggiorna KB §10/§11. Test: complete 3 storie senza hint → badge corretto.
```

### Prompt G — Classifica sessione locale (gruppo)  
**Model: Auto**

```
Progetta e implementa classifica sessione 2–8 nomi con XP aggregati, pensata per proiezione desktop
e opzionale mirror web. Nessun backend cloud. Non fondere le 3 modalità. UI minimale coerente AppText.
```

### Prompt H — Audit regressione Immersive (pre-release)  
**Model: Auto**

```
Esegui la checklist KB §13 Immersive: 3 modalità, anti-spoiler, fitRebusCamera, disposeRebus3D,
IT/EN, fallback-3d, favicon, classic.html editor intatto. Apri preview python -m http.server 8080
in webapp/ e riporta esiti per item. Fix solo bug bloccanti; niente feature nuove.
```

---

## 15. Sequenza consigliata al revisore

1. Approvare **D1–D3** (ops).  
2. Lanciare **Prompt A → B → H** (Auto).  
3. Approvare design **Prompt C** (Grok 4.7 se c’è, else Auto) poi build Auto.  
4. **Prompt D** Android.  
5. Backlog prodotto: **F** poi **G** / percorsi tematici.

---

## 16. Comandi canonici (root repo)

```powershell
# Preview web
cd webapp; python -m http.server 8080

# Sync asset + Android www
python tools/sync_all.py

# Desktop Debug
MSBuild Jw_Quiz_Development.csproj /p:Configuration=Debug

# Deploy Pages (tree pulito)
npx wrangler pages deploy webapp --project-name=jwquiz
```

---

## 17. Riferimenti

- `docs/ARCHITECTURE.md` — architettura prodotto  
- `docs/AGENTS.md` — invarianti agent  
- `.github/KB.md` — decisioni, NBD, troubles, handoff  
- `tools/README.md` — pipeline  
- `android/README.md` — APK  

---

*Report generato per handoff revisore tecnico. Aggiornare questo documento e KB §10/§11 a ogni decisione accettata.*
