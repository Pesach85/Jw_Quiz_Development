using System.Windows.Forms;

namespace Jw_Quiz_Development
{

    public partial class Screen_size
    {
        public static bool IsFullscreen { get; private set; }

        public static bool SetState(bool state)
        {
            IsFullscreen = state;
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
        // Restore fullscreen when a new form opens if the session is in fullscreen mode.
        private static void ShowForm(Form f)
        {
            if (Screen_size.IsFullscreen)
                f.Shown += (s, e) => Screen_size.GoFullscreen(true);
            f.ShowDialog();
            f.PerformAutoScale();
        }

        public void Home() { ShowForm(new Form1()); }

        public void Storia1()  { ShowDynamicStory(1);  }
        public void Storia2()  { ShowDynamicStory(2);  }
        public void Storia3()  { ShowDynamicStory(3);  }
        public void Storia4()  { ShowDynamicStory(4);  }
        public void Storia5()  { ShowDynamicStory(5);  }
        public void Storia6()  { ShowDynamicStory(6);  }
        public void Storia7()  { ShowDynamicStory(7);  }
        public void Storia8()  { ShowDynamicStory(8);  }
        public void Storia9()  { ShowDynamicStory(9);  }
        public void Storia10() { ShowDynamicStory(10); }
        public void Storia11() { ShowDynamicStory(11); }
        public void Storia12() { ShowDynamicStory(12); }

        private void ShowDynamicStory(int id)
        {
            Story story = StoryEngine.GetStory(id);
            if (story != null)
                ShowForm(new DynamicStoryForm(story));
            else
                Fine();
        }

        public void Storia13() { ShowDynamicStory(13); }
        public void Storia14() { ShowDynamicStory(14); }
        public void Storia15() { ShowDynamicStory(15); }
        public void Storia16() { ShowDynamicStory(16); }
        public void Storia17() { ShowDynamicStory(17); }
        public void Storia18() { ShowDynamicStory(18); }

        public void ApriStoria(int id)
        {
            switch (id)
            {
                case 1: Storia1(); break;
                case 2: Storia2(); break;
                case 3: Storia3(); break;
                case 4: Storia4(); break;
                case 5: Storia5(); break;
                case 6: Storia6(); break;
                case 7: Storia7(); break;
                case 8: Storia8(); break;
                case 9: Storia9(); break;
                case 10: Storia10(); break;
                case 11: Storia11(); break;
                case 12: Storia12(); break;
                case 13: Storia13(); break;
                case 14: Storia14(); break;
                case 15: Storia15(); break;
                case 16: Storia16(); break;
                case 17: Storia17(); break;
                case 18: Storia18(); break;
                default:
                    var userStory = StoryEngine.GetStory(id);
                    if (userStory != null && userStory.IsDynamic)
                        ShowForm(new DynamicStoryForm(userStory));
                    else
                        Fine();
                    break;
            }
        }

        public void Fine() { ShowForm(new FINE()); }
        public void Conclusione() { Fine(); }

    }
}
