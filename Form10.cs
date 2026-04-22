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
    public partial class Form10 : Form
    {
        private int StoryId = 9;
        private bool storyCompleted = false;
        private LegacyHintAnimator hintAnimator;

        public Form10()
        {
            InitializeComponent();
            hintAnimator = new LegacyHintAnimator(pictureBox8, button3);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (hintAnimator != null)
            {
                hintAnimator.Dispose();
                hintAnimator = null;
            }
            base.OnFormClosing(e);
            if (!storyCompleted)
            {
                ProgressTracker.Instance.CompleteStory(StoryId);
                storyCompleted = true;
            }
        }

        private void Form10_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
            Forms_list f10 = new Forms_list();
            f10.Storia10();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!pictureBox3.Visible)
            {
                button1.Text = "Nascondi 2 simboli";
                pictureBox3.Visible = true;
                pictureBox5.Visible = true;
            }
            else
            {
                button1.Text = "Rivela 2 simboli";
                pictureBox3.Visible = false;
                pictureBox5.Visible = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (hintAnimator != null)
                hintAnimator.Toggle();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!label1.Visible)
            {
                button4.Text = "Nascondi soluzione";
                label1.Visible = true;
            }
            else
            {
                button4.Text = "Rivela soluzione";
                label1.Visible = false;
            }
        }

        private void tuttoSchermoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Screen_size.SetState(true);
        }

        private void minimizzaSchermoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Screen_size.SetState(false);
        }

        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia1();
            this.Close();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia2();
            this.Close();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia3();
            this.Close();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia4();
            this.Close();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia5();
            this.Close();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia6();
            this.Close();
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia7();
            this.Close();
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia8();
            this.Close();
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia10();
            this.Close();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia12();
            this.Close();
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
            Forms_list f0 = new Forms_list();
            f0.Home();
        }

        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia11();
            this.Close();
        }
    }
}
