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
| Lingua UI | Italiano + English (motore multilanguage desktop + web shared) |

---

## 2. Architettura dei File Chiave

| File | Ruolo |
|------|-------|
| `Program.cs` | Entry point ÔÇö avvia `Form1` |
| `Form1.cs/Designer.cs` | Menu principale + navigazione |
| `Form2.cs`ÔÇô`Form13.cs` | Storie statiche (ID 1ÔÇô12): rebus con PictureBox PNG, 7 immagini, reveal progressivo |
| `Forms_list.cs` | Router di navigazione tra form |
| `FINE.cs` | Schermata finale |
| `Story.cs` | Modello dati storia (VisibleEmojis, HiddenEmojis, HintEmoji come chiavi risorsa PNG) |
| `StoryLibrary.cs` | Catalogo storie (ID 1ÔÇô18): 1ÔÇô12 statiche, 13ÔÇô18 dinamiche |
| `StoryEngine.cs` | Logica progressione/navigazione tra storie |
| `DynamicStoryForm.cs` | Form generico per storie dinamiche (ID 13ÔÇô18 e user-created) |
| `StoryEditorForm.cs` | Editor in-app per creare nuove storie con galleria immagini |
| `UserStoryLibrary.cs` | Persistenza storie utente su `UserStories.dat` |
| `LanguageManager.cs` | Stato lingua corrente app + persistenza preferenza lingua |
| `AppText.cs` | Catalogo centralizzato testi UI desktop it/en |
| `StoryLocalizationService.cs` | Risoluzione testi localizzati per ogni storia + cache traduzioni |
| `StoryTranslationEngine.cs` | Traduttore rule-based it/en, sostituibile con provider futuro |
| `ProgressTracker.cs` | XP, badge, persistenza su `UserProgress.dat` |
| `ProgressPanel.cs` | UI pannello progresso |
| `Resources/` | PNG colorati (~100 file) usati come simboli nel rebus |
| `Properties/Resources.Designer.cs` | Accesso fortemente tipizzato alle risorse |
| `webapp/index.html` | Shell UI web con gameplay + editor locale episodi |
| `webapp/app.js` | Logica gameplay web, fallback locale, integrazione API Cloudflare per storie condivise e PNG custom |
| `webapp/story-i18n.js` | Motore shared i18n web/API: UI text, lingua corrente, auto-traduzione it/en e normalizzazione `sourceLanguage/translations` |
| `webapp/assets.js` | Manifest JS delle chiavi PNG disponibili nel picker immagini web |
| `webapp/stories.js` | Dataset storie dinamiche per la versione web |
| `webapp/styles.css` | Tema visuale web responsive |
| `webapp/assets/*.png` | Asset PNG deployati per gioco e editor web |
| `functions/api/stories.js` | API Cloudflare Pages Functions per leggere/salvare episodi condivisi |
| `functions/api/assets.js` | API Cloudflare Pages Functions per listare/caricare PNG custom condivisi |
| `functions/api/assets/[key].js` | Stream dei PNG custom salvati su bucket R2 |
| `wrangler.toml` | Config deploy Cloudflare (output webapp, compat date, vars) |

---

## 3. Gameplay ÔÇö Meccanica Rebus

**Storie Statiche (Form2ÔÇôForm13)**
- 7 `PictureBox` con PNG colorate dal namespace `Properties.Resources`
- `pictureBox3` e `pictureBox5`: nascosti di default (`Visible=false`), rivelati da "Rivela 2 immagini"
- `pictureBox8`: indizio, nascosto, rivelato da "Rivela indizio"
- `label1`: soluzione testo, nascosta, rivelata da "Rivela soluzione"
- Completamento storia registrato via `ProgressTracker.Instance.CompleteStory(storyId)` alla chiusura
- Fase migrazione F2 completata: le storie 1-12 in `StoryLibrary` ora hanno anche campi dinamici (`VisibleEmojis/HiddenEmojis/HintEmoji/ImageCaptions/ScriptureQuote`) pronti al renderer unificato

**Storie Dinamiche (DynamicStoryForm)**
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

---

## 4. Catalogo Storie

| ID | Titolo | Tema | Tipo |
|----|--------|------|------|
| 1 | Il Giardino di Eden | Obbedienza | Statica (Form2) |
| 2 | Sansone e Dalila | Fedelta' | Statica (Form3) |
| 3 | Giona e il Pesce | Misericordia | Statica (Form4) |
| 4 | Le Pecore e le Capre | Giudizio | Statica (Form5) |
| 5 | Le 10 Piaghe d'Egitto | Potere di Dio | Statica (Form6) |
| 6 | Elia e la Siccita' | Preghiera | Statica (Form7) |
| 7 | Ester Salva il Popolo | Coraggio | Statica (Form8) |
| 8 | Abramo e Isacco | Fede | Statica (Form9) |
| 9 | Il Figlio Prodigo | Perdono | Statica (Form10) |
| 10 | La Profezia di Isaia | Profezia | Statica (Form11) |
| 11 | Noe' e il Diluvio | Salvezza | Statica (Form12) |
| 12 | Filippo e l'Eunuco | Buona Novella | Statica (Form13) |
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
- **Designer.cs** dei form statici: NON modificare via codice ÔÇö modifiche vanno fatte via VS Designer o con grande cautela
- **Emoji come testo** nei Label: evitare ÔÇö usare PictureBox con PNG da Resources per coerenza visiva
- **Cloudflare shared mode** richiede 2 binding configurati in Pages: KV `JWQUIZ_DATA` e R2 `JWQUIZ_UPLOADS`
- In locale (`python -m http.server`) le Pages Functions non esistono: la webapp va automaticamente in fallback locale
- Webapp multilanguage: il motore JS shared (`webapp/story-i18n.js`) e' la fonte unica per testi UI, auto-traduzione e normalizzazione JSON delle storie condivise
- Motore multilanguage corrente applicato ai flussi data-driven desktop (`Form1`, `StoryEditorForm`, `DynamicStoryForm`, storie utente); i form statici legacy 1ÔÇô12 restano candidati a migrazione runtime separata per evitare regressioni nei Designer
- QA migrazione legacy: verificare sempre che le chiavi PNG in `StoryLibrary` esistano davvero in `Resources/*.png` per evitare fallback silenzioso su `2753` durante il rollout dinamico

---

## 9. Git / Delivery

- Branch: `main`
- Push dopo ogni modifica significativa
- No file non tracciati (`git status --short` deve essere pulito)
- Build deve passare (`MSBuild /verbosity:quiet`) prima di ogni commit
- Commit message in italiano, prefisso convenzionale: `feat:`, `fix:`, `refactor:`, `kb:`

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

---

## 11. Next Best Decisions (Proposte Attive)

Aggiornare questa sezione ad ogni sessione di lavoro.

| Priorit├á | Area | Proposta |
|---------|------|---------|
| Alta | Web | ~~Migrare anche storie statiche 1-12 nel renderer web unificato~~ Ô£à **COMPLETATO** |
| Alta | Gamification | **Sistema a Stelle** (1-3 per storia): 3=nessun aiuto, 2=1 aiuto, 1=tutti aiuti ÔÇö visual memorabile per proiezione |
| Alta | Gameplay | Form statici (2ÔÇô13): indizio animato come DynamicStoryForm (pulsazione su pictureBox8) |
| Alta | Content | ~~Aggiungere ImageCaptions[] anche alle storie statiche id 1-12 (attualmente solo ID 13-18)~~ Ô£à **COMPLETATO** |
| Alta | Multilanguage | Migrare anche i form statici legacy 1-12 al runtime multilanguage senza toccare i `Designer.cs` |
| Alta | Architettura | Piano 3 fasi unificazione legacy: Ô£à F1 completata (`Forms_list` + feature flag + fallback), Ô£à F2 completata (`StoryLibrary` 1-12 pronta al renderer dinamico), Ô£à F3 rollout completato (flag ON + fallback). Rifinitura residua: cleanup tecnico graduale dei code-behind legacy non piu' in path runtime principale | 
| Media | Multilanguage | Rifinire il glossario rule-based it/en del motore shared web/desktop con review manuale delle traduzioni bibliche piu' lunghe |
| Media | Gamification | **Streak + Badge**: N storie consecutive senza hint = badge "Saggio/Profeta/Apostolo" |
| Media | Gamification | **Classifica sessione locale**: 2-8 partecipanti inseriscono nome, XP aggregati, classifica finale |
| Media | Gamification | **Percorsi Tematici**: raccolte storie per tema (Fede/Amore/Coraggio) con barra progresso sbloccabile |
| Media | UX | ProgressPanel: aggiungere grafico barre XP e lista storie completate |
| Media | Content | Aggiungere storia ID 19+ (es. La Torre di Babele, Marta e Maria, Saul ÔåÆ Paolo) |
| Bassa | Gamification | **Timer di indovinamento**: 60s opzionale, bonus XP se risposta entro scadenza |
| Bassa | Gamification | **Modalit├á Riflessione**: dopo soluzione, domanda aperta da leggere al gruppo |
| Bassa | Gamification | **Storia del Giorno**: selezione automatica basata sulla data del calendario |
| Bassa | Tecnica | Sostituire BinaryFormatter con `System.Text.Json` + file JSON per persistenza |
| Bassa | UX grafiche | Hero image per ogni storia (immagine panoramica nell'header) |
| Bassa | Distribuzione | Build script Release + copia automatica in cartella distribuzione |
