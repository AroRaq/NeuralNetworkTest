using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Functions;

namespace NeuralNetworkTest
{
    class Program
    {
        static bool FastForward = false;
        static bool OnlyConsole = false;
        static Color NetWindowColor = new Color(127, 127, 127);
        
        static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            
            RenderWindow window = (RenderWindow)sender;
            if (e.Code == Keyboard.Key.Escape)
                window.Close();
            if (e.Code == Keyboard.Key.Q)
            {
                if (FastForward)
                    window.SetFramerateLimit(60);
                else
                    window.SetFramerateLimit(100000);
                FastForward = !FastForward;
            }
            if (e.Code == Keyboard.Key.W)
            {
                OnlyConsole = !OnlyConsole;
                window.Clear(Color.Green);
                window.Display();
            }
        }
        static void Main(string[] args)
        {
            var window = new RenderWindow(new VideoMode((uint)Properties.WindowSize.X, (uint)Properties.WindowSize.Y), "program", Styles.Default);
            var netWindow = new RenderWindow(new VideoMode((uint)Properties.NetWindowSize.X, (uint)Properties.NetWindowSize.Y), "net", Styles.Default);
            window.SetKeyRepeatEnabled(false);
            window.SetFramerateLimit(60);
            window.Closed += (sender, arg) => { window.Close(); };
            window.KeyPressed += OnKeyPressed;

            netWindow.Closed += (sender, arg) => { netWindow.Close(); };

            Game game1 = new Game();

            VertexArray Evolution = new VertexArray(PrimitiveType.Points, 0);
            
            int Generation = 1;
            List<Bot> BotList = new List<Bot>();
            for (int i=0; i<Properties.BotAmount; i++)
            {
                BotList.Add(new Bot(ref game1.Bricks, ref game1.Ball1, ref game1.Paddle1, Properties.Nodes));
            }
            Bot TempBot = new Bot(BotList[0]);
            game1.UseBot(ref TempBot);
                
            int CurrBot = 0;
            
            while(window.IsOpen)
            {
                double b = game1.bot1.CalculateOutput();
                if (b == 1)
                    game1.Paddle1.Right();
                else
                    game1.Paddle1.Left();
                int score = game1.RunFrame();
                if (score>-1)
                {
                    BotList[CurrBot].FinalScore = score;
                    CurrBot++;
                    if (CurrBot==Properties.BotAmount)
                    {
                        System.Console.WriteLine("End of generation {0}.", Generation);
                        Generation++;
                        BotList.Sort((b1, b2) => b1.FinalScore.CompareTo(b2.FinalScore));
                        BotList.Reverse();
                        System.Console.WriteLine("Best Score:   {0} ({1});", 
                            BotList[0].FinalScore, 
                            BotList.FindLastIndex( delegate (Bot b1) { return b1.FinalScore == BotList[0].FinalScore; }) + 1
                        );
                        for (int i=0; i<Properties.NetWindowSize.X; i++)
                        {
                            Evolution.Append(new Vertex(new Vector2f(i, Generation), Utility.Spectral_color(BotList[i].FinalScore + 400)));
                        }
                        //BotList.Shuffle((int)(BotAmount * 0.4d), (int)(BotAmount * 0.8d));
                        CurrBot = 0;
                        for (int i=BotList.Count-1; i>=5; i--)
                        {
                            if (BotList[i].FinalScore==0)
                            {
                                BotList.RemoveAt(i);
                            }
                        }
                        if (BotList.Count > Properties.BotAmount / 2)
                            BotList.RemoveRange(Properties.BotAmount / 2, BotList.Count - Properties.BotAmount / 2);
                        System.Console.WriteLine("Bots lived:   {0};", BotList.Count);
                        int BestAmount = BotList.FindLastIndex(delegate (Bot b1) { return b1.FinalScore == BotList[0].FinalScore; });
                        for (int i=0; i<Properties.BotAmount / 10 - BestAmount; i++)
                        {
                            Bot t = new Bot(BotList[0]);
                            t.Mutate(0.01);
                            BotList.Add(t);
                        }
                        int tmp1 = 0;
                        while (BotList.Count < Properties.BotAmount * 3 / 4)
                        {
                            tmp1++;
                            BotList.Add(new Bot(BotList[Utility.random.Next(0, BotList.Count - 1)].Reproduce(BotList[Utility.random.Next(0, BotList.Count - 1)])));
                        }
                        System.Console.WriteLine("Bots born:    {0}", tmp1);
                        tmp1 = 0;
                        while (BotList.Count < Properties.BotAmount)
                        {
                            tmp1++;
                            BotList.Add(new Bot(ref game1.Bricks, ref game1.Ball1, ref game1.Paddle1, Properties.Nodes));
                        }
                        System.Console.WriteLine("Bots spawned: {0};\n", tmp1);
                        
                        BotList.Shuffle();
                    }
                    game1.Restart();
                    TempBot = new Bot(BotList[CurrBot]);
                    game1.UseBot(ref TempBot);
                }
                
                window.DispatchEvents();
                if (!OnlyConsole)
                {
                    window.Clear();
                    window.Draw(game1);
                    window.Display();
                    ///RYSOWANIE SIECI
                    if (netWindow.IsOpen)
                    {
                        netWindow.DispatchEvents();
                        netWindow.Clear(NetWindowColor);
                        BotList[CurrBot].DrawNet(ref netWindow);
                        netWindow.Draw(Evolution);
                        netWindow.Display();
                    }
                }
            }
        }
    }
}
