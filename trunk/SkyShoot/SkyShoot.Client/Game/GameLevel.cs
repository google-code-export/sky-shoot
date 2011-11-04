using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyShoot.Client.View;

namespace SkyShoot.Client.Game
{
    public class GameLevel : IDrawable 
    {

        public Texture2D Texture { get; private set; }

        public void AddTexture(Texture2D texture, Vector2 position)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Vector2.Zero, Color.White);
        }

    }
}
