using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.Concurrent;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Game.Client.GameObjects;
using SkyShoot.Game.Client.View;

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

		private void ApplyEvents(IEnumerable<AGameEvent> gameEvents)
		{
			foreach (var gameEvent in gameEvents)
			{
				// todo getGameObject
				var gameObject = GetMob(gameEvent.GameObjectId);
				if (gameObject != null)
					gameEvent.UpdateMob(gameObject);
				else
				{
					// todo rewrite!
					Mob mob = new Mob(Textures.SpiderAnimation);
					gameEvent.UpdateMob(mob);
					AddMob(mob);
				}
			}
		}

		private int _updateCouter = 0;

		public void Update(GameTime gameTime)
		{
			// update mobs
			foreach (var aMob in Mobs)
			{
				aMob.Value.Update(gameTime);
			}

			// update projectiles
			foreach (var projectile in Projectiles)
			{
				if (projectile.Value.IsActive)
					projectile.Value.Update(gameTime);
				else
					RemoveProjectile(projectile.Value.Id);
			}

			ApplyEvents(GameController.Instance.GetEvents());

			// var sw = new Stopwatch();
			// sw.Start();
			/*if (_updateCouter++ % 30 == 0)
			{
				var serverGameObjects = GameController.Instance.SynchroFrame();
				if (serverGameObjects == null)
				{
					GameController.Instance.GameOver();
					return;
				}

				foreach (var serverGameObject in serverGameObjects)
				{
					AGameObject clientMob = GetMob(serverGameObject.Id);
					if (clientMob == null)
						Mobs.TryAdd(serverGameObject.Id, GameFactory.CreateClientMob(serverGameObject));
					else
						clientMob.Copy(serverGameObject);
				}
			}*/
			// sw.Stop();
			// Trace.WriteLine("SW:sync: "+sw.ElapsedMilliseconds);
			// sw.Restart();

			// sw.Stop();
			// Trace.WriteLine("SW:mobup: " + sw.ElapsedMilliseconds);
			// sw.Restart();

			// sw.Stop();
			// Trace.WriteLine("SW:prjup: " + sw.ElapsedMilliseconds);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			var me = GetMob(GameController.MyId);

			if (me == null)
			{
				// Trace.Write("DRAW WARNING");
				return;
			}

			Vector2 myPosition = me.CoordinatesM;

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
				// Trace.WriteLine(aMob.Value.Coordinates);
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