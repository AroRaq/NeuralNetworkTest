using SFML.Graphics;
using SFML.System;

namespace NeuralNetworkTest
{
    public class Paddle
    {
        public Paddle()
        {
            hitbox = new RectangleShape(new Vector2f(160, 10));
            hitbox.Origin = new Vector2f(80, 5);
            hitbox.Position = new Vector2f(640, 650);
            hitbox.FillColor = Color.Green;
        }
        public void Restart()
        {
            hitbox.Position = new Vector2f(640, 650);
        }
        public Vector2f Position
        {
            get { return hitbox.Position; }
            set { hitbox.Position = value; }
        }
        public void Left()
        {
            if (hitbox.GetGlobalBounds().Left >= Speed)
                hitbox.Position -= new Vector2f(Speed, 0);
        }
        public void Right()
        {
            if (hitbox.GetGlobalBounds().Left + hitbox.GetGlobalBounds().Width <= Properties.WindowSize.X - Speed)
                hitbox.Position += new Vector2f(Speed, 0);
        }
        public RectangleShape hitbox;
        public float Speed = 30;
    }
}
