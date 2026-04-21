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
                        case "VisibleEmojis": current.VisibleEmojis = SplitPipe(value, 5); break;
                        case "HiddenEmojis": current.HiddenEmojis = SplitPipe(value, 2); break;
                        case "HintEmoji": current.HintEmoji = value; break;
                                            case "ImageCaptions":
                                                current.ImageCaptions = string.IsNullOrWhiteSpace(value)
                                                    ? null
                                                    : value.Split('|');
                                                break;
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
                "VisibleEmojis=" + string.Join("|", story.VisibleEmojis ?? new string[0]),
                "HiddenEmojis=" + string.Join("|", story.HiddenEmojis ?? new string[0]),
                "HintEmoji=" + NullSafe(story.HintEmoji),
                "ImageCaptions=" + (story.ImageCaptions != null
                    ? string.Join("|", story.ImageCaptions)
                    : string.Empty),
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
            story.Title = string.IsNullOrWhiteSpace(story.Title) ? "Storia Utente" : story.Title;
            story.ScriptureReference = string.IsNullOrWhiteSpace(story.ScriptureReference) ? "N/D" : story.ScriptureReference;
            story.Keyword = string.IsNullOrWhiteSpace(story.Keyword) ? "Tema" : story.Keyword;
            story.Hint = string.IsNullOrWhiteSpace(story.Hint) ? "Indizio non disponibile" : story.Hint;
            story.Solution = string.IsNullOrWhiteSpace(story.Solution) ? "Soluzione non disponibile" : story.Solution;
            story.EngagementNote = string.IsNullOrWhiteSpace(story.EngagementNote) ? "Nota non disponibile" : story.EngagementNote;

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
        }

        private static string NullSafe(string value)
        {
            return value ?? string.Empty;
        }
    }
}
