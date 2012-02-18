using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon.Projectiles;

namespace SkyShoot.ServProgram.Session
{
	class ObjectPool<T> where T:AProjectile,new()
	{
		public ObjectPoolNode FirstActive;
		private ObjectPoolNode _firstInActive;
		public int size;

		public ObjectPool()
		{
			size = 0;
		}
		public T getInActive()
		{
			T t;
			if (_firstInActive != null)
			{
				t = _firstInActive.Item;
				_firstInActive = _firstInActive.Next;

			}
			else
			{
				size++;
				t = new T();
			}
			ObjectPoolNode newFirst = new ObjectPoolNode(t, true);
			newFirst.Next = FirstActive;
			FirstActive = newFirst;
			
			return t;
		}

		public ObjectPoolNode Next(ObjectPoolNode value)
		{
			if (value == null) return null;
			if (value.Next != null )
			if (value.Next.isActive == false)
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
			public bool isActive;

			public ObjectPoolNode(T mob,bool isActive)
			{
				Item = mob;
				this.isActive = isActive;
			}

			

		}
	}
}
