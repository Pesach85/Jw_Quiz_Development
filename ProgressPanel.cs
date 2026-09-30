using System;
using System.Drawing;
using System.Windows.Forms;

namespace Jw_Quiz_Development
{
    public class ProgressPanel : Panel
    {
        private Label labelLevel;
        private Label labelXP;
        private ProgressBar progressBar;
        private Label labelProgress;
        private Label labelStreak;
        private Button buttonBadges;
    private Button buttonStats;

        public ProgressPanel()
        {
            this.DoubleBuffered = true;
            InitializeComponents();
            RefreshDisplay();
        }

        private void InitializeComponents()
        {
            // Panel Setup
            this.Height = 110;
            this.Dock = DockStyle.Bottom;
            this.BackColor = Color.FromArgb(11, 18, 32);
            this.BorderStyle = BorderStyle.None;

            // Label Livello
            labelLevel = new Label
            {
                Text = "Livello 1 ⭐",
                Location = new Point(12, 10),
                Size = new Size(130, 24),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(138, 200, 255)
            };
            this.Controls.Add(labelLevel);

            // Label XP
            labelXP = new Label
            {
                Text = "XP 0/500",
                Location = new Point(12, 38),
                Size = new Size(150, 22),
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(183, 236, 200)
            };
            this.Controls.Add(labelXP);

            // Progress Bar (storie completate)
            progressBar = new ProgressBar
            {
                Location = new Point(170, 14),
                Size = new Size(280, 20),
                Minimum = 0,
                Maximum = 18,
                Value = 0
            };
            this.Controls.Add(progressBar);

            // Label Progresso
            labelProgress = new Label
            {
                Text = "Storie 0/18",
                Location = new Point(170, 38),
                Size = new Size(280, 22),
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(230, 240, 252)
            };
            this.Controls.Add(labelProgress);

            labelStreak = new Label
            {
                Text = "Streak 0",
                Location = new Point(170, 62),
                Size = new Size(280, 20),
                Font = new Font("Segoe UI", 8, FontStyle.Regular),
                ForeColor = Color.FromArgb(240, 195, 106)
            };
            this.Controls.Add(labelStreak);

            // Button Badge
            buttonBadges = new Button
            {
                Text = "Badge 🏆 (0)",
                Location = new Point(465, 12),
                Size = new Size(140, 48),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(244, 196, 48),
                FlatStyle = FlatStyle.Flat
            };
            buttonBadges.Click += ButtonBadges_Click;
            this.Controls.Add(buttonBadges);

            buttonStats = new Button
            {
                Text = "Statistiche 📊",
                Location = new Point(614, 12),
                Size = new Size(140, 48),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(73, 141, 238),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            buttonStats.Click += ButtonStats_Click;
            this.Controls.Add(buttonStats);
        }

        public void RefreshDisplay()
        {
            var tracker = ProgressTracker.Instance;
            int level = tracker.GetLevel();
            int xpToCurrent = (level - 1) * 500;
            int xpInCurrentLevel = tracker.CurrentXP - xpToCurrent;
            int totalStories = Math.Max(1, StoryEngine.TotalStories);

            labelLevel.Text = $"Livello {level} ⭐";
            labelXP.Text = $"XP {xpInCurrentLevel}/500";
            
            progressBar.Maximum = totalStories;
            progressBar.Value = tracker.CompletedStories.Count;
            int threeStars = tracker.GetThreeStarCount();
            labelProgress.Text = $"Storie {tracker.CompletedStories.Count}/{totalStories}  \u00b7  \u2605\u00d7{threeStars}";
            string streakLabel = AppText.Get("streak_noHint");
            labelStreak.Text = $"{streakLabel}: {tracker.NoHintStreak}  (max {tracker.MaxNoHintStreak})";
            if (tracker.StreakBadges != null && tracker.StreakBadges.Count > 0)
                labelStreak.Text += "  ·  " + string.Join(", ", tracker.StreakBadges);
            
            buttonBadges.Text = $"Badge ({tracker.UnlockedBadges.Count})";
            buttonBadges.BackColor = tracker.UnlockedBadges.Count > 0 
                ? Color.FromArgb(232, 197, 71)
                : Color.FromArgb(130, 140, 160);
            buttonBadges.ForeColor = Color.FromArgb(11, 18, 32);
            Invalidate();
        }

        private void ButtonBadges_Click(object sender, EventArgs e)
        {
            var tracker = ProgressTracker.Instance;
            
            if (tracker.UnlockedBadges.Count == 0)
            {
                MessageBox.Show(
                    "Nessun badge sbloccato ancora.\nCompleta storie per guadagnare badge!",
                    "Badge",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            string badges = "Badge Sbloccati:\n\n";
            foreach (var badge in tracker.UnlockedBadges)
            {
                badges += $"🏆 {badge.Name}\n   {badge.Description}\n   ({badge.UnlockedDate:dd/MM/yyyy})\n\n";
            }

            MessageBox.Show(badges, "I Tuoi Badge", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ButtonStats_Click(object sender, EventArgs e)
        {
            var tracker = ProgressTracker.Instance;
            string text = "PANORAMICA PROGRESSO\n\n" +
                          $"Livello: {tracker.GetLevel()}\n" +
                          $"XP Totali: {tracker.CurrentXP}\n" +
                          $"Completamenti: {tracker.TotalCompletions}\n" +
                          $"Storie uniche: {tracker.CompletedStories.Count}/{StoryEngine.TotalStories}\n" +
                          $"Percentuale: {tracker.GetProgressPercentage()}%\n" +
                          $"Streak no-hint: {tracker.NoHintStreak} (max {tracker.MaxNoHintStreak})\n" +
                          $"Badge: {tracker.UnlockedBadges.Count}";
            MessageBox.Show(text, "Statistiche Rapide", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var pen = new Pen(Color.FromArgb(58, 94, 140), 2))
            {
                e.Graphics.DrawLine(pen, 0, 1, Width, 1);
            }

            using (var brush = new SolidBrush(Color.FromArgb(19, 32, 51)))
            {
                e.Graphics.FillRectangle(brush, new Rectangle(0, 84, Width, 26));
            }

            using (var textBrush = new SolidBrush(Color.FromArgb(170, 190, 220)))
            {
                e.Graphics.DrawString("Progresso biblico", new Font("Segoe UI", 8, FontStyle.Italic), textBrush, 12, 89);
            }
        }
    }
}
