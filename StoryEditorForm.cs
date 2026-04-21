using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Jw_Quiz_Development
{
    public class StoryEditorForm : Form
    {
        private TextBox titleBox;
        private TextBox scriptureBox;
        private TextBox keywordBox;
        private TextBox hintTextBox;
        private TextBox solutionBox;
        private TextBox engagementBox;

        private TextBox[] visibleEmojiBoxes;
        private TextBox[] hiddenEmojiBoxes;
        private TextBox hintEmojiBox;

        public StoryEditorForm()
        {
            InitializeUi();
        }

        private void InitializeUi()
        {
            Text = "Crea Nuova Storia";
            Width = 860;
            Height = 760;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(248, 250, 255);

            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                ColumnCount = 2,
                RowCount = 16,
                Padding = new Padding(20)
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            Controls.Add(panel);

            AddLabel(panel, "Titolo", 0);
            titleBox = AddText(panel, 0);

            AddLabel(panel, "Riferimento", 1);
            scriptureBox = AddText(panel, 1);

            AddLabel(panel, "Parola chiave", 2);
            keywordBox = AddText(panel, 2);

            AddLabel(panel, "Indizio testo", 3);
            hintTextBox = AddText(panel, 3);

            AddLabel(panel, "Soluzione", 4);
            solutionBox = AddMultilineText(panel, 4, 90);

            AddLabel(panel, "Nota engagement", 5);
            engagementBox = AddMultilineText(panel, 5, 80);

            AddLabel(panel, "5 emoji visibili", 6);
            panel.Controls.Add(BuildEmojiInputRow(out visibleEmojiBoxes, 5), 1, 6);

            AddLabel(panel, "2 emoji nascosti", 7);
            panel.Controls.Add(BuildEmojiInputRow(out hiddenEmojiBoxes, 2), 1, 7);

            AddLabel(panel, "Emoji suggerimento", 8);
            hintEmojiBox = AddText(panel, 8);
            hintEmojiBox.MaxLength = 4;

            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true
            };
            panel.Controls.Add(buttons, 1, 10);

            var preview = new Button
            {
                Text = "Anteprima",
                Width = 150,
                Height = 38,
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            preview.Click += Preview_Click;
            buttons.Controls.Add(preview);

            var save = new Button
            {
                Text = "Salva Storia",
                Width = 150,
                Height = 38,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            save.Click += Save_Click;
            buttons.Controls.Add(save);
        }

        private void AddLabel(TableLayoutPanel panel, string text, int row)
        {
            var lbl = new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            panel.Controls.Add(lbl, 0, row);
        }

        private TextBox AddText(TableLayoutPanel panel, int row)
        {
            var box = new TextBox
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };
            panel.Controls.Add(box, 1, row);
            return box;
        }

        private TextBox AddMultilineText(TableLayoutPanel panel, int row, int height)
        {
            var box = new TextBox
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Multiline = true,
                Height = height,
                ScrollBars = ScrollBars.Vertical
            };
            panel.Controls.Add(box, 1, row);
            return box;
        }

        private Control BuildEmojiInputRow(out TextBox[] boxes, int count)
        {
            var row = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true
            };

            boxes = new TextBox[count];
            for (int i = 0; i < count; i++)
            {
                boxes[i] = new TextBox
                {
                    Width = 56,
                    Font = new Font("Segoe UI Emoji", 14, FontStyle.Regular),
                    MaxLength = 4,
                    TextAlign = HorizontalAlignment.Center
                };
                row.Controls.Add(boxes[i]);
            }
            return row;
        }

        private Story BuildStoryFromInputs()
        {
            var visible = visibleEmojiBoxes.Select(x => x.Text.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var hidden = hiddenEmojiBoxes.Select(x => x.Text.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            if (visible.Length < 5 || hidden.Length < 2 || string.IsNullOrWhiteSpace(hintEmojiBox.Text))
            {
                throw new InvalidOperationException("Inserisci 5 emoji visibili, 2 emoji nascoste e 1 emoji suggerimento.");
            }

            return new Story
            {
                Title = titleBox.Text.Trim(),
                ScriptureReference = scriptureBox.Text.Trim(),
                Keyword = keywordBox.Text.Trim(),
                Hint = hintTextBox.Text.Trim(),
                Solution = solutionBox.Text.Trim(),
                EngagementNote = engagementBox.Text.Trim(),
                VisibleEmojis = visible,
                HiddenEmojis = hidden,
                HintEmoji = hintEmojiBox.Text.Trim(),
                IsDynamic = true,
                IsUserCreated = true
            };
        }

        private void Preview_Click(object sender, EventArgs e)
        {
            try
            {
                Story previewStory = BuildStoryFromInputs();
                previewStory.Id = 0;
                using (var previewForm = new DynamicStoryForm(previewStory))
                {
                    previewForm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Dati incompleti", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                Story story = BuildStoryFromInputs();
                Story saved = UserStoryLibrary.AddStory(story);
                MessageBox.Show("Storia salvata con ID " + saved.Id + ".", "Salvataggio completato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
