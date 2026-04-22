using System.Collections.Generic;

namespace Jw_Quiz_Development
{
    public static class StoryLibrary
    {
        public static IReadOnlyList<Story> Stories { get; } = BuildStories();

        private static List<Story> BuildStories()
        {
            var list = new List<Story>
            {
            // ── STORIE LEGACY 1-12 PREPARATE AL RENDERER DINAMICO (F2) ────────────
            // Nota: in Step 2 manteniamo IsDynamic=false. Il passaggio runtime avviene via feature flag in Forms_list.
            new Story
            {
                Id = 1,
                Title = "Il Giardino di Eden",
                ScriptureReference = "Genesi 2-3",
                Keyword = "Obbedienza",
                Hint = "Un serpente, un albero proibito e una scelta fatale.",
                Solution = "Adamo ed Eva disobbedirono a Geova mangiando il frutto proibito. Le conseguenze toccano l'intera umanita', ma Dio promise la salvezza: Genesi 3:15.",
                ScriptureQuote = "Geova Dio... comando' all'uomo: '...dell'albero della conoscenza del bene e del male non devi mangiare, poiche' nel giorno in cui ne mangerai positivamente morirai.' - Genesi 2:16-17 (TNM)",
                EngagementNote = "Sottolinea il valore dell'obbedienza e la promessa di redenzione gia' nell'Eden.",
                ImageResourceName = "Eden",
                IsDynamic = false,
                VisibleEmojis = new[] { "093-users", "1F333", "1F632", "1F457", "1F456" },
                HiddenEmojis = new[] { "1F34E", "1F480" },
                HintEmoji = "1F40D",
                ImageCaptions = new[]
                {
                    "I primi due esseri umani",
                    "L'albero al centro del giardino",
                    "Lo shock della scoperta",
                    "Vestiti che coprono la vergogna",
                    "Scarpe che camminano lontano dal Creatore",
                    "Il frutto proibito",
                    "La morte entro' nel mondo",
                    "Il serpente ingannatore"
                }
            },
            new Story
            {
                Id = 2,
                Title = "Sansone e Dalila",
                ScriptureReference = "Giudici 14-16",
                Keyword = "Fedelta'",
                Hint = "La vera forza non sta nei capelli, ma nell'alleanza con Dio.",
                Solution = "Sansone tradi' il suo voto nazireo cedendo a Dalila. Cieco e prigioniero, si penti' e Geova gli restitui' la forza per sconfiggere i Filistei: Giudici 16:28-30.",
                ScriptureQuote = "Sansone invoco' Geova e disse: 'Sovrano Signore Geova, ricordati ora di me... fammi vendicare dei Filistei per i miei due occhi.' - Giudici 16:28 (TNM)",
                EngagementNote = "Mostra come cedere ai desideri mondani possa indebolirci, e come il sincero pentimento apra la via al perdono di Dio.",
                ImageResourceName = "Samson",
                IsDynamic = false,
                VisibleEmojis = new[] { "1F4AA-1F3FD", "1F498", "1F487-1F3FC-200D-2642-FE0F", "1F440", "1F3DB" },
                HiddenEmojis = new[] { "159-dancer", "26D4" },
                HintEmoji = "203C",
                ImageCaptions = new[]
                {
                    "Forza sovrumana",
                    "Un amore pericoloso",
                    "La donna che taglia i capelli",
                    "Gli occhi del guerriero",
                    "Il tempio di Dagon",
                    "La danzatrice seduttrice",
                    "La fine della forza",
                    "Attenzione al tradimento!"
                }
            },
            new Story
            {
                Id = 3,
                Title = "Giona e il Pesce",
                ScriptureReference = "Giona 2-3",
                Keyword = "Misericordia",
                Hint = "Tre giorni nel buio insegnano cosa significa correre verso Dio, non lontano da Lui.",
                Solution = "Giona fuggi' dalla missione di Geova, ma dal ventre del pesce grido' a Lui. Dio lo libero' e Ninive si penti'. Giona 2:2.",
                ScriptureQuote = "Quando la mia vita si stava esaurendo, mi ricordai di Geova, e la mia preghiera entro' da te, nel tuo santo tempio. - Giona 2:7 (TNM)",
                EngagementNote = "Evidenzia la misericordia di Dio per i peccatori pentiti e l'importanza di accettare la missione assegnataci.",
                ImageResourceName = "Jonah",
                IsDynamic = false,
                VisibleEmojis = new[] { "26F5", "1F3CA-1F3FB-200D-2642-FE0F", "1F932-1F3FD", "1F3F0", "1F4E3" },
                HiddenEmojis = new[] { "1F433", "1F334" },
                HintEmoji = "2694",
                ImageCaptions = new[]
                {
                    "Una nave verso Tarsis",
                    "Un uomo gettato in mare",
                    "Preghiera dal profondo",
                    "La grande citta' di Ninive",
                    "Il messaggio proclamato",
                    "Il grande pesce",
                    "La pianta di ricino",
                    "La battaglia spirituale"
                }
            },
            new Story
            {
                Id = 4,
                Title = "Le Pecore e le Capre",
                ScriptureReference = "Matteo 25:31-33; Rivelazione 15:3",
                Keyword = "Giudizio",
                Hint = "Il Re separa il gregge: a destra vita eterna, a sinistra distruzione eterna.",
                Solution = "Gesu' insegno' che al momento del giudizio finale sara' separato chi ha dimostrato amore e fedelta' (pecore) da chi non lo ha fatto (capre). Matteo 25:46.",
                ScriptureQuote = "Quando il Figlio dell'uomo arrivera' nella sua gloria... separera' gli uni dagli altri, come il pastore separa le pecore dalle capre. - Matteo 25:31-32 (TNM)",
                EngagementNote = "Collega il giudizio finale all'amore pratico verso Dio e il prossimo.",
                ImageResourceName = "SheepGoats",
                IsDynamic = false,
                VisibleEmojis = new[] { "1f411-w", "1f4112", "1F3B6", "1F1EE-1F1F1", "1F451" },
                HiddenEmojis = new[] { "139-man", "1F3B8" },
                HintEmoji = "1F468-1F3FD-200D-2696-FE0F",
                ImageCaptions = new[]
                {
                    "Le pecore alla destra del Re",
                    "Le capre alla sinistra",
                    "Il canto della vittoria",
                    "La bandiera d'Israele",
                    "La corona del Regno",
                    "Il Figlio dell'uomo glorificato",
                    "L'arpa del giudizio",
                    "Il Giudice giusto"
                }
            },
            new Story
            {
                Id = 5,
                Title = "Le 10 Piaghe d'Egitto",
                ScriptureReference = "Esodo 7-12",
                Keyword = "Potere di Dio",
                Hint = "Dieci segni celesti provano che nessun dio egiziano puo' competere col Creatore.",
                Solution = "Attraverso dieci piaghe devastanti, Geova mostro' la Sua potenza superiore alle divinita' d'Egitto. Esodo 12:12.",
                ScriptureQuote = "E certamente gli Egiziani sapranno che io sono Geova quando stendero' la mia mano contro l'Egitto. - Esodo 7:5 (TNM)",
                EngagementNote = "Usa le piaghe per illustrare che Geova agisce nella storia e difende il Suo nome e il Suo popolo.",
                ImageResourceName = "Plagues",
                IsDynamic = false,
                VisibleEmojis = new[] { "1F51F", "1F438", "1F463", "1F480", "1F193" },
                HiddenEmojis = new[] { "1F318", "1F997" },
                HintEmoji = "1FA78",
                ImageCaptions = new[]
                {
                    "Il segno della fine",
                    "Rane su tutta la terra",
                    "Le orme nella polvere diventata zanzare",
                    "La morte dei primogeniti",
                    "GRATUITO: la liberazione di Dio",
                    "Le tenebre su tutta la terra",
                    "Le cavallette divorano tutto",
                    "La goccia di sangue nel Nilo"
                }
            },
            new Story
            {
                Id = 6,
                Title = "Elia e la Siccita'",
                ScriptureReference = "1 Re 16-18; Giacomo 5:16-18",
                Keyword = "Preghiera",
                Hint = "Un profeta solo affronta 850 sacerdoti pagani sulla cima del Monte Carmelo.",
                Solution = "Elia prego' con fede e Geova invio' fuoco dal cielo consumando il sacrificio, poi apri' i cieli dopo tre anni e mezzo di siccita'. 1 Re 18:38.",
                ScriptureQuote = "Se Geova e' il vero Dio, seguitelo; ma se e' Baal, seguite lui! - 1 Re 18:21 (TNM)",
                EngagementNote = "Sottolinea la forza della preghiera sincera: anche un uomo solo, unito a Dio, puo' cambiare il corso degli eventi.",
                ImageResourceName = "Elijah",
                IsDynamic = false,
                VisibleEmojis = new[] { "270B-1F3FD", "2614", "1F525", "2601", "1F327" },
                HiddenEmojis = new[] { "1F932-1F3FC", "1F404" },
                HintEmoji = "1F6BE",
                ImageCaptions = new[]
                {
                    "La mano alzata del profeta",
                    "Nessuna pioggia per anni",
                    "Il fuoco che consuma il sacrificio",
                    "Le nuvole cominciano a radunarsi",
                    "La pioggia torrenziale",
                    "Il profeta in preghiera",
                    "Il toro sacrificale",
                    "L'acqua versata sull'altare"
                }
            },
            new Story
            {
                Id = 7,
                Title = "Ester Salva il Popolo",
                ScriptureReference = "Ester 4-9",
                Keyword = "Coraggio",
                Hint = "Una regina rischia la vita presentandosi al re senza essere stata convocata.",
                Solution = "Ester si presento' coraggiosamente al re Assuero e smaschero' il malvagio piano di Aman. Ester 4:14.",
                ScriptureQuote = "E se devo perire, periro'! - Ester 4:16 (TNM)",
                EngagementNote = "Mostra come il coraggio ispirato da Dio permette alle persone di agire a favore del prossimo.",
                ImageResourceName = "Esther",
                IsDynamic = false,
                VisibleEmojis = new[] { "1F932-1F3FD", "1F6BC", "1F4E3", "1F442-1F3FE", "093-users" },
                HiddenEmojis = new[] { "1F634", "1F632" },
                HintEmoji = "2694",
                ImageCaptions = new[]
                {
                    "La regina Ester",
                    "Il simbolo del bambino (il futuro del popolo)",
                    "La lettera sigillata",
                    "L'orecchio che ascolta il complotto",
                    "Il popolo persiano",
                    "Il sonno del re disturbato",
                    "Lo shock di Aman smascherato",
                    "La battaglia per la giustizia"
                }
            },
            new Story
            {
                Id = 8,
                Title = "Abramo e Isacco al Monte Moria",
                ScriptureReference = "Genesi 21-22",
                Keyword = "Fede",
                Hint = "Un padre pronto a offrire il suo amato figlio dimostra la fede piu' grande.",
                Solution = "Abramo obbedi' a Geova portando Isacco sull'altare. Un angelo lo fermo' e Dio provvide un ariete. Genesi 22:18.",
                ScriptureQuote = "Per mezzo della tua discendenza tutte le nazioni della terra si benediranno certamente, perche' hai ascoltato la mia voce. - Genesi 22:18 (TNM)",
                EngagementNote = "Evidenzia come questa storia prefiguri il sacrificio di Cristo.",
                ImageResourceName = "Abraham",
                IsDynamic = false,
                VisibleEmojis = new[] { "1F473-200D-2642-FE0F", "1F6B8", "1F52A", "1F5FB", "1f4112" },
                HiddenEmojis = new[] { "1F42A", "094-user" },
                HintEmoji = "270B-1F3FD",
                ImageCaptions = new[]
                {
                    "Il patriarca Abramo",
                    "Il viaggio verso il monte",
                    "Il coltello del sacrificio",
                    "Il monte Moria",
                    "L'ariete provveduto",
                    "Il cammello del viaggio",
                    "Isacco, il figlio promesso",
                    "La mano di Dio si rivela"
                }
            },
            new Story
            {
                Id = 9,
                Title = "Il Figlio Prodigo",
                ScriptureReference = "Luca 15:11-32",
                Keyword = "Perdono",
                Hint = "Il padre vede il figlio da lontano e corre ad abbracciarlo.",
                Solution = "Il figlio minore dilapido' tutto, torno' pentito e il padre lo accolse con gioia immensa. Luca 15:24.",
                ScriptureQuote = "Questo mio figlio era morto ed e' tornato in vita; era perduto ed e' stato ritrovato. - Luca 15:24 (TNM)",
                EngagementNote = "Illustra che Dio accoglie sempre i cuori sinceramente pentiti.",
                ImageResourceName = "ProdigalSon",
                IsDynamic = false,
                VisibleEmojis = new[] { "038-boy-1", "036-man-1", "1F37A", "1F416", "1F416" },
                HiddenEmojis = new[] { "1F4B0", "1F629" },
                HintEmoji = "1F3B6",
                ImageCaptions = new[]
                {
                    "Il figlio minore",
                    "Il padre anziano",
                    "La vita dissoluta",
                    "I maiali che mangia il cibo dei porci",
                    "I maiali che custodisce",
                    "Il denaro sprecato",
                    "Il pianto del pentimento",
                    "La festa del ritorno"
                }
            },
            new Story
            {
                Id = 10,
                Title = "La Profezia della Pace di Isaia",
                ScriptureReference = "Isaia 11:7-11",
                Keyword = "Profezia",
                Hint = "Lupo e agnello, leone e vitello - pace tra animali che ora si combattono.",
                Solution = "Isaia profetizzo' una nuova era di pace paradisiaca sotto il governo del Messia. Isaia 11:6-9.",
                ScriptureQuote = "Il lupo risiedera' temporaneamente con l'agnello... e un semplice ragazzino li condurra'. - Isaia 11:6 (TNM)",
                EngagementNote = "Collega la profezia di Isaia al futuro paradiso terreno che Geova promette.",
                ImageResourceName = "Isaiah",
                IsDynamic = false,
                VisibleEmojis = new[] { "1F932-1F3FC", "1F51A", "2935", "1F199", "1F451" },
                HiddenEmojis = new[] { "1F981", "1F5FA" },
                HintEmoji = "1F5FA",
                ImageCaptions = new[]
                {
                    "Una donna in preghiera",
                    "La fine della violenza",
                    "La freccia che diventa inutile",
                    "OK - tutto va bene nel nuovo mondo",
                    "La corona del re messianico",
                    "Il leone che dimora con l'agnello",
                    "La mappa del piano di Dio",
                    "Un percorso verso la pace promessa"
                }
            },
            new Story
            {
                Id = 11,
                Title = "Noe' e il Diluvio",
                ScriptureReference = "Genesi 6-8",
                Keyword = "Salvezza",
                Hint = "Un uomo giusto costruisce una grande arca mentre il mondo lo deride.",
                Solution = "Noe' obbedi' a Geova costruendo l'arca ed entro' con la sua famiglia. Genesi 6:22.",
                ScriptureQuote = "Noe' fece secondo tutto cio' che Dio gli aveva comandato. Fece proprio cosi'. - Genesi 6:22 (TNM)",
                EngagementNote = "Noe' e' simbolo di salvezza attraverso l'obbedienza.",
                ImageResourceName = "Noah",
                IsDynamic = false,
                VisibleEmojis = new[] { "1F6A2", "1F404", "1F413", "1F327", "1F308" },
                HiddenEmojis = new[] { "1F404", "1F413" },
                HintEmoji = "1F30A",
                ImageCaptions = new[]
                {
                    "L'arca di salvezza",
                    "Le mucche a coppie",
                    "I polli a coppie",
                    "La pioggia torrenziale per 40 giorni",
                    "L'arcobaleno del patto",
                    "Altre mucche (coppia per coppia)",
                    "Altri polli (coppia per coppia)",
                    "Le acque che coprono la terra"
                }
            },
            new Story
            {
                Id = 12,
                Title = "Filippo e l'Eunuco Etiope",
                ScriptureReference = "Atti 8:26-40",
                Keyword = "Buona Novella",
                Hint = "Un carro nel deserto, un rotolo di Isaia e un incontro guidato dagli angeli.",
                Solution = "Lo spirito di Dio guido' Filippo verso il carro dell'eunuco. Filippo spiego' la buona novella e l'eunuco chiese il battesimo. Atti 8:36.",
                ScriptureQuote = "Filippo... gli dichiaro' la buona notizia intorno a Gesu'. - Atti 8:35 (TNM)",
                EngagementNote = "Mostra come Geova apre le porte per la predicazione e prepara cuori pronti.",
                ImageResourceName = "Philip",
                IsDynamic = false,
                VisibleEmojis = new[] { "1F47C", "2753", "1F30A", "1F4D6", "Hackney-100" },
                HiddenEmojis = new[] { "203C", "1F4E3" },
                HintEmoji = "203C",
                ImageCaptions = new[]
                {
                    "Un angelo guida Filippo",
                    "Una domanda: cosa significa?",
                    "L'acqua per il battesimo",
                    "Il rotolo di Isaia",
                    "Il carro etiope",
                    "Una conversazione guidata",
                    "Una decisione immediata",
                    "La buona notizia condivisa"
                }
            },

            // ── EPISODI DINAMICI (ID 13-18) ──────────────────────────────────────
            // VisibleEmojis/HiddenEmojis/HintEmoji contengono chiavi di risorsa PNG (nome file senza estensione)
            // ImageCaptions[0-4] = slot visibili, [5-6] = slot nascosti, [7] = indizio
            new Story
            {
                Id = 13,
                Title = "Davide e Golia",
                ScriptureReference = "1 Samuele 17",
                Keyword = "Coraggio",
                Hint = "Un giovane pastore sconfigge un guerriero gigante con la fede in Geova.",
                Solution = "Davide disse: 'Geova salvera', perche' questa e' la sua battaglia!' Con una fionda e una pietra, abbatte' Golia. 1 Samuele 17:45-47.",
                ScriptureQuote = "«...tutta questa assemblea sapra' che Geova non salva per mezzo di spada e lancia; perche' la battaglia appartiene a Geova, ed egli vi consegnera' nelle nostre mani.» — 1 Samuele 17:47 (TNM)",
                EngagementNote = "Con Geova, anche i giganti cadono.",
                ImageResourceName = "DavidGoliath",
                IsDynamic = true,
                VisibleEmojis = new[] { "038-boy-1", "1F411", "2694", "1F632", "2753" },
                HiddenEmojis = new[] { "1F480", "1F451" },
                HintEmoji = "1F4AA-1F3FD",
                ImageCaptions = new[] {
                    "Un giovane pastore",
                    "Un piccolo gregge",
                    "Armi e combattimento",
                    "Paura nel campo",
                    "Qualcosa manca in questa storia...",
                    "La caduta del nemico",         // nascosto 1
                    "Una vittoria inattesa",        // nascosto 2
                    "Una forza piu' grande delle apparenze" // indizio
                }
            },
            new Story
            {
                Id = 14,
                Title = "Giuseppe Perdona i Fratelli",
                ScriptureReference = "Genesi 45",
                Keyword = "Perdono",
                Hint = "Anni di schiavitu' e prigione nascondevano un piano di Dio.",
                Solution = "Giuseppe si fece conoscere ai fratelli in lacrime: 'Non sono io che vi ho mandato qui, ma Dio!' Genesi 45:5-7.",
                ScriptureQuote = "«...non siete stati voi a mandarmi qui, ma e' Dio; e mi ha posto come padre per il faraone e come signore di tutta la sua casa e come governante su tutto il paese d'Egitto.» — Genesi 45:8 (TNM)",
                EngagementNote = "Anche le ingiustizie piu' dure possono far parte del piano di Dio.",
                ImageResourceName = "Joseph",
                IsDynamic = true,
                VisibleEmojis = new[] { "1F468-1F3FB-200D-1F33E", "1F42A", "1F4B0", "1F629", "1F632" },
                HiddenEmojis = new[] { "1F46D", "1F498" },
                HintEmoji = "1F4D6",
                ImageCaptions = new[] {
                    "Un uomo con grande autorita'",
                    "Il viaggio nel deserto",
                    "Argento e commercio",
                    "Anni di sofferenza",
                    "Un incontro inatteso",
                    "Un abbraccio ritrovato",       // nascosto 1
                    "Un legame ricostruito",        // nascosto 2
                    "Una svolta guidata da Dio"     // indizio
                }
            },
            new Story
            {
                Id = 15,
                Title = "Rut e Boaz",
                ScriptureReference = "Rut 1-4",
                Keyword = "Devozione",
                Hint = "Una vedova straniera scelse di seguire il Dio della suocera.",
                Solution = "Rut dicharo': 'Il tuo popolo sara' il mio popolo, il tuo Dio il mio Dio.' Boaz la sposo' ed ella divenne antenata del Messia. Rut 1:16.",
                ScriptureQuote = "«Dove tu andrai, io andro', e dove tu passerai la notte, anch'io passero' la notte. Il tuo popolo sara' il mio popolo e il tuo Dio sara' il mio Dio.» — Rut 1:16 (TNM)",
                EngagementNote = "La lealta' a Geova va oltre le frontiere etniche e culturali.",
                ImageResourceName = "Ruth",
                IsDynamic = true,
                VisibleEmojis = new[] { "1F6B6-200D-2640-FE0F", "094-user", "1F468-1F3FB-200D-1F33E", "036-man-1", "1F498" },
                HiddenEmojis = new[] { "1F4B0", "039-baby" },
                HintEmoji = "1F411",
                ImageCaptions = new[] {
                    "Una donna in cammino",
                    "Una figura di famiglia",
                    "Un campo di raccolta",
                    "Un uomo con un ruolo decisivo",
                    "Un legame che nasce",
                    "Un diritto di riscatto",       // nascosto 1
                    "Una discendenza benedetta",    // nascosto 2
                    "Scegliere il popolo di Geova"  // indizio
                }
            },
            new Story
            {
                Id = 16,
                Title = "La Nascita di Mose'",
                ScriptureReference = "Esodo 2:1-10",
                Keyword = "Protezione",
                Hint = "Una madre intreccia una cesta di giunchi per salvare il suo bambino dal Nilo.",
                Solution = "La madre di Mose' lo mise in una cesta e la affido' al Nilo. La figlia del faraone lo salvo' e Mose' crebbe a palazzo. Esodo 2:1-10.",
                ScriptureQuote = "«Ma quando non pote' nasconderlo piu' a lungo, prese per lui una cesta di papiro e la spalmò di asfalto e di pece; poi vi pose il bambino e mise la cesta tra i canneti lungo la sponda del fiume Nilo.» — Esodo 2:3 (TNM)",
                EngagementNote = "Geova usa anche le circostanze piu' disperate per proteggere i Suoi servitori.",
                ImageResourceName = "Moses",
                IsDynamic = true,
                VisibleEmojis = new[] { "039-baby", "1F30A", "1F3F0", "1F451", "1F333" },
                HiddenEmojis = new[] { "1F932-1F3FC", "1F47C" },
                HintEmoji = "1F440",
                ImageCaptions = new[] {
                    "Un neonato da proteggere",
                    "Acqua in movimento",
                    "Un luogo di potere",
                    "Una donna di alto rango",
                    "Piante lungo la riva",
                    "Le mani che lo tengono al sicuro",  // nascosto 1
                    "Protetto in modo inatteso",         // nascosto 2
                    "Uno sguardo vigile dall'alto"       // indizio
                }
            },
            new Story
            {
                Id = 17,
                Title = "Anna e Samuele",
                ScriptureReference = "1 Samuele 1",
                Keyword = "Preghiera",
                Hint = "Una donna in lacrime al tempio prega con tale fervore da sembrare ubriaca.",
                Solution = "Anna prego' Geova con tutto il cuore promettendo di consacrare il figlio. Geova la ascolto': nacque Samuele. 1 Samuele 1:27.",
                ScriptureQuote = "«Per questo ragazzo ho pregato, e Geova ha concesso la mia richiesta che gli ho fatto.» — 1 Samuele 1:27 (TNM)",
                EngagementNote = "La preghiera sincera viene sempre ascoltata da Geova.",
                ImageResourceName = "Samuel",
                IsDynamic = true,
                VisibleEmojis = new[] { "1F932-1F3FC", "1F629", "1F3DB", "1F3B6", "1F5FA" },
                HiddenEmojis = new[] { "039-baby", "1F4D6" },
                HintEmoji = "203C",
                ImageCaptions = new[] {
                    "Una preghiera intensa",
                    "Dolore interiore",
                    "Un luogo di adorazione",
                    "Un canto di gratitudine",
                    "Un cammino di fede",
                    "Un dono atteso a lungo",              // nascosto 1
                    "Una vita dedicata a Geova",           // nascosto 2
                    "Una richiesta ascoltata"              // indizio
                }
            },
            new Story
            {
                Id = 18,
                Title = "Il Buon Samaritano",
                ScriptureReference = "Luca 10:30-37",
                Keyword = "Amore per il Prossimo",
                Hint = "Sacerdote e levita passarono oltre. Solo uno straniero si fermo'.",
                Solution = "Gesu' racconta come un Samaritano soccorse un uomo abbandonato. Luca 10:36-37 — chi mostro' misericordia fu il vero prossimo.",
                ScriptureQuote = "«Gesù gli disse: 'Va' e fa' anche tu la stessa cosa.'» — Luca 10:37 (TNM)",
                EngagementNote = "L'amore per il prossimo non ha confini etnici o religiosi.",
                ImageResourceName = "Samaritan",
                IsDynamic = true,
                VisibleEmojis = new[] { "1F6B6-1F3FF-200D-2642-FE0F", "1F4AA-1F3FD", "Hackney-100", "1F4B0", "2753" },
                HiddenEmojis = new[] { "26D4", "1F498" },
                HintEmoji = "1F440",
                ImageCaptions = new[] {
                    "Un uomo ferito lungo la strada",
                    "La violenza dei briganti",
                    "Un animale da viaggio",
                    "Un pagamento per le cure",
                    "Chi si fermera' ad aiutare?",
                    "Chi scelse di tirare dritto",    // nascosto 1
                    "Un gesto di misericordia",       // nascosto 2
                    "Conta chi si ferma ad aiutare"   // indizio
                }
            }
            };
            return list;
        }
    }
}