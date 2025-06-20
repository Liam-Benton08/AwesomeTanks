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
        public int coinValue;

        public int spawnTimer, spawnTime;
        public int hitCooldown;

        public bool canSpawn = true;

        public Image spawnerImage = Properties.Resources.Spawner;

        public Spawner(int _x, int _y, int _type)
        {
            x = _x;
            y = _y;
            type = _type;
           
            if (type == 1) //checks what type
            {
                hp = fullHp = 40; //gives seperate values for each type of spawner
                spawnTimer = 100;
                coinValue = 250;
            }
            else if (type == 2) 
            {
                hp = fullHp = 50;
                spawnTimer = 150;
                coinValue = 400;
            }
            else if (type == 3)
            {
                hp = fullHp = 100;
                spawnTimer = 200;
                coinValue = 2000;
            }
            else if(type == 4)
            {
                hp = fullHp = 80;
                spawnTimer = 80;
                coinValue = 5000;

            }
            else if(type == 5)
            {
                hp = fullHp = 150;
                spawnTimer = 750;
                coinValue = 20000;
            }
        }

        public Enemy spawnBots()
        {
            if (type == 1)
            {
                Enemy newEnemy = new Enemy(x, y, 20, 1, coinValue / 5); //gives enemys seperate values for each type of spawner
                spawnTime = spawnTimer; //resets spawn time

                return newEnemy;
            }
            else if (type == 2)
            {
                Enemy newEnemy = new Enemy(x, y, 35, 1, coinValue / 5);
                spawnTime = spawnTimer;

                return newEnemy;
            }
            else if (type == 3)
            {
                Enemy newEnemy = new Enemy(x, y, 60, 2, coinValue / 2);
                spawnTime = spawnTimer;

                return newEnemy;
            }
            else if (type == 4)
            {
                Enemy newEnemy = new Enemy(x, y, 10, 1, coinValue / 20);
                spawnTime = spawnTimer;

                return newEnemy;
            }
            else if (type == 5)
            {
                Enemy newEnemy = new Enemy(x, y, 100, 2, coinValue / 2);
                spawnTime = spawnTimer;

                return newEnemy;
            }    

            return null;
        }

        public bool BulletCollision(Bullet b)
        {
            Rectangle spawnRec = new Rectangle(Convert.ToInt16(x), Convert.ToInt16(y), size, size);
            Rectangle bulletRec = new Rectangle(Convert.ToInt16(b.x), Convert.ToInt16(b.y), b.size, b.size); //collisions again

            if (spawnRec.IntersectsWith(bulletRec))
            {
                return true;
            }

            return false;
        }
    }
}
