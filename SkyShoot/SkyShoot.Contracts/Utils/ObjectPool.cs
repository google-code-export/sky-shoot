using System.Collections;
using System.Collections.Generic;
using SkyShoot.Contracts.GameObject;

namespace SkyShoot.Contracts.Utils
{
	class ObjectPool<T> : IEnumerable<T> where T : AGameObject, new()
	{
		private ObjectPoolNode _firstInActive;

		public ObjectPoolNode FirstActive;
		public int Size;

		public ObjectPool()
		{
			Size = 0;
		}

		public int Count
		{
			get
			{
				return Size;
			}
		}

		public T GetInActive()
		{
			T t;
			if (_firstInActive != null)
			{
				t = _firstInActive.Item;
				_firstInActive = _firstInActive.Next;

			}
			else
			{
				Size++;
				t = new T();
			}
			var newFirst = new ObjectPoolNode(t, true);
			newFirst.Next = FirstActive;
			FirstActive = newFirst;

			return t;
		}

		public ObjectPoolNode Next(ObjectPoolNode value)
		{
			if (value == null) return null;
			if (value.Next != null)
				if (value.Next.IsActive == false)
				{
					var newInActive = value.Next;
					value.Next = newInActive.Next;
					newInActive.Next = _firstInActive;
					_firstInActive = newInActive;
					//size--;
					return Next(value.Next);
				}
			return value.Next;
		}

		public class ObjectPoolNode
		{
			public T Item;
			public ObjectPoolNode Next;
			public bool IsActive;

			public ObjectPoolNode(T mob, bool isActive)
			{
				Item = mob;
				IsActive = isActive;
			}
		}

		public T[] ToArray()
		{
			var res = new List<T>(Size);
			var t = FirstActive;
			while (t != null)
			{
				res.Add(t.Item);
				t = t.Next;
			}
			return res.ToArray();
		}

		public IEnumerator<T> GetEnumerator()
		{
			throw new System.NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
