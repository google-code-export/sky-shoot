using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyShoot.Contracts.CollisionDetection;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.SynchroFrames;
using SkyShoot.Game.Network;
using SkyShoot.Game.View;
using SkyShoot.Contracts.Utils;

namespace SkyShoot.Game.Game
{
	public class GameModel
	{
		public GameLevel GameLevel { get; private set; }

		public ConcurrentDictionary<Guid, DrawableGameObject> GameObjects { get; private set; }

		// explosions -> exploded time
		private readonly Dictionary<DrawableGameObject, long> _explosions;

		private readonly Logger _logger;

		public Camera2D Camera2D { get; private set; }

		public GameModel(GameLevel gameLevel, Logger logger)
		{
			_logger = logger;

			GameLevel = gameLevel;

			Camera2D = new Camera2D(GameLevel.Width, GameLevel.Height);

			GameObjects = new ConcurrentDictionary<Guid, DrawableGameObject>();

			_explosions = new Dictionary<DrawableGameObject, long>();
		}

		public void AddGameObject(DrawableGameObject drawableGameObject)
		{
			if (!GameObjects.TryAdd(drawableGameObject.Id, drawableGameObject))
				Trace.WriteLine("DrawableGameObject already exists", "GameModel/AddMob");
		}

		public void RemoveGameObject(Guid id)
		{
			DrawableGameObject drawableGameObject;
			if (!GameObjects.TryRemove(id, out drawableGameObject))
				Trace.WriteLine("DrawableGameObject with such ID does not exist", "GameModel/RemoveMob");
		}

		public DrawableGameObject GetGameObject(Guid id)
		{
			DrawableGameObject drawableGameObject;
			if (GameObjects.TryGetValue(id, out drawableGameObject))
				return drawableGameObject;
			Trace.WriteLine("DrawableGameObject with such ID does not exist (" + id.ToString() + ")", "GameModel/GetMob");
			return null;
		}

		public void ApplyEvents(AGameEvent[] gameEvents)
		{
			Logger.PrintEvents(gameEvents);
			foreach (var gameEvent in gameEvents)
			{
				// todo проверить, выполн€етс€ ли
				Debug.Assert(gameEvent != null);

				if (gameEvent.Type == EventType.NewObjectEvent)
				{
					var newGameObject = new AGameObject();
					gameEvent.UpdateMob(newGameObject);

					DrawableGameObject newDrawableGameObject = GameFactory.CreateClientGameObject(newGameObject);
					AddGameObject(newDrawableGameObject);

					if (newDrawableGameObject.Is(AGameObject.EnumObjectType.Explosion))
					{
						_explosions.Add(newDrawableGameObject, DateTime.Now.Ticks / 10000);
					}
				}
				else
				{
					// todo когда выполн€етс€
					Debug.Assert(gameEvent.GameObjectId != null);

					DrawableGameObject drawableGameObject = GetGameObject(gameEvent.GameObjectId.Value);
					if (drawableGameObject != null)
					{
						gameEvent.UpdateMob(drawableGameObject);
						if (drawableGameObject.IsActive == false)
						{
							// if object is explosion, just ignore his deletion
							if (drawableGameObject.Is(AGameObject.EnumObjectType.Explosion))
							{
								drawableGameObject.IsActive = true;
							}
							else
							{
								GameController.Instance.GameObjectDead(drawableGameObject);
							}
						}
					}
				}
			}
		}

		public void UpdateExplosions()
		{
			// todo придумать что-нибудь
			var keys = new DrawableGameObject[_explosions.Count];
			_explosions.Keys.CopyTo(keys, 0);

			foreach (DrawableGameObject explosion in keys)
			{
				if (DateTime.Now.Ticks / 10000 - _explosions[explosion] > Constants.EXPLOSION_LIFE_DISTANCE)
				{
					_explosions.Remove(explosion);
					RemoveGameObject(explosion.Id);
				}
			}
		}

		/// <summary>
		/// ќбновление позиций игровых объектов
		/// </summary>
		public void ApplySynchroFrame(SynchroFrame synchroFrame)
		{
			if (synchroFrame == null)
			{
				GameController.Instance.GameOver();
				return;
			}

			foreach (AGameObject serverGameObject in synchroFrame)
			{
				AGameObject clientMob = GetGameObject(serverGameObject.Id);

				// todo временный фикс, новые (хорошо забытые старые) объекты не добавл€ютс€
				//				if (clientMob == null)
				//					GameObjects.TryAdd(serverGameObject.Id, GameFactory.CreateClientMob(serverGameObject));
				//				else
				//					clientMob.Copy(serverGameObject);

				if (clientMob != null)
					clientMob.Copy(serverGameObject);
			}
		}

		private int _updateCouter;

		public void Update(GameTime gameTime)
		{
			// update explosions
			UpdateExplosions();

			/*foreach (var aMob in GameObjects)
			{
				aMob.Value.Update(gameTime);
				foreach (var slaver in GameObjects)
				{
					//ќчевидно, что 3е условие предусматривает выполнение 2го, но так пули не смещают персонажа при выстреле. ћеньше скачков. 
					if (aMob.Value != slaver.Value && !slaver.Value.IsBullet && aMob.Value.IsPlayer)
						aMob.Value.Coordinates += CollisionDetector.FitObjects(aMob.Value.Coordinates, aMob.Value.Radius,
						                                                       slaver.Value.Coordinates, slaver.Value.Radius);
				}
			}*/

			foreach (var aMob in GameObjects)
			{
				aMob.Value.Update(gameTime);
				foreach (var slaver in GameObjects)
				{
					//ќчевидно, что 3е условие предусматривает выполнение 2го, но так пули не смещают персонажа при выстреле. ћеньше скачков. 
					if (aMob.Value != slaver.Value && !slaver.Value.IsBullet && aMob.Value.IsPlayer)
						if (aMob.Value.Radius * aMob.Value.Radius + slaver.Value.Radius * slaver.Value.Radius <= (aMob.Value.Coordinates - slaver.Value.Coordinates).LengthSquared())
							aMob.Value.Coordinates += CollisionDetector.FitObjects(aMob.Value.Coordinates, aMob.Value.RunVector, aMob.Value.Bounding, slaver.Value.Coordinates, slaver.Value.RunVector, slaver.Value.Bounding);
				}
			}

			if (_updateCouter % 5 == 0)
			{
				ApplyEvents(ConnectionManager.Instance.GetEvents());
			}

			// var sw = new Stopwatch();
			// sw.Start();
			if (_updateCouter++ % 30 == 0)
			{
				ApplySynchroFrame(ConnectionManager.Instance.SynchroFrame());

			}
			// sw.Stop();
			// Trace.WriteLine("SW:sync: "+sw.ElapsedMilliseconds);
			// sw.Restart();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			var me = GetGameObject(GameController.MyId);

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
			foreach (var aMob in GameObjects)
			{
				// Trace.WriteLine(aMob.Value.Coordinates);
				aMob.Value.Draw(spriteBatch);
			}

			spriteBatch.End();
		}
	}
}