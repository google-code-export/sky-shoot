namespace SkyShoot.Contracts.Service
{
	public static class Constants
	{
		#region weapons

		public const float DEFAULT_BULLET_RADIUS = 5f;

		#region pistol

		public const float PISTOL_BULLET_SPEED = 0.1f;
		public const float PISTOL_BULLET_DAMAGE = 10;
		public const float PISTOL_BULLET_LIFE_DISTANCE = 3000;
		public const int PISTOL_ATTACK_RATE = 400;

		#endregion

		#region rocketpistol

		public const float ROCKET_BULLET_SPEED = 0.1f;
		public const float ROCKET_BULLET_DAMAGE = 3f;
		public const float ROCKET_BULLET_LIFE_DISTANCE = 3000f;
		public const int ROCKET_PISTOL_ATTACK_RATE = 1500;
		public const float ROCKET_BULLET_RADIUS = 4f;

		#endregion

		#region shotgun

		public const float SHOTGUN_BULLET_SPEED = 0.2f;
		public const float SHOTGUN_BULLET_DAMAGE = 2f;
		public const float SHOTGUN_BULLET_LIFE_DISTANCE = 70f;
		public const int SHOTGUN_ATTACK_RATE = 500;

		#endregion

		#region flame

		public const int FLAME_BULLETS_COUNT = 24;
		public const float FLAME_SPEED = 0.05f;
		public const float FLAME_DAMAGE = 0.5f;
		public const float FLAME_LIFE_DISTANCE = 120f;
		public const int FLAME_ATTACK_RATE = 700;
		public const float FLAME_RADIUS = 5f;
		public const float TURRET_RADIUS = 10F;

		#endregion

		#region explosion

		public const float EXPLOSION_SPEED = 0f;
		public const float EXPLOSION_DAMAGE = 40f;
		public const float EXPLOSION_LIFE_DISTANCE = 200f;
		public const int EXPLOSION_ATTACK_RATE = 1;
		public const float EXPLOSION_RADIUS = 50f;

		#endregion

		#region heater

		public const float HEATER_BULLET_SPEED = 0.5f;
		public const float HEATER_BULLET_DAMAGE = 100;
		public const float HEATER_BULLET_LIFE_DISTANCE = 3000;
		public const int HEATER_ATTACK_RATE = 7000;

		#endregion

		#region mobGenerator

		public const int MOB_GENERATOR_ATTACK_RATE = 7000;

		#endregion

		#region poisonGun

		public const float POISON_BULLET_SPEED = 0.1f;
		public const float POISON_BULLET_DAMAGE = 5f;
		public const float POISON_BULLET_LIFE_DISTANCE = 30f;
		public const int POISON_GUN_ATTACK_RATE = 3000;
		public const float POISON_BULLET_RADIUS = 4f;

		#endregion

		#region poisonTick
		public const float POISONTICK_DAMAGE = 10f;
		public const int POISONTICK_ATTACK_RATE = 2000;
		#endregion

		#region turretGun

		//TODO: new turretGun constants

		public const float TURRET_GUN_BULLET_SPEED = 0.1f;
		public const float TURRET_GUN_BULLET_DAMAGE = 10;
		public const float TURRET_GUN_BULLET_LIFE_DISTANCE = 300;
		public const int TURRET_GUN_ATTACK_RATE = 1000;

		#endregion

		#region turretMaker

		//TODO: new turretMaker constants

		public const float TURRET_MAKER_ATTACK_RATE = 1000;

		#endregion

		#region turret

		//TODO: new turret constants

		public const float TURRET_HEALTH = 50;
		public const int TURRET_SHOOTING_DELAY = 1000;
		public const float TURRET_TARGET_SEARCHING_RANGE = 200;

		#endregion

		public const int CLAW_ATTACK_RATE = 1000; // В миллисекундах.		public const float PLAYER_DEFAULT_HEALTH = 100;

		#endregion

		#region mobs

		public const float PLAYER_DEFAULT_SPEED = 0.05f;
		public const float PLAYER_RADIUS = 15f;
		public const float PLAYER_HEALTH = 100f;
		public const float SPIDER_SPEED = 0.06f;
		public const float PARENT_MOB_SPEED = 0.01f;
		public const float PARENT_MOB_HEALTH = 700f;
		public const float CHILDREN_MOB_HEALTH = 25f;
		public const float HYDRA_HEALTH = 50f;
		public const float HYDRA_SPEED = 0.03f;
		public const float POISONER_MOB_SPEED = 0.03f;
		public const float POISONER_MOB_HEALTH = 500f;
		public const float POISONING_MOB_SPEED = 0.12f; //Чтобы догонял даже при ускорении.
		public const float POISONING_MOB_HEALTH = 40f;


		#endregion

		#region common
		public const float EPSILON = 0.01f;

		public const int FPS = 1000 / 60;
		public const float LEVELBORDER = 50;

		#endregion

		#region bonuses
		public const int BONUS_TYPE_COUNT = 5;

		public const int SHIELD_MILLISECONDS = 30000;
		public const int DOUBLE_DAMAGE_MILLISECONDS = 30000;
		public const int REMEDY_MILLISECONDS = 30000; // redundant
		public const int MIRROR_MILLISECONDS = 30000; // redundant
		public const int SPEEDUP_MILLISECONDS = 10000;
		#endregion
	}
}