using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GameFiles
{
    public static class GameSettings
    {
        public const int GridDimensionsX = 150;
        public const int GridDimensionsY = 150;
        public const int GridDimensionsZ = 150;
        public const float GridTileSize = 100.0f;

        public static Vector2 Resolution = new Vector2(1280, 720);

        public static String ConnectIP = "83.254.25.3";
        public static int Port = 1234;
        public static bool IsServer = true;
        public static int MessageStepDelay = 2;

        public static String PlayerName = "NAME";

        public static bool DEBUG = false;


        public static int GeneratorAdd = 3;
        public static double GeneratorSub = 15.0;


    }
}
