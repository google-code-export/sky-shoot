using System.Collections.Generic;
using System.Linq;
using SkyShoot.Contracts;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Weapon;

namespace SkyShoot.ServProgram.Mobs
{
	class Poisoning : Mob
	{
		private readonly int _shootingDelay = 1000;
		private long _lastShoot;

		public Poisoning(float health, AWeapon weapon, AGameObject afflicted)//3--Наш страдалец.
			: base(health)
		{
			Weapon = weapon;
			Weapon.Owner = this;
			this.Target = afflicted;//Только один будет жертвой.
			Radius = 1; //Должен помещаться в игрока, и не взаимодействовать ни с чем.
			Speed = 0.12f;//Чтобы наверняка догнал.
			_shootingDelay = Constants.POISONTICK_ATTACK_RATE;
			Damage = Constants.POISONTICK_DAMAGE;
		}

		public override IEnumerable<AGameEvent> Think(List<AGameObject> gameObjects, List<AGameObject> newGameObjects, long time)
		{
			var res = new List<AGameEvent>(base.Think(gameObjects, newGameObjects, time));
			ShootVector = RunVector;
			ShootVector.Normalize();
			if (time - _lastShoot > _shootingDelay)
			{
				_lastShoot = time;
				HealthAmount = HealthAmount - 10;//Сам себя кусает

				if ((Target == null) || (Target.IsActive == false) || (HealthAmount <= 0))
				{
					IsActive = false;
					res.Add(new ObjectDeleted(Id, time));
				}
				else
				{
					Target.HealthAmount -= Damage;
					res.Add(new ObjectHealthChanged(Target.HealthAmount, Target.Id, time));
				}
			}
			return res;
		}
	}
}