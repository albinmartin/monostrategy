using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy
{
    class Mesh
    {
        private Dictionary<int, VertexPositionNormalTexture[]> mesh;

        public Mesh()
        {
            mesh = new Dictionary<int, VertexPositionNormalTexture[]>();
        }

        public VertexPositionNormalTexture[] GetMesh()
        {
            List<VertexPositionNormalTexture> rMesh = new List<VertexPositionNormalTexture>();
            foreach (var vList in mesh.Values)
            {
                rMesh.AddRange(vList);
            }
            return rMesh.ToArray();
        }

        //Could use some optimization (allocate array from start and use getters for poly1 and 2)
        public void AddPoly(int key, VertexPositionNormalTexture[] polygon)
        {
            if(mesh.ContainsKey(key))
            {
                //Add the polygon to existing face
                VertexPositionNormalTexture[] polys = new VertexPositionNormalTexture[6];
                for (int i = 0; i < 6; i++)
                {
                    if (i < 3)
                    {
                        polys[i] = mesh[key][i];
                    }
                    else
                    {
                        polys[i] = polygon[i-3];
                    }
                }
                mesh[key] = polys;
            }
            else
            {
                mesh.Add(key, polygon);
            }
        }

        public void Remove(int key)
        {
            if (mesh.ContainsKey(key))
            {
                mesh.Remove(key);
            }
        }
    }
}
