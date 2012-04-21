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
using SkyShoot.Contracts.Weapon;
using SkyShoot.XNA.Framework;
using Color = System.Drawing.Color;

namespace SkyShoot.WinFormsClient
{
	public partial class MainForm : Form
	{
		#region переменные

		private GameManagerForm _games;
		private readonly ISkyShootService _service;

		private AGameObject _me;
		private readonly List<AGameObject> _objects;

		private Vector2 _prevMove;

		private GameLevel _level;

		private DateTime _prev;
		private readonly Thread _th;
		public bool GameRuning;
		private Vector2 _shoot;

		#endregion

		public void GameStart(GameLevel level)
		{
			if (level == null)
				return;
			GameRuning = true;
			_prev = DateTime.Now;
			_objects.Clear();
			_level = level;
			_prevMove = Vector2.Zero;
		}

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
					AGameObject[] syncObjects; 
					lock (_service)
					{
						if(_prevMove != _me.RunVector)
						{
							ApplyEvents(_service.Move(_prevMove = _me.RunVector));
						}
						if (_shoot != Vector2.Zero)
						{
							ApplyEvents(_service.Shoot(_shoot));
							_shoot = Vector2.Zero;
						}
						if (_curWeapon != _prevWeapon)
						{
							_service.ChangeWeapon(_prevWeapon = _curWeapon);
						}
						syncObjects = _service.SynchroFrame();
						ApplyEvents(_service.GetEvents());
					}
					if (syncObjects == null)
					{
						GameRuning = false;
						continue;
					}
					lock (_objects)
					{
						_objects.Clear();
						_objects.AddRange(syncObjects);
					}
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
								if (m.Is(AGameObject.EnumObjectType.Player))
								{
									m.Coordinates = Vector2.Clamp(m.Coordinates, new Vector2(0, 0), new Vector2(_level.levelWidth, _level.levelHeight)); /**/
								}
							}
						}
						catch (Exception e)
						{
							Trace.WriteLine("Cli: Update: " + e);
						}
					}
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

		private readonly WeaponButton[] _buttons;
		private AWeapon.AWeaponType _curWeapon, _prevWeapon;

		public MainForm()
		{
			InitializeComponent();

			int bts = Enum.GetNames(typeof(AWeapon.AWeaponType)).Length;
			_buttons = new WeaponButton[bts];
			int i = 0;
			foreach (var s in Enum.GetNames(typeof(AWeapon.AWeaponType)))
			{
				var b = new WeaponButton();
				AWeapon.AWeaponType t;
				if(!Enum.TryParse(s, out t))
				{
					MessageBox.Show(@"Can't load weapons list :(", @"Error");
				}
				b.WeaponType = t;
				b.Name = s;
				b.Text = @"("+ (i+1).ToString() + @") " + s;
				b.Click += BClick;
				b.BorderStyle = BorderStyle.Fixed3D;
				_buttons[i++] = b;
			}
			Controls.AddRange(_buttons);
			MakeWeaponButtonsLayout();

			var channelFactory = new ChannelFactory<ISkyShootService>("SkyShootEndpoint");
			_service = channelFactory.CreateChannel();
			GameRuning = false;
			_objects = new List<AGameObject>();
			_th = new Thread(UpdateSt);

			_curWeapon = _prevWeapon = AWeapon.AWeaponType.Pistol;
		}

		private void MakeWeaponButtonsLayout()
		{
			var bts = _buttons.Length;
			var w = this.ClientSize.Width / bts;
			var h = statusStrip1.Top - _pnCanvas.Bottom - 2;
			var x = 1;
			var y = _pnCanvas.Bottom + 1;
			for (var i = 0; i < bts; i++)
			{
				_buttons[i].Top = y;
				_buttons[i].Left = x - 1;
				_buttons[i].Width = w;
				_buttons[i].Height = h;
				x += w;
			}
		}


		void BClick(object sender, EventArgs e)
		{
			var b = sender as WeaponButton;
			if(b == null)
				return;
			_curWeapon = b.WeaponType;
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
			//case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				int ind = e.KeyValue - '0' - 1;
				if(ind < _buttons.Length)
				{
					BClick(_buttons[ind], null);
				}
				break;
			}
			v.Normalize();
			_me.RunVector = v;
			//if (v != _prevMove)
			{
				//_th.Suspend();
				//_prevMove = v;
				//lock (_service)
				//{
				//  ApplyEvents(_service.Move(v));
				//}
				//_th.Resume();
			}
		}

		private void ApplyEvents(IEnumerable<AGameEvent> events)
		{
			if(events ==null)
				return;
			foreach (var gameEvent in events)
			{
				var t = _objects.FindAll(o => o == null);
				if(t.Count != 0)
				{
					Trace.WriteLine(t);
				}
				var gameObject = _objects.Find(o => o!= null && o.Id == gameEvent.GameObjectId);
				if (gameObject != null)
				{
					gameEvent.UpdateMob(gameObject);
				}
				else
				{
					var newObj = new AGameObject();
					gameEvent.UpdateMob(newObj);
					_objects.Add(newObj);
				}
			}
		}


		private void MainForm_OnKeyUp(object sender, KeyEventArgs e)
		{
			_me.RunVector = Vector2.Zero;
			//lock (_service)
			//{
			//  _service.Move(v);
			//}
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
				{
					AGameObject[] objects2Paint;
					lock (_objects)
					{
						objects2Paint = _objects.ToArray();
					}
					foreach (var m in objects2Paint)
					{
						if (!m.IsActive)
						{
							continue;
						}
						x = (m.Coordinates.X) * _pnCanvas.Width / _level.levelWidth;
						y = (m.Coordinates.Y) * _pnCanvas.Height / _level.levelHeight;
						r = m.Radius * _pnCanvas.Width / _level.levelWidth; 
						switch (m.ObjectType)
						{
						case AGameObject.EnumObjectType.Mob:
						case AGameObject.EnumObjectType.Player:
							g.FillEllipse(m.Is(AGameObject.EnumObjectType.Player) ? Brushes.Black : Brushes.Green,
														new RectangleF(
															new PointF(x - r, y - r),
															new SizeF(2 * r, 2 * r)));
							g.DrawLine(new Pen(m.Is(AGameObject.EnumObjectType.Player) ? Color.Red : Color.Green, 3),
												 new PointF(x, y),
												 new PointF(x + m.ShootVector.X * 2 * r, y + m.ShootVector.Y * 2 * r));

							Color c;
							r--;
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
												 x - r, y - r, x + r * m.HealthAmount / m.MaxHealthAmount, y - r);
							break;
						case AGameObject.EnumObjectType.Bullet:
						case AGameObject.EnumObjectType.LaserBullet:
						case AGameObject.EnumObjectType.ShotgunBullet:
						case AGameObject.EnumObjectType.RocketBullet:
							//r = 2f;
							g.FillEllipse(Brushes.Red,
														new RectangleF(
															new PointF(x - r, y - r),
															new SizeF(2 * r, 2 * r)));
							break;
						case AGameObject.EnumObjectType.Explosion:
							g.FillEllipse(new SolidBrush(Color.FromArgb(75, Color.OrangeRed)),
														new RectangleF(
															new PointF(x - r, y - r),
															new SizeF(2 * r, 2 * r)));
							break;
						case AGameObject.EnumObjectType.Wall:
							//r = m.Radius * _pnCanvas.Width / _level.levelWidth;
							var rh = m.Radius * _pnCanvas.Height / _level.levelHeight;
							g.FillRectangle(Brushes.Black,
								new RectangleF(
									new PointF(x - r, y - rh),
									new SizeF(2 * r, 2 * rh)));
							break;
						case AGameObject.EnumObjectType.DoubleDamage:
							//r = m.Radius * _pnCanvas.Width / _level.levelWidth;
							g.FillEllipse(Brushes.Blue,
														new RectangleF(
															new PointF(x - r, y - r),
															new SizeF(2 * r, 2 * r)));
							g.DrawString("x2", SystemFonts.MenuFont, Brushes.Blue, x - r, y - r);
							break;
						case AGameObject.EnumObjectType.Remedy:
							g.DrawLine(Pens.Red, x - r, y, x + r, y);
							g.DrawLine(Pens.Red, x, y - r, x, y + r);
							break;
						default:
							r = 3f;
							g.FillEllipse(Brushes.Blue,
														new RectangleF(
															new PointF(x - r, y - r),
															new SizeF(2 * r, 2 * r)));
							break;
						}
					}
				}
				catch (Exception exc)
				{
					Trace.WriteLine("Exc: " + exc);
				}
			}
		}

		private void MainForm_OnFormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				lock (_service)
				{
					_service.LeaveGame();
				}
			}
			// возможно, сервер уже выключили к этому моменту
			catch (Exception exc)
			{
				Trace.WriteLine(exc);
			}
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
			_shoot = v;
			//lock (_service)
			//{
			//  _service.Shoot(v);
			//}
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
					_me = new AGameObject {Coordinates = Vector2.Zero, Id = (Guid) id};

					SetStatus("Logon successfull");
					return true;
				}
				SetStatus("No such login");
				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}

		#endregion

		private void MainFormResize(object sender, EventArgs e)
		{
			MakeWeaponButtonsLayout();
		}

	}
}
