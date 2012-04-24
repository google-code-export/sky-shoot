using System;
using System.Collections.Generic;
using System.ServiceModel;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;
using System.Diagnostics;
using SkyShoot.GameServer.Session;
using SkyShoot.ServProgram;

namespace SkyShoot.GameServer
{
	//[ServiceBehavior(IncludeExceptionDetailInFaults=true)]
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant,
			InstanceContextMode = InstanceContextMode.PerSession)]
	class SkyShootLoginService :ISkyShootLogin
	{
		public String Name;
		public Guid Id;

		public SkyShootLoginService()
		{
			Id = Guid.NewGuid();
		}

		public bool Register(string username, string password)
		{
			/*bool result = _accountManager.Register(username, password);
			if(result)
			{
				Trace.WriteLine(username + "has registered");
			}
			else
			{
				Trace.WriteLine(username + "is not registered. The name of the employing or other errors");
			}
			return result; */
			return true;
		}

		public Guid? Login(string username, string password)
		{
			bool result = true;//_accountManager.Login(username, password);


			//guid должен быть постоянный для соответствующего аккаунта и выдаваться здесь

			if (result)
			{
				Name = username;
				//_callback = OperationContext.Current.GetCallbackChannel<ISkyShootCallback>();
				//IsPlayer = true;

				//ClientsList.Add(this);
			}
			else
			{
				return null;
			}

			return Id;
		}

		public GameDescription[] GetGameList()
		{
			var list = new List<GameDescription>(SessionManager.Instances[0].GetGameList());
			list.AddRange(SessionManager.Instances[1].GetGameList());
			return list.ToArray();
		}

	    public bool JoinGame(GameDescription game)
	    {
	        throw new NotImplementedException();
	    }

	    public bool RegisterForGame(GameDescription game)
		{
			try
			{
				var manager = SessionManager.Instances[0];//SessionManager.Instances.Find(x => x.GetGameList().First(y => y.GameId == game.GameId) !=null);
				//bool result = manager.JoinGame(game, this);

				bool result = true;

				SkyShootMessageFilter.table.Add(this.Id, manager.ManagerId);

				if (result)
				{
					Trace.WriteLine(this.Name + "has joined the game ID=" + game.GameId);
				}
				else
				{
					Trace.WriteLine(this.Name + "has not joined the game ID=" + game.GameId);
				}
				return result;
			}
			catch (Exception e)
			{
				Trace.Fail(this.Name + "has not joined the game." + e.Message);
				return false;
			}
		}

		public GameDescription CreateGame(GameMode mode, int maxPlayers, TileSet tileSet)
		{
			try
			{
				var manager = SessionManager.Instances[0];//[new Random((int)System.DateTime.Now.ToBinary()).Next(0, 2)];
				var gameDescription = manager.CreateGame(mode, maxPlayers, tileSet);
				return gameDescription;
			}
			catch (Exception e)
			{
				Trace.Fail(this.Name + " unable to create game. " + e.Message);
				return null;
			}
		}

	}
}
