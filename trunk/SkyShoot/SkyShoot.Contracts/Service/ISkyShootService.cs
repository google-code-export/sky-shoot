using System;

using System.ServiceModel;

using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Session;
using SkyShoot.XNA.Framework;
using System.Collections.Generic;
using SkyShoot.Contracts.GameEvents;

namespace SkyShoot.Contracts.Service
{
	public delegate void SomebodyMovesHandler(AGameObject sender, Vector2 direction);
	public delegate void ClientShootsHandler(AGameObject sender, Vector2 direction);
	public delegate void SomebodyDiesHandler(AGameObject sender);
	public delegate void SomebodyHitHandler(AGameObject target, AGameObject projectile);
	
	//[ServiceContract(CallbackContract = typeof(ISkyShootCallback))]
	[ServiceContract]
	public interface ISkyShootService
	{
		[OperationContract(IsInitiating = true)]
		bool Register(string username, string password);

		[OperationContract(IsInitiating = true)]
		Guid? Login(string username, string password);

		[OperationContract]
		GameDescription[] GetGameList();

		[OperationContract]
		GameDescription CreateGame(GameMode mode, int maxPlayers, TileSet level);

		[OperationContract]
		bool JoinGame(GameDescription game);

		[OperationContract]
		AGameEvent[] Move(Vector2 direction);

		[OperationContract]
		Queue<AGameEvent> Shoot(Vector2 direction);

		[OperationContract]
		AGameEvent[] GetEvents();

		//[OperationContract(IsOneWay = true)]
		//void TakeBonus(AObtainableDamageModifier bonus);

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
		GameLevel GameStart(int gameId);

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
