using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAProject
{
    class AsteroidManager
    {
        public const int maxSteroids = 5000;
        public const float edgeLength = 1f;
        List<Vector3> asteroidsPos;
        List<VertexPositionNormalTexture[]> asteroids; 
        MainClass game;
        Random g;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_game"></param>
        /// <param name="_baneRadius"></param>
        /// <param name="_beltWidth"></param>
        /// <param name="_beltTikness"></param>
        public AsteroidManager(MainClass _game, float _baneRadius,float _beltWidth, float _beltTikness)
        {
            g = new Random(this.GetHashCode());
            this.game = _game;
            asteroidsPos = new List<Vector3>();
            asteroids = new List<VertexPositionNormalTexture[]>();

            for (int i = 0; i < maxSteroids; i++)
            {
                float angleX = (float)(g.NextDouble() * MathHelper.TwoPi);
                float x = (float) (Math.Cos(angleX) * (_baneRadius + (g.Next((int)_beltTikness)-_beltTikness/2)));
                float y = (float)(g.Next((int)_beltTikness+1) - _beltTikness / 2);
                float z = (float)(Math.Sin(angleX) * (_baneRadius + (g.Next((int)_beltTikness) - _beltTikness / 2))); 
                Vector3 asteroidPos = new Vector3(x,y,z);
                VertexPositionNormalTexture[] vertex = new VertexPositionNormalTexture[3];

                vertex[0].Position = asteroidPos - new Vector3(-edgeLength,-edgeLength,edgeLength);
                vertex[1].Position = asteroidPos - new Vector3(edgeLength, -edgeLength, edgeLength);
                vertex[2].Position = asteroidPos - new Vector3(edgeLength, -edgeLength, -edgeLength);

                vertex[0].TextureCoordinate.X = 0;
                vertex[0].TextureCoordinate.Y = 1;

                vertex[1].TextureCoordinate.X = 0.25f;
                vertex[1].TextureCoordinate.Y = 1;

                vertex[2].TextureCoordinate.X = 0.5f;
                vertex[2].TextureCoordinate.Y = 1;

                vertex[0].Normal = new Vector3(1, 1, 1);
                vertex[1].Normal = new Vector3(1, 1, 1);
                vertex[2].Normal = new Vector3(1, 1, 1);

                asteroidsPos.Add(asteroidPos);
                asteroids.Add(vertex);

            }

        }
        


        public void Draw(GameTime gt, Effect _effect)
        {
            _effect.Parameters["xWorld"].SetValue(world);
            //_effect.Parameters["xView"].SetValue(_view);
            //_effect.Parameters["xProjection"].SetValue(_projection);
            _effect.CurrentTechnique = _effect.Techniques["Textured"];
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                foreach (VertexPositionNormalTexture[] asteroid in asteroids)
                {
                    game.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleStrip, asteroid, 0, 1);
                }
                
            }


        }




        float RotY, orbRotY;
        public void Update(GameTime gameTime)
        {
            // Allows the game to exit
            Matrix matIdentity, matTrans, matRotateY, matRotateX, matScale, matOrbT, matOrbR, matOrbTX, matOrbRX;

            matIdentity = Matrix.Identity;

            matScale = Matrix.CreateScale(1f);

            //rotasjon om y akse
            RotY += 0.01f * (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;
            RotY = RotY % (float)(2 * Math.PI);
            matRotateY = Matrix.CreateRotationY(RotY);

            //vinkel til jordakse
            matRotateX = Matrix.CreateRotationX(0);

            //orbital rotasjon
            matOrbT = Matrix.CreateTranslation(0.1f, 0, 0.1f);
            orbRotY += 0.001f * (float)gameTime.ElapsedGameTime.Milliseconds / 50000.0f;
            orbRotY = orbRotY % (float)(2 * Math.PI);
            matOrbR = Matrix.CreateRotationY((orbRotY));
            matOrbRX = Matrix.CreateRotationX(0);


            matTrans = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);

            //Kumulativ world‐matrise;
            world = matIdentity * matScale * matRotateY * matRotateX * (matOrbT * matOrbR) * matTrans * matOrbRX;
        }

        public Matrix world { get; set; }
    }

}
