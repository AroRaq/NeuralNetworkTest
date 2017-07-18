using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace NeuralNetworkTest
{
    class Program
    {
        static Random random = new Random();
        public static double RandomDouble(double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }
        public static bool IsMouseInRect(IntRect r, ref RenderWindow w)
        {
            return Mouse.GetPosition(w).X >= r.Left && Mouse.GetPosition(w).X <= r.Left + r.Width &&
                   Mouse.GetPosition(w).Y >= r.Top && Mouse.GetPosition(w).Y <= r.Top + r.Height;
        }
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

            public static void Normalize(ref Vector2f v, float l)
            {
                float L = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
                v.X /= L;
                v.X *= l;
                v.Y /= L;
                v.Y *= l;
            }

            public class Ball
            {
                public Ball()
                {
                    hitbox = new CircleShape(10);
                    hitbox.Origin = new Vector2f(10f, 10f);
                    hitbox.FillColor = Color.Cyan;
                    hitbox.Position = new Vector2f(640, 600);
                    vel = new Vector2f((float)RandomDouble(-5, 5), -5);
                    Normalize(ref vel, MaxSpeed);
                }
                public float MaxSpeed = 10;
                public CircleShape hitbox;
                public Vector2f vel;
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
            public class Brick
            {
                public Brick(int x, int y, int w, int h)
                {
                    hitbox = new RectangleShape(new Vector2f(w - 2, h - 2));
                    hitbox.Position = new Vector2f(x + 1, y + 1);
                    hitbox.FillColor = Color.Yellow;
                }
                public RectangleShape hitbox;
                public int health = 5;
                public bool onHit(Ball b, FloatRect intersection)
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
            }
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
                        if (Bricks[y, x].health>0)
                        { 
                            FloatRect intersection;
                            if (Bricks[y, x].hitbox.GetGlobalBounds().Intersects(Ball1.hitbox.GetGlobalBounds(), out intersection))
                            {
                                FinalScore++;
                                if (Bricks[y, x].onHit(Ball1, intersection))
                                {
                                    FinalScore+=4;
                                    BricksLeft--;
                                    if (BricksLeft==0)
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
                    Normalize(ref Ball1.vel, Ball1.MaxSpeed);
                    if (Ball1.vel.Y>-2f)
                    {
                        Ball1.vel.Y -= 4f;
                        Normalize(ref Ball1.vel, Ball1.MaxSpeed);
                    }
                    Ball1.hitbox.Position += new Vector2f(0f, -intersection1.Height);
                }

                if (Ball1.hitbox.GetGlobalBounds().Top >= 720)
                {
                    return GameOver();
                }
                return 0;
            }

            public int GameOver()
            {
                int f = FinalScore - FrameCount/180;
                if (bot1.BounceCount == 0)
                    f = 1;
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
                    if (b.health>0)
                        target.Draw(b.hitbox);
                }
            }

            public void Right()
            {
                if (Paddle1.hitbox.GetGlobalBounds().Left+Paddle1.hitbox.GetGlobalBounds().Width <=1265)
                    Paddle1.hitbox.Position += new Vector2f(15, 0);
            }
            public void Left()
            {
                if (Paddle1.hitbox.GetGlobalBounds().Left >= 15)
                    Paddle1.hitbox.Position -= new Vector2f(15, 0);
            }
        }

        public class Bot
        {
            public Bot()
            {

            }
            public Bot(Bot other, ref Game input)
            {
                g = input;
                inputs = input.Bricks;
                set1 = other.set1;
                set2 = other.set2;
                Nodes = other.Nodes;
                FinalScore = other.FinalScore;
                BounceCount = other.BounceCount;
            }
            public Bot Reproduce(Bot other, ref Game input)
            {
                Bot Child = new Bot(other, ref input);
                Child.set1 = new double[28, Child.Nodes];
                for (int i = 0; i < 28; i++)
                {
                    for (int j = 0; j < Child.Nodes; j++)
                    {
                        Child.set1[i, j] = (set1[i, j] + other.set1[i, j]) / 2;
                    }
                }
                Child.set2 = new double[Nodes+2];
                for (int i = 0; i < Child.Nodes + 2; i++)
                {
                    Child.set2[i] = (set2[i] + other.set2[i]) / 2;
                }
                return Child;
            }
            public Game g;
            public Game.Brick[,] inputs;
            public double[,] set1;
            public double[] set2;
            public int Nodes;
            public int FinalScore = 0;
            public int BounceCount = 0;
            public void setup(ref Game input, int nodes = 5)
            {
                g = input;
                inputs = input.Bricks;
                Nodes = nodes;
                set1 = new double[28, nodes];
                for (int i = 0; i<28; i++)
                {
                    for (int j=0; j<nodes; j++)
                    {
                        set1[i, j] = RandomDouble(-1d, 1d);
                    }
                }
                set2 = new double[nodes+2];
                for (int i=0; i<nodes+2; i++)
                {
                    set2[i] = RandomDouble(-1d, 1d);
                }
            }
            public void CreateNew(int nodes = 5)
            {
                FinalScore = 0;
                BounceCount = 0;
                Nodes = nodes;
                set1 = new double[28, nodes];
                for (int i = 0; i < 28; i++)
                {
                    for (int j = 0; j < nodes; j++)
                    {
                        set1[i, j] = RandomDouble(-1d, 1d);
                    }
                }
                set2 = new double[nodes + 2];
                for (int i = 0; i < nodes + 2; i++)
                {
                    set2[i] = RandomDouble(-1d, 1d);
                }
            }
            public int CalculateOutput()
            {
                double[] temp = new double[Nodes];
                for (int n = 0; n < Nodes; n++)
                {
                    temp[n] = 0;
                }
                for (int n=0; n<Nodes; n++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        for (int x = 0; x < 7; x++)
                        {
                            temp[n] += Convert.ToDouble(inputs[y, x].health) * 0.2d * set1[7 * y + x, n];
                        }
                    }
                    temp[n] /= 28;
                }
                double output = 0;
                for (int i=0; i<Nodes; i++)
                {
                    output += temp[i] * set2[i];
                }
                output += set2[Nodes] * Convert.ToDouble(g.Ball1.hitbox.Position.X - g.Paddle1.hitbox.Position.X) / 1280f;
                output += set2[Nodes+1] * Convert.ToDouble(g.Ball1.hitbox.Position.Y - g.Paddle1.hitbox.Position.Y) / 720f;
                output /= (Nodes + 2);
                //System.Console.WriteLine(output);
                if (output >= 0)
                    return 1;
                else
                    return -1;
            }
        }

        static void Main(string[] args)
        {
            var window = new RenderWindow(new VideoMode(1280, 720), "test", Styles.Default);
            window.SetFramerateLimit(60);
            window.Closed += (sender, arg) => { window.Close(); };

            Game game1 = new Game();

            int BotAmount = 100;
            List<Bot> BotList = new List<Bot>();
            for (int i=0; i<BotAmount; i++)
            {
                BotList.Add(new Bot());
                BotList[i].setup(ref game1, 5);
            }
            Bot TempBot = new Bot(BotList[0], ref game1);
            game1.UseBot(ref TempBot);
                
            int CurrBot = 0;

            bool FastForward = false;
            
            while(window.IsOpen)
            {
                if (IsMouseInRect(new IntRect(0, 0, 1280, 720), ref window) && )
                {
                    if (FastForward)
                    {
                        window.SetFramerateLimit(60);
                    }
                    else
                    {
                        window.SetFramerateLimit(10000);
                    }
                    System.Console.WriteLine("triggered");
                    FastForward = !FastForward;
                }
                window.DispatchEvents();
                window.Clear();
                double b = game1.bot1.CalculateOutput();
                if (b == 1)
                    game1.Right();
                else
                    game1.Left();
                int score = game1.RunFrame();
                if (score>0)
                {
                    BotList[CurrBot].FinalScore = score;
                    CurrBot++;
                    if (CurrBot==BotAmount)
                    {
                        System.Console.WriteLine("End of generation\n");
                        BotList.Sort((b1, b2) => b1.FinalScore.CompareTo(b2.FinalScore));
                        BotList.Reverse();
                        CurrBot = 0;
                        BotList.RemoveRange(BotAmount / 2, BotAmount / 2);
                        while (BotList.Count < BotAmount*9/10)
                        {
                            Bot TempBot1 = new Bot();
                            TempBot1 = BotList[random.Next(0, BotAmount / 2 - 1)].Reproduce(BotList[random.Next(0, BotAmount / 2 - 1)], ref game1);
                            BotList.Add(TempBot1);
                        }
                        while (BotList.Count < BotAmount)
                        {
                            BotList.Add(new Bot());
                            BotList[BotList.Count - 1].setup(ref game1, 5);
                        }
                    }
                    game1.Restart();
                    TempBot = new Bot(BotList[CurrBot], ref game1);
                    game1.UseBot(ref TempBot);
                }
                window.Draw(game1);
                window.Display();
            }
        }
    }
}
