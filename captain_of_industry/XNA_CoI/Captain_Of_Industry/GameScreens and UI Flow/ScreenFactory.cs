//style from game state management samples.
//http://xbox.create.msdn.com/en-US/education/catalog/sample/game_state_management

using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Captain_Of_Industry
{
    //an enum of the type of screens to make.
    public enum Screen
    { MAIN_MENU = 0, GAME, PAUSE, GAME_OVER_LOSS, GAME_OVER_WIN, CREDITS, SCREEN_COUNT };

    //creates a screen when given a type to do so.
    //KW --> do we want it to read different screens in from an XML file? 
    //Game is small enough to probably not need it.
    class ScreenFactory
    {
        public GameScreen Create(Screen _type)
        {
            GameScreen temp;

            switch (_type)
            {
                case Screen.MAIN_MENU:
                    temp = new MainMenuScreen();
                    break;
                case Screen.CREDITS:
                    temp = new CreditScreen();
                    break;
                case Screen.GAME:
                    temp = new MainMenuScreen();
                    break;
                case Screen.PAUSE:
                    temp = new PauseScreen();
                    break;
                case Screen.GAME_OVER_LOSS:
                case Screen.GAME_OVER_WIN:
                    temp = new GameOverScreen();
                    break;
                default:
                    temp = null;
                    break;
            }

            return temp;
        }
    }
}

//KW
