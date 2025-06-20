using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AwesomeTanks
{
    public partial class MenuScreen : UserControl
    {
        public MenuScreen()
        {
            InitializeComponent();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Form1.buttomPress.Play();
            Application.Exit(); //exits the program
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            Form1.buttomPress.Play();
            Form1.ChangeScreen(this, new LevelSelectScreen()); //changes to level screen
        }

        private void storeButton_Click(object sender, EventArgs e)
        {
            Form1.buttomPress.Play();
            Form1.ChangeScreen(this, new StoreScreen()); //changes to store screen
        }
    }
}
