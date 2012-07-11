using System;

using System.Diagnostics;
using System.Text;

using System.Collections.Concurrent;
using System.Collections.Generic;

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

		public ConcurrentDictionary<Guid, DrawableGameObject> GameObjects { get; private set; }

		private readonly Contracts.Logger _logger = new Contracts.Logger("model_log.txt");

		public Camera2D Camera2D { get; private set; }

		public GameModel(GameLevel gameLevel)
		{
			GameLevel = gameLevel;

			Camera2D = new Camera2D(GameLevel.Width, GameLevel.Height);

			GameObjects = new ConcurrentDictionary<Guid, DrawableGameObject>();
		}

		public void AddGameObject(DrawableGameObject drawableGameObject)
		{
			if (!GameObjects.TryAdd(drawableGameObject.Id, drawableGameObject))
				Trace.WriteLine("DrawableGameObject already exists", "GameModel/AddMob");
		}

		public DrawableGameObject GetGameObject(Guid id)
		{
			DrawableGameObject drawableGameObject;
			if (GameObjects.TryGetValue(id, out drawableGameObject))
				return drawableGameObject;
			Trace.WriteLine("DrawableGameObject with such ID does not exist", "GameModel/GetMob");
			return null;
		}

		public void RemoveGameObject(Guid id)
		{
			DrawableGameObject drawableGameObject;
			if (!GameObjects.TryRemove(id, out drawableGameObject))
				Trace.WriteLine("DrawableGameObject with such ID does not exist", "GameModel/RemoveMob");
		}

		public void ApplyEvents(IList<AGameEvent> gameEvents)
		{
			PrintEvents(gameEvents);
			foreach (var gameEvent in gameEvents)
			{
				// todo проверить, выполняется ли
				Debug.Assert(gameEvent != null);

				if (gameEvent.Type == EventType.NewObjectEvent)
				{
					var newGameObject = new AGameObject();
					gameEvent.UpdateMob(newGameObject);

					DrawableGameObject newDrawableGameObject = GameFactory.CreateClientMob(newGameObject);
					AddGameObject(newDrawableGameObject);
				}
				else
				{
					// todo когда выполняется
					Debug.Assert(gameEvent.GameObjectId != null);

					DrawableGameObject drawableGameObject = GetGameObject(gameEvent.GameObjectId.Value);
					if (drawableGameObject != null)
					{
						gameEvent.UpdateMob(drawableGameObject);
						// todo работа со взрывами
						if (drawableGameObject.IsActive == false)
						{
							GameController.Instance.GameObjectDead(drawableGameObject);
						}
					}
				}
			}
			// catch (Exception exc)
			// {
			//		Trace.WriteLine("game:apply events:" + exc);
			// }
		}

		private int _updateCouter;

		public void Update(GameTime gameTime)
		{
			// update mobs
			//foreach (var aMob in Mobs)
			//{
			//    aMob.Value.Update(gameTime);
			//}

			foreach (var aMob in GameObjects)
			{
				aMob.Value.Update(gameTime);
				foreach (var slaver in GameObjects)
				{
					//Очевидно, что 3е условие предусматривает выполнение 2го, но так пули не смещают персонажа при выстреле. Меньше скачков. 
					if (aMob.Value != slaver.Value && !slaver.Value.IsBullet && aMob.Value.IsPlayer)
						aMob.Value.Coordinates += CollisionDetector.FitObjects(aMob.Value.Coordinates, aMob.Value.Radius,
						                                                       slaver.Value.Coordinates, slaver.Value.Radius);
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
				var serverGameObjects = GameController.Instance.SynchroFrame();
				if (serverGameObjects == null)
				{
					GameController.Instance.GameOver();
					return;
				}

				foreach (var serverGameObject in serverGameObjects)
				{
					AGameObject clientMob = GetGameObject(serverGameObject.Id);
					if (clientMob == null)
						GameObjects.TryAdd(serverGameObject.Id, GameFactory.CreateClientMob(serverGameObject));
					else
						clientMob.Copy(serverGameObject);
				}
			} /**/
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

		private void PrintEvents(IEnumerable<AGameEvent> gameEvents)
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("RECEIVE EVENTS:");

			foreach (var gameEvent in gameEvents)
			{
				stringBuilder.Append("\n  " + gameEvent.Type);
			}

			_logger.WriteLine(stringBuilder.ToString());
		}
	}
}