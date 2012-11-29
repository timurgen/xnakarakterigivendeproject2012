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
    class Menu
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        ContentManager Content;

        public enum GameState
        {
            MainMenu,
            About,
            Playing,
            Ship,
        }

        MyButton buttonPlay;
        MyButton buttonAbout;
        MyButton buttonExit;
        MyButton buttonBack;

        MyButton buttonSpaceOne;
        MyButton buttonSpaceTwo;
        MyButton buttonSpaceThree;

        private MainClass game;

        public GameState CurrentGameState = GameState.MainMenu;

        public Menu(Game _game)
        {
            game = (MainClass)_game;
            graphics = game.graphics;
            spriteBatch = game.spriteBatch;
            spriteFont = game.Content.Load<SpriteFont>(@"Fonts\MenuFont");
            Content = new ContentManager(game.Services);
            Content.RootDirectory = "Content";
        }

        public void LoadContent()
        {
            Texture2D buttonPlayTexture = Content.Load<Texture2D>("textures-menu/Play");
            Texture2D buttonAboutTexture = Content.Load<Texture2D>("textures-menu/About");
            Texture2D buttonExitTexture = Content.Load<Texture2D>("textures-menu/Exit");
            Texture2D buttonBackTexture = Content.Load<Texture2D>("textures-menu/back");

            Texture2D buttonSpaceOneTexture = Content.Load<Texture2D>("textures-menu/spaceone");
            Texture2D buttonSpaceTwoTexture = Content.Load<Texture2D>("textures-menu/spacetwo");
            Texture2D buttonSpaceThreeTexture = Content.Load<Texture2D>("textures-menu/spacethree");

            buttonPlay = new MyButton(buttonPlayTexture, graphics.GraphicsDevice, new Vector2(500, 100));
            buttonPlay.setPosition(new Vector2(570, 500));

            buttonAbout = new MyButton(buttonAboutTexture, graphics.GraphicsDevice, new Vector2(500, 100));
            buttonAbout.setPosition(new Vector2(570, 600));

            buttonExit = new MyButton(buttonExitTexture, graphics.GraphicsDevice, new Vector2(500, 100));
            buttonExit.setPosition(new Vector2(570, 700));

            buttonBack = new MyButton(buttonBackTexture, graphics.GraphicsDevice, new Vector2(500, 100));
            buttonBack.setPosition(new Vector2(900, 700));

            buttonSpaceOne = new MyButton(buttonSpaceOneTexture, graphics.GraphicsDevice, new Vector2(360, 260));
            buttonSpaceOne.setPosition(new Vector2(100, 300));

            buttonSpaceTwo = new MyButton(buttonSpaceTwoTexture, graphics.GraphicsDevice, new Vector2(360, 260));
            buttonSpaceTwo.setPosition(new Vector2(460, 300));

            buttonSpaceThree = new MyButton(buttonSpaceThreeTexture, graphics.GraphicsDevice, new Vector2(360, 260));
            buttonSpaceThree.setPosition(new Vector2(820, 300));
        }

        public void Update(GameTime gameTime)
        {

            MouseState mouse = Mouse.GetState();

            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    if (buttonPlay.isClicked == true)
                        CurrentGameState = GameState.Ship;
                    if (buttonExit.isClicked == true)
                        game.Exit();
                    buttonPlay.Update(mouse);
                    buttonAbout.Update(mouse);
                    buttonExit.Update(mouse);

                    break;

                case GameState.Ship:
                    if (buttonBack.isClicked == true)
                        CurrentGameState = GameState.MainMenu;

                    if (buttonSpaceOne.isClicked == true)
                    {
                        game.LoadShipModel("models/SpaceOne");
                        Console.WriteLine("spaceone");
                        buttonSpaceOne.isClicked = false;
                        CurrentGameState = GameState.Playing;
                        //CurrentGameState = GameState.MainMenu;
                        
                    }
                    if (buttonSpaceTwo.isClicked == true)
                    {
                        game.LoadShipModel("models/SpaceOne");
                        Console.WriteLine("SpaceTwo");
                        buttonSpaceTwo.isClicked = false;
                        CurrentGameState = GameState.Playing;
                        //CurrentGameState = GameState.MainMenu;
                        

                    }
                    if (buttonSpaceThree.isClicked == true)
                    {
                        game.LoadShipModel("models/SpaceOne");
                        Console.WriteLine("SpaceThree");
                        buttonSpaceThree.isClicked = false;
                        CurrentGameState = GameState.Playing;
                        //CurrentGameState = GameState.MainMenu;
                       

                    }

                    buttonBack.Update(mouse);

                    buttonSpaceOne.Update(mouse);
                    buttonSpaceTwo.Update(mouse);
                    buttonSpaceThree.Update(mouse);
                    break;

                case GameState.About:
                    buttonPlay.Update(mouse);
                    buttonAbout.Update(mouse);
                    buttonExit.Update(mouse);
                    break;

                case GameState.Playing:

                    break;

                    
            }

            Console.WriteLine("Menu gamestate: " + CurrentGameState);
        }

        public void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("textures-menu/Menu"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    //spriteBatch.Draw(Content.Load<Texture2D>("GameLogo"), new Rectangle(190, 10, 900, 200), Color.White);
                    spriteBatch.DrawString(spriteFont, "Solar System", new Vector2(190, 10), Color.PaleVioletRed);
                    buttonPlay.Draw(spriteBatch);
                    buttonAbout.Draw(spriteBatch);
                    buttonExit.Draw(spriteBatch);
                    break;

                case GameState.Ship:
                    spriteBatch.Draw(Content.Load<Texture2D>("textures-menu/Menu"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.DrawString(spriteFont, "Solar System", new Vector2(190, 10), Color.PaleVioletRed);

                    buttonSpaceOne.Draw(spriteBatch);
                    buttonSpaceTwo.Draw(spriteBatch);
                    buttonSpaceThree.Draw(spriteBatch);

                    buttonBack.Draw(spriteBatch);
                    break;

                case GameState.About:

                    break;

                case GameState.Playing:

                    break;
            }

            spriteBatch.End();
        }

        public GameState getCurrentGameState()
        {
            return CurrentGameState;
        }

    }
}
