using Microsoft.Xna.Framework;

using System;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Game.Client.Game;
using SkyShoot.Game.Client.View;

using SkyShoot.Game.Controls;

using Nuclex.UserInterface;

using Nuclex.UserInterface.Controls.Desktop;

namespace SkyShoot.Game.Screens
{
	internal class GameplayScreen : GameScreen
	{
		private readonly ContentManager _content;

		public override bool IsMenuScreen
		{
			get { return false; }
		}

		public static AWeapon.AWeaponType Weapon = AWeapon.AWeaponType.Pistol;

		private ButtonControl _pistolButton;
		private ButtonControl _shotgunButton;
		private ButtonControl _rocketButton;
		private ButtonControl _flameButton;
		private ButtonControl _heaterButton;

		public GameplayScreen()
		{
			_content = new ContentManager(ScreenManager.Instance.Game.Services, "Content");
			CreateControls();
			InitializeControls();
		}

		public override void LoadContent()
		{
			// load landscapes
			Textures.SandLandscape = _content.Load<Texture2D>("Textures/Landscapes/SandLandscape");
			Textures.GrassLandscape = _content.Load<Texture2D>("Textures/Landscapes/GrassLandscape");
			Textures.SnowLandscape = _content.Load<Texture2D>("Textures/Landscapes/SnowLandscape");
			Textures.DesertLandscape = _content.Load<Texture2D>("Textures/Landscapes/DesertLandscape");
			Textures.VolcanicLandscape = _content.Load<Texture2D>("Textures/Landscapes/VolcanicLandscape");

			//load weapon
			Textures.Gun = _content.Load<Texture2D>("Textures/Weapon/Gun");
			Textures.Laser = _content.Load<Texture2D>("Textures/Weapon/Laser");
			Textures.FlameProjectile = _content.Load<Texture2D>("Textures/Weapon/FlameProjectile");
			Textures.ShotgunProjectile = _content.Load<Texture2D>("Textures/Weapon/ShotProjectile");
			Textures.RocketProjectile = _content.Load<Texture2D>("Textures/Weapon/RocketProjectile");
			Textures.SpiderProjectile = _content.Load<Texture2D>("Textures/Weapon/SpiderProjectile");
			Textures.Explosion = _content.Load<Texture2D>("Textures/Weapon/Explosion");
			Textures.DoubleDamage = _content.Load<Texture2D>("Textures/BonusesIcons/DoubleDamage");
			Textures.Fire = _content.Load<Texture2D>("Textures/BonusesIcons/Fire");
			Textures.Frozen = _content.Load<Texture2D>("Textures/BonusesIcons/Frozen");
			Textures.MedChest = _content.Load<Texture2D>("Textures/BonusesIcons/MedChest");
			Textures.Mirror = _content.Load<Texture2D>("Textures/BonusesIcons/Mirror");
			Textures.Protection = _content.Load<Texture2D>("Textures/BonusesIcons/Protection");
			Textures.Speed = _content.Load<Texture2D>("Textures/BonusesIcons/Speed");

			// load stones
			for (int i = 1; i <= Textures.StonesAmount; i++)
				Textures.Stones[i - 1] = _content.Load<Texture2D>("Textures/Landscapes/Stone" + i);
			Textures.OneStone = _content.Load<Texture2D>("Textures/Landscapes/Stone" + 1);

			// load player
			Textures.PlayerTexture = _content.Load<Texture2D>("Textures/Mobs/Man");

			// load mobs
			for (int i = 1; i <= Textures.MobsAmount; i++)
				Textures.MobTextures[i - 1] = _content.Load<Texture2D>("Textures/Mobs/Spider" + i);

			// load dead player
			Textures.DeadPlayerTexture =
				_content.Load<Texture2D>("Textures/Mobs/man_animation(new man)/death_animation/death_animation_06");

			// load dead spider
			Textures.DeadSpiderTexture =
				_content.Load<Texture2D>(
					"Textures/Mobs/mob_animation(v.2)/paukan_death_animation/paukan_death_animation_03");

			// load mob animation
			for (int i = 1; i <= Textures.SpiderAnimationFrameCount; i++)
				Textures.SpiderAnimation.AddFrame(
					_content.Load<Texture2D>("Textures/Mobs/spider_animation(uncomplete)/spider_" + i.ToString("D2")));

			// load player animation
			for (int i = 1; i <= Textures.PlayerAnimationFrameCount; i++)
				Textures.PlayerAnimation.AddFrame(
					_content.Load<Texture2D>("Textures/Mobs/man_animation(new man)/run/run_" + i.ToString("D2")));

			ScreenManager.Instance.Game.ResetElapsedTime();
		}

		private void CreateControls()
		{
			_pistolButton = new ButtonControl
			{
				Text = "Pistol",
				Bounds = new UniRectangle(new UniVector(70, 470), new UniVector(80, 40)),
			};

			_shotgunButton = new ButtonControl
			{
				Text = "Shotgun",
				Bounds = new UniRectangle(new UniVector(160, 470), new UniVector(80, 40)),
			};

			_flameButton = new ButtonControl
											{
												Text = "Flame",
												Bounds = new UniRectangle(new UniVector(250, 470), new UniVector(80, 40)),
											};

			_rocketButton = new ButtonControl
												{
													Text = "Rocket",
													Bounds = new UniRectangle(new UniVector(340, 470), new UniVector(80, 40)),
												};

			_heaterButton = new ButtonControl
			{
				Text = "Heater",
				Bounds = new UniRectangle(new UniVector(430, 470), new UniVector(80, 40)),
			};
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_pistolButton);
			Desktop.Children.Add(_shotgunButton);
			Desktop.Children.Add(_flameButton);
			Desktop.Children.Add(_rocketButton);
			Desktop.Children.Add(_heaterButton);

			_pistolButton.Pressed += PistolButtonPressed;
			_shotgunButton.Pressed += ShotgunButtonPressed;
			_flameButton.Pressed += FlameButtonPressed;
			_rocketButton.Pressed += RocketButtonPressed;
			_heaterButton.Pressed += HeaterButtonPressed;

			ScreenManager.Instance.Controller.AddListener(_pistolButton, PistolButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_shotgunButton, ShotgunButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_flameButton, FlameButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_rocketButton, RocketButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_heaterButton, HeaterButtonPressed);

		}

		private void RocketButtonPressed(object sender, EventArgs e)
		{
			Weapon = AWeapon.AWeaponType.RocketPistol;
			UpdateWeapon();
		}


		private void FlameButtonPressed(object sender, EventArgs e)
		{
			Weapon = AWeapon.AWeaponType.FlamePistol;
			UpdateWeapon();
		}

		private void PistolButtonPressed(object sender, EventArgs e)
		{
			Weapon = AWeapon.AWeaponType.Pistol;
			UpdateWeapon();
		}

		private void ShotgunButtonPressed(object sender, EventArgs e)
		{
			Weapon = AWeapon.AWeaponType.Shotgun;
			UpdateWeapon();
		}

		private void HeaterButtonPressed(object sender, EventArgs e)
		{
			Weapon = AWeapon.AWeaponType.Heater;
			UpdateWeapon();
		}

		private void UpdateWeapon()
		{
			GameController.Instance.ChangeWeapon(Weapon);
		}

		public override void UnloadContent()
		{
			Textures.PlayerAnimation.Clear();
			Textures.SpiderAnimation.Clear();

			if (_content != null)
				_content.Unload();
		}

		public override void HandleInput(Controller controller)
		{
			GameController.Instance.HandleInput(controller);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (GameController.Instance.GameModel == null)
				return;

			GameController.Instance.GameModel.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			GraphicsDevice graphicsDevice = ScreenManager.Instance.GraphicsDevice;
			SpriteBatch spriteBatch = ScreenManager.Instance.SpriteBatch;
			graphicsDevice.Clear(Color.SkyBlue);

			GameController.Instance.GameModel.Draw(spriteBatch);
		}
	}
}
