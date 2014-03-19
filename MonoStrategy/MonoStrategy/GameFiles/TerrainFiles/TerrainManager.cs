using MonoStrategy.GameFiles.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GameFiles.TerrainFiles
{
    public class TerrainManager
    {
        private Grid3D<float> grid;

        public Grid3D<float> Grid
        {
            get { return grid; }
        }

        public TerrainManager()
        {
            grid = new Grid3D<float>(GameSettings.GridDimensionsX, GameSettings.GridDimensionsY, GameSettings.GridDimensionsZ, GameSettings.GridTileSize);
        }
    }
}
