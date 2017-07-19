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
        public static double Magnitude(Vector2f p1, Vector2f p2)
        {
            return Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
        }
    }
}
