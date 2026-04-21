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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia2();
            this.Close();
        }

        private void PaginaInizialeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
            Forms_list f0 = new Forms_list();
            f0.Home();
        }

        private void EsciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
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

        private void Button3_Click(object sender, EventArgs e)
        {
            if (!pictureBox8.Visible)
            {
                button3.Text = "Nascondi indizio";
                pictureBox8.Visible = true;
            }
            else
            {
                button3.Text = "Rivela indizio";
                pictureBox8.Visible = false;
            }
        }

        private void Button4_Click(object sender, EventArgs e)
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

        private void Button1_Click_1(object sender, EventArgs e)
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

        private void Button3_Click_1(object sender, EventArgs e)
        {
            if (!pictureBox8.Visible)
            {
                button3.Text = "Nascondi indizio";
                pictureBox8.Visible = true;
            }
            else
            {
                button3.Text = "Rivela indizio";
                pictureBox8.Visible = false;
            }
        }

        private void Button4_Click_1(object sender, EventArgs e)
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

        private void Button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
            Forms_list f2 = new Forms_list();
            f2.Storia2();
        }

        private void storia3ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia3();
            this.Close();
        }

        private void storia3ToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia4();
            this.Close();
        }

        private void storia3ToolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia5();
            this.Close();
        }

        private void storia3ToolStripMenuItem3_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia6();
            this.Close();
        }

        private void storia3ToolStripMenuItem4_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia7();
            this.Close();
        }

        private void storia3ToolStripMenuItem5_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia8();
            this.Close();
        }

        private void storia3ToolStripMenuItem6_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia9();
            this.Close();
        }

        private void storia3ToolStripMenuItem7_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia10();
            this.Close();
        }

        private void storia11ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia12();
            this.Close();
        }

        private void storia2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia2();
            this.Close();
        }

        private void tuttoSchermoToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            Screen_size.SetState(true);
        }

        private void minimizzaSchermoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Screen_size.SetState(false);
        }

        private void esciToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia11();
            this.Close();
        }
    }
}
