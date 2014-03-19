using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy
{
    class VoxelMap
    {
        //DEPCRICATED FÖR STUNDEN
        private List<Dictionary<Vector3, int>> voxelList; //Second value of dict is block properties (type of block etc?)
        private int cursor; //Points to which dictionary to add new voxels

        public Dictionary<Vector3, int>[] VoxelsChunks
        {
            get { return voxelList.ToArray(); }
        }

        public VoxelMap()
        {
            voxelList = new List<Dictionary<Vector3, int>>();
            voxelList.Add(new Dictionary<Vector3, int>());
            cursor = 0;
        }

        public void Add(Vector3 pos, int type)
        {
           // if(VoxelsChunks[cursor].Count >= GameSettings.vBufferSize)
            {
                //Allocate new dictionary 
                voxelList.Add(new Dictionary<Vector3, int>());
                cursor++;
            }

            VoxelsChunks[cursor].Add(pos, type);
        }

        public Boolean Contains(float x, float y, float z)
        {
            return VoxelsChunks[cursor].ContainsKey(new Vector3(x, y, z));
        }

    }
}
