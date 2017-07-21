using System;
using SFML.Graphics;
using SFML.System;
using Functions;

namespace NeuralNetworkTest
{
    public class Bot
    {
        public Bot()
        {
            set1 = new double[Properties.BrickAmount+2, Nodes];
            for (int i = 0; i < Properties.BrickAmount+2; i++)
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
            set1 = new double[Properties.BrickAmount+2, Nodes];
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
            set1 = new double[Properties.BrickAmount+2, Nodes];
            for (int i = 0; i < Properties.BrickAmount+2; i++)
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
            for (int i = 0; i < Properties.BrickAmount+2; i++)
            {
                if (Utility.Chance(0.5))
                {
                    for (int j = 0; j < Child.Nodes; j++)
                    {
                        Child.set1[i, j] = set1[i, j];
                    }
                }
                else
                {
                    for (int j = 0; j < Child.Nodes; j++)
                    {
                        Child.set1[i, j] = other.set1[i, j];
                    }
                }
            }
            Child.set2 = new double[Nodes];
            for (int i = 0; i < Child.Nodes; i++)
            {
                if (Utility.Chance(0.5))
                    Child.set2[i] = set2[i];
                else
                    Child.set2[i] = other.set2[i];
            }
            if (Utility.Chance(0.5))
                Child.Mutate(1);
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
            set1 = new double[Properties.BrickAmount, Nodes];
            for (int i = 0; i < Properties.BrickAmount; i++)
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
            double Sum = 0;
            double Output = 0;
            for (int n = 0; n < Nodes; n++)
            {
                for (int y = 0; y < Properties.BrickTable.Y; y++)
                {
                    for (int x = 0; x < Properties.BrickTable.X; x++)
                    {
                        Sum += Utility.Magnitude(Bricks[y, x].Position, Paddle1.Position) / 1300d * Bricks[y, x].health * 0.2d * set1[7 * y + x, n];
                     }
                }
                Sum += 2*set1[Properties.BrickAmount-2, n] * (Ball1.Position.X - Paddle1.Position.X) / 1280d;
                Sum += set1[Properties.BrickAmount-1, n] * (Ball1.Position.Y - Paddle1.Position.Y) / 720d;
                Sum /= Properties.BrickAmount+2;
                Output += Sum * set2[n];
            }
            Output /= Nodes;
            //System.Console.WriteLine(output);
            if (Output >= 0)
                return 1;
            else
                return -1;
        }
        public void Finished()
        {
            BounceCount = 0;
        }
        public void Mutate(double Intensity = 0.1d)
        {
            for (int i = 0; i < Properties.BrickAmount; i++)
            {
                for (int j = 0; j < Nodes; j++)
                {
                    if (Utility.Chance(Intensity))
                    {
                        set1[i, j] = (set1[i, j] * 4d + Utility.RandomDouble(-1d, 1d)) * 0.2;
                    }
                }
            }
            for (int i = 0; i < Nodes; i++)
            {
                if (Utility.Chance(Intensity))
                {
                    set2[i] = (set2[i] * 4 + Utility.RandomDouble(-1d, 1d)) * 0.2;
                }
            }
        }
        public void DrawNet(ref RenderWindow RW)
        {
            CircleShape[] Circles = new CircleShape[Properties.BrickAmount + Nodes + 3];
            for (int i=0; i< Properties.BrickAmount+2; i++)
            {
                Circles[i] = new CircleShape(5);
                //Circles[i].FillColor = Utility.Monochrome((set1[i - i % Nodes, i % Nodes]+1) / 2 * 255);
                Circles[i].Position = new Vector2f(10, Properties.NetWindowSize.Y / (Properties.BrickAmount + 2) * (i+1));
                Circles[i].Origin = new Vector2f(5, 5);
            }
            for (int i = Properties.BrickAmount+2; i < Properties.BrickAmount + Nodes+2; i++)
            {
                Circles[i] = new CircleShape(5);
                Circles[i].FillColor = Color.White;
                Circles[i].Position = new Vector2f(210, Properties.NetWindowSize.Y / (Nodes+1) * (i - Properties.BrickAmount - 1));
                Circles[i].Origin = new Vector2f(5, 5);
            }

            Circles[Properties.BrickAmount + Nodes + 2] = new CircleShape(5);
            Circles[Properties.BrickAmount + Nodes + 2].FillColor = Color.White;
            Circles[Properties.BrickAmount + Nodes + 2].Position = new Vector2f(310, Properties.NetWindowSize.Y/2);
            Circles[Properties.BrickAmount + Nodes + 2].Origin = new Vector2f(5, 5);

            for (int i = 0; i < Properties.BrickAmount+2; i++)
            {
                for (int n=0; n<Nodes; n++)
                {
                    try
                    {
                        RW.Draw(new Line(Circles[i].Position, Circles[Properties.BrickAmount + n + 2].Position, Utility.Monochrome((set1[i - i % Nodes, i % Nodes] + 1) / 2 * 255)));
                    }
                    catch (System.OverflowException)
                    {
                        System.Console.WriteLine(set1[i - i % Nodes, i % Nodes]);
                    }

                }
            }
            for (int n = 0; n < Nodes; n++)
            {
                RW.Draw(new Line(Circles[Properties.BrickAmount + 2 + n].Position, Circles[Properties.BrickAmount  + 2 + Nodes].Position, Utility.Monochrome((set2[n] + 1) / 2 * 255)));
            }
            for (int i=0; i<Circles.Length; i++)
            {
                RW.Draw(Circles[i]);
            }
        }
    }
}
