using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Jw_Quiz_Development
{
    public static class UserStoryLibrary
    {
        private const string USER_STORIES_FILE = "UserStories.dat";
        private const int USER_STORY_START_ID = 1000;

        public static List<Story> GetUserStories()
        {
            var stories = new List<Story>();
            if (!File.Exists(USER_STORIES_FILE))
            {
                return stories;
            }

            try
            {
                string[] lines = File.ReadAllLines(USER_STORIES_FILE);
                Story current = null;
                foreach (string raw in lines)
                {
                    string line = raw.Trim();
                    if (line == "[STORY]")
                    {
                        current = new Story
                        {
                            IsDynamic = true,
                            IsUserCreated = true
                        };
                        continue;
                    }

                    if (line == "[END]")
                    {
                        if (current != null)
                        {
                            NormalizeStory(current);
                            stories.Add(current);
                            current = null;
                        }
                        continue;
                    }

                    if (current == null || string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    int sep = line.IndexOf('=');
                    if (sep <= 0)
                    {
                        continue;
                    }

                    string key = line.Substring(0, sep).Trim();
                    string value = line.Substring(sep + 1).Trim();

                    switch (key)
                    {
                        case "Id":
                            if (int.TryParse(value, out int id))
                            {
                                current.Id = id;
                            }
                            break;
                        case "Title": current.Title = value; break;
                        case "ScriptureReference": current.ScriptureReference = value; break;
                        case "Keyword": current.Keyword = value; break;
                        case "Hint": current.Hint = value; break;
                        case "Solution": current.Solution = value; break;
                        case "EngagementNote": current.EngagementNote = value; break;
                        case "ScriptureQuote": current.ScriptureQuote = value; break;
                        case "SourceLanguage":
                            AppLanguage parsedLanguage;
                            current.SourceLanguage = System.Enum.TryParse(value, true, out parsedLanguage) ? parsedLanguage : AppLanguage.Italian;
                            break;
                        case "VisibleEmojis": current.VisibleEmojis = SplitPipe(value, 5); break;
                        case "HiddenEmojis": current.HiddenEmojis = SplitPipe(value, 2); break;
                        case "HintEmoji": current.HintEmoji = value; break;
                        case "ImageCaptions":
                            current.ImageCaptions = string.IsNullOrWhiteSpace(value)
                                ? null
                                : value.Split('|');
                            break;
                        case "Title_en": EnsureEnglish(current).Title = value; break;
                        case "ScriptureReference_en": EnsureEnglish(current).ScriptureReference = value; break;
                        case "Keyword_en": EnsureEnglish(current).Keyword = value; break;
                        case "Hint_en": EnsureEnglish(current).Hint = value; break;
                        case "Solution_en": EnsureEnglish(current).Solution = value; break;
                        case "EngagementNote_en": EnsureEnglish(current).EngagementNote = value; break;
                        case "ScriptureQuote_en": EnsureEnglish(current).ScriptureQuote = value; break;
                        case "ImageCaptions_en": EnsureEnglish(current).ImageCaptions = string.IsNullOrWhiteSpace(value) ? null : value.Split('|'); break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Errore caricando UserStories.dat: " + ex.Message);
            }

            return stories.OrderBy(s => s.Id).ToList();
        }

        public static Story AddStory(Story story)
        {
            var userStories = GetUserStories();
            int newId = userStories.Count == 0 ? USER_STORY_START_ID : userStories.Max(s => s.Id) + 1;

            story.Id = newId;
            story.IsDynamic = true;
            story.IsUserCreated = true;
            NormalizeStory(story);
            StoryLocalizationService.EnsureTranslations(story);

            SaveStory(story);
            return story;
        }

        private static void SaveStory(Story story)
        {
            var lines = new List<string>
            {
                "[STORY]",
                "Id=" + story.Id,
                "Title=" + NullSafe(story.Title),
                "ScriptureReference=" + NullSafe(story.ScriptureReference),
                "Keyword=" + NullSafe(story.Keyword),
                "Hint=" + NullSafe(story.Hint),
                "Solution=" + NullSafe(story.Solution),
                "EngagementNote=" + NullSafe(story.EngagementNote),
                "ScriptureQuote=" + NullSafe(story.ScriptureQuote),
                "SourceLanguage=" + story.SourceLanguage,
                "VisibleEmojis=" + string.Join("|", story.VisibleEmojis ?? new string[0]),
                "HiddenEmojis=" + string.Join("|", story.HiddenEmojis ?? new string[0]),
                "HintEmoji=" + NullSafe(story.HintEmoji),
                "ImageCaptions=" + (story.ImageCaptions != null
                    ? string.Join("|", story.ImageCaptions)
                    : string.Empty),
                "Title_en=" + NullSafe(GetTranslation(story, AppLanguage.English).Title),
                "ScriptureReference_en=" + NullSafe(GetTranslation(story, AppLanguage.English).ScriptureReference),
                "Keyword_en=" + NullSafe(GetTranslation(story, AppLanguage.English).Keyword),
                "Hint_en=" + NullSafe(GetTranslation(story, AppLanguage.English).Hint),
                "Solution_en=" + NullSafe(GetTranslation(story, AppLanguage.English).Solution),
                "EngagementNote_en=" + NullSafe(GetTranslation(story, AppLanguage.English).EngagementNote),
                "ScriptureQuote_en=" + NullSafe(GetTranslation(story, AppLanguage.English).ScriptureQuote),
                "ImageCaptions_en=" + JoinPipe(GetTranslation(story, AppLanguage.English).ImageCaptions),
                "[END]"
            };

            File.AppendAllLines(USER_STORIES_FILE, lines);
        }

        private static string[] SplitPipe(string value, int expected)
        {
            var parts = (value ?? string.Empty)
                .Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Take(expected)
                .ToList();

            while (parts.Count < expected)
            {
                parts.Add(StoryResources.KeyUnknown); // use PNG key, not Unicode emoji
            }

            return parts.ToArray();
        }

        private static void NormalizeStory(Story story)
        {
            story.Title = string.IsNullOrWhiteSpace(story.Title) ? AppText.Get(story.SourceLanguage, "CreatedStoryDefaultTitle") : story.Title;
            story.ScriptureReference = string.IsNullOrWhiteSpace(story.ScriptureReference) ? AppText.Get(story.SourceLanguage, "DefaultReference") : story.ScriptureReference;
            story.Keyword = string.IsNullOrWhiteSpace(story.Keyword) ? AppText.Get(story.SourceLanguage, "DefaultKeyword") : story.Keyword;
            story.Hint = string.IsNullOrWhiteSpace(story.Hint) ? AppText.Get(story.SourceLanguage, "DefaultHint") : story.Hint;
            story.Solution = string.IsNullOrWhiteSpace(story.Solution) ? AppText.Get(story.SourceLanguage, "DefaultSolution") : story.Solution;
            story.EngagementNote = string.IsNullOrWhiteSpace(story.EngagementNote) ? AppText.Get(story.SourceLanguage, "DefaultNote") : story.EngagementNote;

            if (story.VisibleEmojis == null || story.VisibleEmojis.Length < 5)
            {
                story.VisibleEmojis = SplitPipe(string.Join("|", story.VisibleEmojis ?? new string[0]), 5);
            }

            if (story.HiddenEmojis == null || story.HiddenEmojis.Length < 2)
            {
                story.HiddenEmojis = SplitPipe(string.Join("|", story.HiddenEmojis ?? new string[0]), 2);
            }

            if (string.IsNullOrWhiteSpace(story.HintEmoji))
                story.HintEmoji = StoryResources.KeyHint; // 🔥 PNG key, not Unicode emoji

            StoryLocalizationService.EnsureTranslations(story);
        }

        private static string NullSafe(string value)
        {
            return value ?? string.Empty;
        }

        private static StoryLocalizedText EnsureEnglish(Story story)
        {
            if (story.Translations == null)
                story.Translations = new StoryTranslations();

            if (story.Translations.English == null)
                story.Translations.English = new StoryLocalizedText();

            return story.Translations.English;
        }

        private static StoryLocalizedText GetTranslation(Story story, AppLanguage language)
        {
            StoryLocalizationService.EnsureTranslations(story);
            return story.Translations.Get(language) ?? new StoryLocalizedText();
        }

        private static string JoinPipe(string[] values)
        {
            return values != null ? string.Join("|", values) : string.Empty;
        }
    }
}
