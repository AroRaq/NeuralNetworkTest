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
        public RectangleShape hitbox;
    }
}
