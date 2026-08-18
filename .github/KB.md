# JW Quiz ÔÇö Knowledge Base

> **AGGIORNATO AUTOMATICAMENTE** dall'agente `jw-quiz-dev` ad ogni decisione, modifica, o proposta.  
> **REGOLA**: Prima di ogni lavoro leggi questa KB. Alla fine di ogni sessione o decisione, aggiorna questa KB.

---

## 1. Stack Tecnico

| Voce | Valore |
|------|--------|
| Linguaggio | C# |
| Framework UI | Windows Forms (WinForms) |
| Runtime | .NET Framework 4.7.2 |
| csproj stile | **SDK-style** (`Microsoft.NET.Sdk.WindowsDesktop`, `net472`, `UseWindowsForms: true`) |
| Build command | `.\build.bat` oppure `MSBuild Jw_Quiz_Development.csproj /p:Configuration=Debug` |
| Output | `bin\Debug\Jw_Quiz_Development.exe` |
| Controllo versione | Git ÔÇö branch `main` |
| Lingua UI | Italiano + English (motore multilanguage desktop + web shared + immersive.html it/en) |
| Web Immersive | `webapp/immersive.html` — Three.js 0.160 CDN (import map), fallback 2D, theater Q&A |

---

## 2. Architettura dei File Chiave

| File | Ruolo |
|------|-------|
| `Program.cs` | Entry point ÔÇö avvia `Form1` |
| `Form1.cs/Designer.cs` | Menu principale + navigazione |
| `Forms_list.cs` | Router di navigazione tra form |
| `FINE.cs` | Schermata finale |
| `Story.cs` | Modello dati storia (VisibleEmojis, HiddenEmojis, HintEmoji come chiavi risorsa PNG) |
| `StoryLibrary.cs` | Catalogo storie (ID 1ÔÇô18): tutte renderizzate dal runtime dinamico |
| `StoryEngine.cs` | Logica progressione/navigazione tra storie |
| `DynamicStoryForm.cs` | Form generico runtime per tutte le storie built-in (ID 1ÔÇô18) e user-created |
| `StoryEditorForm.cs` | Editor in-app per creare nuove storie con galleria immagini |
| `UserStoryLibrary.cs` | Persistenza storie utente su `UserStories.dat` |
| `LanguageManager.cs` | Stato lingua corrente app + persistenza preferenza lingua |
| `AppText.cs` | Catalogo centralizzato testi UI desktop it/en |
| `StoryLocalizationService.cs` | Risoluzione testi localizzati per ogni storia + cache traduzioni |
| `StoryTranslationEngine.cs` | Traduttore rule-based it/en, sostituibile con provider futuro |
| `ProgressTracker.cs` | XP, badge, persistenza su `UserProgress.dat` |
| `ProgressPanel.cs` | UI pannello progresso |
| `Resources/` | PNG rebus (ora pack **fotorealistico** sync da `tools/photo_masters`) |
| `tools/photo_concepts.py` | Mappa concept→prompt→alias chiavi rebus |
| `tools/photo_masters/` | Master PNG fotorealistici (73 concept) |
| `tools/apply_photo_assets.py` | Applica masters → Resources + webapp/assets (+ android) |
| `tools/sync_android_www.py` | Copia `webapp/` → `android/.../assets/www` |
| `android/` | App Android WebView (shell immersiva offline-capable) |
| `.cursor/mcp.json` | MCP Cloudflare (docs/bindings/builds/observability) |
| `Properties/Resources.Designer.cs` | Accesso fortemente tipizzato alle risorse |
| `webapp/index.html` | **Entry principale** — esperienza immersiva (3 modalità: Quiz / Rebus 3D / Avventura), Three.js, audio |
| `webapp/immersive.html` | Redirect → `/` (alias legacy bookmark) |
| `webapp/classic.html` | Editor episodi + admin analytics + rebus flat legacy (mantenuto per tooling) |
| `webapp/app.js` | Logica gameplay web classic, fallback locale, integrazione API Cloudflare |
| `webapp/story-i18n.js` | Motore shared i18n web/API: UI text, lingua corrente, auto-traduzione it/en e normalizzazione `sourceLanguage/translations` |
| `webapp/assets.js` | Manifest JS delle chiavi PNG disponibili nel picker immagini web |
| `webapp/stories.js` | Dataset storie dinamiche per la versione web (rebus classico) |
| `webapp/styles.css` | Tema visuale web responsive |
| `webapp/assets/*.png` | Asset PNG deployati per gioco e editor web |
| `functions/api/stories.js` | API Cloudflare Pages Functions per leggere/salvare episodi condivisi |
| `functions/api/assets.js` | API Cloudflare Pages Functions per listare/caricare PNG custom condivisi |
| `functions/api/assets/[key].js` | Stream dei PNG custom salvati su bucket R2 |
| `wrangler.toml` | Config deploy Cloudflare (output webapp, compat date, vars) |

---

## 3. Gameplay ÔÇö Meccanica Rebus

**Runtime Unificato (DynamicStoryForm)**
- 8 `PictureBox` (slot 0ÔÇô4 visibili, 5ÔÇô6 nascosti, 7 indizio)
- Slot 5ÔÇô6 mostrano `2753.png` (ÔØô) finch├® non rivelati
- Slot 7 mostra `1F525.png` (­ƒöÑ) con animazione pulsante ambra (Timer 300ms) finch├® indizio non cliccato
- Didascalia click immagini: label dedicata ad alto contrasto **in alto al pannello** (non sovrapposta alle immagini)
- Didascalie click immagini: policy **anti-spoiler** attiva su desktop e web; se una descrizione cita titolo, personaggi o dettagli troppo rivelatori, viene sostituita da una formulazione neutra
- Testi storia e chrome del form dinamico localizzati via `StoryLocalizationService` + `AppText`, con lingua runtime it/en
- Pulsante "Rivela 2 immagini": rivela slot 5, poi 6 (secondo click)
- XP base 100, -20 per ogni aiuto usato (minimo 20)
- Header: titolo e riferimento biblico **NASCOSTI** fino a "Rivela soluzione"
- Header mostra solo: `"Episodio X ÔÇö Indovina la storia!"` + categoria/keyword
- Completamento storia registrato via `ProgressTracker.Instance.CompleteStory(storyId)` alla chiusura

---

## 4. Catalogo Storie

| ID | Titolo | Tema | Tipo |
|----|--------|------|------|
| 1 | Il Giardino di Eden | Obbedienza | Dinamica |
| 2 | Sansone e Dalila | Fedelta' | Dinamica |
| 3 | Giona e il Pesce | Misericordia | Dinamica |
| 4 | Le Pecore e le Capre | Giudizio | Dinamica |
| 5 | Le 10 Piaghe d'Egitto | Potere di Dio | Dinamica |
| 6 | Elia e la Siccita' | Preghiera | Dinamica |
| 7 | Ester Salva il Popolo | Coraggio | Dinamica |
| 8 | Abramo e Isacco | Fede | Dinamica |
| 9 | Il Figlio Prodigo | Perdono | Dinamica |
| 10 | La Profezia di Isaia | Profezia | Dinamica |
| 11 | Noe' e il Diluvio | Salvezza | Dinamica |
| 12 | Filippo e l'Eunuco | Buona Novella | Dinamica |
| 13 | Davide e Golia | Coraggio | Dinamica |
| 14 | Giuseppe Perdona i Fratelli | Perdono | Dinamica |
| 15 | Rut e Boaz | Devozione | Dinamica |
| 16 | La Nascita di Mose' | Protezione | Dinamica |
| 17 | Anna e Samuele | Preghiera | Dinamica |
| 18 | Il Buon Samaritano | Amore per il Prossimo | Dinamica |

---

## 5. Immagini Risorse ÔÇö Chiavi Disponibili

Le chiavi risorsa corrispondono ai nomi file PNG in `Resources/` senza estensione.  
Esempi di chiavi PNG particolarmente espressive per storie bibliche:

| Chiave | Descrizione |
|--------|-------------|
| `038-boy-1` | Bambino/ragazzo |
| `039-baby` | Neonato |
| `036-man-1` | Uomo adulto |
| `031-man-2` | Uomo alternativo |
| `094-user` | Persona generica |
| `093-users` | Gruppo persone |
| `1F411` | Pecora |
| `1F410` | Capra |
| `1F413` | Gallo |
| `1F416` | Maiale |
| `1F42A` | Cammello |
| `1F431` | Gatto |
| `1F438` | Rana |
| `1F40D` | Serpente |
| `1F333` | Albero |
| `1F334` | Palma |
| `1F30A` | Onda/Oceano |
| `1F327` | Pioggia |
| `2601` | Nuvola |
| `2614` | Ombrello pioggia |
| `1F525` | Fuoco ­ƒöÑ |
| `1F52A` | Coltello |
| `2694` | Spade incrociate |
| `1F3F0` | Castello |
| `1F451` | Corona |
| `1F4B0` | Sacchetto denaro |
| `1F4D6` | Libro |
| `1F4E3` | Megafono |
| `1F3B6` | Musica |
| `1F498` | Cuore |
| `1F629` | Stanco/dispiaciuto |
| `1F632` | Sorpresa |
| `1F634` | Addormentato |
| `1F480` | Teschio |
| `1F440` | Occhi |
| `1F4AA-1F3FD` | Braccio forte |
| `1F932-1F3FC` | Mani in preghiera |
| `1F47C` | Angioletto |
| `26D4` | Divieto/Stop |
| `2753` | Punto interrogativo (ÔØô placeholder nascosto) |
| `203C` | Doppio esclamativo (ÔÇ╝ enfasi) |
| `Hackney-100` | Cavallo |
| `1F3DB` | Tempio/Colonne |
| `1F5FA` | Mappa/Percorso |
| `1F6BC` | Carrozzina |
| `1F6A2` | Nave |
| `26F5` | Barca a vela |
| `1F6B6-200D-2640-FE0F` | Donna che cammina |
| `1F6B6-1F3FF-200D-2642-FE0F` | Uomo che cammina |
| `1F468-1F3FB-200D-1F33E` | Contadino |
| `1F468-1F3FB-200D-1F3EB` | Insegnante |
| `1F468-1F3FD-200D-1F527` | Operaio |
| `1F468-1F3FD-200D-2696-FE0F` | Giudice |

---

## 6. Sistemi di Progressione

- `ProgressTracker.cs`: singleton, carica/salva `UserProgress.dat` (BinaryFormatter)
- Metodo: `CompleteStory(int storyId)` e `CompleteStory(int storyId, int xp)`
- XP totali, storie completate, badge sbloccati
- `ProgressPanel.cs`: UI panel integrato in Form1

---

## 7. Storie Utente (User-Created)

- `UserStoryLibrary.cs`: gestisce `UserStories.dat`
- `StoryEditorForm.cs`: form editor con galleria visiva PNG (`Scegli...` per ogni slot immagine)
- Storie salvate con `IsUserCreated = true`, `IsDynamic = true`
- Persistenza estesa: `ImageCaptions[]` serializzato/deserializzato in `UserStories.dat`
- Persistenza estesa: `SourceLanguage` + campi tradotti `*_en` serializzati in `UserStories.dat` (backward-compatible con file legacy)
- Fallback immagini utente normalizzati su chiavi PNG (`2753`, `1F525`), mai emoji unicode
- ID assegnato incrementalmente oltre 1000
- All'atto di creazione/salvataggio, il motore genera automaticamente la lingua mancante tra italiano e inglese
- Webapp: editor locale in-browser con `localStorage` (`jwquiz_web_user_stories_v1`)
- Webapp: galleria immagini alimentata da `assets.js`, con ricerca per chiave PNG e anteprima per ogni slot
- Webapp: episodi creati lato browser vengono uniti ai 18 built-in nel selettore senza mostrare il titolo
- Webapp: lingua runtime it/en con selettore dedicato; testi gameplay/editor e contenuti storia risolti da `story-i18n.js`
- Webapp/API: ogni storia condivisa salva `sourceLanguage` + `translations.Italian/English`; la lingua mancante viene generata automaticamente lato browser e lato Pages Function
- Cloudflare: dopo il deploy, gli episodi utente vengono salvati via Pages Functions in KV `JWQUIZ_DATA`
- Cloudflare: i PNG custom vengono caricati su bucket R2 `JWQUIZ_UPLOADS` e riutilizzati nel picker come chiavi `custom:<file>.png`
- Fallback locale: se le API Cloudflare non rispondono, la webapp continua a funzionare con persistenza solo browser-side

---

## 8. Convoluzioni/Vincoli Tecnici Noti

- **PlaceholderText** non disponibile in .NET Framework 4.7.2 (solo .NET 5+) ÔÇö non usare
- **ResourceManager.GetObject** con chiave esatta (senza estensione) per caricare PNG
- Caricamento risorse centralizzato in `StoryResources.cs` (evitare accesso diretto duplicato al ResourceManager)
- **BinaryFormatter** deprecato in .NET 5+ ma funziona in net472
- **DockStyle.Fill** deve essere aggiunto per primo (`Controls.Add`) per corretta precedenza z-order
- **Emoji come testo** nei Label: evitare ÔÇö usare PictureBox con PNG da Resources per coerenza visiva
- **Cloudflare shared mode** richiede 2 binding configurati in Pages: KV `JWQUIZ_DATA` e R2 `JWQUIZ_UPLOADS`
- In locale (`python -m http.server`) le Pages Functions non esistono: la webapp va automaticamente in fallback locale
- Webapp multilanguage: il motore JS shared (`webapp/story-i18n.js`) e' la fonte unica per testi UI, auto-traduzione e normalizzazione JSON delle storie condivise
- Motore multilanguage corrente applicato ai flussi data-driven desktop (`Form1`, `StoryEditorForm`, `DynamicStoryForm`, storie utente)
- QA contenuti dinamici: verificare sempre che le chiavi PNG in `StoryLibrary` esistano davvero in `Resources/*.png` per evitare fallback silenzioso su `2753`

---

## 9. Git / Delivery

- Branch: `main`
- Push dopo ogni modifica significativa
- No file non tracciati (`git status --short` deve essere pulito) — Wrangler Pages avvisa se il tree è sporco
- Build deve passare (`MSBuild /verbosity:quiet`) prima di ogni commit
- Commit message in italiano, prefisso convenzionale: `feat:`, `fix:`, `refactor:`, `kb:`
- Cleanup intelligente e handoff sessione: §16 e §17

---

## 10. Decisioni e Valutazioni ÔÇö Log Cronologico

| Data | Decisione / Proposta | Esito |
|------|----------------------|-------|
| 2026-04-22 | Traduzione completa UI in italiano | Ô£à Applicata |
| 2026-04-22 | Progressione XP + badge + ProgressTracker | Ô£à Implementata |
| 2026-04-22 | Audit storie: mismatch StoryLibrary vs form reali | Ô£à Corretta mappatura |
| 2026-04-22 | 6 nuovi episodi dinamici (ID 13ÔÇô18) approvati | Ô£à Implementati |
| 2026-04-22 | DynamicStoryForm con emoji label ÔåÆ PictureBox PNG colorati | Ô£à Migrato |
| 2026-04-22 | Header nascosto (titolo/scripture rivelati solo con soluzione) | Ô£à Implementato |
| 2026-04-22 | Indizio animato (pulsazione ambra su PictureBox slot 7) | Ô£à Implementato |
| 2026-04-22 | StoryEditorForm: galleria visiva con miniature cliccabili | Ô£à Implementata |
| 2026-04-22 | csproj convertito in SDK-style (net472) per C# Dev Kit | Ô£à Applicata |
| 2026-04-22 | StoryLibrary ID 13ÔÇô18: chiavi PNG al posto di caratteri unicode emoji | Ô£à Applicata |
| 2026-04-22 | Story model: aggiunti campi ImageCaptions[] (8 slot) e ScriptureQuote | Ô£à Implementato |
| 2026-04-22 | StoryLibrary ID 13ÔÇô18: ImageCaptions + citazioni NMW (ScriptureQuote) | Ô£à Popolate |
| 2026-04-22 | DynamicStoryForm: click su immagine mostra didascalia descrittiva | Ô£à Implementato |
| 2026-04-22 | DynamicStoryForm: citazione TNM visibile nel pannello soluzione | Ô£à Implementato |
| 2026-04-22 | Screen_size.IsFullscreen: stato persistente tra apertura di nuovi form | Ô£à Implementato |
| 2026-04-22 | Forms_list.ShowForm(): helper centralizzato che ripristina fullscreen + refactoring metodi | Ô£à Implementato |
| 2026-04-22 | Refactor architetturale: loader immagini centralizzato (`StoryResources`) + cleanup chiamate duplicate | Ô£à Implementato |
| 2026-04-22 | DynamicStoryForm: didascalia click immagini resa sempre leggibile (alto contrasto + posizione non sovrapposta) | Ô£à Implementato |
| 2026-04-22 | `ProgressTracker`: persistenza `StoryAttempts` + soglie badge robuste (`>=`) | Ô£à Implementato |
| 2026-04-22 | `UserStoryLibrary`: serializzazione `ImageCaptions[]` + fallback PNG keys (no unicode emoji) | Ô£à Implementato |
| 2026-04-22 | Compatibilit├á legacy preservata: API pubblica `Forms_list` mantenuta a istanza (no regressioni static forms) | Ô£à Validato |
| 2026-04-22 | Web MVP immediato: creata webapp statica self-contained (episodi 13-18) pronta per hosting gratuito | Ô£à Implementato |
| 2026-04-22 | **Webapp completata**: aggiunti episodi 1-12 in `stories.js` (18 episodi totali, 56 PNG mappati) | Ô£à Implementato |
| 2026-04-22 | **UX webapp**: dropdown mostra solo "Episodio X" (no titolo storia per evitare spoiler) | Ô£à Implementato |
| 2026-04-22 | **Assets webapp**: copiati 49 PNG aggiuntivi per episodi 1-12 ÔåÆ 80 PNG totali self-contained | Ô£à Implementato |
| 2026-04-22 | **Editor web episodi**: aggiunta creazione locale nuovi episodi con picker PNG, merge selettore e persistenza `localStorage` | Ô£à Implementato |
| 2026-04-22 | **Asset parity web**: sincronizzati tutti i PNG da `Resources/` a `webapp/assets/` per avere galleria completa come desktop | Ô£à Implementato |
| 2026-04-22 | **Cloudflare shared persistence**: aggiunte Pages Functions per episodi condivisi su KV e asset PNG custom su R2 | Ô£à Implementato |
| 2026-04-22 | **Picker web avanzato**: aggiunto upload PNG custom dal browser con riuso immediato nei nuovi episodi | Ô£à Implementato |
| 2026-04-22 | **Deploy readiness**: aggiunto `wrangler.toml` con naming `jwquiz` per primo deploy | Ô£à Implementato |
| 2026-04-22 | **Hotfix deploy**: rimossi binding KV/R2 da `wrangler.toml` (placeholder ID invalido) per usare i binding reali configurati nel pannello Pages | Ô£à Implementato |
| 2026-04-22 | **Hotfix runtime webapp**: corretto ordine inizializzazione `builtInAssetLookup` in `webapp/app.js` (risolto errore console e lista episodi vuota) | Ô£à Implementato |
| 2026-04-22 | **Content anti-spoiler**: introdotta policy centrale per neutralizzare didascalie immagini troppo esplicite e ripulite le caption piu' scoperte degli episodi dinamici | Ô£à Implementata |
| 2026-04-22 | **Motore multilanguage desktop**: introdotti `LanguageManager`, `AppText`, `StoryLocalizationService` e auto-traduzione it/en per storie dinamiche e storie utente | Ô£à Implementato |
| 2026-04-22 | **Motore multilanguage web shared**: aggiunti selettore lingua web, modulo shared `story-i18n.js` e persistenza `sourceLanguage/translations` nelle Pages Functions | Ô£à Implementato |
| 2026-04-22 | **Valutazione unificazione legacy 1-12**: analisi impatti completata; proposta migrazione in 3 fasi (router -> data -> dismissione form statici) per convergere su `DynamicStoryForm` | ­ƒƒ¿ Proposta pronta (in attesa approvazione) |
| 2026-04-22 | **Unificazione legacy Step 1**: introdotto feature flag `AppFeatureFlags.UseDynamicRendererForLegacyStories` + routing sicuro in `Forms_list` con fallback automatico ai form statici se dati 1-12 non completi | Ô£à Implementato e validato (build 0 regressioni) |
| 2026-04-22 | **Unificazione legacy Step 2**: popolati in `StoryLibrary` i campi dinamici per le storie 1-12 (slot immagini, indizio, didascalie, scripture quote) mantenendo `IsDynamic=false` e flag routing OFF | Ô£à Implementato e validato (build 0 regressioni) |
| 2026-04-22 | **Unificazione legacy Step 3 (rollout)**: attivato `UseDynamicRendererForLegacyStories=true`; le storie 1-12 ora passano dal renderer dinamico quando i controlli dati sono soddisfatti, con rollback immediato possibile via flag | Ô£à Implementato e validato (build 0 regressioni) |
| 2026-04-22 | **QA post-rollout storie 1-12**: corretti 4 riferimenti PNG mancanti in `StoryLibrary` (`japanese_dolls_facebook`, `75-...old_woman...`, `412-...dancing...`, `1F4AC`) con chiavi reali presenti in `Resources` | Ô£à Implementato e validato (build 0 regressioni) |
| 2026-04-22 | **Refactor runtime Form1**: consolidata la navigazione episodi in metodo unico `OpenStory(id)` per ridurre duplicazioni e mantenere invariato il comportamento utente | Ô£à Implementato e validato (build 0 regressioni) |
| 2026-04-22 | **KB recovery post-troncamento**: ripristinato `.github/KB.md` da snapshot stabile precedente e riallineato il log decisionale all'ultimo stato reale della codebase | Ô£à Implementato |
| 2026-04-22 | **Sistema a Stelle (1-3 per storia)**: stelle live nell'header di `DynamicStoryForm` (★★★→★★☆→★☆☆ man mano che si usano aiuti), persistenza in `ProgressTracker.StoryStars`, contatore perfetti in `ProgressPanel`, stelle nel dialog statistiche | ✅ Implementato e validato (build 0 regressioni) |
| 2026-04-22 | **Webapp UX — transizioni e animazioni**: aggiunti `transition` su btn/caption/select/inputs, animazioni `shellIn`/`cardIn`/`riseIn` per shell/editor/picker/solution panel, hover+active feedback su button e slot, focus ring sui form fields | ✅ Implementato |
| 2026-04-22 | **Webapp — stelle live**: aggiunto `starsBox` nel header della webapp con calcolo ★★★ in tempo reale identico al desktop; aggiornato da `updateStarsUi()` ad ogni reveal/hint | ✅ Implementato |
| 2026-04-22 | **Webapp — utenti online + pannello admin stats**: aggiunto badge "online" pulsante nel topbar; `functions/api/analytics.js` (Cloudflare Pages Function) per heartbeat presenza (TTL 120s), conteggio sessioni 24h, eventi story_view/story_complete; pannello admin con login via `ADMIN_SECRET` env var e 4 metriche aggregate (online ora, visioni, completamenti, sessioni) | ✅ Implementato |
| 2026-04-22 | **Cleanup tecnico legacy (step graduale)**: estratta logica hint condivisa in `LegacyHintAnimator` e integrata nei code-behind statici Form2ÔÇôForm12; ridotta duplicazione handler in Form2/Form3 mantenendo intatto il wiring Designer | ✅ Implementato |
| 2026-04-22 | **Pilot UX episodi 1ÔÇô12 (checklist regressioni reveal/hint/soluzione)**: audit tecnico completato su Form2ÔÇôForm13 (no regressioni su reveal immagini, toggle soluzione e tracking `CompleteStory`; hint animato verificato su Form2ÔÇôForm12). Nota: Form13 non possiede `pictureBox8`/slot hint legacy nel Designer, quindi escluso dal controllo di pulsazione | ✅ Validato |
| 2026-04-22 | **Unificazione architettura runtime legacy completata**: `Forms_list` ora instrada le storie 1ÔÇô12 direttamente su `DynamicStoryForm`, rimosso fallback statico dal path principale e dismesso `AppFeatureFlags` di rollout | ✅ Implementato e validato (build 0 regressioni) |
| 2026-04-22 | **Fix webapp pannello admin**: risolto overlay visibile all'avvio (`[hidden]` ora rispettato), corretto flusso apertura solo su click del pulsante "Login" in alto a destra, chiusura robusta con bottone/overlay/Escape | ✅ Implementato |
| 2026-04-22 | **Deep cleanup legacy completato**: rimossi definitivamente i file `Form2`ÔÇô`Form13` (code-behind, Designer, resx) e `LegacyHintAnimator`; validato che il runtime resta interamente su `DynamicStoryForm` | ✅ Implementato e validato (build 0 regressioni) |
| 2026-07-19 | **Executive plan Immersive 3D**: landing SaaS + hero particelle Three.js + theater Q&A animato per tutte le 18 storie; file singolo CDN-only; i18n IT full + EN; zero regressioni sul rebus web/desktop | ✅ Validato e implementato |
| 2026-07-19 | **No-regression boundary**: `webapp/immersive.html` è additivo; `index.html` solo link + chiavi i18n; nessun tocco a `app.js` / gameplay rebus / WinForms | ✅ Validato |
| 2026-07-19 | **Mobile fallback 3D**: WebGL disabilitato su touch stretto / reduced-motion / low memory / failure → canvas 2D leggero 60fps-friendly | ✅ Implementato |
| 2026-07-19 | **Rebus 3D immersivo**: theater esteso a Intro → Rebus 3D (PNG fluttuanti Three.js + raycast) → Quiz → Morale; riusa `stories.js` + `assets/*.png`; fallback CSS cards; anti-spoiler titolo fino a soluzione; Continua gated da reveal soluzione | ✅ Implementato |
| 2026-07-21 | **3 modalità selezionabili**: `quiz` / `rebus` / `journey` (persistenza localStorage); entry point principale = `index.html` immersivo; editor/admin spostato in `classic.html` | ✅ Implementato |
| 2026-07-21 | **Audio + resa immagini**: Web Audio SFX/ambient; plate 3D più grandi + CanvasTexture upscale 512px; backdrop `theater-atmosphere.png`; fix 404 favicon (`favicon.svg`) | ✅ Implementato |
| 2026-07-21 | **Pack fotorealistico**: 73 concept masters → alias su 87 chiavi rebus; applicato a `Resources/`, `webapp/assets/`, Android `assets/www`; slot desktop 150×150 | ✅ Implementato |
| 2026-07-21 | **Android shell**: progetto WebView `android/` + `tools/sync_android_www.py` per pacchettizzare l’esperienza immersiva | ✅ Implementato |
| 2026-07-21 | **Cloudflare Agent Setup**: skills globali `cloudflare/skills` + MCP in `.cursor/mcp.json` (cloudflare, docs, bindings, builds, observability) | ✅ Configurato (richiede restart agent) |
| 2026-08-18 | **Prodotto unico**: docs ARCHITECTURE/AGENTS; pipeline `tools/sync_all.py`; Android www e photo_masters gitignored; HUD progresso web; loop pedagogico curiosità→sfida→recupero→senso; ponte desktop→web; 3 modalità restano selezionabili | ✅ Implementato |
| 2026-08-18 | **Fix Wrangler dirty tree**: `sync_android_www.py` riscrive `www/.gitkeep` dopo rmtree; 0 regressioni gameplay; KB troubles + cleanup + session handoff | ✅ Implementato |
---

## 11. Next Best Decisions (Proposte Attive)

Aggiornare questa sezione ad ogni sessione di lavoro.

| Priorità | Area | Proposta |
|---------|------|---------|
| Alta | Web Immersive | ~~Landing 3D + theater Q&A episodi 1–18 (single HTML CDN)~~ ✅ **COMPLETATO** (`webapp/immersive.html`) |
| Alta | Multilanguage | Rifinire QA linguistico delle storie 1-12 ora renderizzate nel runtime dinamico |
| Alta | Webapp | Configurare `ADMIN_SECRET` nelle env var di Cloudflare Pages → Settings → Environment Variables per attivare il pannello admin statistiche |
| Alta | Immersive | ~~Pack fotorealistico rebus (concept→alias) + sync web/desktop/Android~~ ✅ **COMPLETATO** |
| Alta | Docs | ~~Unificare documentazione human/agent + pipeline sync~~ ✅ **COMPLETATO** (`docs/`, `tools/sync_all.py`) |
| Alta | Android | Aprire `android/` in Android Studio **dopo** `python tools/sync_all.py`, generare Gradle Wrapper, smoke test APK |
| Alta | Cloudflare | Dopo restart agent: OAuth MCP Cloudflare al primo tool use; deploy Pages con wrangler |
| Media | Immersive | Unificare dataset Q&A immersivo con `stories.js` (source unica) — oggi Q&A vive in `webapp/index.html` |
| Media | Multilanguage | Rifinire il glossario rule-based it/en del motore shared web/desktop con review manuale delle traduzioni bibliche piu' lunghe |
| Media | Immersive | Aggiungere FR/ES come terze lingue riusando lo stesso schema `{ it, en, … }` |
| Media | Gamification | **Streak + Badge**: N storie consecutive senza hint = badge "Saggio/Profeta/Apostolo" |
| Media | Gamification | **Classifica sessione locale**: 2-8 partecipanti inseriscono nome, XP aggregati, classifica finale |
| Media | Gamification | **Percorsi Tematici**: raccolte storie per tema (Fede/Amore/Coraggio) con barra progresso sbloccabile |
| Media | UX | ProgressPanel: aggiungere grafico barre XP e lista storie completate |
| Media | Content | Aggiungere storia ID 19+ (es. La Torre di Babele, Marta e Maria, Saul → Paolo) anche nel theater immersivo |
| Bassa | Gamification | **Timer di indovinamento**: 60s opzionale, bonus XP se risposta entro scadenza |
| Bassa | Gamification | **Modalità Riflessione**: dopo soluzione, domanda aperta da leggere al gruppo |
| Bassa | Gamification | **Storia del Giorno**: selezione automatica basata sulla data del calendario |
| Bassa | Tecnica | Sostituire BinaryFormatter con `System.Text.Json` + file JSON per persistenza |
| Bassa | UX grafiche | Hero image per ogni storia (immagine panoramica nell'header) |
| Bassa | Distribuzione | Build script Release + copia automatica in cartella distribuzione |

---

## 15. Prodotto organico (2026-08-18)

Un solo loop di apprendimento su tre superfici:

- **Web** `index.html` = giocatore (Quiz / Rebus 3D / Avventura)
- **Web** `classic.html` = editor/admin
- **Desktop** = rebus proiezione + menu “Apri JW Quiz Web”
- **Android** = WebView del webapp (bundle generato)

Comando unico: `python tools/sync_all.py`. Dettaglio: `docs/ARCHITECTURE.md`.


### Obiettivo
Offrire una landing SaaS immersiva (Three.js/WebGL) e un “theater” videogioco educativo Q&A ispirato a [Fai vivere il racconto!](https://www.jw.org/it/cosa-dice-la-Bibbia/ragazzi/fai-vivere-il-racconto/), senza regressioni sul rebus classico (web + WinForms).

### Decisioni architetturali (valutate)

| Opzione | Pro | Contro | Esito |
|---------|-----|--------|-------|
| A. Sostituire `index.html` con single-file 3D | Un solo entry | Rompe editor, API, rebus, analytics | ❌ Scartata (regressione) |
| B. File additivo `immersive.html` CDN-only + link da rebus | Zero regressioni; deploy Cloudflare automatico (`pages_build_output_dir=webapp`) | Duplicazione contenuti Q&A vs `stories.js` | ✅ **Scelta** |
| C. Modulo npm/bundler Three.js | Tree-shake, TS | Fuori dal vincolo “CDN only / single HTML” | ❌ Fuori scope richiesta |

### Flusso utente (no spoiler sul rebus)
1. Landing hero 3D (particelle + solidi fluttuanti, parallax mouse)
2. Scroll reveal “Come funziona” + griglia episodi 1–18
3. Theater: **Intro → Rebus 3D (reveal/hint/soluzione) → Quiz MCQ → Insegnamento morale**
4. Rebus 3D carica chiavi PNG da `window.JW_STORIES` (`stories.js`) e texture da `assets/<key>.png`
5. CTA “Apri rebus classico” → `index.html` (gameplay esistente invariato)
6. Selettore lingua IT/EN (persistenza `localStorage` chiave `jwquiz_immersive_lang_v1`)

### Performance (target 60fps)
- DPR capped a 1.75; antialias off; `powerPreference: high-performance`
- Particle count adattivo (~450 mobile-tall / ~900 desktop)
- Un solo `requestAnimationFrame`; lerp mouse; `will-change` solo su simboli theater
- Fallback 2D se: `prefers-reduced-motion`, touch+viewport stretto, `deviceMemory<=4`, WebGL assente/fail

### Multilanguage
- IT = source of truth (UI + tutte le 18 storie, 2 domande ciascuna, morale, citazione)
- EN = traduzione completa parallela nello stesso file
- Rebus web: nuove chiavi `ImmersiveExperienceButton` / `ImmersiveExperienceTitle` in `story-i18n.js`

### Customization map (commenti sezione in `immersive.html`)
1. Theme tokens CSS (`:root`)
2. I18N `UI.it` / `UI.en`
3. Story data `STORIES` (Q&A)
4. Three.js hero init / particle counts
5. Theater game loop
6. Fallback 2D

### Disclaimer contenuti
L’esperienza è **ispirata** allo stile didattico JW.org “Fai vivere il racconto!” (immergersi, riflettere, imparare). Non è un prodotto ufficiale Watch Tower. Link ufficiale in footer.

---

## 13. Troubles & Solutions — Immersive / Web 3D (manutenibilità human+agent)

| Trouble | Sintomo | Soluzione / Guardrail |
|---------|---------|------------------------|
| Regressione rebus | Editor/API/slot break dopo landing 3D | **Mai** fondere gameplay in `immersive.html`; modifiche a `app.js` solo se esplicitamente richieste |
| WebGL crash mobile | Schermo nero / tab kill | `preferFallback()` + try/catch su `initThree()` → canvas 2D |
| 60fps drop | Frame stutter su laptop integrati | Ridurre `COUNT` particelle; abbassare DPR; disabilitare solids su low-end (estensione futura) |
| Import map CDN down | Console: failed to resolve `three` | Pin `three@0.160.0` su unpkg; **dynamic `import("three")`** dentro try/catch così theater/i18n restano usabili anche se CDN/WebGL falliscono |
| Spoiler titoli in rebus | Titolo storia in dropdown | Immersive può mostrare titoli (modalità diversa); rebus resta anti-spoiler |
| Drift contenuti 1–18 | Q&A immersivo ≠ testi rebus | Documentare in Next Best: unificare dataset; finché single-file, sync manuale ID/titolo/tema |
| Encoding KB | Caratteri `ÔÇö` / mojibake in KB | Scrivere nuovi aggiornamenti in UTF-8 puro; evitare copy da terminal legacy |
| i18n link rebus | Bottone “Esperienza 3D” non traduce | Usare `data-i18n` + chiavi in `WEB_TEXT` Italian/English |
| Accessibilità motion | Vertigini / reduced motion | Media query `prefers-reduced-motion` + fallback 2D |
| Deploy path | File non online | Deve stare sotto `webapp/` perché `wrangler.toml` → `pages_build_output_dir = "webapp"` |
| Rebus 3D senza texture | Plane bianchi / ❓ | Verificare `stories.js` caricato prima del module e PNG in `webapp/assets/`; onerror → `2753.png` |
| Memory leak theater | Tab rallenta dopo molti episodi | `disposeRebus3D()` su close/prev/next fuori dal rebus: dispose geometry/material/map + cancel rAF |
| Spoiler titolo in intro | Titolo storia prima del rebus | Intro mostra solo `guessStory` + tema; titolo appare in soluzione rebus e atto morale |
| Wrangler `--commit-dirty` | `Warning: git repo has uncommitted changes` | Working tree **deve** essere pulito prima del deploy. Causa tipica: `sync_all.py` cancellava `android/.../www/.gitkeep`. Fix: lo script riscrive `.gitkeep` dopo il copy. **Non** usare `--commit-dirty=true` come scusa per lasciare sporco il repo |
| `sync_all.py` File not found | lanciato da `android/` | Sempre dal root: `cd D:\Jw_Quiz_Development` poi `python tools/sync_all.py` |
| `npx wrangler` Unknown arguments | due comandi incollati sulla stessa riga | Un comando per volta: `npx wrangler pages deploy webapp --project-name=jwquiz` |
| Wrangler “Unknown arguments: wrangler, pages…” | `npx wrangler` invocato dopo un wrangler già globale/ambiguo | Dal root repo, una sola invocazione; se persiste: `npx --yes wrangler@4.124.0 pages deploy webapp --project-name=jwquiz` |

### Checklist regressione (sessione Immersive)
- [ ] `webapp/app.js` invariato (o solo cambi deliberati)
- [ ] Rebus classic in `classic.html`: reveal/hint/solution/stelle ancora OK
- [ ] 3 modalità selezionabili e indipendenti (`quiz` / `rebus` / `journey`)
- [ ] Theater apre episodi 1–18; Continua bloccato fino a risposta in quiz / soluzione in rebus
- [ ] IT/EN switch aggiorna UI + testi storie
- [ ] Mobile: body ha classe `fallback-3d` oppure WebGL stabile
- [ ] Favicon presente (`favicon.svg`) — evita 404 `/favicon.ico`
- [ ] KB sezioni 10/11/13/16/17 aggiornate

---

## 14. Deploy Cloudflare Pages → https://jwquiz.pages.dev/

### Cosa viene pubblicato
`wrangler.toml` → `pages_build_output_dir = "webapp"`. Quindi `/` serve `webapp/index.html` (immersivo).

| URL | Contenuto |
|-----|-----------|
| `/` | Immersive 3 modalità |
| `/classic.html` | Editor + admin + rebus flat |
| `/assets/*` | PNG + atmosphere |

### Comandi (dal root repo)

```powershell
# Preview locale
cd webapp
python -m http.server 8080

# Deploy (dal ROOT del repo, working tree pulito)
npx wrangler pages deploy webapp --project-name=jwquiz
```

Se Wrangler avvisa `uncommitted changes`: `git status --short` e committare o ripristinare. Non silenziare con `--commit-dirty` salvo generate-only atteso e già gitignored.

### Checklist pre-deploy
1. `webapp/index.html` = esperienza immersiva
2. `webapp/classic.html` raggiungibile per editor
3. `webapp/assets/theater-atmosphere.png` presente (~2 MB)
4. Binding KV `JWQUIZ_DATA` + R2 `JWQUIZ_UPLOADS` già in Pages (per classic editor shared)
5. Opzionale: `ADMIN_SECRET` in Environment Variables

### Post-deploy smoke
- Apri https://jwquiz.pages.dev/ → mode switch Quiz/Rebus/Avventura
- Apri un episodio Rebus 3D → texture PNG caricano
- https://jwquiz.pages.dev/classic.html → editor OK
- Nessun 404 su `/favicon.svg`

---

## 16. Repo cleanup intelligente (generated vs source)

Obiettivo: working tree pulito **senza** cancellare sorgenti e **senza** committare artefatti rigenerabili. Wrangler usa il commit git come metadato del deploy: se `git status --short` non è vuoto, compare `Warning: Your working directory is a git repo and has uncommitted changes`.

### Source of truth (editare / committare)

| Path | Ruolo |
|------|--------|
| `webapp/` | Player (`index.html`) + editor (`classic.html`) + PNG in `webapp/assets/` |
| `Resources/` | PNG desktop WinForms (stesse chiavi del web) |
| `StoryLibrary.cs`, `DynamicStoryForm.cs`, `AppText.cs` | Desktop |
| `functions/` | Cloudflare Pages Functions |
| `android/app/src/main/` eccetto `assets/www/**` | Shell WebView (Manifest, Activity, Gradle) |
| `tools/*.py` | Pipeline sync / photo |
| `.github/KB.md`, `docs/AGENTS.md`, `docs/ARCHITECTURE.md` | Protocollo agent |

### Generated / cache (non committare)

| Path | Come si rigenera / perché ignorato |
|------|-------------------------------------|
| `android/app/src/main/assets/www/**` | `python tools/sync_all.py` dal **root** |
| `android/app/src/main/assets/www/.gitkeep` | **Unica** eccezione tracciata: tiene la cartella in git dopo `rmtree` |
| `tools/photo_masters/*.png` | Master fotorealistici; copie applicate in `Resources/` + `webapp/assets/` |
| `android/.gradle/`, `android/build/`, `android/app/build/` | Cache Gradle |
| `bin/`, `obj/`, `.vs/`, `.vscode/`, `.wrangler/` | Build / IDE / Wrangler local |

### Cosa fare (e non fare) quando il tree è sporco

1. `git status --short` dal root. Classificare ogni riga: sorgente vs generated vs accidentale.
2. **Generated visibile** (`www/app.js`, `www/assets/*`, `.gradle`): non `git add`. Deve coprirlo `.gitignore`. Se compare, il ignore è rotto o il file era già tracked — non aggiungere, sistemare ignore.
3. **`.gitkeep` deleted** dopo sync: non è contenuto perso. `sync_android_www.py` deve riscriverlo **byte-identico** al file tracked. Poi `git checkout -- android/app/src/main/assets/www/.gitkeep` se serve allineare a HEAD.
4. **PNG in `Resources/` / `webapp/assets/`**: sono source applicate (non masters). Committare solo se l’art è voluto in produzione.
5. **Non** `git clean -fd` alla cieca: cancella untracked utili (`android/README.md` nuovo, tool nuovi). Prima `git clean -nd` (dry-run).
6. **Non** `--commit-dirty=true` come default. Serve solo se si deve pubblicare `webapp/` sapendo che il commit git non corrisponde (hotfix). Il warning non è un errore: il deploy può comunque riuscire.
7. Comandi dal **root** `D:\Jw_Quiz_Development`. Mai `python tools/sync_all.py` da `android/`. Mai due comandi incollati sulla stessa riga (`npx wrangler…npx wrangler…` → Unknown arguments).

### Checklist cleanup pre-deploy / pre-commit

- [ ] `git status --short` vuoto, oppure solo file sorgente che stai per committare
- [ ] Nessun `www/**` staged (solo `.gitkeep` se il testo è cambiato di proposito)
- [ ] `python tools/sync_all.py` non ha lasciato `.gitkeep` deleted
- [ ] MSBuild Debug esce 0 se hai toccato C#
- [ ] Tre modalità ancora selezionabili (nessuna fusione Quiz/Rebus/Avventura)

---

## 17. Session handoff (human + agent)

Protocollo per chiudere una sessione e far ripartire la successiva **senza regressioni** e senza ricalcolare da zero.

### All’avvio (obbligo)

1. Leggere `.github/KB.md` (§10 decisioni, §13 troubles, §16 cleanup, questa §17).
2. Leggere `docs/AGENTS.md` (invarianti prodotto) e `docs/ARCHITECTURE.md` se si tocca sync/Android/web.
3. `git status --short` + ultimo `git log -5 --oneline`.
4. Se il task è un follow-up, cercare nel transcript della chat precedente le keyword (`wrangler`, `sync_all`, `gitkeep`, nome file).

### Invarianti da non rompere (regressione = fallimento handoff)

- Tre modalità selezionabili e distinte: `quiz` / `rebus` / `journey` (Avventura).
- Anti-spoiler rebus: niente titolo/scrittura prima della soluzione.
- PNG keys, mai emoji Unicode nei dati storia.
- Italiano source of truth; English dizionario senza chiavi duplicate (`AppText`).
- Desktop resta net472 WinForms.
- Player = `webapp/index.html`; editor = `webapp/classic.html`. Non fondere gameplay in `app.js` se non richiesto.
- Android `www/` è copia: si edita `webapp/`.

### Stop and ask (non decidere in autonomia)

- Testo di versetti / accuratezza dottrinale
- `ADMIN_SECRET`, login Cloudflare, token
- 19ª storia non richiesta
- Git distruttivo (`push --force`, `reset --hard`, `git clean -fd` senza dry-run)

### Come chiudere la sessione (obbligo)

Aggiornare questa KB:

| Sezione | Cosa scrivere |
|---------|----------------|
| §10 log | 1 riga data + decisione + esito |
| §13 troubles | Sintomo nuovo + soluzione se hai sbloccato un errore |
| §16 | Solo se cambia generated vs source o ignore |
| **§17 Stato corrente** (sotto) | Sostituire il blocco con lo stato **vero** a fine sessione |

Commit se il tree deve tornare pulito (deploy Wrangler). Push solo se richiesto esplicitamente.

### Stato corrente (handoff) — 2026-08-18

- **Fatto:** `tools/sync_android_www.py` riscrive `www/.gitkeep` identico al tracked dopo `rmtree`, così `python tools/sync_all.py` non sporca git. Wrangler warning `uncommitted changes` non va silenziato con `--commit-dirty` se la causa è `.gitkeep` deleted.
- **Deploy:** dal root `npx wrangler pages deploy webapp --project-name=jwquiz`. Produzione https://jwquiz.pages.dev/ . Output dir = `webapp`.
- **Non fatto dall’agent (umano):** Android Studio su cartella `android/` dopo sync; `ADMIN_SECRET`; smoke APK.
- **Non toccare a meno di richiesta:** `webapp/app.js` (editor), testi versetti, fusione modalità.
- **Comandi root:** `python tools/sync_all.py` ; MSBuild `Jw_Quiz_Development.csproj` Debug ; deploy Wrangler come sopra (un comando per volta).
- **Git:** branch `main`, remote `https://github.com/Pesach85/Jw_Quiz_Development.git`. Non committare `www/**` (eccetto `.gitkeep`), `.gradle`, `photo_masters/*.png`.
