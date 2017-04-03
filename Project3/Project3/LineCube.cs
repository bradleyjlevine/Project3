using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project3
{
    class LineCube
    {
        static VertexPositionColorTexture[] vertices;

        //Calculate the position of the vertices on the top face
        static Vector3 topLeftFront = new Vector3(-1.0f, 1.0f, -1.0f);
        static Vector3 topLeftBack = new Vector3(-1.0f, 1.0f, 1.0f);
        static Vector3 topRightFront = new Vector3(1.0f, 1.0f, -1.0f);
        static Vector3 topRightBack = new Vector3(1.0f, 1.0f, 1.0f);

        //Calculate the postion of the vertices on the bottom face
        static Vector3 btmLeftFront = new Vector3(-1.0f, -1.0f, -1.0f);
        static Vector3 btmLeftBack = new Vector3(-1.0f, -1.0f, 1.0f);
        static Vector3 btmRightFront = new Vector3(1.0f, -1.0f, -1.0f);
        static Vector3 btmRightBack = new Vector3(1.0f, -1.0f, 1.0f);

        const int NUM_VERTICES = 24;

        public LineCube()
        {

            vertices = new VertexPositionColorTexture[NUM_VERTICES];

            vertices[0] = new VertexPositionColorTexture(topLeftFront, Color.White, Vector2.Zero);
            vertices[1] = new VertexPositionColorTexture(topLeftBack, Color.Red, Vector2.Zero);

            vertices[2] = new VertexPositionColorTexture(topRightFront, Color.White, Vector2.Zero);
            vertices[3] = new VertexPositionColorTexture(topRightBack, Color.Red, Vector2.Zero);

            vertices[4] = new VertexPositionColorTexture(btmLeftFront, Color.White, Vector2.Zero);
            vertices[5] = new VertexPositionColorTexture(btmLeftBack, Color.Red, Vector2.Zero);

            vertices[6] = new VertexPositionColorTexture(btmRightFront, Color.White, Vector2.Zero);
            vertices[7] = new VertexPositionColorTexture(btmRightBack, Color.Red, Vector2.Zero);

            vertices[8] = new VertexPositionColorTexture(topLeftFront, Color.White, Vector2.Zero);
            vertices[9] = new VertexPositionColorTexture(topRightFront, Color.Red, Vector2.Zero);

            vertices[10] = new VertexPositionColorTexture(btmLeftFront, Color.White, Vector2.Zero);
            vertices[11] = new VertexPositionColorTexture(btmRightFront, Color.Red, Vector2.Zero);

            vertices[12] = new VertexPositionColorTexture(topLeftFront, Color.White, Vector2.Zero);
            vertices[13] = new VertexPositionColorTexture(btmLeftFront, Color.Red, Vector2.Zero);

            vertices[14] = new VertexPositionColorTexture(topRightFront, Color.White, Vector2.Zero);
            vertices[15] = new VertexPositionColorTexture(btmRightFront, Color.Red, Vector2.Zero);

            vertices[16] = new VertexPositionColorTexture(topLeftBack, Color.White, Vector2.Zero);
            vertices[17] = new VertexPositionColorTexture(topRightBack, Color.Red, Vector2.Zero);

            vertices[18] = new VertexPositionColorTexture(btmLeftBack, Color.White, Vector2.Zero);
            vertices[19] = new VertexPositionColorTexture(btmRightBack, Color.Red, Vector2.Zero);

            vertices[20] = new VertexPositionColorTexture(topLeftBack, Color.White, Vector2.Zero);
            vertices[21] = new VertexPositionColorTexture(btmLeftBack, Color.Red, Vector2.Zero);

            vertices[22] = new VertexPositionColorTexture(topRightBack, Color.White, Vector2.Zero);
            vertices[23] = new VertexPositionColorTexture(btmRightBack, Color.Red, Vector2.Zero);
        }

        public void Render(GraphicsDevice device)
        {

            VertexBuffer buffer = new VertexBuffer(device, typeof(VertexPositionColorTexture), NUM_VERTICES, BufferUsage.WriteOnly);

            buffer.SetData<VertexPositionColorTexture>(vertices);

            device.SetVertexBuffer(buffer);

            device.DrawPrimitives(PrimitiveType.LineList, 0, NUM_VERTICES / 2);

        }
    }
}
