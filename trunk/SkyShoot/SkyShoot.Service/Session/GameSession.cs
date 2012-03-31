﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Weapon.Projectiles;
using System.Timers;
using SkyShoot.Contracts;
using System.Diagnostics;
using SkyShoot.ServProgram.Session;
using SkyShoot.Service.Bonus;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Bonuses;

namespace SkyShoot.Service.Session
{
	
	public class GameSession
	{
		public List<MainSkyShootService> Players { get; set; }

		private List<Mob> _mobs { get; set; }
		private List<AGameBonus> _bonuses { get; set; }
		private ObjectPool<AProjectile> _projectiles { get; set; }

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

		public GameSession(TileSet tileSet, int maxPlayersAllowed, GameMode gameType, int gameID)
		{
			IsStarted = false;
			GameLevel = new GameLevel(tileSet);

			var playerNames = new List<string>();

			_mobs = new List<Mob>();
			_bonuses = new List<AGameBonus>();
			Players = new List<MainSkyShootService>();
			_projectiles = new ObjectPool<AProjectile>();
			//_gameEventStack = new Stack<AGameEvent>();

			LocalGameDescription = new GameDescription(playerNames, maxPlayersAllowed, gameType, gameID, tileSet);
			_spiderFactory = new SpiderFactory(GameLevel);
			_bonusFactory = new BonusFactory();
		}

		//public event SomebodyMovesHandler SomebodyMoves; 
		//public event SomebodyDiesHandler SomebodyDies;
		//public event SomebodyHitHandler SomebodyHit;
		
		private void SomebodyMoved(AGameObject sender, Vector2 direction)
		{
			sender.RunVector = direction;
			pushEvent(new ObjectDirectionChanged(direction,sender.Id,_timerCounter));
		}

		private void SomebodyShot(AGameObject sender, Vector2 direction)
		{
			sender.ShootVector = direction;
			
			if ((sender as MainSkyShootService).Weapon != null)
			{
				if ((sender as MainSkyShootService).Weapon.Reload(System.DateTime.Now.Ticks / 10000))
				{
					var a = (sender as MainSkyShootService).Weapon.CreateBullets(sender, direction);
					AGameBonus doubleDamage = (sender as MainSkyShootService).getBonus(AGameObject.EnumObjectType.DoubleDamage);
					float damage = doubleDamage == null ? 1f : doubleDamage.damageFactor;
					(sender as MainSkyShootService).Weapon.ApplyModifier(a, damage);
					foreach (var b in a)
					{
						_projectiles.getInActive().Copy(b);
						pushEvent(new NewObjectEvent(b,_timerCounter));
					}
					//Trace.WriteLine("projectile added", "GameSession");
				}
			}
		}

		public void SomebodyDied(AGameObject sender)
		{
			pushEvent(new ObjectDeleted(sender.Id, _timerCounter));
		}

		public void SomebodySpawned(AGameObject sender) 
		{			
			pushEvent(new NewObjectEvent(sender, _timerCounter));
		}

		private void NewBonusDropped(AGameObject bonus)
		{
			_bonuses.Add((AGameBonus) bonus);
			pushEvent(new NewObjectEvent(bonus, _timerCounter));
		}

		public void MobDead(Mob mob)
		{
			//SomebodyDied(mob);
			//mob.MeMoved -= SomebodyMoved;
			NewBonusDropped(this._bonusFactory.CreateBonus(mob.Coordinates));
			pushEvent(new ObjectDeleted(mob.Id, _timerCounter));
			_mobs.Remove(mob);
		}

		public void SomebodyHitted(AGameObject mob, AProjectile projectile)
		{
			pushEvent(new ObjectHealthChanged(mob.HealthAmount, mob.Id, _timerCounter));
		}

		public void PlayerLeave(MainSkyShootService player)
		{
			LocalGameDescription.Players.Remove(player.Name);

			player.MeMoved -= SomebodyMoved;
			player.MeShot -= SomebodyShot;

			Players.Remove(player);
			Trace.WriteLine(player.Name + "leaved game");
		}

		private void pushEvent(AGameEvent gameEvent)
		{
			foreach (var player in Players)
			{
				player.NewEvents.Enqueue(gameEvent);
			}
		}

		public void PlayerDead(MainSkyShootService player)
		{

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
			var allObjects = new List<AGameObject>(_mobs);
			allObjects.AddRange(Players);
			// Trace.WriteLine("SynchroFrame");
			return allObjects.ToArray();

		}

		public void Start()
		{
			for(int i = 0; i < Players.Count; i++)
			{
				var player = Players[i];
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
			//	player.Weapon = new Weapon.Shotgun(Guid.NewGuid());
				player.RunVector = new Vector2(0, 0);
				player.MaxHealthAmount = player.HealthAmount = 100f;
				
			}
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
			if (Players.Count >= LocalGameDescription.MaximumPlayersAllowed || IsStarted)
				return false;

			Players.Add(player);
			LocalGameDescription.Players.Add(player.Name);
			var names = new String[Players.Count];
			//UpdatePlayersList(player);

			//if (NewPlayerConnected != null) NewPlayerConnected(player);

			//StartGame += player.GameStart;

			if (Players.Count == LocalGameDescription.MaximumPlayersAllowed)
			{
				// Trace.WriteLine("player added"+player.Name);
				var startThread = new System.Threading.Thread(new System.Threading.ThreadStart(Start));
				startThread.Start();
			}
			return true;
		}

		private void TimerElapsedListener(object sender, EventArgs e)
		{			
			update();
					
		}

	#region local functions
		public void SpawnMob()
		{
			if (_intervalToSpawn == 0)
			{
				_intervalToSpawn = (long) Math.Exp(4.8 - (float)_timerCounter/40000f);
				
				var mob = _spiderFactory.CreateMob();
				// System.Diagnostics.Trace.WriteLine("mob spawned" + mob.Id);
				
				_mobs.Add(mob);
				SomebodySpawned(mob);
				//mob.MeMoved += new SomebodyMovesHandler(SomebodyMoved);
			}
			else
			{
				_intervalToSpawn--;
			}
		}

		// здесь будут производится обработка всех действий
		private void update() 
		{
			if (!System.Threading.Monitor.TryEnter(_updating)) return;

			// Trace.WriteLine("update begin "+ _timerCounter);
			SpawnMob();
			 var now = DateTime.Now.Ticks/10000;
			_updateDelay = now - _lastUpdate;
			_lastUpdate = now;

			var gameObjects = new List<AGameObject>();
			gameObjects.AddRange(Players);

			for (int i = 0; i < _mobs.Count; i++)
			{
				var mob = _mobs[i];
				mob.Think(gameObjects);
				mob.Coordinates = ComputeMovement(mob);
				//System.Diagnostics.Trace.WriteLine("Mob cord: " + mob.Coordinates); 

			}

			for(int i = 0; i < Players.Count; i++)
			{
				var player = Players[i];
				player.Coordinates = ComputeMovement(player);
				player.DeleteExpiredBonuses(_lastUpdate);

				//Проверка на касание игрока и моба
				hitTestTouch(player);

				List<AGameBonus> bonuses2delete = new List<AGameBonus>();
				for(int j = 0; j < _bonuses.Count; j ++)
				{
					var bonus = _bonuses[j];
					if (Vector2.Distance(bonus.Coordinates, player.Coordinates) < player.Radius)
					{
						if(!bonus.IsActive)
						{
							continue;
						}
						player.AddBonus(bonus);
						bonus.taken(_lastUpdate);
						bonus.IsActive = false;
						pushEvent(new ObjectDeleted(bonus.Id, _timerCounter));
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
				
				if (pr == null || pr.Item == null) continue;
				var projectile = pr.Item;
				if (projectile.LifeDistance <= 0)
				{
					pr.isActive = false;
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
					hitedMob.DamageTaken(projectile);
					SomebodyHitted(hitedMob, projectile);
					if (hitedMob.HealthAmount <= 0)
					{
						MobDead(hitedMob);
					}
					projectile.LifeDistance = -1;
				}

			}

			//_projectiles.RemoveAll(x => (x==null) || (x.LifeDistance <= 0));
			// Trace.WriteLine(System.DateTime.Now.Ticks/10000 - now);
			// Trace.WriteLine("update end " + _timerCounter);
			_timerCounter++;
			//_updated = false;
			System.Threading.Monitor.Exit(_updating);
		}
		
		private Mob hitTestProjectile(AProjectile projectile, Vector2 newCord)
		{
			var prX = newCord.X - projectile.Coordinates.X;
			var prY = newCord.Y - projectile.Coordinates.Y;

			Mob hitedTarget = null;
			var minDist = double.MaxValue;

			for (int i = 0; i < _mobs.Count; i++)
			{
				var mob = _mobs[i];
				var mX = mob.Coordinates.X - projectile.Coordinates.X;
				var mY = mob.Coordinates.Y - projectile.Coordinates.Y;
				var mR = mob.Radius;
				var mDist = Math.Sqrt(mX * mX + mY * mY);

				if (mDist <= mR && mDist < minDist)
				{
					hitedTarget = mob;
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

		private void hitTestTouch(MainSkyShootService player)
		{
			for (int i = 0; i < _mobs.Count;i++ )
			{
				var mob = _mobs[i];
				if ((mob.Coordinates - player.Coordinates).Length() <= mob.Radius + player.Radius)
				{
					if (mob.Weapon.Reload(System.DateTime.Now.Ticks / 10000))
					{
						AGameBonus shield = player.getBonus(AGameBonus.EnumObjectType.Shield);
						float damage = shield == null ? 1f : shield.damageFactor;
						player.HealthAmount -= damage * mob.Damage;
						SomebodyHitted(player, null);
						mob.Stop();
					}

					if (player.HealthAmount <= 0)
					{
						PlayerDead(player);
					}
				}
			}
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
	#endregion
	}
}