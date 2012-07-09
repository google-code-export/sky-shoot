using System;

using System.Diagnostics;
using System.ServiceModel;
using System.Threading;

using System.Collections.Generic;

using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Perks;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Statistics;
using SkyShoot.Contracts.Weapon;

using SkyShoot.Game.Controls;
using SkyShoot.Game.Screens;

namespace SkyShoot.Game.Client.Game
{
	internal class ConnectionManager : ISkyShootService
	{
		#region singleton

		private static ConnectionManager _localInstance;

		public static ConnectionManager Instance
		{
			get { return _localInstance ?? (_localInstance = new ConnectionManager()); }
		}

		#endregion

		private ISkyShootService _service;

		#region очередь + поток

		private readonly EventWaitHandle _queue = new AutoResetEvent(false);

		private readonly object _locker = new object();

		private readonly Thread _thread;

		#endregion

		#region поля для работы с GameEvent'ами

		// received from server
		private AGameEvent[] _lastServerEvents;
		private bool _isNoServerGameEvents;

		private readonly Queue<AGameEvent> _lastClientEvents;

		#endregion

		public ConnectionManager()
		{
			_lastClientEvents = new Queue<AGameEvent>();
			_thread = new Thread(Run)
			          	{
			          		Name = "ConnectionManager"
			          	};
			_thread.Start();
		}

		public void Run()
		{
			while (true)
			{
				AGameEvent gameEvent = null;
				// Trace.WriteLine("Working", "Information");

				lock (_locker)
				{
					if (_lastClientEvents.Count > 0)
					{
						gameEvent = _lastClientEvents.Dequeue();
						if (gameEvent == null)
							return;
					}
				}

				if (gameEvent != null)
					SendClientGameEvent(gameEvent);
				else
					_queue.WaitOne();
			}
		}

		public void Stop()
		{
			// stopping thread
			AddClientGameEvent(null);
			_thread.Join();
			// close EventWaitHandle
			_queue.Close();
		}

		#region соединение с сервером

		private void InitConnection()
		{
			try
			{
				var channelFactory = new ChannelFactory<ISkyShootService>("SkyShootEndpoint");
				_service = channelFactory.CreateChannel();
			}
			catch (Exception e)
			{
				FatalError(e);
			}
		}

		private void FatalError(Exception e)
		{
			Trace.WriteLine(e);

			MessageBox.Message = "Connection error!";
			MessageBox.Next = ScreenManager.ScreenEnum.LoginScreen;
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MessageBoxScreen);
		}

		#endregion

		#region служебные методы для работы с GameEvent'ами

		private AGameEvent[] GetLatestServerGameEvent()
		{
			_isNoServerGameEvents = true;
			return _lastServerEvents;
		}

		private void AddClientGameEvent(AGameEvent gameEvent)
		{
			lock (_locker)
				_lastClientEvents.Enqueue(gameEvent);
			_queue.Set();
		}

		private void SendClientGameEvent(AGameEvent gameEvent)
		{
			try
			{
				switch (gameEvent.Type)
				{
					case EventType.ObjectShootEvent:
						var objectShootEvent = gameEvent as ObjectShootEvent;
						if (objectShootEvent != null) 
							_service.Shoot(objectShootEvent.ShootDirection);
						break;
					case EventType.ObjectDirectionChangedEvent:
						var objectDirectionChanged = gameEvent as ObjectDirectionChanged;
						if (objectDirectionChanged != null)
							_service.Move(objectDirectionChanged.NewRunDirection);
						break;
					case EventType.EmptyGameEvent:
						AGameEvent[] newServerEvents = _service.GetEvents();
						if (_isNoServerGameEvents)
						{
							_lastServerEvents = newServerEvents;
						}
						else
						{
							int totalLength = _lastServerEvents.Length + newServerEvents.Length;
							var allServerGameEvents = new AGameEvent[totalLength];
							
							_lastServerEvents.CopyTo(allServerGameEvents, 0);
							newServerEvents.CopyTo(allServerGameEvents, _lastServerEvents.Length);

							_lastServerEvents = allServerGameEvents;
						}
						break;
					case EventType.WeaponChangedEvent:
						var weaponChanged = gameEvent as WeaponChanged;
						if (weaponChanged != null) 
							_service.ChangeWeapon(weaponChanged.WeaponType);
						break;
					default:
						throw new Exception("Invalid argument");
				}
			} 
			catch (Exception e)
			{
				FatalError(e);
			}
		}

		#endregion

		// Методы для общения с сервером во время игры

		#region service implementation

		public AGameEvent[] Move(XNA.Framework.Vector2 direction)
		{
			AGameEvent moveGameEvent = new ObjectDirectionChanged(direction, null, 0);

			AddClientGameEvent(moveGameEvent);

			return GetLatestServerGameEvent();
		}

		public AGameEvent[] Shoot(XNA.Framework.Vector2 direction)
		{
			AGameEvent shootGameEvent = new ObjectShootEvent(direction, null, 0);

			AddClientGameEvent(shootGameEvent);

			return GetLatestServerGameEvent();
		}

		public AGameEvent[] ChangeWeapon(WeaponType type)
		{
			AGameEvent weaponChangedGameEvent = new WeaponChanged(null, type, null, 0);

			AddClientGameEvent(weaponChangedGameEvent);

			return GetLatestServerGameEvent();
		}

		public AGameEvent[] GetEvents()
		{
			AGameEvent emptyGameEvent = new EmptyEvent(null, 0);

			AddClientGameEvent(emptyGameEvent);

			return GetLatestServerGameEvent();
		}

		public AGameObject[] SynchroFrame()
		{
			// TODO написать также, как и с GameEvent, только без слияния
			try
			{
				return _service.SynchroFrame();
			}
			catch (Exception exc)
			{
				Trace.WriteLine("game:SynchroFrame" + exc);
				return new AGameObject[] {};
			}
		}

		public Stats? GetStats()
		{
			try
			{
				return _service.GetStats();
			}
			catch(Exception e)
			{
				FatalError(e);
				return null;
			}
		}

		#endregion

		#region other service methods

		public bool Register(string username, string password)
		{
			// initialize connection
			InitConnection();

			try
			{
				return _service.Register(username, HashHelper.GetMd5Hash(password));
			}
			catch (Exception e)
			{
				FatalError(e);
				return false;
			}
		}

		public Guid? Login(string username, string password)
		{
			// initialize connection
			InitConnection();

			Guid? login = null;
			try
			{
				login = _service.Login(username, HashHelper.GetMd5Hash(password));
			}
			catch (Exception e)
			{
				FatalError(e);
			}

			if (!login.HasValue)
			{
				MessageBox.Message = "Login error!";
				MessageBox.Next = ScreenManager.ScreenEnum.LoginScreen;
				ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MessageBoxScreen);
			}

			return login;
		}

		public GameDescription[] GetGameList()
		{
			try
			{
				return _service.GetGameList();
			}
			catch (Exception e)
			{
				FatalError(e);
				return null;
			}
		}

		public GameDescription CreateGame(GameMode mode, int maxPlayers, TileSet tile, int teams)
		{
			try
			{
				return _service.CreateGame(mode, maxPlayers, tile, teams);
			}
			catch (Exception e)
			{
				FatalError(e);
				return null;
			}
		}

		public bool JoinGame(GameDescription game)
		{
			try
			{
				return _service.JoinGame(game);
			}
			catch (Exception e)
			{
				FatalError(e);
				return false;
			}
		}

		public AGameEvent[] Move( /*XNA.Framework.Vector2 direction*/)
		{
//			try
//			{
//				// var sw = new Stopwatch();
//				// sw.Start();
//				return _service.Move(direction);
//				// sw.Stop();
//				// Trace.WriteLine("SW:serv:Move " + sw.ElapsedMilliseconds);
//			}
//			catch (Exception e)
//			{
//				FatalError(e);
//				return null;
//			}
			return null;
		}

		public void LeaveGame()
		{
			try
			{
				_service.LeaveGame();
			}
			catch (Exception e)
			{
				FatalError(e);
			}
		}

		public Contracts.Session.GameLevel GameStart(int gameId)
		{
			try
			{
				return _service.GameStart(gameId);
			}
			catch (Exception e)
			{
				FatalError(e);
				throw;
			}
		}

		public PlayerDescription[] PlayerListUpdate()
		{
			try
			{
				return _service.PlayerListUpdate();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc);
				return new PlayerDescription[] {};
			}
		}

		public void TakePerk(Perk perk)
		{
			try
			{
				//_service.TakePerk(perk);
			}
			catch (Exception e)
			{
				FatalError(e);
			}
		}

		#endregion
	}
}
