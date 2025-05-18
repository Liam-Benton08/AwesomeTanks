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
        public double x, y;
        public int size = 15;
        public double speedX = 5, speedY = 5;
        public double health = 10, fullhealth = 10;

        public int hitCooldown;
        public int fireCooldown;

        public Enemy(double _x, double _y)
        {
            x = _x;
            y = _y;
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
            double angle = GameScreen.CalculateAngle(x1, y1, x2, y2);

            double speed = 2;

            var (xSpeed, ySpeed) = GameScreen.GetVelocity(angle, speed);

            x += xSpeed;
            y += ySpeed;
        }

        public Bullet FireBullet(double x1, double y1, double x2, double y2)
        {
            double angle = GameScreen.CalculateAngle(x1, y1, x2, y2);

            double speed = 8;

            var (xSpeed, ySpeed) = GameScreen.GetVelocity(angle, speed);

            Bullet newBullet = new Bullet(xSpeed, ySpeed, x, y, angle, "Enemy");
            return newBullet;
        }
    }
}
