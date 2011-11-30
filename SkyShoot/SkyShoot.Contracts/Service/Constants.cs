using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SkyShoot.Contracts
{
	public static class Constants
	{
		//pistolbullet
		public const float PISTOL_BULLET_SPEED = 0.1f;
		public const float PISTOL_DAMAGE = 10;
		public const float PISTOL_BULLET_LIFE_DISTANCE = 3000;
        //shotgunbullet
        public const float SHOTGUN_BULLET_SPEED = 0.1f;
        public const float SHOTGUN_BULLET_DAMAGE = 5;
        public const float SHOTGUN_BULLET_LIFE_DISTANCE = 3000;

		public const int CLAW_ATTACK_SPEED = 1000; // В миллисекундах.		public const float PLAYER_DEFAULT_HEALTH = 100;
		public const float PLAYER_DEFAULT_SPEED = 0.025f;
		public const float PLAYER_RADIUS = 15f;
		public const int FPS = 1000 / 60;
		public const float SPIDER_SPEED = 0.035f;
		public const float LEVELBORDER = 50;
		//public static Vector2 LEVEL_CENTER = new Vector2(1000, 1000);
	}
}
