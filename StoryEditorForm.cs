using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Jw_Quiz_Development
{
    public class StoryEditorForm : Form
    {
        private Label titleFieldLabel;
        private Label scriptureFieldLabel;
        private Label keywordFieldLabel;
        private Label hintFieldLabel;
        private Label solutionFieldLabel;
        private Label engagementFieldLabel;
        private Label sourceLanguageFieldLabel;
        private Label visibleImagesLabel;
        private Label hiddenImagesLabel;
        private Label hintImageLabel;
        private TextBox titleBox;
        private TextBox scriptureBox;
        private TextBox keywordBox;
        private TextBox hintTextBox;
        private TextBox solutionBox;
        private TextBox engagementBox;
        private ComboBox sourceLanguageBox;
        private Button previewButton;
        private Button saveButton;

        private TextBox[] visibleImageBoxes;
        private TextBox[] hiddenImageBoxes;
        private TextBox hintImageBox;

        public StoryEditorForm()
        {
            LanguageManager.LanguageChanged += LanguageManager_LanguageChanged;
            InitializeUi();
        }

        private void LanguageManager_LanguageChanged(object sender, EventArgs e)
        {
            if (!IsDisposed)
                ApplyLocalization();
        }

        private void InitializeUi()
        {
            Text = AppText.Get("EditorTitle");
            Width = 920;
            Height = 820;
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
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            Controls.Add(panel);

            titleFieldLabel = AddLabel(panel, AppText.Get("EditorFieldTitle"), 0);
            titleBox = AddText(panel, 0);

            scriptureFieldLabel = AddLabel(panel, AppText.Get("EditorFieldReference"), 1);
            scriptureBox = AddText(panel, 1);

            keywordFieldLabel = AddLabel(panel, AppText.Get("EditorFieldKeyword"), 2);
            keywordBox = AddText(panel, 2);

            hintFieldLabel = AddLabel(panel, AppText.Get("EditorFieldHint"), 3);
            hintTextBox = AddText(panel, 3);

            solutionFieldLabel = AddLabel(panel, AppText.Get("EditorFieldSolution"), 4);
            solutionBox = AddMultilineText(panel, 4, 90);

            engagementFieldLabel = AddLabel(panel, AppText.Get("EditorFieldEngagement"), 5);
            engagementBox = AddMultilineText(panel, 5, 80);

            sourceLanguageFieldLabel = AddLabel(panel, AppText.Get("EditorFieldInputLanguage"), 6);
            sourceLanguageBox = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };
            sourceLanguageBox.Items.Add(AppLanguage.Italian);
            sourceLanguageBox.Items.Add(AppLanguage.English);
            sourceLanguageBox.SelectedItem = LanguageManager.CurrentLanguage;
            sourceLanguageBox.Format += SourceLanguageBox_Format;
            panel.Controls.Add(sourceLanguageBox, 1, 6);

            visibleImagesLabel = AddLabel(panel, AppText.Get("EditorVisibleImages"), 7);
            panel.Controls.Add(BuildImageInputRow(out visibleImageBoxes, 5), 1, 7);

            hiddenImagesLabel = AddLabel(panel, AppText.Get("EditorHiddenImages"), 8);
            panel.Controls.Add(BuildImageInputRow(out hiddenImageBoxes, 2), 1, 8);

            hintImageLabel = AddLabel(panel, AppText.Get("EditorHintImage"), 9);
            panel.Controls.Add(BuildSingleImageInput(out hintImageBox), 1, 9);

            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 0)
            };
            panel.Controls.Add(buttons, 1, 10);

            previewButton = new Button
            {
                Text = AppText.Get("Preview"),
                Width = 150,
                Height = 38,
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            previewButton.Click += Preview_Click;
            buttons.Controls.Add(previewButton);

            saveButton = new Button
            {
                Text = AppText.Get("SaveStory"),
                Width = 150,
                Height = 38,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            saveButton.Click += Save_Click;
            buttons.Controls.Add(saveButton);

            ApplyLocalization();
        }

        private void SourceLanguageBox_Format(object sender, ListControlConvertEventArgs e)
        {
            if (!(e.ListItem is AppLanguage))
                return;

            var language = (AppLanguage)e.ListItem;
            e.Value = AppText.Get(language == AppLanguage.English ? "English" : "Italian");
        }

        private void ApplyLocalization()
        {
            Text = AppText.Get("EditorTitle");
            titleFieldLabel.Text = AppText.Get("EditorFieldTitle");
            scriptureFieldLabel.Text = AppText.Get("EditorFieldReference");
            keywordFieldLabel.Text = AppText.Get("EditorFieldKeyword");
            hintFieldLabel.Text = AppText.Get("EditorFieldHint");
            solutionFieldLabel.Text = AppText.Get("EditorFieldSolution");
            engagementFieldLabel.Text = AppText.Get("EditorFieldEngagement");
            sourceLanguageFieldLabel.Text = AppText.Get("EditorFieldInputLanguage");
            visibleImagesLabel.Text = AppText.Get("EditorVisibleImages");
            hiddenImagesLabel.Text = AppText.Get("EditorHiddenImages");
            hintImageLabel.Text = AppText.Get("EditorHintImage");
            previewButton.Text = AppText.Get("Preview");
            saveButton.Text = AppText.Get("SaveStory");
            sourceLanguageBox.Refresh();
        }

        private Label AddLabel(TableLayoutPanel panel, string text, int row)
        {
            var lbl = new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            panel.Controls.Add(lbl, 0, row);
            return lbl;
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

        // Builds a row of image-key inputs, each with a "Scegli..." pick button.
        private Control BuildImageInputRow(out TextBox[] boxes, int count)
        {
            var row = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                WrapContents = true
            };

            boxes = new TextBox[count];
            for (int i = 0; i < count; i++)
            {
                var box = new TextBox
                {
                    Width = 160,
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    TextAlign = HorizontalAlignment.Left
                };
                boxes[i] = box;

                var pick = new Button
                {
                    Text = AppText.Get("ChooseImage"),
                    Width = 72,
                    Height = box.Height + 4,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(52, 73, 94),
                    ForeColor = Color.White,
                    Margin = new Padding(0, 0, 8, 0)
                };
                var capturedBox = box; // capture for closure
                pick.Click += (s, e) =>
                {
                    string chosen = ShowImagePicker();
                    if (chosen != null) capturedBox.Text = chosen;
                };

                var cell = new Panel { Width = 240, Height = 28, Margin = new Padding(0, 2, 0, 2) };
                box.Left = 0; box.Top = 0; box.Width = 160; box.Height = 24;
                pick.Left = 164; pick.Top = 0; pick.Width = 72; pick.Height = 24;
                cell.Controls.Add(box);
                cell.Controls.Add(pick);
                row.Controls.Add(cell);
            }
            return row;
        }

        // Builds a single image-key input with a pick button (for hint).
        private Control BuildSingleImageInput(out TextBox box)
        {
            var row = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true
            };

            box = new TextBox
            {
                Width = 160,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                TextAlign = HorizontalAlignment.Left
            };

            var pick = new Button
            {
                Text = AppText.Get("ChooseImage"),
                Width = 72,
                Height = box.Height + 4,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White
            };
            var capturedBox = box;
            pick.Click += (s, e) =>
            {
                string chosen = ShowImagePicker();
                if (chosen != null) capturedBox.Text = chosen;
            };
            row.Controls.Add(box);
            row.Controls.Add(pick);
            return row;
        }

        // Shows a gallery of all available colored PNG resources; returns the chosen resource key.
        private string ShowImagePicker()
        {
            var keys = GetAvailableImageKeys();
            if (keys.Count == 0)
            {
                MessageBox.Show(AppText.Get("NoImageAvailable"), AppText.Get("ImageGallery"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            string chosen = null;

            using (var picker = new Form())
            {
                picker.Text = AppText.Get("ImageGallery");
                picker.Width = 780;
                picker.Height = 560;
                picker.StartPosition = FormStartPosition.CenterParent;
                picker.BackColor = Color.FromArgb(30, 40, 55);

                var info = new Label
                {
                    Text = AppText.Get("ImageGalleryHint"),
                    Dock = DockStyle.Top,
                    Height = 30,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.FromArgb(200, 220, 255),
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    BackColor = Color.FromArgb(44, 62, 80)
                };
                picker.Controls.Add(info);

                var flow = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    BackColor = Color.FromArgb(22, 35, 56),
                    Padding = new Padding(8)
                };
                picker.Controls.Add(flow);

                foreach (var key in keys)
                {
                    var img = StoryResources.GetImage(key);
                    if (img == null) continue;

                    var cell = new Panel
                    {
                        Width = 100,
                        Height = 120,
                        Margin = new Padding(4),
                        BackColor = Color.FromArgb(34, 52, 77),
                        Cursor = Cursors.Hand
                    };

                    var pb = new PictureBox
                    {
                        Width = 90,
                        Height = 90,
                        Left = 5,
                        Top = 4,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Image = img,
                        BackColor = Color.Transparent,
                        Cursor = Cursors.Hand
                    };

                    // Short label: last part of key (trim long filenames)
                    string displayKey = key.Length > 14 ? key.Substring(0, 12) + "…" : key;
                    var lbl = new Label
                    {
                        Text = displayKey,
                        Left = 2,
                        Top = 96,
                        Width = 96,
                        Height = 22,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Segoe UI", 7, FontStyle.Regular),
                        ForeColor = Color.FromArgb(180, 210, 255),
                        BackColor = Color.Transparent
                    };

                    var capturedKey = key;
                    EventHandler selectAction = (s, e) =>
                    {
                        chosen = capturedKey;
                        picker.DialogResult = DialogResult.OK;
                        picker.Close();
                    };

                    cell.Click += selectAction;
                    pb.Click += selectAction;
                    lbl.Click += selectAction;

                    cell.Controls.Add(pb);
                    cell.Controls.Add(lbl);
                    flow.Controls.Add(cell);
                }

                picker.ShowDialog(this);
            }

            return chosen;
        }

        // Enumerates all image/bitmap resources from the embedded Resources.
        private static List<string> GetAvailableImageKeys()
        {
            var result = new List<string>();
            var rm = Properties.Resources.ResourceManager;
            var rs = rm.GetResourceSet(
                System.Globalization.CultureInfo.InvariantCulture, true, true);
            if (rs != null)
            {
                foreach (DictionaryEntry entry in rs)
                {
                    if (entry.Value is Image)
                        result.Add((string)entry.Key);
                }
            }
            result.Sort();
            return result;
        }

        private Story BuildStoryFromInputs()
        {
            var visible = visibleImageBoxes.Select(x => x.Text.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var hidden  = hiddenImageBoxes.Select(x => x.Text.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            if (visible.Length < 5 || hidden.Length < 2 || string.IsNullOrWhiteSpace(hintImageBox.Text))
            {
                throw new InvalidOperationException("Inserisci 5 immagini visibili, 2 immagini nascoste e 1 immagine indizio.");
            }

            var story = new Story
            {
                VisibleEmojis = visible,
                HiddenEmojis = hidden,
                HintEmoji = hintImageBox.Text.Trim(),
                IsDynamic = true,
                IsUserCreated = true
            };

            var sourceText = new StoryLocalizedText
            {
                Title = titleBox.Text.Trim(),
                ScriptureReference = scriptureBox.Text.Trim(),
                Keyword = keywordBox.Text.Trim(),
                Hint = hintTextBox.Text.Trim(),
                Solution = solutionBox.Text.Trim(),
                EngagementNote = engagementBox.Text.Trim()
            };

            StoryLocalizationService.SetSourceText(story, sourceText, (AppLanguage)sourceLanguageBox.SelectedItem);
            return story;
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
                MessageBox.Show(ex.Message, AppText.Get("IncompleteData"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                Story story = BuildStoryFromInputs();
                Story saved = UserStoryLibrary.AddStory(story);
                MessageBox.Show(string.Format(AppText.Get("SaveStoryMessage"), saved.Id), AppText.Get("SaveCompleted"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            LanguageManager.LanguageChanged -= LanguageManager_LanguageChanged;
        }
    }
}

