using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project3
{
    class LineCube
    {
        static VertexPositionColor[] vertices;

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

            vertices[0] = new VertexPositionColor(topLeftFront, Color.White);
            vertices[1] = new VertexPositionColor(topLeftBack, Color.Red);

            vertices[2] = new VertexPositionColor(topRightFront, Color.White);
            vertices[3] = new VertexPositionColor(topRightBack, Color.Red);

            vertices[4] = new VertexPositionColor(btmLeftFront, Color.White);
            vertices[5] = new VertexPositionColor(btmLeftBack, Color.Red);

            vertices[6] = new VertexPositionColor(btmRightFront, Color.White);
            vertices[7] = new VertexPositionColor(btmRightBack, Color.Red);

            vertices[8] = new VertexPositionColor(topLeftFront, Color.White);
            vertices[9] = new VertexPositionColor(topRightFront, Color.Red);

            vertices[10] = new VertexPositionColor(btmLeftFront, Color.White);
            vertices[11] = new VertexPositionColor(btmRightFront, Color.Red);

            vertices[12] = new VertexPositionColor(topLeftFront, Color.White);
            vertices[13] = new VertexPositionColor(btmLeftFront, Color.Red);

            vertices[14] = new VertexPositionColor(topRightFront, Color.White);
            vertices[15] = new VertexPositionColor(btmRightFront, Color.Red);

            vertices[16] = new VertexPositionColor(topLeftBack, Color.White);
            vertices[17] = new VertexPositionColor(topRightBack, Color.Red);

            vertices[18] = new VertexPositionColor(btmLeftBack, Color.White);
            vertices[19] = new VertexPositionColor(btmRightBack, Color.Red);

            vertices[20] = new VertexPositionColor(topLeftBack, Color.White);
            vertices[21] = new VertexPositionColor(btmLeftBack, Color.Red);

            vertices[22] = new VertexPositionColor(topRightBack, Color.White);
            vertices[23] = new VertexPositionColor(btmRightBack, Color.Red);
        }

        public void Render(GraphicsDevice device)
        {

            VertexBuffer buffer = new VertexBuffer(device, typeof(VertexPositionColor), NUM_VERTICES, BufferUsage.WriteOnly);

            buffer.SetData<VertexPositionColor>(vertices);

            device.SetVertexBuffer(buffer);

            device.DrawPrimitives(PrimitiveType.LineList, 0, NUM_VERTICES / 2);
        }
    }
}
