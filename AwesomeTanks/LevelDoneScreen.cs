using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AwesomeTanks
{
    public partial class LevelDoneScreen : UserControl
    {
        public LevelDoneScreen()
        {
            InitializeComponent();

            if (GameScreen.levelFailed == true) // checks to see if user lost
            {
                levelOverLabel.Text = "Level Failed";
            }
            else if(GameScreen.levelFailed == false)  
            {
                levelOverLabel.Text = "Level Complete";
            }

            moneyEarnedLabel.Text = $"You Earned: {GameScreen.moneyEarned}"; //shows how much money was made

            GameScreen.currency += GameScreen.moneyEarned;
            GameScreen.moneyEarned = 0;

            if (LevelSelectScreen.level == 6)
            {
                levelOverLabel.Text = "You Beat The Gane!!!";
                moneyEarnedLabel.Text = "Congrats!!!";
            }

        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            Form1.ChangeScreen(this, new LevelSelectScreen()); //changes screens
            Form1.buttomPress.Play();
        }
    }
}
