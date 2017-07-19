using SFML.Graphics;
using SFML.System;
using Functions;

namespace NeuralNetworkTest
{
    public class Ball
    {
        public Ball()
        {
            hitbox = new CircleShape(10);
            hitbox.Origin = new Vector2f(10f, 10f);
            hitbox.FillColor = Color.Cyan;
            Restart();
        }
        public void Restart()
        {
            Position = new Vector2f(640, 400);
            vel = new Vector2f(0.1f, 5f);
            Utility.Normalize(ref vel, MaxSpeed);

        }
        public float MaxSpeed = 10;
        public CircleShape hitbox;
        public Vector2f vel;

        public Vector2f Position
        {
            set { hitbox.Position = value; }
            get { return hitbox.Position; }
        }
        public void Update()
        {
            hitbox.Position += vel;
            if (hitbox.GetGlobalBounds().Left < 0)
            {
                vel.X *= -1;
                hitbox.Position += new Vector2f(-2 * hitbox.GetGlobalBounds().Left, 0);
            }
            if (hitbox.GetGlobalBounds().Left + hitbox.GetGlobalBounds().Width >= 1280)
            {
                vel.X *= -1;
                hitbox.Position += new Vector2f(-2 * (hitbox.GetGlobalBounds().Left + hitbox.GetGlobalBounds().Width - 1280), 0);
            }
            if (hitbox.GetGlobalBounds().Top < 0)
            {
                vel.Y *= -1;
                hitbox.Position += new Vector2f(0, -2 * hitbox.GetGlobalBounds().Top);
            }
        }
    }
}
