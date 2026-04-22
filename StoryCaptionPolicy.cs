using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Jw_Quiz_Development
{
    internal static class StoryCaptionPolicy
    {
        private static readonly HashSet<string> StopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "a", "ad", "agli", "ai", "al", "alla", "alle", "allo", "anche", "chi", "che", "come",
            "con", "da", "dagli", "dai", "dal", "dalla", "dalle", "dallo", "dei", "degli", "del",
            "della", "delle", "dello", "di", "dove", "e", "ed", "fra", "gli", "ha", "ho", "i", "il",
            "in", "indizio", "l", "la", "le", "lo", "ma", "mi", "ne", "nei", "negli", "nel", "nella",
            "nelle", "nello", "no", "non", "o", "per", "piu", "questa", "queste", "questi", "questo",
            "quella", "quelle", "quelli", "quello", "racconto", "solo", "storia", "su", "sua", "sue",
            "suo", "suoi", "tnm", "tra", "un", "una", "uno", "va"
        };

        private static readonly Dictionary<string, string> NeutralByImageKey = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "038-boy-1", "Un giovane coinvolto nel racconto" },
            { "039-baby", "Un bambino al centro della scena" },
            { "036-man-1", "Un uomo nella scena" },
            { "031-man-2", "Un altro uomo nella scena" },
            { "094-user", "Una persona importante" },
            { "093-users", "Un gruppo di persone" },
            { "1F411", "Un animale del gregge" },
            { "1F42A", "Un animale da carico" },
            { "Hackney-100", "Un animale da viaggio" },
            { "2694", "Armi e conflitto" },
            { "1F632", "Sorpresa e tensione" },
            { "1F629", "Dolore e fatica" },
            { "1F498", "Un gesto di amore o compassione" },
            { "1F4B0", "Un pagamento o una ricompensa" },
            { "1F3DB", "Un luogo di adorazione" },
            { "1F30A", "Acqua in movimento" },
            { "1F3F0", "Un luogo di potere" },
            { "1F451", "Un'autorita' importante" },
            { "1F333", "Piante lungo il cammino" },
            { "1F932-1F3FC", "Una preghiera o una supplica" },
            { "1F47C", "Un aiuto inatteso" },
            { "1F3B6", "Un canto o una celebrazione" },
            { "1F5FA", "Un percorso da seguire" },
            { "1F440", "Un dettaglio da osservare bene" },
            { "203C", "Un richiamo importante" },
            { "1F4D6", "Un messaggio da comprendere" },
            { "26D4", "Un rifiuto o un ostacolo" },
            { "2753", "Un dettaglio ancora nascosto" }
        };

        public static string GetDisplayCaption(Story story, int slotIndex)
        {
            if (story == null || slotIndex < 0)
                return string.Empty;

            string imageKey = GetImageKey(story, slotIndex);
            string fallback = BuildFallbackCaption(slotIndex, imageKey);
            string raw = GetRawCaption(story, slotIndex);

            if (string.IsNullOrWhiteSpace(raw))
                return fallback;

            raw = raw.Trim();
            return IsTooExplicit(story, raw) ? fallback : raw;
        }

        private static string GetRawCaption(Story story, int slotIndex)
        {
            if (story.ImageCaptions == null || slotIndex >= story.ImageCaptions.Length)
                return string.Empty;

            return story.ImageCaptions[slotIndex] ?? string.Empty;
        }

        private static string GetImageKey(Story story, int slotIndex)
        {
            if (slotIndex < 5)
                return story.VisibleEmojis != null && slotIndex < story.VisibleEmojis.Length ? story.VisibleEmojis[slotIndex] : string.Empty;
            if (slotIndex < 7)
                return story.HiddenEmojis != null && slotIndex - 5 < story.HiddenEmojis.Length ? story.HiddenEmojis[slotIndex - 5] : string.Empty;
            if (slotIndex == 7)
                return story.HintEmoji ?? string.Empty;
            return string.Empty;
        }

        private static string BuildFallbackCaption(int slotIndex, string imageKey)
        {
            string neutral;
            if (!string.IsNullOrWhiteSpace(imageKey) && NeutralByImageKey.TryGetValue(imageKey, out neutral))
                return neutral;

            if (slotIndex == 7)
                return "Un indizio visivo da interpretare";
            if (slotIndex >= 5)
                return "Un dettaglio che si rivelera' piu' avanti";
            return "Un elemento importante del racconto";
        }

        private static bool IsTooExplicit(Story story, string caption)
        {
            HashSet<string> captionTerms = Tokenize(caption);
            if (captionTerms.Count == 0)
                return false;

            HashSet<string> strongTerms = GetStrongTerms(story);
            foreach (string term in captionTerms)
            {
                if (strongTerms.Contains(term))
                    return true;
            }

            int overlap = 0;
            HashSet<string> contextTerms = GetContextTerms(story);
            foreach (string term in captionTerms)
            {
                if (!contextTerms.Contains(term))
                    continue;

                overlap++;
                if (overlap >= 2)
                    return true;
            }

            return false;
        }

        private static HashSet<string> GetStrongTerms(Story story)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AddTokens(result, story.Title);
            AddCapitalizedWords(result, story.Title);
            AddCapitalizedWords(result, story.Keyword);
            AddCapitalizedWords(result, story.Hint);
            AddCapitalizedWords(result, story.Solution);
            AddCapitalizedWords(result, story.ScriptureQuote);
            return result;
        }

        private static HashSet<string> GetContextTerms(Story story)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AddTokens(result, story.Title);
            AddTokens(result, story.Keyword);
            AddTokens(result, story.Hint);
            AddTokens(result, story.Solution);
            AddTokens(result, story.ScriptureQuote);
            return result;
        }

        private static void AddCapitalizedWords(HashSet<string> target, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            foreach (Match match in Regex.Matches(value, @"[\p{L}']+"))
            {
                string raw = match.Value;
                if (string.IsNullOrWhiteSpace(raw) || !char.IsUpper(raw[0]))
                    continue;

                AddToken(target, raw);
            }
        }

        private static void AddTokens(HashSet<string> target, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            foreach (string token in Tokenize(value))
                target.Add(token);
        }

        private static HashSet<string> Tokenize(string value)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(value))
                return result;

            string normalized = Normalize(value);
            string[] pieces = normalized.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < pieces.Length; i++)
                AddToken(result, pieces[i]);

            return result;
        }

        private static void AddToken(HashSet<string> target, string value)
        {
            string token = Normalize(value);
            if (token.Length < 4 || StopWords.Contains(token))
                return;

            target.Add(token);
        }

        private static string Normalize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            string decomposed = value.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder(decomposed.Length);

            for (int i = 0; i < decomposed.Length; i++)
            {
                char current = decomposed[i];
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(current);
                if (category == UnicodeCategory.NonSpacingMark)
                    continue;

                builder.Append(char.IsLetterOrDigit(current) ? char.ToLowerInvariant(current) : ' ');
            }

            return builder.ToString();
        }
    }
}