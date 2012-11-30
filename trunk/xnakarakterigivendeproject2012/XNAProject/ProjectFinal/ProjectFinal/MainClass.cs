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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public partial class MainClass : Microsoft.Xna.Framework.Game
    {
        #region fields
        public const int HEIGHT = 800;
        public const int WIDTH = 1280;
        public GraphicsDeviceManager graphics {get; set;}
        public GraphicsDevice device {get; set;}
        public SpriteBatch spriteBatch {get; set;}
        SpriteFont spriteFont;
        public Matrix view, projection;
        Random g;
        Effect effect;
        Effect effectskybox;
        public SpaceShip spaceShip { get; set; }
        float gameSpeed = 1.0f;


        #region variabler til planetter og måner
        /*********************************************************/
        /*Objekter i rommet*/


        //Sola
        SpaceObject Sol;

        //Planetter
        SpaceObject mercury, venus, earth, mars, jupiter, saturn, uran, neptun, pluto;

        //rings hos saturn
        SpaceObject saturnRing;

        /*Naturlige satellitter*/

        //earth
        SpaceObject moon;
        //mars
        SpaceObject fobos, deimos;

        //jupiter
        //største satellitter
        SpaceObject Io, Europa, Ganymede, Callisto;


        //Saturn
        //Største satellitter
        SpaceObject Mimas, Enceladus, Tethys, Dione, Rhea, Titan, Iapetus;


        //Uran
        //Største satelitter
        SpaceObject Miranda, Ariel, Umbriel, Titania, Oberon;

        //Neptun
        //Størst satellitt
        SpaceObject Triton;

        #endregion

        #region variabler til skybox
        Skybox skybox;
        Texture2D[] skyboxTextures;
        Model skyboxModel;

        public Model spaceShipMode {get; set;}

        public Vector3 cameraPosition;

        public enum GameState
        {
            MainMenu,
            About,
            Playing,
            Ship,
        } 
        public GameState CurrentGameState = GameState.MainMenu;
        Menu menu;

        #endregion

        #endregion




        public MainClass()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            g = new Random(this.GetHashCode());
            initDevice();
            initCamera();
            initSolarSystem();
            initSpaceShip();
            //initSkybox();
            base.Initialize();      
        }

        private void initSpaceShip()
        {
            this.spaceShip = new SpaceShip(this, this.view, this.projection);         
        }


        #region Solar System init
        private void initSolarSystem()
        {
            //Sola
            this.Sol = new SpaceObject(this, 10f, 0, 0, 0, 0, 0.0f, null, ref this.view, ref this.projection);
            this.Sol.isEmissive = true;
            this.Components.Add(this.Sol);

            //Planetter
            this.mercury = new SpaceObject(this, 0.1f, 300f, 1f, 0.75f, 0.0f, 1.0f, this.Sol, ref this.view, ref this.projection);
            this.Components.Add(this.mercury);

            this.venus = new SpaceObject(this, 0.2f, 500f, 1f, -0.75f, 0.0f, 1.0f, this.Sol, ref this.view, ref this.projection);
            this.Components.Add(this.venus);

            this.earth = new SpaceObject(this, 0.3f, 700f, 1f, 0.75f, 0.0f, 1.0f, this.Sol, ref this.view, ref this.projection);
            this.Components.Add(this.earth);

            this.mars = new SpaceObject(this, 0.25f, 900f, 1f, 0.3f, 0, 1.0f, this.Sol, ref this.view, ref this.projection);
            this.Components.Add(this.mars);

            this.jupiter = new SpaceObject(this, 0.6f, 1200f, 1f, 0.2f, 0, 1.0f, this.Sol, ref this.view, ref this.projection);
            this.Components.Add(this.jupiter);

            this.saturn = new SpaceObject(this, 0.5f, 1500f, 1f, 0.7f, 0, 1.0f, this.Sol, ref this.view, ref this.projection);
            this.Components.Add(this.saturn);

            this.saturnRing = new SpaceObject(this, 5.5f, 0f, 0f, 0.0f, 0, 0.0f, this.saturn, ref this.view, ref this.projection);
            this.Components.Add(this.saturnRing);

            this.uran = new SpaceObject(this, 0.45f, 1700f, 1f, 1f, 0, 1.0f, this.Sol, ref this.view, ref this.projection);
            this.Components.Add(this.uran);

            this.neptun = new SpaceObject(this, 0.4f, 2100f, 1f, 0.2f, 0, 1.0f, this.Sol, ref this.view, ref this.projection);
            this.Components.Add(this.neptun);

            this.pluto = new SpaceObject(this, 0.05f, 2400f, 1f, 0.1f, 0, 1.0f, this.Sol, ref this.view, ref this.projection);
            this.Components.Add(this.pluto);


            //satellitter

            //earth
            this.moon = new SpaceObject(this, 0.1f, 100f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, this.earth, ref this.view, ref this.projection);
            this.Components.Add(this.moon);
            //mars
            this.fobos = new SpaceObject(this, 0.1f, 100f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, this.mars, ref this.view, ref this.projection);
            this.Components.Add(this.fobos);

            this.deimos = new SpaceObject(this, 0.12f, 130f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, this.mars, ref this.view, ref this.projection);
            this.Components.Add(this.deimos);

            //jupiter
            //største satellitter
            this.Io = new SpaceObject(this, 0.1f, 100f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, this.jupiter, ref this.view, ref this.projection);
            this.Components.Add(this.Io);

            this.Europa = new SpaceObject(this, 0.1f, 100f, (float)g.NextDouble(), (float)g.NextDouble(), (float)g.NextDouble(), (float)g.NextDouble(), this.jupiter, ref this.view, ref this.projection);
            this.Components.Add(this.Europa);

            this.Ganymede = new SpaceObject(this, 0.1f, 100f, (float)g.NextDouble(), (float)g.NextDouble(), (float)g.NextDouble(), (float)g.NextDouble(), this.jupiter, ref this.view, ref this.projection);
            this.Components.Add(this.Ganymede);

            this.Callisto = new SpaceObject(this, 0.1f, 100f, (float)g.NextDouble(), (float)g.NextDouble(), (float)g.NextDouble(), (float)g.NextDouble(), this.jupiter, ref this.view, ref this.projection);
            this.Components.Add(this.Callisto);

            //Saturn
            //Største satellitter
            this.Mimas = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.saturn, ref this.view, ref this.projection);
            this.Components.Add(this.Mimas);

            this.Enceladus = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.saturn, ref this.view, ref this.projection);
            this.Components.Add(this.Enceladus);

            this.Tethys = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.saturn, ref this.view, ref this.projection);
            this.Components.Add(this.Tethys);

            this.Dione = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.saturn, ref this.view, ref this.projection);
            this.Components.Add(this.Dione);

            this.Rhea = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.saturn, ref this.view, ref this.projection);
            this.Components.Add(this.Rhea);

            this.Titan = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.saturn, ref this.view, ref this.projection);
            this.Components.Add(this.Titan);

            this.Iapetus = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.saturn, ref this.view, ref this.projection);
            this.Components.Add(this.Iapetus);


            //Uran
            //Største satelitter
            this.Miranda = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.uran, ref this.view, ref this.projection);
            this.Components.Add(this.Miranda);

            this.Ariel = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.uran, ref this.view, ref this.projection);
            this.Components.Add(this.Ariel);

            this.Umbriel = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.uran, ref this.view, ref this.projection);
            this.Components.Add(this.Umbriel);

            this.Titania = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.uran, ref this.view, ref this.projection);
            this.Components.Add(this.Titania);

            this.Oberon = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.uran, ref this.view, ref this.projection);
            this.Components.Add(this.Oberon);



            //Neptun
            //Størst satellitt
            this.Triton = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.neptun, ref this.view, ref this.projection);
            this.Components.Add(this.Triton);

        }
        #endregion

        private void initCamera()
        {
            cameraPosition = new Vector3(10000, 10000, 10000);
            Vector3 cameraTarget = Vector3.Zero;
            Vector3 cameraUpVector = new Vector3(0.0f, 1.0f, 0.0f);
            float aspectRatio = (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height;
            Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 100f, 200000.0f, out projection);
            Matrix.CreateLookAt(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out view);
        }

        private void initDevice()
        {
            this.device = graphics.GraphicsDevice;
            this.IsMouseVisible = true;
            this.graphics.PreferredBackBufferWidth = WIDTH;
            this.graphics.PreferredBackBufferHeight = HEIGHT;
            this.graphics.IsFullScreen = false;
            this.graphics.ApplyChanges();
            this.Window.Title = "Prosjekt";
        }

        private void initSkybox()
        {
            effectskybox = Content.Load<Effect>(@"effects/effects");
            this.skybox = new Skybox(this, effectskybox);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            loadSolarSystem();
            loadSolaParticles();
            this.spaceShip.load(this.effect, Content.Load<Model>(@"models/testa1"), Content.Load <Texture2D>("textures-planets/ship"));
            this.Components.Add(this.spaceShip);
            //this.skybox.load(this.effectskybox, Content.Load<Model>(@"textures-skybox/skybox2"));
            //this.Components.Add(this.skybox);

            skyboxModel = LoadModel("textures-skybox/skybox2", out skyboxTextures);

            menu = new Menu(this);
            menu.LoadContent();
        }

        private Model LoadModel(string assetName, out Texture2D[] textures)
        {

            Model newModel = Content.Load<Model>(assetName);
            textures = new Texture2D[newModel.Meshes.Count];
            int i = 0;
            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (BasicEffect currentEffect in mesh.Effects)
                    textures[i++] = currentEffect.Texture;

            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect.Clone();

            return newModel;
        }

        #region load solar  system
        private void loadSolarSystem()
        {
            effect = Content.Load<Effect>(@"effects/effectsRiemersTut");
            effect.Parameters["xLightPos"].SetValue(new Vector3(0f, 0f, 0f));
            effect.Parameters["xLightPower"].SetValue(0.99f);
            effect.Parameters["xAmbient"].SetValue(0.1f);
            
            Model model = LoadModelWithBoundingSphere(@"models/planet", ref Sol.matrixBoneTr, ref Sol.matrixOriginBoneTr);
            this.Sol.load(effect, model, Content.Load<Texture2D>(@"textures-planets/sunmap"));
            //Planetter

            model = LoadModelWithBoundingSphere(@"models/planet", ref mercury.matrixBoneTr, ref mercury.matrixOriginBoneTr);
            this.mercury.load(effect, model, Content.Load<Texture2D>(@"textures-planets/mercurymap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref venus.matrixBoneTr, ref venus.matrixOriginBoneTr);
            this.venus.load(effect, model, Content.Load<Texture2D>(@"textures-planets/venusmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref earth.matrixBoneTr, ref earth.matrixOriginBoneTr);
            this.earth.load(effect, model, Content.Load<Texture2D>(@"textures-planets/earthmap1k"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref mars.matrixBoneTr, ref mars.matrixOriginBoneTr);
            this.mars.load(effect, model, Content.Load<Texture2D>(@"textures-planets/mars_1k_color"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref jupiter.matrixBoneTr, ref jupiter.matrixOriginBoneTr);
            this.jupiter.load(effect, model, Content.Load<Texture2D>(@"textures-planets/jupitermap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref saturn.matrixBoneTr, ref saturn.matrixOriginBoneTr);
            this.saturn.load(effect, model, Content.Load<Texture2D>(@"textures-planets/saturnmap"));


            model = LoadModelWithBoundingSphere(@"models/ring", ref saturnRing.matrixBoneTr, ref saturnRing.matrixOriginBoneTr);
            this.saturnRing.load(effect, model, Content.Load<Texture2D>(@"textures-planets/saturnringcolor"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref uran.matrixBoneTr, ref uran.matrixOriginBoneTr);
            this.uran.load(effect, model, Content.Load<Texture2D>(@"textures-planets/uranusmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref neptun.matrixBoneTr, ref neptun.matrixOriginBoneTr);
            this.neptun.load(effect, model, Content.Load<Texture2D>(@"textures-planets/neptunemap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref pluto.matrixBoneTr, ref pluto.matrixOriginBoneTr);
            this.pluto.load(effect, model, Content.Load<Texture2D>(@"textures-planets/plutomap1k"));
            //satellitter

            model = LoadModelWithBoundingSphere(@"models/planet", ref moon.matrixBoneTr, ref moon.matrixOriginBoneTr);
            this.moon.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref fobos.matrixBoneTr, ref fobos.matrixOriginBoneTr);
            this.fobos.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref deimos.matrixBoneTr, ref deimos.matrixOriginBoneTr);
            this.deimos.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref Io.matrixBoneTr, ref Io.matrixOriginBoneTr);
            this.Io.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref Europa.matrixBoneTr, ref Europa.matrixOriginBoneTr);
            this.Europa.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref Ganymede.matrixBoneTr, ref Ganymede.matrixOriginBoneTr);
            this.Ganymede.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref Callisto.matrixBoneTr, ref Callisto.matrixOriginBoneTr);
            this.Callisto.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref Mimas.matrixBoneTr, ref Mimas.matrixOriginBoneTr);
            this.Mimas.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref Enceladus.matrixBoneTr, ref Enceladus.matrixOriginBoneTr);
            this.Enceladus.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref Dione.matrixBoneTr, ref Dione.matrixOriginBoneTr);
            this.Dione.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref Tethys.matrixBoneTr, ref Tethys.matrixOriginBoneTr);
            this.Tethys.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref Rhea.matrixBoneTr, ref Rhea.matrixOriginBoneTr);
            this.Rhea.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref Titan.matrixBoneTr, ref Titan.matrixOriginBoneTr);
            this.Titan.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref Iapetus.matrixBoneTr, ref Iapetus.matrixOriginBoneTr);
            this.Iapetus.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref Miranda.matrixBoneTr, ref Miranda.matrixOriginBoneTr);
            this.Miranda.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref Ariel.matrixBoneTr, ref Ariel.matrixOriginBoneTr);
            this.Ariel.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref Umbriel.matrixBoneTr, ref Umbriel.matrixOriginBoneTr);
            this.Umbriel.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref Titania.matrixBoneTr, ref Titania.matrixOriginBoneTr);
            this.Titania.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref Oberon.matrixBoneTr, ref Oberon.matrixOriginBoneTr);
            this.Oberon.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

            model = LoadModelWithBoundingSphere(@"models/planet", ref Triton.matrixBoneTr, ref Triton.matrixOriginBoneTr);
            this.Triton.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));

        }
        #endregion

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (CurrentGameState == GameState.Playing)
            {
                UpdateExplosions(gameTime);
                addAsteroid(gameTime);
                UpdateCamera();
                ProcessKeyboard(gameTime);
                float moveSpeed = gameTime.ElapsedGameTime.Milliseconds / 500.0f * gameSpeed;
                MoveForward(ref spaceShip.shipPosition, spaceShip.shipRotation, moveSpeed);
            }

            CurrentGameState = (MainClass.GameState)menu.getCurrentGameState();
            menu.Update(gameTime);
            base.Update(gameTime);
        }


        private void UpdateCamera()
        {
            Vector3 campos = new Vector3(0, 0f, 600f);
            campos = Vector3.Transform(campos, Matrix.CreateFromQuaternion(this.spaceShip.shipRotation));
            campos += this.spaceShip.shipPosition;
            Vector3 camup = new Vector3(0, 1, 0);
            camup = Vector3.Transform(camup, Matrix.CreateFromQuaternion(this.spaceShip.shipRotation));
            this.view = Matrix.CreateLookAt(campos, this.spaceShip.shipPosition, camup);
            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.2f, 200000.0f);

        }

        private void ProcessKeyboard(GameTime gameTime)
        {
            float leftRightRot = 0;

            float turningSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            turningSpeed *= 1.6f * gameSpeed;
            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.Right))
                leftRightRot += turningSpeed;
            if (keys.IsKeyDown(Keys.Left))
                leftRightRot -= turningSpeed;

            float upDownRot = 0;
            if (keys.IsKeyDown(Keys.Down))
                upDownRot += turningSpeed;
            if (keys.IsKeyDown(Keys.Up))
                upDownRot -= turningSpeed;

            Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, -1), leftRightRot) * Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), upDownRot);
            this.spaceShip.shipRotation *= additionalRot;      
        }

        private void MoveForward(ref Vector3 position, Quaternion rotationQuat, float speed)
        {
            Vector3 addVector = Vector3.Transform(new Vector3(0, 0, -400), rotationQuat);
            position += addVector * speed;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            rs.FillMode = FillMode.Solid;
            device.RasterizerState = rs;
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            DrawSkybox();

            menu.Draw(gameTime);

            foreach (ParticleExplosion p in explosions)
            {
                p.Draw(this.effect, ref this.view, ref this.projection, this.StraalTexture);
            }
            base.Draw(gameTime);
        }

        private void DrawSkybox()
        {
            SamplerState ss = new SamplerState();
            ss.AddressU = TextureAddressMode.Clamp;
            ss.AddressV = TextureAddressMode.Clamp;
            device.SamplerStates[0] = ss;

            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferEnable = false;

            device.DepthStencilState = dss;

            Matrix[] skyboxTransforms = new Matrix[skyboxModel.Bones.Count];
            skyboxModel.CopyAbsoluteBoneTransformsTo(skyboxTransforms);
            int i = 0;
            foreach (ModelMesh mesh in skyboxModel.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    Matrix worldMatrix = skyboxTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(cameraPosition);
                    effect.CurrentTechnique = effect.Techniques["Textured"];
                    effect.Parameters["xWorld"].SetValue(worldMatrix);
                    effect.Parameters["xView"].SetValue(view);
                    effect.Parameters["xProjection"].SetValue(projection);
                    effect.Parameters["xTexture"].SetValue(skyboxTextures[i++]);
                }
                mesh.Draw();
            }

            dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            device.DepthStencilState = dss;
        }



        #region entry point
        static void Main(string[] args)
        {
            using (MainClass game = new MainClass())
            {
                game.Run();
            }
        }
        #endregion
    }
}
