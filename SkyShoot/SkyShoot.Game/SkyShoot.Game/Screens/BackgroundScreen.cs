using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SkyShoot.Game.Screens
{
    class BackgroundScreen : ScreenManager.GameScreen
    {
        ContentManager content;
        Texture2D backgroundTexture;
        Color backgroundColor = Color.Crimson; // delete

        public BackgroundScreen()
        {
            TransitionPosition = 0;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        //delete
        public BackgroundScreen(Color color)
        {
            backgroundColor = color;
            TransitionPosition = 0;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            //backgroundTexture = content.Load<Texture2D>("MenuBackground");
        }
        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }


        public override void Draw(GameTime gameTime)
        {
           // Заменить на текстуру
           // ScreenManager.SpriteBatch.Draw(backgroundTexture, ScreenManager.GraphicsDevice.Viewport.Bounds, new Color(TransitionAlpha,TransitionAlpha,TransitionAlpha));
            Color color = new Color(backgroundColor.ToVector3()*TransitionAlpha);
            ScreenManager.GraphicsDevice.Clear(color);
        }
    }
}
