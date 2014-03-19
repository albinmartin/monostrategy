using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoStrategy.GameFiles.Utilities
{
    public class Grid3D<T> 
    {
        private T[, ,] tiles;
        protected float tileSize;
        private int gridSizeX;
        private int gridSizeY;
        private int gridSizeZ;


        public T[, ,] Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }

        public Grid3D(int gridSizeX, int gridSizeY, int gridSizeZ, float tileSize)
        {
            tiles = new T[gridSizeX, gridSizeY, gridSizeZ];
            this.tileSize = tileSize;
            this.gridSizeX = gridSizeX;
            this.gridSizeY = gridSizeY;
            this.gridSizeZ = gridSizeZ;

        }
        
        protected int toGridCoord(float fp)
        {
            return (int)((fp + tileSize / 2) / tileSize);
        }

        protected Vector3 toGridCoord(Vector3 fp)
        {
            return new Vector3(toGridCoord(fp.X), toGridCoord(fp.Y), toGridCoord(fp.Z));
        }

        private bool inGrid(int x, int y, int z)
        {
            return (x < 0 || y < 0 || z < 0 || x >= gridSizeX || y >= gridSizeY || z >= gridSizeZ);

        }

        public bool IsPointInGrid(Vector3 point)
        {
            int x = toGridCoord(point.X);
            int y = toGridCoord(point.Y);
            int z = toGridCoord(point.Z);
            return inGrid(x, y, z);
        }

        public T SelectCube(Vector3 position)
        {
            int x = toGridCoord(position.X);
            int y = toGridCoord(position.Y);
            int z = toGridCoord(position.Z);
            
            return Tiles[x, y, z];
        }

        public T SelectCube(int x, int y, int z)
        {
            return Tiles[x, y, z];
        }

        public void SetCube(int x, int y, int z, T val)
        {
            Tiles[x, y, z] = val;
        }

        public void SetCube(Vector3 position, T val)
        {
            int x = toGridCoord(position.X);
            int y = toGridCoord(position.Y);
            int z = toGridCoord(position.Z);

            Tiles[x, y, z] = val;
        }

        public List<T> SelectCubesInLine(Vector3 l1, Vector3 l2, int width)
        {
            List<Vector3> coordinates = new List<Vector3>();
            return SelectCubesInLine(l1, l2, width, ref coordinates);
        }

        public List<T> SelectCubesInLine(Vector3 l1, Vector3 l2, int width,  ref List<Vector3> coordinates)
        {
            List<T> result = new List<T>();
            coordinates = new List<Vector3>(); //debug
            int x1 = toGridCoord(l1.X);
            int y1 = toGridCoord(l1.Y);
            int z1 = toGridCoord(l1.Z);

            int x2 = toGridCoord(l2.X);
            int y2 = toGridCoord(l2.Y);
            int z2 = toGridCoord(l2.Z);

            int dx = x2 - x1;
            int dy = y2 - y1;
            int dz = z2 - z1;

            int ax = Math.Abs(dx) * 2;
            int ay = Math.Abs(dy) * 2;
            int az = Math.Abs(dz) * 2;

            int sx = Math.Sign(dx);
            int sy = Math.Sign(dy);
            int sz = Math.Sign(dz);

            int x = x1;
            int y = y1;
            int z = z1;

            if (ax >= Math.Max(ay, az)) // x dominant
            {
                float yd = ay - ax / 2;
                float zd = az - ax / 2;

                while (true)
                {
                    for (int zw = -width; zw <= width; zw++)
                    {
                        for (int yw = -width; yw <= width; yw++)
                        {
                            if (inGrid(x, y + yw, z + zw) && Tiles[x, y + yw, z + zw] != null)
                                result.Add(Tiles[x, y + yw , z + zw]);
                            coordinates.Add(new Vector3(x, y + yw, z + zw));
                        }
                    }

                    if (x == x2)
                        break;
                    if (yd >= 0)
                    {
                        y = y + sy;
                        yd = yd - ax;
                    }
                    if (zd >= 0)
                    {
                        z = z + sz;
                        zd = zd - ax;
                    }
                    x = x + sx;
                    yd = yd + ay;
                    zd = zd + az;
                }
            }
            else if (ay >= Math.Max(ax, az)) //y dominant
            {
                float xd = ax - ay / 2;
                float zd = az - ay / 2;

                while (true)
                {
                    for (int zw = -width; zw <= width; zw++)
                    {
                        for (int xw = -width; xw <= width; xw++)
                        {
                            if (inGrid(x + xw, y, z + zw) && Tiles[x + xw, y, z + zw] != null)
                                result.Add(Tiles[x + xw, y, z + zw]);
                            coordinates.Add(new Vector3(x + xw, y, z + zw));
                        }
                    }
                    if (y == y2)
                        break;
                    if (xd >= 0)
                    {
                        x = x + sx;
                        xd = xd - ay;
                    }
                    if (zd >= 0)
                    {
                        z = z + sz;
                        zd = zd - ax;
                    }
                    y = y + sy;
                    xd = xd + ax;
                    zd = zd + az;
                }
            }
            else if (az >= Math.Max(ax, ay)) //z dominant
            {
                float xd = ax - az / 2;
                float yd = ay - az / 2;

                while (true)
                {
                    for (int yw = -width; yw <= width; yw++)
                    {
                        for (int xw = -width; xw <= width; xw++)
                        {
                            if (inGrid(x + xw, y + yw, z) && Tiles[x + xw, y + yw, z] != null)
                                result.Add(Tiles[x + xw, y + yw, z]);
                            coordinates.Add(new Vector3(x + xw, y + yw, z));
                        }
                    }
                    if (z == z2)
                        break;
                    if (xd >= 0)
                    {
                        x = x + sx;
                        xd = xd - az; 
                    }
                    if (yd >= 0)
                    {
                        y = y + sy;
                        yd = yd - az;
                    }

                    z = z + sz; 
                    xd = xd + ax;
                    yd = yd + ay;
                }
            }
            return result;
        }

        public List<T> SelectCubesInBox(Vector3 position, Vector3 bounds)
        {
            List<T> result = new List<T>();
            for (int x = toGridCoord(position.X) - toGridCoord(bounds.X / 2); x <= toGridCoord(position.X) + toGridCoord(bounds.X / 2); x++)
            {
                for (int y = toGridCoord(position.Y) - toGridCoord(bounds.Y / 2); y <= toGridCoord(position.Y) + toGridCoord(bounds.Y / 2); y++)
                {
                    for (int z = toGridCoord(position.Z) - toGridCoord(bounds.Z / 2); z <= toGridCoord(position.Z) + toGridCoord(bounds.Z / 2); z++)
                    {
                        if (inGrid(x, y, z) && Tiles[x, y, z] != null)
                            result.Add(Tiles[x, y, z]);
                    }
                }
            }
            return result;
        }

        public List<T> SelectCubesOnSphere(Vector3 position, float radius, int longs, int lats, ref List<Vector3> coordinates)
        {
            List<T> result = new List<T>();
            coordinates = new List<Vector3>();
            Vector3 gPos = toGridCoord(position);
            int i, j;
            for (i = 0; i <= lats; i++)
            {
                float lat0 = (float)Math.PI * (-0.5f + (float)(i - 1) / lats);
                float z0 = (float)Math.Sin(lat0) * radius;
                float zr0 = (float)Math.Cos(lat0) * radius;

                float lat1 = (float)Math.PI * (-0.5f + (float)i / lats);
                float z1 = (float)Math.Sin(lat1) * radius;
                float zr1 = (float)Math.Cos(lat1) * radius;

                for (j = 0; j <= longs; j++)
                {
                    float lng = 2 * (float)Math.PI * (float)(j - 1) / longs;
                    float x = (float)Math.Cos(lng);
                    float y = (float)Math.Sin(lng);
                    coordinates.Add(this.toGridCoord(new Vector3(x * zr0, y * zr0, z0)) + gPos);
                    coordinates.Add(this.toGridCoord(new Vector3(x * zr1, y * zr1, z1)) + gPos);
                    //plot(x * zr0, y * zr0, z0);
                    //plot(x * zr1, y * zr1, z1);
                }
            }

            return result;
        }

        public List<T> SelectCubesInBox(Vector3 position, Vector3 bounds, ref List<Vector3> coordinates)
        {
            List<T> result = new List<T>();
            coordinates = new List<Vector3>();
            for (int x = toGridCoord(position.X) - toGridCoord(bounds.X / 2); x <= toGridCoord(position.X) + toGridCoord(bounds.X / 2); x++)
            {
                for (int y = toGridCoord(position.Y) - toGridCoord(bounds.Y / 2); y <= toGridCoord(position.Y) + toGridCoord(bounds.Y / 2); y++)
                {
                    for (int z = toGridCoord(position.Z) - toGridCoord(bounds.Z / 2); z <= toGridCoord(position.Z) + toGridCoord(bounds.Z / 2); z++)
                    {
                        if (inGrid(x, y, z) && Tiles[x, y, z] != null)
                            result.Add(Tiles[x, y, z]);
                        coordinates.Add(new Vector3(x, y, z));
                    }
                }
            }
            return result;
        }

    }

}
