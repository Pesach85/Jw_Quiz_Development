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
        private ToolStripMenuItem nuoviEpisodiMenuItem;
        private ToolStripMenuItem storieUtenteMenuItem;
        private ToolStripMenuItem creaStoriaMenuItem;
        private ToolStripMenuItem linguaMenuItem;
        private ToolStripMenuItem italianoMenuItem;
        private ToolStripMenuItem englishMenuItem;

        public Form1()
        {
            InitializeComponent();
            LanguageManager.LanguageChanged += LanguageManager_LanguageChanged;
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

            BuildDynamicMenus();
            BuildLanguageMenu();
            ApplyLocalization();
            RefreshUserStoriesMenu();
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

            nuoviEpisodiMenuItem.Text = AppText.Get("NewEpisodes");
            creaStoriaMenuItem.Text = AppText.Get("CreateStory");
            storieUtenteMenuItem.Text = AppText.Get("UserStories");
            linguaMenuItem.Text = AppText.Get("Language");
            italianoMenuItem.Text = AppText.Get("Italian");
            englishMenuItem.Text = AppText.Get("English");
            italianoMenuItem.Checked = LanguageManager.CurrentLanguage == AppLanguage.Italian;
            englishMenuItem.Checked = LanguageManager.CurrentLanguage == AppLanguage.English;

            for (int id = 13; id <= 18 && id - 13 < nuoviEpisodiMenuItem.DropDownItems.Count; id++)
            {
                nuoviEpisodiMenuItem.DropDownItems[id - 13].Text = AppText.Get("StoryPrefix") + " " + id;
            }
        }

        private void BuildDynamicMenus()
        {
            if (nuoviEpisodiMenuItem != null)
                return;

            nuoviEpisodiMenuItem = new ToolStripMenuItem(AppText.Get("NewEpisodes"));
            for (int id = 13; id <= 18; id++)
            {
                int capturedId = id;
                var item = new ToolStripMenuItem(AppText.Get("StoryPrefix") + " " + capturedId);
                item.Click += (s, e) => OpenStory(capturedId);
                nuoviEpisodiMenuItem.DropDownItems.Add(item);
            }

            creaStoriaMenuItem = new ToolStripMenuItem(AppText.Get("CreateStory"));
            creaStoriaMenuItem.Click += CreaStoriaMenuItem_Click;

            storieUtenteMenuItem = new ToolStripMenuItem(AppText.Get("UserStories"));

            storieToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
            storieToolStripMenuItem.DropDownItems.Add(nuoviEpisodiMenuItem);
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
