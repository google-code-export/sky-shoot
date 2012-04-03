using System;
using System.ServiceModel;

using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using SkyShoot.Contracts;
using SkyShoot.XNA.Framework;

using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Bonuses;
using SkyShoot.Service.Session;
using SkyShoot.ServProgram;

namespace SkyShoot.Service
{

	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant,
			InstanceContextMode = InstanceContextMode.PerSession)]
	public class MainSkyShootService : AGameObject, ISkyShootService//, ISkyShootCallback
	{
		//public static int globalID = 0;
		//public int localID;
		//private ISkyShootCallback _callback;
		public string Name;
		public Queue<AGameEvent> NewEvents;
		public List<AGameBonus> bonuses;

		//private Account.AccountManager _accountManager = new Account.AccountManager();
		private Session.SessionManager _sessionManager;

		//private static readonly List<MainSkyShootService> ClientsList = new List<MainSkyShootService>();

		public MainSkyShootService()
			: base(new Vector2(0, 0), new Guid())
		{
			_sessionManager = Session.SessionManager.Instances[0];
			//фиг знает как выбрать инстанс
			IsPlayer = true;
			NewEvents = new Queue<AGameEvent>();
			//localID = globalID;
			//globalID ++;
			bonuses = new List<AGameBonus>();
		}

		public void Disconnect() { this.LeaveGame(); SkyShootMessageFilter.DeleteFromTable(Id); }







		public override void Think(List<AGameObject> players)
		{
			throw new NotImplementedException();
		}

		public event SomebodyMovesHandler MeMoved;
		public event ClientShootsHandler MeShot;
		//public event SomebodySpawnsHandler MobSpawned;
		//public event SomebodyDiesHandler MobDied;

		//public Queue<AGameEvent> Move(Vector2 direction) // приходит снаружи от клиента
		public AGameEvent[] Move(Vector2 direction) // приходит снаружи от клиента
		{
			if (MeMoved != null)
			{
				MeMoved(this, direction);
			}
			return GetEvents();
		}

		public Queue<AGameEvent> Shoot(Vector2 direction)
		{
			if (MeShot != null)
			{
				MeShot(this, direction);
			}
			return null;// GetEvents();
		}

		public AGameEvent[] GetEvents()
		{
			var events = NewEvents.ToArray();//new Queue<AGameEvent>(NewEvents);
			NewEvents.Clear();
			return events;
		}
		/*
		public void TakeBonus(Contracts.Bonuses.AObtainableDamageModifier bonus)
		{
			bonus.Owner.State |= bonus.Type;
		}

		public void TakePerk(Contracts.Perks.Perk perk)
		{
			throw new NotImplementedException();
		}
		*/
		public void LeaveGame()
		{
			bool result = _sessionManager.LeaveGame(this);
			if (!result)
			{
				Trace.WriteLine(Name + "left the game");
				return;
			}

			//ClientsList.Remove(this);
		}

		public GameLevel GameStart(int gameId)
		{
			Trace.WriteLine("GameStarted");
			return _sessionManager.GameStarted(gameId);
		}


		public AGameObject[] SynchroFrame()
		{
			GameSession session;
			_sessionManager.SessionTable.TryGetValue(Id, out session);
			if (session == null)
			{
				return null;
			}
			return GameObjectConverter.ArrayedObjects(session.GetSynchroFrame());
		}

		public String[] PlayerListUpdate()
		{
			GameSession session;
			_sessionManager.SessionTable.TryGetValue(Id, out session);
			if (session == null)
			{
				return new String[] { };
			}
			return session.LocalGameDescription.Players.ToArray();
		}

		
		public bool JoinGame(GameDescription description)
		{
			try
			{
				this.Id = OperationContext.Current.IncomingMessageHeaders.GetHeader<Guid>("ID", "namespace");
				this.Name = "default";// должен браться из AccountManager
				var manager = _sessionManager;//SessionManager.Instances.Find(x => x.GetGameList().First(y => y.GameId == description.GameId) != null);
				manager.JoinGame(description, this);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
			
		}
	}
}
