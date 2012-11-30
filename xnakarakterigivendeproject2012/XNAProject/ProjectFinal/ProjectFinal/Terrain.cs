using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ProjectFinal
{

    public class Terrain : DrawableGameComponent
    {
        /************************VARIABLER****************************************************************************/
        Random colorGen;
        //adderer struct som støtter normalvektore
        public struct VertexPositionColorNormal
        {
            public Vector3 Position;
            public Color Color;
            public Vector3 Normal;

            public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
            (
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
            );
        }

        //vil bruke egendefinert shader 
        Effect effect;
        MainClass game;

        VertexPositionColorNormal[] vertices;
        Matrix viewMatrix;
        Matrix projectionMatrix;
        int[] indices;
        Texture2D heightMap;

        private float angle = 0f;
        private int terrainWidth = 4;
        private int terrainHeight = 3;
        private float[,] heightData;

        /*****************************END OF VARIABLER **********************************************************************/

        /*****************************METODER********************************************************************************/


        //beregner normalvektore
        /// <summary>
        /// 
        /// </summary>
        private void CalculateNormals()
        {
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < indices.Length / 3; i++)
            {
                int index1 = indices[i * 3];
                int index2 = indices[i * 3 + 1];
                int index3 = indices[i * 3 + 2];

                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;
            }
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal.Normalize();
            }

        }

        public Terrain(MainClass _game) :base(_game)
        {
            this.game = _game;
        }


        public void LoadContent(Effect _effect, Texture2D _heightMap)
        {
            effect = _effect;
            heightMap = _heightMap;
            LoadHeightData(heightMap);
            SetUpVertices();
            SetUpIndices();
            CalculateNormals();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetUpIndices()
        {
            indices = new int[(terrainWidth - 1) * (terrainHeight - 1) * 6];
            int counter = 0;
            for (int y = 0; y < terrainHeight - 1; y++)
            {
                for (int x = 0; x < terrainWidth - 1; x++)
                {
                    int lowerLeft = x + y * terrainWidth;
                    int lowerRight = (x + 1) + y * terrainWidth;
                    int topLeft = x + (y + 1) * terrainWidth;
                    int topRight = (x + 1) + (y + 1) * terrainWidth;

                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;

                    indices[counter++] = topLeft;
                    indices[counter++] = topRight;
                    indices[counter++] = lowerRight;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetUpVertices()
        {
            vertices = new VertexPositionColorNormal[terrainWidth * terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    vertices[x + y * terrainWidth].Position = new Vector3(x, heightData[x, y], -y);
                    //vertices[x + y * terrainWidth].Color = new Color((float)colorGen.NextDouble(), (float)colorGen.NextDouble(), (float)colorGen.NextDouble());
                    vertices[x + y * terrainWidth].Color = Color.RoyalBlue;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="heightMap"></param>
        private void LoadHeightData(Texture2D heightMap)
        {
            terrainWidth = heightMap.Width;
            terrainHeight = heightMap.Height;

            Color[] heightMapColors = new Color[terrainWidth * terrainHeight];
            heightMap.GetData(heightMapColors);

            heightData = new float[terrainWidth, terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainHeight; y++)
                    heightData[x, y] = heightMapColors[x + y * terrainWidth].R / 5.0f;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetUpCamera()
        {
            viewMatrix = Matrix.CreateLookAt(new Vector3(80, 60, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.GraphicsDevice.Viewport.AspectRatio, 1.0f, 300.0f);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {



            Matrix worldMatrix = Matrix.CreateTranslation(-terrainWidth / 2.0f, 0, terrainHeight / 2.0f);
            effect.Parameters["xWorld"].SetValue(Matrix.Identity* worldMatrix);
            effect.Parameters["xView"].SetValue(game.view);
            effect.Parameters["xProjection"].SetValue(game.projection);
            effect.Parameters["xTexture"].SetValue(this.heightMap);
            effect.Parameters["xEmissiveColor"].SetValue(new Vector4(1f, 1f, 1f, 0f));
            effect.Parameters["isEmissive"].SetValue(false);
            effect.Parameters["xLightsWorldViewProjection"].SetValue(Matrix.Identity * game.view * game.projection);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3, VertexPositionColorNormal.VertexDeclaration);
            }

        }

    }
}
