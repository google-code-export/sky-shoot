using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;
using SkyShoot.XNA.Framework;
using Color = System.Drawing.Color;

namespace SkyShoot.WinFormsClient
{
	public partial class MainForm : Form //, ISkyShootCallback
	{
		#region переменные

		private GameManagerForm _games;
		private readonly ISkyShootService _service;

		private AGameObject _me;
		private List<AGameObject> _objects;

		private Vector2 _prevMove;

		private GameLevel _level;

		private DateTime _prev;
		private Thread _th;
		public bool GameRuning;

		#endregion

		//#region ISkyShootServiceCallback

		public void GameStart(GameLevel level)
		{
			if (level == null)
				return;
			GameRuning = true;
			_prev = DateTime.Now;
			_objects.Clear();
			_level = level;
			_prevMove = Vector2.Zero;

			//_me = _objects.Find(m => m.Id == _me.Id);

		}

		//public void MobShot(AGameObject mob, AProjectile[] projectiles)
		//{
		//  try
		//  {
		//    Trace.WriteLine("MobShot: " + projectiles.Length);
		//    //lock (_bullets)
		//    {
		//      //_bullets.AddRange(projectiles);
		//    }
		//  }
		//  catch (Exception e)
		//  {
		//    Trace.WriteLine("MobShot: " + e);
		//  }
		//}

		//public void SpawnMob(AGameObject mob)
		//{
		//  try
		//  {
		//    Trace.WriteLine("SpawnMob: " + mob.Coordinates + ";" + mob.IsPlayer);
		//    lock (_objects)
		//    {
		//      _objects.Add(mob);
		//    }
		//  }
		//  catch (Exception e)
		//  {
		//    Trace.WriteLine("MobShot: " + e);
		//  }

		//}

		//public void Hit(AGameObject mob, AProjectile projectile)
		//{
		//  try
		//  {
		//    Trace.WriteLine("Hit: " + projectile);
		//    //var b = _bullets.Find(x => x.Id == projectile.Id);
		//    //lock (_bullets)
		//    //{
		//    //  _bullets.Remove(b);
		//    //}
		//  }
		//  catch (Exception e)
		//  {
		//    Trace.WriteLine("Hit: " + e);

		//  }
		//}

		//public void MobMoved(AGameObject mob, Vector2 direction)
		//{
		//  var m = _objects.Find(curm => curm.Id == mob.Id);
		//  if (m == null)
		//    return;
		//  lock (_objects)
		//  {
		//    m.Copy(mob);
		//    m.RunVector = direction;
		//  }
		//  Redraw();
		//}

		//public void MobDead(AGameObject mob)
		//{
		//  AGameObject m = _objects.Find(curm => curm.Id == mob.Id);
		//  if (m == null)
		//    return;
		//  lock (_objects)
		//  {
		//    _objects.Remove(m);
		//  }
		//}

		//public void BonusDropped(AObtainableDamageModifier bonus)
		//{
		//  throw new NotImplementedException();
		//}

		//public void BonusExpired(AObtainableDamageModifier bonus)
		//{
		//  throw new NotImplementedException();
		//}

		//public void BonusDisappeared(AObtainableDamageModifier bonus)
		//{
		//  throw new NotImplementedException();
		//}

		//private void Stop()
		//{
		//  if (InvokeRequired)
		//  {
		//    Invoke(new MethodInvoker(Stop));
		//    return;
		//  } 
		//  GameRuning = false;
		//  if (_th != null)
		//  {
		//    _th.Abort();
		//  }
		//  Hide();
		//  _games.ShowDialog();
		//}

		//public void GameOver()
		//{
		//  (new Thread(Stop)).Start();
		//}

		//public void PlayerLeft(AGameObject mob)
		//{
		//  //throw new NotImplementedException();
		//}

		//public void SynchroFrame(AGameObject[] mobs)
		//{
		//  //return;
		//  foreach (var m in mobs)
		//  {
		//    var t = _objects.Find(cm => cm.Id == m.Id);
		//    if (t == null)
		//      continue;
		//    t.Copy(m);
		//    if (_me.Id == t.Id)
		//    {
		//    }
		//  }
		//}

		//public void PlayerListChanged(string[] names)
		//{
		//  if(!GameRuning && names != null && names.Length != 0)
		//  {
		//    _games.UpdatePlayerList(names);
		//  }
		//}

		//#endregion

		#region манипуляции с интерфейсом

		private void Redraw()
		{
			if (InvokeRequired)
			{
				Invoke(new MethodInvoker(Redraw));
				return;
			}
			//this.DoubleBuffered = true;
			_pnCanvas.Invalidate();
			//this._pnCanvas.stySetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true)
		}

		void AskGame()
		{
			if (InvokeRequired)
			{
				Invoke(new MethodInvoker(AskGame));
				return;
			}
			Hide();
			_games = new GameManagerForm(_service);
			_games.ShowDialog(this);
			if (IsDisposed)
			{
				return;
			}
			Show();
		}

		private void UpdateSt(Object stateInfo)
		{
			while (true)
			{
				if (!GameRuning)
				{
					AskGame();

					_level = _games.Level;
					GameStart(_level);
				}
				else
				{
					Thread.Sleep(100);
					DateTime now = DateTime.Now;
					_objects.Clear();
					var syncObjects = _service.SynchroFrame();
					if (syncObjects == null)
					{
						GameRuning = false;
						continue;
					}
					_objects.AddRange(syncObjects);
					var tempMe = _objects.Find(m => m.Id == _me.Id);
					if (tempMe != null)
					{
						_me.Copy(tempMe);
					}
					//lock (_objects)
					{
						try
						{
							foreach (var t in _objects)
							{
								var m = t;
								if (float.IsNaN(m.RunVector.X) || float.IsNaN(m.RunVector.Y))
									continue;
								m.Coordinates +=
									m.RunVector *
									(m.Speed * (float)((now - _prev).TotalMilliseconds));
								if (m.IsPlayer)
								{
									m.Coordinates = Vector2.Clamp(m.Coordinates,
																								new Vector2(0, 0),
																								new Vector2(_level.levelWidth, _level.levelHeight)); /**/
								}
							}
						}
						catch (Exception e)
						{
							Trace.WriteLine("Cli: Update: " + e);
						}
					}
					//lock (_bullets)
					//{
					//  try
					//  {
					//    _bullets.RemoveAll(x => (x.LifeDistance <= 0));
					//    foreach (var m in _bullets)
					//    {
					//      var diff = m.RunVector*
					//                 (m.Speed*(float) ((now - _prev).TotalMilliseconds));
					//      m.LifeDistance -= diff.Length();
					//      m.Coordinates += diff;

					//      Vector2 v = Vector2.Clamp(m.Coordinates,
					//                                new Vector2(0, 0),
					//                                new Vector2(_level.levelWidth, _level.levelHeight));
					//      if (v != m.Coordinates)
					//      {
					//        m.LifeDistance = -1;
					//      }
					//    }
					//  }
					//  catch (Exception e)
					//  {
					//    Trace.WriteLine("Cli: Update: " + e);
					//  }
					//}
					_prev = now;
					SetStatus("[pos: " + _me.Coordinates.X + ";" + _me.Coordinates.Y + "] [dir: " +
										_me.RunVector.X + ";" + _me.RunVector.Y + "]");
					Redraw();
				}
			}
		}

		private void SetStatus(String sts)
		{
			try
			{
				toolStripStatusLabel1.Text = sts;
			}
			catch (Exception e)
			{
				Trace.WriteLine("Cli: Status: " + e);
			}
		}

		#endregion

		public MainForm()
		{
			InitializeComponent();
			var channelFactory = new ChannelFactory<ISkyShootService>("SkyShootEndpoint");
			_service = channelFactory.CreateChannel();
			GameRuning = false;
			_objects = new List<AGameObject>();
			_th = new Thread(UpdateSt);

		}

		#region events

		private void MainForm_OnKeyDown(object sender, KeyEventArgs e)
		{
			if (!GameRuning)
				return;
			var v = new Vector2();
			switch (e.KeyValue)
			{
			case 'W':
			case 'w':
				v.Y = -1;
				break;
			case 'S':
			case 's':
				v.Y = 1;
				break;
			case 'D':
			case 'd':
				v.X = 1;
				break;
			case 'A':
			case 'a':
				v.X = -1;
				break;
			}
			v.Normalize();
			_me.RunVector = v;
			if (v != _prevMove)
			{
				//_th.Suspend();
				_prevMove = v;
				ApplyEvents(_service.GetEvents());
				ApplyEvents(_service.Move(v));
				//_th.Resume();
			}
		}

		private void ApplyEvents(IEnumerable<AGameEvent> events)
		{
			foreach (var gameEvent in events)
			{
				var gameObject = _objects.Find(o => o.Id == gameEvent.Id);
				if (gameObject != null)
				{
					gameEvent.UpdateMob(gameObject);
				}
			}
		}


		private void MainForm_OnKeyUp(object sender, KeyEventArgs e)
		{
			var v = new Vector2(0, 0);
			_me.RunVector = v;
			_service.Move(v);
		}

		private void MainForm_OnLoad(object sender, EventArgs e)
		{
			var login = new LoginForm();
			if (login.ShowDialog() == DialogResult.OK)
			{
				if (!Login(login.UserName, login.Password))
				{
					MessageBox.Show(@"Can't login.", @"Error",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				_th.Start();
			}
			else
			{
				Application.Exit();
			}
		}

		private void MainForm_OnPaint(object sender, PaintEventArgs e)
		{
			float r, x, y;
			using (var g = e.Graphics)
			{
				try
				//lock (_objects)
				{
					foreach (var m in _objects)
					{
						r = 5f;
						x = (m.Coordinates.X) * _pnCanvas.Width / _level.levelWidth;
						y = (m.Coordinates.Y) * _pnCanvas.Height / _level.levelHeight;
						//r = m.Radius*0.5f*_pnCanvas.Width/_level.levelWidth;
						g.FillEllipse(m.IsPlayer ? Brushes.Black : Brushes.Green,
													new RectangleF(
														new PointF(x - r, y - r),
														new SizeF(2 * r, 2 * r)));
						//if (m.IsPlayer)
						{
							g.DrawLine(new Pen(m.IsPlayer ? Color.Red : Color.Green, 3),
												 new PointF(x, y),
												 new PointF(x + m.ShootVector.X * 2 * r, y + m.ShootVector.Y * 2 * r));
						}
						Color c;
						if (m.HealthAmount > 0.60f * m.MaxHealthAmount)
						{
							c = Color.Lime;
						}
						else if (m.HealthAmount > 0.30f * m.MaxHealthAmount)
						{
							c = Color.Yellow;
						}
						else
						{
							c = Color.Red;
						}
						g.DrawLine(new Pen(c, 2f),
											 x - r, y - r, x + 2 * r * m.HealthAmount / m.MaxHealthAmount, y - r);
					}
				}
				catch (Exception exc)
				{
					Trace.WriteLine("Exc: " + exc);
				}
				//r = 2f;
				//try
				//  //lock (_bullets)
				//{
				//  foreach (var m in _bullets)
				//  {
				//    x = (m.Coordinates.X)*_pnCanvas.Width/_level.levelWidth;
				//    y = (m.Coordinates.Y)*_pnCanvas.Height/_level.levelHeight;
				//    g.FillEllipse(Brushes.Red,
				//                  new RectangleF(
				//                    new PointF(x - r, y - r),
				//                    new SizeF(2*r, 2*r)));
				//  }
				//}
				//catch (Exception exc)
				//{
				//  Trace.WriteLine("Exc: " + exc);
				//}
			}
		}

		private void MainForm_OnFormClosing(object sender, FormClosingEventArgs e)
		{
			_service.LeaveGame();
			if (_th != null)
			{
				_th.Abort();
			}
		}

		private void pnCanvas_OnMouseMove(object sender, MouseEventArgs e)
		{
			if (!GameRuning)
				return;
			var v = (new Vector2(
								((float)e.X) / ((float)_pnCanvas.Width) * _level.levelWidth,
								((float)e.Y) / ((float)_pnCanvas.Height) * _level.levelHeight))
							- _me.Coordinates;
			v.Normalize();
			_me.ShootVector = v;
		}

		private void pnCanvas_OnClick(object sender, MouseEventArgs e)
		{
			if (!GameRuning)
				return;
			var v = (new Vector2(
								((float)e.X) / ((float)_pnCanvas.Width) * _level.levelWidth,
								((float)e.Y) / ((float)_pnCanvas.Height) * _level.levelHeight))
							- _me.Coordinates;
			v.Normalize();
			_me.ShootVector = v;
			_service.Shoot(v);
		}


		#endregion

		#region просто методы

		private bool Login(string username, string password)
		{
			try
			{
				Guid? id = _service.Login(username, password);
				if (id != null)
				{
					_me = new AGameObject { Coordinates = Vector2.Zero };

					_me.Id = (Guid)id;
					SetStatus("Logon successfull");
					return true;
				}
				else
				{
					SetStatus("No such login");
					return false;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		#endregion

	}
}
