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

    public class MainClass : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;



        /*********************************************************/
        /*Objekter i rommet*/


        //Sola
        SpaceObject Sol;

        //Planetter
        SpaceObject mercury, venus, earth, mars, jupiter, saturn, uran, neptun, pluto;

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
        /*********************************************************/

        //skybox
        private VertexPositionColorTexture[] vericesSkyBox;

        private const float BOUNDARY = 80000.0f;
        private const float EDGE = BOUNDARY * 2.0f;

        //textures
        private Texture2D skbxFront;
        private Texture2D skbxLeft;
        private Texture2D skbxRight;
        private Texture2D skbxTop;
        private Texture2D skbxBottom;
        private Texture2D skbxBack;




        public MainClass()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            initializeSolarSystemObjects();
            initializeSkyBox();
            base.Initialize();
        }

        private void initializeSkyBox()
        {
            const float min = 0.003f;
            const float max = 0.997f;

            this.vericesSkyBox = new VertexPositionColorTexture[4];

            this.vericesSkyBox[0].Position = new Vector3(EDGE, -EDGE, 0.0f);
            this.vericesSkyBox[0].Color = Color.White;
            this.vericesSkyBox[0].TextureCoordinate = new Vector2(min, max);

            this.vericesSkyBox[1].Position = new Vector3(EDGE, EDGE, 0.0f);
            this.vericesSkyBox[1].Color = Color.White;
            this.vericesSkyBox[1].TextureCoordinate = new Vector2(min, min);

            this.vericesSkyBox[2].Position = new Vector3(-EDGE, -EDGE, 0.0f);
            this.vericesSkyBox[2].Color = Color.White;
            this.vericesSkyBox[2].TextureCoordinate = new Vector2(max, max);

            this.vericesSkyBox[3].Position = new Vector3(-EDGE, EDGE, 0.0f);
            this.vericesSkyBox[3].Color = Color.White;
            this.vericesSkyBox[3].TextureCoordinate = new Vector2(max, min);
        }


        private void initializeSolarSystemObjects()
        {
            //Sola
            this.Sol = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, null);

            //Planetter
            this.mercury = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.Sol);
            this.venus = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.Sol);
            this.earth = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.Sol);
            this.mars = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.Sol);
            this.jupiter = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.Sol);
            this.saturn = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.Sol);
            this.uran = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.Sol);
            this.neptun = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.Sol);
            this.pluto = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.Sol);

            //satellitter

            //earth
            this.moon = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.earth);
            //mars
            this.fobos = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.mars);
            this.deimos = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.mars);

            //jupiter
            //største satellitter
            this.Io = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.jupiter);
            this.Europa = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.jupiter);
            this.Ganymede = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.jupiter);
            this.Callisto = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.jupiter);
            //små satellitter , antall 63
            this.smallSatelitesOfJupiter = new SpaceObject[63];

            //Saturn
            //Største satellitter
            this.Mimas = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.saturn);
            this.Enceladus = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.saturn);
            this.Tethys = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.saturn);
            this.Dione = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.saturn);
            this.Rhea = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.saturn);
            this.Titan = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.saturn);
            this.Iapetus = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.saturn);
            //små satellitter, antall 55
            this.smallSatelitesOfSaturn = new SpaceObject[55];

            //Uran
            //Største satelitter
            this.Miranda = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.uran);
            this.Ariel = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.uran);
            this.Umbriel = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.uran);
            this.Titania = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.uran);
            this.Oberon = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.uran);
            //små satellitter, antall 22
            this.smallSatelitesOfUranus = new SpaceObject[22];

            //Neptun
            //Størst satellitt
            this.Triton = new SpaceObject(this, 10f, 0, 0, 0, 0, 1.0f, this.neptun);
            //små satellitter, antall 12
            this.smallSatelitesOfNeptune = new SpaceObject[12];


        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

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
