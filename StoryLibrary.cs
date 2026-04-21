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
            // ── STORIE STATICHE (ID 1-12) ─────────────────────────────────────────
            new Story
            {
                Id = 1,
                Title = "Il Giardino di Eden",
                ScriptureReference = "Genesi 2-3",
                Keyword = "Obbedienza",
                Hint = "Un serpente, un albero proibito e una scelta fatale.",
                Solution = "Adamo ed Eva disobbedirono a Geova mangiando il frutto proibito. Le conseguenze toccano l'intera umanita', ma Dio promise la salvezza: Genesi 3:15.",
                EngagementNote = "Sottolinea il valore dell'obbedienza e la promessa di redenzione gia' nell'Eden.",
                ImageResourceName = "Eden",
                IsDynamic = false
            },
            new Story
            {
                Id = 2,
                Title = "Sansone e Dalila",
                ScriptureReference = "Giudici 14-16",
                Keyword = "Fedelta'",
                Hint = "La vera forza non sta nei capelli, ma nell'alleanza con Dio.",
                Solution = "Sansone tradì il suo voto nazireo cedendo a Dalila. Cieco e prigioniero, si pentì e Geova gli restitui' la forza per sconfiggere i Filistei: Giudici 16:28-30.",
                EngagementNote = "Mostra come cedere ai desideri mondani possa indebolirci, e come il sincero pentimento apra la via al perdono di Dio.",
                ImageResourceName = "Samson",
                IsDynamic = false
            },
            new Story
            {
                Id = 3,
                Title = "Giona e il Pesce",
                ScriptureReference = "Giona 2-3",
                Keyword = "Misericordia",
                Hint = "Tre giorni nel buio insegnano cosa significa correre verso Dio, non lontano da Lui.",
                Solution = "Giona fuggi' dalla missione di Geova, ma dal ventre del pesce grido' a Lui. Dio lo libero' e Ninive si pentì. Giona 2:2.",
                EngagementNote = "Evidenzia la misericordia di Dio per i peccatori pentiti e l'importanza di accettare la missione assegnataci.",
                ImageResourceName = "Jonah",
                IsDynamic = false
            },
            new Story
            {
                Id = 4,
                Title = "Le Pecore e le Capre",
                ScriptureReference = "Matteo 25:31-33; Rivelazione 15:3",
                Keyword = "Giudizio",
                Hint = "Il Re separa il gregge: a destra vita eterna, a sinistra distruzione eterna.",
                Solution = "Gesu' insegno' che al momento del giudizio finale sara' separato chi ha dimostrato amore e fedelta' (pecore) da chi non lo ha fatto (capre). Matteo 25:46.",
                EngagementNote = "Collega il giudizio finale all'amore pratico verso Dio e il prossimo.",
                ImageResourceName = "SheepGoats",
                IsDynamic = false
            },
            new Story
            {
                Id = 5,
                Title = "Le 10 Piaghe d'Egitto",
                ScriptureReference = "Esodo 7-12",
                Keyword = "Potere di Dio",
                Hint = "Dieci segni celesti provano che nessun dio egiziano puo' competere col Creatore.",
                Solution = "Attraverso dieci piaghe devastanti, Geova mostro' la Sua potenza superiore alle divinita' d'Egitto. Esodo 12:12.",
                EngagementNote = "Usa le piaghe per illustrare che Geova agisce nella storia e difende il Suo nome e il Suo popolo.",
                ImageResourceName = "Plagues",
                IsDynamic = false
            },
            new Story
            {
                Id = 6,
                Title = "Elia e la Siccita'",
                ScriptureReference = "1 Re 16-18; Giacomo 5:16-18",
                Keyword = "Preghiera",
                Hint = "Un profeta solo affronta 850 sacerdoti pagani sulla cima del Monte Carmelo.",
                Solution = "Elia prego' con fede e Geova invio' fuoco dal cielo consumando il sacrificio, poi apri' i cieli dopo tre anni e mezzo di siccita'. 1 Re 18:38.",
                EngagementNote = "Sottolinea la forza della preghiera sincera: anche un uomo solo, unito a Dio, puo' cambiare il corso degli eventi.",
                ImageResourceName = "Elijah",
                IsDynamic = false
            },
            new Story
            {
                Id = 7,
                Title = "Ester Salva il Popolo",
                ScriptureReference = "Ester 4-9",
                Keyword = "Coraggio",
                Hint = "Una regina rischia la vita presentandosi al re senza essere stata convocata.",
                Solution = "Ester si presento' coraggiosamente al re Assuero e smascero' il malvagio piano di Aman. Ester 4:14.",
                EngagementNote = "Mostra come il coraggio ispirato da Dio permette alle persone di agire a favore del prossimo.",
                ImageResourceName = "Esther",
                IsDynamic = false
            },
            new Story
            {
                Id = 8,
                Title = "Abramo e Isacco al Monte Moria",
                ScriptureReference = "Genesi 21-22",
                Keyword = "Fede",
                Hint = "Un padre pronto a offrire il suo amato figlio dimostra la fede piu' grande.",
                Solution = "Abramo obbedi' a Geova portando Isacco sull'altare. Un angelo lo fermo' e Dio provvide un ariete. Genesi 22:18.",
                EngagementNote = "Evidenzia come questa storia prefiguri il sacrificio di Cristo.",
                ImageResourceName = "Abraham",
                IsDynamic = false
            },
            new Story
            {
                Id = 9,
                Title = "Il Figlio Prodigo",
                ScriptureReference = "Luca 15:11-32",
                Keyword = "Perdono",
                Hint = "Il padre vede il figlio da lontano e corre ad abbracciarlo.",
                Solution = "Il figlio minore dilapido' tutto, torno' pentito e il padre lo accolse con gioia immensa. Luca 15:24.",
                EngagementNote = "Illustra che Dio accoglie sempre i cuori sinceramente pentiti.",
                ImageResourceName = "ProdigalSon",
                IsDynamic = false
            },
            new Story
            {
                Id = 10,
                Title = "La Profezia della Pace di Isaia",
                ScriptureReference = "Isaia 11:7-11",
                Keyword = "Profezia",
                Hint = "Lupo e agnello, leone e vitello — pace tra animali che ora si combattono.",
                Solution = "Isaia profetizo' una nuova era di pace paradisiaca sotto il governo del Messia. Isaia 11:6-9.",
                EngagementNote = "Collega la profezia di Isaia al futuro paradiso terreno che Geova promette.",
                ImageResourceName = "Isaiah",
                IsDynamic = false
            },
            new Story
            {
                Id = 11,
                Title = "Noe' e il Diluvio",
                ScriptureReference = "Genesi 6-8",
                Keyword = "Salvezza",
                Hint = "Un uomo giusto costruisce una grande arca mentre il mondo lo deride.",
                Solution = "Noe' obbedi' a Geova costruendo l'arca ed entro' con la sua famiglia. Genesi 6:22.",
                EngagementNote = "Noe' e' simbolo di salvezza attraverso l'obbedienza.",
                ImageResourceName = "Noah",
                IsDynamic = false
            },
            new Story
            {
                Id = 12,
                Title = "Filippo e l'Eunuco Etiope",
                ScriptureReference = "Atti 8:26-40",
                Keyword = "Buona Novella",
                Hint = "Un carro nel deserto, un rotolo di Isaia e un incontro guidato dagli angeli.",
                Solution = "Lo spirito di Dio guido' Filippo verso il carro dell'eunuco. Filippo spiego' la buona novella e l'eunuco chiese il battesimo. Atti 8:36.",
                EngagementNote = "Mostra come Geova apre le porte per la predicazione e prepara cuori pronti.",
                ImageResourceName = "Philip",
                IsDynamic = false
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
                    "Un giovane pastore di Betlemme",
                    "Le pecore al suo pascolo",
                    "Armi e combattimento",
                    "Paura e sorpresa nel campo",
                    "Qualcosa manca in questa storia...",
                    "Il destino del gigante",      // nascosto 1
                    "La vittoria e il regno",       // nascosto 2
                    "La forza che viene da Geova"   // indizio
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
                    "Un governatore d'Egitto",
                    "Il viaggio nel deserto",
                    "Argento e commercio",
                    "Anni di sofferenza",
                    "Un riconoscimento inaspettato",
                    "I fratelli riconciliati",       // nascosto 1
                    "L'amore fraterno restaurato",  // nascosto 2
                    "Un messaggio di speranza"       // indizio
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
                    "Una donna che cammina verso l'ignoto",
                    "La suocera Naomi",
                    "Il campo di grano",
                    "Il parente redentore",
                    "Un amore nascente",
                    "Il riscatto del terreno",      // nascosto 1
                    "Il frutto della devozione",    // nascosto 2
                    "Il popolo di Geova"            // indizio
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
                    "Un neonato da salvare",
                    "Il fiume Nilo",
                    "Il palazzo del faraone",
                    "La figlia del faraone",
                    "I giunchi della riva del Nilo",
                    "Le mani che lo tengono al sicuro",  // nascosto 1
                    "Protetto come un dono celeste",     // nascosto 2
                    "Lo sguardo vigilante di Geova"      // indizio
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
                    "Una preghiera fervente al Signore",
                    "Lacrime e tormento interiore",
                    "Il santuario di Silo",
                    "Un canto di lode a Geova",
                    "Un percorso di fede",
                    "Il dono tanto atteso",                // nascosto 1
                    "Una vita consacrata alla Parola",     // nascosto 2
                    "Geova ha ascoltato!"                  // indizio
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
                    "Un uomo derubato e abbandonato",
                    "La violenza dei briganti",
                    "L'asino del Samaritano",
                    "Denari lasciati per le cure",
                    "Chi si fermerà ad aiutare?",
                    "Chi passò oltre senza fermarsi",  // nascosto 1
                    "L'amore per il prossimo",         // nascosto 2
                    "Geova vede chi mostra misericordia" // indizio
                }
            }
            };
            return list;
        }
    }
}