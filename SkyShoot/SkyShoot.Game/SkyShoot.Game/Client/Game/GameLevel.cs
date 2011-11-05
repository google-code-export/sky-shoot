using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyShoot.Contracts.Session;
using SkyShoot.Game.Client.View;
using IDrawable = SkyShoot.Game.Client.View.IDrawable;

namespace SkyShoot.Game.Client.Game
{
    public class GameLevel : IDrawable
    {

        public const int StonesNumber = 10;

        public Texture2D Texture { get; private set; }

        public GameLevel(TileSet tileSet)
        {
            //todo using const arrays
            switch (tileSet)
            {
                case TileSet.Grass:
                    Texture = Textures.GrassLandscape; break;
                case TileSet.Desert:
                    Texture = Textures.DesertLandscape; break;
                case TileSet.Sand:
                    Texture = Textures.SandLandscape; break;
                case TileSet.Snow:
                    Texture = Textures.SnowLandscape; break;
                case TileSet.Volcanic:
                    Texture = Textures.VolcanicLandscape; break;
            }

            var random = new Random();
            
            int width = Textures.GraphicsDevice.Viewport.Width;
            int height = Textures.GraphicsDevice.Viewport.Height;

            for (int i = 0; i < StonesNumber; i++) {
                var randomPosition = new Vector2(random.Next(width), random.Next(height));
                int stone = random.Next(3);
                Textures.Merge(Texture, Textures.Stones[stone], randomPosition);
            }

        }

        public void AddTexture(Texture2D texture, Vector2 position)
        {
            Textures.Merge(Texture, texture, position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Vector2.Zero, Color.White);
        }

    }
}
