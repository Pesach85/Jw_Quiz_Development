# Brand Identity Proposal — JW Quiz

**Data:** 2026-09-30  
**Stato:** proposta read-only — **STOP HITL B1–B8** (nessun runtime)  
**HEAD base:** `99f7e59`  
**Scope:** audit + proposta. Zero modifiche a codice / assets.

---

## A1 — Audit attuale (evidenza)

### A1.1 Web immersive — `webapp/index.html`

#### Palette CSS (`:root`, L26–41)

| Token | Hex / valore | Ruolo |
|-------|--------------|-------|
| `--bg-deep` | `#0b1220` | fondo primario |
| `--bg-mid` | `#132033` | fondo secondario / gradient mid |
| (body gradient end) | `#0e1828` | L53 |
| `--ink` | `#e8eef7` | testo primario |
| `--muted` | `#9bb0c9` | testo secondario |
| `--brand` | `#f0c36a` | oro brand |
| `--brand-soft` | `#d4a24a` | oro soft / hover |
| `--accent` | `#3d8fd1` | blu accent |
| `--ok` | `#3cb371` | successo |
| `--bad` | `#e05a5a` | errore |
| `--line` | `rgba(240,195,106,0.28)` | bordi |
| `--panel` | `rgba(19,32,51,0.82)` | pannelli |
| `.btn-primary` | `#c9942e` → `#f0c36a` | L96–97 |

**Screenshot palette (web):** `.local/smoke/j_journey_disabled.png` (hero blu scuro + oro; top brand “JW Quiz Immersive”).  
Sample bucket dominante (resize 48×48): `(0,16,32)` / `(16,16,32)` — allineato a `--bg-deep`/`--bg-mid`.

#### Tipografia

| Voce | Evidenza |
|------|----------|
| CDN Google Fonts | L11–13: Cormorant Garamond 500/700 + Outfit 400/600/700 |
| Display | `--font-display: "Cormorant Garamond", Georgia, serif` L38 |
| Body | `--font-body: "Outfit", "Segoe UI", sans-serif` L39 |
| Brand size | `.brand` `clamp(1.6rem, 3vw, 2.2rem)` L75–77 |
| H1 hero | `clamp(2.4rem, 7vw, 4.4rem)` L131–134 |

#### Logo / favicon

| Asset | Evidenza |
|-------|----------|
| Favicon | `webapp/favicon.svg` — rect `#0b1220` rx=14, cerchio stroke gradient `#f0c36a`→`#3d8fd1`, checkmark oro |
| Brand testo | HTML L806: `data-i18n="brand"` → `UI.it.brand` / `UI.en.brand` = **"JW Quiz Immersive"** (L998, L1127) |
| `<title>` | L8: `JW Quiz Immersive` |
| Nessun PNG logo separato in nav | brand = testo tipografico |

#### Naming UI (estratto)

| Chiave | IT | EN |
|--------|----|----|
| `brand` | JW Quiz Immersive | JW Quiz Immersive |
| `heroTitle` | Fai vivere il racconto | Bring the account to life |
| `modeQuiz` / `modeRebus` / `modeJourney` | Quiz / Rebus 3D / Avventura | Quiz / 3D Rebus / Adventure |
| `footerNote` | …**non è un prodotto ufficiale Watch Tower** | …**not an official Watch Tower product** (L1035 / L1155) |
| `onb_discBody` | Prodotto didattico **non ufficiale Watch Tower** (L1072) | Unofficial… not Watch Tower (L1192) |
| Onboarding | `onb_stepOf`: “Passo {n} di **7**” (L1068) — step 1…7 in render (L2649–2709) |

---

### A1.2 Desktop — `Form1.Designer.cs` + `Resources/Intro.jpg`

| Voce | Evidenza |
|------|----------|
| Window title | `this.Text = "JW Quiz"` — `Form1.Designer.cs` L265 |
| Font form | `AutoScaleDimensions = 6F, 13F` (default WinForms) L259 — **nessun** `Font =` custom nel Designer |
| BackColor/ForeColor | **non** impostati nel Designer → SystemColors default (Control/Window) |
| Logo / splash | `pictureBox1.Image = Properties.Resources.Intro` L234; resx → `Resources\Intro.jpg` (`Properties/Resources.resx` L454–455) |
| Intro.jpg palette (PIL) | 1200×1200; top bucket **`(80,48,128)` ≈ `#503080` viola** + bianco `(240,240,240)`; `purpleish_px=3076`, `goldish_px=0` |

**Nota vs web:** desktop splash = viola + bianco (Intro.jpg); web immersive = blu scuro + oro (`:root`). Naming desktop corto “JW Quiz”; web “JW Quiz Immersive”.

Screenshot correlati: `.local/smoke/bug2_ep1.png`, `.local/smoke/fix_ep13_desktop.png` (chrome WinForms / rebus, non splash).

---

### A1.3 Android — manifest + res

| Voce | Evidenza |
|------|----------|
| App name | `android/.../res/values/strings.xml`: `app_name` = **JW Quiz** |
| Icon | `@mipmap/ic_launcher` — `AndroidManifest.xml` L6–7 |
| Theme | `Theme.AppCompat.DayNight.NoActionBar` L10 |
| Package | `com.jwquiz.app` (`android/app/build.gradle` applicationId) |
| UI visuale | WebView → stesso `webapp/` post-`sync_all` (shell non ridisegna brand) |
| Icon sample | `mipmap-hdpi/ic_launcher.png` 1024×1024 — bucket dominanti neri/grigi scuri (PIL) |

---

### A1.4 Invarianti disclaimer (docs)

| Fonte | Testo / regola |
|-------|----------------|
| `docs/ARCHITECTURE.md` L3–4 | Inspired by jw.org style; **Not** an official Watch Tower product; do not copy JW.org artwork |
| `docs/ARCHITECTURE.md` L50 | Footer must keep unofficial disclaimer + link to jw.org |
| `docs/TECHNICAL_REVIEW_REPORT.md` L12 | prodotto didattico originale (non ufficiale Watch Tower) |
| Onboarding + footer in `index.html` | vedi A1.1 |

`docs/AGENTS.md` non ripete il disclaimer Watch Tower in una sezione dedicata; l’invariante brand legale è in **ARCHITECTURE** + UI onboarding/footer.

---

### A1.5 Web classic — naming

| Voce | Evidenza |
|------|----------|
| `<title>` | `JW Quiz — Editor / Admin` (`classic.html` L6) |
| H1 | `JW Quiz Editor` L16 |
| Favicon | stesso `favicon.svg` L7 |

---

## A2 — Proposta identity

### 1. Nome + tagline

| | Proposta default |
|--|------------------|
| **Nome prodotto** | **JW Quiz** (corto, già desktop/Android) |
| **Sottotitolo / tagline IT** | *Fai vivere il racconto* (già `heroTitle`) |
| **Sottotitolo EN** | *Bring the account to life* |
| **Variante surface** | Immersive UI può restare “JW Quiz Immersive” in nav **oppure** uniformare a “JW Quiz” (HITL B1) |

Conferma proposta: **nome A (attuale famiglia JW Quiz)**; tagline attuale OK.

### 2. Palette (5 colori) — proposta = **evoluzione leggera dalla web attuale (B2→A)**

| Ruolo | Hex | Motivazione (legata all’audit) |
|-------|-----|--------------------------------|
| Primary (brand) | `#f0c36a` | già `--brand`; oro leggibile su dark |
| Secondary (accent) | `#3d8fd1` | già `--accent`; già in favicon gradient |
| Accent soft | `#d4a24a` | già `--brand-soft` |
| Background | `#0b1220` | già `--bg-deep`; campione smoke hero |
| Text | `#e8eef7` | già `--ink` |

**Non** adottare il viola splash desktop (`#503080` Intro.jpg) come primary web: crea fork brand (viola vs oro/blu). Evoluzione = **allineare desktop splash al sistema web** (fase runtime futura, fuori scope).

### 3. Tipografia — proposta **B3→A system stack** (zero CDN)

| Ruolo | Stack proposto | Scala |
|-------|----------------|-------|
| Display / H1 | `Georgia, "Times New Roman", serif` | H1 ≈ `clamp(2.2rem, 6vw, 3.6rem)` |
| H2 | stesso display, peso bold | ≈ `clamp(1.6rem, 3vw, 2.2rem)` |
| Body | `system-ui, "Segoe UI", Roboto, sans-serif` | 1rem / 1.5 line |
| Small | stesso body | 0.875rem |

**Motivazione:** oggi Google Fonts CDN (`index.html` L11–13) — offline/Android WebView + privacy + dipendenza rete. System stack = zero CDN. (Se HITL sceglie B Google Fonts, mantenere Cormorant+Outfit.)

### 4. Marchio — proposta **B4→B evolve**

| Oggi | Evolve |
|------|--------|
| Favicon: dark square + ring gradient + check | Mantenere geometria; **aggiungere monogramma “JQ”** o parola “JW” in stroke oro dentro il cerchio (stesso SVG) |
| Desktop Intro.jpg viola + testo “JW QUIZ” | Sostituire (futuro) con splash allineato a favicon/web (blu `#0b1220` + oro) |
| Nav brand testo | Tipografia display + eventuale icona 24px = favicon |

**Non** ridisegnare da zero (C) finché palette non è congelata HITL.

### 5. Favicon

- Source of truth: `webapp/favicon.svg` (già coerente con `:root`).
- Derive: `favicon.ico` / Android mipmap da SVG (fase implementazione).
- Evitare viola Intro come icona launcher (oggi launcher è dark).

### 6. Tono voce — proposta **B5→B informale cordiale**

| Contesto | Before (attuale) | After (esempio) |
|----------|------------------|-----------------|
| Hero lead | “Tre modalità di gioco: Quiz, Rebus 3D o Avventura completa — immersiva, con audio e scene animate.” | “Scegli come giocare: quiz, rebus 3D o l’avventura intera — con scene e audio.” |
| Onboarding disc | “Prodotto didattico non ufficiale Watch Tower…” | “Questa app è **didattica e non ufficiale**. Ci ispiriamo allo stile di jw.org — il link ufficiale è in fondo.” (disclaimer **obbligatorio**) |
| Feedback quiz | “Ottimo! Risposta corretta.” | “Bene! Continua.” / “Quasi — rifletti e vai avanti.” |

Formale solo su disclaimer legale; gameplay = informale chiaro.

### 7. Cross-surface

| Surface | Proposta |
|---------|----------|
| Web immersive | Brand core (nome + palette + favicon) |
| Web classic | **Variante editor** (HITL B6→B): stesso logo/palette, suffix “Editor” (già `JW Quiz Editor`) |
| Desktop | **Allineare al web** (B7→A): title “JW Quiz”; splash futurasostitutivo Intro.jpg |
| Android | Stesso web bundle; `app_name` “JW Quiz”; icon da favicon |

### 8. Rischi

| Rischio | Mitigazione |
|---------|-------------|
| Confusione con prodotti Watch Tower / jw.org | Disclaimer onboarding + footer **invariati**; no artwork JW.org; no wordmark ufficiale |
| Over-branding | Non aggiungere badge/chip nel hero; brand = nav + favicon + un CTA |
| Fork viola desktop vs oro web | Unificare splash (future) o documentare “legacy Intro” esplicitamente |
| CDN font down / offline | Preferire system stack (B3 A) |

---

## A3 — HITL table (rispondi B1–B8)

| # | Domanda | Opzioni | **Proposta** |
|---|---------|---------|--------------|
| **B1** | Nome | A) famiglia attuale (JW Quiz / Immersive) B) nuovo nome | **A** |
| **B2** | Palette | A) attuale web (oro+blu) B) evoluzione C) redesign | **A** |
| **B3** | Tipografia | A) system stack B) Google Fonts (attuale) C) custom | **A** |
| **B4** | Logo | A) mantieni favicon B) evolve C) ridisegna | **B** |
| **B5** | Tono | A) formale B) informale C) misto | **B** |
| **B6** | Classic brand | A) uguale player B) variante editor | **B** |
| **B7** | Desktop brand | A) uguale web B) nativo WinForms distinto | **A** |
| **B8** | `jwquiz_theme_v1` (persistenza tema) | A) sì B) no | **B** |

**Rispondi:** `BRAND OK` oppure `B1:A B2:…` puntuale.

---

## Non-goals (questo doc)

- Nessun commit di asset/CSS/runtime.
- Nessun cambio copy finché HITL non conferma.
- Onboarding 7-step (navigazione back/forward) = fuori scope brand (feature UX separata).
