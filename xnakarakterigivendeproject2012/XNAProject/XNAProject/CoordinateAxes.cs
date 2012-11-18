using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace XNAProject
{
    /// <summary>
    /// Her tegnes koordinate aksene
    /// </summary>
    class CoordinateAxes
    {
        /// <summary>
        /// neste variabler beskriver koordinate akses
        /// </summary>
        private VertexPositionColor[] lineX, lineY, lineZ;
        private float endPoint;

        public CoordinateAxes()
        {
            endPoint = 1000f;
            lineX = new VertexPositionColor[2];
            lineY = new VertexPositionColor[2];
            lineZ = new VertexPositionColor[2];

            lineX[0].Position = new Microsoft.Xna.Framework.Vector3(0f, 0f, 0f);
            lineY[0].Position = new Microsoft.Xna.Framework.Vector3(0f, 0f, 0f);
            lineZ[0].Position = new Microsoft.Xna.Framework.Vector3(0f, 0f, 0f);
            lineX[1].Position = new Microsoft.Xna.Framework.Vector3(endPoint, 0f, 0f);
            lineY[1].Position = new Microsoft.Xna.Framework.Vector3(0f, endPoint, 0f);
            lineZ[1].Position = new Microsoft.Xna.Framework.Vector3(0f, 0f, endPoint);

            lineX[0].Color = Microsoft.Xna.Framework.Color.Red;
            lineY[0].Color = Microsoft.Xna.Framework.Color.Green;
            lineZ[0].Color = Microsoft.Xna.Framework.Color.Blue;

            lineX[1].Color = Microsoft.Xna.Framework.Color.Red;
            lineY[1].Color = Microsoft.Xna.Framework.Color.Green;
            lineZ[1].Color = Microsoft.Xna.Framework.Color.Blue;

        }

        public void draw(GraphicsDevice device)
        {
            device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, lineX, 0, 1);
            device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, lineY, 0, 1);
            device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, lineZ, 0, 1);
        }
    }
}
