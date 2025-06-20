using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeTanks
{
    internal class Coin
    {
        public int x, y;
        public int size = 25;
        public int value;

        public Coin(int _x, int _y, int _value) 
        {
            x = _x;
            y = _y;
            value = _value;
        }


    }
}
