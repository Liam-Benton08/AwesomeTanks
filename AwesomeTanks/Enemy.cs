using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeTanks
{
    internal class Enemy
    {
        public double x, y, lastX, lastY;
        public int size = 25;
        public double speedX = 5, speedY = 5;
        public double health, fullhealth;
        public int damageMulti;
        public int coinValue;

        public int hitCooldown;
        public int fireCooldown;

        public bool wallInBetween;

        public Image enemyImage = Properties.Resources.Enemy;
        public Image enemyCannonImage = Properties.Resources.EnemyCannon;

        public Bitmap rotatedImage = Properties.Resources.Enemy;

        public Enemy(double _x, double _y, double _health, int _damageMulti, int _coinValue)
        {
            x = _x;
            y = _y;
            health = fullhealth = _health;
            damageMulti = _damageMulti;
            coinValue = _coinValue;

            if (health == 10000)
            {
                size = 50;
            }
        }

        public bool BulletCollision(Enemy e, Bullet b)
        {
            Rectangle enemyRec = new Rectangle(Convert.ToInt16(e.x), Convert.ToInt16(e.y), e.size, e.size);
            Rectangle bulletRec = new Rectangle(Convert.ToInt16(b.x), Convert.ToInt16(b.y), b.size, b.size);

            if (enemyRec.IntersectsWith(bulletRec))
            {
                return true;
            }

            return false;
        }

        public void MoveEnemy(double x1, double y1, double x2, double y2)
        {
            double angle = GameScreen.CalculateAngle(x1, y1, x2, y2); //calculates the angle between the player and enemy 

            double speed = 2;

            var (xSpeed, ySpeed) = GameScreen.GetVelocity(angle, speed); //gets speed proportional to angle

            x += xSpeed;
            y += ySpeed;
        }

        public Bullet FireBullet(double x1, double y1, double x2, double y2, double speed)
        {
            double angle = GameScreen.CalculateAngle(x1, y1, x2, y2);

            var (xSpeed, ySpeed) = GameScreen.GetVelocity(angle, speed);

            Bullet newBullet = new Bullet(xSpeed, ySpeed, x1, y1, angle, "Enemy"); //fires bullet based of angle at player
            return newBullet;
        }
    }
}
