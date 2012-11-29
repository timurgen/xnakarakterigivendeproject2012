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

namespace XNAProject
{
    class SpaceShip
    {
        //Variables
        private Vector3 cameraPosition;// = new Vector3(0, 1000, 1000);
        private Vector3 cameraTarget;// = new Vector3(0, 0, 0);
        private Vector3 cameraUpVector;// = new Vector3(0, 1, 0);
        private Vector3 viewVector;

        private float cameraX = 50000;
        private float cameraY = 50000;
        private float cameraZ = 50000;

        Matrix matrixWorld;
        Matrix matrixView;
        Matrix matrixProjection;

        Vector3 shipPosition = new Vector3(0.0f, 0.0f, 0.0f);
        Quaternion shipRotation;

        private MainClass game;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        float gameSpeed = 1.0f;

        public SpaceShip(MainClass _game)
        {
            game = (MainClass)_game;
            graphics = game.graphics;
            spriteBatch = game.spriteBatch;
        }

        public void update(GameTime _gametime)
        {
            ProcessKeyboard(_gametime);

            float moveSpeed = _gametime.ElapsedGameTime.Milliseconds / 500.0f * gameSpeed * 10;
            MoveForward(ref shipPosition, shipRotation, moveSpeed);

            UpdateCamera();
        }

        private void ProcessKeyboard(GameTime _gameTime)
        {
            float leftRightRot = 0;
            float turningSpeed = (float)_gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            turningSpeed *= 1.6f * gameSpeed;

            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.Right))
            {
                leftRightRot += turningSpeed;
            }

            if (keys.IsKeyDown(Keys.Left))
            {
                leftRightRot -= turningSpeed;
            }

            float upDownRot = 0;
            if (keys.IsKeyDown(Keys.Down))
            {
                upDownRot += turningSpeed;
            }
            if (keys.IsKeyDown(Keys.Up))
            {
                upDownRot -= turningSpeed;
            }

            Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, -1), leftRightRot) * Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), upDownRot);
            shipRotation *= additionalRot;
        }

        private void MoveForward(ref Vector3 position, Quaternion rotationQuat, float speed)
        {
            Vector3 addVector = Vector3.Transform(new Vector3(0, 0, -1), rotationQuat);
            position += addVector * speed;
        }


        private void UpdateCamera()
        {
            cameraPosition = new Vector3(100.0f, 100.0f, 10.0f);
            cameraPosition = Vector3.Transform(cameraPosition, Matrix.CreateFromQuaternion(shipRotation));
            cameraPosition += shipPosition;

            cameraUpVector = new Vector3(0.0f, 1.0f, 0.0f);
            cameraUpVector = Vector3.Transform(cameraUpVector, Matrix.CreateFromQuaternion(shipRotation));

            matrixView = Matrix.CreateLookAt(cameraPosition, shipPosition, cameraUpVector);
            matrixProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.GraphicsDevice.Viewport.AspectRatio, 1f, 2000000.0f);
        }

        public void draw()
        {

            Matrix[] spaceShipTransforms = new Matrix[game.shipModel.Bones.Count];
            game.shipModel.CopyAbsoluteBoneTransformsTo(spaceShipTransforms);

            foreach (ModelMesh mesh in game.shipModel.Meshes)
            {
                foreach (Effect e in mesh.Effects)
                {
                    Matrix matrixWorld = Matrix.CreateScale(0.1f) * Matrix.CreateFromQuaternion(shipRotation) * Matrix.CreateTranslation(shipPosition);
                    //Console.WriteLine(matrixWorld);
                    e.CurrentTechnique = e.Techniques["Textured"];
                    e.Parameters["xWorld"].SetValue(spaceShipTransforms[mesh.ParentBone.Index] * matrixWorld);
                    e.Parameters["xView"].SetValue(matrixView);
                    e.Parameters["xProjection"].SetValue(matrixProjection);
                    e.Parameters["xTexture"].SetValue(game.Content.Load<Texture2D>("textures-planets/ASTEROIDS"));
                }
                mesh.Draw();
            }
        }

    }
}
