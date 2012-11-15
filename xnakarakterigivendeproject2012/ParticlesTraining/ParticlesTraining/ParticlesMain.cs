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

namespace ParticlesTraining
{

    public class ParticlesMain : Microsoft.Xna.Framework.Game
    {
        private GraphicsDevice device;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Vector3 cameraPos, cameraTarget, cameraUp;

        /// <summary>
        /// neste variabler beskriver matriser som brukes i projektet
        /// </summary>
        private Matrix world, view, projection;

        /// <summary>
        /// bruker basic effect i utgangspunk, men det skal byttes med egen shader
        /// </summary>
        BasicEffect effect;

        public ParticlesMain()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.cameraPos = new Vector3(60f,60f,60f);
            this.cameraTarget = Vector3.Zero;
            this.cameraUp = Vector3.Up;

        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            initDevice();
            initCamera();
            base.Initialize();
        }

        private void initCamera()
        {
            float aspectRatio = (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height;
            Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1f, 100.0f, out projection);
            Matrix.CreateLookAt(ref cameraPos, ref cameraTarget, ref cameraUp, out view);


        }

        private void initDevice()
        {
            device = graphics.GraphicsDevice;
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Oblig tre";
            effect = new BasicEffect(graphics.GraphicsDevice);
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
