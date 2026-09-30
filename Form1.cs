using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jw_Quiz_Development
{

    public partial class Form1 : Form
    {
        private ProgressPanel progressPanel;
        private ToolStripMenuItem band1_12MenuItem;
        private ToolStripMenuItem band13_18MenuItem;
        private ToolStripMenuItem band19_23MenuItem;
        private ToolStripMenuItem storieUtenteMenuItem;
        private ToolStripMenuItem creaStoriaMenuItem;
        private ToolStripMenuItem linguaMenuItem;
        private ToolStripMenuItem italianoMenuItem;
        private ToolStripMenuItem englishMenuItem;
        private ToolStripMenuItem openWebMenuItem;
        private bool storiesMenuDynamicOk;
        private ToolStripMenuItem[] designerStoryItems;

        public Form1()
        {
            InitializeComponent();
            ApplyBrandTheme();
            LanguageManager.LanguageChanged += LanguageManager_LanguageChanged;
        }

        /// <summary>
        /// HITL B7:A — align Form1 chrome to web immersive palette (oro/blu).
        /// Does not recolor PictureBox rebus grids. Intro.jpg splash remains legacy purple (future work).
        /// </summary>
        private void ApplyBrandTheme()
        {
            Color bg = Color.FromArgb(0x0b, 0x12, 0x20);
            Color mid = Color.FromArgb(0x13, 0x20, 0x33);
            Color ink = Color.FromArgb(0xe8, 0xee, 0xf7);
            this.BackColor = bg;
            this.ForeColor = ink;
            this.Font = new Font("Segoe UI", 9F);
            if (groupBox1 != null)
            {
                groupBox1.BackColor = bg;
                groupBox1.ForeColor = ink;
            }
            if (menuStrip1 != null)
            {
                menuStrip1.BackColor = mid;
                menuStrip1.ForeColor = ink;
                foreach (ToolStripItem item in menuStrip1.Items)
                {
                    item.ForeColor = ink;
                    item.BackColor = mid;
                }
            }
        }

        private void LanguageManager_LanguageChanged(object sender, EventArgs e)
        {
            if (IsDisposed)
                return;

            ApplyLocalization();
            RefreshUserStoriesMenu();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            // Inizializza il Progress Panel
            progressPanel = new ProgressPanel();
            this.Controls.Add(progressPanel);
            
            // Sposta il groupBox1 sopra al panel
            this.groupBox1.Dock = DockStyle.Fill;

            PopulateStoriesMenu();
            BuildLanguageMenu();
            BuildWebBridgeMenu();
            BuildOnboardingResetMenu();
            ApplyLocalization();
            RefreshUserStoriesMenu();
            MaybeShowOnboarding();
        }

        private ToolStripMenuItem resetLocalDataMenuItem;

        private void BuildOnboardingResetMenu()
        {
            if (resetLocalDataMenuItem != null)
                return;
            resetLocalDataMenuItem = new ToolStripMenuItem();
            resetLocalDataMenuItem.Click += ResetLocalOnboarding_Click;
            impostazioniToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
            impostazioniToolStripMenuItem.DropDownItems.Add(resetLocalDataMenuItem);
        }

        private void ResetLocalOnboarding_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                AppText.Get("OnbResetConfirm"),
                AppText.Get("OnbResetLocal"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;
            UserOnboardingStore.Clear();
            MessageBox.Show(AppText.Get("OnbResetDone"), AppText.Get("Settings"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            MaybeShowOnboarding();
        }

        private void MaybeShowOnboarding()
        {
            if (UserOnboardingStore.IsCompleted())
                return;
            using (var dlg = new OnboardingForm())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK && dlg.StartEpisode1)
                {
                    OpenStory(1);
                }
            }
        }

        private void BuildLanguageMenu()
        {
            if (linguaMenuItem != null)
                return;

            linguaMenuItem = new ToolStripMenuItem();
            italianoMenuItem = new ToolStripMenuItem();
            englishMenuItem = new ToolStripMenuItem();

            italianoMenuItem.Click += delegate { LanguageManager.SetLanguage(AppLanguage.Italian); };
            englishMenuItem.Click += delegate { LanguageManager.SetLanguage(AppLanguage.English); };

            linguaMenuItem.DropDownItems.Add(italianoMenuItem);
            linguaMenuItem.DropDownItems.Add(englishMenuItem);
            impostazioniToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
            impostazioniToolStripMenuItem.DropDownItems.Add(linguaMenuItem);
        }

        private void BuildWebBridgeMenu()
        {
            if (openWebMenuItem != null)
                return;

            openWebMenuItem = new ToolStripMenuItem();
            openWebMenuItem.Click += OpenWebImmersive_Click;
            guidaToolStripMenuItem.DropDownItems.Insert(0, openWebMenuItem);
        }

        private void OpenWebImmersive_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://jwquiz.pages.dev/");
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("https://jwquiz.pages.dev/");
            }
        }

        private void ApplyLocalization()
        {
            Text = AppText.Get("AppTitle");
            menuToolStripMenuItem.Text = AppText.Get("Menu");
            storieToolStripMenuItem.Text = AppText.Get("Stories");
            esciToolStripMenuItem.Text = AppText.Get("Exit");
            impostazioniToolStripMenuItem.Text = AppText.Get("Settings");
            guidaToolStripMenuItem.Text = AppText.Get("Guide");
            aiutoToolStripMenuItem.Text = AppText.Get("Help");
            tuttoschermoToolStripMenuItem.Text = AppText.Get("Fullscreen");
            minimizzaSchermoToolStripMenuItem.Text = AppText.Get("Windowed");
            statisticheToolStripMenuItem.Text = AppText.Get("StatsTitle");

            storia1ToolStripMenuItem.Text = AppText.Get("StoryPrefix") + " 1";
            storia2ToolStripMenuItem.Text = AppText.Get("StoryPrefix") + " 2";
            storia2ToolStripMenuItem1.Text = AppText.Get("StoryPrefix") + " 3";
            storia2ToolStripMenuItem2.Text = AppText.Get("StoryPrefix") + " 4";
            storia2ToolStripMenuItem3.Text = AppText.Get("StoryPrefix") + " 5";
            storia2ToolStripMenuItem4.Text = AppText.Get("StoryPrefix") + " 6";
            storia2ToolStripMenuItem5.Text = AppText.Get("StoryPrefix") + " 7";
            storia2ToolStripMenuItem6.Text = AppText.Get("StoryPrefix") + " 8";
            storia9ToolStripMenuItem.Text = AppText.Get("StoryPrefix") + " 9";
            toolStripMenuItem1.Text = AppText.Get("StoryPrefix") + " 10";
            toolStripMenuItem2.Text = AppText.Get("StoryPrefix") + " 11";
            storia10ToolStripMenuItem.Text = AppText.Get("StoryPrefix") + " 12";

            if (storiesMenuDynamicOk)
            {
                if (band1_12MenuItem != null)
                    band1_12MenuItem.Text = AppText.Get("StoriesBand1_12");
                if (band13_18MenuItem != null)
                    band13_18MenuItem.Text = AppText.Get("StoriesBand13_18");
                if (band19_23MenuItem != null)
                    band19_23MenuItem.Text = AppText.Get("StoriesBand19_23");
                LocalizeBandItems(band1_12MenuItem);
                LocalizeBandItems(band13_18MenuItem);
                LocalizeBandItems(band19_23MenuItem);
            }

            if (creaStoriaMenuItem != null)
                creaStoriaMenuItem.Text = AppText.Get("CreateStory");
            if (storieUtenteMenuItem != null)
                storieUtenteMenuItem.Text = AppText.Get("UserStories");
            linguaMenuItem.Text = AppText.Get("Language");
            italianoMenuItem.Text = AppText.Get("Italian");
            englishMenuItem.Text = AppText.Get("English");
            if (openWebMenuItem != null)
                openWebMenuItem.Text = AppText.Get("OpenWebImmersive");
            if (resetLocalDataMenuItem != null)
                resetLocalDataMenuItem.Text = AppText.Get("OnbResetLocal");
            italianoMenuItem.Checked = LanguageManager.CurrentLanguage == AppLanguage.Italian;
            englishMenuItem.Checked = LanguageManager.CurrentLanguage == AppLanguage.English;
        }

        private static void LocalizeBandItems(ToolStripMenuItem band)
        {
            if (band == null)
                return;
            foreach (ToolStripItem item in band.DropDownItems)
            {
                if (item.Tag is int id)
                    item.Text = AppText.Get("StoryPrefix") + " " + id;
            }
        }

        /// <summary>
        /// A+D leggero: tre fasce da StoryEngine; Designer 1–12 nascosti (non rimossi).
        /// Fallback: ripristina menu Designer + Nuovi Episodi se populate fallisce.
        /// </summary>
        private void PopulateStoriesMenu()
        {
            if (creaStoriaMenuItem != null)
                return;

            designerStoryItems = new[]
            {
                storia1ToolStripMenuItem,
                storia2ToolStripMenuItem,
                storia2ToolStripMenuItem1,
                storia2ToolStripMenuItem2,
                storia2ToolStripMenuItem3,
                storia2ToolStripMenuItem4,
                storia2ToolStripMenuItem5,
                storia2ToolStripMenuItem6,
                storia9ToolStripMenuItem,
                toolStripMenuItem1,
                toolStripMenuItem2,
                storia10ToolStripMenuItem
            };

            try
            {
                foreach (var item in designerStoryItems)
                    item.Visible = false;

                storieToolStripMenuItem.DropDownItems.Clear();

                band1_12MenuItem = BuildStoryBand(AppText.Get("StoriesBand1_12"), 1, 12);
                band13_18MenuItem = BuildStoryBand(AppText.Get("StoriesBand13_18"), 13, 18);
                band19_23MenuItem = BuildStoryBand(AppText.Get("StoriesBand19_23"), 19, 23);

                creaStoriaMenuItem = new ToolStripMenuItem(AppText.Get("CreateStory"));
                creaStoriaMenuItem.Click += CreaStoriaMenuItem_Click;
                storieUtenteMenuItem = new ToolStripMenuItem(AppText.Get("UserStories"));

                storieToolStripMenuItem.DropDownItems.Add(band1_12MenuItem);
                storieToolStripMenuItem.DropDownItems.Add(band13_18MenuItem);
                storieToolStripMenuItem.DropDownItems.Add(band19_23MenuItem);
                storieToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
                storieToolStripMenuItem.DropDownItems.Add(creaStoriaMenuItem);
                storieToolStripMenuItem.DropDownItems.Add(storieUtenteMenuItem);

                storiesMenuDynamicOk = true;
            }
            catch
            {
                storiesMenuDynamicOk = false;
                RestoreDesignerStoriesMenuFallback();
            }
        }

        private ToolStripMenuItem BuildStoryBand(string title, int idFrom, int idTo)
        {
            var band = new ToolStripMenuItem(title);
            var stories = StoryEngine.GetAllStories()
                .Where(s => s.Id >= idFrom && s.Id <= idTo)
                .OrderBy(s => s.Id);
            foreach (var story in stories)
            {
                int capturedId = story.Id;
                var item = new ToolStripMenuItem(AppText.Get("StoryPrefix") + " " + capturedId);
                item.Tag = capturedId;
                item.Click += (s, e) => OpenStory(capturedId);
                band.DropDownItems.Add(item);
            }
            return band;
        }

        private void RestoreDesignerStoriesMenuFallback()
        {
            storieToolStripMenuItem.DropDownItems.Clear();
            if (designerStoryItems != null)
            {
                foreach (var item in designerStoryItems)
                {
                    item.Visible = true;
                    storieToolStripMenuItem.DropDownItems.Add(item);
                }
            }

            var nuovi = new ToolStripMenuItem(AppText.Get("NewEpisodes"));
            foreach (var dyn in StoryEngine.GetDynamicStories())
            {
                int capturedId = dyn.Id;
                var item = new ToolStripMenuItem(AppText.Get("StoryPrefix") + " " + capturedId);
                item.Click += (s, e) => OpenStory(capturedId);
                nuovi.DropDownItems.Add(item);
            }

            creaStoriaMenuItem = new ToolStripMenuItem(AppText.Get("CreateStory"));
            creaStoriaMenuItem.Click += CreaStoriaMenuItem_Click;
            storieUtenteMenuItem = new ToolStripMenuItem(AppText.Get("UserStories"));

            storieToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
            storieToolStripMenuItem.DropDownItems.Add(nuovi);
            storieToolStripMenuItem.DropDownItems.Add(creaStoriaMenuItem);
            storieToolStripMenuItem.DropDownItems.Add(storieUtenteMenuItem);
        }

        private void RefreshUserStoriesMenu()
        {
            if (storieUtenteMenuItem == null)
                return;

            storieUtenteMenuItem.DropDownItems.Clear();
            var userStories = UserStoryLibrary.GetUserStories();
            if (userStories.Count == 0)
            {
                var empty = new ToolStripMenuItem(AppText.Get("NoUserStories"));
                empty.Enabled = false;
                storieUtenteMenuItem.DropDownItems.Add(empty);
                return;
            }

            foreach (var story in userStories)
            {
                var item = new ToolStripMenuItem("#" + story.Id + " - " + StoryLocalizationService.GetText(story).Title);
                int capturedId = story.Id;
                item.Click += (s, e) => OpenStory(capturedId);
                storieUtenteMenuItem.DropDownItems.Add(item);
            }
        }

        private void CreaStoriaMenuItem_Click(object sender, EventArgs e)
        {
            using (var editor = new StoryEditorForm())
            {
                if (editor.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshUserStoriesMenu();
                }
            }
        }

        private void OpenStory(int storyId)
        {
            this.Hide();
            new Forms_list().ApriStoria(storyId);
            this.Close();
        }

        private void tuttoschermoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Screen_size.SetState(true);
        }

        private void minimizzaSchermoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Screen_size.SetState(false);
        }

        private void esciToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }

        private void storia1ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenStory(1);
        }

        private void storia2ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenStory(2);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenStory(1);
        }

        private void storia2ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenStory(3);
        }

        private void storia2ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            OpenStory(4);
        }

        private void storia2ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            OpenStory(5);
        }

        private void storia2ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            OpenStory(6);
        }

        private void storia2ToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            OpenStory(7);
        }

        private void storia2ToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            OpenStory(8);
        }

        private void storia9ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenStory(9);
        }

        private void storia10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenStory(12);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            OpenStory(11);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenStory(10);
        }

        private void statisticheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tracker = ProgressTracker.Instance;
            string stats = AppText.Get("StatsPlayer") + "\n\n";
            stats += AppText.Get("StatsLevel") + ": " + tracker.GetLevel() + "\n";
            stats += AppText.Get("StatsXp") + ": " + tracker.CurrentXP + "\n";
            stats += AppText.Get("StatsProgress") + ": " + tracker.CompletedStories.Count + "/" + StoryEngine.TotalStories + " " + AppText.Get("Stories").ToLowerInvariant() + "\n";
            stats += AppText.Get("StatsPercentage") + ": " + tracker.GetProgressPercentage() + "%\n";
            stats += AppText.Get("StatsStartDate") + ": " + tracker.StartDate.ToString("dd/MM/yyyy HH:mm") + "\n";
            stats += AppText.Get("StatsBadges") + ": " + tracker.UnlockedBadges.Count + "\n\n";
            stats += AppText.Get("StatsCompletedStories") + ":\n";
            
            var completedIds = tracker.CompletedStories.OrderBy(x => x).ToList();
            foreach (int id in completedIds)
            {
                var story = StoryEngine.GetStory(id);
                if (story != null)
                {
                    int attempts = tracker.StoryAttempts.ContainsKey(id) ? tracker.StoryAttempts[id] : 0;
                    int sr = tracker.GetStarRating(id);
                    string starStr = sr > 0 ? "  " + new string('\u2605', sr) + new string('\u2606', 3 - sr) : "";
                    stats += "  " + id + ". " + StoryLocalizationService.GetText(story).Title + " (" + attempts + " " + AppText.Get("Times") + ")" + starStr + "\n";
                }
            }

            MessageBox.Show(stats, AppText.Get("StatsTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            LanguageManager.LanguageChanged -= LanguageManager_LanguageChanged;
        }
    }
}
