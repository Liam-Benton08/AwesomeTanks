using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AwesomeTanks
{
    public partial class StoreScreen : UserControl
    {
        public int coins = GameScreen.currency;

        public static int armorLevel = Form1.armorLevel;
        public static int speedLevel = Form1.speedLevel;
        public static int pierceLevel = Form1.pierceLevel;

        public static bool rocketLauncher = Form1.rocketLauncher; //sets all of these to what they previously were since its on form1

        public StoreScreen()
        {
            InitializeComponent();

            coinsLabel.Text = $"Coins: {coins}";

            SettingShop(); //Everything resets, so doing this will put it back to however i left it
        }

        private void SettingShop()
        {
            //Armor
            if (armorLevel == 2)
            {
                armor2.BackColor = Color.Green;
                armorAmountLabel.Text = "Price : 2500";
            }
            else if (armorLevel == 3)
            {
                armor2.BackColor = Color.Green;
                armor3.BackColor = Color.Green;
                armorAmountLabel.Text = "Price : 6000";
            }
            else if (armorLevel == 4)
            {
                armor2.BackColor = Color.Green;
                armor3.BackColor = Color.Green;
                armor4.BackColor = Color.Green;
                armorAmountLabel.Text = "Price : 15000";
            }
            else if (armorLevel == 5)
            {
                armor2.BackColor = Color.Green;
                armor3.BackColor = Color.Green;
                armor4.BackColor = Color.Green;
                armor5.BackColor = Color.Green;
                armorAmountLabel.Text = "";
                armorBuyButton.Text = "MAX";
            }

            //Speed
            if (speedLevel == 2)
            {
                speed2.BackColor = Color.Green;
                speedAmountLabel.Text = "Price : 1250";
            }
            else if (speedLevel == 3)
            {
                speed2.BackColor = Color.Green;
                speed3.BackColor = Color.Green;
                speedAmountLabel.Text = "Price : 3000";
            }
            else if (speedLevel == 4)
            {
                speed2.BackColor = Color.Green;
                speed3.BackColor = Color.Green;
                speed4.BackColor = Color.Green;
                speedAmountLabel.Text = "Price : 7000";
            }
            else if (speedLevel == 5)
            {
                speed2.BackColor = Color.Green;
                speed3.BackColor = Color.Green;
                speed4.BackColor = Color.Green;
                speed5.BackColor = Color.Green;
                speedAmountLabel.Text = "";
                speedBuyButton.Text = "MAX";
            }
            //Pierce
            if (pierceLevel == 2)
            {
                pierce2.BackColor = Color.Green;
                pierceAmountLabel.Text = "Price : 10000";
            }
            else if (pierceLevel == 3)
            {
                pierce2.BackColor = Color.Green;
                pierce3.BackColor = Color.Green;
                pierceAmountLabel.Text = "";
                pierceBuyButton.Text = "MAX";
            }

            //Rocket
            if (rocketLauncher == true)
            {
                rLBuyButton.Text = "PURCHASED";
                rLBuyButton.Size = new Size(250, 50);
                rLBuyButton.BackColor = Color.Green;

                scammedLabel.ForeColor = Color.Red;
                scammedLabel.Text = "SCAMMED";
            }
        }

        private void rLBuyButton_Click(object sender, EventArgs e)
        {
            if (coins >= 100000) //if enough coins
            {
                coins -= 100000; //subtract coins
                coinsLabel.Text = $"Coins: {coins}";
                rLBuyButton.Text = "PURCHASED";
                rLBuyButton.Size = new Size(250, 50);
                rLBuyButton.BackColor = Color.Green;

                scammedLabel.ForeColor = Color.Red;
                scammedLabel.Text = "SCAMMED";

                Form1.buttomPress.Play();

                Form1.rocketLauncher = true; //set to true
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Form1.ChangeScreen(this, new MenuScreen());//change screen
            GameScreen.currency = coins; 
        }

        private void armorBuyButton_Click(object sender, EventArgs e)
        { 
            if (armorLevel == 1) //checks to see what last level was, then bases the change off of that
            {
                if(coins >= 1000) //if enough coins
                {
                    coins -= 1000; //subtract coins
                    coinsLabel.Text = $"Coins:  {coins}"; //display amount left
                    Form1.armorLevel++;
                    armor2.BackColor = Color.Green;
                    armorAmountLabel.Text = "Price : 2500"; //change label
                    Form1.buttomPress.Play();
                }
            }
            else if(armorLevel == 2) 
            {
                if(coins >= 2500)
                {
                    coins -= 2500;
                    coinsLabel.Text = $"Coins: {coins}";
                    Form1.armorLevel++;
                    armor3.BackColor = Color.Green;
                    armorAmountLabel.Text = "Price : 6000";
                    Form1.buttomPress.Play();
                }
            }
            else if(armorLevel == 3)
            {
                if(coins >= 6000)
                {
                    coins -= 6000;
                    coinsLabel.Text = $"Coins: {coins}";
                    Form1.armorLevel++;
                    armor4.BackColor = Color.Green;
                    armorAmountLabel.Text = "Price : 15000";
                    Form1.buttomPress.Play();
                }
            }
            else if(armorLevel == 4)
            {
                if (coins >= 15000)
                {
                    coins -= 15000;
                    coinsLabel.Text = $"Coins: {coins}";
                    Form1.armorLevel++;
                    armor5.BackColor = Color.Green;
                    armorAmountLabel.Text = "";
                    armorBuyButton.Text = "MAX";
                    Form1.buttomPress.Play();
                }
            }

            armorLevel = Form1.armorLevel;
        }

        private void speedBuyButton_Click(object sender, EventArgs e)
        {
            if (speedLevel == 1)
            {
                if (coins >= 500)
                {
                    coins -= 500;
                    coinsLabel.Text = $"Coins: {coins}";
                    Form1.speedLevel++;
                    speed2.BackColor = Color.Green;
                    speedAmountLabel.Text = "Price : 1250";
                    Form1.buttomPress.Play();
                }
            }
            else if (speedLevel == 2)
            {
                if (coins >= 1250)
                {
                    coins -= 1250;
                    coinsLabel.Text = $"Coins: {coins}";
                    Form1.speedLevel++;
                    speed3.BackColor = Color.Green;
                    speedAmountLabel.Text = "Price : 3000";
                    Form1.buttomPress.Play();
                }
            }
            else if (speedLevel == 3)
            {
                if (coins >= 3000)
                {
                    coins -= 3000;
                    coinsLabel.Text = $"Coins: {coins}";
                    Form1.speedLevel++;
                    speed4.BackColor = Color.Green;
                    speedAmountLabel.Text = "Price : 7000";
                    Form1.buttomPress.Play();
                }
            }
            else if (speedLevel == 4)
            {
                if (coins >= 7000)
                {
                    coins -= 7000;
                    coinsLabel.Text = $"Coins: {coins}";
                    Form1.speedLevel++;
                    speed5.BackColor = Color.Green;
                    speedAmountLabel.Text = "";
                    speedBuyButton.Text = "MAX";
                    Form1.buttomPress.Play();
                }
            }

            speedLevel = Form1.speedLevel;
        }

        private void pierceBuyButton_Click(object sender, EventArgs e)
        {
            if (pierceLevel == 1)
            {
                if (coins > 3000)
                {
                    coins -= 3000;
                    coinsLabel.Text = $"Coins: {coins}";
                    Form1.pierceLevel++;
                    pierce2.BackColor = Color.Green;
                    pierceAmountLabel.Text = "Price : 10000";
                    Form1.buttomPress.Play();
                }
            }
            else if (pierceLevel == 2)
            {
                if (coins > 10000)
                {
                    coins -= 10000;
                    coinsLabel.Text = $"Coins: {coins}";
                    Form1.pierceLevel++;
                    pierce3.BackColor = Color.Green;
                    pierceAmountLabel.Text = "";
                    pierceBuyButton.Text = "MAX";
                    Form1.buttomPress.Play();
                }
            }

            pierceLevel = Form1.pierceLevel;
        }
    }
}
