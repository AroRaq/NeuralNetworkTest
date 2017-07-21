using SFML.Graphics;
using SFML.System;
using Functions;

namespace NeuralNetworkTest
{
    public class Game : Drawable
    {
        public Game()
        {
            for (int y = 0; y < Properties.BrickTable.Y; y++)
            {
                for (int x = 0; x < Properties.BrickTable.X; x++)
                {
                    Bricks[y, x] = new Brick(80 + x * 160, 40 + y * 80, 160, 80);
                }
            }
        }

        public Ball Ball1 = new Ball();
        public Paddle Paddle1 = new Paddle();
        public Brick[,] Bricks = new Brick[Properties.BrickTable.Y, Properties.BrickTable.X];
        public Bot bot1;
        public int FrameCount = 0;
        public int SinceHit = 0;
        public int FinalScore = 0;
        public int BricksLeft = Properties.BrickAmount;
        public void Restart()
        {
            SinceHit = 0;
            BricksLeft = Properties.BrickAmount;
            FinalScore = 0;
            FrameCount = 0;
            Ball1.Restart();
            Paddle1.Restart();
            for (int y = 0; y < Properties.BrickTable.Y; y++)
            {
                for (int x = 0; x < Properties.BrickTable.X; x++)
                {
                    Bricks[y, x].Heal();
                }
            }
        }

        public void UseBot(ref Bot b)
        {
            bot1 = b;
        }

        public int RunFrame()
        {
            FloatRect intersection;
            FrameCount++;
            SinceHit++;
            //if (Keyboard.IsKeyPressed(Keyboard.Key.Left) && Paddle1.hitbox.GetGlobalBounds().Left >= 15)
            //    Paddle1.hitbox.Position -= new Vector2f(15, 0);
            //if (Keyboard.IsKeyPressed(Keyboard.Key.Right) && Paddle1.hitbox.GetGlobalBounds().Left+Paddle1.hitbox.GetGlobalBounds().Width <=1265)
            //    Paddle1.hitbox.Position += new Vector2f(15, 0);

            if (SinceHit > 1200)
            {
                GameOver(false);
            }
            Ball1.Update();
            for (int y = 0; y < Properties.BrickTable.Y; y++)
            {
                for (int x = 0; x < Properties.BrickTable.X; x++)
                {
                    if (Bricks[y, x].health > 0)
                    {
                        if (Bricks[y, x].hitbox.GetGlobalBounds().Intersects(Ball1.hitbox.GetGlobalBounds(), out intersection))
                        {
                            SinceHit = 0;
                            FinalScore++;
                            if (Bricks[y, x].OnHit(Ball1, intersection))
                            {
                                FinalScore += 5;
                                BricksLeft--;
                                if (BricksLeft == 0)
                                {
                                    FinalScore += 50;
                                    return GameOver();
                                }
                            }
                        }
                    }
                }
            }
            
            if (Paddle1.hitbox.GetGlobalBounds().Intersects(Ball1.hitbox.GetGlobalBounds(), out intersection))
            {
                bot1.BounceCount++;
                Ball1.vel.Y *= -1;
                Ball1.vel.X += -0.05f * (Paddle1.Position.X - Ball1.Position.X);
                Utility.Normalize(ref Ball1.vel, Ball1.MaxSpeed);
                if (Ball1.vel.Y > -2f)
                {
                    Ball1.vel.Y *= 2f;
                    Utility.Normalize(ref Ball1.vel, Ball1.MaxSpeed);
                }
                Ball1.hitbox.Position += new Vector2f(0f, -intersection.Height);
            }

            if (Ball1.hitbox.GetGlobalBounds().Top >= 720)
            {
                return GameOver();
            }
            return -1;
        }

        public int GameOver(bool b = true)
        {
            int f = FinalScore - FrameCount / 180;
            //int f = FinalScore;
            if (bot1.BounceCount == 0 || !b)
                f = 0;
            Restart();
            return f;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Ball1.hitbox);
            target.Draw(Paddle1.hitbox);
            foreach (Brick b in Bricks)
            {
                if (b.health > 0)
                    target.Draw(b.hitbox);
            }
        }
    }

}
