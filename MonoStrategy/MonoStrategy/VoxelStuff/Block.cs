using MonoStrategy.GameFiles.TerrainFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.VoxelStuff
{
    class Block
    {
        Tuple<int, int, int, TerrainTypes> block;

        #region Getters & Setters
        public int X
        {
            get { return block.Item1; }
        }
        public int Y
        {
            get { return block.Item2; }
        }
        public int Z
        {
            get { return block.Item3; }
        }
        public TerrainTypes TerrainType
        {
            get { return block.Item4; }
        }
        #endregion

        public Block(int x, int y, int z, TerrainTypes terrainType)
        {
            block = new Tuple<int, int, int, TerrainTypes>(x, y, z, terrainType);
        }

    }
}
