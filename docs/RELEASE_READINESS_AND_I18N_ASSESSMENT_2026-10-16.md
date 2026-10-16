# JW Quiz — Release readiness e assessment multilingua (2026-10-16)

Stato verificato a HEAD `23be792` (`main`, 7 commit avanti a `origin/main` = `67eb4f9`, nessun push).
Classi di evidenza: **VERIFIED** (comando/test eseguito in questa sessione), **DEDUCED** (inferenza con premesse), **UNKNOWN**.
Fonte canonica dei gate: `.github/KB.md` §10, §11, §17. Questo documento non autorizza push, deploy o release.

---

## 1. Matrice di readiness

| Area | Stato verificato | Evidenza | Gap | Severità | Prossima azione |
|---|---|---|---|---|---|
| Git/governance | `main` = `23be792`, ahead 7, behind 0; worktree deploy `D:\Jw_Quiz_deploy_41a75db` = `41a75db` pulito, antenato di HEAD (delta solo KB) | `git log origin/main..HEAD`, `git ls-remote`, `git worktree list` (VERIFIED) | Push non eseguito: la KB §17 richiede richiesta esplicita | HIGH (non blocca build locali) | Decisione push del proprietario |
| Web/Pages | Live = `67eb4f9` (index, styles, stories, app hash = blob dopo normalizzazione EOL); sorgente HEAD differisce solo in `index.html` + `styles.css` | `curl` + `git show` (VERIFIED) | Token OAuth Wrangler scaduto, refresh non interattivo impossibile | **BLOCKER** (pubblicazione) | Il proprietario esegue `npx --yes wrangler@4.151.1 login` |
| AND-2 legacy WebView | `inset` assente da `index.html` e `styles.css`; WebView 83 reale senza patch: onboarding, teatro, Rebus, 3 flussi OK | `74971aa`; emulatore API 30 (VERIFIED) | `aspect-ratio` (Chrome 88+) residuo in onboarding/challenge, non bloccante | LOW | Nessuna per il rilascio |
| Mobile spoiler-hint UX | Didascalia in alto nel dialog + sticky sopra la griglia; verticale 320/375/430, orizzontale 917×408/844×390/740×360; desktop identico alla baseline | `74971aa`, `d6c5f89`; test browser + Motorola reale (VERIFIED) | Orizzontale non provato sul telefono fisico | LOW | Facoltativo, al prossimo smoke |
| Android build/runtime | APK Debug 14:25 = `webapp/` HEAD (11/11 statico), firma Android Debug, `com.jwquiz.app` 1.0.0 (vc 1); smoke reale Motorola PASS, dati invariati | `and1_apk_check`, `apksigner`, smoke 2026-10-16 (VERIFIED) | Nessuna firma release (`release {}` senza `signingConfig`), nessun AAB, canale di distribuzione non definito | **BLOCKER** (distribuzione) / HIGH | Decisione canale (sideload / Play / altro) + chiave di upload gestita dal proprietario |
| Desktop build/runtime | Nessuna modifica C#/csproj/resx dopo `f9444a1`; binari Debug/Release del 2026-10-16 12:10 → harness 10/10 ×2, `UserProgress.dat` reale invariato | `git log -- '*.cs'`, `dsk1_harness.ps1` (VERIFIED) | Canale non definito; `bin\Release` contiene `.pdb` e un `app.publish/` di aprile; nessuna firma | **BLOCKER** (distribuzione) / MEDIUM | Decisione canale; packaging con solo `.exe` + `.exe.config` |
| Security/artifact hygiene | APK: solo `assets/www`, nessun `.wrangler`/`.dev.vars`/`.pfx`; live `/.wrangler/...` = fallback SPA (non pubblicato); nessun segreto a stringa in web/functions/tools | scan `git grep`, `zipfile`, `curl` (VERIFIED) | `Jw_Quiz_Development_TemporaryKey.pfx` tracciato nel repo **pubblico** dal primo commit (non usato dalla build: `SignManifests`/`SignAssembly` = false) | MEDIUM | Decisione proprietario: rimozione dal tree + `.gitignore`; mai usarlo per firmare |
| Multilingua | Solo IT + EN; vedi §3 | ispezione codice (VERIFIED) | Il validator rifiuta lingue diverse da it/en; EN rebus via traduzione a regole; 7 ternari `it/en` hardcoded nel player | NON-BLOCKING per il rilascio | Milestone I18N-0 (§4) |

Pulizia eseguita: rimosso `bin\Release\UserOnboarding.dat`, creato dallo smoke DSK-1 di questa sessione (contenuto: flag `completed` + nome di default). Cartella `bin/` gitignored.

## 2. Percorso minimo al rilascio

1. **Web** (unico rilascio con tutte le precondizioni tecniche soddisfatte): login Wrangler del proprietario → `whoami` → `pages deployment list` (stop se la produzione non è `67eb4f9` su `main`) → deploy dal worktree `41a75db` con `--branch=main --commit-hash=41a75db2e8673d186eb14d899cacf15c7fae8fe8` → verifica hash live + runtime (overlay, indizio 375 px e orizzontale, desktop). Rollback: deployment `5739b69f`.
2. **Push** di `main` (decisione esplicita, KB §17), consigliato prima o insieme al deploy perché il commit pubblicato sia tracciabile su GitHub.
3. **Android**: scelta del canale → configurazione firma release (chiave fuori repo) → `assembleRelease`/`bundleRelease` → `versionCode` > 1 → smoke su dispositivo con `install -r` (aggiornare da firma debug a firma release richiede disinstallazione = **perdita dati** WebView: va pianificato).
4. **Desktop**: scelta del canale → cartella con `Jw_Quiz_Development.exe` + `.exe.config` (escludere `.pdb`, `*.dat`, `app.publish/`) → nota: i file utente (`UserProgress.dat`, `UserStories.dat`) sono relativi alla cartella di lavoro, `UserOnboarding.dat` alla cartella dell'exe.

DEDUCED: il punto 3 ha un rischio dati non ovvio. Android rifiuta l'aggiornamento tra firme diverse, quindi chi ha installato l'APK debug (es. Motorola) perderebbe i progressi passando a una build release, salvo export/import.

## 3. Multilingua — stato attuale

| Livello | Struttura attuale | Limite per IT/EN/ES/EL/DE/FR/RU/JA/ZH |
|---|---|---|
| Canone `data/episodes.json` (schema v1) | Due pattern: campi a suffisso (`rebus.titleIt`, `hintIt`, `solutionIt`, `scriptureQuoteIt`, `imageCaptionsIt[8]`, `immersive.titleEn`, `themeIt/themeEn`) e oggetti `{it, en}` (intro, domande, risposte, morale, citazione) | Rebus solo IT; `scriptureReference` è una stringa italiana ("Genesi 2-3") senza id libro |
| Validator `tools/validate_episodes.py` | `extra = set(keys) - {"it","en"}` → errore; `en` obbligatorio | Impossibile aggiungere anche una sola lingua senza modificarlo |
| Generator `tools/generate_story_artifacts.py` | Emette esplicitamente `it:` ed `en:` | Va reso generico per lingua |
| Player `webapp/index.html` | `LANGS = ["it","en"]`; `t(key)` → `UI[lang] ?? UI.it ?? key`; `tl(obj)` → `obj[lang] ?? obj.it` (fallback già corretto); ~133 chiavi `UI.it` | 7 ternari `lang === "it" ? … : …` fuori dizionario; `documentElement.lang` binario; selettore lingua a 2 voci |
| Editor `classic.html` + `story-i18n.js` | Traduzione **a regole** IT↔EN parola per parola | Non scalabile e rischiosa per testi biblici; `app.js` off-limits (KB) |
| Desktop | `enum AppLanguage { Italian, English }`; `AppText` 216 voci; `StoryTranslationEngine` dizionario IT→EN; `StoryCaptionPolicy` con stopword italiane | Enum, dizionari e policy anti-spoiler sono bilingui per costruzione |
| Font/layout | `system-ui` / Georgia; Android WebView e Windows hanno fallback per greco, cirillico e CJK (DEDUCED, non testato) | Overflow con tedesco nei bottoni; a capo CJK; nessuna lingua RTL richiesta |
| Plurali/formati | Nessun `Intl.PluralRules`; poche stringhe con numeri (`{0}`, "0/23 · 0 XP") | Russo (plurali) da gestire solo dove servono |

Decisioni umane necessarie prima della traduzione:
- codici lingua: `el` (GR), `de` (GER), `fr` (FRA), `ja` (JPN), **`zh-Hans` o `zh-Hant`** (CHN: da scegliere);
- revisione dei contenuti biblici: parafrasi e citazioni per lingua soggette allo stesso gate di D1 (AGENTS.md: wording delle scritture = stop and ask);
- nomi dei libri biblici per lingua (tabella di riferimento approvata).

## 4. Proposta prima milestone: I18N-0 — infrastruttura + pilota

**Obiettivo misurabile**: il player Web/Android mostra l'Episodio 1 completo (rebus, didascalie, indizio, soluzione, domande, morale) in **ES** e in **una lingua a script non latino** (proposta: **JA**, che mette alla prova font, a capo e lunghezze), con fallback dichiarato per tutti gli altri episodi; IT/EN restano byte-identici negli artefatti.

Scelta strutturale (additiva, schema v2 compatibile):
- nuovo blocco per episodio `locales: { "<lang>": { status: "draft" | "reviewed", ...solo testi } }`, con la stessa forma testuale dei campi IT (titolo, keyword, hint, solution, scriptureQuote, engagementNote, captions[8], intro, questions[].prompt, answers[] testo, moral, theaterQuote);
- la struttura (chiavi PNG, ordine, `ok`) resta solo nel canone: le traduzioni non la duplicano;
- `scriptureReference` affiancato da `scriptureRef: { book: "GEN", chapters: "2-3" }` + tabella libri per lingua.

Lavoro incluso:
1. validator: accetta `locales`, verifica parità di lunghezze (8 didascalie, n domande, 3 risposte), nessun campo strutturale, report di copertura per lingua;
2. generator: emette `JW_LOCALES` in `stories.js`; IT/EN invariati (controllo `--check` + parity);
3. player: `LANGS` da configurazione, i 7 ternari spostati in `UI`, `documentElement.lang` reale, risoluzione `locale[lang] → it`; badge "bozza" se `status: draft`;
4. test: parity, validator, Web 320/375/430 + desktop per IT/EN/ES/JA, WebView 83, APK statico;
5. Desktop: **fuori dal pilota** — resta IT/EN con test che una lingua non supportata ricade su IT.

Escluso: traduzione degli altri 22 episodi, editor `classic.html`/`app.js`, policy anti-spoiler per lingua sul Desktop, nuove dipendenze.
Rollback: revert del commit; i campi `locales` sono additivi e ignorati dagli artefatti precedenti.
Criteri PASS: validator/parity/`--check` exit 0; artefatti IT/EN invariati byte per byte; Ep1 ES/JA leggibile senza overflow ai viewport previsti; nessuna regressione delle tre modalità.
