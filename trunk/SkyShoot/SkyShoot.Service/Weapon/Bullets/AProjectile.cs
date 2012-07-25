using System;
using System.Collections.Generic;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Service;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon.Bullets
{
	public class AProjectile : AGameObject
	{
		private bool _mirrored;

		public AProjectile()
		{
			Owner = null;
			Radius = Constants.DEFAULT_BULLET_RADIUS;
			_mirrored = false;
		}

		protected AProjectile(AGameObject owner, Guid id, Vector2 direction)
		{
			Owner = owner;
			Id = id;
			ShootVector = RunVector = direction;
			Coordinates = owner.Coordinates;
			Radius = Constants.DEFAULT_BULLET_RADIUS;
			_mirrored = false;
		}

		//TODO: make uniform constructor?
		protected AProjectile(AGameObject owner, Guid id, Vector2 direction, Vector2 birthPlace)
		{
			Owner = owner;
			Id = id;
			ShootVector = RunVector = direction;
			Coordinates = birthPlace;
			Radius = Constants.DEFAULT_BULLET_RADIUS;
			_mirrored = false;
		}

		public override void Copy(AGameObject other)
		{
			if (!(other is AProjectile))
			{
				throw new Exception("Type mistmath");
			}
			var otherProjectile = other as AProjectile;
			base.Copy(other);
			// 'cause health used
			//LifeDistance = otherProjectile.LifeDistance;
			Owner = otherProjectile.Owner;
		}

		public AGameObject Owner { get; set; }

		public override IEnumerable<AGameEvent> Do(AGameObject obj, List<AGameObject> newObjects, long time)
		{
			var res = new List<AGameEvent>(base.Do(obj, newObjects, time));
			if (Owner.TeamIdentity == obj.TeamIdentity && obj.Is(EnumObjectType.LivingObject) && !obj.Is(EnumObjectType.Poisoning))//Препятствия не должны пролетаться насквозь
				return res;

			if (obj.Is(EnumObjectType.LivingObject))
			{
				var owner = Owner as MainSkyShootService;
				var damageMod = 1f;
				if (obj.Is(EnumObjectType.Player))
				{
					var player = obj as MainSkyShootService;
					if (player != null)
					{
						var mirror = player.GetBonus(EnumObjectType.Mirror);
						if (mirror != null && !_mirrored)
						{
							RunVector = -RunVector;
							RunVector.Normalize();// some times we have strange errors with length of run and shoot vectors
							ShootVector = RunVector;
							res.Add(new ObjectDirectionChanged(RunVector, Id, time));
							_mirrored = true;
							return res;
						}
						var shield = player.GetBonus(EnumObjectType.Shield);
						damageMod = shield == null ? 1f : shield.DamageFactor;
					}
				}
				obj.HealthAmount -= Damage * damageMod;

				#region Изменение статистики
				int teamMembers = 1;

				if (owner != null)
				{
					teamMembers = owner.TeamIdentity.Members.Count;
					owner.Tracker.AddExpPlayer(owner, obj, (int)(Damage * damageMod));

					foreach (AGameObject member in owner.TeamIdentity.Members)
					{
						var player = member as MainSkyShootService;
						if (player != null) //TODO: assert?
							player.Tracker.AddExpTeam(player, obj, (int)(Damage * damageMod), teamMembers);
					}
				}
				#endregion

				res.Add(new ObjectHealthChanged(obj.HealthAmount, obj.Id, time));
				// убираем пулю
				IsActive = false;
			}

			if (obj.Is(EnumObjectType.Wall))
			{
				IsActive = false;
			}

			if (!IsActive)
			{
				res.Add(new ObjectDeleted(Id, time));
			}

			return res;
		}

		public override IEnumerable<AGameEvent> Think(List<AGameObject> players, List<AGameObject> newGameObjects, long time)
		{
			HealthAmount -= PrevMoveDiff.Length();
			return base.Think(players, newGameObjects, time);
		}
	}
}
