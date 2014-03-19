using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoStrategy.Utility;

namespace MonoStrategy.Utility
{

    class CubeVBO
    {
        public VertexPositionNormalTexture[] vertices;

        public int[] indices;

        private static CubeVBO instance = null;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        private GraphicsDevice device;

        public static CubeVBO Instance
        {
            get { return instance; }
        }

        private CubeVBO(GraphicsDevice device)
        {
            this.device = device;

            BuildVBO();
        }

        public static CubeVBO Initialize(GraphicsDevice device)
        {
            instance = new CubeVBO(device);

            return instance;
        }

        #region Vertex Sides
        public static VertexPositionNormalTexture[] Front()
        {
            VertexPositionNormalTexture[] side = new VertexPositionNormalTexture[6];
            Array.Copy(instance.vertices, 0, side, 0, 6);
            return side;
        }

        public static VertexPositionNormalTexture[] Right()
        {
            VertexPositionNormalTexture[] side = new VertexPositionNormalTexture[6];
            Array.Copy(instance.vertices, 6, side, 0, 6);
            return side;
        }

        public static VertexPositionNormalTexture[] Top()
        {
            VertexPositionNormalTexture[] side = new VertexPositionNormalTexture[6];
            Array.Copy(instance.vertices, 12, side, 0, 6);
            return side;
        }

        public static VertexPositionNormalTexture[] Bottom()
        {
            VertexPositionNormalTexture[] side = new VertexPositionNormalTexture[6];
            Array.Copy(instance.vertices, 18, side, 0, 6);
            return side;
        }

        public static VertexPositionNormalTexture[] Left()
        {
            VertexPositionNormalTexture[] side = new VertexPositionNormalTexture[6];
            Array.Copy(instance.vertices, 24, side, 0, 6);
            return side;
        }

        public static VertexPositionNormalTexture[] Back()
        {
            VertexPositionNormalTexture[] side = new VertexPositionNormalTexture[6];
            Array.Copy(instance.vertices, 30, side, 0, 6);
            return side;
        }


        #endregion

        private void BuildVBO()
        {
            vertices = new VertexPositionNormalTexture[36];
            Vector3[] positions = new Vector3[8];

            positions[0] = new Vector3(-1f / 2f, -1f / 2f, -1f / 2f);
            positions[1] = new Vector3(-1f / 2f, -1f / 2f, 1f / 2f);
            positions[2] = new Vector3(-1f / 2f, 1f / 2f, -1f / 2f);
            positions[3] = new Vector3(-1f / 2f, 1f / 2f, 1f / 2f);
            positions[4] = new Vector3(1f / 2f, -1f / 2f, -1f / 2f);
            positions[5] = new Vector3(1f / 2f, -1f / 2f, 1f / 2f);
            positions[6] = new Vector3(1f / 2f, 1f / 2f, -1f / 2f);
            positions[7] = new Vector3(1f / 2f, 1f / 2f, 1f / 2f);


            indices = new int[36];
            for (int i = 0; i < 36; i++)
                indices[i] = i;

            //front face
            vertices[0].Position = positions[6];
            vertices[1].Position = positions[2];
            vertices[2].Position = positions[0];
            vertices[3].Position = positions[0];
            vertices[4].Position = positions[4];
            vertices[5].Position = positions[6];

            vertices[0].TextureCoordinate = new Vector2(1, 1);
            vertices[1].TextureCoordinate = new Vector2(0, 1);
            vertices[2].TextureCoordinate = new Vector2(0, 0);
            vertices[3].TextureCoordinate = new Vector2(0, 0);
            vertices[4].TextureCoordinate = new Vector2(1, 0);
            vertices[5].TextureCoordinate = new Vector2(1, 1);

            for (int i = 0; i < 6; i++)
                vertices[i].Normal = new Vector3(0, 0, -1);


            //right face
            vertices[6].Position = positions[6];
            vertices[7].Position = positions[4];
            vertices[8].Position = positions[5];
            vertices[9].Position = positions[5];
            vertices[10].Position = positions[7];
            vertices[11].Position = positions[6];

            vertices[6].TextureCoordinate = new Vector2(1, 1);
            vertices[7].TextureCoordinate = new Vector2(0, 1);
            vertices[8].TextureCoordinate = new Vector2(0, 0);
            vertices[9].TextureCoordinate = new Vector2(0, 0);
            vertices[19].TextureCoordinate = new Vector2(1, 0);
            vertices[11].TextureCoordinate = new Vector2(1, 1);

            for (int i = 6; i < 12; i++)
                vertices[i].Normal = new Vector3(1, 0, 0);

            //top face
            vertices[12].Position = positions[6];
            vertices[13].Position = positions[7];
            vertices[14].Position = positions[3];
            vertices[15].Position = positions[3];
            vertices[16].Position = positions[2];
            vertices[17].Position = positions[6];

            for (int i = 12; i < 18; i++)
                vertices[i].Normal = new Vector3(0, 1, 0);

            vertices[12].TextureCoordinate = new Vector2(1, 1);
            vertices[13].TextureCoordinate = new Vector2(0, 1);
            vertices[14].TextureCoordinate = new Vector2(0, 0);
            vertices[15].TextureCoordinate = new Vector2(0, 0);
            vertices[16].TextureCoordinate = new Vector2(1, 0);
            vertices[17].TextureCoordinate = new Vector2(1, 1);

            //bottom face
            vertices[18].Position = positions[1];
            vertices[19].Position = positions[5];
            vertices[20].Position = positions[4];
            vertices[21].Position = positions[4];
            vertices[22].Position = positions[0];
            vertices[23].Position = positions[1];

            for (int i = 18; i < 24; i++)
                vertices[i].Normal = new Vector3(0, -1, 0);

            vertices[18].TextureCoordinate = new Vector2(1, 1);
            vertices[19].TextureCoordinate = new Vector2(0, 1);
            vertices[20].TextureCoordinate = new Vector2(0, 0);
            vertices[21].TextureCoordinate = new Vector2(0, 0);
            vertices[22].TextureCoordinate = new Vector2(1, 0);
            vertices[23].TextureCoordinate = new Vector2(1, 1);

            //left face
            vertices[24].Position = positions[1];
            vertices[25].Position = positions[0];
            vertices[26].Position = positions[2];
            vertices[27].Position = positions[2];
            vertices[28].Position = positions[3];
            vertices[29].Position = positions[1];

            for (int i = 24; i < 30; i++)
                vertices[i].Normal = new Vector3(-1, 0, 0);

            vertices[24].TextureCoordinate = new Vector2(1, 1);
            vertices[25].TextureCoordinate = new Vector2(0, 1);
            vertices[26].TextureCoordinate = new Vector2(0, 0);
            vertices[27].TextureCoordinate = new Vector2(0, 0);
            vertices[28].TextureCoordinate = new Vector2(1, 0);
            vertices[29].TextureCoordinate = new Vector2(1, 1);

            //back face
            vertices[30].Position = positions[1];
            vertices[31].Position = positions[3];
            vertices[32].Position = positions[7];
            vertices[33].Position = positions[7];
            vertices[34].Position = positions[5];
            vertices[35].Position = positions[1];

            for (int i = 30; i < 36; i++)
                vertices[i].Normal = new Vector3(0, 0, 1);

            vertices[30].TextureCoordinate = new Vector2(1, 1);
            vertices[31].TextureCoordinate = new Vector2(0, 1);
            vertices[32].TextureCoordinate = new Vector2(0, 0);
            vertices[33].TextureCoordinate = new Vector2(0, 0);
            vertices[34].TextureCoordinate = new Vector2(1, 0);
            vertices[35].TextureCoordinate = new Vector2(1, 1);

            //Init buffers
            vertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);

            indexBuffer = new IndexBuffer(device, typeof(int), indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);
        }



        #region Draw functions
        public void Draw(Transformations transformations)
        {
            Effect effect = GameEngine.GetInstance().ResourceManager.GetEffect("regulareffect");
            device.DepthStencilState = DepthStencilState.Default;
            device.RasterizerState = RasterizerState.CullCounterClockwise;
            device.BlendState = BlendState.Opaque;

            effect.Parameters["xWorld"].SetValue(transformations.World);
            effect.Parameters["xView"].SetValue(transformations.View);
            effect.Parameters["xProjection"].SetValue(transformations.Projection);
            effect.CurrentTechnique.Passes[0].Apply();

            device.Indices = indexBuffer;
            device.SetVertexBuffer(vertexBuffer);
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertices.Length, 0, 12);
        }

        public void Draw(Effect effect, Transformations transformations)
        {
            device.DepthStencilState = DepthStencilState.Default;
            device.RasterizerState = RasterizerState.CullCounterClockwise;
            device.BlendState = BlendState.Opaque;

            effect.Parameters["xWorld"].SetValue(transformations.World);
            effect.Parameters["xView"].SetValue(transformations.View);
            effect.Parameters["xProjection"].SetValue(transformations.Projection);
            effect.CurrentTechnique.Passes[0].Apply();

            device.Indices = indexBuffer;
            device.SetVertexBuffer(vertexBuffer);
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertices.Length, 0, 12);
        }

        public void Draw(Effect effect, Transformations transformations, Vector3 position, float scale)
        {
            device.DepthStencilState = DepthStencilState.Default;
            device.RasterizerState = RasterizerState.CullCounterClockwise;
            device.BlendState = BlendState.Opaque;

            effect.Parameters["xWorld"].SetValue(Matrix.CreateScale(scale) * Matrix.CreateTranslation(position));
            effect.Parameters["xView"].SetValue(transformations.View);
            effect.Parameters["xProjection"].SetValue(transformations.Projection);
            effect.CurrentTechnique.Passes[0].Apply();

            device.Indices = indexBuffer;
            device.SetVertexBuffer(vertexBuffer);
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertices.Length, 0, 12);
        }
        #endregion
    }
}
