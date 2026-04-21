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
        public Form1()
        {
            InitializeComponent();
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
    }
}
