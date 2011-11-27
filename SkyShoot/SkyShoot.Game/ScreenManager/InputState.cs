using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Input;

namespace SkyShoot.Game.ScreenManager
{
    public class InputState
    {
        public KeyboardState CurrentKeyboardState { get { return _currentKeyboardState; } }
        private KeyboardState _currentKeyboardState;

        public MouseState CurrentMouseState { get { return _currentMouseState; } }
        private MouseState _currentMouseState;

        public GamePadState CurrentGamePadState { get { return _currentGamePadState; } }
        private GamePadState _currentGamePadState;


        public KeyboardState LastKeyboardState { get { return _lastKeyState; } }
        private KeyboardState _lastKeyState;

        public MouseState LastMouseState { get { return _lastMouseState; } }
        private MouseState _lastMouseState;

        public GamePadState LastGamePadState { get { return _lastGamePadState; } }
        private GamePadState _lastGamePadState;



        public InputState()
        {
            _currentKeyboardState = Keyboard.GetState();
            _currentGamePadState = GamePad.GetState(PlayerIndex.One);
            _currentMouseState = Mouse.GetState();

            _lastKeyState = new KeyboardState();
            _lastGamePadState = new GamePadState(); 

        }

        public void Update()
        {
            _lastKeyState = _currentKeyboardState;
            _lastGamePadState = _currentGamePadState;
            _lastMouseState = _currentMouseState;

            _currentKeyboardState = Keyboard.GetState();
            _currentGamePadState = GamePad.GetState(PlayerIndex.One);
            _currentMouseState = Mouse.GetState();
        }

        public Vector2 RunVector(KeyboardState keyboardState)
        {
            short keyblay = Settings.Default.KeyboardLayout;
            Vector2 runVector = Vector2.Zero;
            if (_currentGamePadState.IsConnected)
                runVector = _currentGamePadState.ThumbSticks.Left;
            else
            {
                switch (keyblay)
                {
                    case 0:
                        {
                            if (keyboardState.IsKeyDown(Keys.W)) runVector -= Vector2.UnitY;
                            if (keyboardState.IsKeyDown(Keys.S)) runVector += Vector2.UnitY;
                            if (keyboardState.IsKeyDown(Keys.A)) runVector -= Vector2.UnitX;
                            if (keyboardState.IsKeyDown(Keys.D)) runVector += Vector2.UnitX;
                        }
                        break;
                    case 1:
                        {
                            if (keyboardState.IsKeyDown(Keys.Up)) runVector -= Vector2.UnitY;
                            if (keyboardState.IsKeyDown(Keys.Down)) runVector += Vector2.UnitY;
                            if (keyboardState.IsKeyDown(Keys.Left)) runVector -= Vector2.UnitX;
                            if (keyboardState.IsKeyDown(Keys.Right)) runVector += Vector2.UnitX;
                        }
                        break;
                }
            }
            if (runVector.Length() > 0)
                runVector.Normalize();
            return runVector;
        }

        public bool IsNewKeyPressed(Keys key)
        {
            return (_currentKeyboardState.IsKeyDown(key) &&
                    _lastKeyState.IsKeyUp(key));
        }

        public bool IsNewButtonPressed(Buttons button)
        {
            return (_currentGamePadState.IsButtonDown(button) &&
                    _lastGamePadState.IsButtonDown(button));
        }

        public bool IsMenuSelect()
        {
            return IsNewKeyPressed(Keys.Space) ||
                   IsNewKeyPressed(Keys.Enter) ||
                   IsNewButtonPressed(Buttons.A) ||
                   IsNewButtonPressed(Buttons.Start);
        }

        public bool IsMenuCancel()
        {
            return IsNewKeyPressed(Keys.Escape) ||
                   IsNewButtonPressed(Buttons.B) ||
                   IsNewButtonPressed(Buttons.Back);
        }

        public bool IsMenuUp()
        {
            return IsNewKeyPressed(Keys.Up) ||
                   IsNewButtonPressed(Buttons.DPadUp) ||
                   IsNewButtonPressed(Buttons.LeftThumbstickUp);
        }

        public bool IsMenuDown()
        {
            return IsNewKeyPressed(Keys.Down) ||
                   IsNewButtonPressed(Buttons.DPadDown) ||
                   IsNewButtonPressed(Buttons.LeftThumbstickDown);
        }
    }
}
