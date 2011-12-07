using Microsoft.Xna.Framework;

namespace SkyShoot.Game.ScreenManager
{
    public abstract class GameScreen
    {
        public bool OtherScreenHasFocus;

		public bool IsActive { get; set; }

        protected GameScreen()
        {
        }

        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void HandleInput(InputState input) { }

        public virtual void Draw(GameTime gameTime) { }
    }
}
