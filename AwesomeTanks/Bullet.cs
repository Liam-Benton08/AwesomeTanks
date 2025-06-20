using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeTanks
{
    internal class Bullet
    {
        public double speedX, speedY;
        public double x, y;
        public double angle;
        public int size = 5;
        public int damage = 5;
        public string shooter;


        Image bulletImage;

        public Bullet(double _speedX, double _speedY, double _x, double _y, double _angle, string _shooter)
        {
            speedX = _speedX;
            speedY = _speedY;
            x = _x;
            y = _y;
            angle = _angle;
            shooter = _shooter;
        }

        public void MoveBullet()
        {
            x += speedX; //moves bullets
            y += speedY;
        }
    }
}
