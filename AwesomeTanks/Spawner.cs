using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AwesomeTanks
{
    internal class Spawner
    {
        public int x, y;
        public int size = 30;
        public double hp, fullHp;
        public int type;

        public int spawnTimer, spawnTime;
        public int hitCooldown;



        public Spawner(int _x, int _y, int _type)
        {
            x = _x;
            y = _y;
            type = _type;
           
            if (type == 1)
            {
                hp = fullHp = 40;
                spawnTimer = spawnTime = 200;
            }
        }

        public Enemy spawnBots()
        {
            if (type == 1)
            {
                Enemy newEnemy = new Enemy(x, y, 10, 50);
                spawnTime = spawnTimer;

                return newEnemy;
            }

            return null;
        }

        public bool BulletCollision(Bullet b)
        {
            Rectangle spawnRec = new Rectangle(Convert.ToInt16(x), Convert.ToInt16(y), size, size);
            Rectangle bulletRec = new Rectangle(Convert.ToInt16(b.x), Convert.ToInt16(b.y), b.size, b.size);

            if (spawnRec.IntersectsWith(bulletRec))
            {
                return true;
            }

            return false;
        }
    }
}
