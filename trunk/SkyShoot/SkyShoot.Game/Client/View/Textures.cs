using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkyShoot.Game.Client.View
{
    public static class Textures
    {
        public const int StonesAmount = 4;
        public const int MobsAmount = 2;

        //current graphic device
        public static GraphicsDevice GraphicsDevice;

        //Landscape textures
        public static Texture2D SandLandscape;
        public static Texture2D GrassLandscape;
        public static Texture2D SnowLandscape;
        public static Texture2D DesertLandscape;
        public static Texture2D VolcanicLandscape;

        //stone textures
        public static Texture2D[] Stones = new Texture2D[StonesAmount];

        //mob textures
        public static Texture2D PlayerTexture;
        public static Texture2D[] MobTextures = new Texture2D[MobsAmount];

        public static Texture2D ProjectileTexture
        {
            get { return Create(30, 3, Color.Red); }
        }

        // create a colored rectangle
        public static Texture2D Create(int width, int height, Color color)
        {
            // create the rectangle texture without colors
            var texture = new Texture2D(GraphicsDevice, height, width);

            // create a color array for the pixels
            var colors = new Color[width * height];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color(color.ToVector3());
            }

            // set the color data for the texture
            texture.SetData(colors);

            return texture;
        }

        // add small texture into big texture at Vector2D position
        public static void Merge(Texture2D big, Texture2D small, Vector2 position)
        {
            // get pixels from big texture
            var bigData = new Color[big.Width * big.Height];
            big.GetData(bigData);

            // get pixels from small texture
            var smallData = new Color[small.Width * small.Height];
            small.GetData(smallData);

            // replace transparent pixels
            for (int i = 0; i < small.Height; i++)
                for (int j = 0; j < small.Width; j++)
                    if (smallData[i * small.Width + j] == Color.Transparent)
                        smallData[i * small.Width + j] = bigData[((int)position.Y + i) * big.Width + ((int)position.X + j)];

            // set the new data
            big.SetData(
                0,
                new Rectangle((int)position.X, (int)position.Y, small.Width, small.Height),
                smallData, 
                0, 
                small.Width * small.Height);
            
        }

    }
}
