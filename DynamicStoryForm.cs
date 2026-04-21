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

        private Label titleLabel;
        private Label referenceLabel;
        private PictureBox[] picBoxes;   // 0-4 visible, 5-6 hidden, 7 hint
        private Label solutionLabel;
        private Label xpLabel;
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
            InitializeUi();
            RenderStory();
        }

        private void InitializeUi()
        {
            Text = "JW Quiz - Indovina la Storia";
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
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 214, 92),
                Location = new Point(780, 25),
                Size = new Size(240, 40)
            };
            header.Controls.Add(xpLabel);

            var imagePanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 260,
                BackColor = Color.FromArgb(22, 35, 56)
            };
            Controls.Add(imagePanel);

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
                pb.Top = 20 + row * 120;
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

            revealButton = CreateActionButton("Rivela 2 immagini", Color.FromArgb(52, 152, 219));
            revealButton.Click += RevealButton_Click;
            buttons.Controls.Add(revealButton);

            hintButton = CreateActionButton("Mostra indizio", Color.FromArgb(241, 196, 15));
            hintButton.Click += HintButton_Click;
            buttons.Controls.Add(hintButton);

            solutionButton = CreateActionButton("Rivela soluzione", Color.FromArgb(243, 156, 18));
            solutionButton.Click += SolutionButton_Click;
            buttons.Controls.Add(solutionButton);

            nextButton = CreateActionButton("Prossima storia", Color.FromArgb(39, 174, 96));
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
        private static Image GetResourceImage(string resourceKey)
        {
            if (string.IsNullOrWhiteSpace(resourceKey)) return null;
            return Properties.Resources.ResourceManager.GetObject(resourceKey) as Image;
        }

        private void RenderStory()
        {
            // Hide title and scripture reference — player must guess them!
            titleLabel.Text = "Episodio " + story.Id + "  —  Indovina la storia!";
            referenceLabel.Text = "Categoria: " + story.Keyword;

            var fallback = GetResourceImage("2753"); // ❓ placeholder
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
            picBoxes[7].Image = GetResourceImage("1F525") ?? fallback; // 🔥 fire = "hot clue"

            solutionLabel.Text = "Soluzione: " + story.Solution
                + Environment.NewLine + Environment.NewLine
                + "Nota: " + story.EngagementNote;
            UpdateXpLabel();
        }

        private int CalculateXp()
        {
            int used = revealed.Count(x => x);
            int xp = 100 - (used * 20);
            return Math.Max(20, xp);
        }

        private void UpdateXpLabel()
        {
            xpLabel.Text = "XP Previsti: " + CalculateXp();
        }

        private void RevealButton_Click(object sender, EventArgs e)
        {
            if (!revealed[0])
            {
                var img = GetResourceImage(picBoxes[5].Tag as string);
                if (img != null) picBoxes[5].Image = img;
                revealed[0] = true;
                UpdateXpLabel();
            }
            else if (!revealed[1])
            {
                var img = GetResourceImage(picBoxes[6].Tag as string);
                if (img != null) picBoxes[6].Image = img;
                revealed[1] = true;
                revealButton.Enabled = false;
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
                UpdateXpLabel();
            }
        }

        private void SolutionButton_Click(object sender, EventArgs e)
        {
            solutionLabel.Visible = !solutionLabel.Visible;
            solutionButton.Text = solutionLabel.Visible ? "Nascondi soluzione" : "Rivela soluzione";

            // Reveal (or hide) the story title and scripture reference together with the solution panel
            if (solutionLabel.Visible)
            {
                titleLabel.Text = "Episodio " + story.Id + "  —  " + story.Title;
                referenceLabel.Text = story.ScriptureReference + "   |   Categoria: " + story.Keyword;
            }
            else
            {
                titleLabel.Text = "Episodio " + story.Id + "  —  Indovina la storia!";
                referenceLabel.Text = "Categoria: " + story.Keyword;
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
            ProgressTracker.Instance.CompleteStory(story.Id, CalculateXp());
        }
    }
}
