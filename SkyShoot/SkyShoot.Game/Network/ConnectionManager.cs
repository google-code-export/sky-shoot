using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Statistics;
using SkyShoot.Contracts.Utils;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Game.Screens;
using SkyShoot.Game.Utils;
using Timer = System.Timers.Timer;

namespace SkyShoot.Game.Network
{
	internal class ConnectionManager : ISkyShootService
	{
		#region singleton

		private static ConnectionManager _localInstance;

		public static ConnectionManager Instance
		{
			get { return _localInstance ?? (_localInstance = new ConnectionManager()); }
		}

		private ConnectionManager()
		{
			_eventTimer = new Timer(EVENT_TIMER_DELAY_TIME);
			_synchroFrameTimer = new Timer(SYNCHRO_FRAME_DELAY_TIME);
		}

		#endregion

		private ISkyShootService _service;

		private Queue<AGameEvent> _lastClientGameEvents;

		private const int MAX_SERVER_GAME_EVENTS = 100;

		#region local thread

		private readonly EventWaitHandle _queue = new AutoResetEvent(false);

		private readonly object _locker = new object();

		private Thread _thread;

		#endregion

		#region server game events and synchroFrame

		// received from server
		private List<AGameEvent> _lastServerGameEvents;
		private List<AGameObject> _lastServerSynchroFrame;

		private readonly object _gameEventLocker = new object();
		private readonly object _synchroFrameLocker = new object();

		#endregion

		#region timers and constants

		private readonly Timer _eventTimer;
		private readonly Timer _synchroFrameTimer;

		private const int EVENT_TIMER_DELAY_TIME = 25;
		private const int SYNCHRO_FRAME_DELAY_TIME = 500;

		#endregion

		private void InitializeConnection()
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

		#region run/stop thread, initialization

		private void InitializeThreadAndTimers()
		{
			_thread = new Thread(Run)
			{
				Name = "ConnectionManager"
			};
			_thread.Start();

			_lastClientGameEvents = new Queue<AGameEvent>();

			_eventTimer.Elapsed += (sender, args) =>
			{
				AGameEvent emptyGameEvent = new EmptyEvent(null, 0);
				AddClientGameEvent(emptyGameEvent);
			};
			_synchroFrameTimer.Elapsed += (sender, args) => GetLatestServerSynchroFrame();

			_lastServerGameEvents = new List<AGameEvent>(MAX_SERVER_GAME_EVENTS);
			_lastServerSynchroFrame = new List<AGameObject>(MAX_SERVER_GAME_EVENTS);

			// getting first synchroFrame
			GetLatestServerSynchroFrame();

			_eventTimer.Start();
			_synchroFrameTimer.Start();
		}

		public void Run()
		{
			while (true)
			{
				AGameEvent gameEvent = null;
				// Trace.WriteLine("Working", "Information");

				lock (_locker)
				{
					if (_lastClientGameEvents.Count > 0)
					{
						gameEvent = _lastClientGameEvents.Dequeue();
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

			_eventTimer.Stop();
			_synchroFrameTimer.Stop();

		}

		public void Dispose()
		{
			// stopping thread
			AddClientGameEvent(null);
			_thread.Join();

			// close EventWaitHandle
			_queue.Close();

			_eventTimer.Dispose();
			_synchroFrameTimer.Dispose();
		}

		#endregion

		#region getting the last synchroFrame and game events from server

		public void GetLatestServerSynchroFrame()
		{
			try
			{
				lock (_synchroFrameLocker)
				{
					AGameObject[] serverGameObjects = _service.SynchroFrame();

					if (serverGameObjects == null)
					{
						_lastServerSynchroFrame = null;
					}
					else
					{
						_lastServerSynchroFrame.Clear();
						_lastServerSynchroFrame.AddRange(_service.SynchroFrame());
					}
				}
			}
			catch (Exception exc)
			{
				Trace.WriteLine("game:SynchroFrame" + exc);
				// todo rewrite
				Debug.Assert(_lastServerSynchroFrame != null, "_lastServerSynchroFrame != null");
				_lastServerSynchroFrame.Clear();
			}
		}

		public void GetLatestServerGameEvents()
		{
			AGameEvent[] newServerEvents;
			try
			{
				newServerEvents = _service.GetEvents();
			}
			catch (Exception e)
			{
				Trace.WriteLine("game:GetEvents" + e);
				FatalError(e);
				return;
			}
			lock (_gameEventLocker)
			{
				_lastServerGameEvents.AddRange(newServerEvents);
			}
		}

		#endregion

		#region sending client game events

		private void AddClientGameEvent(AGameEvent gameEvent)
		{
			lock (_locker)
				_lastClientGameEvents.Enqueue(gameEvent);
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
						GetLatestServerGameEvents();
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

		#region service implementation

		/// <summary>
		/// ¬озвращает последние событи€ от сервера, которые были получены с помощью другого потока
		/// »спользуетс€ клиентом
		/// </summary>
		public AGameEvent[] GetEvents()
		{
			AGameEvent[] events;
			lock (_gameEventLocker)
			{
				events = _lastServerGameEvents.ToArray();
				_lastServerGameEvents.Clear();
			}
			// Logger.PrintEvents(events);
			return events;
		}

		public AGameObject[] SynchroFrame()
		{
			AGameObject[] synchroFrame;
			lock (_synchroFrameLocker)
			{
				if (_lastServerSynchroFrame == null)
					return null;
				synchroFrame = _lastServerSynchroFrame.ToArray();
				_lastServerSynchroFrame.Clear();

				// Trace.WriteLine("SYNCHRO_FRAME");
			}
			return synchroFrame;
		}

		public void Move(XNA.Framework.Vector2 direction)
		{
			AGameEvent moveGameEvent = new ObjectDirectionChanged(direction, null, 0);

			AddClientGameEvent(moveGameEvent);
		}

		public void Shoot(XNA.Framework.Vector2 direction)
		{
			AGameEvent shootGameEvent = new ObjectShootEvent(direction, null, 0);

			AddClientGameEvent(shootGameEvent);
		}

		public void ChangeWeapon(WeaponType type)
		{
			AGameEvent weaponChangedGameEvent = new WeaponChanged(null, type, null, 0);

			AddClientGameEvent(weaponChangedGameEvent);
		}

		public Stats? GetStats()
		{
			try
			{
				return _service.GetStats();
			}
			catch (Exception e)
			{
				FatalError(e);
				return null;
			}
		}

		#endregion

		#region other service methods

		public AccountManagerErrorCode Register(string username, string password)
		{
			// initialize connection
			InitializeConnection();

			try
			{
				return _service.Register(username, HashHelper.GetMd5Hash(password));
			}
			catch (Exception e)
			{
				FatalError(e);
				return AccountManagerErrorCode.UnknownExceptionOccured;
			}
		}

		public Guid? Login(string username, string password, out AccountManagerErrorCode accountManagerErrorCode)
		{
			// initialize connection
			InitializeConnection();

			accountManagerErrorCode = AccountManagerErrorCode.Ok;

			Guid? login = null;
			try
			{
				login = _service.Login(username, HashHelper.GetMd5Hash(password), out accountManagerErrorCode);
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

		public GameLevel GameStart(int gameId)
		{
			try
			{
				var level = _service.GameStart(gameId);
				if (level != null)
				{
					InitializeThreadAndTimers();
				}
				return level;
			}
			catch (Exception e)
			{
				FatalError(e);
				throw;
			}
		}

		public string[] PlayerListUpdate()
		{
			try
			{
				return _service.PlayerListUpdate();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc);
				return new string[] { };
			}
		}

		#endregion
	}
}
