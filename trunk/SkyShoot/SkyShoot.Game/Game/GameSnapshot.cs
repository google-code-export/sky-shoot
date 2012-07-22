using System;
using System.Collections.Generic;
using System.Diagnostics;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.SynchroFrames;
using SkyShoot.Game.View;

namespace SkyShoot.Game.Game
{
	/// <summary>
	/// Снимок игры в определенный момент времени
	/// </summary>
	internal class GameSnapshot
	{
		/// <summary>
		/// Момент времени на сервере с начала игры, к
		/// которому относится данный снимок 
		/// </summary>
		public long Time { get; private set; }

		/// <summary>
		/// Коллекция всех игровых объектов, присутсвующих в
		/// момент времени Time
		/// </summary>
		private readonly IDictionary<Guid, DrawableGameObject> _gameObjects;

		/// <summary>
		/// Игровой уровень
		/// </summary>
		private readonly GameLevel _gameLevel;

		public GameSnapshot(SynchroFrame serverSynchroFrame, GameLevel gameLevel)
		{
			_gameObjects = new Dictionary<Guid, DrawableGameObject>();
			foreach (AGameObject serverGameObject in serverSynchroFrame)
			{
				DrawableGameObject newDrawableGameObject = GameFactory.CreateClientGameObject(serverGameObject);
				_gameObjects.Add(newDrawableGameObject.Id, newDrawableGameObject);
			}

			_gameLevel = gameLevel;
			Time = serverSynchroFrame.Time;
		}

		#region public methods

		/// <summary>
		/// Применение игровых событий к снимку игрового мира и 
		/// перемешение его в будущее
		/// </summary>
		/// <param name="gameEvents">Новые игровые события</param>
		public void ApplyEvents(IEnumerable<AGameEvent> gameEvents)
		{
			foreach (var gameEvent in gameEvents)
			{
				#region моделирование вперед

				// моделируем мир ко времени GameEvent'а
				long elapsedTime = gameEvent.TimeStamp - Time;
				Debug.Assert(elapsedTime >= 0);

				ComputeMovement(elapsedTime);
				Time = gameEvent.TimeStamp;

				#endregion

				#region применение произошедшего события

				if (gameEvent.Type == EventType.NewObjectEvent)
				{
					Debug.Assert((gameEvent as NewObjectEvent) != null);
					var serverGameObject = (gameEvent as NewObjectEvent).NewGameObject;

					DrawableGameObject newDrawableGameObject = GameFactory.CreateClientGameObject(serverGameObject);
					AddGameObject(newDrawableGameObject);

					//					// todo починить 
					//					if (newDrawableGameObject.Is(AGameObject.EnumObjectType.Explosion))
					//					{
					//						_explosions.Add(newDrawableGameObject, DateTime.Now.Ticks / 10000);
					//					}
				}
				else
				{
					Debug.Assert(gameEvent.GameObjectId.HasValue);

					DrawableGameObject drawableGameObject = _gameObjects[gameEvent.GameObjectId.Value];

					gameEvent.UpdateMob(drawableGameObject);
					if (drawableGameObject.IsActive == false)
					{
						//						todo починить 
						//						// if object is explosion, just ignore his deletion
						//						if (drawableGameObject.Is(AGameObject.EnumObjectType.Explosion))
						//						{
						//							drawableGameObject.IsActive = true;
						//						}
						_gameObjects.Remove(drawableGameObject.Id);
					}
				}

				#endregion
			}
		}

		public DrawableGameObject[] ExtrapolateTo(long gameTime)
		{
			Trace.WriteLine("Extrapolation time = " + (gameTime - Time));

			var result = new DrawableGameObject[_gameObjects.Count];
			int index = 0;
			foreach (DrawableGameObject gameObject in _gameObjects.Values)
			{
				DrawableGameObject futureObject = result[index] = new DrawableGameObject();
				futureObject.Copy(gameObject);
				futureObject.Coordinates = futureObject.ComputeMovement(gameTime - Time, _gameLevel);
				index++;
			}
			return result;
		}

		#endregion

		#region private methods

		/// <summary>
		/// Моделирование мира на время time вперед
		/// </summary>
		/// <param name="time">время, на которое нужно обновить мир</param>
		private void ComputeMovement(long time)
		{
			foreach (DrawableGameObject drawableGameObject in _gameObjects.Values)
			{
				drawableGameObject.Coordinates = drawableGameObject.ComputeMovement(time, _gameLevel);
			}
		}

		private void AddGameObject(DrawableGameObject drawableGameObject)
		{
			Debug.Assert(!_gameObjects.ContainsKey(drawableGameObject.Id));
			_gameObjects.Add(drawableGameObject.Id, drawableGameObject);
		}

		#endregion
	}
}
