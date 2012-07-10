using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.CollisionDetection;
using SkyShoot.Game.Client.GameObjects;
using SkyShoot.Game.Client.View;

namespace SkyShoot.Game.Client.Game
{
	public class GameModel
	{
		public GameLevel GameLevel { get; private set; }

		public ConcurrentDictionary<Guid, DrawableGameObject> Mobs { get; private set; }

		// todo //!! delete
		//public ConcurrentDictionary<Guid, Projectile> Projectiles { get; private set; }

		//public ConcurrentDictionary<Guid, GameBonus> GameBonuses { get; private set; }

		public Camera2D Camera2D { get; private set; }

		public GameModel(GameLevel gameLevel)
		{
			GameLevel = gameLevel;

			Camera2D = new Camera2D(GameLevel.Width, GameLevel.Height);

			Mobs = new ConcurrentDictionary<Guid, DrawableGameObject>();
			//Projectiles = new ConcurrentDictionary<Guid, Projectile>();
			//GameBonuses = new ConcurrentDictionary<Guid, GameBonus>();
		}

		public void AddMob(DrawableGameObject drawableGameObject)
		{
			if (!Mobs.TryAdd(drawableGameObject.Id, drawableGameObject))
				Trace.WriteLine("DrawableGameObject already exists", "GameModel/AddMob");
		}

		// todo //!!  delete
		//public void AddProjectile(Projectile projectile)
		//{
		//  if (!Projectiles.TryAdd(projectile.Id, projectile))
		//    Trace.WriteLine("Projectile already exists", "GameModel/AddProjectile");
		//}

		//public void AddGameBonus(GameBonus gameBonus)
		//{
		//  if (!GameBonuses.TryAdd(gameBonus.Id, gameBonus))
		//    Trace.WriteLine("GameBonus already exists", "GameModel/AddGameBonus");
		//}

		public DrawableGameObject GetMob(Guid id)
		{
			DrawableGameObject drawableGameObject;
			if (Mobs.TryGetValue(id, out drawableGameObject))
				return drawableGameObject;
			Trace.WriteLine("DrawableGameObject with such ID does not exist", "GameModel/GetMob");
			return null;
		}
		// todo //!! delete

		//public Projectile GetProjectile(Guid id)
		//{
		//  Projectile projectile;
		//  if (Projectiles.TryGetValue(id, out projectile))
		//    return projectile;
		//  Trace.WriteLine("Projectile with such ID does not exist", "GameModel/GetProjectile");
		//  return null;
		//}

		//public GameBonus GetGameBonus(Guid id)
		//{
		//  GameBonus gameBonus;
		//  if (GameBonuses.TryGetValue(id, out gameBonus))
		//    return gameBonus;
		//  Trace.WriteLine("GameBonus with such ID does not exist", "GameModel/GetGameBonus");
		//  return null;
		//}

		public void RemoveMob(Guid id)
		{
			DrawableGameObject drawableGameObject;
			if (!Mobs.TryRemove(id, out drawableGameObject))
				Trace.WriteLine("DrawableGameObject with such ID does not exist", "GameModel/RemoveMob");
		}

		//public void RemoveProjectile(Guid id)
		//{
		//  Projectile projectile;
		//  if (!Projectiles.TryRemove(id, out projectile))
		//    Trace.WriteLine("Projectile with such ID does not exist", "GameModel/RemoveProjectile");
		//}

		//public void RemoveGameBonus(Guid id)
		//{
		//  GameBonus gameBonus;
		//  if (!GameBonuses.TryRemove(id, out gameBonus))
		//    Trace.WriteLine("GameBonus with such ID does not exist", "GameModel/RemoveGameBonus");
		//}

		private void ApplyEvents(IEnumerable<AGameEvent> gameEvents)
		{
			try
			{
				if(gameEvents == null)
					return;
			foreach (var gameEvent in gameEvents)
			{
				if(gameEvent == null)
					continue;
				//if (gameEvent.GetType() == typeof(ObjectDirectionChanged))
				//{
				//  Trace.WriteLine("OBJECT_DIRECTION_CHANGED", "GameModel/ApplyEvents");
				//}

				//if (gameEvent.GetType() == typeof(ObjectHealthChanged))
				//{
				//  Trace.WriteLine("OBJECT_HEALTH_CHANGED", "GameModel/ApplyEvents");
				//}

				//if (gameEvent.GetType() == typeof(ObjectDeleted))
				//{
				//  Trace.WriteLine("OBJECT_DELETED", "GameModel/ApplyEvents");
				//}

				// todo rewrite!
				if (gameEvent.GetType() == typeof(NewObjectEvent))
				{
					//Trace.WriteLine("NEW_OBJECT_EVENT", "GameModel/ApplyEvents");

					var newGameObject = new AGameObject();
					gameEvent.UpdateMob(newGameObject);

					//if (newGameObject.Is(AGameObject.EnumObjectType.LivingObject))
					{
						DrawableGameObject newDrawableGameObject = GameFactory.CreateClientMob(newGameObject);
						AddMob(newDrawableGameObject);
					}

					//else if (newGameObject.Is(AGameObject.EnumObjectType.Bullet))
					//{
					//  Projectile newProjectile = GameFactory.CreateClientProjectile(newGameObject);
					//  AddProjectile(newProjectile);
					//}

					//else if (newGameObject.Is(AGameObject.EnumObjectType.Bonus))
					//{
					//  GameBonus newGameBonus = GameFactory.CreateClientGameBonus(newGameObject);
					//  AddGameBonus(newGameBonus);
					//}
				}

				else
				{
					// TODO check null
					DrawableGameObject drawableGameObject = GetMob(gameEvent.GameObjectId.Value);
					if (drawableGameObject != null)
					{
						gameEvent.UpdateMob(drawableGameObject);
						if (drawableGameObject.IsActive == false)
						{
							//if (DrawableGameObject.Is(AGameObject.EnumObjectType.Player))
							{
								RemoveMob(drawableGameObject.Id);
								GameController.Instance.MobDead(drawableGameObject);
							}
						}
					}
					//else
					//{
					//  // is projectile?
					//  Projectile projectile = GetProjectile(gameEvent.GameObjectId);
					//  if (projectile != null)
					//  {
					//    gameEvent.UpdateMob(projectile);
					//    if (projectile.IsActive == false)
					//    {
					//      RemoveProjectile(projectile.Id);
					//    }
					//  }

					//  else
					//  {
					//    // is gameBonus?
					//    GameBonus gameBonus = GetGameBonus(gameEvent.GameObjectId);
					//    if (gameBonus != null)
					//    {
					//      gameEvent.UpdateMob(gameBonus);
					//      if (gameBonus.IsActive == false)
					//      {
					//        RemoveGameBonus(gameBonus.Id);
					//      }
					//    }
					//  }
					//}
				}
			}
			}
			catch (Exception exc)
			{
				Trace.WriteLine("game:applayevents:"+exc);
			}

		}

		private int _updateCouter = 0;

		public void Update(GameTime gameTime)
		{
			// update mobs
			//foreach (var aMob in Mobs)
			//{
			//    aMob.Value.Update(gameTime);
			//}

			foreach (var aMob in Mobs)
			{
				aMob.Value.Update(gameTime);
				foreach (var slaver in Mobs)
				{
					if (aMob.Value != slaver.Value && !slaver.Value.IsBullet && aMob.Value.IsPlayer)
						aMob.Value.Coordinates += CollisionDetector.FitObjects(aMob.Value.Coordinates, aMob.Value.Radius, slaver.Value.Coordinates, slaver.Value.Radius);
				}
			}

			// todo //!! delete

			// update projectiles
			//foreach (var projectile in Projectiles)
			//{
			//  if (projectile.Value.IsActive)
			//    projectile.Value.Update(gameTime);
			//  else
			//    RemoveProjectile(projectile.Value.Id);
			//}

			if (_updateCouter % 5 == 0)
			{
				ApplyEvents(GameController.Instance.GetEvents());
			}

			// var sw = new Stopwatch();
			// sw.Start();
			if (_updateCouter++ % 30 == 0)
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
			}/**/
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
				Trace.Write("DRAW WARNING");
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

			// todo //!! delete


			// draw projectiles
			//foreach (var aProjectile in Projectiles)
			//{
			//  aProjectile.Value.Draw(spriteBatch);
			//}

			spriteBatch.End();
		}
	}
}