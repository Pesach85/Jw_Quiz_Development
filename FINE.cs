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
    public partial class FINE : Form
    {
        public FINE()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia8();
            this.Close();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia10();
            this.Close();
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

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia9();
            this.Close();
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forms_list().Storia11();
            this.Close();
        }

        private void paginaInizialeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
            Forms_list f0 = new Forms_list();
            f0.Home();
        }

        private void tuttoSchermoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Screen_size.SetState(true);
        }

        private void minimizzaSchermoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Screen_size.SetState(false);
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
            Forms_list f0 = new Forms_list();
            f0.Home();
        }
    }
}
