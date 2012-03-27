
namespace SkyShoot.Contracts
{
	public static class Constants
	{
		#region wepons

		#region pistol

		public const float PISTOL_BULLET_SPEED = 0.1f;
		public const float PISTOL_DAMAGE = 10;
		public const float PISTOL_BULLET_LIFE_DISTANCE = 3000;
		public const int PISTOL_ATTACK_RATE = 400;

		#endregion

		#region shotgun

		public const float SHOTGUN_BULLET_SPEED = 0.2f;
		public const float SHOTGUN_BULLET_DAMAGE = 2;
		public const float SHOTGUN_BULLET_LIFE_DISTANCE = 70;
		public const int SHOTGUN_ATTACK_RATE = 500;

		#endregion

		public const int CLAW_ATTACK_RATE = 1000; // В миллисекундах.		public const float PLAYER_DEFAULT_HEALTH = 100;

		#endregion

		#region mobs

		public const float PLAYER_DEFAULT_SPEED = 0.05f;
		public const float PLAYER_RADIUS = 15f;
		public const float SPIDER_SPEED = 0.06f;

		#endregion

		#region common

		public const int FPS = 1000/60;
		public const float LEVELBORDER = 50;

		#endregion

		public const int SHIELD_MILLISECONDS = 30000;
		public const int DOUBLE_DAMAGE_MILLISECONDS = 30000;
	}
}