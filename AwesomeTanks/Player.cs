using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeTanks
{
    internal class Player
    {
        public int x, y;
        public int size = 25;
        public int speedX = 4 + Form1.speedLevel, speedY = 4 + Form1.speedLevel;
        public double health = 20, fullhealth = 20;

        public Image tankImage = Properties.Resources.Tank;
        public Image cannonImage = Properties.Resources.Cannon;

        public Player(int _x, int _y) 
        { 
            x = _x;
            y = _y;
        }

        public void MovePlayer(bool up, bool down, bool left, bool right)
        {
            if (up == true) //if up key is pressed move up
            {
                y -= speedY;
            }
            if (down == true) //if down key is pressed move down
            {
                y += speedY;
            }
            if (left == true) //if left is pressed...
            {
                x -= speedX;
            }
            if (right == true) //if right is pressed...
            {
                x += speedX;
            }
        }

        public bool BulletCollision(Bullet b)
        {
            Rectangle heroRec = new Rectangle(Convert.ToInt16(x), Convert.ToInt16(y), size, size);
            Rectangle bulletRec = new Rectangle(Convert.ToInt16(b.x), Convert.ToInt16(b.y), b.size, b.size); //gets both rectangles

            if (heroRec.IntersectsWith(bulletRec))//then sees if they would collide
            {
                return true;
            }

            return false;
        }

        public bool EnemyCollision(Enemy e)
        {
            Rectangle heroRec = new Rectangle(Convert.ToInt16(x), Convert.ToInt16(y), size, size);
            Rectangle enemyRec = new Rectangle(Convert.ToInt16(e.x), Convert.ToInt16(e.y), e.size, e.size); //same code different rectangles

            if (heroRec.IntersectsWith(enemyRec))
            {
                return true;
            }

            return false;
        }

        public bool CoinCollision(Coin c)
        {
            Rectangle heroRec = new Rectangle(Convert.ToInt16(x), Convert.ToInt16(y), size, size);
            Rectangle coinRec = new Rectangle(c.x, c.y, c.size, c.size);

            if (heroRec.IntersectsWith(coinRec))
            {
                return true;
            }

            return false;
        }

        public bool SpawnerCollision(Spawner s)
        {
            Rectangle heroRec = new Rectangle(Convert.ToInt16(x), Convert.ToInt16(y), size, size);
            Rectangle spawnerRec = new Rectangle(s.x, s.y, s.size, s.size);

            if (heroRec.IntersectsWith(spawnerRec))
            {
                return true;
            }

            return false;
        }
    }
}
