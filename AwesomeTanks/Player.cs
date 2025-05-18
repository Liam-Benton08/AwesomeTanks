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
        public int x = 400, y = 400;
        public int size = 15;
        public int speedX = 5, speedY = 5;
        public double health = 20, fullhealth = 20;

        public string direction = "none";

        public Player() 
        { 
        
        }

        public void MovePlayer(bool up, bool down, bool left, bool right)
        {
            if (up == true)
            {
                y -= speedY;
            }
            if (down == true)
            {
                y += speedY;
            }
            if (left == true)
            {
                x -= speedX;
            }
            if (right == true)
            {
                x += speedX;
            }
        }

        public bool BulletCollision(Bullet b)
        {
            Rectangle heroRec = new Rectangle(Convert.ToInt16(x), Convert.ToInt16(y), size, size);
            Rectangle bulletRec = new Rectangle(Convert.ToInt16(b.x), Convert.ToInt16(b.y), b.size, b.size);

            if (heroRec.IntersectsWith(bulletRec))
            {
                return true;
            }

            return false;
        }
    }
}
