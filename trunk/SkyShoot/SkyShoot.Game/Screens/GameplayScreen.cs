using System.Diagnostics;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Game.Game;
using SkyShoot.Game.Input;
using SkyShoot.Game.Network;
using SkyShoot.Game.View;

namespace SkyShoot.Game.Screens
{
	internal class GameplayScreen : GameScreen
	{
		private static WeaponType _weapon = WeaponType.Pistol;

		private readonly ContentManager _content;

		private LabelControl _pistolLabel;
		private LabelControl _shotgunLabel;
		private LabelControl _rocketLabel;
		private LabelControl _flameLabel;
		private LabelControl _heaterLabel;
		private LabelControl _turretLabel;

		private LabelControl _levelLabel;
		private LabelControl _expLabel;
		private LabelControl _fragLabel;
		private LabelControl _creepsLabel;
		private LabelControl _healthPoints;

		private int _counter;

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

			// load weapon
			Textures.Gun = _content.Load<Texture2D>("Textures/Weapon/Gun");
			Textures.Laser = _content.Load<Texture2D>("Textures/Weapon/Laser");
			Textures.FlameProjectile = _content.Load<Texture2D>("Textures/Weapon/FlameProjectile");
			Textures.ShotgunProjectile = _content.Load<Texture2D>("Textures/Weapon/ShotProjectile");
			Textures.RocketProjectile = _content.Load<Texture2D>("Textures/Weapon/RocketProjectile");
			Textures.TurretProjectile = _content.Load<Texture2D>("Textures/Weapon/TurretProjectile");
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
			Textures.ParentMob = _content.Load<Texture2D>("Textures/Mobs/Spider3");
			Textures.Caterpillar = _content.Load<Texture2D>("Textures/Mobs/CaterpillerSegmentHead");
			//load minions

			Textures.Turret = _content.Load<Texture2D>("Textures/Mobs/Turret");

			// load stones
			for (int i = 1; i <= Textures.STONES_AMOUNT; i++)
				Textures.Stones[i - 1] = _content.Load<Texture2D>("Textures/Landscapes/Stone" + i);
			Textures.OneStone = _content.Load<Texture2D>("Textures/Landscapes/Stone" + 1);

			// load bricks
			Textures.Brick = _content.Load<Texture2D>("Textures/Landscapes/Brick");

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

			Debug.Assert(GameController.Instance.IsGameStarted);

			GameController.Instance.UpdateWorld(gameTime);

			if (_counter % 60 == 0)
			{
				var stat = ConnectionManager.Instance.GetStats();
				if (stat != null)
				{
					_levelLabel.Text = "Level " + stat.Value.Level.ToString(CultureInfo.InvariantCulture);
					_expLabel.Text = "Exp " + stat.Value.Experience.ToString(CultureInfo.InvariantCulture);
					_fragLabel.Text = "Frag " + stat.Value.Frag.ToString(CultureInfo.InvariantCulture);
					_creepsLabel.Text = "Creeps " + stat.Value.Creeps.ToString(CultureInfo.InvariantCulture);
					//_healthPoints.Text = "Health points " + 100.ToString(CultureInfo.InvariantCulture);
				}
			}
			_counter++;
		}

		public override void Draw(GameTime gameTime)
		{
			Debug.Assert(GameController.Instance.IsGameStarted);

			GraphicsDevice graphicsDevice = ScreenManager.Instance.GraphicsDevice;
			SpriteBatch spriteBatch = ScreenManager.Instance.SpriteBatch;
			graphicsDevice.Clear(Color.SkyBlue);

			GameController.Instance.DrawWorld(spriteBatch);
		}

		private void CreateControls()
		{
			#region Вывод статистики на экран

			_levelLabel = new LabelControl
							{
								Text = "Level",
								Bounds = new UniRectangle(new UniVector(-60, -40), new UniVector(0, 0)),
							};

			_expLabel = new LabelControl
							{
								Text = "Exp",
								Bounds = new UniRectangle(new UniVector(-60, -20), new UniVector(0, 0)),
							};

			_fragLabel = new LabelControl
							{
								Text = "Frag",
								Bounds = new UniRectangle(new UniVector(-60, 0), new UniVector(0, 0)),
							};

			_creepsLabel = new LabelControl
							{
								Text = "Creeps",
								Bounds = new UniRectangle(new UniVector(-60, 20), new UniVector(0, 0)),
							};
			/*_healthPoints = new LabelControl
			{
				Text = "Health points",
				Bounds = new UniRectangle(new UniVector(-60, 500), new UniVector(0, 0)),
			};*/

			#endregion

			#region список оружия

			const int labelWidth = 160;
			const float xLeft = 800 - labelWidth;
			const int y = -50;

			_pistolLabel = new LabelControl
								{
									Text = "1 - Pistol",
									Bounds = new UniRectangle(new UniVector(xLeft, y), new UniVector(0, 0)),
								};

			_shotgunLabel = new LabelControl
								{
									Text = "2 - Shotgun",
									Bounds = new UniRectangle(new UniVector(xLeft, y + 20), new UniVector(0, 0)),
								};

			_flameLabel = new LabelControl
							{
								Text = "3 - Flame",
								Bounds = new UniRectangle(new UniVector(xLeft, y + 40), new UniVector(0, 0)),
							};

			_rocketLabel = new LabelControl
								{
									Text = "4 - Rocket",
									Bounds = new UniRectangle(new UniVector(xLeft, y + 60), new UniVector(0, 0)),
								};

			_heaterLabel = new LabelControl
								{
									Text = "5 - Heater",
									Bounds = new UniRectangle(new UniVector(xLeft, y + 80), new UniVector(0, 0)),
								};

			_turretLabel = new LabelControl
								{
									Text = "6 - Turret",
									Bounds = new UniRectangle(new UniVector(xLeft, y + 100), new UniVector(0, 0)),
								};

			#endregion
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_pistolLabel);
			Desktop.Children.Add(_shotgunLabel);
			Desktop.Children.Add(_flameLabel);
			Desktop.Children.Add(_rocketLabel);
			Desktop.Children.Add(_heaterLabel);
			Desktop.Children.Add(_turretLabel);

			Desktop.Children.Add(_levelLabel);
			Desktop.Children.Add(_expLabel);
			Desktop.Children.Add(_fragLabel);
			Desktop.Children.Add(_creepsLabel);
//			Desktop.Children.Add(_healthPoints);
		}
	}
}
