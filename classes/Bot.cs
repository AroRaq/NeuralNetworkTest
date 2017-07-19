﻿using System;
using SFML.Graphics;
using SFML.System;
using Functions;

namespace NeuralNetworkTest
{
    public class Bot
    {
        public Bot()
        {
            set1 = new double[30, Nodes];
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < Nodes; j++)
                {
                    set1[i, j] = Utility.RandomDouble(-1d, 1d);
                }
            }
            set2 = new double[Nodes];
            for (int i = 0; i < Nodes; i++)
            {
                set2[i] = Utility.RandomDouble(-1d, 1d);
            }
        }
        public Bot(Bot other)
        {
            Bricks = other.Bricks;
            Ball1 = other.Ball1;
            Paddle1 = other.Paddle1;
            set1 = new double[30, Nodes];
            set1 = (double[,])other.set1.Clone();
            set2 = (double[])other.set2.Clone();
            Nodes = other.Nodes;
            //FinalScore = other.FinalScore;
            //BounceCount = other.BounceCount;
        }
        public Bot(ref Brick[,] _Bricks, ref Ball _Ball1, ref Paddle _Paddle1, int _Nodes = 5)
        {
            Bricks = _Bricks;
            Nodes = _Nodes;
            Ball1 = _Ball1;
            Paddle1 = _Paddle1;
            set1 = new double[30, Nodes];
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < Nodes; j++)
                {
                    set1[i, j] = Utility.RandomDouble(-1d, 1d);
                }
            }
            set2 = new double[Nodes];
            for (int i = 0; i < Nodes; i++)
            {
                set2[i] = Utility.RandomDouble(-1d, 1d);
            }
        }
        public Bot Reproduce(Bot other)
        {
            Bot Child = new Bot(other);
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < Child.Nodes; j++)
                {
                    Child.set1[i, j] = (set1[i, j] + other.set1[i, j]) / 2;
                }
            }
            Child.set2 = new double[Nodes];
            for (int i = 0; i < Child.Nodes; i++)
            {
                Child.set2[i] = (set2[i] + other.set2[i]) / 2;
            }
            if (Utility.Chance(0.5))
                Child.Mutate();
            return Child;
        }
        public Paddle Paddle1;
        public Ball Ball1;
        public Brick[,] Bricks;
        public double[,] set1;
        public double[] set2;
        public int Nodes = 5;
        public int FinalScore = 0;
        public int BounceCount = 0;
        public void CreateNew(int nodes = 5)
        {
            FinalScore = 0;
            BounceCount = 0;
            Nodes = nodes;
            set1 = new double[30, Nodes];
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < Nodes; j++)
                {
                    set1[i, j] = Utility.RandomDouble(-1d, 1d);
                }
            }
            set2 = new double[Nodes];
            for (int i = 0; i < Nodes; i++)
            {
                set2[i] = Utility.RandomDouble(-1d, 1d);
            }
        }
        public int CalculateOutput()
        {
            double[] temp = new double[Nodes];
            for (int n = 0; n < Nodes; n++)
            {
                temp[n] = 0;
            }
            for (int n = 0; n < Nodes; n++)
            {
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 7; x++)
                    {
                        temp[n] += Convert.ToDouble(Utility.Magnitude(Bricks[y, x].Position, Paddle1.Position)) * 0.2d * set1[7 * y + x, n];
                    }
                }
                temp[n] += set1[28, n] * Convert.ToDouble(Ball1.Position.X - Ball1.Position.X) / 1280f * 10;
                temp[n] += set1[29, n] * Convert.ToDouble(Ball1.Position.Y - Ball1.Position.Y) / 720f * 10;
                temp[n] /= 30;
            }
            double output = 0;
            for (int i = 0; i < Nodes; i++)
            {
                output += temp[i] * set2[i];
            }
            output /= Nodes;
            //System.Console.WriteLine(output);
            if (output >= 0)
                return 1;
            else
                return -1;
        }
        public void Finished()
        {
            BounceCount = 0;
        }
        public void Mutate()
        {
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < Nodes; j++)
                {
                    if (Utility.Chance(0.1d))
                        set1[i, j] = (set1[i, j] + Utility.RandomDouble(-1d, 1d)) / 2;
                }
            }
            for (int i = 0; i < Nodes; i++)
            {
                set2[i] = (set2[i] + Utility.RandomDouble(-1d, 1d)) / 2;
            }
        }
        public void DrawNet(ref RenderWindow RW)
        {
            CircleShape[] Circles = new CircleShape[30+Nodes+1];
            for (int i=0; i<30; i++)
            {
                Circles[i] = new CircleShape(5);
                //Circles[i].FillColor = Utility.Monochrome((set1[i - i % Nodes, i % Nodes]+1) / 2 * 255);
                Circles[i].Position = new Vector2f(10, 10+i*30);
                Circles[i].Origin = new Vector2f(5, 5);
            }
            for (int i=30; i<30+Nodes; i++)
            {
                Circles[i] = new CircleShape(5);
                Circles[i].FillColor = Color.White;
                Circles[i].Position = new Vector2f(210, 15 + (i-30) * 215);
                Circles[i].Origin = new Vector2f(5, 5);
            }

            Circles[30 + Nodes] = new CircleShape(5);
            Circles[30 + Nodes].FillColor = Color.White;
            Circles[30 + Nodes].Position = new Vector2f(310, 440);
            Circles[30 + Nodes].Origin = new Vector2f(5, 5);

            for (int i=0; i<30; i++)
            {
                for (int n=0; n<Nodes; n++)
                {
                    RW.Draw(new Line(Circles[i].Position, Circles[30+n].Position, Utility.WhiteAlpha((set1[i - i % Nodes, i % Nodes] + 1) / 2 * 255)));
                }
            }
            for (int n = 0; n < Nodes; n++)
            {
                RW.Draw(new Line(Circles[30+n].Position, Circles[30+Nodes].Position, Utility.WhiteAlpha((set2[n] + 1) / 2 * 255)));
            }
            for (int i=0; i<Circles.Length; i++)
            {
                RW.Draw(Circles[i]);
            }
        }
    }
}
