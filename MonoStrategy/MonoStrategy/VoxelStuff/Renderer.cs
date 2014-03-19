using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoStrategy.GameFiles;
using MonoStrategy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.VoxelStuff
{
    class Renderer
    {
        private GraphicsDevice graphics;
        private Effect effect;
        private DynamicVertexBuffer vertexBuffer;
        private int vBufferSize;
        private int numVertices = 0;
        private Vector3 lightDirection;

        public Renderer(GraphicsDevice graphics) 
        {
            this.graphics = graphics;
            CubeVBO.Initialize(graphics);

            //Init effect
            effect = GameEngine.GetInstance().ResourceManager.GetEffect("regulareffect"); ;
            effect.CurrentTechnique = effect.Techniques["Colored"];
            effect.Parameters["Color"].SetValue(new Vector4(1,1,1,1));
            effect.Parameters["LightDir"].SetValue(new Vector3(0, -1, 0));

            //Init buffer
            vBufferSize = GameSettings.GridDimensionsX*GameSettings.GridDimensionsZ*GameSettings.GridDimensionsY;
            vertexBuffer = new DynamicVertexBuffer(graphics, VertexPositionNormalTexture.VertexDeclaration, vBufferSize, BufferUsage.WriteOnly);
        }

        public void SetVertexBuffer(VertexPositionNormalTexture[] vertices)
        {
            // Avaktiverar vertexbuffern
            //graphics.SetVertexBuffer(null);
            numVertices = vertices.Length;
            if (numVertices > 0)
            {
                vertexBuffer.SetData(vertices);
                graphics.SetVertexBuffer(vertexBuffer);
            }
        }

        public void Draw()
        {
            if (numVertices > 0)
            {
                Camera camera = GameEngine.GetInstance().Camera;
                graphics.DepthStencilState = DepthStencilState.Default;
                graphics.RasterizerState = RasterizerState.CullCounterClockwise;
                graphics.BlendState = BlendState.Opaque;
                effect.Parameters["World"].SetValue(Matrix.Identity);
                effect.Parameters["View"].SetValue(camera.GetTransform());
                effect.Parameters["Projection"].SetValue(camera.Perspective);
                effect.Parameters["Eye"].SetValue(camera.Position);
                effect.CurrentTechnique.Passes[0].Apply();

                graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, numVertices / 3);
            }
            /* Ritar ut oberoende av storlek på vertexbuffer
             * -- TA INTE BORT --
            int numVertices = vertices.Length;
            VertexPositionNormalTexture[] vert, restVert = vertices;
            int i = 0;
            while (numVertices > 0) // Måste köras i multipler av 3
            {
                i++;
                if (numVertices >= vBufferSize)
                {
                    int marginal = vBufferSize % 3;
                    vert = restVert.Take(vBufferSize - marginal).ToArray();
                    restVert = restVert.Skip(vBufferSize - marginal).ToArray();
                }
                else
                {
                    vert = restVert.Take(numVertices).ToArray();
                    restVert = restVert.Skip(numVertices).ToArray();
                }

                vertexBuffer.SetData(vert);
                graphics.Indices = indexBuffer;
                graphics.SetVertexBuffer(vertexBuffer);
                graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, vert.Length / 3);

                //Deactivate vertex buffer
                graphics.SetVertexBuffer(null);
                numVertices -= vBufferSize;
            }
            */
        }
    }
}
