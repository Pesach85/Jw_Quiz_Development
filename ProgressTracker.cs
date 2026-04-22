using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Jw_Quiz_Development
{
    public class ProgressTracker
    {
        private static ProgressTracker _instance;
        private const string PROGRESS_FILE = "UserProgress.dat";
        
        public DateTime StartDate { get; set; }
        public int TotalCompletions { get; set; }
        public int CurrentXP { get; set; }
        public HashSet<int> CompletedStories { get; set; }
        public Dictionary<int, int> StoryAttempts { get; set; }

        // Badge system
        public List<Badge> UnlockedBadges { get; set; }

        // Star ratings per story (1=bronze, 2=silver, 3=gold). Stores best rating.
        public Dictionary<int, int> StoryStars { get; set; }

        private ProgressTracker()
        {
            CompletedStories = new HashSet<int>();
            StoryAttempts = new Dictionary<int, int>();
            UnlockedBadges = new List<Badge>();
            StoryStars = new Dictionary<int, int>();
            StartDate = DateTime.Now;
            CurrentXP = 0;
            TotalCompletions = 0;
        }

        public static ProgressTracker Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = LoadOrCreate();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Segna una storia come completata e assegna punti esperienza.
        /// </summary>
        public void CompleteStory(int storyId, int xpToAward = 100, int stars = 3)
        {
            if (!CompletedStories.Contains(storyId))
            {
                CompletedStories.Add(storyId);
                TotalCompletions++;
            CurrentXP += xpToAward;
            }
            else
            {
            CurrentXP += Math.Max(20, xpToAward / 2);
            }

            if (!StoryAttempts.ContainsKey(storyId))
                StoryAttempts[storyId] = 0;
            StoryAttempts[storyId]++;

            // Track best star rating for this story (higher is better)
            int currentStars;
            if (!StoryStars.TryGetValue(storyId, out currentStars) || stars > currentStars)
                StoryStars[storyId] = stars;

            CheckBadges();
            Save();
        }

        /// <summary>
        /// Calcola il livello in base a XP (ogni 500 XP = 1 livello).
        /// </summary>
        public int GetLevel()
        {
            return (CurrentXP / 500) + 1;
        }

        /// <summary>
        /// Percentuale di progresso totale (quante storie completate su 12).
        /// </summary>
        public int GetProgressPercentage()
        {
              int total = StoryEngine.TotalStories;
              if (total == 0) return 0;
              return (CompletedStories.Count * 100) / total;
        }

        /// <summary>
        /// XP necessari al prossimo livello.
        /// </summary>
        public int GetXPToNextLevel()
        {
            int currentLevel = GetLevel();
            int xpForNextLevel = currentLevel * 500;
            return Math.Max(0, xpForNextLevel - CurrentXP);
        }

        private void CheckBadges()
        {
            // Badge: "Primi Passi" - Completa 1 storia
            if (CompletedStories.Count >= 1 && !HasBadge("PrimiPassi"))
            {
                UnlockedBadges.Add(new Badge 
                { 
                    Id = "PrimiPassi", 
                    Name = "Primi Passi", 
                    Description = "Completa la tua prima storia",
                    UnlockedDate = DateTime.Now
                });
            }

            // Badge: "Studioso" - Completa 5 storie
            if (CompletedStories.Count >= 5 && !HasBadge("Studioso"))
            {
                UnlockedBadges.Add(new Badge 
                { 
                    Id = "Studioso", 
                    Name = "Studioso", 
                    Description = "Completa 5 storie bibliche",
                    UnlockedDate = DateTime.Now
                });
            }

            // Badge: "Esperto" - Completa tutte le 12 storie
            // Threshold 12 = minimum for mastery (stays meaningful even as story count grows)
            if (CompletedStories.Count >= 12 && !HasBadge("Esperto"))
            {
                UnlockedBadges.Add(new Badge 
                { 
                    Id = "Esperto", 
                    Name = "Esperto Biblico", 
                    Description = "Completa tutte le 12 storie",
                    UnlockedDate = DateTime.Now
                });
            }

            // Badge: "Studioso Diligente" - 20+ replay
            int totalReplays = StoryAttempts.Values.Sum() - CompletedStories.Count;
            if (totalReplays >= 20 && !HasBadge("StudioDiligente"))
            {
                UnlockedBadges.Add(new Badge 
                { 
                    Id = "StudioDiligente", 
                    Name = "Studioso Diligente", 
                    Description = "Rileggi 20 storie per approfondire la comprensione",
                    UnlockedDate = DateTime.Now
                });
            }
        }

        private bool HasBadge(string badgeId)
        {
            return UnlockedBadges.Any(b => b.Id == badgeId);
        }

        /// <summary>Restituisce il miglior punteggio a stelle (1-3) per una storia, o 0 se mai completata.</summary>
        public int GetStarRating(int storyId)
        {
            int s;
            return StoryStars.TryGetValue(storyId, out s) ? s : 0;
        }

        /// <summary>Quante storie sono state completate con 3 stelle (nessun aiuto).</summary>
        public int GetThreeStarCount()
        {
            int count = 0;
            foreach (var s in StoryStars.Values)
                if (s >= 3) count++;
            return count;
        }

        public void Save()
        {
            try
            {
                // Implementazione semplice: salvataggio JSON
                // In produzione, usare JSON.NET o similar
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine(StartDate.ToString("o"));
                sb.AppendLine(TotalCompletions.ToString());
                sb.AppendLine(CurrentXP.ToString());
                sb.AppendLine(string.Join(",", CompletedStories.OrderBy(x => x)));
                // Line 5: StoryAttempts  format  "id:count,id:count,..."
                var attemptsStr = string.Join(",", StoryAttempts
                    .OrderBy(kv => kv.Key)
                    .Select(kv => kv.Key + ":" + kv.Value));
                sb.AppendLine(attemptsStr);
                // Line 5: StoryStars  format  "id:stars,id:stars,..."
                var starsStr = string.Join(",", StoryStars
                    .OrderBy(kv => kv.Key)
                    .Select(kv => kv.Key + ":" + kv.Value));
                sb.Append(starsStr);
                
                File.WriteAllText(PROGRESS_FILE, sb.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore nel salvataggio: {ex.Message}");
            }
        }

        public static ProgressTracker LoadOrCreate()
        {
            ProgressTracker tracker = new ProgressTracker();
            
            try
            {
                if (File.Exists(PROGRESS_FILE))
                {
                    string[] lines = File.ReadAllLines(PROGRESS_FILE);
                    if (lines.Length >= 4)
                    {
                        if (DateTime.TryParse(lines[0], out DateTime startDate))
                            tracker.StartDate = startDate;

                        if (int.TryParse(lines[1], out int completions))
                            tracker.TotalCompletions = completions;

                        if (int.TryParse(lines[2], out int xp))
                            tracker.CurrentXP = xp;

                        if (!string.IsNullOrWhiteSpace(lines[3]))
                        {
                            foreach (var id in lines[3].Split(','))
                                if (int.TryParse(id.Trim(), out int storyId))
                                    tracker.CompletedStories.Add(storyId);
                        }

                        // Line 5 (optional): StoryAttempts  "id:count,id:count,..."
                        if (lines.Length >= 5 && !string.IsNullOrWhiteSpace(lines[4]))
                        {
                            foreach (var pair in lines[4].Split(','))
                            {
                                var parts = pair.Split(':');
                                if (parts.Length == 2
                                    && int.TryParse(parts[0].Trim(), out int sid)
                                    && int.TryParse(parts[1].Trim(), out int cnt))
                                    tracker.StoryAttempts[sid] = cnt;
                            }
                        }

                        // Line 6 (optional): StoryStars  "id:stars,id:stars,..."
                        if (lines.Length >= 6 && !string.IsNullOrWhiteSpace(lines[5]))
                        {
                            foreach (var pair in lines[5].Split(','))
                            {
                                var parts = pair.Split(':');
                                if (parts.Length == 2
                                    && int.TryParse(parts[0].Trim(), out int storyId2)
                                    && int.TryParse(parts[1].Trim(), out int starsVal)
                                    && starsVal >= 1 && starsVal <= 3)
                                    tracker.StoryStars[storyId2] = starsVal;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore nel caricamento: {ex.Message}");
            }

            tracker.CheckBadges();
            return tracker;
        }
    }

    public class Badge
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime UnlockedDate { get; set; }
    }
}
