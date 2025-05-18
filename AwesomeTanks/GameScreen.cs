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
    public partial class GameScreen : UserControl
    {
        public double mouseX, mouseY;
        public double x, y;
        bool upArrowDown, downArrowDown, leftArrowDown, rightArrowDown;

        int playerFireCooldown;
        int playerHitCooldown;

        Pen healthPen = new Pen(Color.Red);

        Brush playerBrush = new SolidBrush(Color.White);
        

        List<Bullet> bullets = new List<Bullet>();
        List<Enemy> enemys = new List<Enemy>();

        Player hero = new Player();
        Enemy badGuy = new Enemy(600, 200);
        Enemy redManz = new Enemy(200, 600);
        Enemy joker = new Enemy(200, 200);
        Enemy himmothy = new Enemy(600, 600);

        Random randGen = new Random();
        int randNum;

        public GameScreen()
        {
            InitializeComponent();
            enemys.Add(badGuy);
            enemys.Add(joker);
            enemys.Add(redManz);
            enemys.Add(himmothy);

        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            hero.MovePlayer(upArrowDown, downArrowDown, leftArrowDown, rightArrowDown);

            if (bullets.Count > 0)
            {
                foreach (Bullet b in bullets)
                {
                    b.MoveBullet();
                }
            }

            DeleteBullet();

            BulletDamage();

            MoveEnemy();

            Console.WriteLine($"{bullets.Count} bullets");

            foreach (Enemy enemy in enemys)
            {
                enemy.fireCooldown--;
                enemy.hitCooldown--;
            }

            playerHitCooldown--;
            playerFireCooldown--;
            Refresh();
        }

        public void MoveEnemy()
        {
            for (int i = 0; i < enemys.Count; i++)
            {
                double x1 = enemys[i].x + 7.5;
                double y1 = enemys[i].y + 7.5;
                double x2 = hero.x + 7.5;
                double y2 = hero.y + 7.5;

                randNum = randGen.Next(1, 5);
                
                if (enemys[i].fireCooldown < 0)
                {
                    Bullet newBullet = enemys[i].FireBullet(x1, y1, x2, y2);
                    bullets.Add(newBullet);

                    enemys[i].fireCooldown = 30;
                }
                else
                {
                    enemys[i].MoveEnemy(x1, y1, x2, y2);
                }
            }
        }

        public void BulletDamage()
        {
            for(int e = 0; e < enemys.Count; e++)
            {
                for(int i = 0; i < bullets.Count; i++)
                {
                    if (enemys[e].BulletCollision(enemys[e], bullets[i]) && bullets[i].shooter == "Player" && enemys[e].hitCooldown <= 0)
                    {
                        enemys[e].health -= bullets[i].damage;
                        bullets.RemoveAt(i);
                        enemys[e].hitCooldown = 1;
                    }

                    if (enemys[e].health <= 0)
                    {
                        enemys.RemoveAt(e);
                        return;
                    }
                }
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                if (hero.BulletCollision(bullets[i]) && bullets[i].shooter == "Enemy" && playerHitCooldown <= 0)
                {
                    hero.health -= bullets[i].damage;
                    bullets.RemoveAt(i);
                    playerHitCooldown = 1;
                }

                if(hero.health <= 0)
                {
                    gameTimer.Stop();
                }
            }
        }

        public void DeleteBullet()
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].x + bullets[i].size < 0)
                {
                    bullets.RemoveAt(i);
                }
                else if (bullets[i].y + bullets[i].size < 0)
                {
                    bullets.RemoveAt(i);
                }
                else if (bullets[i].x > this.Width)
                {
                    bullets.RemoveAt(i);
                }
                else if (bullets[i].y > this.Height)
                {
                    bullets.RemoveAt(i);
                }
            }
        }

        private void GameScreen_MouseDown(object sender, MouseEventArgs e)
        {
            if (playerFireCooldown <= 0)
            {
                mouseX = e.X;
                mouseY = e.Y;

                x = hero.x + hero.size / 2;
                y = hero.y + hero.size / 2;

                double angle = CalculateAngle(x, y, mouseX, mouseY);
                Console.WriteLine($"Angle: {angle} degrees");

                double speed = 8;

                var (xSpeed, ySpeed) = GetVelocity(angle, speed);

                Console.WriteLine($"Speed: {xSpeed}, {ySpeed}");

                Bullet newBullet = new Bullet(xSpeed, ySpeed, x, y, angle, "Player");
                bullets.Add(newBullet);
                playerFireCooldown = 5;
            }
        }

        public static (double xSpeed, double ySpeed) GetVelocity(double angleDeg, double speed)
        {
            double angleRadians = angleDeg * (Math.PI / 180f);
            double xSpeed = speed * Math.Sin(angleRadians);
            double ySpeed = speed * Math.Cos(angleRadians);
            return (xSpeed, ySpeed);
        }

        public static double CalculateAngle(double x1, double y1, double x2, double y2)
        {
            double deltaX = x2 - x1;
            double deltaY = y2 - y1;

            double angleRadians = Math.Atan2(deltaX, deltaY);
            double angleDegrees = angleRadians * (180f / Math.PI);


            if (angleDegrees < 0)
            {
                angleDegrees += 360f;
            }

            return angleDegrees;
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
                case Keys.W:
                    upArrowDown = false;
                    break;
                case Keys.S:
                    downArrowDown = false;
                    break;
                case Keys.A:
                    leftArrowDown = false;
                    break;
                case Keys.D:
                    rightArrowDown = false;
                    break;


            }
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                case Keys.W:
                    upArrowDown = true;
                    break;
                case Keys.S:
                    downArrowDown = true;
                    break;
                case Keys.A:
                    leftArrowDown = true;
                    break;
                case Keys.D:
                    rightArrowDown = true;
                    break;
            }
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //Drawing the Player
            e.Graphics.FillRectangle(playerBrush, hero.x, hero.y, hero.size, hero.size);

            //Health Bar
            e.Graphics.FillRectangle(playerBrush, hero.x + 1, hero.y + hero.size + 3, Convert.ToInt16(15 * (hero.health / hero.fullhealth)), 3);
            e.Graphics.DrawRectangle(healthPen, hero.x, hero.y + hero.size + 2, 15, 5);

            //Drawing the bullets
            foreach (Bullet b in bullets)
            {
                e.Graphics.FillRectangle(playerBrush, Convert.ToInt16(b.x), Convert.ToInt16(b.y), b.size, b.size);
            }

            //Drawing the enemys
            foreach(Enemy en in enemys)
            {
                e.Graphics.FillRectangle(playerBrush, Convert.ToInt16(en.x), Convert.ToInt16(en.y), en.size, en.size);

                //Health Bar
                e.Graphics.FillRectangle(playerBrush, Convert.ToInt16(en.x) + 1, Convert.ToInt16(en.y) + en.size + 3, Convert.ToInt16(15 * (en.health / en.fullhealth)), 3);
                e.Graphics.DrawRectangle(healthPen, Convert.ToInt16(en.x), Convert.ToInt16(en.y) + en.size + 2, 15, 5);
            }
        }
    }
}
