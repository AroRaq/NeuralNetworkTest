using System;
using SFML.Graphics;
using SFML.System;

namespace NeuralNetworkTest
{
    public class Brick
    {
        public Brick(int x, int y, int w, int h)
        {
            hitbox = new RectangleShape(new Vector2f(w - 2, h - 2));
            hitbox.Origin = new Vector2f(w / 2, h / 2);
            hitbox.Position = new Vector2f(x + 1, y + 1);
            hitbox.FillColor = Color.Yellow;
        }
        public RectangleShape hitbox;
        public int health = 5;
        public bool OnHit(Ball b, FloatRect intersection)
        {
            health--;
            hitbox.FillColor = new Color(255, 255, 0, Convert.ToByte(health * 50));
            if (intersection.Height < intersection.Width)
            {
                b.vel.Y *= -1;
                b.hitbox.Position += new Vector2f(0f, intersection.Height * Math.Sign(b.vel.Y));
            }
            else if (intersection.Height > intersection.Width)
            {
                b.vel.X *= -1;
                b.hitbox.Position += new Vector2f(intersection.Width * Math.Sign(-b.vel.X), 0f);
            }
            else
            {
                b.vel *= -1;
                b.hitbox.Position += new Vector2f(intersection.Width * Math.Sign(-b.vel.X), intersection.Height * Math.Sign(b.vel.Y));
            }
            return health == 0;
        }
        public Vector2f Position
        {
            get { return hitbox.Position; }
            set { hitbox.Position = value; }
        }
    }
}
