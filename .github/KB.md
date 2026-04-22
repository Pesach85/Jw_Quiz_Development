# JW Quiz — Knowledge Base

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
| Controllo versione | Git — branch `main` |
| Lingua UI | Italiano |

---

## 2. Architettura dei File Chiave

| File | Ruolo |
|------|-------|
| `Program.cs` | Entry point — avvia `Form1` |
| `Form1.cs/Designer.cs` | Menu principale + navigazione |
| `Form2.cs`–`Form13.cs` | Storie statiche (ID 1–12): rebus con PictureBox PNG, 7 immagini, reveal progressivo |
| `Forms_list.cs` | Router di navigazione tra form |
| `FINE.cs` | Schermata finale |
| `Story.cs` | Modello dati storia (VisibleEmojis, HiddenEmojis, HintEmoji come chiavi risorsa PNG) |
| `StoryLibrary.cs` | Catalogo storie (ID 1–18): 1–12 statiche, 13–18 dinamiche |
| `StoryEngine.cs` | Logica progressione/navigazione tra storie |
| `DynamicStoryForm.cs` | Form generico per storie dinamiche (ID 13–18 e user-created) |
| `StoryEditorForm.cs` | Editor in-app per creare nuove storie con galleria immagini |
| `UserStoryLibrary.cs` | Persistenza storie utente su `UserStories.dat` |
| `ProgressTracker.cs` | XP, badge, persistenza su `UserProgress.dat` |
| `ProgressPanel.cs` | UI pannello progresso |
| `Resources/` | PNG colorati (~100 file) usati come simboli nel rebus |
| `Properties/Resources.Designer.cs` | Accesso fortemente tipizzato alle risorse |
| `webapp/index.html` | Shell UI web con gameplay + editor locale episodi |
| `webapp/app.js` | Logica gameplay web, fallback locale, integrazione API Cloudflare per storie condivise e PNG custom |
| `webapp/assets.js` | Manifest JS delle chiavi PNG disponibili nel picker immagini web |
| `webapp/stories.js` | Dataset storie dinamiche per la versione web |
| `webapp/styles.css` | Tema visuale web responsive |
| `webapp/assets/*.png` | Asset PNG deployati per gioco e editor web |
| `functions/api/stories.js` | API Cloudflare Pages Functions per leggere/salvare episodi condivisi |
| `functions/api/assets.js` | API Cloudflare Pages Functions per listare/caricare PNG custom condivisi |
| `functions/api/assets/[key].js` | Stream dei PNG custom salvati su bucket R2 |
| `wrangler.toml` | Config deploy Cloudflare (output webapp, compat date, vars) |

---

## 3. Gameplay — Meccanica Rebus

**Storie Statiche (Form2–Form13)**
- 7 `PictureBox` con PNG colorate dal namespace `Properties.Resources`
- `pictureBox3` e `pictureBox5`: nascosti di default (`Visible=false`), rivelati da "Rivela 2 immagini"
- `pictureBox8`: indizio, nascosto, rivelato da "Rivela indizio"
- `label1`: soluzione testo, nascosta, rivelata da "Rivela soluzione"
- Completamento storia registrato via `ProgressTracker.Instance.CompleteStory(storyId)` alla chiusura

**Storie Dinamiche (DynamicStoryForm)**
- 8 `PictureBox` (slot 0–4 visibili, 5–6 nascosti, 7 indizio)
- Slot 5–6 mostrano `2753.png` (❓) finché non rivelati
- Slot 7 mostra `1F525.png` (🔥) con animazione pulsante ambra (Timer 300ms) finché indizio non cliccato
- Didascalia click immagini: label dedicata ad alto contrasto **in alto al pannello** (non sovrapposta alle immagini)
- Pulsante "Rivela 2 immagini": rivela slot 5, poi 6 (secondo click)
- XP base 100, -20 per ogni aiuto usato (minimo 20)
- Header: titolo e riferimento biblico **NASCOSTI** fino a "Rivela soluzione"
- Header mostra solo: `"Episodio X — Indovina la storia!"` + categoria/keyword

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

## 5. Immagini Risorse — Chiavi Disponibili

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
| `1F525` | Fuoco 🔥 |
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
| `2753` | Punto interrogativo (❓ placeholder nascosto) |
| `203C` | Doppio esclamativo (‼ enfasi) |
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
- Fallback immagini utente normalizzati su chiavi PNG (`2753`, `1F525`), mai emoji unicode
- ID assegnato incrementalmente oltre 1000
- Webapp: editor locale in-browser con `localStorage` (`jwquiz_web_user_stories_v1`)
- Webapp: galleria immagini alimentata da `assets.js`, con ricerca per chiave PNG e anteprima per ogni slot
- Webapp: episodi creati lato browser vengono uniti ai 18 built-in nel selettore senza mostrare il titolo
- Cloudflare: dopo il deploy, gli episodi utente vengono salvati via Pages Functions in KV `JWQUIZ_DATA`
- Cloudflare: i PNG custom vengono caricati su bucket R2 `JWQUIZ_UPLOADS` e riutilizzati nel picker come chiavi `custom:<file>.png`
- Fallback locale: se le API Cloudflare non rispondono, la webapp continua a funzionare con persistenza solo browser-side

---

## 8. Convoluzioni/Vincoli Tecnici Noti

- **PlaceholderText** non disponibile in .NET Framework 4.7.2 (solo .NET 5+) — non usare
- **ResourceManager.GetObject** con chiave esatta (senza estensione) per caricare PNG
- Caricamento risorse centralizzato in `StoryResources.cs` (evitare accesso diretto duplicato al ResourceManager)
- **BinaryFormatter** deprecato in .NET 5+ ma funziona in net472
- **DockStyle.Fill** deve essere aggiunto per primo (`Controls.Add`) per corretta precedenza z-order
- **Designer.cs** dei form statici: NON modificare via codice — modifiche vanno fatte via VS Designer o con grande cautela
- **Emoji come testo** nei Label: evitare — usare PictureBox con PNG da Resources per coerenza visiva
- **Cloudflare shared mode** richiede 2 binding configurati in Pages: KV `JWQUIZ_DATA` e R2 `JWQUIZ_UPLOADS`
- In locale (`python -m http.server`) le Pages Functions non esistono: la webapp va automaticamente in fallback locale

---

## 9. Git / Delivery

- Branch: `main`
- Push dopo ogni modifica significativa
- No file non tracciati (`git status --short` deve essere pulito)
- Build deve passare (`MSBuild /verbosity:quiet`) prima di ogni commit
- Commit message in italiano, prefisso convenzionale: `feat:`, `fix:`, `refactor:`, `kb:`

---

## 10. Decisioni e Valutazioni — Log Cronologico

| Data | Decisione / Proposta | Esito |
|------|----------------------|-------|
| 2026-04-22 | Traduzione completa UI in italiano | ✅ Applicata |
| 2026-04-22 | Progressione XP + badge + ProgressTracker | ✅ Implementata |
| 2026-04-22 | Audit storie: mismatch StoryLibrary vs form reali | ✅ Corretta mappatura |
| 2026-04-22 | 6 nuovi episodi dinamici (ID 13–18) approvati | ✅ Implementati |
| 2026-04-22 | DynamicStoryForm con emoji label → PictureBox PNG colorati | ✅ Migrato |
| 2026-04-22 | Header nascosto (titolo/scripture rivelati solo con soluzione) | ✅ Implementato |
| 2026-04-22 | Indizio animato (pulsazione ambra su PictureBox slot 7) | ✅ Implementato |
| 2026-04-22 | StoryEditorForm: galleria visiva con miniature cliccabili | ✅ Implementata |
| 2026-04-22 | csproj convertito in SDK-style (net472) per C# Dev Kit | ✅ Applicata |
| 2026-04-22 | StoryLibrary ID 13–18: chiavi PNG al posto di caratteri unicode emoji | ✅ Applicata |
| 2026-04-22 | Story model: aggiunti campi ImageCaptions[] (8 slot) e ScriptureQuote | ✅ Implementato |
| 2026-04-22 | StoryLibrary ID 13–18: ImageCaptions + citazioni NMW (ScriptureQuote) | ✅ Popolate |
| 2026-04-22 | DynamicStoryForm: click su immagine mostra didascalia descrittiva | ✅ Implementato |
| 2026-04-22 | DynamicStoryForm: citazione TNM visibile nel pannello soluzione | ✅ Implementato |
| 2026-04-22 | Screen_size.IsFullscreen: stato persistente tra apertura di nuovi form | ✅ Implementato |
| 2026-04-22 | Forms_list.ShowForm(): helper centralizzato che ripristina fullscreen + refactoring metodi | ✅ Implementato |
| 2026-04-22 | Refactor architetturale: loader immagini centralizzato (`StoryResources`) + cleanup chiamate duplicate | ✅ Implementato |
| 2026-04-22 | DynamicStoryForm: didascalia click immagini resa sempre leggibile (alto contrasto + posizione non sovrapposta) | ✅ Implementato |
| 2026-04-22 | `ProgressTracker`: persistenza `StoryAttempts` + soglie badge robuste (`>=`) | ✅ Implementato |
| 2026-04-22 | `UserStoryLibrary`: serializzazione `ImageCaptions[]` + fallback PNG keys (no unicode emoji) | ✅ Implementato |
| 2026-04-22 | Compatibilità legacy preservata: API pubblica `Forms_list` mantenuta a istanza (no regressioni static forms) | ✅ Validato |
| 2026-04-22 | Web MVP immediato: creata webapp statica self-contained (episodi 13-18) pronta per hosting gratuito | ✅ Implementato |
| 2026-04-22 | **Webapp completata**: aggiunti episodi 1-12 in `stories.js` (18 episodi totali, 56 PNG mappati) | ✅ Implementato |
| 2026-04-22 | **UX webapp**: dropdown mostra solo "Episodio X" (no titolo storia per evitare spoiler) | ✅ Implementato |
| 2026-04-22 | **Assets webapp**: copiati 49 PNG aggiuntivi per episodi 1-12 → 80 PNG totali self-contained | ✅ Implementato |
| 2026-04-22 | **Editor web episodi**: aggiunta creazione locale nuovi episodi con picker PNG, merge selettore e persistenza `localStorage` | ✅ Implementato |
| 2026-04-22 | **Asset parity web**: sincronizzati tutti i PNG da `Resources/` a `webapp/assets/` per avere galleria completa come desktop | ✅ Implementato |
| 2026-04-22 | **Cloudflare shared persistence**: aggiunte Pages Functions per episodi condivisi su KV e asset PNG custom su R2 | ✅ Implementato |
| 2026-04-22 | **Picker web avanzato**: aggiunto upload PNG custom dal browser con riuso immediato nei nuovi episodi | ✅ Implementato |
| 2026-04-22 | **Deploy readiness**: aggiunto `wrangler.toml` con naming `jwquiz` per primo deploy | ✅ Implementato |
| 2026-04-22 | **Hotfix deploy**: rimossi binding KV/R2 da `wrangler.toml` (placeholder ID invalido) per usare i binding reali configurati nel pannello Pages | ✅ Implementato |

---

## 11. Next Best Decisions (Proposte Attive)

Aggiornare questa sezione ad ogni sessione di lavoro.

| Priorità | Area | Proposta |
|---------|------|---------|
| Alta | Web | ~~Migrare anche storie statiche 1-12 nel renderer web unificato~~ ✅ **COMPLETATO** |
| Alta | Gamification | **Sistema a Stelle** (1-3 per storia): 3=nessun aiuto, 2=1 aiuto, 1=tutti aiuti — visual memorabile per proiezione |
| Alta | Gameplay | Form statici (2–13): indizio animato come DynamicStoryForm (pulsazione su pictureBox8) |
| Alta | Content | Aggiungere ImageCaptions[] anche alle storie statiche id 1-12 (attualmente solo ID 13-18) |
| Media | Gamification | **Streak + Badge**: N storie consecutive senza hint = badge "Saggio/Profeta/Apostolo" |
| Media | Gamification | **Classifica sessione locale**: 2-8 partecipanti inseriscono nome, XP aggregati, classifica finale |
| Media | Gamification | **Percorsi Tematici**: raccolte storie per tema (Fede/Amore/Coraggio) con barra progresso sbloccabile |
| Media | UX | ProgressPanel: aggiungere grafico barre XP e lista storie completate |
| Media | Content | Aggiungere storia ID 19+ (es. La Torre di Babele, Marta e Maria, Saul → Paolo) |
| Bassa | Gamification | **Timer di indovinamento**: 60s opzionale, bonus XP se risposta entro scadenza |
| Bassa | Gamification | **Modalità Riflessione**: dopo soluzione, domanda aperta da leggere al gruppo |
| Bassa | Gamification | **Storia del Giorno**: selezione automatica basata sulla data del calendario |
| Bassa | Tecnica | Sostituire BinaryFormatter con `System.Text.Json` + file JSON per persistenza |
| Bassa | UX grafiche | Hero image per ogni storia (immagine panoramica nell'header) |
| Bassa | Distribuzione | Build script Release + copia automatica in cartella distribuzione |
