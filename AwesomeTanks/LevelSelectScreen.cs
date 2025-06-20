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
    public partial class LevelSelectScreen : UserControl
    {
       // public static Button tutorial;
        //public static Button lvl;

        public static int level;
        public static int completedLevel;
        public static List<Button> buttons = new List<Button>();

        public LevelSelectScreen()
        {
            InitializeComponent();

            int doneLevel = Form1.completedLevel;

            if (doneLevel >= 0)
            {
                tutButton.BackColor = Color.Green;
            }

            for (int i = 1; i <= doneLevel; i++)
            {
                Button lvlButton = this.Controls.Find("lvl" + i + "Button", true).FirstOrDefault() as Button; //checks to find all completed levels
                if (lvlButton != null)
                {
                    lvlButton.BackColor = Color.Green; //if completed turn it green
                }
            }

            for (int i = 1; i + 1 <= doneLevel; i++)
            {
                Button lvlButton = this.Controls.Find("lvl" + i + "Button", true).FirstOrDefault() as Button; //checks to see all levels that i can do
                if (lvlButton != null)
                {
                    lvlButton.Enabled = true; //enables the button
                }
            }

            for (int i = 12; i - 2 >= doneLevel; i--)
            {
                Button lvlButton = this.Controls.Find("lvl" + i + "Button", true).FirstOrDefault() as Button; //checks to see all levels i cant do
                if (lvlButton != null)
                {
                    lvlButton.BackColor = Color.Red;
                    lvlButton.Enabled = false; //disables and turns color to red on buttons
                }
            }

            coinsLabel.Text = $"Coins: {GameScreen.currency}";
        }

        private void tutButton_Click(object sender, EventArgs e)
        {
            level = 0; //sets level
            Form1.ChangeScreen(this, new GameScreen()); //changes screen
            Form1.buttomPress.Play(); //plays sound
        }

        private void lvl1Button_Click(object sender, EventArgs e)
        {
            level = 1; //same thing
            Form1.ChangeScreen(this, new GameScreen());
            Form1.buttomPress.Play();
        }

        private void lvl2Button_Click(object sender, EventArgs e)
        {
            level = 2;
            Form1.ChangeScreen(this, new GameScreen());
            Form1.buttomPress.Play();
        }

        private void lvl3Button_Click(object sender, EventArgs e)
        {
            level = 3;
            Form1.ChangeScreen(this, new GameScreen());
            Form1.buttomPress.Play();
        }

        private void lvl4Button_Click(object sender, EventArgs e)
        {
            level = 4;
            Form1.ChangeScreen(this, new GameScreen());
            Form1.buttomPress.Play();
        }

        private void lvl5Button_Click(object sender, EventArgs e)
        {
            level = 5;
            Form1.ChangeScreen(this, new GameScreen());
            Form1.buttomPress.Play();
        }

        private void lvl6Button_Click(object sender, EventArgs e)
        {
            level = 6;
            Form1.ChangeScreen(this, new GameScreen());
            Form1.buttomPress.Play();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Form1.ChangeScreen(this, new MenuScreen()); //changes to menu screen
            Form1.buttomPress.Play();
        }
    }
}
