using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Jw_Quiz_Development
{
    public interface IStoryTranslationEngine
    {
        StoryLocalizedText Translate(StoryLocalizedText source, AppLanguage sourceLanguage, AppLanguage targetLanguage);
    }

    public sealed class RuleBasedStoryTranslationEngine : IStoryTranslationEngine
    {
        private static readonly KeyValuePair<string, string>[] ExactItalianToEnglish = new[]
        {
            Pair("Il Giardino di Eden", "The Garden of Eden"),
            Pair("Sansone e Dalila", "Samson and Delilah"),
            Pair("Giona e il Pesce", "Jonah and the Fish"),
            Pair("Le Pecore e le Capre", "The Sheep and the Goats"),
            Pair("Le 10 Piaghe d'Egitto", "The 10 Plagues of Egypt"),
            Pair("Elia e la Siccita'", "Elijah and the Drought"),
            Pair("Ester Salva il Popolo", "Esther Saves Her People"),
            Pair("Abramo e Isacco al Monte Moria", "Abraham and Isaac on Mount Moriah"),
            Pair("Il Figlio Prodigo", "The Prodigal Son"),
            Pair("La Profezia della Pace di Isaia", "Isaiah's Prophecy of Peace"),
            Pair("Noe' e il Diluvio", "Noah and the Flood"),
            Pair("Filippo e l'Eunuco Etiope", "Philip and the Ethiopian Eunuch"),
            Pair("Davide e Golia", "David and Goliath"),
            Pair("Giuseppe Perdona i Fratelli", "Joseph Forgives His Brothers"),
            Pair("Rut e Boaz", "Ruth and Boaz"),
            Pair("La Nascita di Mose'", "The Birth of Moses"),
            Pair("Anna e Samuele", "Hannah and Samuel"),
            Pair("Il Buon Samaritano", "The Good Samaritan"),
            Pair("Obbedienza", "Obedience"),
            Pair("Fedelta'", "Faithfulness"),
            Pair("Misericordia", "Mercy"),
            Pair("Giudizio", "Judgment"),
            Pair("Potere di Dio", "God's Power"),
            Pair("Preghiera", "Prayer"),
            Pair("Coraggio", "Courage"),
            Pair("Fede", "Faith"),
            Pair("Perdono", "Forgiveness"),
            Pair("Profezia", "Prophecy"),
            Pair("Salvezza", "Salvation"),
            Pair("Buona Novella", "Good News"),
            Pair("Devozione", "Devotion"),
            Pair("Protezione", "Protection"),
            Pair("Amore per il Prossimo", "Love for Neighbor"),
            Pair("Storia Utente", "User Story"),
            Pair("N/D", "N/A"),
            Pair("Tema", "Theme"),
            Pair("Indizio non disponibile", "Hint not available"),
            Pair("Soluzione non disponibile", "Solution not available"),
            Pair("Nota non disponibile", "Note not available"),
            Pair("Un giovane pastore", "A young shepherd"),
            Pair("Un piccolo gregge", "A small flock"),
            Pair("Armi e combattimento", "Weapons and battle"),
            Pair("Paura nel campo", "Fear in the camp"),
            Pair("Qualcosa manca in questa storia...", "Something is missing from this story..."),
            Pair("La caduta del nemico", "The enemy's fall"),
            Pair("Una vittoria inattesa", "An unexpected victory"),
            Pair("Una forza piu' grande delle apparenze", "A strength greater than appearances"),
            Pair("Un uomo con grande autorita'", "A man with great authority"),
            Pair("Il viaggio nel deserto", "The journey through the desert"),
            Pair("Argento e commercio", "Silver and trade"),
            Pair("Anni di sofferenza", "Years of suffering"),
            Pair("Un incontro inatteso", "An unexpected meeting"),
            Pair("Un abbraccio ritrovato", "A restored embrace"),
            Pair("Un legame ricostruito", "A rebuilt bond"),
            Pair("Una svolta guidata da Dio", "A turn guided by God"),
            Pair("Una donna in cammino", "A woman on a journey"),
            Pair("Una figura di famiglia", "A family figure"),
            Pair("Un campo di raccolta", "A harvest field"),
            Pair("Un uomo con un ruolo decisivo", "A man with a decisive role"),
            Pair("Un legame che nasce", "A bond taking shape"),
            Pair("Un diritto di riscatto", "A right of redemption"),
            Pair("Una discendenza benedetta", "A blessed lineage"),
            Pair("Scegliere il popolo di Geova", "Choosing Jehovah's people"),
            Pair("Un neonato da proteggere", "A newborn to protect"),
            Pair("Acqua in movimento", "Moving water"),
            Pair("Un luogo di potere", "A place of power"),
            Pair("Una donna di alto rango", "A high-ranking woman"),
            Pair("Piante lungo la riva", "Plants along the riverbank"),
            Pair("Le mani che lo tengono al sicuro", "Hands keeping him safe"),
            Pair("Protetto in modo inatteso", "Protected in an unexpected way"),
            Pair("Uno sguardo vigile dall'alto", "A watchful gaze from above"),
            Pair("Una preghiera intensa", "An intense prayer"),
            Pair("Dolore interiore", "Inner pain"),
            Pair("Un luogo di adorazione", "A place of worship"),
            Pair("Un canto di gratitudine", "A song of gratitude"),
            Pair("Un cammino di fede", "A path of faith"),
            Pair("Un dono atteso a lungo", "A long-awaited gift"),
            Pair("Una vita dedicata a Geova", "A life dedicated to Jehovah"),
            Pair("Una richiesta ascoltata", "A prayer that was heard"),
            Pair("Un uomo ferito lungo la strada", "A wounded man on the road"),
            Pair("La violenza dei briganti", "The violence of bandits"),
            Pair("Un animale da viaggio", "A travel animal"),
            Pair("Un pagamento per le cure", "A payment for the care"),
            Pair("Chi si fermera' ad aiutare?", "Who will stop to help?"),
            Pair("Chi scelse di tirare dritto", "Who chose to keep walking"),
            Pair("Un gesto di misericordia", "An act of mercy"),
            Pair("Conta chi si ferma ad aiutare", "What matters is who stops to help")
        };

        private static readonly KeyValuePair<string, string>[] FragmentItalianToEnglish = new[]
        {
            Pair("Rivela", "Reveal"),
            Pair("Nascondi", "Hide"),
            Pair("Storia successiva", "Next story"),
            Pair("La vera forza non sta nei capelli, ma nell'alleanza con Dio.", "True strength is not in the hair, but in one's covenant with God."),
            Pair("Un serpente, un albero proibito e una scelta fatale.", "A serpent, a forbidden tree, and a fatal choice."),
            Pair("Tre giorni nel buio insegnano cosa significa correre verso Dio, non lontano da Lui.", "Three days in the darkness teach what it means to run toward God, not away from Him."),
            Pair("Il padre vede il figlio da lontano e corre ad abbracciarlo.", "The father sees the son from far away and runs to embrace him."),
            Pair("Una regina rischia la vita presentandosi al re senza essere stata convocata.", "A queen risks her life by appearing before the king uninvited."),
            Pair("Un uomo giusto costruisce una grande arca mentre il mondo lo deride.", "A righteous man builds a great ark while the world mocks him."),
            Pair("Un carro nel deserto, un rotolo di Isaia e un incontro guidato dagli angeli.", "A chariot in the desert, a scroll of Isaiah, and a meeting guided by angels."),
            Pair("Sacerdote e levita passarono oltre. Solo uno straniero si fermo'.", "A priest and a Levite passed by. Only a stranger stopped."),
            Pair("Una vedova straniera scelse di seguire il Dio della suocera.", "A foreign widow chose to follow the God of her mother-in-law."),
            Pair("Una madre intreccia una cesta di giunchi per salvare il suo bambino dal Nilo.", "A mother weaves a basket of reeds to save her baby from the Nile."),
            Pair("Una donna in lacrime al tempio prega con tale fervore da sembrare ubriaca.", "A woman in tears at the sanctuary prays so intensely that she seems drunk."),
            Pair("Anni di schiavitu' e prigione nascondevano un piano di Dio.", "Years of slavery and prison were hiding God's purpose."),
            Pair("Con Geova, anche i giganti cadono.", "With Jehovah, even giants fall."),
            Pair("Anche le ingiustizie piu' dure possono far parte del piano di Dio.", "Even the harshest injustices can be part of God's purpose."),
            Pair("La lealta' a Geova va oltre le frontiere etniche e culturali.", "Loyalty to Jehovah goes beyond ethnic and cultural boundaries."),
            Pair("Geova usa anche le circostanze piu' disperate per proteggere i Suoi servitori.", "Jehovah can use even the most desperate circumstances to protect His servants."),
            Pair("La preghiera sincera viene sempre ascoltata da Geova.", "Sincere prayer is always heard by Jehovah."),
            Pair("L'amore per il prossimo non ha confini etnici o religiosi.", "Love for one's neighbor has no ethnic or religious boundaries.")
        };

        private static readonly Dictionary<string, string> ItalianToEnglishWords = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "adorazione", "worship" }, { "ai", "to the" }, { "al", "to the" }, { "alla", "to the" },
            { "amore", "love" }, { "angelo", "angel" }, { "angeli", "angels" }, { "animale", "animal" },
            { "animali", "animals" }, { "arca", "ark" }, { "ascoltato", "heard" }, { "autorita'", "authority" },
            { "bambino", "baby" }, { "battaglia", "battle" }, { "benedetta", "blessed" }, { "briganti", "bandits" },
            { "campo", "camp" }, { "cammino", "journey" }, { "care", "care" }, { "carro", "chariot" },
            { "cesta", "basket" }, { "chi", "who" }, { "cielo", "heaven" }, { "citta'", "city" },
            { "compassione", "compassion" }, { "coraggio", "courage" }, { "creatore", "Creator" }, { "cure", "care" },
            { "dalla", "from the" }, { "dalle", "from the" }, { "dal", "from the" }, { "dell'", "of the " },
            { "della", "of the" }, { "delle", "of the" }, { "deserto", "desert" }, { "dio", "God" },
            { "dolore", "pain" }, { "donna", "woman" }, { "dono", "gift" }, { "dritto", "straight" },
            { "e", "and" }, { "figlio", "son" }, { "fiume", "river" }, { "forza", "strength" },
            { "fuga", "escape" }, { "fede", "faith" }, { "famiglia", "family" }, { "geova", "Jehovah" },
            { "gesto", "act" }, { "gigante", "giant" }, { "giorno", "day" }, { "giovane", "young" },
            { "grande", "great" }, { "gregge", "flock" }, { "guerra", "war" }, { "guida", "guide" },
            { "ha", "has" }, { "il", "the" }, { "imprevisto", "unexpected" }, { "indizio", "hint" },
            { "incontro", "meeting" }, { "ingiustizie", "injustices" }, { "interiore", "inner" }, { "lacrime", "tears" },
            { "legame", "bond" }, { "lungo", "along" }, { "luogo", "place" }, { "mani", "hands" },
            { "misericordia", "mercy" }, { "movimento", "motion" }, { "nascita", "birth" }, { "neonato", "newborn" },
            { "nemico", "enemy" }, { "note", "note" }, { "nuovo", "new" }, { "pagamento", "payment" },
            { "parola", "word" }, { "pastore", "shepherd" }, { "paura", "fear" }, { "per", "for" },
            { "persone", "people" }, { "piu'", "more" }, { "popolo", "people" }, { "potere", "power" },
            { "preghiera", "prayer" }, { "prossimo", "neighbor" }, { "proteggere", "protect" }, { "protezione", "protection" },
            { "raccolta", "harvest" }, { "rango", "rank" }, { "richiesta", "request" }, { "riscatto", "redemption" },
            { "riva", "shore" }, { "ruolo", "role" }, { "salvezza", "salvation" }, { "santuario", "sanctuary" },
            { "scena", "scene" }, { "scegliere", "choosing" }, { "sicuro", "safe" }, { "sicurezza", "safety" },
            { "soluzione", "solution" }, { "sofferenza", "suffering" }, { "sorpresa", "surprise" }, { "storia", "story" },
            { "strada", "road" }, { "tempio", "temple" }, { "terra", "earth" }, { "tirare", "keep" },
            { "uomo", "man" }, { "una", "a" }, { "un", "a" }, { "viaggio", "journey" }, { "vigile", "watchful" },
            { "vittoria", "victory" }, { "volta", "time" }
        };

        public StoryLocalizedText Translate(StoryLocalizedText source, AppLanguage sourceLanguage, AppLanguage targetLanguage)
        {
            if (source == null)
                return new StoryLocalizedText();

            if (sourceLanguage == targetLanguage)
                return source.Clone();

            return new StoryLocalizedText
            {
                Title = TranslateText(source.Title, sourceLanguage, targetLanguage),
                ScriptureReference = TranslateText(source.ScriptureReference, sourceLanguage, targetLanguage),
                Keyword = TranslateText(source.Keyword, sourceLanguage, targetLanguage),
                Hint = TranslateText(source.Hint, sourceLanguage, targetLanguage),
                Solution = TranslateText(source.Solution, sourceLanguage, targetLanguage),
                EngagementNote = TranslateText(source.EngagementNote, sourceLanguage, targetLanguage),
                ScriptureQuote = TranslateText(source.ScriptureQuote, sourceLanguage, targetLanguage),
                ImageCaptions = TranslateArray(source.ImageCaptions, sourceLanguage, targetLanguage)
            };
        }

        private static string[] TranslateArray(string[] source, AppLanguage sourceLanguage, AppLanguage targetLanguage)
        {
            if (source == null)
                return null;

            var result = new string[source.Length];
            for (int i = 0; i < source.Length; i++)
                result[i] = TranslateText(source[i], sourceLanguage, targetLanguage);
            return result;
        }

        private static string TranslateText(string value, AppLanguage sourceLanguage, AppLanguage targetLanguage)
        {
            if (string.IsNullOrWhiteSpace(value) || sourceLanguage == targetLanguage)
                return value ?? string.Empty;

            if (sourceLanguage == AppLanguage.Italian && targetLanguage == AppLanguage.English)
                return TranslateItalianToEnglish(value);

            if (sourceLanguage == AppLanguage.English && targetLanguage == AppLanguage.Italian)
                return TranslateEnglishToItalian(value);

            return value;
        }

        private static string TranslateItalianToEnglish(string value)
        {
            string translated = ApplyExactAndFragments(value, ExactItalianToEnglish, FragmentItalianToEnglish);
            translated = TranslateWords(translated, ItalianToEnglishWords);
            translated = Regex.Replace(translated, @"\s+", " ").Trim();
            return translated;
        }

        private static string TranslateEnglishToItalian(string value)
        {
            var exact = ExactItalianToEnglish.Select(pair => Pair(pair.Value, pair.Key)).ToArray();
            var fragments = FragmentItalianToEnglish.Select(pair => Pair(pair.Value, pair.Key)).ToArray();
            var words = ItalianToEnglishWords.ToDictionary(pair => pair.Value, pair => pair.Key, StringComparer.OrdinalIgnoreCase);

            string translated = ApplyExactAndFragments(value, exact, fragments);
            translated = TranslateWords(translated, words);
            translated = Regex.Replace(translated, @"\s+", " ").Trim();
            return translated;
        }

        private static string ApplyExactAndFragments(string value, KeyValuePair<string, string>[] exact, KeyValuePair<string, string>[] fragments)
        {
            string trimmed = (value ?? string.Empty).Trim();
            for (int i = 0; i < exact.Length; i++)
            {
                if (string.Equals(trimmed, exact[i].Key, StringComparison.OrdinalIgnoreCase))
                    return exact[i].Value;
            }

            string translated = trimmed;
            for (int i = 0; i < fragments.Length; i++)
            {
                translated = Regex.Replace(
                    translated,
                    Regex.Escape(fragments[i].Key),
                    fragments[i].Value,
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            }
            return translated;
        }

        private static string TranslateWords(string value, IDictionary<string, string> glossary)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value ?? string.Empty;

            return Regex.Replace(value, @"[A-Za-zÀ-ÿ']+", delegate(Match match)
            {
                string replacement;
                return glossary.TryGetValue(match.Value, out replacement) ? PreserveWordCase(match.Value, replacement) : match.Value;
            });
        }

        private static string PreserveWordCase(string source, string target)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target))
                return target;

            if (source.ToUpperInvariant() == source)
                return target.ToUpperInvariant();

            if (char.IsUpper(source[0]))
                return char.ToUpperInvariant(target[0]) + target.Substring(1);

            return target;
        }

        private static KeyValuePair<string, string> Pair(string key, string value)
        {
            return new KeyValuePair<string, string>(key, value);
        }
    }
}