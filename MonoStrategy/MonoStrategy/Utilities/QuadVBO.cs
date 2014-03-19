using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoStrategy.Utility;

namespace MonoStrategy.VBOs
{
    class QuadVBO
    {
        private VertexPositionNormalTexture[] vertices;

        public int[] indices;

        private static QuadVBO instance = null;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        private GraphicsDevice device;

        public static QuadVBO Instance
        {
            get { return instance; }
        }

        private QuadVBO(GraphicsDevice device)
        {
            this.device = device;

            BuildVBO();
        }

        private void BuildVBO()
        {
            vertices = new VertexPositionNormalTexture[4];

            float s = 0.5f;
            vertices[0].Position = new Vector3(s, s, 0);
            vertices[0].TextureCoordinate = new Vector2(0.0f, 0.0f);
            vertices[0].Normal = new Vector3(0.0f, 0.0f, -1.0f);

            vertices[1].Position = new Vector3(-s, s, 0);
            vertices[1].TextureCoordinate = new Vector2(1.0f, 0.0f);
            vertices[1].Normal = new Vector3(0.0f, 0.0f, -1.0f);

            vertices[2].Position = new Vector3(-s, -s, 0);
            vertices[2].TextureCoordinate = new Vector2(1.0f, 1.0f);
            vertices[2].Normal = new Vector3(0.0f, 0.0f, -1.0f);

            vertices[3].Position = new Vector3(s, -s, 0);
            vertices[3].TextureCoordinate = new Vector2(0.0f, 1.0f);
            vertices[3].Normal = new Vector3(0.0f, 0.0f, -1.0f);

            indices = new int[6];

            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;

            indices[3] = 2;
            indices[4] = 3;
            indices[5] = 0;

            vertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);

            indexBuffer = new IndexBuffer(device, typeof(int), indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);
        }

        public static QuadVBO Initialize(GraphicsDevice device)
        {
            instance = new QuadVBO(device);

            return instance;
        }

        public void Draw(Effect effect, Transformations transformations)
        {
            device.DepthStencilState = DepthStencilState.Default;
            device.RasterizerState = RasterizerState.CullCounterClockwise;
            device.BlendState = BlendState.AlphaBlend;

            effect.Parameters["World"].SetValue(transformations.World);
            effect.Parameters["View"].SetValue(transformations.View);
            effect.Parameters["Projection"].SetValue(transformations.Projection);

            effect.CurrentTechnique.Passes[0].Apply();
            device.Indices = indexBuffer;
            device.SetVertexBuffer(vertexBuffer);
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertices.Length, 0, 2);

            device.BlendState = BlendState.Opaque;
        }

        public void DrawWithAlphablend(Effect effect, Transformations transformations)
        {
            //Save device state
            RasterizerState r = device.RasterizerState;
            BlendState b = device.BlendState;

            // Enable alpha blend
            /*BlendState _blendState = new BlendState();
            _blendState.AlphaSourceBlend = Blend.SourceAlpha;
            _blendState.AlphaDestinationBlend = Blend.InverseSourceAlpha;
            _blendState.ColorSourceBlend = Blend.SourceAlpha;
            _blendState.ColorDestinationBlend = Blend.InverseSourceAlpha;
            device.BlendState = _blendState;*/
            device.DepthStencilState = DepthStencilState.Default;
            device.BlendState = BlendState.AlphaBlend;
            device.RasterizerState = RasterizerState.CullNone;

            effect.Parameters["World"].SetValue(transformations.World);
            effect.Parameters["View"].SetValue(transformations.View);
            effect.Parameters["Projection"].SetValue(transformations.Projection);

            effect.CurrentTechnique.Passes[0].Apply();
            device.Indices = indexBuffer;
            device.SetVertexBuffer(vertexBuffer);
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertices.Length, 0, 2);

            device.RasterizerState = r;
            device.BlendState = b;
        }
    }
}
