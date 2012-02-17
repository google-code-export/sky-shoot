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
	public interface ISkyShootAdministratorCallback
	{
		[OperationContract(IsOneWay = true)]
		void GameStart(AMob[] mobs, GameLevel arena);

		[OperationContract(IsOneWay = true)]
		void GameOver();

		[OperationContract(IsOneWay = true)]
		void PlayerLeft(AMob mob);

		[OperationContract(IsOneWay = true)]
		void PlayerListChanged(String[] names);
	}
}
