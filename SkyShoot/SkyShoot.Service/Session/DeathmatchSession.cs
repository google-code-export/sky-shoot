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
	class DeathmatchSession : GameSession
	{
		public DeathmatchSession(TileSet tileSet, int maxPlayersAllowed,
			GameMode gameType, int gameID, int teams)
			: base(tileSet, maxPlayersAllowed, gameType, gameID, teams)
		{
		}


		public override void Start()
		{
			#region инициализация объектов

			var randomNumberGenerator = new Random();

			int nextTeamRefill = 1;

			for (int i = 0; i < base._gameObjects.Count; i++)
			{
				if (!base._gameObjects[i].Is(AGameObject.EnumObjectType.Player))
					continue;

				var player = base._gameObjects[i] as MainSkyShootService;
				if (player == null)
				{
					Trace.WriteLine("Error: !!! IsPlayer true for non player object");
					continue;
				}

				base._gameObjects[i].TeamIdentity = base._sessionTeamsList.GetTeamByNymber(nextTeamRefill);//Закидываем игроков поочерёдно в разные команды
				base._gameObjects[i].TeamIdentity.Members.Add(_gameObjects[i]);
				
				player.MeMoved += SomebodyMoved;
				player.MeShot += SomebodyShot;
				player.MeChangeWeapon += SomebodyChangedWeapon;

				player.Coordinates = new Vector2((float)(500 + (400 * Math.Sin(2 * nextTeamRefill * Math.PI / PlayersCount()))),
					(float)(500 + 400 * Math.Cos(2 * nextTeamRefill * Math.PI / PlayersCount())));//Раскидываем игроков подальше друг от друга. Командами.
				player.Speed = Constants.PLAYER_DEFAULT_SPEED;
				player.Radius = Constants.PLAYER_RADIUS;
				player.Weapon = new Weapon.Pistol(Guid.NewGuid(), player);
				player.RunVector = new Vector2(0, 0);
				player.MaxHealthAmount = player.HealthAmount = Constants.PLAYER_HEALTH * 10;
				nextTeamRefill++;
			}

			base._gameObjects.AddRange(_wallFactory.CreateWalls());

			#endregion

			base._timeHelper = new TimeHelper(TimeHelper.NowMilliseconds);

			base._timerCounter = 0;
			base._updating = false;

			base._lastUpdate = 0;
			base._updateDelay = 0;

			base._gameTimer = new Timer(Constants.FPS) { AutoReset = true };
			base._gameTimer.Elapsed += TimerElapsedListener;
			base._gameTimer.Start();

			// todo номер игры
			//Trace.Listeners.Add(new Logger(Logger.SolutionPath + "\\logs\\server_game_" + LocalGameDescription.GameId + ".txt", _timeHelper) {Name = "game logger"});

			Trace.WriteLine("Game Started");

			Trace.Listeners.Remove(Logger.ServerLogger);

			base.IsStarted = true;
		}

		public override void Stop()
		{
			if (base._gameTimer != null)
			{
				base._gameTimer.Enabled = false;
				base._gameTimer.AutoReset = false;
				base._gameTimer.Elapsed -= TimerElapsedListener;
			}
			Trace.Listeners.Add(Logger.ServerLogger);

			Trace.WriteLine("Game Over");

			Trace.Listeners.Remove("game");
			base.IsStarted = false;
		}

		public override void Update()
		{
			if (!System.Threading.Monitor.TryEnter(_updating)) return;

			// Trace.WriteLine("update begin "+ _timerCounter);
			var now = base._timeHelper.GetTime();

			base._updateDelay = now - base._lastUpdate;
			base._lastUpdate = now;

			var eventsCash = new List<AGameEvent>(_gameObjects.Count * 3);

			eventsCash.AddRange(SpawnMob(now));
			lock (base._gameObjects)
			{
				for (int i = 0; i < base._gameObjects.Count; i++)
				{
					var activeObject = base._gameObjects[i];
					// объект не существует
					if (!activeObject.IsActive || activeObject.ObjectType == AGameObject.EnumObjectType.Wall)
					{
						continue;
					}

					lock (base._newObjects)
					{
						eventsCash.AddRange(activeObject.Think(base._gameObjects, base._newObjects, now));
					}

					var newCoord = activeObject.ComputeMovement(base._updateDelay, base.GameLevel);
					//var canMove = true;
					/* <b>int j = 0</b> потому что каждый с каждым, а действия не симметричны*/
					for (int j = 0; j < base._gameObjects.Count; j++)
					{
						// тот же самый объект. сам с собой он ничего не делает
						if (i == j)
						{
							continue;
						}
						var slaveObject = base._gameObjects[j];
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
						lock (base._newObjects)
						{
							eventsCash.AddRange(activeObject.Do(slaveObject, base._newObjects, now));
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

				for (int i = 0; i < base._gameObjects.Count; i++)
				{
					if (!base._gameObjects[i].IsActive)
					{
						eventsCash.AddRange(MobDead(base._gameObjects[i], now));
						eventsCash.AddRange(base._gameObjects[i].OnDead(base._gameObjects[i], base._gameObjects, now));
					}
				}

				// flush of events cash
				foreach (var ev in eventsCash)
				{
					PushEvent(ev);
				}

				base._gameObjects.RemoveAll(m => !m.IsActive);
				lock (_newObjects)
				{
					base._gameObjects.AddRange(base._newObjects);
					base._newObjects.Clear();
				}
			}

			// Trace.WriteLine(System.DateTime.Now.Ticks/10000 - now);
			// Trace.WriteLine("update end " + _timerCounter);
			base._timerCounter++;
			//_updated = false;
			System.Threading.Monitor.Exit(base._updating);
		}
		protected override void PlayerDead(MainSkyShootService player)
		{
			base.PlayerDead(player);
			if (player.TeamIdentity.Members.Count == 0)
			{
				_sessionTeamsList.Teams.Remove(player.TeamIdentity);
			}
			if (_sessionTeamsList.Teams.Count <= 1)
			{
				if (_sessionTeamsList.Teams.Count == 1) {
					List<AGameObject> remainingTeams = _sessionTeamsList.Teams.First().Members;
					Debug.Assert(remainingTeams.Count == 1);
					MainSkyShootService lastPlayer = remainingTeams.First() as MainSkyShootService;
					System.Console.WriteLine(lastPlayer.Name + " wins");
					lastPlayer.Disconnect();
					player.TeamIdentity.Members.Remove(lastPlayer);
				}
				// TODO: final screen?
			}
		}
	}
}
