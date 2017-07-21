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
    static class Utility
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
        public static bool Chance(double c)
        {
            if (RandomDouble(0, 1) <= c)
                return true;
            return false;
        }
        public static void ShuffleArray<T>(T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = random.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public static void Shuffle<T>(this IList<T> list, int start, int stop)
        {
            int n = stop - start;
            while (n > 1)
            {
                n--;
                int k = random.Next(start , start + n + 1);
                T value = list[k];
                list[k] = list[start + n];
                list[start + n] = value;
            }
        }
        public static double Magnitude(Vector2f p1, Vector2f p2)
        {
            return Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
        }
        public static Color Spectral_color(double l) // RGB <0,1> <- lambda l <400,700> [nm]
        {
            double t = 0;
            Color c = Color.Black;
            if ((l >= 400.0) && (l < 410.0))
            {
                t = (l - 400.0) / (410.0 - 400.0);
                c.R = (Byte)(255 * ((0.33 * t) - (0.20 * t * t)));
            }
            else if ((l >= 410.0) && (l < 475.0))
            {
                t = (l - 410.0) / (475.0 - 410.0);
                c.R = (Byte)(255 * (0.14 - (0.13 * t * t)));
            }
            else if ((l >= 545.0) && (l < 595.0))
            {
                t = (l - 545.0) / (595.0 - 545.0);
                c.R = (Byte)(255 * ((1.98 * t) - (t * t)));
            }
            else if ((l >= 595.0) && (l < 650.0))
            {
                t = (l - 595.0) / (650.0 - 595.0);
                c.R = (Byte)(255 * (0.98 + (0.06 * t) - (0.40 * t * t)));
            }
            else if ((l >= 650.0) && (l < 700.0))
            {
                t = (l - 650.0) / (700.0 - 650.0);
                c.R = (Byte)(255 * (0.65 - (0.84 * t) + (0.20 * t * t)));
            }
            if ((l >= 415.0) && (l < 475.0))
            {
                t = (l - 415.0) / (475.0 - 415.0);
                c.G = (Byte)(255 * ((0.80 * t * t)));
            }
            else if ((l >= 475.0) && (l < 590.0))
            {
                t = (l - 475.0) / (590.0 - 475.0);
                c.G = (Byte)(255 * (0.8 + (0.76 * t) - (0.80 * t * t)));
            }
            else if ((l >= 585.0) && (l < 639.0))
            {
                t = (l - 585.0) / (639.0 - 585.0);
                c.G = (Byte)(255 * (0.84 - (0.84 * t)));
            }
            if ((l >= 400.0) && (l < 475.0))
            {
                t = (l - 400.0) / (475.0 - 400.0);
                c.B = (Byte)(255 * ((2.20 * t) - (1.50 * t * t)));
            }
            else if ((l >= 475.0) && (l < 560.0))
            {
                t = (l - 475.0) / (560.0 - 475.0);
                c.B = (Byte)(255*(0.7 - (t) + (0.30 * t * t)));
            }
            return c;
        }
        public static Color Spectral_color2(double l) // RGB <0,1> <- lambda l <400,700> [nm]
        {
            l *= 300;
            l += 400;
            double t = 0;
            Color c = Color.Black;
            if ((l >= 400.0) && (l < 410.0))
            {
                t = (l - 400.0) / (410.0 - 400.0);
                c.R = (Byte)(255 * ((0.33 * t) - (0.20 * t * t)));
            }
            else if ((l >= 410.0) && (l < 475.0))
            {
                t = (l - 410.0) / (475.0 - 410.0);
                c.R = (Byte)(255 * (0.14 - (0.13 * t * t)));
            }
            else if ((l >= 545.0) && (l < 595.0))
            {
                t = (l - 545.0) / (595.0 - 545.0);
                c.R = (Byte)(255 * ((1.98 * t) - (t * t)));
            }
            else if ((l >= 595.0) && (l < 650.0))
            {
                t = (l - 595.0) / (650.0 - 595.0);
                c.R = (Byte)(255 * (0.98 + (0.06 * t) - (0.40 * t * t)));
            }
            else if ((l >= 650.0) && (l < 700.0))
            {
                t = (l - 650.0) / (700.0 - 650.0);
                c.R = (Byte)(255 * (0.65 - (0.84 * t) + (0.20 * t * t)));
            }
            if ((l >= 415.0) && (l < 475.0))
            {
                t = (l - 415.0) / (475.0 - 415.0);
                c.G = (Byte)(255 * ((0.80 * t * t)));
            }
            else if ((l >= 475.0) && (l < 590.0))
            {
                t = (l - 475.0) / (590.0 - 475.0);
                c.G = (Byte)(255 * (0.8 + (0.76 * t) - (0.80 * t * t)));
            }
            else if ((l >= 585.0) && (l < 639.0))
            {
                t = (l - 585.0) / (639.0 - 585.0);
                c.G = (Byte)(255 * (0.84 - (0.84 * t)));
            }
            if ((l >= 400.0) && (l < 475.0))
            {
                t = (l - 400.0) / (475.0 - 400.0);
                c.B = (Byte)(255 * ((2.20 * t) - (1.50 * t * t)));
            }
            else if ((l >= 475.0) && (l < 560.0))
            {
                t = (l - 475.0) / (560.0 - 475.0);
                c.B = (Byte)(255 * (0.7 - (t) + (0.30 * t * t)));
            }
            return c;
        }
    }
}
