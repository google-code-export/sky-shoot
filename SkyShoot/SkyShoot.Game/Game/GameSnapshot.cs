using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
		private readonly IDictionary<Guid, DrawableGameObject> _serverGameObjects;

		/// <summary>
		/// Коллекция игровых объектов, которые должен
		/// отобразить клиент
		/// </summary>
		private readonly IDictionary<Guid, DrawableGameObject> _futureGameObjects;

		/// <summary>
		/// Игровой уровень
		/// </summary>
		private readonly GameLevel _gameLevel;

		/// <summary>
		/// Все GameEvent'ы с момента последнего синхрокадра,
		/// нужно хранить их! 
		/// </summary>
		private readonly List<AGameEvent> _serverGameEvents;

		public GameSnapshot(GameLevel gameLevel)
		{
			_serverGameObjects = new Dictionary<Guid, DrawableGameObject>();
			_futureGameObjects = new Dictionary<Guid, DrawableGameObject>();
			_serverGameEvents = new List<AGameEvent>();

			_gameLevel = gameLevel;
		}

		#region public methods

		/// <summary>
		/// Применение игровых событий к снимку игрового мира и 
		/// перемешение его в будущее
		/// </summary>
		/// <param name="gameEvents">Новые игровые события</param>
		public void ApplyEvents(IEnumerable<AGameEvent> gameEvents)
		{
			gameEvents = gameEvents.OrderBy(x => x.TimeStamp);

			foreach (var gameEvent in gameEvents)
			{
				#region моделирование вперед

				// моделируем мир ко времени GameEvent'а
				long elapsedTime = gameEvent.TimeStamp - Time;

				Debug.Assert(elapsedTime >= 0);

				if (elapsedTime > 0)
				{
					ComputeMovement(elapsedTime);
					Time = gameEvent.TimeStamp;
				}

				#endregion

				#region применение произошедшего события

				if (gameEvent.Type == EventType.NewObjectEvent)
				{
					Debug.Assert((gameEvent as NewObjectEvent) != null);
					var serverGameObject = (gameEvent as NewObjectEvent).NewGameObject;

					AddGameObject(serverGameObject);
				}
				else
				{
					Debug.Assert(gameEvent.GameObjectId.HasValue);

					DrawableGameObject drawableGameObject = _serverGameObjects[gameEvent.GameObjectId.Value];

					gameEvent.UpdateMob(drawableGameObject);
					
					if (drawableGameObject.IsActive == false)
					{
						RemoveGameObject(drawableGameObject);
					}
				}

				#endregion
			}

			_serverGameEvents.AddRange(gameEvents);
		}

		public void ApplySynchroFrame(SynchroFrame synchroFrame)
		{
			Trace.WriteLine(synchroFrame); 

			#region обновление объектов

			_serverGameObjects.Clear();
			_futureGameObjects.Clear();
			foreach (AGameObject serverGameObject in synchroFrame)
			{
				AddGameObject(serverGameObject);
			}

			#endregion

			// устанавливаем время
			Time = synchroFrame.Time;

			// применение актуальных событий
			ApplyEvents(_serverGameEvents.Where(x => x.TimeStamp >= Time));

			// очистка событий
			_serverGameEvents.Clear();
		}

		public ICollection<DrawableGameObject> ExtrapolateTo(long gameTime)
		{
			long extrapolationTime = gameTime - Time;
			Trace.WriteLine("Extrapolation time = " + extrapolationTime);

			foreach (Guid gameObjectId in _serverGameObjects.Keys)
			{
				DrawableGameObject serverGameObject = _serverGameObjects[gameObjectId];
				DrawableGameObject futureGameObject = _futureGameObjects[gameObjectId];
				
				futureGameObject.Copy(serverGameObject);
				futureGameObject.Coordinates = futureGameObject.ComputeMovement(extrapolationTime, _gameLevel);
			}

			return _futureGameObjects.Values;
		}

		#endregion

		#region private methods

		/// <summary>
		/// Моделирование мира на время time вперед
		/// </summary>
		/// <param name="time">время, на которое нужно обновить мир</param>
		private void ComputeMovement(long time)
		{
			foreach (DrawableGameObject drawableGameObject in _serverGameObjects.Values)
			{
				drawableGameObject.Coordinates = drawableGameObject.ComputeMovement(time, _gameLevel);
			}
		}

		private void AddGameObject(AGameObject serverGameObject)
		{
			Debug.Assert(!_serverGameObjects.ContainsKey(serverGameObject.Id));
			
			DrawableGameObject drawableGameObject = GameFactory.CreateClientGameObject(serverGameObject);
			DrawableGameObject futureObject = GameFactory.CreateClientGameObject(serverGameObject);
			
			_serverGameObjects.Add(drawableGameObject.Id, drawableGameObject);
			_futureGameObjects.Add(drawableGameObject.Id, futureObject);
		}

		private void RemoveGameObject(DrawableGameObject drawableGameObject)
		{
			Guid gameObjectId = drawableGameObject.Id;

			Debug.Assert(_serverGameObjects.ContainsKey(gameObjectId));
			Debug.Assert(_futureGameObjects.ContainsKey(gameObjectId));

			_serverGameObjects.Remove(gameObjectId);
			_futureGameObjects.Remove(gameObjectId);
		}

		#endregion
	}
}
