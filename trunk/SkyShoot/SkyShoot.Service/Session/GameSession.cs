using System;
using System.Collections.Generic;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon.Projectiles;
using SkyShoot.Service.Bonuses;
using SkyShoot.Service.Mobs;
using SkyShoot.XNA.Framework;
using System.Timers;
using SkyShoot.Contracts;
using System.Diagnostics;
using SkyShoot.ServProgram.Session;
using SkyShoot.Service.Bonus;
using SkyShoot.Contracts.GameEvents;

namespace SkyShoot.Service.Session
{
	
	public class GameSession
	{
		// merged to _gameObjects
		//public List<MainSkyShootService> Players { get; set; }

		private List<AGameObject> _gameObjects;
		//private ObjectPool<AGameObject> _mobs;// { get; set; }
		//private List<AProjectile> _projectiles;
		//private ObjectPool<AProjectile> _projectiles;
		//private List<Mob> _mobs { get; set; }
		//private List<AGameBonus> _bonuses { get; set; }
		//private ObjectPool<AProjectile> _projectiles { get; set; }

		public GameDescription LocalGameDescription { get; private set; }

		public bool IsStarted { get; set; }
		public GameLevel GameLevel { get; private set; }
		private SpiderFactory _spiderFactory;
		private BonusFactory _bonusFactory;
		private long _timerCounter;
		private long _intervalToSpawn = 0;

		private long _lastUpdate;
		private long _updateDelay;

		private Timer _gameTimer;
		private object _updating;
// ReSharper disable FieldCanBeMadeReadOnly.Local
		private WallFactory wallFactory;
// ReSharper restore FieldCanBeMadeReadOnly.Local

		public GameSession(TileSet tileSet, int maxPlayersAllowed, GameMode gameType, int gameID)
		{
			IsStarted = false;
			GameLevel = new GameLevel(tileSet);

			var playerNames = new List<string>();

			_gameObjects = new List<AGameObject>();
			//_mobs = new ObjectPool<AGameObject>();
			//_bonuses = new List<AGameBonus>();
			//Players = new List<MainSkyShootService>();
			//_projectiles = new List<AProjectile>();
			//_projectiles = new ObjectPool<AProjectile>();
			//_gameEventStack = new Stack<AGameEvent>();

			LocalGameDescription = new GameDescription(playerNames, maxPlayersAllowed, gameType, gameID, tileSet);
			_spiderFactory = new SpiderFactory(GameLevel);
			_bonusFactory = new BonusFactory();

			// создание стенок
			wallFactory = new WallFactory(GameLevel);
		}

		//public event SomebodyMovesHandler SomebodyMoves; 
		//public event SomebodyDiesHandler SomebodyDies;
		//public event SomebodyHitHandler SomebodyHit;
		
		private void SomebodyMoved(AGameObject sender, Vector2 direction)
		{
			AGameBonus speedUpBonus = (sender as MainSkyShootService).GetBonus(AGameObject.EnumObjectType.Speedup);
			float speedUp = speedUpBonus == null ? 1f : speedUpBonus.DamageFactor;
			direction.X *= speedUp;
			direction.Y *= speedUp;
			sender.RunVector = direction;
			PushEvent(new ObjectDirectionChanged(direction,sender.Id,_timerCounter));
		}

		private void SomebodyShot(AGameObject sender, Vector2 direction)
		{
			sender.ShootVector = direction;
			
			if (sender.Weapon != null)
			{
				if (sender.Weapon.Reload(System.DateTime.Now.Ticks / 10000))
				{
					var a = sender.Weapon.CreateBullets(sender, direction);
					AGameBonus doubleDamage = (sender as MainSkyShootService).GetBonus(AGameObject.EnumObjectType.DoubleDamage);
					float damage = doubleDamage == null ? 1f : doubleDamage.DamageFactor;
					(sender as MainSkyShootService).Weapon.ApplyModifier(a, damage);
					foreach (var b in a)
					{
						//_projectiles.Add(b);
						//_projectiles.GetInActive().Copy(b);
						lock (_gameObjects)
						{
							_gameObjects.Add(b);// GetInActive().Copy(b);
						}
						PushEvent(new NewObjectEvent(b,_timerCounter));
					}
					//Trace.WriteLine("projectile added", "GameSession");
				}
			}
		}

		public void SomebodyDied(AGameObject sender)
		{
			PushEvent(new ObjectDeleted(sender.Id, _timerCounter));
		}

		public void SomebodySpawned(AGameObject sender) 
		{			
			PushEvent(new NewObjectEvent(sender, _timerCounter));
		}

		private void NewBonusDropped(AGameObject bonus)
		{
			_gameObjects.Add((AGameBonus) bonus);
			PushEvent(new NewObjectEvent(bonus, _timerCounter));
		}

		public void MobDead(AGameObject mob)
		{
			//SomebodyDied(mob);
			//mob.MeMoved -= SomebodyMoved;
			if (mob.Is(AGameObject.EnumObjectType.LivingObject))
			{
				AGameBonus b = _bonusFactory.CreateBonus(mob.Coordinates);
				b.IsActive = true;
				_gameObjects.Add(b);
				NewBonusDropped(b);
			}

			if(mob.IsPlayer)
			{
				PlayerDead(mob as MainSkyShootService);
				return;
			}
			PushEvent(new ObjectDeleted(mob.Id,_timerCounter));
			_gameObjects.Remove(mob);
		}

		public void SomebodyHitted(AGameObject mob, AProjectile projectile)
		{
			PushEvent(new ObjectHealthChanged(mob.HealthAmount, mob.Id, _timerCounter));
		}

		public void PlayerLeave(MainSkyShootService player)
		{
			LocalGameDescription.Players.Remove(player.Name);

			player.MeMoved -= SomebodyMoved;
			player.MeShot -= SomebodyShot;

			//Players.Remove(player);
			_gameObjects.Remove(player);
			Trace.WriteLine(player.Name + "leaved game");
		}

		private void PushEvent(AGameEvent gameEvent)
		{
			foreach (var player in _gameObjects)
			{
				if (player.IsPlayer)
				{
					(player as MainSkyShootService).NewEvents.Enqueue(gameEvent);
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

			SomebodyDied(player);			
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
				if(!_gameObjects[i].IsPlayer)
					continue;
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

			_gameObjects.AddRange(wallFactory.CreateWalls());

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
				var startThread = new System.Threading.Thread(new System.Threading.ThreadStart(Start));
				startThread.Start();
			}
			return true;
		}

		private void TimerElapsedListener(object sender, EventArgs e)
		{			
			Update();
					
		}

	#region local functions
		public void SpawnMob()
		{
#if DEBUG
			//!! debug
			var t = _gameObjects.FindAll(m => m.ObjectType == AGameObject.EnumObjectType.Mob);
			if(t != null && t.Count > 2)
				return;
#endif
			if (_intervalToSpawn == 0)
			{
				_intervalToSpawn = (long) Math.Exp(4.8 - (float)_timerCounter/40000f);
				
				var mob = _spiderFactory.CreateMob();
				// System.Diagnostics.Trace.WriteLine("mob spawned" + mob.Id);
				
				_gameObjects.Add(mob);
				SomebodySpawned(mob);
				//mob.MeMoved += new SomebodyMovesHandler(SomebodyMoved);
			}
			else
			{
				_intervalToSpawn--;
			}
		}

		/// <summary>
		/// здесь будут производится обработка всех действий
		/// </summary>
		private void Update() 
		{
			if (!System.Threading.Monitor.TryEnter(_updating)) return;

			// Trace.WriteLine("update begin "+ _timerCounter);
			SpawnMob();
			 var now = DateTime.Now.Ticks/10000;
			_updateDelay = now - _lastUpdate;
			_lastUpdate = now;

			//int shift = Players.Count;
			lock (_gameObjects)
			{

				for (int i = 0; i < /*shift + /**/ _gameObjects.Count; i++)
				{
					AGameObject activeObject;
					//if (i < shift)
					//{
					//  activeObject = Players[i];
					//}
					//else
					//{
					activeObject = _gameObjects[i];// - shift];
					//}
					// объект не существует
					if (!activeObject.IsActive)
					{
						continue;
					}
					//var t = new List<AGameObject>(Players);
					//t.AddRange(_gameObjects);
					activeObject.Think(_gameObjects, now);

					var newCoord = activeObject.ComputeMovement(_updateDelay, GameLevel);
					var canMove = true;
					/* <b>int j = 0</b> потому что каждый с каждым, а действия не симметричны*/
					for (int j = 0; j < /*shift +/**/ _gameObjects.Count; j++)
					{
						// тот же самый объект. сам с собой он ничего не делает
						if (i == j)
						{
							continue;
						}
						AGameObject slaveObject;
						//if (j < shift)
						//{
						//  slaveObject = Players[j];
						//}
						//else
						//{
						slaveObject = _gameObjects[j];// - shift];
						//}
						// объект не существует
						if (!slaveObject.IsActive)
						{
							continue;
						}
						//!! rewrite sqrt!!
						// объект далеко. не рассамтриваем
						var rR = (activeObject.Radius + slaveObject.Radius);
						if (Vector2.DistanceSquared(newCoord, slaveObject.Coordinates) > rR*rR &&
							Vector2.DistanceSquared(activeObject.Coordinates, slaveObject.Coordinates) > rR * rR)
						{
							continue;
						}
						activeObject.Do(slaveObject, now);
						//!! @todo rewrite
						if (!slaveObject.IsBullet && !activeObject.IsBullet && 
						    slaveObject.Is(AGameObject.EnumObjectType.Bonus) &&
							activeObject.Is(AGameObject.EnumObjectType.Bonus))
						{
							//удаляемся ли мы от объекта
							// если да, то можем двигаться
							canMove = Vector2.DistanceSquared(activeObject.Coordinates, slaveObject.Coordinates) <
							          Vector2.DistanceSquared(newCoord, slaveObject.Coordinates);
						}
					}
					if (canMove)
					{
						activeObject.Coordinates = newCoord;
					}
					else
					{
						activeObject.RunVector = Vector2.Zero;
					}
				}

				//for (int i = 0; i < Players.Count; i++)
				//{
				//  var activeObject = Players[i];
				//  if (!activeObject.IsActive)
				//  {
				//    PlayerDead(activeObject);
				//  }
				//}
				for (int i = 0; i < _gameObjects.Count; i++)
				{
					if (!_gameObjects[i].IsActive)
					{
						MobDead(_gameObjects[i]);
					}

				}

				_gameObjects.RemoveAll(m => !m.IsActive);
			}

			/*
			for (int i = 0; i < _gameObjects.Count; i++)
			{
				var mob = _mobs[i];
				Vector2 oldVector = new Vector2(mob.RunVector.X,mob.RunVector.Y);
				mob.Think(gameObjects);
				mob.Coordinates = ComputeMovement(mob);
				if ((mob.RunVector - oldVector).Length() > 0.001) 
				{
					SomebodyMoved(mob, mob.RunVector);
				}
				//System.Diagnostics.Trace.WriteLine("Mob cord: " + mob.Coordinates); 

			}

			for(int i = 0; i < Players.Count; i++)
			{
				var player = Players[i];
				player.Coordinates = ComputeMovement(player);
				player.DeleteExpiredBonuses(_lastUpdate);

				//Проверка на касание игрока и моба
				HitTestTouch(player);

				List<AGameBonus> bonuses2delete = new List<AGameBonus>();
				for(int j = 0; j < _bonuses.Count; j ++)
				{
					var bonus = _bonuses[j];
					if (Vector2.Distance(bonus.Coordinates, player.Coordinates) < player.Radius)
					{
						if (!bonus.IsActive)
						{
							continue;
						}
						player.AddBonus(bonus);
						bonus.taken(_lastUpdate);
						bonus.IsActive = false;
						PushEvent(new ObjectDeleted(bonus.Id, _timerCounter));
						bonuses2delete.Add(bonus);
					} 
				}
				foreach (AGameBonus bonus in bonuses2delete)
				{
					_bonuses.Remove(bonus);
				}
			}
			// Trace.WriteLine("" + _projectiles.size);
			for (var pr = _projectiles.FirstActive; pr != null; pr = _projectiles.Next(pr) )
			{
				
				//if (pr == null || pr.Item == null) continue;
				//var projectile = pr.Item;
				if (projectile.LifeDistance <= 0)
				{
					projectile.IsActive = false;
					//pr.isActive = false;
					continue;
				}
				//var projectile = _projectiles[i];
				var newCord = projectile.Coordinates + projectile.RunVector * projectile.Speed * _updateDelay;

				//Проверка на касание пули и моба
				var hitedMob = hitTestProjectile(projectile, newCord);
				if (hitedMob == null)
				{
					projectile.OldCoordinates = projectile.Coordinates;
					projectile.Coordinates = newCord;
					projectile.LifeDistance -= Vector2.Distance(projectile.Coordinates, projectile.OldCoordinates);
				}
				else
				{
					projectile.Do(hitedMob);
					//hitedMob.DamageTaken(projectile);
					SomebodyHitted(hitedMob, projectile);
					if (hitedMob.HealthAmount <= 0)
					{
						MobDead(hitedMob);
					}
					projectile.LifeDistance = -1;
				}

			}
			/**/

			//_projectiles.RemoveAll(x => (x==null) || (x.LifeDistance <= 0));
			// Trace.WriteLine(System.DateTime.Now.Ticks/10000 - now);
			// Trace.WriteLine("update end " + _timerCounter);
			_timerCounter++;
			//_updated = false;
			System.Threading.Monitor.Exit(_updating);
		}
		
		private AGameObject HitTestProjectile(AProjectile projectile, Vector2 newCord)
		{
			var prX = newCord.X - projectile.Coordinates.X;
			var prY = newCord.Y - projectile.Coordinates.Y;

			AGameObject hitedTarget = null;
			var minDist = double.MaxValue;

			for (int i = 0; i < _gameObjects.Count; i++)
			{
				var mob = _gameObjects[i];
				var mX = mob.Coordinates.X - projectile.Coordinates.X;
				var mY = mob.Coordinates.Y - projectile.Coordinates.Y;
				var mR = mob.Radius;
				var mDist = Math.Sqrt(mX * mX + mY * mY);

				if (mDist <= mR && mDist < minDist)
				{
					//!! hitedTarget = mob;
					minDist = mDist;
					continue;
				}

				if (prX == 0 && prY == 0)
				{
					continue;
				}

				var h = Math.Abs(prX * mY - prY * mX) / Math.Sqrt(prX * prX * + prY * prY);

				//@TODO Проверка углов. Над ней еще надо будет подумать.
				var cos1 = mX * prX + mY * prY;
				var cos2 = -1 * (prX * (mX - prX) + prY * (mY - prY));

				if (h <= mR && Math.Sign(cos1) == Math.Sign(cos2) && mDist < minDist)
				{
					hitedTarget = mob;
					minDist = mDist;
				}

			}

			return hitedTarget;
		}

		private void HitTestTouch(MainSkyShootService player)
		{
			for (int i = 0; i < _gameObjects.Count;i++ )
			{
				var mob = _gameObjects[i];
				if ((mob.Coordinates - player.Coordinates).Length() <= mob.Radius + player.Radius)
				{
					if (mob.Weapon.Reload(DateTime.Now.Ticks / 10000))
					{
						AGameBonus shield = player.GetBonus(AGameBonus.EnumObjectType.Shield);
						float damage = shield == null ? 1f : shield.DamageFactor;
						player.HealthAmount -= damage * mob.Damage;
						SomebodyHitted(player, null);
					}

					if (player.HealthAmount <= 0)
					{
						PlayerDead(player);
					}
				}
			}
		} 


	#endregion

		public int PlayersCount()
		{
			return _gameObjects.FindAll(o => o.IsPlayer).Count;
		}

		private Vector2 ComputeMovement(AGameObject mob)
		{
			
			var newCoord = mob.RunVector*mob.Speed*_updateDelay + mob.Coordinates ;

			if (mob.IsPlayer)
			{
				newCoord.X = MathHelper.Clamp(newCoord.X, 0, GameLevel.levelHeight);
				newCoord.Y = MathHelper.Clamp(newCoord.Y, 0, GameLevel.levelWidth);
			}
			
			return newCoord;
		}
	}
}