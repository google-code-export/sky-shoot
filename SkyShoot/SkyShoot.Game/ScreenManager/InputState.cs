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


        public KeyboardState LastKeyState { get { return _lastKeyState; } }
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

        public Vector2 RunVectorA
        {
            get
            {
                Vector2 runVector = Vector2.Zero;
                if (_currentGamePadState.IsConnected)
                    runVector = _currentGamePadState.ThumbSticks.Left;
                else
                {
                    if (_currentKeyboardState.IsKeyDown(Keys.Up)) runVector -= Vector2.UnitY;
                    if (_currentKeyboardState.IsKeyDown(Keys.Down)) runVector += Vector2.UnitY;
                    if (_currentKeyboardState.IsKeyDown(Keys.Left)) runVector -= Vector2.UnitX;
                    if (_currentKeyboardState.IsKeyDown(Keys.Right)) runVector += Vector2.UnitX;
                }
                if (runVector.Length() > 0)
                    runVector.Normalize();
                return runVector;
            }
        }

        public Vector2 RunVectorK
        {
            get
            {
                Vector2 runVector = Vector2.Zero;
                if (_currentGamePadState.IsConnected)
                    runVector = _currentGamePadState.ThumbSticks.Left;
                else
                {
                    if (_currentKeyboardState.IsKeyDown(Keys.W)) runVector -= Vector2.UnitY;
                    if (_currentKeyboardState.IsKeyDown(Keys.S)) runVector += Vector2.UnitY;
                    if (_currentKeyboardState.IsKeyDown(Keys.A)) runVector -= Vector2.UnitX;
                    if (_currentKeyboardState.IsKeyDown(Keys.D)) runVector += Vector2.UnitX;
                }
                if (runVector.Length() > 0)
                    runVector.Normalize();
                return runVector;
            }
        }

        public bool KeyPressed(Keys key)
        {
            return (_currentKeyboardState.IsKeyDown(key) &&
                    _lastKeyState.IsKeyUp(key));
        }

        public bool ButtonPressed(Buttons button)
        {
            return (_currentGamePadState.IsButtonDown(button) &&
                    _lastGamePadState.IsButtonDown(button));
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
