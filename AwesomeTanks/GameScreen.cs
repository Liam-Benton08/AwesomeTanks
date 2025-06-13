using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        Brush redBrush = new SolidBrush(Color.Red);

        public Rectangle camera = new Rectangle();
        
        List<Region> regions = new List<Region>();
        List<Bullet> bullets = new List<Bullet>();
        List<Enemy> enemys = new List<Enemy>();
        List <Wall> walls = new List<Wall>();
        List <Spawner> spawners = new List<Spawner>();

        Player hero = new Player(150, 150);
        Enemy joker = new Enemy(400, 400, 10, 50);
        Spawner idk = new Spawner(700, 300, 1);

        Random randGen = new Random();
        int randNum;

        int arenaWidth = 920;  // much larger than screen
        int arenaHeight = 930;

        int screenWidth = 600;
        int screenHeight = 600;

        public static bool noRight, noLeft, noUp, noDown;
        public GameScreen()
        {
            InitializeComponent();
            enemys.Add(joker);
            spawners.Add(idk);

            camera = new Rectangle(0, 0, screenWidth, screenHeight);

            WallCreation();
        }

        public void WallCreation()
        {
            Wall w;

            w = new Wall(50, 50, 800, 30, "unbreakable");
            walls.Add(w);

            w = new Wall(50, 50, 30, 200, "unbreakable");
            walls.Add(w);

            w = new Wall(50, 250, 200, 30, "unbreakable");
            walls.Add(w);

            w = new Wall(250, 250, 30, 600, "unbreakable");
            walls.Add(w);

            w = new Wall(545, 50, 30, 600, "unbreakable");
            walls.Add(w);

            w = new Wall(250, 850, 600, 30, "unbreakable");
            walls.Add(w);

            w = new Wall(850, 50, 30, 830, "unbreakable");
            walls.Add(w);

            w = new Wall(545, 650, 30, 40, "unbreakable");
            walls.Add(w);

            w = new Wall(545, 780, 30, 70, "unbreakable");
            walls.Add(w);

            w = new Wall(250, 80, 30, 40, "unbreakable");
            walls.Add(w);

            w = new Wall(250, 210, 30, 40, "unbreakable");
            walls.Add(w);

            w = new Wall(250, 120, 30, 30, "soloBreak");
            walls.Add(w);

            w = new Wall(250, 150, 30, 30, "soloBreak");
            walls.Add(w);

            w = new Wall(250, 180, 30, 30, "soloBreak");
            walls.Add(w);

            w = new Wall(545, 690, 30, 30, "groupBreak");
            walls.Add(w);

            w = new Wall(545, 720, 30, 30, "groupBreak");
            walls.Add(w);

            w = new Wall(545, 750, 30, 30, "groupBreak");
            walls.Add(w);
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            int lastX = hero.x;
            int lastY = hero.y;

            foreach (Enemy enemy in enemys)
            {
                enemy.lastX = enemy.x;
                enemy.lastY = enemy.y;

                enemy.fireCooldown--;
                enemy.hitCooldown--;
            }

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
        
            Camera();

            SpawnEnemy();

            Collisions(lastX, lastY);

            playerHitCooldown--;
            playerFireCooldown--;

            if (spawners.Count == 0 && enemys.Count == 0)
            {
                Console.WriteLine("Next Level");
            }

            Refresh();
        }

        public void SpawnEnemy()
        {
            
            foreach (Spawner s in spawners)
            {
                double deltaX = Math.Abs(hero.x - s.x);
                double deltaY = Math.Abs(hero.y - s.y);

                double deltaDistance = Math.Abs(deltaX + deltaY);

                if (deltaDistance < 350)
                {
                    if (s.spawnTime <= 0)
                    {
                        Enemy e = s.spawnBots();
                        enemys.Add(e);
                    }
                }

                s.spawnTime--;
                s.hitCooldown--;
            }
           
        }

        public void Camera()
        {
            camera.X = hero.x - screenWidth / 2;
            camera.Y = hero.y - screenHeight / 2;

            // Clamp camera to arena bounds
            camera.X = Clamp(camera.X, 0, arenaWidth - screenWidth);
            camera.Y = Clamp(camera.Y, 0, arenaHeight - screenHeight);
        }

        public void Collisions(int lastX, int lastY)
        {
            foreach (Wall w in walls)
            {

                if (w.Collision(w, hero))
                {
                    hero.x = lastX;
                    hero.y = lastY;
                }

                foreach (Enemy enemy in enemys)
                {
                    if (w.AICollision(w, enemy))
                    {
                        enemy.x = enemy.lastX;
                        enemy.y = enemy.lastY;
                    }

                    if (hero.EnemyCollision(enemy))
                    {
                        hero.x = lastX;
                        hero.y = lastY;
                    }
                }
            }
        }

        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public void MoveEnemy()
        {
            Graphics g = this.CreateGraphics();

            regions.Clear();

            foreach (Enemy e in enemys)
            {
                e.wallInBetween = false;

                double deltaX = Math.Abs(hero.x - e.x);
                double deltaY = Math.Abs(hero.y - e.y);

                double deltaDistance = Math.Abs(deltaX + deltaY);

                double x1 = e.x + e.size / 2;
                double y1 = e.y + e.size / 2;
                double x2 = hero.x + hero.size / 2;
                double y2 = hero.y + hero.size / 2;

         
                Region enemyView = new Region(EnemyViewRegion(e));
                Region temp1 = enemyView;
                regions.Add(enemyView);
               

                foreach (Wall w in walls)
                {
                    Region wallRegion = new Region(WallSetRegion(w));
                    Region temp2 = wallRegion;

                   // temp1.Intersect(temp2);

                    if (!temp1.IsEmpty(g))
                    {
                        e.wallInBetween = true;
                        Form1.f.Text = e.wallInBetween.ToString();
                    }
                }

                if (e.wallInBetween == false)
                {
                    if (e.fireCooldown < 0)
                    {
                        Bullet newBullet = e.FireBullet(x1, y1, x2, y2, 8);
                        bullets.Add(newBullet);

                        e.fireCooldown = 20;
                    }
                    else if (deltaDistance > 50)
                    {
                        e.MoveEnemy(x1, y1, x2, y2);
                    }
                }
            }
        }

        private GraphicsPath WallSetRegion(Wall w)
        {
            GraphicsPath wall = new GraphicsPath(); // using System.Drawing.2D;

            // Create an open figure
            wall.AddLine(w.x, w.y, w.x + w.width, w.y); // a of polygon
            wall.AddLine(w.x + w.width, w.y, w.x + w.width, w.y + w.height); // b of polygon
            wall.AddLine(w.x + w.width, w.y + w.height, w.x, w.y + w.height); // b of polygon
            wall.CloseFigure();           // close polygon

            return wall;
        }
        private GraphicsPath EnemyViewRegion(Enemy e)
        {
            GraphicsPath grp = new GraphicsPath(); // using System.Drawing.2D;
            
            // Create a cone region between hero and a rectangle, (enemy), on screen
            grp.AddLine(hero.x + hero.size / 2 - camera.X, hero.y + hero.size / 2 - camera.Y, Convert.ToInt16(e.x + (e.size / 2) - 8) - camera.X, Convert.ToInt16(e.y + (e.size / 2) - 10) - camera.Y);
            grp.AddLine(Convert.ToInt16(e.x + (e.size / 2) - 8) - camera.X, Convert.ToInt16(e.y + (e.size / 2) - 10) - camera.Y, Convert.ToInt16(e.x + (e.size / 2) + 10) - camera.X, Convert.ToInt16(e.y + (e.size / 2) + 10) - camera.Y);
            grp.CloseFigure();

            return grp;
        }


        public void BulletDamage()
        {
            for (int e = 0; e < enemys.Count; e++)
            {
                for (int i = 0; i < bullets.Count; i++)
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

                if (hero.health <= 0)
                {
                    gameTimer.Stop();
                }
            }

            for (int i = 0; i < spawners.Count; i++)
            {
                for (int b = 0; b < bullets.Count; b++)
                {
                    if (spawners[i].BulletCollision(bullets[b]) && bullets[i].shooter == "Player" && spawners[i].hitCooldown <= 0)
                    {
                        spawners[i].hp -= bullets[b].damage;
                        bullets.RemoveAt(i) ;
                        spawners[i].hitCooldown = 1;

                        if (spawners[i].hp <= 0)
                        {
                            spawners.RemoveAt(i);
                            return;
                        }
                    }
                }
            }
        }

        public void DeleteBullet()
        {
            if (bullets.Count > 0)
            {
                for (int i = 0; bullets.Count > i; i++)
                {
                    foreach (Wall w in walls)
                    {
                        if (w.BulletCollision(w, bullets[i]))
                        {
                            if (bullets[i].shooter == "Player" && w.type != "unbreakable")
                            {
                                w.hp -= bullets[i].damage;

                                if(w.hp <= 0)
                                {
                                    if(w.type == "groupBreak")
                                    {
                                        walls.RemoveAll(wall => wall.type == "groupBreak");
                                    }
                                    walls.Remove(w);
                                }
                            }

                            bullets.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        private void GameScreen_MouseDown(object sender, MouseEventArgs e)
        {
            if (playerFireCooldown <= 0)
            {
                mouseX = e.X + camera.X;
                mouseY = e.Y + camera.Y;

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
            e.Graphics.FillRectangle(playerBrush, hero.x - camera.X, hero.y - camera.Y, hero.size, hero.size);

            //Health Bar
            e.Graphics.FillRectangle(playerBrush, hero.x + 1 - camera.X, hero.y + hero.size + 3 - camera.Y, Convert.ToInt16(hero.size * (hero.health / hero.fullhealth)), 3);
            e.Graphics.DrawRectangle(healthPen, hero.x - camera.X, hero.y + hero.size + 2 - camera.Y, hero.size, 5);

            //Drawing the bullets
            foreach (Bullet b in bullets)
            {
                e.Graphics.FillRectangle(playerBrush, Convert.ToInt16(b.x) - camera.X, Convert.ToInt16(b.y) - camera.Y, b.size, b.size);
            }

            //Drawing the enemys
            foreach(Enemy en in enemys)
            {
                e.Graphics.FillRectangle(playerBrush, Convert.ToInt16(en.x) - camera.X, Convert.ToInt16(en.y) - camera.Y, en.size, en.size);

                //Health Bar
                e.Graphics.FillRectangle(playerBrush, Convert.ToInt16(en.x) + 1 - camera.X, Convert.ToInt16(en.y) + en.size + 3 - camera.Y, Convert.ToInt16(en.size * (en.health / en.fullhealth)), 3);
                e.Graphics.DrawRectangle(healthPen, Convert.ToInt16(en.x) - camera.X, Convert.ToInt16(en.y) + en.size + 2 - camera.Y, en.size, 5);
            }

            foreach (Wall w in walls)
            {
                e.Graphics.FillRectangle(w.wallBrush, w.x - camera.X, w.y - camera.Y, w.width, w.height);
            }

            foreach (Spawner s in spawners)
            {
                e.Graphics.FillRectangle(playerBrush, s.x - camera.X, s.y - camera.Y, s.size, s.size);

                e.Graphics.FillRectangle(playerBrush, Convert.ToInt16(s.x) + 1 - camera.X, Convert.ToInt16(s.y) + s.size + 3 - camera.Y, Convert.ToInt16(s.size * (s.hp / s.fullHp)), 3);
                e.Graphics.DrawRectangle(healthPen, Convert.ToInt16(s.x) - camera.X, Convert.ToInt16(s.y) + s.size + 2 - camera.Y, s.size, 5);
            }

            for (int i = 0; i < regions.Count; i++) 
            {
                e.Graphics.FillRegion(redBrush, regions[i]);
            }
        }
    }
}
