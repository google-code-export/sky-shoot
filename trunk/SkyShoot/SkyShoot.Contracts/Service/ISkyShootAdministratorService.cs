using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using Microsoft.Xna.Framework;

using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Session;

namespace SkyShoot.Contracts.Service
{
	public delegate void StartGameHandler(AMob[] mobs, GameLevel arena);
	public delegate void NewPlayerConnectedHandler(AMob player);
	public delegate void PlayerLeftHandler(AMob player);

	[ServiceContract(CallbackContract = typeof(ISkyShootGameCallback))]
	public interface ISkyShootAdministratorService
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
		void LeaveGame();
	}
}
