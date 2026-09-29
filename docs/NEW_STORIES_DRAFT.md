# Bozze episodi 19+ (review umana)

**Stato:** DRAFT — **non** in `data/episodes.json`.  
**Data:** 2026-09-29  
**Schema:** v1 (invariato — nessun campo nuovo proposto).  
**Vincolo citazioni:** nessun testo di versetto inventato o copiato.  
`scriptureQuoteIt` / `theaterQuote` sotto = **riferimento + parafrasi didattica neutra** in attesa di OK umano sul testo ufficiale (es. TNM).

Dopo approvazione: inserire in `data/episodes.json` → `validate_episodes.py --full-catalog` → `sync_all.py` → `verify_episode_parity.py` + riga `DECOR_SYMBOLS` in `webapp/index.html`.

---

## Tabella riassuntiva

| id | Titolo IT | Tema IT / EN | Scripture | PNG |
|----|-----------|--------------|-----------|-----|
| 19 | La Torre di Babele | Orgoglio / Pride | Genesi 11:1-9 | tutte esistenti |
| 20 | Daniele nella fossa dei leoni | Fede / Faith | Daniele 6 | tutte esistenti |
| 21 | Saul diventa Paolo | Conversione / Conversion | Atti 9:1-22 | tutte esistenti |
| 22 | Giosuè e Gerico | Coraggio / Courage | Giosuè 6 | tutte esistenti |
| 23 | Marta e Maria | Priorità / Priorities | Luca 10:38-42 | tutte esistenti |

---

## Episodio 19 — La Torre di Babele

| Campo | Valore |
|-------|--------|
| id | 19 |
| titleIt | La Torre di Babele |
| titleEn | The Tower of Babel |
| themeIt / themeEn | Orgoglio / Pride |
| keywordIt | Orgoglio |
| scriptureReference | Genesi 11:1-9 |

**intro**

```json
{
  "it": "Un popolo unito decide di costruire qualcosa di grandioso. Ma le intenzioni del cuore non sfuggono a Dio.",
  "en": "A united people set out to build something magnificent. Yet the heart’s motives are not hidden from God."
}
```

**hintIt / solutionIt / engagementNoteIt** (rebus)

- hintIt: Una città, una torre verso il cielo e lingue che non si capiscono più.
- solutionIt: A Babele l’umanità volle farsi un nome; Geova confuse le lingue e disperse le persone. Genesi 11:1-9.
- engagementNoteIt: Collega l’orgoglio umano all’umiltà di ascoltare Dio invece di esaltare sé stessi.

**scriptureQuoteIt** (tier 2 — **pending OK testo ufficiale**)

> Genesi 11:7 — *[parafrasi didattica]* Dio interviene perché l’umanità non continui un progetto guidato dall’orgoglio.  
> **Azione umana:** sostituire con citazione approvata (TNM o altra) prima del merge.

**theaterQuote** (tier 1 — **pending OK**)

```json
{
  "it": "[BOZZA] Riferimento Genesi 11:7 — Dio ferma l’esaltazione umana confondendo le lingue.",
  "en": "[DRAFT] Reference Genesis 11:7 — God stops human self-exaltation by confusing languages."
}
```

**questions** (2 × 3, una `ok: true`)

1. Prompt IT: Perché Geova intervenne a Babele? / EN: Why did Jehovah intervene at Babel?  
   - L’umanità stava agendo con orgoglio, non per onorare Dio (ok)  
   - Mancavano mattoni (false)  
   - Volevano solo dipingere la torre (false)
2. Prompt IT: Quale atteggiamento opposto all’orgoglio ci invita questa storia? / EN: Which attitude opposite to pride does this account invite?  
   - Umiltà e dipendenza da Dio (ok)  
   - Competizione senza limiti (false)  
   - Isolarsi da tutti (false)

**moral**

```json
{
  "it": "I progetti grandi non bastano se il cuore è pieno di sé. L’umiltà apre la via alla benedizione di Dio.",
  "en": "Grand projects are not enough if the heart is full of self. Humility opens the way to God’s blessing."
}
```

**PNG keys** (tutte in `webapp/assets/`)

| Ruolo | Key | Note |
|-------|-----|------|
| visible[5] | `1F3DB`, `093-users`, `1F4AC`, `1F632`, `1F5FA` | edificio, folla, discorso, shock, mappa |
| hidden[2] | `1F3F0`, `1F334` | torre/castello, palma (crescita “verso l’alto”) |
| hintKey | `1F3F0` | pattern Ep 9/10/12 (hint = hidden[0]) |

**imageCaptionsIt[8]** (anti-spoiler)

1. Un edificio imponente  
2. Tante persone insieme  
3. Voci che si sovrappongono  
4. Lo stupore sul volto  
5. Una mappa del mondo conosciuto  
6. Una torre che punta in alto  
7. Una pianta che cresce verso il cielo  
8. La stessa torre vista da lontano  

---

## Episodio 20 — Daniele nella fossa dei leoni

| Campo | Valore |
|-------|--------|
| id | 20 |
| titleIt | Daniele nella fossa dei leoni |
| titleEn | Daniel in the Lions’ Den |
| themeIt / themeEn | Fede / Faith |
| keywordIt | Fede |
| scriptureReference | Daniele 6 |

**intro**

```json
{
  "it": "Un uomo fedele prega ogni giorno, anche quando una legge lo mette in pericolo.",
  "en": "A faithful man keeps praying every day — even when a law puts him in danger."
}
```

**hintIt / solutionIt / engagementNoteIt**

- hintIt: Finestre aperte verso Gerusalemme, una fossa e leoni che non divorano.
- solutionIt: Daniele continuò a pregare nonostante il decreto; Geova lo protesse nella fossa dei leoni. Daniele 6.
- engagementNoteIt: La fedeltà nella preghiera vale più del favore dei potenti.

**scriptureQuoteIt** (**pending OK**)

> Daniele 6:22 — *[parafrasi]* Dio manda aiuto e i leoni non fanno male al suo servo.  
> **Azione umana:** testo ufficiale da approvare.

**theaterQuote** (**pending OK**)

```json
{
  "it": "[BOZZA] Riferimento Daniele 6:22 — Dio protegge chi gli resta fedele.",
  "en": "[DRAFT] Reference Daniel 6:22 — God protects those who remain loyal to him."
}
```

**questions**

1. Cosa fece Daniele dopo il decreto che vietava la preghiera?  
   - Continuò a pregare Dio come prima (ok) / Smise del tutto (f) / Fuggì in un altro paese (f)  
2. Cosa insegna questa storia sulla protezione di Dio?  
   - Dio può proteggere chi gli obbedisce anche in pericolo (ok) / Solo i re sono al sicuro (f) / La preghiera è inutile (f)

**moral**

```json
{
  "it": "La fedeltà quotidiana prepara il cuore alle prove. Dio vede chi gli resta vicino.",
  "en": "Daily loyalty prepares the heart for tests. God sees those who stay close to him."
}
```

**PNG**

| Ruolo | Key |
|-------|-----|
| visible | `1F932-1F3FC`, `1F451`, `1F440`, `1F47C`, `1F6B8` |
| hidden | `1F981`, `2694` |
| hintKey | `1F981` |

**imageCaptionsIt**

1. Qualcuno in preghiera  
2. Una corona di autorità  
3. Occhi che osservano  
4. Una figura di protezione  
5. Un segnale di pericolo  
6. Un leone potente  
7. Un’arma che non decide il destino  
8. Lo stesso leone, da vicino  

---

## Episodio 21 — Saul diventa Paolo

| Campo | Valore |
|-------|--------|
| id | 21 |
| titleIt | Saul diventa Paolo |
| titleEn | Saul Becomes Paul |
| themeIt / themeEn | Conversione / Conversion |
| keywordIt | Conversione |
| scriptureReference | Atti 9:1-22 |

**intro**

```json
{
  "it": "Un uomo sicuro delle sue ragioni viaggia per fermare una fede. Lungo la strada, qualcosa lo ferma.",
  "en": "A man sure of his cause travels to stop a faith. Along the road, something stops him."
}
```

**hintIt / solutionIt / engagementNoteIt**

- hintIt: Una luce accecante sulla via di Damasco e una voce che cambia tutto.
- solutionIt: Gesù apparve a Saul sulla via di Damasco; da persecutore divenne apostolo. Atti 9:1-22.
- engagementNoteIt: Nessuno è troppo lontano perché Dio possa cambiare il cuore.

**scriptureQuoteIt** (**pending OK**)

> Atti 9:4-5 — *[parafrasi]* Una voce chiede perché viene perseguitato; l’incontro cambia la direzione della vita.  
> **Azione umana:** testo ufficiale da approvare.

**theaterQuote** (**pending OK**)

```json
{
  "it": "[BOZZA] Riferimento Atti 9:6 — Dopo l’incontro, resta solo da chiedere cosa fare.",
  "en": "[DRAFT] Reference Acts 9:6 — After the encounter, the only question left is what to do."
}
```

**questions**

1. Cosa accadde a Saul sulla via di Damasco?  
   - Incontrò Gesù e cambiò direzione di vita (ok) / Vinse una battaglia militare (f) / Costruì una torre (f)  
2. Cosa insegna questa conversione?  
   - Dio può trasformare anche chi si oppone (ok) / Solo i perfetti possono servire (f) / Il passato non può essere perdonato (f)

**moral**

```json
{
  "it": "Il perdono di Dio apre una nuova missione. Il passato non blocca chi risponde alla chiamata.",
  "en": "God’s forgiveness opens a new mission. The past does not block those who answer the call."
}
```

**PNG**

| Ruolo | Key |
|-------|-----|
| visible | `1F6B6-1F3FF-200D-2642-FE0F`, `1F525`, `1F632`, `1F4D6`, `1F440` |
| hidden | `1F4AC`, `1F318` |
| hintKey | `1F4AC` |

**imageCaptionsIt**

1. Un viaggio a piedi  
2. Una luce intensa  
3. Lo shock improvviso  
4. Un libro di istruzioni  
5. Occhi che devono riaprire  
6. Parole che guariscono  
7. La notte che lascia spazio al giorno  
8. Di nuovo le parole, come missione  

---

## Episodio 22 — Giosuè e Gerico

| Campo | Valore |
|-------|--------|
| id | 22 |
| titleIt | Giosuè e Gerico |
| titleEn | Joshua and Jericho |
| themeIt / themeEn | Coraggio / Courage |
| keywordIt | Coraggio |
| scriptureReference | Giosuè 6 |

**intro**

```json
{
  "it": "Una città fortificata sembra impossibile da conquistare. Il piano di Dio chiede fiducia e pazienza.",
  "en": "A fortified city looks impossible to take. God’s plan asks for trust and patience."
}
```

**hintIt / solutionIt / engagementNoteIt**

- hintIt: Marce intorno alle mura, suoni di corni e un grido all’unisono.
- solutionIt: Israele ubbidì al piano di Geova: dopo le marce e il grido, le mura di Gerico caddero. Giosuè 6.
- engagementNoteIt: Il coraggio vero è obbedire anche quando il metodo sembra strano.

**scriptureQuoteIt** (**pending OK**)

> Giosuè 6:20 — *[parafrasi]* Al grido del popolo, le mura crollano perché Dio combatte per loro.  
> **Azione umana:** testo ufficiale da approvare.

**theaterQuote** (**pending OK**)

```json
{
  "it": "[BOZZA] Riferimento Giosuè 6:2 — La vittoria è dono di Dio, non solo forza umana.",
  "en": "[DRAFT] Reference Joshua 6:2 — Victory is God’s gift, not human strength alone."
}
```

**questions**

1. Come caddero le mura di Gerico?  
   - Seguendo il piano di Dio con fede (ok) / Con macchine da guerra immense (f) / Comprando la città (f)  
2. Cosa richiede il coraggio in questa storia?  
   - Fidarsi di Dio anche senza capire tutto (ok) / Agire da soli senza ascoltare (f) / Aspettare in silenzio per sempre (f)

**moral**

```json
{
  "it": "Il coraggio biblico cammina con l’obbedienza. Dio apre strade dove l’uomo vede solo muri.",
  "en": "Biblical courage walks with obedience. God opens ways where humans only see walls."
}
```

**PNG**

| Ruolo | Key |
|-------|-----|
| visible | `1F3DB`, `1F6B6-200D-2640-FE0F`, `1F3B6`, `1F4E3`, `1F463` |
| hidden | `1F3F0`, `203C` |
| hintKey | `1F3F0` |

**imageCaptionsIt**

1. Mura antiche  
2. Passi in processione  
3. Musica e corni  
4. Un annuncio forte  
5. Orme sul terreno  
6. Una fortezza che sembra eterna  
7. Un’esclamazione improvvisa  
8. La fortezza vista da fuori  

---

## Episodio 23 — Marta e Maria

| Campo | Valore |
|-------|--------|
| id | 23 |
| titleIt | Marta e Maria |
| titleEn | Martha and Mary |
| themeIt / themeEn | Priorità / Priorities |
| keywordIt | Priorità |
| scriptureReference | Luca 10:38-42 |

**intro**

```json
{
  "it": "In una casa ospitale due sorelle accolgono l’ospite in modi diversi. Una scelta rivela cosa conta di più.",
  "en": "In a welcoming home two sisters receive the guest in different ways. One choice shows what matters most."
}
```

**hintIt / solutionIt / engagementNoteIt**

- hintIt: Una cucina piena di impegni e una sorella seduta ad ascoltare.
- solutionIt: Maria scelse di ascoltare Gesù; Marta era distratta dai molti servizi. Gesù lodò la parte migliore. Luca 10:38-42.
- engagementNoteIt: Il servizio è buono, ma non deve soffocare l’ascolto della Parola.

**scriptureQuoteIt** (**pending OK**)

> Luca 10:42 — *[parafrasi]* C’è bisogno di poche cose, anzi di una sola: scegliere ciò che non sarà tolto.  
> **Azione umana:** testo ufficiale da approvare.

**theaterQuote** (**pending OK**)

```json
{
  "it": "[BOZZA] Riferimento Luca 10:42 — La parte migliore è restare vicini all’insegnamento.",
  "en": "[DRAFT] Reference Luke 10:42 — The better part is staying close to the teaching."
}
```

**questions**

1. Cosa scelse Maria mentre Marta era occupata?  
   - Ascoltare l’insegnamento (ok) / Uscire di casa (f) / Contare i soldi (f)  
2. Quale equilibrio ci invita questa storia?  
   - Servire senza dimenticare di ascoltare Dio (ok) / Solo lavoro, mai ascolto (f) / Solo riposo, mai servizio (f)

**moral**

```json
{
  "it": "Le priorità giuste mettono Dio al primo posto. Ascoltare la Parola nutre ogni altro servizio.",
  "en": "Right priorities put God first. Listening to the Word feeds every other service."
}
```

**PNG**

| Ruolo | Key |
|-------|-----|
| visible | `1F46D`, `1F3DB`, `1F52A`, `1F37A`, `1F442-1F3FE` |
| hidden | `1F4D6`, `1F932-1F3FD` |
| hintKey | `1F4D6` |

**imageCaptionsIt**

1. Due persone in casa  
2. Un luogo di accoglienza  
3. Attrezzi per preparare cibo  
4. Una tavola apparecchiata  
5. Orecchie attente  
6. Un libro aperto  
7. Qualcuno in atteggiamento raccolto  
8. Di nuovo il libro, al centro  

---

## Nuove PNG keys necessarie

**Nessuna obbligatoria** per le bozze 19–23: tutte le chiavi sopra esistono in `webapp/assets/`.

### Opzionali (qualità visiva — solo se OK umano)

| Chiave proposta | Concept (1 riga per `tools/photo_concepts.py`) | Ep |
|-----------------|------------------------------------------------|-----|
| `babel_tower_brick` | Torre antica in mattoni a vista, cielo chiaro, nessuna iscrizione | 19 |
| `jericho_ram_horn` | Corno di ariete stilizzato su sfondo neutro, no logo | 22 |
| `damascus_road_light` | Strada polverosa con bagliore bianco frontale, senza volti | 21 |

Se non approvate → restano le chiavi esistenti.

---

## Decisioni aperte per l’umano

1. **Citazioni tier 1/2:** approvare testo ufficiale (TNM o altra) per `theaterQuote` + `scriptureQuoteIt` di ogni episodio; sostituire i placeholder `[BOZZA]` / parafrasi.
2. **Temi:** Confermare le coppie Orgoglio / Fede / Conversione / Coraggio / Priorità (Coraggio già usato in catalogo esistente — ok riuso o preferisci sinonimo “Obbedienza coraggiosa”?).
3. **Ordine id 19–23:** tenere l’ordine candidati o riordinare per difficoltà/tema?
4. **`DECOR_SYMBOLS`:** scegliere 5 emoji intro (una per episodio) dopo merge — non in schema JSON.
5. **PNG opzionali:** creare i 3 concept sopra o shippare solo chiavi esistenti?
6. **Overlap tematici:** Ep 20 “Fede” vs storie fede già presenti; Ep 22 “Coraggio” vs episodi coraggio 1–18 — OK o rietichettare?
7. **Hint = hidden[0]:** pattern applicato a tutti e 5; preferisci hint distinto (come Eden `1F40D` ∉ hidden) per qualcuno?
8. **Merge gate:** dopo OK → solo `data/episodes.json` + DECOR; **non** hand-edit `stories.js` / `StoryLibrary.cs`.

---

## Checklist pre-merge (dopo OK)

- [ ] Citazioni approvate (niente placeholder)
- [ ] `python tools/validate_episodes.py data/episodes.json --full-catalog`
- [ ] `python tools/sync_all.py`
- [ ] `python tools/verify_episode_parity.py`
- [ ] Aggiornare `DECOR_SYMBOLS` (5 righe)
- [ ] Smoke web Ep 19–23 (1 rebus + 1 quiz ciascuno)

**Schema v2:** non richiesto.
