﻿using System;
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
                System.Console.WriteLine(FastForward);
            }
        }
        static void Main(string[] args)
        {
            var window = new RenderWindow(new VideoMode(1280, 720), "program", Styles.Default);
            var netWindow = new RenderWindow(new VideoMode(340, 890), "net", Styles.Default);
            window.SetKeyRepeatEnabled(false);
            window.SetFramerateLimit(60);
            window.Closed += (sender, arg) => { window.Close(); };
            window.KeyPressed += OnKeyPressed;

            netWindow.Closed += (sender, arg) => { netWindow.Close(); };

            Game game1 = new Game();

            int BotAmount = 100;
            List<Bot> BotList = new List<Bot>();
            for (int i=0; i<BotAmount; i++)
            {
                BotList.Add(new Bot());
                BotList[i] = new Bot(ref game1.Bricks, ref game1.Ball1);
            }
            Bot TempBot = new Bot();
            TempBot = BotList[0];
            game1.UseBot(ref TempBot);
                
            int CurrBot = 0;
            
            while(window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();
                double b = game1.bot1.CalculateOutput();
                if (b == 1)
                    game1.Right();
                else
                    game1.Left();
                int score = game1.RunFrame();
                if (score>-1)
                {
                    BotList[CurrBot].FinalScore = score;
                    CurrBot++;
                    if (CurrBot==BotAmount)
                    {
                        System.Console.WriteLine("End of generation");
                        BotList.Sort((b1, b2) => b1.FinalScore.CompareTo(b2.FinalScore));
                        BotList.Reverse();
                        CurrBot = 0;
                        for (int i=BotList.Count-1; i>=5; i--)
                        {
                            if (BotList[i].FinalScore==0)
                            {
                                BotList.RemoveAt(i);
                            }
                        }
                        if (BotList.Count>BotAmount/2)
                        {
                            BotList.RemoveRange(BotAmount / 2, BotList.Count - BotAmount / 2);
                        }
                        System.Console.WriteLine("{0} Bots lived;", BotList.Count);
                        int tmp1 = 0;
                        while (BotList.Count < BotAmount*9/10)
                        {
                            tmp1++;
                            Bot TempBot1 = new Bot();
                            TempBot1 = BotList[Utility.random.Next(0, BotList.Count-1)].Reproduce(BotList[Utility.random.Next(0, BotList.Count-1)]);
                            BotList.Add(TempBot1);
                        }
                        System.Console.WriteLine("{0} Bots born;", tmp1);
                        tmp1 = 0;
                        while (BotList.Count < BotAmount)
                        {
                            tmp1++;
                            BotList.Add(new Bot(ref game1.Bricks, ref game1.Ball1, 5));
                        }
                        System.Console.WriteLine("{0} Bots appeared;\n", tmp1);
                    }
                    game1.Restart();
                    TempBot = new Bot(BotList[CurrBot]);
                    //TempBot = BotList[CurrBot];
                    game1.UseBot(ref TempBot);
                }

                ///RYSOWANIE SIECI
                if (netWindow.IsOpen)
                {
                    netWindow.DispatchEvents();
                    netWindow.Clear();
                    BotList[CurrBot].DrawNet(ref netWindow);
                    netWindow.Display();

                }
                window.Draw(game1);
                window.Display();
            }
        }
    }
}
