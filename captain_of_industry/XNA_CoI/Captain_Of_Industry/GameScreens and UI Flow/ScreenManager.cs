//style from game state management samples.
//http://xbox.create.msdn.com/en-US/education/catalog/sample/game_state_management

using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.IO;
using System.IO.IsolatedStorage;

namespace Captain_Of_Industry
{
    //manages as many screens as we need, enables easy transitioning, updating, drawing, etc. 
    //tl, dr; a manager, as you'd expect.
    //KW --> May want to extend from DrawableGameComponent? Look into/discuss Tuesday.
    class ScreenManager
    {
        private Game game;
        private ScreenFactory factory;

        //master list of screens in game.
        private List<GameScreen> screens = new List<GameScreen>();
        //basically refers to anything that is active at this point and worth updating.
        private List<GameScreen> shortList = new List<GameScreen>();

        //KW --> needs a reference to input manager in the main. 
        //KW --> if we go xml, needs reference to resource file of screens.

        //in place so fonts/etc are common to screens (unless we want it otherwise?)
        private SpriteBatch spriteBatch;
        public SpriteBatch SpriteBatch
        { get { return spriteBatch; } }

        private SpriteFont font;
        public SpriteFont SpriteFont
        { get { return font; } }

        private Texture2D emptyTexture;

        //kW --> needs a reference either to the game it's in,
        //or to a couple different things in the game. Likely refer to Game1 or whatever.
        public ScreenManager(Game _game)
        {
            game = _game;
            
            //KW --> needs filling once we decide on xml/not and our screens.
            Init();
        }

        public void Init()
        {
            //do some stuff -> create and add all necessary screens to the master list.
            screens.Add(factory.Create(Screen.MAIN_MENU));
            screens.Add(factory.Create(Screen.CREDITS));
            screens.Add(factory.Create(Screen.GAME));
            screens.Add(factory.Create(Screen.PAUSE));
            screens.Add(factory.Create(Screen.GAME_OVER_LOSS));
        }

        //get content into screen manager.
        protected void LoadContent()
        {
            //either from xml or manually here.
        }

        //release content from screenmanager.
        protected void UnloadContent()
        {
        }

        public void Update(GameTime _dt)
        {  
            //update/check for input.

            shortList.Clear();
            foreach (GameScreen screen in screens)
            {
                shortList.Add(screen);
            }

            bool otherHasFocus = !game.IsActive;
            bool coveredByOtherScreen = false;

            while (shortList.Count > 0)
            {
                GameScreen temp = shortList[shortList.Count - 1];
                shortList.RemoveAt(shortList.Count - 1);

                //update that screen, removei t from the short list.
                temp.Update(_dt, otherHasFocus, coveredByOtherScreen);

                if ((temp.TransitionState == TransitionState.TRANSITION_ON) || (temp.TransitionState == TransitionState.ACTIVE))
                {
                    //let first screen handle input as needed.
                    if (!otherHasFocus)
                    {
                        //handle input. 
                        otherHasFocus = true;
                    }
                }
            }
        }

        //render any visible screens.
        public void Draw(GameTime _dt)
        {
            foreach (GameScreen screen in screens)
            {
                if (screen.TransitionState == TransitionState.HIDDEN)
                { continue; }

                screen.Render(_dt);
            }
        }

        //adding/removing screens.
        public void AddScreen(GameScreen _screen)
        {
            screens.Add(_screen);
        }

        public void RemoveScreen(GameScreen _screen)
        {
            //should unload content from screen first
            _screen.Unload();

            screens.Remove(_screen);
            shortList.Remove(_screen);
        }

        //returns copy of list of screens.
        public GameScreen[] GetScreenListAsArray()
        { return screens.ToArray(); }

        //Draws a black sprite for fading between screens. 
        public void FadeBackBufferToBlack(float alpha)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(emptyTexture, Game1.screenRect, Color.Black * alpha);
            spriteBatch.End();
        }

        //futureproofing for saving, assuming we do it this way.
        //refers to xml -> used for saving.
        //KW --> not guaranteed, depends on saving.
        //public bool Activate(bool instancePreserved)
        //{
        //    // If the game instance was preserved, the game wasn't dehydrated so our screens still exist.
        //    // We just need to activate them and we're ready to go.
        //    if (instancePreserved)
        //    {
        //        // Make a copy of the master screen list, to avoid confusion if
        //        // the process of activating one screen adds or removes others.
        //        tempScreensList.Clear();

        //        foreach (GameScreen screen in screens)
        //            tempScreensList.Add(screen);

        //        foreach (GameScreen screen in tempScreensList)
        //            screen.Activate(true);
        //    }

        //    // Otherwise we need to refer to our saved file and reconstruct the screens that were present
        //    // when the game was deactivated.
        //    else
        //    {
        //        // Try to get the screen factory from the services, which is required to recreate the screens
        //        IScreenFactory screenFactory = Game.Services.GetService(typeof(IScreenFactory)) as IScreenFactory;
        //        if (screenFactory == null)
        //        {
        //            throw new InvalidOperationException(
        //                "Game.Services must contain an IScreenFactory in order to activate the ScreenManager.");
        //        }

        //        // Open up isolated storage
        //        using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
        //        {
        //            // Check for the file; if it doesn't exist we can't restore state
        //            if (!storage.FileExists(StateFilename))
        //                return false;

        //            // Read the state file so we can build up our screens
        //            using (IsolatedStorageFileStream stream = storage.OpenFile(StateFilename, FileMode.Open))
        //            {
        //                XDocument doc = XDocument.Load(stream);

        //                // Iterate the document to recreate the screen stack
        //                foreach (XElement screenElem in doc.Root.Elements("GameScreen"))
        //                {
        //                    // Use the factory to create the screen
        //                    Type screenType = Type.GetType(screenElem.Attribute("Type").Value);
        //                    GameScreen screen = screenFactory.CreateScreen(screenType);

        //                    // Rehydrate the controlling player for the screen
        //                    PlayerIndex? controllingPlayer = screenElem.Attribute("ControllingPlayer").Value != ""
        //                        ? (PlayerIndex)Enum.Parse(typeof(PlayerIndex), screenElem.Attribute("ControllingPlayer").Value, true)
        //                        : (PlayerIndex?)null;
        //                    screen.ControllingPlayer = controllingPlayer;

        //                    // Add the screen to the screens list and activate the screen
        //                    screen.ScreenManager = this;
        //                    screens.Add(screen);
        //                    screen.Activate(false);

        //                    // update the TouchPanel to respond to gestures this screen is interested in
        //                    TouchPanel.EnabledGestures = screen.EnabledGestures;
        //                }
        //            }
        //        }
        //    }
        //    return true;
        //}
    }
}

//KW --> needs LoadContent if xml read-in.
//KW --> do we care to serialize our screens for whatever reason? seems unnec/massive stretch.

//KW