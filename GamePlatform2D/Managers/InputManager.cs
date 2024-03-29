﻿using Microsoft.Xna.Framework.Input;

namespace GamePlatform2D
{
    public class InputManager
    {
        #region Variables
        KeyboardState prevKeyState;
        KeyboardState keyState;
        #endregion

        #region Properties
        public KeyboardState PrevKeyState
        {
            get { return prevKeyState; }
            set { prevKeyState = value; }
        }

        public KeyboardState KeyState
        {
            get { return keyState; }
            set { keyState = value; }
        }
        #endregion

        #region Public Methods
        public void Update()
        {
            prevKeyState = keyState;

            keyState = Keyboard.GetState();
        }

        public bool KeyPressed(Keys key)
        {
            if (keyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
                return true;
            else return false;
        }

        public bool KeyPressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (keyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
                    return true;
            }
            return false;
        }

        public bool KeyReleased(Keys key)
        {
            if (keyState.IsKeyUp(key) && prevKeyState.IsKeyDown(key))
                return true;
            else return false;
        }

        public bool KeyReleased(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (keyState.IsKeyUp(key) && prevKeyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        public bool KeyDown(Keys key)
        {
            if (keyState.IsKeyDown(key))
                return true;
            else return false;
        }

        public bool KeyDown(params Keys[] keys)
        {
            foreach (Keys key in keys) {
                if (keyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }
        #endregion

        #region PrivateMethods

        #endregion
    }
}
