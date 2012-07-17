using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Game.Client.Game;
using SkyShoot.Game.Client.View;
using SkyShoot.Game.Controls;

namespace SkyShoot.Game.Screens
{
	internal class GameplayScreen : GameScreen
	{
		private static WeaponType _weapon = WeaponType.Pistol;

		private readonly ContentManager _content;

		private ButtonControl _pistolButton;
		private ButtonControl _shotgunButton;
		private ButtonControl _rocketButton;
		private ButtonControl _flameButton;
		private ButtonControl _heaterButton;
		private ButtonControl _turretButton;

		private LabelControl _lableLevel;
		private LabelControl _lableExp;
		private LabelControl _lableFrag;
		private LabelControl _lableCreeps;

		private int _counter;

		public GameplayScreen()
		{
			_content = new ContentManager(ScreenManager.Instance.Game.Services, "Content");
			CreateControls();
			InitializeControls();
		}

		public override bool IsMenuScreen
		{
			get { return false; }
		}

		public override void LoadContent()
		{
			// load landscapes
			Textures.SandLandscape = _content.Load<Texture2D>("Textures/Landscapes/SandLandscape");
			Textures.GrassLandscape = _content.Load<Texture2D>("Textures/Landscapes/GrassLandscape");
			Textures.SnowLandscape = _content.Load<Texture2D>("Textures/Landscapes/SnowLandscape");
			Textures.DesertLandscape = _content.Load<Texture2D>("Textures/Landscapes/DesertLandscape");
			Textures.VolcanicLandscape = _content.Load<Texture2D>("Textures/Landscapes/VolcanicLandscape");

			// load weapon
			Textures.Gun = _content.Load<Texture2D>("Textures/Weapon/Gun");
			Textures.Laser = _content.Load<Texture2D>("Textures/Weapon/Laser");
			Textures.FlameProjectile = _content.Load<Texture2D>("Textures/Weapon/FlameProjectile");
			Textures.ShotgunProjectile = _content.Load<Texture2D>("Textures/Weapon/ShotProjectile");
			Textures.RocketProjectile = _content.Load<Texture2D>("Textures/Weapon/RocketProjectile");
			Textures.PoisonProjectile = _content.Load<Texture2D>("Textures/Weapon/PoisonProjectile");
			Textures.SpiderProjectile = _content.Load<Texture2D>("Textures/Weapon/SpiderProjectile");
			Textures.Explosion = _content.Load<Texture2D>("Textures/Weapon/Explosion");
			Textures.DoubleDamage = _content.Load<Texture2D>("Textures/BonusesIcons/DoubleDamage");
			Textures.Fire = _content.Load<Texture2D>("Textures/BonusesIcons/Fire");
			Textures.Frozen = _content.Load<Texture2D>("Textures/BonusesIcons/Frozen");
			Textures.MedChest = _content.Load<Texture2D>("Textures/BonusesIcons/MedChest");
			Textures.Mirror = _content.Load<Texture2D>("Textures/BonusesIcons/Mirror");
			Textures.Protection = _content.Load<Texture2D>("Textures/BonusesIcons/Protection");
			Textures.Speed = _content.Load<Texture2D>("Textures/BonusesIcons/Speed");
			Textures.Poisoner = _content.Load<Texture2D>("Textures/Mobs/Spider1");
			Textures.Hydra = _content.Load<Texture2D>("Textures/Mobs/Spider2");
			//load minions

			Textures.Turret = _content.Load<Texture2D>("Textures/Mobs/Turret");

			// load stones
			for (int i = 1; i <= Textures.STONES_AMOUNT; i++)
				Textures.Stones[i - 1] = _content.Load<Texture2D>("Textures/Landscapes/Stone" + i);
			Textures.OneStone = _content.Load<Texture2D>("Textures/Landscapes/Stone" + 1);

			// load player
			Textures.PlayerTexture = _content.Load<Texture2D>("Textures/Mobs/Man");

			// load mobs
			for (int i = 1; i <= Textures.MOBS_AMOUNT; i++)
				Textures.MobTextures[i - 1] = _content.Load<Texture2D>("Textures/Mobs/Spider" + i);

			// load dead player
			Textures.DeadPlayerTexture =
				_content.Load<Texture2D>("Textures/Mobs/man_animation(new man)/death_animation/death_animation_06");

			// load dead spider
			Textures.DeadSpiderTexture =
				_content.Load<Texture2D>(
					"Textures/Mobs/mob_animation(v.2)/paukan_death_animation/paukan_death_animation_03");

			// load mob animation
			for (int i = 1; i <= Textures.SPIDER_ANIMATION_FRAME_COUNT; i++)
				Textures.SpiderAnimation.AddFrame(
					_content.Load<Texture2D>("Textures/Mobs/spider_animation(uncomplete)/spider_" + i.ToString("D2")));

			// load player animation
			for (int i = 1; i <= Textures.PLAYER_ANIMATION_FRAME_COUNT; i++)
				Textures.PlayerAnimation.AddFrame(
					_content.Load<Texture2D>("Textures/Mobs/man_animation(new man)/run/run_" + i.ToString("D2")));

			ScreenManager.Instance.Game.ResetElapsedTime();
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

			if (_counter % 60 == 0)
			{
				var stat = ConnectionManager.Instance.GetStats();
				if (stat != null)
				{
					_lableLevel.Text = "Level " + stat.Value.Lvl.ToString(CultureInfo.InvariantCulture);
				}
				if (stat != null)
				{
					_lableExp.Text = "Exp " + stat.Value.Exp.ToString(CultureInfo.InvariantCulture);
				}
				if (stat != null)
				{
					_lableFrag.Text = "Frag " + stat.Value.Frag.ToString(CultureInfo.InvariantCulture);
				}
				if (stat != null)
				{
					_lableCreeps.Text = "Creeps " + stat.Value.Creeps.ToString(CultureInfo.InvariantCulture);
				}
			}
			_counter++;
		}

		public override void Draw(GameTime gameTime)
		{
			// todo remove this
			if (GameController.Instance.GameModel == null)
				return;

			GraphicsDevice graphicsDevice = ScreenManager.Instance.GraphicsDevice;
			SpriteBatch spriteBatch = ScreenManager.Instance.SpriteBatch;
			graphicsDevice.Clear(Color.SkyBlue);

			GameController.Instance.GameModel.Draw(spriteBatch);
		}

		private void CreateControls()
		{
			#region Вывод статистики на экран

			_lableLevel = new LabelControl
			              	{
			              		Text = "Level",
			              		Bounds = new UniRectangle(new UniVector(-60, -40), new UniVector(0, 0)),
			              	};

			_lableExp = new LabelControl
			            	{
			            		Text = "Exp",
			            		Bounds = new UniRectangle(new UniVector(-60, -20), new UniVector(0, 0)),
			            	};

			_lableFrag = new LabelControl
			             	{
			             		Text = "Frag",
			             		Bounds = new UniRectangle(new UniVector(-60, 0), new UniVector(0, 0)),
			             	};

			_lableCreeps = new LabelControl
			               	{
			               		Text = "Creeps",
			               		Bounds = new UniRectangle(new UniVector(-60, 20), new UniVector(0, 0)),
			               	};

			#endregion

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

			_turretButton = new ButtonControl
			                	{
			                		Text = "Turret",
			                		Bounds = new UniRectangle(new UniVector(520, 470), new UniVector(80, 40)),
			                	};
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_pistolButton);
			Desktop.Children.Add(_shotgunButton);
			Desktop.Children.Add(_flameButton);
			Desktop.Children.Add(_rocketButton);
			Desktop.Children.Add(_heaterButton);
			Desktop.Children.Add(_turretButton);
			Desktop.Children.Add(_lableLevel);
			Desktop.Children.Add(_lableExp);
			Desktop.Children.Add(_lableFrag);
			Desktop.Children.Add(_lableCreeps);

			_pistolButton.Pressed += PistolButtonPressed;
			_shotgunButton.Pressed += ShotgunButtonPressed;
			_flameButton.Pressed += FlameButtonPressed;
			_rocketButton.Pressed += RocketButtonPressed;
			_heaterButton.Pressed += HeaterButtonPressed;
			_turretButton.Pressed += TurretButtonPressed;

			ScreenManager.Instance.Controller.AddListener(_pistolButton, PistolButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_shotgunButton, ShotgunButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_flameButton, FlameButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_rocketButton, RocketButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_heaterButton, HeaterButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_turretButton, TurretButtonPressed);
		}

		private void RocketButtonPressed(object sender, EventArgs e)
		{
			_weapon = WeaponType.RocketPistol;
			UpdateWeapon();
		}

		private void FlameButtonPressed(object sender, EventArgs e)
		{
			_weapon = WeaponType.FlamePistol;
			UpdateWeapon();
		}

		private void PistolButtonPressed(object sender, EventArgs e)
		{
			_weapon = WeaponType.Pistol;
			UpdateWeapon();
		}

		private void ShotgunButtonPressed(object sender, EventArgs e)
		{
			_weapon = WeaponType.Shotgun;
			UpdateWeapon();
		}

		private void HeaterButtonPressed(object sender, EventArgs e)
		{
			_weapon = WeaponType.Heater;
			UpdateWeapon();
		}

		private void TurretButtonPressed(object sender, EventArgs e)
		{
			_weapon = WeaponType.TurretMaker;
			UpdateWeapon();
		}

		private void UpdateWeapon()
		{
			ConnectionManager.Instance.ChangeWeapon(_weapon);
		}
	}
}
