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
            new Story
            {
                Id = 13,
                Title = "Davide e Golia",
                ScriptureReference = "1 Samuele 17",
                Keyword = "Coraggio",
                Hint = "Un giovane pastore sconfigge un guerriero gigante con la fede in Geova.",
                Solution = "Davide disse: 'Geova salvera', perche' questa e' la sua battaglia!' Con una fionda e una pietra, abbatte' Golia. 1 Samuele 17:45-47.",
                EngagementNote = "Con Geova, anche i giganti cadono.",
                ImageResourceName = "DavidGoliath",
                IsDynamic = true,
                VisibleEmojis = new[] { "\U0001F466", "\U0001F411", "\u2694\uFE0F", "\U0001F632", "\u2753" },
                HiddenEmojis = new[] { "\U0001F3C6", "\U0001F480" },
                HintEmoji = "\U0001FAA8"
            },
            new Story
            {
                Id = 14,
                Title = "Giuseppe Perdona i Fratelli",
                ScriptureReference = "Genesi 45",
                Keyword = "Perdono",
                Hint = "Anni di schiavitu' e prigione nascondevano un piano di Dio.",
                Solution = "Giuseppe si fece conoscere ai fratelli in lacrime: 'Non sono io che vi ho mandato qui, ma Dio!' Genesi 45:5-7.",
                EngagementNote = "Anche le ingiustizie piu' dure possono far parte del piano di Dio.",
                ImageResourceName = "Joseph",
                IsDynamic = true,
                VisibleEmojis = new[] { "\U0001F33E", "\U0001F42A", "\U0001F4B0", "\U0001F629", "\U0001F622" },
                HiddenEmojis = new[] { "\U0001F91D", "\U0001F496" },
                HintEmoji = "\U0001F38B"
            },
            new Story
            {
                Id = 15,
                Title = "Rut e Boaz",
                ScriptureReference = "Rut 1-4",
                Keyword = "Devozione",
                Hint = "Una vedova straniera scelse di seguire il Dio della suocera.",
                Solution = "Rut dicharo': 'Il tuo popolo sara' il mio popolo, il tuo Dio il mio Dio.' Boaz la sposo' ed ella divenne antenata del Messia. Rut 1:16.",
                EngagementNote = "La lealta' a Geova va oltre le frontiere etniche e culturali.",
                ImageResourceName = "Ruth",
                IsDynamic = true,
                VisibleEmojis = new[] { "\U0001F6B6\u200D\u2640\uFE0F", "\U0001F475", "\U0001F33E", "\U0001F468", "\U0001F496" },
                HiddenEmojis = new[] { "\U0001F4B0", "\U0001F476" },
                HintEmoji = "\U0001F411"
            },
            new Story
            {
                Id = 16,
                Title = "La Nascita di Mose'",
                ScriptureReference = "Esodo 2:1-10",
                Keyword = "Protezione",
                Hint = "Una madre intreccia una cesta di giunchi per salvare il suo bambino dal Nilo.",
                Solution = "La madre di Mose' lo mise in una cesta e la affido' al Nilo. La figlia del faraone lo salvo' e Mose' crebbe a palazzo. Esodo 2:1-10.",
                EngagementNote = "Geova usa anche le circostanze piu' disperate per proteggere i Suoi servitori.",
                ImageResourceName = "Moses",
                IsDynamic = true,
                VisibleEmojis = new[] { "\U0001F476", "\U0001F30A", "\U0001F3F0", "\U0001F451", "\U0001F33F" },
                HiddenEmojis = new[] { "\U0001F932", "\U0001F47C" },
                HintEmoji = "\U0001F440"
            },
            new Story
            {
                Id = 17,
                Title = "Anna e Samuele",
                ScriptureReference = "1 Samuele 1",
                Keyword = "Preghiera",
                Hint = "Una donna in lacrime al tempio prega con tale fervore da sembrare ubriaca.",
                Solution = "Anna prego' Geova con tutto il cuore prometiendo di consacrare il figlio. Geova la ascolto': nacque Samuele. 1 Samuele 1:27.",
                EngagementNote = "La preghiera sincera viene sempre ascoltata da Geova.",
                ImageResourceName = "Samuel",
                IsDynamic = true,
                VisibleEmojis = new[] { "\U0001F932", "\U0001F622", "\U0001F3DB\uFE0F", "\U0001F3B6", "\U0001F5FA\uFE0F" },
                HiddenEmojis = new[] { "\U0001F476", "\U0001F4D6" },
                HintEmoji = "\u203C\uFE0F"
            },
            new Story
            {
                Id = 18,
                Title = "Il Buon Samaritano",
                ScriptureReference = "Luca 10:30-37",
                Keyword = "Amore per il Prossimo",
                Hint = "Sacerdote e levita passarono oltre. Solo uno straniero si fermo'.",
                Solution = "Gesu' racconta come un Samaritano soccorse un uomo abbandonato. Luca 10:36-37 — chi mostro' misericordia fu il vero prossimo.",
                EngagementNote = "L'amore per il prossimo non ha confini etnici o religiosi.",
                ImageResourceName = "Samaritan",
                IsDynamic = true,
                VisibleEmojis = new[] { "\U0001F6B6", "\U0001F9B8", "\U0001F434", "\U0001F4AA", "\U0001F4B0" },
                HiddenEmojis = new[] { "\u26D4", "\U0001F496" },
                HintEmoji = "\U0001F440"
            }
            };
            return list;
        }
    }
}