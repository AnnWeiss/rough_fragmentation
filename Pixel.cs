using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace App1
{
    class Pixel
    {
        public int X { get; set; }
        public int Y { get; set; }
        Color pixelColor;

        public Pixel(int x, int y, Color c)
        {
            X = x;
            Y = y;
            pixelColor = c;
        }
        public Color GetColor()
        {
            return pixelColor;
        }
    }


}
