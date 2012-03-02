using System;

using System.ServiceModel;

using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Perks;
using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Session;

using SkyShoot.XNA.Framework;
using SkyShoot.Contracts.Weapon.Projectiles;
using System.Collections.Generic;
using SkyShoot.Contracts.GameEvents;

namespace SkyShoot.Contracts.Service
{
	public delegate void SomebodyMovesHandler(AGameObject sender, Vector2 direction);
	public delegate void ClientShootsHandler(AGameObject sender, Vector2 direction);
	public delegate void SomebodyDiesHandler(AGameObject sender);
	public delegate void SomebodyHitHandler(AGameObject target, Weapon.Projectiles.AProjectile projectile);
	
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
		Queue<AGameEvent> Move(Vector2 direction);

		[OperationContract]
		Queue<AGameEvent> Shoot(Vector2 direction);

		[OperationContract]
		Queue<AGameEvent> GetEvents();

		//[OperationContract(IsOneWay = true)]
		//void TakeBonus(AObtainableDamageModifier bonus);

		//[OperationContract]
		//void TakePerk(Perk perk);

		[OperationContract]
		void LeaveGame();

		[OperationContract]
		GameLevel GameStart(int gameId);//если игра не началась возвращает null
		//void GameStart(AGameObject[] mobs, GameLevel arena);


		[OperationContract]
		AGameObject[] SynchroFrame();

		[OperationContract]
		String[] PlayerListUpdate();
	}
}
