using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

namespace Captain_Of_Industry
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        AudioManager audioManager;
        public static Rectangle screenRect;

        SpriteBatch spriteBatch;
        ObjectFactory objectFactory;

        // A random number generator
        Random random;

        // The font used to display UI elements
        SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
        
            Content.RootDirectory = "Content";
        
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;

            device = graphics.GraphicsDevice;

            screenRect = new Rectangle(0, 0, 768, 1024);

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Enable the FreeDrag gesture.
            TouchPanel.EnabledGestures = GestureType.FreeDrag;

            audioManager = new AudioManager(Content);
            // Initialize our random number generator
            random = new Random();
            objectFactory = new ObjectFactory();
            objectFactory.Initialize();

            InputManager.Init();

            base.Initialize();
        }

        // Called once to load in assets
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the score font
            font = Content.Load<SpriteFont>("gameFont");

            audioManager.LoadContent();

            objectFactory.LoadContent(Content);
            objectFactory.SetupLevel();
            audioManager.PlayMusic("bgAudio", true);
        }
        /*
        private void PlayMusic(Song song)
        {
            // Due to the way the MediaPlayer plays music,
            // we have to catch the exception. Music will play when the game is not tethered
            try
            {
                // Play the music
                MediaPlayer.Play(song);

                // Loop the currently playing song
                MediaPlayer.IsRepeating = true;
            }
            catch { }
        }
        */
        // Unload all content from content manager
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        // Update all entities in the world based on time since last update
        // <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime _gameTime)
        {
            InputManager.Update(_gameTime);

            objectFactory.UpdateCollision(graphics.GraphicsDevice, spriteBatch, true);
            objectFactory.UpdateAll(_gameTime);

            base.Update(_gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime _gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Start drawing
            spriteBatch.Begin();

            objectFactory.RenderAll(spriteBatch);

            // Define area for drawing text to
            Vector2 tempUIPOS = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y - 5);
            tempUIPOS.X += GraphicsDevice.Viewport.Width / 12;
            // Draw player health
            spriteBatch.DrawString(font, "Health: " + objectFactory.GetPlayerHealth(), tempUIPOS, Color.White);
            // Draw player sanity
            tempUIPOS.X += GraphicsDevice.Viewport.Width / 3;
            spriteBatch.DrawString(font, "Sanity: " + objectFactory.GetPlayerSanity(), tempUIPOS, Color.White);
            // Draw player wealth
            tempUIPOS.X += GraphicsDevice.Viewport.Width / 3;
            spriteBatch.DrawString(font, "Wealth: " + objectFactory.GetPlayerWealth(), tempUIPOS, Color.White);

            //Stop drawing
            spriteBatch.End();

            base.Draw(_gameTime);
        }
    }
}
