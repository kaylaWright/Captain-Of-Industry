using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Captain_Of_Industry
{
    static public class InputManager
    {
        // Enum of input actions
        public enum IA
        {
            UP = 0, 
            DOWN, 
            LEFT, 
            RIGHT, 
            USE_INTERACT,
            CANCEL,
            TOGGLE_INVENTORY,
            ACTIONS_COUNT
        };

        // Struct for each registry position
        // Which holds a key, a button, and flags for the state of that action
        public struct KEY_REG
        {
            public KEY_REG(Keys _k = Keys.W, Buttons _btn = Buttons.A)
            {
                key = _k;
                button = _btn;
                isKeyDown = false;
                wasKeyReleased = false;
            }
            public Keys key;
            public Buttons button;
            public bool isKeyDown;
            public bool wasKeyReleased;
        }

        static private List<KEY_REG> keyRegistry;

        // Stores keyboard states to determine button presses
        static private KeyboardState currentKeyboardState;
        static private KeyboardState previousKeyboardState;

        // Stores gamepad states to determine button presses
        static private GamePadState currentGamePadState;
        static private GamePadState previousGamePadState;

        // Get initial states so we have something to pass in updates
        static public void Init()
        {
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);
            keyRegistry = new List<KEY_REG>((int)IA.ACTIONS_COUNT);
            for (int i = 0; i < (int)IA.ACTIONS_COUNT; i++)
            {
                keyRegistry.Add(new KEY_REG());
            }
            SetupKeys();
        }
        // Setup default key/button values
        static public void SetupKeys()
        {
            keyRegistry[(int)IA.UP] = new KEY_REG(Keys.W, Buttons.DPadUp);
            keyRegistry[(int)IA.DOWN] = new KEY_REG(Keys.S, Buttons.DPadDown);
            keyRegistry[(int)IA.LEFT] = new KEY_REG(Keys.A, Buttons.DPadLeft);
            keyRegistry[(int)IA.RIGHT] = new KEY_REG(Keys.D, Buttons.DPadRight);
            keyRegistry[(int)IA.USE_INTERACT] = new KEY_REG(Keys.E, Buttons.A);
            keyRegistry[(int)IA.CANCEL] = new KEY_REG(Keys.Q, Buttons.B);
            keyRegistry[(int)IA.TOGGLE_INVENTORY] = new KEY_REG(Keys.I, Buttons.Y);
        }
        // Register a specified key to a specified action
        static public void RegisterKey(Keys _key, IA _regAction)
        {
            KEY_REG temp = keyRegistry[(int)_regAction];
            temp.key = _key;
            keyRegistry[(int)_regAction] = temp;
        }
        static public void RegisterButton(Buttons _btn, IA _regAction)
        {
            KEY_REG temp = keyRegistry[(int)_regAction];
            temp.button = _btn;
            keyRegistry[(int)_regAction] = temp;
        }
        static public void Update(GameTime _gameTime)
        {
            // Save the previous state of the keyboard and game pad so we can determinesingle key/button presses
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            // Read the current state of the keyboard and gamepad and store it
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            // iterate through all registered keys, change flag as necessary
            for (int i = 0; i < (int)IA.ACTIONS_COUNT; i++)
            {
                KEY_REG temp;
                temp = keyRegistry[i];

                if (currentKeyboardState.IsKeyDown(temp.key) == true || currentGamePadState.IsButtonDown(temp.button) == true)
                {
                    temp.isKeyDown = true;
                    temp.wasKeyReleased = false;
                }
                else if (previousKeyboardState.IsKeyDown(temp.key) == true && currentKeyboardState.IsKeyUp(temp.key) == true)
                {
                    temp.isKeyDown = false;
                    temp.wasKeyReleased = true;
                }
                else if (previousGamePadState.IsButtonDown(temp.button) == true && currentGamePadState.IsButtonUp(temp.button) == true)
                {
                    temp.isKeyDown = false;
                    temp.wasKeyReleased = true;
                }
                else if (currentKeyboardState.IsKeyUp(keyRegistry[i].key) == true || currentGamePadState.IsButtonUp(temp.button) == true)
                {
                    temp.isKeyDown = false;
                    temp.wasKeyReleased = false;
                }
                keyRegistry[i] = temp;
            }
        }

        // @Brief - Return true if the specified action key is being pressed
        // @Parem - Takes in an InputManager.IA enum (representing the target input action you want to check on)
        static public bool IsKeyDown(IA _actionKey) { return keyRegistry[(int)_actionKey].isKeyDown; }
        // @Brief - Return true if the specified action was just released
        // @Parem - Takes in an InputManager.IA enum (representing the target input action you want to check on)
        static public bool WasKeyReleased(IA _actionKey) { return keyRegistry[(int)_actionKey].wasKeyReleased; }

    }
}
