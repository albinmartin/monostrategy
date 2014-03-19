using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoStrategy.GameFiles;
using MonoStrategy.GameFiles.TerrainFiles;
using MonoStrategy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.VoxelStuff
{
    class VoxelManager
    {
        private Dictionary<Vector3, TerrainTypes> voxelMap;  
        private Mesh mesh;
        private List<Vector3> updates; //Replace vector3 with block? using ints and block type
        private int VertexCount;
        private float cubeSize = 1.0f;
        private int chunk = 120;
        private enum Faces { LEFT, RIGHT, BACK, FRONT, BOTTOM, TOP }
        public Boolean VertexAdded;
        public Boolean processChunks;
        
        public VoxelManager()
        {
            voxelMap = new Dictionary<Vector3, TerrainTypes>();
            mesh = new Mesh();
            updates = new List<Vector3>();
            processChunks = false;
        }

        public VertexPositionNormalTexture[] GetMesh()
        {
            return mesh.GetMesh();
        }

        //Add block to mesh (and world)
        public void Add(Vector3 pos, TerrainTypes type)
        {
            voxelMap.Add(pos, type);
            updates.Add(pos);
        }

        //Removes block from mesh (and world)
        public void Remove(Vector3 pos)
        {
            //Remove from volume data
            if(voxelMap.ContainsKey(pos))
                voxelMap.Remove(pos);
            
            //Remove from update queue
            updates.Remove(pos);

            //Remove faces from mesh 
            //TODO: need to add faces for adjacent blocks
            foreach (Faces face in (Faces[])Enum.GetValues(typeof(Faces)))
            {
                int key = (int)(pos.X * 100000 + pos.Y * 1000 + pos.Z * 10 + (int)face);
                mesh.Remove(key);
            }
        }

        public Boolean IsSolid(float x, float y, float z)
        {
            return voxelMap.ContainsKey(new Vector3(x, y, z));
        }

        public void Update()
        {
            VertexAdded = false;

            if (processChunks)
            {
                //Update mesh chunk-wise
                if (updates.Count > chunk)
                {
                    UpdateMesh(updates.Take(chunk).ToArray());
                    updates = updates.Skip(chunk).ToList();
                    VertexAdded = true;
                }
                else if (updates.Count > 0)
                {
                    UpdateMesh(updates.ToArray());
                    updates = new List<Vector3>();
                    VertexAdded = true;
                }
            }
            else
            {
                //Create the whole mesh in one go
                UpdateMesh(updates.ToArray());
                updates = new List<Vector3>();
                VertexAdded = true;
            }
        }

        private void UpdateMesh(Vector3[] Blocks)
        {
            for (int i = 0; i < Blocks.Length; i++)
            {
                Vector3 position = Blocks[i];
                // Set visible vertices TODO: FIX last side (eg. x < worldXSize)
                // X
                if (position.X > 0)
                {
                    if (!IsSolid(position.X - 1, position.Y, position.Z))
                    {
                        setFace(position, Faces.LEFT);
                    }
                }
                else
                {
                    setFace(position, Faces.LEFT);
                }
                if (position.X < GameSettings.GridDimensionsX - 1)
                {
                    if (!IsSolid(position.X + 1, position.Y, position.Z))
                    {
                        setFace(position, Faces.RIGHT);
                    }
                }
                else
                {
                    setFace(position, Faces.RIGHT);
                }


                // Y
                if (position.Y > 0)
                {
                    if (!IsSolid(position.X, position.Y - 1, position.Z))
                    {
                        setFace(position, Faces.BOTTOM);
                    }
                }
                else
                {
                    setFace(position, Faces.BOTTOM);
                }
                if (position.Y < GameSettings.GridDimensionsY - 1)
                {
                    if (!IsSolid(position.X, position.Y + 1, position.Z))
                    {
                        setFace(position, Faces.TOP);
                    }
                }
                else
                {
                    setFace(position, Faces.TOP);
                }

                // Z
                if (position.Z > 0)
                {
                    if (!IsSolid(position.X, position.Y, position.Z - 1))
                    {
                        setFace(position, Faces.FRONT);

                    }
                }
                else
                {
                    setFace(position, Faces.FRONT);

                }
                if (position.Z < GameSettings.GridDimensionsZ - 1)
                {
                    if (!IsSolid(position.X, position.Y, position.Z + 1))
                    {
                        setFace(position, Faces.BACK);
                    }
                }
                else
                {
                    setFace(position, Faces.BACK);
                }
            }
        }

        //Update mesh with face-vertices
        private void setFace(Vector3 pos, Faces side)
        {
            //Get vertices for side
            VertexPositionNormalTexture[] vert = getSide(side);

            //Transform to correct position
            for (int i = 0; i < vert.Length; i++)
            {
                Vector3 vPos = vert[i].Position;
                float blockType = (float)voxelMap[pos];
                vert[i].Position = Vector3.Transform(vert[i].Position, Matrix.CreateTranslation(pos * cubeSize));
                vert[i].TextureCoordinate = new Vector2(blockType,blockType);
                VertexCount++;
            }

            //Add to draw list
            int key = (int)(pos.X * 10000000 + pos.Y * 10000 + pos.Z * 10 + (int)side);
            mesh.AddPoly(key, vert.Take(3).ToArray());
            mesh.AddPoly(key, vert.Skip(3).ToArray());
        }

        // Get vertices for a specified side of a cube
        private VertexPositionNormalTexture[] getSide(Faces side)
        {
            VertexPositionNormalTexture[] vert = new VertexPositionNormalTexture[6];
            switch (side)
            {
                case Faces.LEFT:
                    vert = CubeVBO.Left();
                    break;
                case Faces.RIGHT:
                    vert = CubeVBO.Right();
                    break;
                case Faces.BOTTOM:
                    vert = CubeVBO.Bottom();
                    break;
                case Faces.TOP:
                    vert = CubeVBO.Top();
                    break;
                case Faces.FRONT:
                    vert = CubeVBO.Front();
                    break;
                case Faces.BACK:
                    vert = CubeVBO.Back();
                    break;
                default:
                    break;
            }
            return vert;
        }
    }
}
