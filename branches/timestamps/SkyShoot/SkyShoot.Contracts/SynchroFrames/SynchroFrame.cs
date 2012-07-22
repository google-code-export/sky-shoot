using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SkyShoot.Contracts.GameObject;

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
		private AGameObject[] _gameObjects;

		/// <summary>
		/// Время с начала игры на сервере, к которому относится синхрокадр
		/// </summary>
		[DataMember]
		public long Time { get; private set; }

		public AGameObject this[int index]
		{
			get { return _gameObjects[index]; }
		}

		public SynchroFrame(AGameObject[] gameObjects, long time)
		{
			_gameObjects = gameObjects;
			Time = time;
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
	}
}
