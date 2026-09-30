using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Jw_Quiz_Development
{
    /// <summary>
    /// Local first-run onboarding flag. File: UserOnboarding.dat next to the executable.
    /// Does not touch UserProgress.dat / admin secrets.
    /// </summary>
    public static class UserOnboardingStore
    {
        public const string FileName = "UserOnboarding.dat";
        public const string CompletedToken = "completed";

        public static string FilePath
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName); }
        }

        public static bool IsCompleted()
        {
            try
            {
                if (!File.Exists(FilePath))
                    return false;
                string raw = File.ReadAllText(FilePath, Encoding.UTF8).Trim();
                return string.Equals(raw, CompletedToken, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        public static void MarkCompleted(string playerName)
        {
            string name = SanitizeName(playerName);
            if (string.IsNullOrWhiteSpace(name))
                name = AppText.Get("OnbGuest");
            File.WriteAllText(FilePath, CompletedToken + "\n" + name, Encoding.UTF8);
        }

        public static void Clear()
        {
            try
            {
                if (File.Exists(FilePath))
                    File.Delete(FilePath);
            }
            catch
            {
                // ignore IO races
            }
        }

        public static string SanitizeName(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return string.Empty;
            string s = raw.Trim();
            if (s.Length > 20)
                s = s.Substring(0, 20);
            s = s.Replace("<", string.Empty).Replace(">", string.Empty);
            return s;
        }
    }

    public sealed class OnboardingForm : Form
    {
        private int step = 1;
        private readonly ComboBox langBox = new ComboBox();
        private readonly TextBox nameBox = new TextBox();
        private readonly ComboBox modeBox = new ComboBox();
        private readonly CheckBox audioBox = new CheckBox();
        private readonly CheckBox motionBox = new CheckBox();
        private readonly Label bodyLabel = new Label();
        private readonly Label stepLabel = new Label();
        private readonly Button nextBtn = new Button();
        private readonly Button skipBtn = new Button();
        private readonly Button backBtn = new Button();

        public string ChosenMode { get; private set; }
        public string ChosenName { get; private set; }
        public bool StartEpisode1 { get; private set; }

        public OnboardingForm()
        {
            ChosenMode = "journey";
            ChosenName = string.Empty;
            Text = AppText.Get("OnbTitle");
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new System.Drawing.Size(480, 340);
            Font = new System.Drawing.Font("Segoe UI", 10F);

            stepLabel.SetBounds(20, 16, 440, 24);
            bodyLabel.SetBounds(20, 48, 440, 120);
            bodyLabel.AutoSize = false;

            langBox.DropDownStyle = ComboBoxStyle.DropDownList;
            langBox.SetBounds(20, 180, 200, 28);
            langBox.Items.AddRange(new object[] { AppText.Get("Italian"), AppText.Get("English") });
            langBox.SelectedIndex = LanguageManager.CurrentLanguage == AppLanguage.English ? 1 : 0;
            langBox.SelectedIndexChanged += (s, e) =>
            {
                LanguageManager.SetLanguage(langBox.SelectedIndex == 1 ? AppLanguage.English : AppLanguage.Italian);
                ApplyTexts();
            };

            nameBox.SetBounds(20, 180, 440, 28);
            nameBox.MaxLength = 20;

            modeBox.DropDownStyle = ComboBoxStyle.DropDownList;
            modeBox.SetBounds(20, 180, 440, 28);
            modeBox.Items.AddRange(new object[] { "quiz", "rebus", "journey" });
            modeBox.SelectedIndex = 2;

            audioBox.SetBounds(20, 180, 440, 28);
            audioBox.Checked = true;
            motionBox.SetBounds(20, 220, 440, 28);

            skipBtn.SetBounds(20, 290, 100, 32);
            skipBtn.Click += (s, e) => Advance(true);
            backBtn.SetBounds(130, 290, 100, 32);
            backBtn.Click += (s, e) => { if (step > 1) { step--; Render(); } };
            nextBtn.SetBounds(340, 290, 120, 32);
            nextBtn.Click += (s, e) => Advance(false);

            Controls.Add(stepLabel);
            Controls.Add(bodyLabel);
            Controls.Add(langBox);
            Controls.Add(nameBox);
            Controls.Add(modeBox);
            Controls.Add(audioBox);
            Controls.Add(motionBox);
            Controls.Add(skipBtn);
            Controls.Add(backBtn);
            Controls.Add(nextBtn);
            ApplyTexts();
            Render();
        }

        private void ApplyTexts()
        {
            Text = AppText.Get("OnbTitle");
            skipBtn.Text = AppText.Get("OnbSkip");
            backBtn.Text = AppText.Get("OnbBack");
            nextBtn.Text = AppText.Get("OnbNext");
            audioBox.Text = AppText.Get("OnbPrefAudio");
            motionBox.Text = AppText.Get("OnbPrefMotion");
            stepLabel.Text = AppText.Get("OnbStepOf").Replace("{n}", step.ToString());
        }

        private void Render()
        {
            ApplyTexts();
            langBox.Visible = step == 1;
            nameBox.Visible = step == 3;
            modeBox.Visible = step == 4;
            audioBox.Visible = step == 6;
            motionBox.Visible = step == 6;
            backBtn.Visible = step > 1 && step < 7;

            if (step == 1)
                bodyLabel.Text = AppText.Get("OnbLangTitle") + "\n\n" + AppText.Get("OnbLangBody");
            else if (step == 2)
            {
                bodyLabel.Text = AppText.Get("OnbDiscTitle") + "\n\n" + AppText.Get("OnbDiscBody");
                nextBtn.Text = AppText.Get("OnbDiscCta");
            }
            else if (step == 3)
                bodyLabel.Text = AppText.Get("OnbNameTitle") + "\n\n" + AppText.Get("OnbNameBody");
            else if (step == 4)
                bodyLabel.Text = AppText.Get("OnbModeTitle") + "\n\n" + AppText.Get("OnbModeBody");
            else if (step == 5)
                bodyLabel.Text = AppText.Get("OnbTutTitle") + "\n\n" + AppText.Get("OnbTutBody");
            else if (step == 6)
                bodyLabel.Text = AppText.Get("OnbPrefTitle");
            else
            {
                bodyLabel.Text = AppText.Get("OnbFinalTitle") + "\n\n" + AppText.Get("OnbFinalBody");
                nextBtn.Text = AppText.Get("OnbStartEp1");
                skipBtn.Text = AppText.Get("OnbExplore");
            }
        }

        private void Advance(bool skip)
        {
            if (step == 3)
            {
                ChosenName = skip ? string.Empty : UserOnboardingStore.SanitizeName(nameBox.Text);
            }
            if (step == 4 && modeBox.SelectedItem != null)
                ChosenMode = modeBox.SelectedItem.ToString();

            if (step >= 7 || (skip && step >= 7))
            {
                Finish(startEp1: !skip && step >= 7);
                return;
            }

            if (step == 7)
            {
                Finish(startEp1: !skip);
                return;
            }

            if (skip && step < 7)
            {
                // skip advances one step (same as web)
            }
            step++;
            if (step > 7)
            {
                Finish(false);
                return;
            }
            Render();
        }

        private void Finish(bool startEp1)
        {
            if (string.IsNullOrWhiteSpace(ChosenName))
                ChosenName = AppText.Get("OnbGuest");
            StartEpisode1 = startEp1;
            UserOnboardingStore.MarkCompleted(ChosenName);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
