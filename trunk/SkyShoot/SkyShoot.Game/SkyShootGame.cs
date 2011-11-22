using Microsoft.Xna.Framework;
using SkyShoot.Game.Client.View;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SkyShoot.Game
{
    public class SkyShootGame : Microsoft.Xna.Framework.Game
    {
        private ScreenManager.ScreenManager _screenManager;
        Texture2D cursorTexture;
        Vector2 cursorPosition;
        SpriteBatch spriteBatch;

        public SkyShootGame()
        {
            new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Textures.GraphicsDevice = GraphicsDevice;
            ScreenManager.ScreenManager.Init(this);
            _screenManager = ScreenManager.ScreenManager.Instance;
            Components.Add(_screenManager);

            base.Initialize();

            _screenManager.AddScreen(new Screens.BackgroundScreen(Color.LightSeaGreen));
            _screenManager.AddScreen(new Screens.MainMenuScreen());
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            cursorTexture = Content.Load<Texture2D>("Textures/Cursors/Arrow");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            cursorPosition.X = Mouse.GetState().X;
            cursorPosition.Y = Mouse.GetState().Y;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            spriteBatch.Draw(cursorTexture, cursorPosition, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);

        }
    }
}
