using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using monostrategy.Utility;

namespace monostrategy.VBOs
{

    class CubeVBO2
    {
        private VertexPositionNormalTexture[] vertices;

        public int[] indices;

        private static CubeVBO2 instance = null;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        private GraphicsDevice device;

        public static CubeVBO2 Instance
        {
            get { return instance; }
        }

        private CubeVBO2(GraphicsDevice device)
        {
            this.device = device;

            BuildVBO();
        }

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

            //bottom face
            vertices[18].Position = positions[1];
            vertices[19].Position = positions[5];
            vertices[20].Position = positions[4];
            vertices[21].Position = positions[4];
            vertices[22].Position = positions[0];
            vertices[23].Position = positions[1];
            for (int i = 18; i < 24; i++)
                vertices[i].Normal = new Vector3(0, -1, 0);

            //left face
            vertices[24].Position = positions[1];
            vertices[25].Position = positions[0];
            vertices[26].Position = positions[2];
            vertices[27].Position = positions[2];
            vertices[28].Position = positions[3];
            vertices[29].Position = positions[1];
            for (int i = 24; i < 30; i++)
                vertices[i].Normal = new Vector3(-1, 0, 0);

            //back face
            vertices[30].Position = positions[1];
            vertices[31].Position = positions[3];
            vertices[32].Position = positions[7];
            vertices[33].Position = positions[7];
            vertices[34].Position = positions[5];
            vertices[35].Position = positions[1];
            for (int i = 30; i < 36; i++)
                vertices[i].Normal = new Vector3(0, 0, 1);


            vertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);

            indexBuffer = new IndexBuffer(device, typeof(int), indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);
        }

        public static CubeVBO2 Initialize(GraphicsDevice device)
        {
            instance = new CubeVBO2(device);

            return instance;
        }

        #region Draw functions
        public void Draw(Transformations transformations)
        {
            Effect effect = ResourceManager.Instance.Regulareffect;
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

            effect.Parameters["World"].SetValue(Matrix.CreateScale(scale) * Matrix.CreateTranslation(position));
            effect.Parameters["View"].SetValue(transformations.View);
            effect.Parameters["Projection"].SetValue(transformations.Projection);
            effect.CurrentTechnique.Passes[0].Apply();

            device.Indices = indexBuffer;
            device.SetVertexBuffer(vertexBuffer);
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertices.Length, 0, 12);
        }
        #endregion
    }
}
