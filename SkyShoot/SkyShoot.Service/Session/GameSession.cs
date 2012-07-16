using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using SkyShoot.Contracts;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.CollisionDetection;
using SkyShoot.Service.Bonus;
using SkyShoot.Service.Bonuses;
using SkyShoot.Service.Mobs;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Session
{	
	public class GameSession
	{
		private readonly List<AGameObject> _gameObjects;
		private readonly List<AGameObject> _newObjects;

		public TeamsList SessionTeamsList = new TeamsList();
		
		public GameDescription LocalGameDescription { get; private set; }

		public bool IsStarted { get; set; }
		public GameLevel GameLevel { get; private set; }
		private readonly SpiderFactory _spiderFactory;
		private readonly BonusFactory _bonusFactory;
		private readonly WallFactory _wallFactory;
		private long _timerCounter;
		private long _intervalToSpawn;

		private long _lastUpdate;
		private long _updateDelay;

		private Timer _gameTimer;
		private object _updating;

		public GameSession(TileSet tileSet, int maxPlayersAllowed,
			GameMode gameType, int gameID, int teams)
		{
		//	SessionTeamsList = new TeamsList();

			for (int i = 1; i <= teams; i++)
			{
				SessionTeamsList.Teams.Add(new Team(i));
			}
			
			IsStarted = false;
			GameLevel = new GameLevel(tileSet);

			var playerNames = new List<string>();

			_gameObjects = new List<AGameObject>();
			_newObjects = new List<AGameObject>();

			LocalGameDescription = new GameDescription(playerNames, maxPlayersAllowed, gameType, gameID, tileSet, teams);
			_spiderFactory = new SpiderFactory(GameLevel);
			_bonusFactory = new BonusFactory();

			// создание стенок
			_wallFactory = new WallFactory(GameLevel);
		}

		private void SomebodyChangedWeapon(AGameObject sender, SkyShoot.Contracts.Weapon.WeaponType type)
		{
			sender.ChangeWaponTo(type);
		}

		private void SomebodyMoved(AGameObject sender, Vector2 direction)
		{
			sender.RunVector = direction;
			lock (_gameObjects)
			{
				PushEvent(new ObjectDirectionChanged(direction, sender.Id, _timerCounter));
			}
		}

		private void SomebodyShot(AGameObject sender, Vector2 direction)
		{
			sender.ShootVector = direction;
			sender.ShootVector.Normalize();
			
			if (sender.Weapon != null)
			{
				if (sender.Weapon.IsReload(DateTime.Now.Ticks / 10000))
				{
					var a = sender.Weapon.CreateBullets(sender, direction);
					var player = sender as MainSkyShootService;
					if (player != null)
					{
						AGameBonus doubleDamage = player.GetBonus(AGameObject.EnumObjectType.DoubleDamage);
						float damage = doubleDamage == null ? 1f : doubleDamage.DamageFactor;
						player.Weapon.ApplyModifier(a, damage);
					}
					foreach (var b in a)
					{
						//_projectiles.Add(b);
						//_projectiles.GetInActive().Copy(b);
						lock (_newObjects)
						{
							_newObjects.Add(b);// GetInActive().Copy(b);
							PushEvent(new NewObjectEvent(b, _timerCounter));
						}
					}
					//Trace.WriteLine("projectile added", "GameSession");
				}
			}
		}

		//public void SomebodyDied(AGameObject sender)
		//{
		//  PushEvent(new ObjectDeleted(sender.Id, _timerCounter));
		//}

		//public void SomebodySpawned(AGameObject sender) 
		//{			
		//}

		private IEnumerable<AGameEvent> NewBonusDropped(AGameObject bonus, long time)
		{
			_newObjects.Add(bonus);
			return new[] { new NewObjectEvent(bonus, time) };
		}

		public IEnumerable<AGameEvent> MobDead(AGameObject mob, long time)
		{
			// result = 
			var r = new List<AGameEvent>();
			//SomebodyDied(mob);
			//mob.MeMoved -= SomebodyMoved;
			if ((mob.Is(AGameObject.EnumObjectType.LivingObject)) && (mob.Is(AGameObject.EnumObjectType.Poisoning) == false))
			{
				AGameBonus b = _bonusFactory.CreateBonus(mob.Coordinates);
				b.IsActive = true;
				//_NewObjects.Add(b);
				r.AddRange(NewBonusDropped(b, time));
			}

			if(mob.Is(AGameObject.EnumObjectType.Player))
			{
				PlayerDead(mob as MainSkyShootService);
			}
			r.Add(new ObjectDeleted(mob.Id, _timerCounter));
			// will be delete later
			//_gameObjects.Remove(mob);
			return r;
		}

		//public void SomebodyHitted(AGameObject mob, AProjectile projectile)
		//{
		//  PushEvent(new ObjectHealthChanged(mob.HealthAmount, mob.Id, _timerCounter));
		//}

		public void PlayerLeave(MainSkyShootService player)
		{
			LocalGameDescription.Players.Remove(player.Name);

			player.MeMoved -= SomebodyMoved;
			player.MeShot -= SomebodyShot;
			player.MeChangeWeapon -= SomebodyChangedWeapon;

			//Players.Remove(player);
			lock (_gameObjects)
			{
				player.TeamIdentity.members.Remove(player);
				_gameObjects.Remove(player);
			}
			Trace.WriteLine(player.Name + " leaved game");
		}

		private void PushEvent(AGameEvent gameEvent)
		{
			foreach (var playerConverted in _gameObjects.Where(player => player.Is(AGameObject.EnumObjectType.Player)).OfType<MainSkyShootService>())
			{
				lock (playerConverted.NewEvents)
				{
					playerConverted.NewEvents.Enqueue(gameEvent);
				}
			}
		}

		public void PlayerDead(MainSkyShootService player)
		{
			if(player == null)
			{
				Trace.WriteLine("strange error");
				return;
			}
			//player.GameOver();

			//SomebodyDied(player);			
			player.Disconnect();//временно
		}

		public void Stop()
		{
			if (_gameTimer != null)
			{
				_gameTimer.Enabled = false;
				_gameTimer.AutoReset = false;
				_gameTimer.Elapsed -= TimerElapsedListener;
			}
			IsStarted = false;
		}

		public AGameObject[] GetSynchroFrame()
		{
			var allObjects = new List<AGameObject>(_gameObjects.ToArray());
			//allObjects.AddRange(Players);
			// Trace.WriteLine("SynchroFrame");
			return allObjects.ToArray();

		}

		public void Start()
		{
			for(int i = 0; i < _gameObjects.Count; i++)
			{
				if (!_gameObjects[i].Is(AGameObject.EnumObjectType.Player))
				{
					_gameObjects[i].TeamIdentity.members.Add(_gameObjects[i]);
					_gameObjects[i].TeamIdentity = SessionTeamsList.GetTeamByNymber(1);
				}
				var player = _gameObjects[i] as MainSkyShootService;
				if (player == null)
				{
					Trace.WriteLine("Error: !!! IsPlayer true for non player object");
					continue;
				}
				//this.SomebodyMoves += player.MobMoved;
				player.MeMoved += SomebodyMoved;
				//this.SomebodyShoots += player.MobShot;
				player.MeShot += SomebodyShot;

				player.MeChangeWeapon += SomebodyChangedWeapon;
				//this.SomebodySpawns += player.SpawnMob;

				//this.SomebodyDies += player.MobDead;
				
				//this.SomebodyHit += player.Hit;

				player.Coordinates = new Vector2(500,500);
				player.Speed = Constants.PLAYER_DEFAULT_SPEED;
				player.Radius = Constants.PLAYER_RADIUS;
				player.Weapon = new Weapon.Pistol(Guid.NewGuid(), player);
				player.RunVector = new Vector2(0, 0);
				player.MaxHealthAmount = player.HealthAmount = 100f;
				
			}

			_gameObjects.AddRange(_wallFactory.CreateWalls());

			System.Threading.Thread.Sleep(1000);
			if (!IsStarted)
			{
				IsStarted = true;
			}
			_timerCounter = 0;
			_updating = false;

			_lastUpdate = DateTime.Now.Ticks/10000;
			_updateDelay = 0;
			_gameTimer = new Timer(Constants.FPS);
			_gameTimer.AutoReset = true;
			_gameTimer.Elapsed += TimerElapsedListener;
			_gameTimer.Start();
			Trace.WriteLine("Game Started");
		}

		public bool AddPlayer(MainSkyShootService player)
		{
			if (_gameObjects.Count >= LocalGameDescription.MaximumPlayersAllowed || IsStarted)
				return false;
			
			_gameObjects.Add(player);
			LocalGameDescription.Players.Add(player.Name);
			var names = new String[_gameObjects.Count];

			//UpdatePlayersList(player);

			//if (NewPlayerConnected != null) NewPlayerConnected(player);

			//StartGame += player.GameStart;

			if (_gameObjects.Count == LocalGameDescription.MaximumPlayersAllowed)
			{
				// Trace.WriteLine("player added"+player.Name);
				var startThread = new System.Threading.Thread(Start);
				startThread.Start();
			}

			return true;
		}

		private void TimerElapsedListener(object sender, EventArgs e)
		{			
			Update();
		}

	#region local functions
		public IEnumerable<AGameEvent> SpawnMob(long time)
		{
			var r = new List<AGameEvent>();
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
				_intervalToSpawn = 3 * (long) Math.Exp(4.8f - _timerCounter/4000f);
				
				var mob = _spiderFactory.CreateMob();
				// System.Diagnostics.Trace.WriteLine("mob spawned" + mob.Id);
				
				_newObjects.Add(mob);
				r.Add(new NewObjectEvent(mob, time));

			}
			else
			{
				_intervalToSpawn--;
			}
			return r;
		}

		/// <summary>
		/// здесь будут производится обработка всех действий
		/// </summary>
		private void Update()
		{
			if (!System.Threading.Monitor.TryEnter(_updating)) return;

			// Trace.WriteLine("update begin "+ _timerCounter);
			var now = DateTime.Now.Ticks / 10000;
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
					if (!activeObject.IsActive)
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
							newCoord += CollisionDetector.FitObjects(newCoord, activeObject.RunVector, activeObject.Bounding, slaveObject.Coordinates, slaveObject.RunVector, slaveObject.Bounding);
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
					if ((coordDiff - activeObject.PrevMoveDiff).LengthSquared() > Constants.Epsilon)
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
				foreach (var ev in eventsCash)
				{
					PushEvent(ev);
				}

				_gameObjects.RemoveAll(m => !m.IsActive);
				lock (_newObjects)
				{
					_gameObjects.AddRange(_newObjects);
					_newObjects.Clear();
				}
			}

			//_projectiles.RemoveAll(x => (x==null) || (x.LifeDistance <= 0));
			// Trace.WriteLine(System.DateTime.Now.Ticks/10000 - now);
			// Trace.WriteLine("update end " + _timerCounter);
			_timerCounter++;
			//_updated = false;
			System.Threading.Monitor.Exit(_updating);
		}

		public int PlayersCount()
		{
			return _gameObjects.FindAll(o => o.Is(AGameObject.EnumObjectType.Player)).Count;
		}

	#endregion
	}
}