using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AwesomeTanks
{
    internal class Wall
    {
        public int x, y;
        public int width, height;
        public int hp, fullHp;
        public string type;

        public Brush wallBrush;
        public Image wallImage;

        public Wall(int _x, int _y, int _width, int _height, string _type)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            type = _type;

            GetProperties();
        }

        private void GetProperties()
        {
            if (type == "unbreakable")
            {
                wallBrush = new SolidBrush(Color.White); //sets colours based off of type
                hp = 1;
                wallImage = null;
            }
            else if (type == "soloBreak")
            {
                wallBrush = new SolidBrush(Color.Red);
                hp = 20;
                wallImage = Properties.Resources.brick;
            }
            else
            {
                wallBrush = new SolidBrush(Color.Yellow);
                hp = 30;
                wallImage= Properties.Resources.stone;
            }
        }

        public bool Collision(Wall w, Player hero)
        {
            Rectangle playerRec = new Rectangle(hero.x, hero.y, hero.size, hero.size);
            Rectangle wallRec = new Rectangle(w.x, w.y, w.width, w.height); //makes the rectangles

            if (playerRec.IntersectsWith(wallRec)) //checks for collisions
            {
                return true;
            }

            return false;
        }

        public bool AICollision(Wall w, Enemy enemy)
        {
            Rectangle enemyRec = new Rectangle(Convert.ToInt16(enemy.x), Convert.ToInt16(enemy.y), enemy.size, enemy.size);
            Rectangle wallRec = new Rectangle(w.x, w.y, w.width, w.height);//same old same old

            if (enemyRec.IntersectsWith(wallRec))
            {
                return true;
            }

            return false;
        }

        public bool BulletCollision(Wall w, Bullet b)
        {
            Rectangle bulletRec = new Rectangle(Convert.ToInt16(b.x), Convert.ToInt16(b.y), b.size, b.size);
            Rectangle wallRec = new Rectangle(w.x, w.y, w.width, w.height);

            if (bulletRec.IntersectsWith(wallRec))
            {
                return true;
            }

            return false;
        }
    }
}
