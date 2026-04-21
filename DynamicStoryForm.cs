using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Jw_Quiz_Development
{
    public class DynamicStoryForm : Form
    {
        private readonly Story story;
        private readonly bool[] revealed = new bool[3]; // 0/1 = hidden emoji, 2 = hint

        private Label titleLabel;
        private Label referenceLabel;
        private Label[] emojiLabels;
        private Label solutionLabel;
        private Label xpLabel;
        private Button revealButton;
        private Button hintButton;
        private Button solutionButton;
        private Button nextButton;

        private bool storyCompleted;

        public DynamicStoryForm(Story story)
        {
            this.story = story ?? throw new ArgumentNullException(nameof(story));
            InitializeUi();
            RenderStory();
        }

        private void InitializeUi()
        {
            Text = "JW Quiz - Episodio Dinamico";
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

            var emojiPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 260,
                BackColor = Color.FromArgb(22, 35, 56)
            };
            Controls.Add(emojiPanel);

            emojiLabels = new Label[8];
            for (int i = 0; i < emojiLabels.Length; i++)
            {
                var lbl = new Label
                {
                    Width = 110,
                    Height = 110,
                    Font = new Font("Segoe UI Emoji", 46, FontStyle.Regular),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.FromArgb(34, 52, 77),
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle
                };

                int row = i / 4;
                int col = i % 4;
                lbl.Left = 95 + col * 210;
                lbl.Top = 20 + row * 120;
                emojiPanel.Controls.Add(lbl);
                emojiLabels[i] = lbl;
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

            revealButton = CreateActionButton("Rivela 2 simboli", Color.FromArgb(52, 152, 219));
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

        private void RenderStory()
        {
            titleLabel.Text = "Storia " + story.Id + " - " + story.Title;
            referenceLabel.Text = story.ScriptureReference + "   |   Tema: " + story.Keyword;

            string[] visible = story.VisibleEmojis ?? new[] { "❔", "❔", "❔", "❔", "❔" };
            string[] hidden = story.HiddenEmojis ?? new[] { "❔", "❔" };

            for (int i = 0; i < 5; i++)
            {
                emojiLabels[i].Text = i < visible.Length ? visible[i] : "❔";
            }

            emojiLabels[5].Text = "❓";
            emojiLabels[6].Text = "❓";
            emojiLabels[7].Text = "💡";

            emojiLabels[5].Tag = hidden.Length > 0 ? hidden[0] : "❔";
            emojiLabels[6].Tag = hidden.Length > 1 ? hidden[1] : "❔";
            emojiLabels[7].Tag = string.IsNullOrWhiteSpace(story.HintEmoji) ? "💡" : story.HintEmoji;

            solutionLabel.Text = "Soluzione: " + story.Solution + Environment.NewLine + Environment.NewLine +
                                 "Nota: " + story.EngagementNote;
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
                emojiLabels[5].Text = (string)emojiLabels[5].Tag;
                revealed[0] = true;
            }
            else if (!revealed[1])
            {
                emojiLabels[6].Text = (string)emojiLabels[6].Tag;
                revealed[1] = true;
                revealButton.Enabled = false;
            }

            UpdateXpLabel();
        }

        private void HintButton_Click(object sender, EventArgs e)
        {
            if (!revealed[2])
            {
                emojiLabels[7].Text = (string)emojiLabels[7].Tag;
                revealed[2] = true;
                hintButton.Enabled = false;
                UpdateXpLabel();
            }
        }

        private void SolutionButton_Click(object sender, EventArgs e)
        {
            solutionLabel.Visible = !solutionLabel.Visible;
            solutionButton.Text = solutionLabel.Visible ? "Nascondi soluzione" : "Rivela soluzione";
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
