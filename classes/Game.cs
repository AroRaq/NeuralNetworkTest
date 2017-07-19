﻿using SFML.Graphics;
using SFML.System;
using Functions;

namespace NeuralNetworkTest
{
    public class Game : Drawable
    {
        public Game()
        {
            FinalScore = 0;
            FrameCount = 0;
            Ball1 = new Ball();
            Paddle1 = new Paddle();
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    Bricks[y, x] = new Brick(80 + x * 160, 40 + y * 80, 160, 80);
                }
            }
        }

        public Ball Ball1 = new Ball();
        public Paddle Paddle1 = new Paddle();
        public Brick[,] Bricks = new Brick[4, 7];
        public Bot bot1;
        public int FrameCount = 0;
        public int FinalScore = 0;
        public int BricksLeft = 28;
        public void Restart()
        {
            BricksLeft = 28;
            FinalScore = 0;
            FrameCount = 0;
            Ball1 = new Ball();
            Paddle1 = new Paddle();
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    Bricks[y, x] = new Brick(80 + x * 160, 40 + y * 80, 160, 80);
                }
            }
        }

        public void UseBot(ref Bot b)
        {
            bot1 = b;
        }

        public int RunFrame()
        {
            FrameCount++;
            //if (Keyboard.IsKeyPressed(Keyboard.Key.Left) && Paddle1.hitbox.GetGlobalBounds().Left >= 15)
            //    Paddle1.hitbox.Position -= new Vector2f(15, 0);
            //if (Keyboard.IsKeyPressed(Keyboard.Key.Right) && Paddle1.hitbox.GetGlobalBounds().Left+Paddle1.hitbox.GetGlobalBounds().Width <=1265)
            //    Paddle1.hitbox.Position += new Vector2f(15, 0);

            Ball1.Update();
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    if (Bricks[y, x].health > 0)
                    {
                        FloatRect intersection;
                        if (Bricks[y, x].hitbox.GetGlobalBounds().Intersects(Ball1.hitbox.GetGlobalBounds(), out intersection))
                        {
                            FinalScore++;
                            if (Bricks[y, x].onHit(Ball1, intersection))
                            {
                                FinalScore += 4;
                                BricksLeft--;
                                if (BricksLeft == 0)
                                {
                                    FinalScore += 20;
                                    return GameOver();
                                }
                            }
                        }
                    }
                }
            }

            FloatRect intersection1;
            if (Paddle1.hitbox.GetGlobalBounds().Intersects(Ball1.hitbox.GetGlobalBounds(), out intersection1))
            {
                bot1.BounceCount++;
                Ball1.vel.Y *= -1;
                Ball1.vel.X += -0.1f * (Paddle1.hitbox.Position.X - Ball1.hitbox.Position.X);
                Utility.Normalize(ref Ball1.vel, Ball1.MaxSpeed);
                if (Ball1.vel.Y > -2f)
                {
                    Ball1.vel.Y -= 4f;
                    Utility.Normalize(ref Ball1.vel, Ball1.MaxSpeed);
                }
                Ball1.hitbox.Position += new Vector2f(0f, -intersection1.Height);
            }

            if (Ball1.hitbox.GetGlobalBounds().Top >= 720)
            {
                return GameOver();
            }
            return -1;
        }

        public int GameOver()
        {
            int f = FinalScore - FrameCount / 180;
            if (bot1.BounceCount == 0)
                f = 0;
            System.Console.WriteLine(f);
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

        public void Right()
        {
            if (Paddle1.hitbox.GetGlobalBounds().Left + Paddle1.hitbox.GetGlobalBounds().Width <= 1265)
                Paddle1.hitbox.Position += new Vector2f(15, 0);
        }
        public void Left()
        {
            if (Paddle1.hitbox.GetGlobalBounds().Left >= 15)
                Paddle1.hitbox.Position -= new Vector2f(15, 0);
        }
    }

}