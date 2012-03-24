using System;
using System.Diagnostics;
using System.Collections.Concurrent;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Game.Client.View;
using SkyShoot.Game.Client.Weapon;
using SkyShoot.Game.Client.Players;

namespace SkyShoot.Game.Client.Game
{
	public class GameModel
	{
		public GameLevel GameLevel { get; private set; }

		public ConcurrentDictionary<Guid, Mob> Mobs { get; private set; }

		public ConcurrentDictionary<Guid, Projectile> Projectiles { get; private set; }

		public Camera2D Camera2D { get; private set; }

		public GameModel(GameLevel gameLevel)
		{
			GameLevel = gameLevel;

			Camera2D = new Camera2D(GameLevel.Width, GameLevel.Height);

			Mobs = new ConcurrentDictionary<Guid, Mob>();
			Projectiles = new ConcurrentDictionary<Guid, Projectile>();
		}

		public void AddMob(Mob mob)
		{
			if (!Mobs.TryAdd(mob.Id, mob))
				Trace.WriteLine("Mob already exists", "GameModel/AddMob");
		}

		public void AddProjectile(Projectile projectile)
		{
			if (!Projectiles.TryAdd(projectile.Id, projectile))
				Trace.WriteLine("Projectile already exists", "GameModel/AddProjectile");
		}

		public Mob GetMob(Guid id)
		{
			Mob mob;
			if (Mobs.TryGetValue(id, out mob))
				return mob;
			Trace.WriteLine("Mob with such ID does not exist", "GameModel/GetMob");
			return null;
		}

		public Projectile GetProjectile(Guid id)
		{
			Projectile projectile;
			if (Projectiles.TryGetValue(id, out projectile))
				return projectile;
			Trace.WriteLine("Projectile with such ID does not exist", "GameModel/GetProjectile");
			return null;
		}

		public void RemoveMob(Guid id)
		{
			Mob mob;
			if (!Mobs.TryRemove(id, out mob))
				Trace.WriteLine("Mob with such ID does not exist", "GameModel/RemoveMob");
		}

		public void RemoveProjectile(Guid id)
		{
			Projectile projectile;
			if (!Projectiles.TryRemove(id, out projectile))
				Trace.WriteLine("Projectile with such ID does not exist", "GameModel/RemoveProjectile");
		}

		private int _updateCouter = 0;
		public void Update(GameTime gameTime)
		{
			var sw = new Stopwatch();
			sw.Start();
			if (_updateCouter++ % 30 == 0)
			{
				var newMobs = GameController.Instance.SynchroFrame();
				if (newMobs == null)
				{
					GameController.Instance.GameOver();
					return;
				}
				Mobs.Clear();
				foreach (var aGameObject in newMobs)
				{
					Mobs.TryAdd(aGameObject.Id, GameFactory.CreateClientMob(aGameObject));
				}
			}
			// update mobs
			sw.Stop();
			Trace.WriteLine("SW:sync: "+sw.ElapsedMilliseconds);
			sw.Restart();
			foreach (var aMob in Mobs)
			{
				aMob.Value.Update(gameTime);
			}
			sw.Stop();
			Trace.WriteLine("SW:mobup: " + sw.ElapsedMilliseconds);
			sw.Restart();

			// update projectiles
			foreach (var projectile in Projectiles)
			{
				if (projectile.Value.IsActive)
					projectile.Value.Update(gameTime);
				else
					RemoveProjectile(projectile.Value.Id);
			}
			sw.Stop();
			Trace.WriteLine("SW:prjup: " + sw.ElapsedMilliseconds);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Vector2 myPosition;

			var me = GetMob(GameController.MyId);
			if (me == null)
			{
				return;
			}
			myPosition = me.CoordinatesM;


			Camera2D.Position = myPosition;

			spriteBatch.Begin(SpriteSortMode.Immediate,
												BlendState.AlphaBlend,
												null,
												null,
												null,
												null,
												Camera2D.GetTransformation(Textures.GraphicsDevice));

			// draw background
			GameLevel.Draw(spriteBatch);

			// draw mobs
			foreach (var aMob in Mobs)
			{
				aMob.Value.Draw(spriteBatch);
			}

			// draw projectiles
			foreach (var aProjectile in Projectiles)
			{
				aProjectile.Value.Draw(spriteBatch);
			}

			spriteBatch.End();
		}
	}
}
