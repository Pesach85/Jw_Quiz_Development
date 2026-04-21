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

        public Form1()
        {
            InitializeComponent();
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
            RefreshUserStoriesMenu();
        }

        private void BuildDynamicMenus()
        {
            if (nuoviEpisodiMenuItem != null)
                return;

            nuoviEpisodiMenuItem = new ToolStripMenuItem("Nuovi Episodi");
            for (int id = 13; id <= 18; id++)
            {
                int capturedId = id;
                var item = new ToolStripMenuItem("Storia " + capturedId);
                item.Click += (s, e) => OpenStory(capturedId);
                nuoviEpisodiMenuItem.DropDownItems.Add(item);
            }

            creaStoriaMenuItem = new ToolStripMenuItem("Crea Nuova Storia");
            creaStoriaMenuItem.Click += CreaStoriaMenuItem_Click;

            storieUtenteMenuItem = new ToolStripMenuItem("Storie Utente");

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
                var empty = new ToolStripMenuItem("Nessuna storia utente");
                empty.Enabled = false;
                storieUtenteMenuItem.DropDownItems.Add(empty);
                return;
            }

            foreach (var story in userStories)
            {
                var item = new ToolStripMenuItem("#" + story.Id + " - " + story.Title);
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
            this.Hide();
            Forms_list f1 = new Forms_list();
            f1.Storia1();
            this.Close();
        }

        private void storia2ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia2();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();          
            Forms_list f1 = new Forms_list();
            f1.Storia1();
            this.Close();
        }

        private void storia2ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia3();
            this.Close();
        }

        private void storia2ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia4();
            this.Close();
        }

        private void storia2ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia5();
            this.Close();
        }

        private void storia2ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia6();
            this.Close();
        }

        private void storia2ToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia7();
            this.Close();
        }

        private void storia2ToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia8();
            this.Close();
        }

        private void storia9ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia9();
            this.Close();
        }

        private void storia10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia12();
            this.Close();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia11();
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia10();
            this.Close();
        }

        private void statisticheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tracker = ProgressTracker.Instance;
            string stats = $"STATISTICHE GIOCATORE\n\n";
            stats += $"Livello: {tracker.GetLevel()}\n";
            stats += $"Esperienza (XP): {tracker.CurrentXP}\n";
            stats += $"Progresso: {tracker.CompletedStories.Count}/12 storie\n";
            stats += $"Percentuale: {tracker.GetProgressPercentage()}%\n";
            stats += $"Data Inizio: {tracker.StartDate:dd/MM/yyyy HH:mm}\n";
            stats += $"Badge Sbloccati: {tracker.UnlockedBadges.Count}\n\n";
            stats += $"STORIE COMPLETATE:\n";
            
            var completedIds = tracker.CompletedStories.OrderBy(x => x).ToList();
            foreach (int id in completedIds)
            {
                var story = StoryEngine.GetStory(id);
                if (story != null)
                {
                    int attempts = tracker.StoryAttempts.ContainsKey(id) ? tracker.StoryAttempts[id] : 0;
                    stats += $"  {id}. {story.Title} ({attempts} volte)\n";
                }
            }

            MessageBox.Show(stats, "Statistiche Dettagliate", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
