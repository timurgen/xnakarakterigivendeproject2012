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
using System.Diagnostics;

namespace XNAProject
{

    public partial class MainClass : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public SpriteFont spriteFont;

        GraphicsDevice device { get; set; }

        public enum GameState
        {
            MainMenu,
            About,
            Playing,
            Ship,
        }

        Effect effectSky;
        BasicEffect basicEffect;
        Effect spaceObjectEffect;
        Effect effect;

        public Matrix matrixView;
        public Matrix matrixProjection;

        /// <summary>
        /// Variabler til kamera
        /// </summary>
        private Vector3 cameraPosition;// = new Vector3(0, 1000, 1000);
        private Vector3 cameraTarget;// = new Vector3(0, 0, 0);
        private Vector3 cameraUpVector;// = new Vector3(0, 1, 0);
        private Vector3 viewVector;

        private float cameraX = 50000;
        private float cameraY = 50000;
        private float cameraZ = 50000;

        private CoordinateAxes cAxes;

        public const int WIDTH = 1280;

        public const int HEIGHT = 800;

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
        //små satellitter , antall 63
        SpaceObject[] smallSatelitesOfJupiter;

        //Saturn
        //Største satellitter
        SpaceObject Mimas, Enceladus, Tethys, Dione, Rhea, Titan, Iapetus;
        //små satellitter, antall 55
        SpaceObject[] smallSatelitesOfSaturn;

        //Uran
        //Største satelitter
        SpaceObject Miranda, Ariel, Umbriel, Titania, Oberon;
        //små satellitter, antall 22
        SpaceObject[] smallSatelitesOfUranus;

        //Neptun
        //Størst satellitt
        SpaceObject Triton;
        //små satellitter, antall 12
        SpaceObject[] smallSatelitesOfNeptune;

        //kunstige objekter

        //International space station
        SpaceObject ISS;

        //StarWars star of death
        SpaceObject StarOfDeath;

        //Asteroids 
        //AsteroidBelt asteroidBelt;
        AsteroidManager astman;
        /*********************************************************/

        private int maxAsteroid = 3000;
        List<VertexPositionNormalTexture[]> asteroids;
        #endregion


        #region Skybox
        //skybox
        Matrix matrixWorld = Matrix.Identity;
        Matrix matrixIdentity = Matrix.Identity;
        Matrix matrixScale = Matrix.CreateScale(1.0f);
        Matrix matrixRotationX = Matrix.CreateRotationX(0.0f);
        Matrix matrixRotationY = Matrix.CreateRotationY(0.0f);
        Matrix matrixTranslation = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);

        /// <summary>
        /// Skybox
        /// </summary>
        private VertexPositionColorTexture[] vericesSkyBox;
        private const float BOUNDARY = 80000.0f;
        private const float EDGE = BOUNDARY * 2.0f;

        //textures skybox
        private Texture2D textureSkyboxFront;
        private Texture2D textureSkyboxBack;
        private Texture2D textureSkyboxLeft;
        private Texture2D textureSkyboxRight;
        private Texture2D textureSkyboxBottom;
        private Texture2D textureSkyboxTop;

        private Random g; //generator av tilfeldige tall

        //Skybox mk2
        //Skybox skybox;

        //Skybox mk3
        Texture2D[] skyboxTextures;
        Model skyboxModel;
        #endregion

        //Space ships
        public Model shipModel;

        Menu menu;

        SpaceShip spaceShip;

        //Kamera
        private FirstPersonCamera camera;
        private InputHandler input;
        List<ParticleExplosion> explosions = new List<ParticleExplosion>();
        ParticleExplosionSettings particleExplosionSettings = new ParticleExplosionSettings();
        ParticleSettings particleSettings = new ParticleSettings();
        Effect explosionEffect;
        Texture2D explosionTexture;

        public GameState CurrentGameState = GameState.MainMenu;

        public MainClass()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            g = new Random(this.GetHashCode());
            camera = new FirstPersonCamera(this);
        }


        protected override void Initialize()
        {
            initDevice();
            initCamera();
            this.cAxes = new CoordinateAxes();
            initializeSolarSystemObjects();
            initializeSkyBox();

            spriteFont = Content.Load<SpriteFont>(@"Fonts\Arial");

            base.Initialize();
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
            this.basicEffect = new BasicEffect(graphics.GraphicsDevice);
        }

        

        private void initCamera()
        {
            cameraPosition = new Vector3(cameraX, cameraY, cameraZ);
            cameraTarget = Vector3.Zero;
            cameraUpVector = new Vector3(0.0f, 1.0f, 0.0f);
            float aspectRatio = (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height;
            Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1f, 2000000.0f, out matrixProjection);
            Matrix.CreateLookAt(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out matrixView);
            basicEffect.Projection = matrixProjection;
            basicEffect.View = matrixView;
            basicEffect.VertexColorEnabled = true;
            camera.View = matrixView;
            camera.Projection = matrixProjection;
        }

        private void initializeSkyBox()
        {
            const float min = 0.003f;
            const float max = 0.997f;

            this.vericesSkyBox = new VertexPositionColorTexture[4];

            this.vericesSkyBox[0].Position = new Vector3(EDGE, -EDGE, 0.0f);
            this.vericesSkyBox[0].Color = Color.Green;
            this.vericesSkyBox[0].TextureCoordinate = new Vector2(min, max);

            this.vericesSkyBox[1].Position = new Vector3(EDGE, EDGE, 0.0f);
            this.vericesSkyBox[1].Color = Color.Green;
            this.vericesSkyBox[1].TextureCoordinate = new Vector2(min, min);

            this.vericesSkyBox[2].Position = new Vector3(-EDGE, -EDGE, 0.0f);
            this.vericesSkyBox[2].Color = Color.Green;
            this.vericesSkyBox[2].TextureCoordinate = new Vector2(max, max);

            this.vericesSkyBox[3].Position = new Vector3(-EDGE, EDGE, 0.0f);
            this.vericesSkyBox[3].Color = Color.Green;
            this.vericesSkyBox[3].TextureCoordinate = new Vector2(max, min);
        }


        #region Her opprettes alle planetter og måner
        /// <summary>
        /// Her opprettes alle objekter som tilhører solarsystemmet
        /// </summary>
        private void initializeSolarSystemObjects()
        {
            //Sola
            this.Sol = new SpaceObject(this, 50f, 0, 0, 0, 0, 0.0f, null);
            this.Sol.isEmissive = true;
            this.Components.Add(this.Sol);

            //Planetter
            this.mercury = new SpaceObject(this, 0.1f, 300f, 1f, 0.75f, 0.0f, 1.0f, this.Sol);
            this.Components.Add(this.mercury);

            this.venus = new SpaceObject(this, 0.2f, 500f, 1f, -0.75f, 0.0f, 1.0f, this.Sol);
            this.Components.Add(this.venus);

            this.earth = new SpaceObject(this, 0.3f, 700f, 1f, 0.75f, 0.0f, 1.0f, this.Sol);
            this.Components.Add(this.earth);

            this.mars = new SpaceObject(this, 0.25f, 900f, 1f, 0.3f, 0, 1.0f, this.Sol);
            this.Components.Add(this.mars);

            this.jupiter = new SpaceObject(this, 0.6f, 1200f, 1f, 0.2f, 0, 1.0f, this.Sol);
            this.Components.Add(this.jupiter);

            this.saturn = new SpaceObject(this, 0.5f, 1500f, 1f, 0.7f, 0, 1.0f, this.Sol);
            this.Components.Add(this.saturn);

            this.saturnRing = new SpaceObject(this, 5.5f, 0f, 0f, 0.0f, 0, 0.0f, this.saturn);
            this.Components.Add(this.saturnRing);

            this.uran = new SpaceObject(this, 0.45f, 1700f, 1f, 1f, 0, 1.0f, this.Sol);
            this.Components.Add(this.uran);

            this.neptun = new SpaceObject(this, 0.4f, 2100f, 1f, 0.2f, 0, 1.0f, this.Sol);
            this.Components.Add(this.neptun);

            this.pluto = new SpaceObject(this, 0.05f, 2400f, 1f, 0.1f, 0, 1.0f, this.Sol);
            this.Components.Add(this.pluto);


            //satellitter

            //earth
            this.moon = new SpaceObject(this, 0.1f, 100f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, this.earth);
            this.Components.Add(this.moon);
            //mars
            this.fobos = new SpaceObject(this, 0.1f, 100f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, this.mars);
            this.Components.Add(this.fobos);

            this.deimos = new SpaceObject(this, 0.12f, 130f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, this.mars);
            this.Components.Add(this.deimos);

            //jupiter
            //største satellitter
            this.Io = new SpaceObject(this, 0.1f, 100f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, (float)g.NextDouble() + 0.1f, this.jupiter);
            this.Components.Add(this.Io);

            this.Europa = new SpaceObject(this, 0.1f, 100f, (float)g.NextDouble(), (float)g.NextDouble(), (float)g.NextDouble(), (float)g.NextDouble(), this.jupiter);
            this.Components.Add(this.Europa);

            this.Ganymede = new SpaceObject(this, 0.1f, 100f, (float)g.NextDouble(), (float)g.NextDouble(), (float)g.NextDouble(), (float)g.NextDouble(), this.jupiter);
            this.Components.Add(this.Ganymede);

            this.Callisto = new SpaceObject(this, 0.1f, 100f, (float)g.NextDouble(), (float)g.NextDouble(), (float)g.NextDouble(), (float)g.NextDouble(), this.jupiter);
            this.Components.Add(this.Callisto);


            //små satellitter , antall 63
            this.smallSatelitesOfJupiter = new SpaceObject[63];
            //TODO add to components

            //Saturn
            //Største satellitter
            this.Mimas = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.saturn);
            this.Components.Add(this.Mimas);

            this.Enceladus = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.saturn);
            this.Components.Add(this.Enceladus);

            this.Tethys = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.saturn);
            this.Components.Add(this.Tethys);

            this.Dione = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.saturn);
            this.Components.Add(this.Dione);

            this.Rhea = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.saturn);
            this.Components.Add(this.Rhea);

            this.Titan = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.saturn);
            this.Components.Add(this.Titan);

            this.Iapetus = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.saturn);
            this.Components.Add(this.Iapetus);

            //små satellitter, antall 55
            this.smallSatelitesOfSaturn = new SpaceObject[55];
            //TODO add to components

            //Uran
            //Største satelitter
            this.Miranda = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.uran);
            this.Components.Add(this.Miranda);

            this.Ariel = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.uran);
            this.Components.Add(this.Ariel);

            this.Umbriel = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.uran);
            this.Components.Add(this.Umbriel);

            this.Titania = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.uran);
            this.Components.Add(this.Titania);

            this.Oberon = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.uran);
            this.Components.Add(this.Oberon);

            //små satellitter, antall 22
            this.smallSatelitesOfUranus = new SpaceObject[22];
            //TODO add to components


            //Neptun
            //Størst satellitt
            this.Triton = new SpaceObject(this, 0.1f, 100f, 1, 1, 1, 1.0f, this.neptun);
            this.Components.Add(this.Triton);

            //små satellitter, antall 12
            this.smallSatelitesOfNeptune = new SpaceObject[12];
            //TODO add to components

            //Asteroidbelt
            //this.asteroidBelt = new AsteroidBelt(this, 0.25f, 900f, 1f, 0.3f, 0, 1.0f, this.Sol);
            //this.Components.Add(asteroidBelt);
            astman = new AsteroidManager(this, 200, 200, 100);
        }
        #endregion

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            //
            loadSpaceObjects();

            IsMouseVisible = true;

            effect = Content.Load<Effect>("effects/effects");

            effectSky = Content.Load<Effect>("effects/effectsRiemersTut");

            //skybox mk3
            skyboxModel = LoadModel("textures-skybox/skybox2", out skyboxTextures);
            menu = new Menu(this);
            menu.LoadContent();

            explosionTexture = Content.Load<Texture2D>(@"textures-planets/sunmap");
            explosionEffect = Content.Load<Effect>(@"effects/Particle2");
            explosionEffect.CurrentTechnique = explosionEffect.Techniques["Technique1"];
            explosionEffect.Parameters["theTexture"].SetValue(explosionTexture);

            spaceShip = new SpaceShip(this);
        }

        /// <summary>
        /// Metoden som brukes til å laste ned skybox til programmet
        /// 
        /// </summary>
        /// <param name="assetName">Path til skybox model</param>
        /// <param name="textures"></param>
        /// <returns></returns>
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


        public void LoadShipModel(string assetName)
        {
            Model newModel = Content.Load<Model>(assetName);
            foreach (ModelMesh mesh in newModel.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = effect.Clone();
                }
                    
            }
            this.shipModel = newModel;
        }



        /// <summary>
        /// Laster alle objekter i Solarsystemmet
        /// </summary>
        private void loadSpaceObjects()
        {

            spaceObjectEffect = Content.Load<Effect>("effects/effectsRiemersTut");
            spaceObjectEffect.Parameters["xView"].SetValue(this.matrixView);
            spaceObjectEffect.Parameters["xProjection"].SetValue(this.matrixProjection);
            spaceObjectEffect.Parameters["xLightPos"].SetValue(new Vector3(0f,0f,0f));
            spaceObjectEffect.Parameters["xLightPower"].SetValue(0.99f);
            spaceObjectEffect.Parameters["xAmbient"].SetValue(0.1f);

            Model planet = Content.Load<Model>("models/planet");
            
            //Sola
            this.Sol.load(spaceObjectEffect.Clone(), planet, Content.Load<Texture2D>("textures-planets/sunmap"));
            
            //Planetter
            this.mercury.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/mercurymap"));
            this.venus.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/venusmap"));
            this.earth.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/earthmap1k"));
            this.mars.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/mars_1k_color"));
            this.jupiter.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/jupitermap"));
            this.saturn.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/saturnmap"));
            this.saturnRing.load(spaceObjectEffect, Content.Load<Model>("models/ring"), Content.Load<Texture2D>("textures-planets/saturnringcolor"));
            this.uran.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/uranusmap"));
            this.neptun.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/neptunemap"));
            this.pluto.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/plutomap1k"));
            //satellitter

            this.moon.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.fobos.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.deimos.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.Io.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.Europa.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.Ganymede.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.Callisto.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.Mimas.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.Enceladus.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.Dione.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.Tethys.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.Rhea.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.Titan.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.Iapetus.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.Miranda.load(spaceObjectEffect.Clone(), planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.Ariel.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.Umbriel.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.Titania.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.Oberon.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));
            this.Triton.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/moonmap"));

            //asteroidbelt
            //this.asteroidBelt.load(spaceObjectEffect, planet, Content.Load<Texture2D>("textures-planets/ASTEROIDS"));
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Delete))
            {
                cameraPosition = Vector3.Transform(cameraPosition - cameraTarget, Matrix.CreateRotationY(-0.01f)) + cameraTarget;
                //cameraPosition = new Vector3(cameraX, cameraY, cameraZ);
                Matrix.CreateLookAt(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out matrixView);
                cameraX = cameraPosition.X;
                cameraY = cameraPosition.Y;
                cameraZ = cameraPosition.Z;
                spaceObjectEffect.Parameters["xView"].SetValue(this.matrixView);
                basicEffect.View = matrixView; 
            }
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.PageDown))
            {
                cameraPosition = Vector3.Transform(cameraPosition - cameraTarget, Matrix.CreateRotationY(0.01f)) + cameraTarget;
                Matrix.CreateLookAt(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out matrixView);
                cameraX = cameraPosition.X;
                cameraY = cameraPosition.Y;
                cameraZ = cameraPosition.Z;
                spaceObjectEffect.Parameters["xView"].SetValue(this.matrixView);
                basicEffect.View = matrixView;

            }

            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Home))
            {
                cameraPosition = Vector3.Transform(cameraPosition - cameraTarget, Matrix.CreateRotationX(-0.01f)) + cameraTarget;
                //cameraPosition = new Vector3(cameraX, cameraY, cameraZ);
                Matrix.CreateLookAt(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out matrixView);
                cameraX = cameraPosition.X;
                cameraY = cameraPosition.Y;
                cameraZ = cameraPosition.Z;
                spaceObjectEffect.Parameters["xView"].SetValue(this.matrixView);
                basicEffect.View = matrixView;

            }
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.End))
            {
                cameraPosition = Vector3.Transform(cameraPosition - cameraTarget, Matrix.CreateRotationX(0.01f)) + cameraTarget;
                Matrix.CreateLookAt(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out matrixView);
                cameraX = cameraPosition.X;
                cameraY = cameraPosition.Y;
                cameraZ = cameraPosition.Z;
                spaceObjectEffect.Parameters["xView"].SetValue(this.matrixView);
                basicEffect.View = matrixView;

            }

            ///Zooming
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.PageUp))
            {
                cameraX += 1000.0001f;
                cameraY += 1000.0001f;
                cameraZ += 1000.0001f;
                //cameraPosition = Vector3.Transform(cameraPosition - cameraTarget, Matrix.CreateTranslation(cameraX, cameraY, cameraZ)) + cameraTarget;
                cameraPosition = new Vector3(cameraX, cameraY, cameraZ);
                Matrix.CreateLookAt(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out matrixView);
                
                spaceObjectEffect.Parameters["xView"].SetValue(this.matrixView);
                basicEffect.View = matrixView;

            }
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Insert))
            {
                cameraX -= 1000.0001f;
                cameraY -= 1000.0001f;
                cameraZ -= 1000.0001f;
                //cameraPosition = Vector3.Transform(cameraPosition - cameraTarget, Matrix.CreateTranslation(-cameraX, -cameraY, -cameraZ)) + cameraTarget;
                cameraPosition = new Vector3(cameraX, cameraY, cameraZ);
                Matrix.CreateLookAt(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out matrixView);
                spaceObjectEffect.Parameters["xView"].SetValue(this.matrixView);
                basicEffect.View = matrixView;

            }

            menu.Update(gameTime);
            CurrentGameState = (MainClass.GameState)menu.getCurrentGameState();
            //Console.WriteLine("MainClass gamestate: " + CurrentGameState.ToString());

            UpdateExplosions(gameTime);

            if (CurrentGameState == GameState.Playing)
            {
                spaceShip.update(gameTime);
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            //felles ting
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            rs.FillMode = FillMode.Solid;
            device.RasterizerState = rs;
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            menu.Draw(gameTime);
            
            //tegner koordinater
            if (CurrentGameState == GameState.Playing)
            {
                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    this.cAxes.draw(this.device);
                }
            }
            //tegner asteroidbelt
            this.astman.Draw(gameTime, spaceObjectEffect);

            //this.DrawSkybox();
            //this.DrawInfo(gameTime);


            //skybox ver3
            if (CurrentGameState == GameState.Playing)
            {
                DrawSkybox();
            }

            foreach (EffectPass pass in explosionEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //this.cAxes.draw(this.device);
                foreach (ParticleExplosion p in explosions)
                {
                    p.Draw(explosionEffect, camera);
                }

            }


            //tegner romferge
            //DrawSpaceshipModel();
            if (CurrentGameState == GameState.Playing)
            {
                spaceShip.draw();
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
                    effect.Parameters["xView"].SetValue(matrixView);
                    effect.Parameters["xProjection"].SetValue(matrixProjection);
                    effect.Parameters["xTexture"].SetValue(skyboxTextures[i++]);
                }
                mesh.Draw();
            }

            dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            device.DepthStencilState = dss;
        }


        /// <summary>
        /// 
        /// </summary>
        /*
        private void DrawSpaceshipModel()
        {
            if (CurrentGameState == GameState.Playing)
            {
                foreach (ModelMesh mesh in shipModel.Meshes)
                {
                    foreach (Effect e in mesh.Effects)
                    {
                        Matrix World = Matrix.Identity * Matrix.CreateScale(150f);
                        e.CurrentTechnique = e.Techniques["Textured"];
                        e.Parameters["xWorld"].SetValue(World);
                        e.Parameters["xView"].SetValue(matrixView);
                        e.Parameters["xProjection"].SetValue(matrixProjection);
                        e.Parameters["xTexture"].SetValue(Content.Load<Texture2D>("textures-planets/ASTEROIDS"));
                    }
                    mesh.Draw();
                }
            }

        }
        */
        #region Spritebatch
        private void DrawInfo(GameTime _gameTime)
        {
            #region view matrix test informasjon
            this.DrawOverlayText("View Matrix", 0 ,0);
            
            this.DrawOverlayText(matrixView.M11.ToString(), 0, 15);
            this.DrawOverlayText(matrixView.M12.ToString(), 100, 15);
            this.DrawOverlayText(matrixView.M13.ToString(), 200, 15);
            this.DrawOverlayText(matrixView.M14.ToString(), 300, 15);

            this.DrawOverlayText(matrixView.M21.ToString(), 0, 30);
            this.DrawOverlayText(matrixView.M22.ToString(), 100, 30);
            this.DrawOverlayText(matrixView.M23.ToString(), 200, 30);
            this.DrawOverlayText(matrixView.M24.ToString(), 300, 30);

            this.DrawOverlayText(matrixView.M31.ToString(), 0, 45);
            this.DrawOverlayText(matrixView.M32.ToString(), 100, 45);
            this.DrawOverlayText(matrixView.M33.ToString(), 200, 45);
            this.DrawOverlayText(matrixView.M34.ToString(), 300, 45);

            this.DrawOverlayText(matrixView.M41.ToString(), 0, 60);
            this.DrawOverlayText(matrixView.M42.ToString(), 100, 60);
            this.DrawOverlayText(matrixView.M43.ToString(), 200, 60);
            this.DrawOverlayText(matrixView.M44.ToString(), 300, 60);

            this.DrawOverlayText("xView", 0, 75);

            this.DrawOverlayText(effectSky.Parameters["xView"].GetValueMatrix().M11.ToString(), 0, 90);
            this.DrawOverlayText(effectSky.Parameters["xView"].GetValueMatrix().M12.ToString(), 100, 90);
            this.DrawOverlayText(effectSky.Parameters["xView"].GetValueMatrix().M13.ToString(), 200, 90);
            this.DrawOverlayText(effectSky.Parameters["xView"].GetValueMatrix().M14.ToString(), 300, 90);

            this.DrawOverlayText(effectSky.Parameters["xView"].GetValueMatrix().M21.ToString(), 0, 105);
            this.DrawOverlayText(effectSky.Parameters["xView"].GetValueMatrix().M22.ToString(), 100, 105);
            this.DrawOverlayText(effectSky.Parameters["xView"].GetValueMatrix().M23.ToString(), 200, 105);
            this.DrawOverlayText(effectSky.Parameters["xView"].GetValueMatrix().M24.ToString(), 300, 105);

            this.DrawOverlayText(effectSky.Parameters["xView"].GetValueMatrix().M31.ToString(), 0, 120);
            this.DrawOverlayText(effectSky.Parameters["xView"].GetValueMatrix().M32.ToString(), 100, 120);
            this.DrawOverlayText(effectSky.Parameters["xView"].GetValueMatrix().M33.ToString(), 200, 120);
            this.DrawOverlayText(effectSky.Parameters["xView"].GetValueMatrix().M34.ToString(), 300, 120);

            this.DrawOverlayText(effectSky.Parameters["xView"].GetValueMatrix().M41.ToString(), 0, 135);
            this.DrawOverlayText(effectSky.Parameters["xView"].GetValueMatrix().M42.ToString(), 100, 135);
            this.DrawOverlayText(effectSky.Parameters["xView"].GetValueMatrix().M43.ToString(), 200, 135);
            this.DrawOverlayText(effectSky.Parameters["xView"].GetValueMatrix().M44.ToString(), 300, 135);
            #endregion
        }

        private void DrawOverlayText(String _text, int _x, int _y)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, _text, new Vector2(_x, _y), Color.White);

            spriteBatch.End();
        }//end of DrawOverlayText
        #endregion

        /// <summary>
        /// returnerer effect
        /// </summary>
        /// <returns></returns>
        public Effect getEffect()
        {
            return spaceObjectEffect;
        }

    }
}
