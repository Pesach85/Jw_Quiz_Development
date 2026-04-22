using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Jw_Quiz_Development
{
    public class DynamicStoryForm : Form
    {
        private readonly Story story;
        private readonly bool[] revealed = new bool[3]; // 0/1 = hidden images, 2 = hint
        private StoryLocalizedText localizedText;

        private Label titleLabel;
        private Label referenceLabel;
        private PictureBox[] picBoxes;   // 0-4 visible, 5-6 hidden, 7 hint
        private Label captionLabel;      // shows image description on click
        private Label solutionLabel;
        private Label xpLabel;
        private Label starsLabel;        // live star rating (★★★ degrades as hints used)
        private Button revealButton;
        private Button hintButton;
        private Button solutionButton;
        private Button nextButton;
        private Timer hintPulseTimer;
        private bool hintPulseGrow;
        private bool storyCompleted;

        public DynamicStoryForm(Story story)
        {
            this.story = story ?? throw new ArgumentNullException(nameof(story));
            LanguageManager.LanguageChanged += LanguageManager_LanguageChanged;
            InitializeUi();
            RenderStory();
        }

        private void LanguageManager_LanguageChanged(object sender, EventArgs e)
        {
            if (IsDisposed)
                return;

            RenderStory();
        }

        private void InitializeUi()
        {
            Text = AppText.Get("AppTitle");
            BackColor = Color.FromArgb(18, 28, 44);
            ForeColor = Color.White;
            Width = 1050;
            Height = 680;
            StartPosition = FormStartPosition.CenterScreen;

            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 90,
                BackColor = Color.FromArgb(44, 62, 80)
            };
            Controls.Add(header);

            titleLabel = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(18, 10),
                Size = new Size(760, 40)
            };
            header.Controls.Add(titleLabel);

            referenceLabel = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.FromArgb(203, 225, 252),
                Location = new Point(20, 52),
                Size = new Size(700, 30)
            };
            header.Controls.Add(referenceLabel);

            xpLabel = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 214, 92),
                Location = new Point(780, 14),
                Size = new Size(240, 30)
            };
            header.Controls.Add(xpLabel);

            starsLabel = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 17, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 215, 0),
                Location = new Point(780, 50),
                Size = new Size(240, 34)
            };
            header.Controls.Add(starsLabel);

            var imagePanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 300,
                BackColor = Color.FromArgb(22, 35, 56)
            };
            Controls.Add(imagePanel);

            captionLabel = new Label
            {
                Left = 16,
                Top = 10,
                Width = 1000,
                Height = 36,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(230, 242, 255),
                BackColor = Color.FromArgb(19, 37, 59),
                BorderStyle = BorderStyle.FixedSingle,
                Text = AppText.Get("ClickImageHint")
            };
            imagePanel.Controls.Add(captionLabel);

            picBoxes = new PictureBox[8];
            for (int i = 0; i < picBoxes.Length; i++)
            {
                var pb = new PictureBox
                {
                    Width = 110,
                    Height = 110,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.FromArgb(34, 52, 77),
                    BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(4)
                };

                int row = i / 4;
                int col = i % 4;
                pb.Left = 95 + col * 210;
                pb.Top = 58 + row * 120;

                int capturedIndex = i;
                pb.Click += (s, e) => ShowCaption(capturedIndex);
                pb.Cursor = Cursors.Hand;

                imagePanel.Controls.Add(pb);
                picBoxes[i] = pb;
            }

            var center = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(14),
                BackColor = Color.FromArgb(16, 24, 38)
            };
            Controls.Add(center);

            solutionLabel = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 120,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.FromArgb(235, 245, 255),
                BackColor = Color.FromArgb(23, 41, 63),
                Padding = new Padding(10),
                Visible = false
            };
            center.Controls.Add(solutionLabel);

            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 74,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, 14, 0, 0)
            };
            center.Controls.Add(buttons);

            revealButton = CreateActionButton(AppText.Get("RevealImages"), Color.FromArgb(52, 152, 219));
            revealButton.Click += RevealButton_Click;
            buttons.Controls.Add(revealButton);

            hintButton = CreateActionButton(AppText.Get("ShowHint"), Color.FromArgb(241, 196, 15));
            hintButton.Click += HintButton_Click;
            buttons.Controls.Add(hintButton);

            solutionButton = CreateActionButton(AppText.Get("RevealSolution"), Color.FromArgb(243, 156, 18));
            solutionButton.Click += SolutionButton_Click;
            buttons.Controls.Add(solutionButton);

            nextButton = CreateActionButton(AppText.Get("NextStory"), Color.FromArgb(39, 174, 96));
            nextButton.Click += NextButton_Click;
            buttons.Controls.Add(nextButton);

            hintPulseTimer = new Timer { Interval = 300 };
            hintPulseTimer.Tick += HintPulseTimer_Tick;
            hintPulseTimer.Start();
        }

        private void HintPulseTimer_Tick(object sender, EventArgs e)
        {
            if (revealed[2])
            {
                hintPulseTimer.Stop();
                picBoxes[7].BackColor = Color.FromArgb(34, 52, 77);
                return;
            }

            hintPulseGrow = !hintPulseGrow;
            picBoxes[7].BackColor = hintPulseGrow
                ? Color.FromArgb(180, 130, 0)   // amber glow
                : Color.FromArgb(34, 52, 77);   // normal dark
        }

        private Button CreateActionButton(string text, Color color)
        {
            return new Button
            {
                Text = text,
                Width = 220,
                Height = 46,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = color,
                ForeColor = Color.White,
                Margin = new Padding(8, 0, 8, 0)
            };
        }

        // Loads a colored PNG from embedded resources by its resource key (filename without extension).
        // Delegate to the central resource loader (StoryResources).
        private static Image GetResourceImage(string key) => StoryResources.GetImage(key);

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (Screen_size.IsFullscreen)
                Screen_size.GoFullscreen(true);
        }

        private void ShowCaption(int slotIndex)
        {
            string text = StoryCaptionPolicy.GetDisplayCaption(story, slotIndex);
            if (string.IsNullOrWhiteSpace(text))
                return;

            captionLabel.Text = "\u25B6  " + text;
            captionLabel.ForeColor = Color.FromArgb(255, 236, 142); // warm gold on click
            captionLabel.BackColor = Color.FromArgb(44, 62, 80);
        }

        private void RenderStory()
        {
            localizedText = StoryLocalizationService.GetText(story);
            Text = AppText.Get("AppTitle");
            captionLabel.Text = AppText.Get("ClickImageHint");
            captionLabel.ForeColor = Color.FromArgb(230, 242, 255);
            captionLabel.BackColor = Color.FromArgb(19, 37, 59);

            // Hide title and scripture reference — player must guess them!
            titleLabel.Text = AppText.Get("EpisodePrefix") + " " + story.Id + "  —  " + AppText.Get("GuessStory");
            referenceLabel.Text = AppText.Get("Category") + ": " + localizedText.Keyword;

            var fallback = GetResourceImage(StoryResources.KeyUnknown); // ❓ placeholder
            string[] visible = story.VisibleEmojis ?? new string[0];
            string[] hidden  = story.HiddenEmojis  ?? new string[0];

            // Slots 0-4: visible colored images
            for (int i = 0; i < 5; i++)
            {
                string key = i < visible.Length ? visible[i] : null;
                picBoxes[i].Image = (string.IsNullOrWhiteSpace(key) ? null : GetResourceImage(key)) ?? fallback;
            }

            // Slots 5-6: hidden — show "?" until revealed
            picBoxes[5].Image = fallback;
            picBoxes[6].Image = fallback;
            picBoxes[5].Tag   = hidden.Length > 0 ? hidden[0] : null;
            picBoxes[6].Tag   = hidden.Length > 1 ? hidden[1] : null;

            // Slot 7: hint — show 🔥 placeholder, pulses amber until clicked
            picBoxes[7].Tag   = story.HintEmoji;
            picBoxes[7].Image = GetResourceImage(StoryResources.KeyHint) ?? fallback; // 🔥 fire = "hot clue"

            // Build solution text with scripture quote if available
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(AppText.Get("Solution") + ": " + localizedText.Solution);
            if (!string.IsNullOrWhiteSpace(localizedText.ScriptureQuote))
            {
                sb.AppendLine();
                sb.AppendLine(localizedText.ScriptureQuote);
            }
            sb.AppendLine();
            sb.Append(AppText.Get("Note") + ": " + localizedText.EngagementNote);
            solutionLabel.Text = sb.ToString();
            solutionLabel.Height = string.IsNullOrWhiteSpace(localizedText.ScriptureQuote) ? 120 : 160;

            revealButton.Text = revealed[0] || revealed[1] ? AppText.Get("HideImages") : AppText.Get("RevealImages");
            hintButton.Text = revealed[2] ? AppText.Get("HideHint") : AppText.Get("ShowHint");
            solutionButton.Text = solutionLabel.Visible ? AppText.Get("HideSolution") : AppText.Get("RevealSolution");
            nextButton.Text = AppText.Get("NextStory");

            UpdateXpLabel();
        }

        private int CalculateXp()
        {
            int used = revealed.Count(x => x);
            int xp = 100 - (used * 20);
            return Math.Max(20, xp);
        }

        private int CalculateStars()
        {
            int used = revealed.Count(x => x);
            return Math.Max(1, 3 - used);
        }

        private void UpdateXpLabel()
        {
            xpLabel.Text = AppText.Get("ExpectedXp") + ": " + CalculateXp();
            int s = CalculateStars();
            starsLabel.Text = new string('\u2605', s) + new string('\u2606', 3 - s);
            starsLabel.ForeColor = s == 3 ? Color.FromArgb(255, 215, 0)
                                 : s == 2 ? Color.FromArgb(200, 200, 200)
                                 :          Color.FromArgb(205, 127, 50);
        }

        private void RevealButton_Click(object sender, EventArgs e)
        {
            if (!revealed[0])
            {
                var img = GetResourceImage(picBoxes[5].Tag as string);
                if (img != null) picBoxes[5].Image = img;
                revealed[0] = true;
                revealButton.Text = AppText.Get("HideImages");
                UpdateXpLabel();
            }
            else if (!revealed[1])
            {
                var img = GetResourceImage(picBoxes[6].Tag as string);
                if (img != null) picBoxes[6].Image = img;
                revealed[1] = true;
                revealButton.Enabled = false;
                revealButton.Text = AppText.Get("HideImages");
                UpdateXpLabel();
            }
        }

        private void HintButton_Click(object sender, EventArgs e)
        {
            if (!revealed[2])
            {
                var img = GetResourceImage(picBoxes[7].Tag as string);
                if (img != null) picBoxes[7].Image = img;
                revealed[2] = true;
                hintButton.Enabled = false;
                hintButton.Text = AppText.Get("HideHint");
                UpdateXpLabel();
            }
        }

        private void SolutionButton_Click(object sender, EventArgs e)
        {
            solutionLabel.Visible = !solutionLabel.Visible;
            solutionButton.Text = solutionLabel.Visible ? AppText.Get("HideSolution") : AppText.Get("RevealSolution");

            // Reveal (or hide) the story title and scripture reference together with the solution panel
            if (solutionLabel.Visible)
            {
                titleLabel.Text = AppText.Get("EpisodePrefix") + " " + story.Id + "  —  " + localizedText.Title;
                referenceLabel.Text = localizedText.ScriptureReference + "   |   " + AppText.Get("Category") + ": " + localizedText.Keyword;
                // Reset caption label to neutral invite color when solution is revealed
                captionLabel.ForeColor = Color.FromArgb(180, 210, 255);
            }
            else
            {
                titleLabel.Text = AppText.Get("EpisodePrefix") + " " + story.Id + "  —  " + AppText.Get("GuessStory");
                referenceLabel.Text = AppText.Get("Category") + ": " + localizedText.Keyword;
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            CompleteStoryIfNeeded();

            var next = StoryEngine.GetNextStory(story.Id);
            Hide();
            if (next != null && next.IsDynamic)
            {
                new DynamicStoryForm(next).ShowDialog();
            }
            else if (next != null)
            {
                new Forms_list().ApriStoria(next.Id);
            }
            else
            {
                new Forms_list().Fine();
            }
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            LanguageManager.LanguageChanged -= LanguageManager_LanguageChanged;
            if (hintPulseTimer != null)
            {
                hintPulseTimer.Stop();
                hintPulseTimer.Dispose();
            }
            CompleteStoryIfNeeded();
        }

        private void CompleteStoryIfNeeded()
        {
            if (storyCompleted)
            {
                return;
            }

            storyCompleted = true;
            ProgressTracker.Instance.CompleteStory(story.Id, CalculateXp(), CalculateStars());
        }
    }
}
