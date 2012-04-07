using System;
using System.Collections.Generic;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Service.Bonuses;
using SkyShoot.Service.Mobs;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.XNA.Framework;
using System.Timers;
using SkyShoot.Contracts;
using System.Diagnostics;
using SkyShoot.Service.Bonus;
using SkyShoot.Contracts.GameEvents;

namespace SkyShoot.Service.Session
{
	
	public class GameSession
	{
		private readonly List<AGameObject> _gameObjects;

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
			GameMode gameType, int gameID)
		{
			IsStarted = false;
			GameLevel = new GameLevel(tileSet);

			var playerNames = new List<string>();

			_gameObjects = new List<AGameObject>();

			LocalGameDescription = new GameDescription(playerNames, maxPlayersAllowed, gameType, gameID, tileSet);
			_spiderFactory = new SpiderFactory(GameLevel);
			_bonusFactory = new BonusFactory();

			// создание стенок
			_wallFactory = new WallFactory(GameLevel);
		}

		//public event SomebodyMovesHandler SomebodyMoves; 
		//public event SomebodyDiesHandler SomebodyDies;
		//public event SomebodyHitHandler SomebodyHit;
		
		private void SomebodyMoved(AGameObject sender, Vector2 direction)
		{
			var player = sender as MainSkyShootService;
			if (player != null)
			{
				AGameBonus speedUpBonus = player.GetBonus(AGameObject.EnumObjectType.Speedup);
				float speedUp = speedUpBonus == null ? 1f : speedUpBonus.DamageFactor;
				direction.X *= speedUp;
				direction.Y *= speedUp;
			}
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
			_gameObjects.Add(bonus);
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
				//_gameObjects.Add(b);
				NewBonusDropped(b);
			}

			if(mob.Is(AGameObject.EnumObjectType.Player))
			{
				PlayerDead(mob as MainSkyShootService);
				return;
			}
			PushEvent(new ObjectDeleted(mob.Id, _timerCounter));
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
				if (player.Is(AGameObject.EnumObjectType.Player))
				{
					var playerConverted = player as MainSkyShootService;
					if (playerConverted != null) 
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
				if(!_gameObjects[i].Is(AGameObject.EnumObjectType.Player))
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
#if false
			// DEBUG 
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
					activeObject.Think(_gameObjects, now);

					var newCoord = activeObject.ComputeMovement(_updateDelay, GameLevel);
					var canMove = true;
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
						//!! rewrite sqrt!!
						// объект далеко. не рассматриваем
						var rR = (activeObject.Radius + slaveObject.Radius);
						if (Vector2.DistanceSquared(newCoord, slaveObject.Coordinates) > rR*rR &&
							Vector2.DistanceSquared(activeObject.Coordinates, slaveObject.Coordinates) > rR * rR)
						{
							continue;
						}
						activeObject.Do(slaveObject, now);
						//!! @todo rewrite
						if (!slaveObject.IsBullet && !activeObject.IsBullet && 
						    !slaveObject.Is(AGameObject.EnumObjectType.Bonus) &&
							!activeObject.Is(AGameObject.EnumObjectType.Bonus))
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

		public int PlayersCount()
		{
			return _gameObjects.FindAll(o => o.Is(AGameObject.EnumObjectType.Player)).Count;
		}

	#endregion
		#region todo delete

		//private AGameObject HitTestProjectile(AProjectile projectile, Vector2 newCord)
		//{
		//  var prX = newCord.X - projectile.Coordinates.X;
		//  var prY = newCord.Y - projectile.Coordinates.Y;

		//  AGameObject hitedTarget = null;
		//  var minDist = double.MaxValue;

		//  for (int i = 0; i < _gameObjects.Count; i++)
		//  {
		//    var mob = _gameObjects[i];
		//    var mX = mob.Coordinates.X - projectile.Coordinates.X;
		//    var mY = mob.Coordinates.Y - projectile.Coordinates.Y;
		//    var mR = mob.Radius;
		//    var mDist = Math.Sqrt(mX * mX + mY * mY);

		//    if (mDist <= mR && mDist < minDist)
		//    {
		//      //!! hitedTarget = mob;
		//      minDist = mDist;
		//      continue;
		//    }

		//    if (prX == 0 && prY == 0)
		//    {
		//      continue;
		//    }

		//    var h = Math.Abs(prX * mY - prY * mX) / Math.Sqrt(prX * prX * + prY * prY);

		//    var cos1 = mX * prX + mY * prY;
		//    var cos2 = -1 * (prX * (mX - prX) + prY * (mY - prY));

		//    if (h <= mR && Math.Sign(cos1) == Math.Sign(cos2) && mDist < minDist)
		//    {
		//      hitedTarget = mob;
		//      minDist = mDist;
		//    }

		//  }

		//  return hitedTarget;
		//}

		//!! todo delete

		//private void HitTestTouch(MainSkyShootService player)
		//{
		//  for (int i = 0; i < _gameObjects.Count;i++ )
		//  {
		//    var mob = _gameObjects[i];
		//    if ((mob.Coordinates - player.Coordinates).Length() <= mob.Radius + player.Radius)
		//    {
		//      if (mob.Weapon.Reload(DateTime.Now.Ticks / 10000))
		//      {
		//        AGameBonus shield = player.GetBonus(AGameObject.EnumObjectType.Shield);
		//        float damage = shield == null ? 1f : shield.DamageFactor;
		//        player.HealthAmount -= damage * mob.Damage;
		//        SomebodyHitted(player, null);
		//      }

		//      if (player.HealthAmount <= 0)
		//      {
		//        PlayerDead(player);
		//      }
		//    }
		//  }
		//} 




		//!! todo delete

		//private Vector2 ComputeMovement(AGameObject mob)
		//{
			
		//  var newCoord = mob.RunVector*mob.Speed*_updateDelay + mob.Coordinates ;

		//  if (mob.IsPlayer)
		//  {
		//    newCoord.X = MathHelper.Clamp(newCoord.X, 0, GameLevel.levelHeight);
		//    newCoord.Y = MathHelper.Clamp(newCoord.Y, 0, GameLevel.levelWidth);
		//  }
			
		//  return newCoord;
		//}
	#endregion
	}
}