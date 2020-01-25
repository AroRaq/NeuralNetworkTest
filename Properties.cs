using SFML.System;

namespace NeuralNetworkTest
{
    static class Properties
    {
        public static Vector2i BrickTable = new Vector2i(8, 2);
        public static int BrickAmount = BrickTable.X * BrickTable.Y;
        public static Vector2i WindowSize = new Vector2i(1280, 720);
        public static Vector2i NetWindowSize = new Vector2i(320, 800);
        public static int Nodes = 1;
        public static int BotAmount = 100;
    }
}
