using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SkyShoot.Game.ScreenManager
{
    public class InputState
    {
        public KeyboardState CurrentKeyboardState { get { return currentKeyboardState; } }
        private KeyboardState currentKeyboardState;

        public MouseState CurrentMouseState { get { return currentMouseState; } }
        private MouseState currentMouseState;

        public GamePadState CurrentGamePadState { get { return currentGamePadState; } }
        private GamePadState currentGamePadState;


        public KeyboardState LastKeyState { get { return lastKeyState; } }
        private KeyboardState lastKeyState;

        public MouseState LastMouseState { get { return lastMouseState; } }
        private MouseState lastMouseState;

        public GamePadState LastGamePadState { get { return lastGamePadState; } }
        private GamePadState lastGamePadState;



        public InputState()
        {
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);
            currentMouseState = Mouse.GetState();

            lastKeyState = new KeyboardState();
            lastGamePadState = new GamePadState(); 

        }

        public void Update()
        {
            lastKeyState = currentKeyboardState;
            lastGamePadState = currentGamePadState;
            lastMouseState = currentMouseState;

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);
            currentMouseState = Mouse.GetState();
        }

        public Vector2 RunVector
        {
            get
            {
                Vector2 runVector = Vector2.Zero;
                if (currentGamePadState.IsConnected) runVector = currentGamePadState.ThumbSticks.Left;
                else
                {
                    if (currentKeyboardState.IsKeyDown(Keys.Up)) runVector += Vector2.UnitY;
                    if (currentKeyboardState.IsKeyDown(Keys.Down)) runVector -= Vector2.UnitY;
                    if (currentKeyboardState.IsKeyDown(Keys.Left)) runVector -= Vector2.UnitX;
                    if (currentKeyboardState.IsKeyDown(Keys.Right)) runVector += Vector2.UnitX;
                }
                runVector.Normalize();
                return runVector;
            }
        }

        public bool KeyPressed(Keys key)
        {
            return (currentKeyboardState.IsKeyDown(key) &&
                    lastKeyState.IsKeyUp(key));
        }

        public bool ButtonPressed(Buttons button)
        {
            return (currentGamePadState.IsButtonDown(button) &&
                    lastGamePadState.IsButtonDown(button));
        }

        public bool IsMenuSelect()
        {
            return KeyPressed(Keys.Space) ||
                   KeyPressed(Keys.Enter) ||
                   ButtonPressed(Buttons.A) ||
                   ButtonPressed(Buttons.Start);
        }

        public bool IsMenuCancel()
        {
            return KeyPressed(Keys.Escape) ||
                   ButtonPressed(Buttons.B) ||
                   ButtonPressed(Buttons.Back);
        }

        public bool IsMenuUp()
        {
            return KeyPressed(Keys.Up) ||
                   ButtonPressed(Buttons.DPadUp) ||
                   ButtonPressed(Buttons.LeftThumbstickUp);
        }

        public bool IsMenuDown()
        {
            return KeyPressed(Keys.Down) ||
                   ButtonPressed(Buttons.DPadDown) ||
                   ButtonPressed(Buttons.LeftThumbstickDown);
        }
    }
}
