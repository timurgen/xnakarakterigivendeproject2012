using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace ProjectFinal
{
    public class Menu
    {
        //Variabler
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont, spriteFontAbout;

        ContentManager Content;

        Model model;

        //Viser tilstand som spill tilhører
        public enum GameState
        {
            MainMenu,
            About,
            Playing,
            Ship,
            Info,
            Keyboard,
        }

        //romskip typer
        public enum ShipType
        { 
            ShipOne,
            ShipTo,
            ShipThree,
        }

        //Knapper
        MyButton buttonPlay;
        MyButton buttonAbout;
        MyButton buttonExit;
        MyButton buttonBack;

        MyButton buttonSpaceOne;
        MyButton buttonSpaceTwo;
        MyButton buttonSpaceThree;

        MyButton buttonKeyboard;

        private MainClass game;

        //Spill tilstand og romskip type
        public GameState CurrentGameState = GameState.MainMenu;
        public ShipType CurrenShipType = ShipType.ShipOne;

        //Konstruktør
        public Menu(Game _game)
        {
            game = (MainClass)_game;
            graphics = game.graphics;
            spriteBatch = game.spriteBatch;
            spriteFont = game.Content.Load<SpriteFont>(@"Fonts\MenuFont");
            spriteFontAbout = game.Content.Load<SpriteFont>(@"Fonts\About");
            Content = new ContentManager(game.Services);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// laste spill kontent
        /// </summary>
        public void LoadContent()
        {
            //teksturer til knapper
            Texture2D buttonPlayTexture = Content.Load<Texture2D>("textures-menu/Play");
            Texture2D buttonAboutTexture = Content.Load<Texture2D>("textures-menu/About");
            Texture2D buttonExitTexture = Content.Load<Texture2D>("textures-menu/Exit");
            Texture2D buttonBackTexture = Content.Load<Texture2D>("textures-menu/back");

            Texture2D buttonSpaceOneTexture = Content.Load<Texture2D>("textures-menu/shipmk2");
            Texture2D buttonSpaceTwoTexture = Content.Load<Texture2D>("textures-menu/spacetwo");
            Texture2D buttonSpaceThreeTexture = Content.Load<Texture2D>("textures-menu/spacethree");

            Texture2D buttonKeyboardTexture = Content.Load<Texture2D>("textures-menu/info");

            //Knapper inisialisasjon
            buttonPlay = new MyButton(buttonPlayTexture, graphics.GraphicsDevice, new Vector2(500, 100));
            buttonPlay.setPosition(new Vector2(400, 500));

            buttonAbout = new MyButton(buttonAboutTexture, graphics.GraphicsDevice, new Vector2(500, 100));
            buttonAbout.setPosition(new Vector2(400, 600));

            buttonExit = new MyButton(buttonExitTexture, graphics.GraphicsDevice, new Vector2(500, 100));
            buttonExit.setPosition(new Vector2(400, 700));

            buttonBack = new MyButton(buttonBackTexture, graphics.GraphicsDevice, new Vector2(500, 100));
            buttonBack.setPosition(new Vector2(900, 700));

            buttonSpaceOne = new MyButton(buttonSpaceOneTexture, graphics.GraphicsDevice, new Vector2(360, 260));
            buttonSpaceOne.setPosition(new Vector2(100, 300));

            buttonSpaceTwo = new MyButton(buttonSpaceTwoTexture, graphics.GraphicsDevice, new Vector2(360, 260));
            buttonSpaceTwo.setPosition(new Vector2(460, 300));

            buttonSpaceThree = new MyButton(buttonSpaceThreeTexture, graphics.GraphicsDevice, new Vector2(360, 260));
            buttonSpaceThree.setPosition(new Vector2(820, 300));

            buttonKeyboard = new MyButton(buttonKeyboardTexture, graphics.GraphicsDevice, new Vector2(128, 128));
            buttonKeyboard.setPosition(new Vector2(0, 0));
        }

        /// <summary>
        /// Oppdatering metode
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //mouse listener
            MouseState mouse = Mouse.GetState();

            //Sjekker spill tilstand
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    if (buttonPlay.isClicked == true)
                    {
                        CurrentGameState = GameState.Ship;
                        buttonPlay.isClicked = false;
                    }
                    if (buttonExit.isClicked == true)
                        game.Exit();
                    if (buttonAbout.isClicked == true)
                    {
                        CurrentGameState = GameState.About;
                        buttonAbout.isClicked = false;
                    }
                    if (buttonKeyboard.isClicked == true)
                    {
                        CurrentGameState = GameState.Keyboard;
                        buttonKeyboard.isClicked = false;
                    }

                    buttonPlay.Update(mouse);
                    buttonAbout.Update(mouse);
                    buttonKeyboard.Update(mouse);
                    buttonExit.Update(mouse);

                    break;

                case GameState.Ship:

                    buttonBack.Update(mouse);

                    buttonSpaceOne.Update(mouse);
                    buttonSpaceTwo.Update(mouse);
                    buttonSpaceThree.Update(mouse);

                    if (buttonBack.isClicked == true)
                    {
                        buttonBack.isClicked = false;
                        CurrentGameState = GameState.MainMenu;
                    }

                    //Skekker romskip model
                    if (buttonSpaceOne.isClicked == true)
                    {
                        //game.spaceShip.model = game.Content.Load<Model>(@"models/testa1");
                        model = game.LoadModelWithBoundingSphere(@"models/testa1", ref  game.spaceShip.matrixBoneTr, ref game.spaceShip.matrixOriginBoneTr);
                        game.spaceShip.load(game.effect, model, Content.Load<Texture2D>("textures-planets/ship"));
                        //game.spaceShip.load(game.effect, game.Content.Load<Model>(@"models/testa1"), Content.Load<Texture2D>("textures-planets/ship"));
                        //Console.WriteLine("spaceone");
                        buttonSpaceOne.isClicked = false;
                        CurrenShipType = ShipType.ShipOne;
                        CurrentGameState = GameState.Playing;
                        //CurrentGameState = GameState.MainMenu;

                    }
                    if (buttonSpaceTwo.isClicked == true)
                    {
                        //game.spaceShip.model = game.Content.Load<Model>(@"models/spaceto");
                        model = game.LoadModelWithBoundingSphere(@"models/spaceto", ref  game.spaceShip.matrixBoneTr, ref game.spaceShip.matrixOriginBoneTr);
                        game.spaceShip.load(game.effect, model, Content.Load<Texture2D>("textures-planets/ship"));
                        //Console.WriteLine("SpaceTwo");
                        buttonSpaceTwo.isClicked = false;
                        CurrenShipType = ShipType.ShipTo;
                        CurrentGameState = GameState.Playing;
                        //CurrentGameState = GameState.MainMenu;


                    }
                    if (buttonSpaceThree.isClicked == true)
                    {
                        //game.spaceShip.model = game.Content.Load<Model>(@"models/spaceship33");
                        model = game.LoadModelWithBoundingSphere(@"models/spaceship33", ref  game.spaceShip.matrixBoneTr, ref game.spaceShip.matrixOriginBoneTr);
                        game.spaceShip.load(game.effect, model, Content.Load<Texture2D>("textures-planets/ship"));
                        //Console.WriteLine("SpaceThree");
                        buttonSpaceThree.isClicked = false;
                        CurrenShipType = ShipType.ShipThree;
                        CurrentGameState = GameState.Playing;
                        //CurrentGameState = GameState.MainMenu;


                    }


                    break;

                case GameState.About:
                    buttonBack.Update(mouse);
                    if (buttonBack.isClicked == true)
                    {
                        buttonBack.isClicked = false;
                        CurrentGameState = GameState.MainMenu;
                    }
                    break;

                case GameState.Playing:
                    break;

                case GameState.Info:
                    if (game.spaceShip.collisionName == null)
                    {
                        CurrentGameState = GameState.Playing;
                    }
                    else
                    {
                        buttonBack.Update(mouse);
                        if (buttonBack.isClicked == true)
                        {
                            buttonBack.isClicked = false;
                            CurrentGameState = GameState.Playing;
                        }
                    }
                    break;
                case GameState.Keyboard:
                     buttonBack.Update(mouse);
                    if (buttonBack.isClicked == true)
                    {
                        buttonBack.isClicked = false;
                        CurrentGameState = GameState.MainMenu;
                    }
                    break;


            }

            //Console.WriteLine("Menu gamestate: " + CurrentGameState);
        }

        /// <summary>
        /// Tegne metode
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("textures-menu/Menu"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    //spriteBatch.Draw(Content.Load<Texture2D>("GameLogo"), new Rectangle(190, 10, 900, 200), Color.White);
                    spriteBatch.DrawString(spriteFont, "Solar System", new Vector2(270, 10), Color.PaleVioletRed);
                    buttonPlay.Draw(spriteBatch);
                    buttonAbout.Draw(spriteBatch);

                    buttonKeyboard.Draw(spriteBatch);

                    buttonExit.Draw(spriteBatch);
                    break;

                case GameState.Ship:
                    spriteBatch.Draw(Content.Load<Texture2D>("textures-menu/Menu"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.DrawString(spriteFont, "Solar System", new Vector2(270, 10), Color.PaleVioletRed);

                    buttonSpaceOne.Draw(spriteBatch);
                    buttonSpaceTwo.Draw(spriteBatch);
                    buttonSpaceThree.Draw(spriteBatch);

                    buttonBack.Draw(spriteBatch);
                    break;

                case GameState.About:
                    spriteBatch.Draw(Content.Load<Texture2D>("textures-menu/Menu"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.DrawString(spriteFont, "Solar System", new Vector2(270, 10), Color.PaleVioletRed);

                    spriteBatch.DrawString(spriteFontAbout, "Obligatorisk og karaktergivende oppgave nr 1", new Vector2(290, 300), Color.Green);
                    spriteBatch.DrawString(spriteFontAbout, "Datamaskingrafikk", new Vector2(500, 350), Color.Green);
                    spriteBatch.DrawString(spriteFontAbout, "Hoegskolen i Narvik, Hoesten 2012", new Vector2(380, 400), Color.Green);

                    buttonBack.Draw(spriteBatch);
                    break;

                case GameState.Playing:
                    break;

                case GameState.Info:
                    if (game.spaceShip.collisionName.Equals("Sol"))
                        spriteBatch.Draw(Content.Load<Texture2D>("textures-planets/suninfo"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    else if (game.spaceShip.collisionName.Equals("mercury"))
                        spriteBatch.Draw(Content.Load<Texture2D>("textures-planets/mercuryinfo"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    else if (game.spaceShip.collisionName.Equals("venus"))
                        spriteBatch.Draw(Content.Load<Texture2D>("textures-planets/venusinfo"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    else if (game.spaceShip.collisionName.Equals("earth"))
                        spriteBatch.Draw(Content.Load<Texture2D>("textures-planets/earthinfo"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    else if (game.spaceShip.collisionName.Equals("mars"))
                        spriteBatch.Draw(Content.Load<Texture2D>("textures-planets/marsinfo"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    else if (game.spaceShip.collisionName.Equals("jupiter"))
                        spriteBatch.Draw(Content.Load<Texture2D>("textures-planets/jupiterinfo"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    else if (game.spaceShip.collisionName.Equals("saturn"))
                        spriteBatch.Draw(Content.Load<Texture2D>("textures-planets/saturninfo"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    else if (game.spaceShip.collisionName.Equals("uran"))
                        spriteBatch.Draw(Content.Load<Texture2D>("textures-planets/uranusinfo"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    else if (game.spaceShip.collisionName.Equals("neptun"))
                        spriteBatch.Draw(Content.Load<Texture2D>("textures-planets/neptuneinfo"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    else if (game.spaceShip.collisionName.Equals("pluto"))
                        spriteBatch.Draw(Content.Load<Texture2D>("textures-planets/plutoinfo"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    else if (game.spaceShip.collisionName.Equals("saturnring"))
                        spriteBatch.Draw(Content.Load<Texture2D>("textures-planets/paaskeegg"), new Rectangle(331, 196, 618, 407), Color.White);

                        buttonBack.Draw(spriteBatch);
                    break;

                case GameState.Keyboard:
                    spriteBatch.Draw(Content.Load<Texture2D>("textures-menu/Menu"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                    spriteBatch.DrawString(spriteFontAbout, "Controls:", new Vector2(550, 300), Color.Green);
                    spriteBatch.DrawString(spriteFontAbout, "W - akselererende fremover", new Vector2(400, 350), Color.Green);
                    spriteBatch.DrawString(spriteFontAbout, "S - stoppe", new Vector2(400, 400), Color.Green);
                    spriteBatch.DrawString(spriteFontAbout, "X - akselerasjon siden", new Vector2(400, 450), Color.Green);
                    spriteBatch.DrawString(spriteFontAbout, "Up - fly ned", new Vector2(400, 500), Color.Green);
                    spriteBatch.DrawString(spriteFontAbout, "Down - fly opp", new Vector2(400, 550), Color.Green);
                    spriteBatch.DrawString(spriteFontAbout, "Left - fly til venstre", new Vector2(400, 600), Color.Green);
                    spriteBatch.DrawString(spriteFontAbout, "Right - fly til hoeyre", new Vector2(400, 650), Color.Green);

                    buttonBack.Draw(spriteBatch);
                    break;
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Returnenerer spill tilstand
        /// </summary>
        /// <returns></returns>
        public GameState getCurrentGameState()
        {
            return CurrentGameState;
        }

    }
}
