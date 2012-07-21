using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.GameEvents;
using System.Collections.Concurrent;
using System.Diagnostics;
using SkyShoot.Contracts.Session;

namespace SkyShoot.Contracts.SynchroFrames
{
	[DataContract]
	public class SynchroFrame : IEnumerable<AGameObject>
	{
		/// <summary>
		/// Массив из всех игровых объектов 
		/// в момент времени Time
		/// </summary>
		[DataMember]
		public ConcurrentDictionary<Guid, AGameObject> _gameObjects;

		private GameLevel _gameLevel;

		/// <summary>
		/// Время с начала игры на сервере, к которому относится синхрокадр
		/// </summary>
		[DataMember]
		public long Time { get; private set; }

		public AGameObject this[Guid index]
		{
			get { return _gameObjects[index]; }
		}

		public SynchroFrame(AGameObject[] gameObjects, long time/*, GameLevel gameLevel*/)
		{
			//_gameObjects = gameObjects;
			foreach (var gameObject in gameObjects)
			{
				_gameObjects.TryAdd(gameObject.Id, gameObject);
			}
			Time = time;
			//_gameLevel = gameLevel;
		}

		IEnumerator<AGameObject> IEnumerable<AGameObject>.GetEnumerator()
		{
			return (_gameObjects as IEnumerable<AGameObject>).GetEnumerator();
		}

		public IEnumerator GetEnumerator()
		{
			return _gameObjects.GetEnumerator();
		}

		public override string ToString()
		{
			return String.Format("SynchroFrame, Time = {0}", Time);
		}

		public void ApplyEvents(AGameEvent[] gameEvents)
		{
			long timeSinceLastEvent;
			foreach (var gameEvent in gameEvents) {
				if (gameEvent.Type == EventType.NewObjectEvent)
				{
					var newGameObject = new AGameObject();
					gameEvent.UpdateMob(newGameObject);

					//DrawableGameObject newDrawableGameObject = GameFactory.CreateClientGameObject(newGameObject);
					//AddGameObject(newDrawableGameObject);
				}
				timeSinceLastEvent = gameEvent.TimeStamp - Time;
				Debug.Assert(timeSinceLastEvent >= 0);
				_gameObjects[gameEvent.GameObjectId.Value].Coordinates = _gameObjects[gameEvent.GameObjectId.Value].ComputeMovement(timeSinceLastEvent, _gameLevel); // TODO: чтобы работало с новым ComputeMovement
			}
		}

		public AGameObject[] ExtrapolateTo(long gameTime)
		{
			AGameObject[] result = new AGameObject[_gameObjects.Count];
			int index = 0;
			foreach (var gameObject in _gameObjects)
			{
				result[index].Copy(gameObject.Value);
				result[index].Coordinates = result[index].ComputeMovement(gameTime - Time, _gameLevel); // TODO: чтобы работало с новым ComputeMovement
			}
			return result;
		}
	}
}
