using System;
using System.ServiceModel;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Statistics;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.Service
{
	public delegate void SomebodyMovesHandler(AGameObject sender, Vector2 direction);

	public delegate void ClientShootsHandler(AGameObject sender, Vector2 direction);

	public delegate void ClientChangeWeaponHandler(AGameObject sender, Weapon.WeaponType type);

	public delegate void SomebodyDiesHandler(AGameObject sender);

	public delegate void SomebodyHitHandler(AGameObject target, AGameObject projectile);

	[ServiceContract]
	public interface ISkyShootService
	{
		[OperationContract(IsInitiating = true)]
		AccountManagerErrorCode Register(string username, string password);

		[OperationContract(IsInitiating = true)]
		Guid? Login(string username, string password, out AccountManagerErrorCode errorCode);

		[OperationContract]
		GameDescription[] GetGameList();

		[OperationContract]
		GameDescription CreateGame(GameMode mode, int maxPlayers, TileSet level, int teams);

		[OperationContract]
		bool JoinGame(GameDescription game);

		[OperationContract]
		AGameEvent[] Move(Vector2 direction);

		[OperationContract]
		AGameEvent[] Shoot(Vector2 direction);

		[OperationContract]
		AGameEvent[] ChangeWeapon(Weapon.WeaponType type);

		[OperationContract]
		AGameEvent[] GetEvents();

		[OperationContract]
		void LeaveGame();

		/// <summary>
		/// проверка началась ли игра
		/// </summary>
		/// <param name="gameId">идетификатор игры</param>
		/// <returns>если игра не началась возвращает null</returns>
		[OperationContract]
		GameLevel GameStart(int gameId);

		[OperationContract]
		AGameObject[] SynchroFrame();

		[OperationContract] // Выдает таблицу данных о уровне, опыте, фрагах
		Stats? GetStats();

		/// <summary>
		/// возвращает список игроков
		/// </summary>
		/// <returns>массив имен игроков</returns>
		[OperationContract]
		String[] PlayerListUpdate();
	}
}
