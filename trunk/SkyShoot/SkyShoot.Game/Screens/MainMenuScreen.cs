using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SkyShoot.Game.ScreenManager;

namespace SkyShoot.Game.Screens
{
    class MainMenuScreen : MenuScreen
    {
        ContentManager content;

        public MainMenuScreen()
            : base("SkyShoot")
        {

            MenuEntry multiplayerMenuEntry = new MenuEntry("Multiplayer");
            MenuEntry playGameMenuEntry = new MenuEntry("Start");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            multiplayerMenuEntry.Selected += MultiplayerMenuEntrySelected;
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += ExitMenuEntrySelected;

            MenuEntries.Add(multiplayerMenuEntry);
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        public override void LoadContent()
        {
            base.LoadContent();

        }
        
        public override void UnloadContent()
        {
            base.UnloadContent();
            if (content != null)
                content.Unload();
        }

        void PlayGameMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new LoginScreen()); 
        }


        void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            //ScreenManager.AddScreen(new OptionsMenuScreen());
        }

        void ExitMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        void MultiplayerMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new LoginScreen());
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);            
        }

        protected override void OnCancel()
        {
            base.OnCancel();
            ScreenManager.Game.Exit();
        }
    }
}
