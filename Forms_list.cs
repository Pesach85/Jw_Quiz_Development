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

    public partial class Screen_size
    {
        public static bool SetState(bool state)
        {
            if (state)
            {
                GoFullscreen(true);
                return true;
            }
            else
            {
                GoFullscreen(false);
                return false;
            }
        }

        public static void GoFullscreen(bool fullscreen)
        {
            if (Form.ActiveForm != null)
            {
                if (fullscreen)
                {
                    Form.ActiveForm.WindowState = FormWindowState.Normal;
                    Form.ActiveForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    Form.ActiveForm.Bounds = Screen.PrimaryScreen.Bounds;
                }
                else
                {
                    Form.ActiveForm.WindowState = FormWindowState.Maximized;
                    Form.ActiveForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                }
            }
        }
    }

    public partial class Forms_list : Form
    {

        public void Home()
        {
            Form1 f1 = new Form1();
            f1.ShowDialog();
            f1.PerformAutoScale();
        }

        public void Storia1()
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
            f2.PerformAutoScale();
        }

        public void Storia2()
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
            f3.PerformAutoScale();
        }

        public void Storia3()
        {
            Form4 f4 = new Form4();
            f4.ShowDialog();
            f4.PerformAutoScale();
        }

        public void Storia4()
        {
            Form5 f5 = new Form5();
            f5.ShowDialog();
            f5.PerformAutoScale();
        }

        public void Storia5()
        {
            Form6 f6 = new Form6();
            f6.ShowDialog();
            f6.PerformAutoScale();
        }

        public void Storia6()
        {
            Form7 f7 = new Form7();
            f7.ShowDialog();
            f7.PerformAutoScale();
        }

        public void Storia7()
        {
            Form8 f8 = new Form8();
            f8.ShowDialog();
            f8.PerformAutoScale();
        }

        public void Storia8()
        {
            Form9 f9 = new Form9();
            f9.ShowDialog();
            f9.PerformAutoScale();
        }

        public void Storia9()
        {
            Form10 f10 = new Form10();
            f10.ShowDialog();
            f10.PerformAutoScale();
        }

        public void Storia10()
        {
            Form11 f11 = new Form11();
            f11.ShowDialog();
            f11.PerformAutoScale();
        }

        public void Storia11()
        {
            Form12 f12 = new Form12();
            f12.ShowDialog();
            f12.PerformAutoScale();
        }

        public void Storia12()
        {
            Form13 f13 = new Form13();
            f13.ShowDialog();
            f13.PerformAutoScale();
        }

        public void Fine()
        {
            FINE fine = new FINE();
            fine.ShowDialog();
            fine.PerformAutoScale();
        }

    }
}
