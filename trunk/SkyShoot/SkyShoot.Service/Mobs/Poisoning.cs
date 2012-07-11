using System.Collections.Generic;
using System.Linq;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Mobs;
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
		}

		public override IEnumerable<AGameEvent> Think(List<AGameObject> gameObjects, List<AGameObject> newGameObjects, long time)
		{
			var res = new List<AGameEvent>(base.Think(gameObjects, newGameObjects, time));
			ShootVector = RunVector;
			ShootVector.Normalize();
			if (time - _lastShoot > _shootingDelay && Weapon != null && Weapon.IsReload(time))
			{
				_lastShoot = time;
				var bullets = Weapon.CreateBullets(this, ShootVector);
				
				if ((Target == null) || (Target.IsActive == false))
				{
					this.IsActive = false;
					res.Add(new ObjectDeleted(Id, time));
				}
				this.HealthAmount = this.HealthAmount - 10;//Са
				if (this.HealthAmount <= 0)//TODO: Можно покрасивее переписать
				{
					this.IsActive = false;
					res.Add(new ObjectDeleted(Id, time));
				}
				res.AddRange(bullets.Select(bullet => new NewObjectEvent(bullet, time)));
				newGameObjects.AddRange(bullets);
			}
			return res;
		}
	}
}