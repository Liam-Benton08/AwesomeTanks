using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Media;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


namespace AwesomeTanks
{
    public partial class GameScreen : UserControl
    {

        //Declaring global values
        public double mouseX, mouseY;
        public double x, y;
        bool upArrowDown, downArrowDown, leftArrowDown, rightArrowDown;

        int playerFireCooldown;
        int playerHitCooldown;

        //Setting my money values
        public static int currency = 1000;
        public static int moneyEarned;

        //Creating this for my health bars
        Pen healthPen = new Pen(Color.Red);
        Brush blueBrush = new SolidBrush(Color.Blue);
        Brush playerBrush = new SolidBrush(Color.White);
        
        //Creating camera for sight purposes to follow me around
        public Rectangle camera = new Rectangle();
       
        //Making all my lists in this chunk
        List<Bullet> bullets = new List<Bullet>();
        List<Enemy> enemys = new List<Enemy>();
        List <Wall> walls = new List<Wall>();
        List <Spawner> spawners = new List<Spawner>();
        List <Coin> coins = new List<Coin>();
        List <int> levels = new List<int>();

        //Declaring my hero
        Player hero;

        //Making my arena values here, changing depending on level
        int arenaWidth;
        int arenaHeight;

        int screenWidth = 600;
        int screenHeight = 600;

        //This is for the tutorial, just setting the values
        bool tutorial = false;
        int stage = 1;
        bool enemyKilled = false;

        //For the end of the Level, just making the timer here
        int endOfLevelTimer = 0;
        public static int finishedLevel = -1;

        //Setting my public static values
        public static bool noRight, noLeft, noUp, noDown;

        //this is for the fun level that creates chaos
        bool level3CheckPoint = false;
        public static bool levelFailed = false;

        //Had to make a bitmap because i cant rotate my Images
        public Bitmap rotatedImage = Properties.Resources.Cannon;

        //Creating Sound players
        SoundPlayer coin = new SoundPlayer(Properties.Resources.coinSound);
        SoundPlayer levelWin = new SoundPlayer(Properties.Resources.levelWinSound);

        public GameScreen()
        {
            InitializeComponent();

            tutorial = false;

            camera = new Rectangle(0, 0, screenWidth, screenHeight);


            SetLevel();

            if (LevelSelectScreen.level < 6)
            {
                LoadLevelXML();
            }
            else
            {
                lvl6Creation();
            }
        }


        public void lvl6Creation()
        {
            Wall w;

            w = new Wall(250, 250, 500, 30, "unbreakable");
            walls.Add(w);

            w = new Wall(250, 250, 30, 500, "unbreakable");
            walls.Add(w);

            w = new Wall(750, 250, 30, 530, "unbreakable");
            walls.Add(w);

            w = new Wall(250, 750, 500, 30, "unbreakable");
            walls.Add(w);

            Enemy e = new Enemy(300, 500, 500, 10, 9999999);
            enemys.Add(e);
        }

        public void SetLevel()
        {
            int level = LevelSelectScreen.level;

            if (level == 0)
            {
                Enemy tutBot = new Enemy(400, 400, 20, 50, 125); //Creates a tutorial bot
                enemys.Add(tutBot); //Adds it to enemies list
                tutorial = true; //Sets tutorial bool to true

                hero = new Player(150, 150); //Creates player, gives it proper spawn points

                arenaWidth = 920;
                arenaHeight = 930; //Sets the arena width and height for camera to lock

                tutorialLabel.Location = new Point(50, 450); //Moves label into sight
            }
            else if (level == 1)
            {
                hero = new Player(150, 150);

                arenaWidth = 920;
                arenaHeight = 960;

                tutorialLabel.Location = new Point(1111150, 4111150); //Moves label off screen so it doesnt get in the way
            }
            else if (level == 2)
            {
                hero = new Player(150, 150);

                arenaHeight = 920;
                arenaWidth = 800;

                tutorialLabel.Location = new Point(1111150, 4111150);
            }
            else if (level == 3)
            {
                hero = new Player(150, 250);

                arenaHeight = 600;
                arenaWidth = 1150; //Sets the arena to really wide

                tutorialLabel.Location = new Point(50, 450); //Moves label back onto the screen
                tutorialLabel.Text = "GOODLUCK!"; //They will need it
            }
            else if (level == 4)
            {
                hero = new Player(950, 150);

                arenaWidth = 1200;
                arenaHeight = 930;

                tutorialLabel.Location = new Point(1111150, 4111150);
            }
            else if (level == 5)
            {
                hero = new Player(250, 350);

                arenaWidth = 1200;
                arenaHeight = 1000;

                tutorialLabel.Location = new Point(51111110, 411111150);
            }
            else if (level == 6)
            {
                hero = new Player(650, 550);

                arenaWidth = 1000;
                arenaHeight = 1000;

                tutorialLabel.Location = new Point(50, 450);
                tutorialLabel.Text = "ITS THE BOSS!";
            }

        }

        public void LoadLevelXML()
        {
            XmlReader wallReader = XmlReader.Create($"Resources/level{LevelSelectScreen.level}Walls.xml"); //Creates an xml reader to read through my wall xml files based on current level

            while (wallReader.Read())
            {
                while (wallReader.Read()) //repeats code untill all walls have been added
                {
                    if (wallReader.NodeType == XmlNodeType.Text) //Just meaning if its actual text
                    {
                        int x = Convert.ToInt16(wallReader.ReadString()); //taking the x value 

                        wallReader.ReadToNextSibling("y");
                        int y = Convert.ToInt16(wallReader.ReadString()); //taking the y value

                        wallReader.ReadToNextSibling("width");
                        int width = Convert.ToInt16(wallReader.ReadString()); //taking the width

                        wallReader.ReadToNextSibling("height");
                        int height = Convert.ToInt16(wallReader.ReadString()); //taking the height

                        wallReader.ReadToNextSibling("type"); //taking the type
                        string type = wallReader.ReadString();

                        Wall w = new Wall(x, y, width, height, type); //putting all of the taken information and creating a wall with it
                        walls.Add(w); //adds wall to list
                    }
                }
                wallReader.Close(); //end of xml
            }

            XmlReader spawnerReader = XmlReader.Create($"Resources/level{LevelSelectScreen.level}Spawners.xml"); //Same thing but for Spawners this time

            while (spawnerReader.Read())
            {
                while (spawnerReader.Read())
                {
                    if (spawnerReader.NodeType == XmlNodeType.Text)
                    {
                        int x = Convert.ToInt16(spawnerReader.ReadString());

                        spawnerReader.ReadToNextSibling("y");
                        int y = Convert.ToInt16(spawnerReader.ReadString());

                        spawnerReader.ReadToNextSibling("type");
                        int type = Convert.ToInt16(spawnerReader.ReadString());

                        Spawner s = new Spawner(x, y, type);
                        spawners.Add(s);
                    }
                }
                spawnerReader.Close();
            }
        }


        private void gameTimer_Tick(object sender, EventArgs e)
        {
            int lastX = hero.x;
            int lastY = hero.y; //Sets these values so if something collides it will go back to these points

            if (tutorial == true)
            {
                PlayTutorial(lastX, lastY); //Just takes the user through steps to help understand the game
            }

            foreach (Enemy enemy in enemys)
            {
                enemy.lastX = enemy.x;
                enemy.lastY = enemy.y; //Same thing but for enemy, makes sure no errors

                enemy.fireCooldown--;
                enemy.hitCooldown--; //Needed for shooting to work and glitched damage
            }

            hero.MovePlayer(upArrowDown, downArrowDown, leftArrowDown, rightArrowDown); //pretty straight forward just moving the player

            if (bullets.Count > 0) //if there are any bullets on the screen
            {
                foreach (Bullet b in bullets)
                {
                    b.MoveBullet(); //Move the bullets
                }
                foreach (Bullet b in bullets)
                {
                    if (LevelSelectScreen.level == 5)
                    {
                        if (b.x + b.size < 0 || b.x > arenaWidth || b.y + b.size < 0 || b.y > arenaHeight) //if bullet offscreen
                        {
                            bullets.Remove(b); //remove
                            break;
                        }
                    }
                }
            }

            DeleteBullet(); //Checks to see if the bullet hits a wall

            BulletDamage(); //Checks to see if the bullet hits an object

            MoveEnemy(); //Moves enemy

            Camera(); //Updates camera and locks the screen if on the edge

            SpawnEnemy(); //Spawns enemy if able sa

            Collisions(lastX, lastY); //Checks for all collisions

            playerHitCooldown--;
            playerFireCooldown--; //Makes sure there is no machine guns, and bullets dont double hit

            coinsLabel.Text = $"Coins Gained: {moneyEarned}"; //Displays currency to screen

            LevelOverCheck(); //Checks to see if the level is over

            Refresh();

            
        }

        private void LevelOverCheck()
        {
            if (spawners.Count == 0 && enemys.Count == 0) //if there are no spawners or enemies left
            {
                endOfLevelTimer++; //Starts a timer

                if (endOfLevelTimer == 100) //checks to see if timer has reached point
                {
                    if (Form1.completedLevel < LevelSelectScreen.level)
                    {
                        Form1.AddLevel(); //adds a completed level so that the levelselectscreen functions can work
                    }

                    moneyEarned += 1000 * LevelSelectScreen.level; //adds money bonus depending on level
                    levelFailed = false;
                    levelWin.Play(); //plays sound
                    gameTimer.Stop(); //stops gametimer
                    Form1.ChangeScreen(this, new LevelDoneScreen());//Changes screens
                }
            }
        }

        private void PlayTutorial(int lastX, int lastY)
        {
            if (stage == 1) //checks to see what part of tutorial user is on and gives specific feedback
            {
                if (lastX != 150 || lastY != 150) //if user has moved
                {
                    stage++; //next stage
                }
                else //otherwise
                {
                    tutorialLabel.Text = "Use w a s d or arrow keys to move";
                }
            }
            else if (stage == 2)
            {
                if (bullets.Count > 0) //if the user has shot yet
                {
                    stage++; //next stage
                }
                else //otherwise
                {
                    tutorialLabel.Text = "Great! Use your mouse pad to aim, press down to fire";
                }
            }
            else if (stage == 3)
            {
                if (walls.Count < 17) //if there user has broken any walls
                {
                    stage++; //next level
                }
                else
                {
                    tutorialLabel.Text = "Your getting the hang of it! Shoot the bricks to destroy them and enter through the hole";
                }
            }
            else if (stage == 4)
            {
                if (enemyKilled == true) //if enemy is defeated
                {
                    stage++; //next level
                }
                else
                {
                    tutorialLabel.Text = "Lets see what you got! Shoot the enemy, make sure to dodge its bullets!";
                }
            }
            else if (stage == 5) 
            {
                int groupCounter = 0;

                foreach (Wall w in walls)
                {
                    if (w.type == "groupBreak")
                    {
                        groupCounter++; //takes the total of grouped walls
                    }
                }

                if (groupCounter < 3) //checks if user destroyed any grouped walls 
                {
                    stage++;
                }
                else
                {
                    tutorialLabel.Text = "Great, grab your coins and shoot at the next set of walls, if one of them breaks they all will!";
                }
            }
            else if (stage == 6)
            {
                if (spawners.Count == 0 && enemys.Count == 0) //if there are no enemies or spawners left
                {
                    stage++;
                }
                else
                {
                    tutorialLabel.Text = "This last box is a spawner! Take it out as quick as possible before it spawns more enemies";
                }
            }
            else
            {
                tutorialLabel.Text = "Good work! You beat this level, grab your coins before time runs out!";
            }
        }

        public void SpawnEnemy()
        {
            foreach (Spawner s in spawners)
            {
                s.canSpawn = true; //sets this to true, changed if not able too

                double deltaX = Math.Abs(hero.x - s.x);
                double deltaY = Math.Abs(hero.y - s.y); //simple math to find the delta x and ys

                double deltaDistance = Math.Abs(deltaX + deltaY); //using delta x and y to find distance

                double x1 = s.x + s.size / 2;
                double y1 = s.y + s.size / 2;
                double x2 = hero.x + hero.size / 2;
                double y2 = hero.y + hero.size / 2; //takes 4 neccesary ints

                PointF start = new PointF(Convert.ToInt16(x1), Convert.ToInt16(y1));
                PointF end = new PointF(Convert.ToInt16(x2), Convert.ToInt16(y2)); //creates points out of those ints before

                foreach (Wall w in walls)
                {
                    RectangleF wallRect = new RectangleF(w.x, w.y, w.width, w.height);

                    if (LineIntersectsRect(start, end, wallRect)) //Checks to see if there is a wall inbetween with geometry  
                    {
                        s.canSpawn = false; //sets spawns to false if there is a wall inbetween
                        break;
                    }
                }

                if (s.canSpawn == true)
                {
                    if (s.spawnTime <= 0)
                    {
                        Enemy e = s.spawnBots(); //spwans enemy depending on spawn timer
                        enemys.Add(e); //add enemy to the list
                    }
                }

                s.spawnTime--;
                s.hitCooldown--; //for the timers
            }
           
        }

        public void Camera()
        {
            camera.X = hero.x - screenWidth / 2;
            camera.Y = hero.y - screenHeight / 2; //gets values for camera

            // Clamp camera to arena bounds
            camera.X = Clamp(camera.X, 0, arenaWidth - screenWidth);
            camera.Y = Clamp(camera.Y, 0, arenaHeight - screenHeight);
        }

        public void Collisions(int lastX, int lastY)
        {
            foreach (Wall w in walls)
            {
                if (w.Collision(w, hero)) //if hero collides with wall
                {
                    hero.x = lastX; 
                    hero.y = lastY; //reset to last values
                }

                foreach (Enemy enemy in enemys)
                {
                    if (w.AICollision(w, enemy)) //if enemy collides with wall
                    {
                        enemy.x = enemy.lastX;
                        enemy.y = enemy.lastY;
                    }

                    if (hero.EnemyCollision(enemy)) // if hero collides with enemy
                    {
                        hero.x = lastX;
                        hero.y = lastY;
                    }
                }
            }

            foreach (Coin c in coins)
            {
                if (hero.CoinCollision(c)) //if hero collides with coins
                {
                    coin.Play();

                    moneyEarned += c.value;
                    coins.Remove(c);
                    break;
                }
            }

            foreach (Spawner s in spawners)
            {
                if (hero.SpawnerCollision(s)) //if hero collides with spawner
                {
                    hero.x = lastX;
                    hero.y = lastY;

                    break;
                }
            }
        }

        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value; //mathematical sequence to find if user is near the edges
        }

        public void MoveEnemy()
        {
            foreach (Enemy e in enemys)
            {
                e.wallInBetween = false; //sets this to false, later can be turned to true

                double deltaX = Math.Abs(hero.x - e.x);
                double deltaY = Math.Abs(hero.y - e.y); //once again takes the delta x and y values

                double deltaDistance = Math.Abs(deltaX + deltaY); //then finds the distance

                double x1 = e.x + e.size / 2;
                double y1 = e.y + e.size / 2;
                double x2 = hero.x + hero.size / 2;
                double y2 = hero.y + hero.size / 2; //Takes four values to find the angle

                PointF start = new PointF(Convert.ToInt16(x1), Convert.ToInt16(y1));
                PointF end = new PointF(Convert.ToInt16(x2), Convert.ToInt16(y2)); //makes two points

                double angle = CalculateAngle(x1, y1, x2, y2); //code to find what the angle is

                e.rotatedImage = RotateImage(e.enemyCannonImage, angle); //rotates the cannon depending on where the cursor is

                foreach (Wall w in walls)
                {
                    RectangleF wallRect = new RectangleF(w.x, w.y, w.width, w.height);

                    if (LineIntersectsRect(start, end, wallRect)) //geometric way of finding if there is a wall inbetween
                    {
                        e.wallInBetween = true; //sets this to true
                        break;
                    }
                }

                if (e.wallInBetween == false) // if there isnt
                {
                    if (e.fireCooldown < 0) //if able to shoot
                    {
                        Bullet newBullet = e.FireBullet(x1, y1, x2, y2, 10); //fires bullet
                        bullets.Add(newBullet); //adds bullet to the list

                        e.fireCooldown = 20; //resets the fire cooldown
                    }
                    else if (deltaDistance > 50) //else moves towards hero
                    {
                        e.MoveEnemy(x1, y1, x2, y2);
                    }
                }
            }
        }

        public bool LineIntersectsRect(PointF p1, PointF p2, RectangleF r)
        {
            return LineIntersectsLine(p1, p2, new PointF(r.Left, r.Top), new PointF(r.Right, r.Top)) ||
                   LineIntersectsLine(p1, p2, new PointF(r.Right, r.Top), new PointF(r.Right, r.Bottom)) ||
                   LineIntersectsLine(p1, p2, new PointF(r.Right, r.Bottom), new PointF(r.Left, r.Bottom)) ||
                   LineIntersectsLine(p1, p2, new PointF(r.Left, r.Bottom), new PointF(r.Left, r.Top)); //finding line intersections with another method
        }

        public bool LineIntersectsLine(PointF a1, PointF a2, PointF b1, PointF b2)
        {
            float d = (a2.X - a1.X) * (b2.Y - b1.Y) - (a2.Y - a1.Y) * (b2.X - b1.X);
            if (d == 0) return false;

            float u = ((b1.X - a1.X) * (b2.Y - b1.Y) - (b1.Y - a1.Y) * (b2.X - b1.X)) / d;
            float v = ((b1.X - a1.X) * (a2.Y - a1.Y) - (b1.Y - a1.Y) * (a2.X - a1.X)) / d;

            return (u >= 0 && u <= 1) && (v >= 0 && v <= 1); //Takes a lot of points and uses math to find intersections
        }

        public void BulletDamage()
        {
            for (int e = 0; e < enemys.Count; e++)
            {
                for (int i = 0; i < bullets.Count; i++)
                {
                    if (enemys[e].BulletCollision(enemys[e], bullets[i]) && bullets[i].shooter == "Player" && enemys[e].hitCooldown <= 0) //if bullet collides with enemy and player shot it 
                    {
                        enemys[e].health -= bullets[i].damage * Form1.pierceLevel; //enemy takes damage from bullet plus the player pierce level
                        bullets.RemoveAt(i); //removes the bullet from the list
                        enemys[e].hitCooldown = 1; //resets the hit cooldown
                    }

                    if (enemys[e].health <= 0) // if the enemies health is too low
                    {
                        Coin newCoin = new Coin(Convert.ToInt16(enemys[e].x), Convert.ToInt16(enemys[e].y), enemys[e].coinValue); //create new coin
                        coins.Add(newCoin);//add coin to the list
                        enemys.RemoveAt(e); //remove the enemy
                        enemyKilled = true; //sets this to true for the tutorial
                        return;
                    }
                }
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                if (hero.BulletCollision(bullets[i]) && bullets[i].shooter == "Enemy" && playerHitCooldown <= 0) //if bullet collides with hero and enemy shot it
                {
                    double damageMultiplier = 100.0 / (100 + (StoreScreen.armorLevel * 30)); //finds out what the multiplier is
                    int effectiveDamage = (int)(bullets[i].damage * damageMultiplier); //effective damage is lowered based on armour
                    hero.health -= effectiveDamage; //takes health away but not as much
                    
                    bullets.RemoveAt(i); //remove the bullet
                    playerHitCooldown = 1; //reset the hit cooldown
                }

                if (hero.health <= 0) //if the heros health is too low
                {
                    gameTimer.Stop(); //stops the gametimer

                    levelFailed = true; //for the next screen to pop up a message
                    Form1.ChangeScreen(this, new LevelDoneScreen()); //changes screens to the levelDoneScreen
                    break;
                }
            }

            for (int i = 0; i < spawners.Count; i++)
            {
                for (int b = 0; b < bullets.Count; b++)
                {
                    if (spawners[i].BulletCollision(bullets[b]) && bullets[b].shooter == "Player" && spawners[i].hitCooldown <= 0) //if bullet collieds with spawner and hero shot it
                    {
                        spawners[i].hp -= bullets[b].damage * Form1.pierceLevel; //same stuff as before
                        bullets.RemoveAt(b) ;
                        spawners[i].hitCooldown = 1;

                        if (spawners[i].hp <= 0)
                        {
                            Coin newcoin = new Coin(spawners[i].x, spawners[i].y, spawners[i].coinValue);
                            coins.Add(newcoin);
                            spawners.RemoveAt(i);
                            return;
                        }
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e) //I had to do a ovveride becasue regular mouse move didnt work
        {
            base.OnMouseMove(e);

            mouseX = e.X + camera.X;
            mouseY = e.Y + camera.Y;

            x = hero.x + hero.size / 2;
            y = hero.y + hero.size / 2; //takes points to find the angle so hero cannon can move with cursor

            double angle = CalculateAngle(x, y, mouseX, mouseY);//Finds the angle

            rotatedImage = RotateImage(hero.cannonImage, angle); //Rotates image
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
                            if (bullets[i].shooter == "Player" && w.type != "unbreakable") //if the wall isnt ubreakable and the player shit it
                            {
                                w.hp -= bullets[i].damage * StoreScreen.pierceLevel; //wall health goes down depending on the damage level

                                if(w.hp <= 0) //if the health is too low
                                {
                                    if(w.type == "groupBreak") //if the wall type is a groupbreak
                                    {
                                        walls.RemoveAll(wall => wall.type == "groupBreak"); //delets all other group breaks
                                    }
                                    walls.Remove(w);
                                }
                            }

                            bullets.RemoveAt(i);
                            break;
                        }
                    }
                    //if (bullets[i])
                }
            }
        }

        private void GameScreen_MouseDown(object sender, MouseEventArgs e)
        {
            if (playerFireCooldown <= 0) //if player can shoot
            {
                playerFireCooldown = 5; //reset the player fire cooldown

                mouseX = e.X + camera.X;
                mouseY = e.Y + camera.Y;

                x = hero.x + hero.size / 2;
                y = hero.y + hero.size / 2;

                double angle = CalculateAngle(x, y, mouseX, mouseY);

                double speed = 10; //sets bullet speed to 8

                var (xSpeed, ySpeed) = GetVelocity(angle, speed); //takes the x and y speed that will square together and equal 8

                Bullet newBullet = new Bullet(xSpeed, ySpeed, x, y, angle, "Player"); //create the bullet

                if (Form1.rocketLauncher == true)
                {
                    newBullet.damage = 50;
                    playerFireCooldown = 50;
                }
                bullets.Add(newBullet);//add the bullet to a list
               
            }
           
        }

        public static (double xSpeed, double ySpeed) GetVelocity(double angleDeg, double speed)
        {
            double angleRadians = angleDeg * (Math.PI / 180f);
            double xSpeed = speed * Math.Sin(angleRadians);
            double ySpeed = speed * Math.Cos(angleRadians); //some dumb math
            return (xSpeed, ySpeed);
        }

        public static double CalculateAngle(double x1, double y1, double x2, double y2)
        {
            double deltaX = x2 - x1;
            double deltaY = y2 - y1;

            double angleRadians = Math.Atan2(deltaX, deltaY);
            double angleDegrees = angleRadians * (180f / Math.PI);


            if (angleDegrees < 0) //if the angle is below 0
            {
                angleDegrees += 360f; //add 360 to get proper angle
            }

            return angleDegrees; //returning the proper angle 
        }

        static Bitmap RotateImage(Image image, double angle)
        {
            double radians = angle * Math.PI / 180;
            double cos = Math.Abs(Math.Cos(radians));
            double sin = Math.Abs(Math.Sin(radians));
            int newWidth = (int)(image.Width * cos + 25 * sin);
            int newHeight = (int)(image.Width * sin + 25 * cos);

            // Create new bitmap
            Bitmap rotatedBmp = new Bitmap(newWidth, newHeight);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics g = Graphics.FromImage(rotatedBmp))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;

                // Set the rotation point to the center of the new image
                g.TranslateTransform(newWidth / 2f, newHeight / 2f);
                g.RotateTransform(Convert.ToInt16(angle) * -1);
                g.TranslateTransform(-image.Width / 2f, -image.Height / 2f);

                // Draw original image at the origin
                g.DrawImage(image, new Point(0, 0));
            }

            return rotatedBmp;
        }


        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode) //simple key press code
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
            e.Graphics.DrawImage(hero.tankImage, hero.x - camera.X, hero.y - camera.Y, hero.size, hero.size);
            e.Graphics.DrawImage(rotatedImage, hero.x  - camera.X, hero.y - camera.Y, hero.size, hero.size);

            //Health Bar
            e.Graphics.FillRectangle(playerBrush, hero.x + 1 - camera.X, hero.y + hero.size + 3 - camera.Y, Convert.ToInt16(hero.size * (hero.health / hero.fullhealth)), 3);
            e.Graphics.DrawRectangle(healthPen, hero.x - camera.X, hero.y + hero.size + 2 - camera.Y, hero.size, 5);

            //Draws the spawners
            foreach (Spawner s in spawners)
            {
                e.Graphics.DrawImage(s.spawnerImage, s.x - camera.X, s.y - camera.Y, s.size, s.size);

                e.Graphics.FillRectangle(playerBrush, Convert.ToInt16(s.x) + 1 - camera.X, Convert.ToInt16(s.y) + s.size + 3 - camera.Y, Convert.ToInt16(s.size * (s.hp / s.fullHp)), 3);
                e.Graphics.DrawRectangle(healthPen, Convert.ToInt16(s.x) - camera.X, Convert.ToInt16(s.y) + s.size + 2 - camera.Y, s.size, 5);
            }

            //Draws the coins
            foreach (Coin c in coins)
            {
                e.Graphics.DrawImage(Properties.Resources.Coin, c.x - camera.X, c.y - camera.Y, c.size, c.size);
            }

            //Drawing the bullets
            foreach (Bullet b in bullets)
            {
                Brush redBrush = new SolidBrush(Color.Red);
                if (b.shooter == "Enemy")
                {
                    e.Graphics.FillRectangle(redBrush, Convert.ToInt16(b.x) - camera.X, Convert.ToInt16(b.y) - camera.Y, b.size, b.size);
                }
                else
                {
                    e.Graphics.FillRectangle(blueBrush, Convert.ToInt16(b.x) - camera.X, Convert.ToInt16(b.y) - camera.Y, b.size, b.size);
                }
            }

            //Drawing the enemys
            foreach (Enemy en in enemys)
            {
                e.Graphics.DrawImage(en.enemyImage, Convert.ToInt16(en.x) - camera.X, Convert.ToInt16(en.y) - camera.Y, en.size, en.size);
                e.Graphics.DrawImage(en.rotatedImage, Convert.ToInt16(en.x) - camera.X, Convert.ToInt16(en.y) - camera.Y, en.size, en.size);

                //Health Bar
                e.Graphics.FillRectangle(playerBrush, Convert.ToInt16(en.x) + 1 - camera.X, Convert.ToInt16(en.y) + en.size + 3 - camera.Y, Convert.ToInt16(en.size * (en.health / en.fullhealth)), 3);
                e.Graphics.DrawRectangle(healthPen, Convert.ToInt16(en.x) - camera.X, Convert.ToInt16(en.y) + en.size + 2 - camera.Y, en.size, 5);
            }

            //Draws the walls
            foreach (Wall w in walls)
            {
                if (w.type != "unbreakable")
                {
                    e.Graphics.DrawImage(w.wallImage, w.x - camera.X, w.y - camera.Y, w.width, w.height);
                }
                else
                {
                    e.Graphics.FillRectangle(w.wallBrush, w.x - camera.X, w.y - camera.Y, w.width, w.height);
                }
            }


           
        }
    }
}
