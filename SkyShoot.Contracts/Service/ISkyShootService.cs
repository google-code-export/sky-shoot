﻿using System;
using System.ServiceModel;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Session;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.Service
{
	public delegate void SomebodyMovesHandler(AGameObject sender, Vector2 direction);
	public delegate void ClientShootsHandler(AGameObject sender, Vector2 direction);
	public delegate void ClientChangeWeaponHandler(AGameObject sender, Weapon.AWeapon.AWeaponType type);
	public delegate void SomebodyDiesHandler(AGameObject sender);
	public delegate void SomebodyHitHandler(AGameObject target, AGameObject projectile);
	
	[ServiceContract]
	public interface ISkyShootService
	{
        [OperationContract]
        GameDescription CreateGame(GameMode mode, int maxPlayers, TileSet level);

		[OperationContract]
		bool JoinGame(GameDescription game);

        [OperationContract]
        GameDescription[] GetGameList();

		[OperationContract]
		AGameEvent[] Move(Vector2 direction);

		[OperationContract]
		AGameEvent[] Shoot(Vector2 direction);

		[OperationContract]
		AGameEvent[] ChangeWeapon(Weapon.AWeapon.AWeaponType type);

		[OperationContract]
		AGameEvent[] GetEvents();

		//[OperationContract]
		//void TakePerk(Perk perk);

		[OperationContract]
		void LeaveGame();

		/// <summary>
		/// проверка началась ли игра
		/// </summary>
		/// <param name="gameId">идетификатор игры</param>
		/// <returns>если игра не началась возвращает null</returns>
		[OperationContract]
		GameLevel GameStart(Guid gameId);

		[OperationContract]
		AGameObject[] SynchroFrame();

		/// <summary>
		/// возвращает список игроков
		/// </summary>
		/// <returns>массив имен игроков</returns>
		[OperationContract]
		String[] PlayerListUpdate();
	}
}
