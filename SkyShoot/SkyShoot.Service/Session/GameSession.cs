using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using SkyShoot.Contracts;
using SkyShoot.Contracts.CollisionDetection;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Utils;
using SkyShoot.Service.Bonus;
using SkyShoot.Service.Bonuses;
using SkyShoot.Service.Mobs;
using SkyShoot.ServProgram.Mobs;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Session
{
	public class GameSession
	{
		#region private fields

		protected readonly List<AGameObject> _gameObjects;
		protected readonly List<AGameObject> _newObjects;

		protected readonly SpiderFactory _spiderFactory;
		protected readonly BonusFactory _bonusFactory;
		protected readonly WallFactory _wallFactory;

		protected readonly TeamsList _sessionTeamsList = new TeamsList();

		protected long _timerCounter;
		protected long _intervalToSpawn;

		protected long _lastUpdate;
		protected long _updateDelay;

		protected Timer _gameTimer;
		protected object _updating;

		protected TimeHelper _timeHelper;

		#endregion

		public GameDescription LocalGameDescription { get; private set; }
		public bool IsStarted { get; protected set; }
		public GameLevel GameLevel { get; private set; }

		public GameSession(TileSet tileSet, int maxPlayersAllowed,
			GameMode gameType, int gameID, int teams)
		{
			//	SessionTeamsList = new TeamsList();

			for (int i = 1; i <= teams; i++)
			{
				_sessionTeamsList.Teams.Add(new Team(i));
			}

			IsStarted = false;
			GameLevel = new GameLevel(Constants.LEVEL_WIDTH, Constants.LEVEL_HEIGHT, tileSet);

			var playerNames = new List<string>();

			_gameObjects = new List<AGameObject>();
			_newObjects = new List<AGameObject>();

			LocalGameDescription = new GameDescription(playerNames, maxPlayersAllowed, gameType, gameID, tileSet, teams);
			if (gameType == GameMode.Deathmatch)
			{
				_spiderFactory = new DeathmatchSpiderFactory(GameLevel);
			}
			else
			{
				_spiderFactory = new DefaultSpiderFactory(GameLevel);
			}
			_bonusFactory = new BonusFactory();
			_wallFactory = new WallFactory(GameLevel);
		}

		#region private methods

		public virtual void Start()
		{
			#region инициализация объектов

			var randomNumberGenerator = new Random();

			for (int i = 0; i < _gameObjects.Count; i++)
			{
				if (!_gameObjects[i].Is(AGameObject.EnumObjectType.Player))
					continue;

				var player = _gameObjects[i] as MainSkyShootService;
				if (player == null)
				{
					Trace.WriteLine("Error: !!! IsPlayer true for non player object");
					continue;
				}

				_gameObjects[i].TeamIdentity = _sessionTeamsList.GetTeamByNymber(1);
				_gameObjects[i].TeamIdentity.Members.Add(_gameObjects[i]);

				player.MeMoved += SomebodyMoved;
				player.MeShot += SomebodyShot;
				player.MeChangeWeapon += SomebodyChangedWeapon;

				player.Coordinates = new Vector2(500 + (randomNumberGenerator.Next() % 200 - 100), 500 + (randomNumberGenerator.Next() % 200 - 100));
				player.Speed = Constants.PLAYER_DEFAULT_SPEED;
				player.Radius = Constants.PLAYER_RADIUS;
				player.Weapon = new Weapon.Pistol(Guid.NewGuid(), player);
				player.RunVector = new Vector2(0, 0);
				player.MaxHealthAmount = player.HealthAmount = Constants.PLAYER_HEALTH;
			}

			_gameObjects.AddRange(_wallFactory.CreateWalls());

			#endregion

			_timeHelper = new TimeHelper(TimeHelper.NowMilliseconds);

			_timerCounter = 0;
			_updating = false;

			_lastUpdate = 0;
			_updateDelay = 0;

			_gameTimer = new Timer(Constants.FPS) { AutoReset = true };
			_gameTimer.Elapsed += TimerElapsedListener;
			_gameTimer.Start();

			// todo номер игры
			Trace.Listeners.Add(new Logger(Logger.SolutionPath + "\\logs\\server_game_" + LocalGameDescription.GameId + ".txt", _timeHelper) {Name = "game logger"});

			Trace.WriteLine("Game Started");
			
			Trace.Listeners.Remove(Logger.ServerLogger);

			IsStarted = true;
		}

		protected void SomebodyChangedWeapon(AGameObject sender, Contracts.Weapon.WeaponType type)
		{
			sender.ChangeWaponTo(type);
		}

		protected void SomebodyMoved(AGameObject sender, Vector2 direction)
		{
			sender.RunVector = direction;
			lock (_gameObjects)
			{
				PushEvent(new ObjectDirectionChanged(direction, sender.Id, _timeHelper.GetTime()));
			}
		}

		protected void SomebodyShot(AGameObject sender, Vector2 direction)
		{
			sender.ShootVector = direction;
			sender.ShootVector.Normalize();

			if (sender.Weapon != null)
			{
				if (sender.Weapon.IsReload(DateTime.Now.Ticks / 10000))
				{
					var bullets = sender.Weapon.CreateBullets(direction);
					var player = sender as MainSkyShootService;
					if (player != null)
					{
						AGameBonus doubleDamage = player.GetBonus(AGameObject.EnumObjectType.DoubleDamage);
						float damage = doubleDamage == null ? 1f : doubleDamage.DamageFactor;
						player.Weapon.ApplyModifier(bullets, damage);
					}
					foreach (var bullet in bullets)
					{
						lock (_newObjects)
						{
							_newObjects.Add(bullet);
							PushEvent(new NewObjectEvent(bullet, _timeHelper.GetTime()));
						}
					}
					//Trace.WriteLine("projectile added", "GameSession");
				}
			}
		}

		protected IEnumerable<AGameEvent> NewBonusDropped(AGameObject bonus, long time)
		{
			_newObjects.Add(bonus);
			return new[] { new NewObjectEvent(bonus, time) };
		}

		protected void PushEvent(AGameEvent gameEvent)
		{
			foreach (var playerConverted in _gameObjects.Where(player => player.Is(AGameObject.EnumObjectType.Player)).OfType<MainSkyShootService>())
			{
				lock (playerConverted.NewEvents)
				{
					playerConverted.NewEvents.Enqueue(gameEvent);
				}
			}
		}

		protected void PushEvents(IList<AGameEvent> events)
		{
			foreach (var playerConverted in _gameObjects.Where(player => player.Is(AGameObject.EnumObjectType.Player)).OfType<MainSkyShootService>())
			{
				lock (playerConverted.NewEvents)
				{
					foreach (AGameEvent aGameEvent in events)
					{
						playerConverted.NewEvents.Enqueue(aGameEvent);	
					}
				}
			}
		}

		protected void TimerElapsedListener(object sender, EventArgs e)
		{
			Update();
		}

		/// <summary>
		/// здесь будут производится обработка всех действий
		/// </summary>
		public virtual void Update()
		{
			if (!System.Threading.Monitor.TryEnter(_updating)) return;

			// Trace.WriteLine("update begin "+ _timerCounter);
			var now = _timeHelper.GetTime();

			_updateDelay = now - _lastUpdate;
			_lastUpdate = now;

			var eventsCash = new List<AGameEvent>(_gameObjects.Count * 3);

			eventsCash.AddRange(SpawnMob(now));
			lock (_gameObjects)
			{
				for (int i = 0; i < _gameObjects.Count; i++)
				{
					var activeObject = _gameObjects[i];
					// объект не существует
					if (!activeObject.IsActive || activeObject.ObjectType == AGameObject.EnumObjectType.Wall)
					{
						continue;
					}

					lock (_newObjects)
					{
						eventsCash.AddRange(activeObject.Think(_gameObjects, _newObjects, now));
					}

					var newCoord = activeObject.ComputeMovement(_updateDelay, GameLevel);
					//var canMove = true;
					/* <b>int j = 0</b> потому что каждый с каждым, а действия не симметричны*/
					for (int j = 0; j < _gameObjects.Count; j++)
					{
						// тот же самый объект. сам с собой он ничего не делает
						if (i == j)
						{
							continue;
						}
						var slaveObject = _gameObjects[j];
						// объект не существует
						if (!slaveObject.IsActive)
						{
							continue;
						}
						// объект далеко. не рассматриваем
						var rR = (activeObject.Radius + slaveObject.Radius);
						if (Vector2.DistanceSquared(newCoord, slaveObject.Coordinates) > rR * rR &&
							Vector2.DistanceSquared(activeObject.Coordinates, slaveObject.Coordinates) > rR * rR)
						{
							continue;
						}
						lock (_newObjects)
						{
							eventsCash.AddRange(activeObject.Do(slaveObject, _newObjects, now));
						}
						if (slaveObject.Is(AGameObject.EnumObjectType.Block)
							&& activeObject.Is(AGameObject.EnumObjectType.Block))
						{
							//удаляемся ли мы от объекта
							// если да, то можем двигаться
							//canMove = Vector2.DistanceSquared(activeObject.Coordinates, slaveObject.Coordinates) <
							//		  Vector2.DistanceSquared(newCoord, slaveObject.Coordinates);
							newCoord += CollisionDetector.FitObjects(newCoord, activeObject.RunVector, activeObject.Bounding,
																	 slaveObject.Coordinates, slaveObject.RunVector, slaveObject.Bounding);
						}
					}
					var coordDiff = newCoord - activeObject.Coordinates;
					//coordDiff.Normalize();
					//if (canMove)
					//{
					activeObject.Coordinates = newCoord;
					//}
					//else
					//{
					//	activeObject.RunVector = Vector2.Zero;
					//}
					if ((coordDiff - activeObject.PrevMoveDiff).LengthSquared() > Constants.EPSILON)
					{
						eventsCash.Add(new ObjectDirectionChanged(activeObject.RunVector, activeObject.Id, now));
					}
					activeObject.PrevMoveDiff = coordDiff;
				}

				for (int i = 0; i < _gameObjects.Count; i++)
				{
					if (!_gameObjects[i].IsActive)
					{
						eventsCash.AddRange(MobDead(_gameObjects[i], now));
						eventsCash.AddRange(_gameObjects[i].OnDead(_gameObjects[i], _gameObjects, now));
					}
				}

				// flush of events cash
				PushEvents(eventsCash);

				_gameObjects.RemoveAll(m => !m.IsActive);
				lock (_newObjects)
				{
					_gameObjects.AddRange(_newObjects);
					_newObjects.Clear();
				}
			}

			// Trace.WriteLine(System.DateTime.Now.Ticks/10000 - now);
			// Trace.WriteLine("update end " + _timerCounter);
			_timerCounter++;
			//_updated = false;
			System.Threading.Monitor.Exit(_updating);
		}

		protected IEnumerable<AGameEvent> SpawnMob(long time)
		{
			var events = new List<AGameEvent>();
#if false
			// DEBUG 
			//!! debug
			var t = _gameObjects.FindAll(m => m.ObjectType == AGameObject.EnumObjectType.Mob);
			if(t != null && t.Count > 1)
				return new AGameEvent[]{};
#endif
			if (_intervalToSpawn < 1)
			{
				// todo //!! rewrite!!
				_intervalToSpawn = 3 * (long)Math.Exp(4.8f - _timerCounter / 4000f);

				var mob = _spiderFactory.CreateMob();
				// System.Diagnostics.Trace.WriteLine("mob spawned" + mob.Id);

				_newObjects.Add(mob);
				events.Add(new NewObjectEvent(mob, time));
			}
			else
			{
				_intervalToSpawn--;
			}
			return events;
		}

		protected IEnumerable<AGameEvent> MobDead(AGameObject mob, long time)
		{
			var events = new List<AGameEvent>();
			//SomebodyDied(mob);
			//mob.MeMoved -= SomebodyMoved;
			if ((mob.Is(AGameObject.EnumObjectType.LivingObject)) && !mob.Is(AGameObject.EnumObjectType.Poisoning) && !mob.Is(AGameObject.EnumObjectType.Turret) && !mob.Is(AGameObject.EnumObjectType.Caterpillar))
			{
				AGameBonus gameBonus = _bonusFactory.CreateBonus(mob.Coordinates);
				gameBonus.IsActive = true;
				events.AddRange(NewBonusDropped(gameBonus, time));
			}

			if (mob.Is(AGameObject.EnumObjectType.Player))
			{
				PlayerDead(mob as MainSkyShootService);
			}
			events.Add(new ObjectDeleted(mob.Id, _timeHelper.GetTime()));
			// will be delete later
			//_gameObjects.Remove(mob);
			return events;
		}

		protected virtual void PlayerDead(MainSkyShootService player)
		{
			if (player == null)
			{
				Trace.WriteLine("strange error");
				return;
			}
			//player.GameOver();
			//SomebodyDied(player);			
			player.Disconnect();//временно
			player.TeamIdentity.Members.Remove(player);
		}

		#endregion

		#region public methods

		public void PlayerLeave(MainSkyShootService player)
		{
			if (LocalGameDescription.Players.Contains(player.Name)) LocalGameDescription.Players.Remove(player.Name);

			else return;

			player.MeMoved -= SomebodyMoved;
			player.MeShot -= SomebodyShot;
			player.MeChangeWeapon -= SomebodyChangedWeapon;

			//Players.Remove(player);
			lock (_gameObjects)
			{
				//	player.TeamIdentity.members.Remove(player);
				_gameObjects.Remove(player);
			}
			Trace.WriteLine(player.Name + " leaved game");
		}

		public virtual void Stop()
		{
			if (_gameTimer != null)
			{
				_gameTimer.Enabled = false;
				_gameTimer.AutoReset = false;
				_gameTimer.Elapsed -= TimerElapsedListener;
			}
			Trace.Listeners.Add(Logger.ServerLogger);

			Trace.WriteLine("Game Over");

			Trace.Listeners.Remove("game");
			IsStarted = false;
		}

		public AGameObject[] GetSynchroFrame()
		{
			var allObjects = new List<AGameObject>(_gameObjects.ToArray());
			// Trace.WriteLine("SynchroFrame");
			return allObjects.ToArray();
		}

		public bool AddPlayer(MainSkyShootService player)
		{
			if (_gameObjects.Count >= LocalGameDescription.MaximumPlayersAllowed || IsStarted)
				return false;
			
			_gameObjects.Add(player);

			LocalGameDescription.Players.Add(player.Name);

			if (_gameObjects.Count == LocalGameDescription.MaximumPlayersAllowed)
			{
				// Trace.WriteLine("player added"+player.Name);
				var startThread = new System.Threading.Thread(Start);
				startThread.Start();
			}

			return true;
		}

		public int PlayersCount()
		{
			return _gameObjects.FindAll(o => o.Is(AGameObject.EnumObjectType.Player)).Count;
		}

		public long GetTime()
		{
			return _timeHelper.GetTime();
		}

		#endregion
	}
}