using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace Functions
{
    class Utility
    {
        public static Random random = new Random();
        public static double RandomDouble(double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }
        public static void Normalize(ref Vector2f v, float l)
        {
            float L = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
            v.X /= L;
            v.X *= l;
            v.Y /= L;
            v.Y *= l;
        }
        public static bool IsMouseInRect(IntRect r, ref RenderWindow w)
        {
            return Mouse.GetPosition(w).X >= r.Left && Mouse.GetPosition(w).X <= r.Left + r.Width &&
                   Mouse.GetPosition(w).Y >= r.Top && Mouse.GetPosition(w).Y <= r.Top + r.Height;
        }
        public static Color Monochrome(double c)
        {
            return new Color(Convert.ToByte(c), Convert.ToByte(c), Convert.ToByte(c));
        }
        public static Color WhiteAlpha(double c)
        {
            return new Color(255, 255, 255, Convert.ToByte(c));
        }
    }
}
