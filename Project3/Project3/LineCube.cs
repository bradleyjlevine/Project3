using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project3
{
    class LineCube
    {
        #region Fields
        VertexPositionColor[] vertices;
        ushort[] indices;

        //Calculate the position of the vertices on the top face
        Vector3 topLeftFront = new Vector3(-1.0f, 1.0f, -1.0f);
        Vector3 topLeftBack = new Vector3(-1.0f, 1.0f, 1.0f);
        Vector3 topRightFront = new Vector3(1.0f, 1.0f, -1.0f);
        Vector3 topRightBack = new Vector3(1.0f, 1.0f, 1.0f);

        //Calculate the postion of the vertices on the bottom face
        Vector3 btmLeftFront = new Vector3(-1.0f, -1.0f, -1.0f);
        Vector3 btmLeftBack = new Vector3(-1.0f, -1.0f, 1.0f);
        Vector3 btmRightFront = new Vector3(1.0f, -1.0f, -1.0f);
        Vector3 btmRightBack = new Vector3(1.0f, -1.0f, 1.0f);

        const int NUM_VERTICES = 24;
        #endregion

        #region Intialize
        /// <summary>
        /// Creates the stenciled cubes info.
        /// </summary>
        public LineCube()
        {
            vertices = new VertexPositionColor[8];
            indices = new ushort[NUM_VERTICES];

            vertices[0] = new VertexPositionColor(topLeftFront, Color.White);
            vertices[1] = new VertexPositionColor(topRightFront, Color.Red);
            vertices[2] = new VertexPositionColor(btmRightFront, Color.White);
            vertices[3] = new VertexPositionColor(btmLeftFront, Color.Red);
            vertices[4] = new VertexPositionColor(topRightBack, Color.Red); 
            vertices[5] = new VertexPositionColor(topLeftBack, Color.White);
            vertices[6] = new VertexPositionColor(btmRightBack, Color.White);
            vertices[7] = new VertexPositionColor(btmLeftBack, Color.Red);

            indices[0] = 0; indices[1] = 1;
            indices[2] = 1; indices[3] = 2;
            indices[4] = 2; indices[5] = 3;
            indices[6] = 3; indices[7] = 7;
            indices[8] = 1; indices[9] = 4;
            indices[10] = 0; indices[11] = 5;
            indices[12] = 3; indices[13] = 0;
            indices[14] = 2; indices[15] = 6;
            indices[16] = 4; indices[17] = 5;
            indices[18] = 5; indices[19] = 7;
            indices[20] = 7; indices[21] = 6;
            indices[22] = 6; indices[23] = 4;
        }

        #endregion

        /// <summary>
        /// Renders the stincle cube.
        /// </summary>
        /// <param name="device">The current graphics device.</param>
        public void Render(GraphicsDevice device)
        {

            VertexBuffer buffer = new VertexBuffer(device, typeof(VertexPositionColor), 8, BufferUsage.WriteOnly);
            IndexBuffer indexBuffer = new IndexBuffer(device, typeof(ushort), NUM_VERTICES, BufferUsage.WriteOnly);

            buffer.SetData<VertexPositionColor>(vertices);
            indexBuffer.SetData(indices);

            device.SetVertexBuffer(buffer);
            device.Indices = indexBuffer;

            device.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, NUM_VERTICES / 2);
        }
    }
}
