using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AwesomeTanks
{
    public partial class Form1 : Form
    {
        public static int completedLevel = -1;

        public static int armorLevel = 1;
        public static int speedLevel = 1;
        public static int pierceLevel = 1; // keeps these values safe stored here, when i reset the screen it changes to default.

        public static bool rocketLauncher = false;

        public static Form f;

        public static SoundPlayer buttomPress = new SoundPlayer(Properties.Resources.buttonPressSound);

        public Form1()
        {
            InitializeComponent();
            ChangeScreen(this, new MenuScreen());
            f = this;
        }

        public static void AddLevel()
        {
            completedLevel++;
        }

        public static void ChangeScreen(object sender, UserControl next)
        {
            Form f; // will either be the sender or parent of sender

            if (sender is Form)
            {
                f = (Form)sender;                          //f is sender
            }
            else
            {
                UserControl current = (UserControl)sender;  //create UserControl from sender
                f = current.FindForm();                     //find Form UserControl is on
                f.Controls.Remove(current);                 //remove current UserControl
            }

            // add the new UserControl to the middle of the screen and focus on it
            next.Location = new Point((f.ClientSize.Width - next.Width) / 2, (f.ClientSize.Height - next.Height) / 2);
            f.Controls.Add(next);
            next.Focus();
        }
    }
}
