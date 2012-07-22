using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyShoot.Contracts.CollisionDetection;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.SynchroFrames;
using SkyShoot.Contracts.Utils;
using SkyShoot.Game.Network;
using SkyShoot.Game.View;

namespace SkyShoot.Game.Game
{
	public class GameModel
	{
		public Camera2D Camera2D { get; private set; }

		// explosions -> exploded time
		// private readonly Dictionary<DrawableGameObject, long> _explosions;

		private int _updateCouter;

		private GameSnapshot _gameSnapshot;

		private readonly TimeHelper _timeHelper;

		/// <summary>
		/// Все GameEvent'ы с момента последнего синхрокадра,
		/// нужно хранить их! 
		/// </summary>
		private readonly List<AGameEvent> _serverGameEvents = new List<AGameEvent>();

		private readonly GameLevel _gameLevel;

		private DrawableGameObject[] _drawableGameObjects;

		public GameModel(GameLevel gameLevel, TimeHelper timeHelper)
		{
			_gameLevel = gameLevel;
			_timeHelper = timeHelper;

			Camera2D = new Camera2D(gameLevel.Width, gameLevel.Height);

			// _explosions = new Dictionary<DrawableGameObject, long>();
		}

		// todo придумать что-нибудь
		//		public void UpdateExplosions()
		//		{
		//			var keys = new DrawableGameObject[_explosions.Count];
		//			_explosions.Keys.CopyTo(keys, 0);
		//
		//			foreach (DrawableGameObject explosion in keys)
		//			{
		//				if (DateTime.Now.Ticks / 10000 - _explosions[explosion] > Constants.EXPLOSION_LIFE_DISTANCE)
		//				{
		//					_explosions.Remove(explosion);
		//					RemoveGameObject(explosion.Id);
		//				}
		//			}
		//		}

		private void ApplyEvents(IEnumerable<AGameEvent> gameEvents)
		{
			_gameSnapshot.ApplyEvents(gameEvents.Where(x => x.TimeStamp >= _gameSnapshot.Time));
		}

		/// <summary>
		/// Обновление позиций игровых объектов
		/// </summary>
		private void ApplySynchroFrame(SynchroFrame synchroFrame)
		{
			_gameSnapshot = new GameSnapshot(synchroFrame, _gameLevel);

			_gameSnapshot.ApplyEvents(_serverGameEvents.Where(x => x.TimeStamp >= _gameSnapshot.Time));
		}

		public void Update(GameTime gameTime)
		{
			// todo
			// update explosions
			// UpdateExplosions();

			#region применение синхрокадра

			if (_updateCouter % 30 == 0)
			{
				SynchroFrame serverSynchroFrame = ConnectionManager.Instance.SynchroFrame();

				if (serverSynchroFrame == null)
				{
					GameController.Instance.GameOver();
					return;
				}
				Trace.WriteLine(serverSynchroFrame);
				ApplySynchroFrame(serverSynchroFrame);

				// очистка списка GameEvent'ов c последнего синхрокадра
				_serverGameEvents.Clear();
			}

			#endregion

			#region применение игровых событий

			if (_updateCouter % 5 == 0)
			{
				AGameEvent[] serverGameEvents = ConnectionManager.Instance.GetEvents();
				_serverGameEvents.AddRange(serverGameEvents);

				Logger.PrintEvents(serverGameEvents);
				ApplyEvents(serverGameEvents);
			}

			#endregion

			#region экстраполяция

			long serverTime = _timeHelper.GetTime() + ConnectionManager.DifferenceTime;
			_drawableGameObjects = _gameSnapshot.ExtrapolateTo(serverTime);

			#endregion

			#region обнаружение столкновений

			foreach (DrawableGameObject gameObject in _drawableGameObjects)
			{
				foreach (DrawableGameObject slaver in _drawableGameObjects)
				{
					if (gameObject != slaver && slaver.Is(AGameObject.EnumObjectType.Block) &&
						gameObject.Is(AGameObject.EnumObjectType.Block))
						if (gameObject.Radius * gameObject.Radius + slaver.Radius * slaver.Radius <=
							(gameObject.Coordinates - slaver.Coordinates).LengthSquared())
							gameObject.Coordinates += CollisionDetector.FitObjects(gameObject.Coordinates,
																				   gameObject.RunVector,
																				   gameObject.Bounding, slaver.Coordinates,
																				   slaver.RunVector, slaver.Bounding);
				}
			}

			#endregion

			#region обновление текстур

			foreach (DrawableGameObject gameObject in _drawableGameObjects)
			{
				gameObject.Update(gameTime);
			}

			#endregion

			_updateCouter++;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			var me = GetGameObject(GameController.MyId);

			Vector2 myPosition = me.CoordinatesM;

			Camera2D.Position = myPosition;

			spriteBatch.Begin(SpriteSortMode.Immediate,
							  BlendState.AlphaBlend,
							  null,
							  null,
							  null,
							  null,
							  Camera2D.GetTransformation(Textures.GraphicsDevice));

			// draw background
			_gameLevel.Draw(spriteBatch);

			// draw game objects
			foreach (DrawableGameObject drawableGameObject in _drawableGameObjects)
			{
				drawableGameObject.Draw(spriteBatch);
			}

			spriteBatch.End();
		}

		public DrawableGameObject GetGameObject(Guid id)
		{
			return _drawableGameObjects.First(x => x.Id == GameController.MyId);
		}
	}
}