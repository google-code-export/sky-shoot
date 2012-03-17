using System;
using System.Threading;
using System.Windows.Forms;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Service;
using System.Diagnostics;

namespace SkyShoot.WinFormsClient
{
	public partial class GameManagerForm : Form
	{
		#region переменные

		private ISkyShootService _service;
		private GameDescription[] _list;
		private GameDescription _activeGame;
		private Thread _updatingThread;
		#endregion

		#region

		protected override void OnLoad(EventArgs e)
		{
			_updatingThread = new Thread(UpdateStatus);
			_updatingThread.Start();
			base.OnLoad(e);
		}

		protected override void OnClosed(EventArgs e)
		{
			_updatingThread.Abort();
			base.OnClosed(e);
		}

		private void UpdateStatus()
		{
			while (Level == null)
			{
				Thread.Sleep(100);
				if (_activeGame != null)
				{
					Level = _service.GameStart(_activeGame.GameId);
					UpdatePlayerList();
				}
			}
			// close this windows
			CloseThis();
		}

		void CloseThis()
		{
			if(InvokeRequired)
			{
				Invoke(new MethodInvoker(CloseThis));
				return;
			}
			DialogResult = DialogResult.OK;
			
			Close();
		}

		private void UpdatePlayerList()
		{
			if (InvokeRequired)
			{
				Invoke(new MethodInvoker(UpdatePlayerList));
				return;
			}
			lstbPlayers.Items.Clear();
			lstbPlayers.Items.AddRange(_service.PlayerListUpdate());
		}


		#endregion

		enum Modes
		{
			Choosing,
			Created,
			Joined
		}
		Modes _mode;
		Modes Mode
		{
			set
			{
				switch (value)
				{
					case Modes.Choosing:
						btCreate.Enabled = true;
						btRefresh.Enabled = true;
						btJoin.Enabled = (_activeGame != null);
						btLeave.Enabled = false;
						lstbGames.Enabled = true;
						lstbGames.Visible = true;
						break;
					case Modes.Created:
					case Modes.Joined:
						btCreate.Enabled = false;
						btRefresh.Enabled = false;
						btJoin.Enabled = false;
						btLeave.Enabled = true;
						lstbGames.Enabled = false;
						break;
				}
				_mode = value;
				lblStatus.Text = _mode.ToString();
			}
		}

		public GameLevel Level { get; private set; }

		public GameManagerForm(ISkyShootService service)
		{
			_service = service;
			InitializeComponent();
		}

		private void RefreshClick(object sender, EventArgs e)
		{
			try
			{
				_list = _service.GetGameList();
			}
			catch (Exception exc)
			{
				Trace.WriteLine("Cli: Refresh: " + exc);
			}
			lstbGames.Items.Clear();
			foreach (var i in _list)
			{
				lstbGames.Items.Add(i);
			}
		}

		private void GameManagerForm_OnLoad(object sender, EventArgs e)
		{
			Mode = Modes.Choosing;
			RefreshClick(sender, e);
		}

		private void btCreate_OnClick(object sender, EventArgs e)
		{
			try
			{
				var newGame = new CreateGameDialog();
				if(newGame.ShowDialog() != DialogResult.OK)
				{
					return;
				}
				_activeGame = _service.CreateGame(newGame.Mode, newGame.MaxPlayers,
				                                  newGame.Tile);
				if (_activeGame != null)
				{
					Mode = Modes.Created;
				}
				else
				{
					Trace.WriteLine("Cli: CreateGame: can't create");
				}
			}
			catch (Exception exc)
			{
				Trace.WriteLine("Cli: CreateGame: " + exc);
			}
		}

		private void lstbGames_OnSelectedIndexChanged(object sender, EventArgs e)
		{
			_activeGame = lstbGames.SelectedItem as GameDescription;
			if (_activeGame == null)
				return;
			lstbPlayers.Items.Clear();
			foreach (var name in _activeGame.Players)
			{
				lstbPlayers.Items.Add(name);
			}
			Mode = Modes.Choosing;
		}

		private void btLeave_OnClick(object sender, EventArgs e)
		{
			_service.LeaveGame();
			lstbGames.ClearSelected();
			RefreshClick(sender, e);
			_activeGame = null;
			Mode = Modes.Choosing;
		}

		private void btJoin_OnClick(object sender, EventArgs e)
		{
			if (_activeGame == null)
				return;
			_service.JoinGame(_activeGame);
			RefreshClick(sender, e);
			Mode = Modes.Joined;
		}



		private void GameManagerForm_OnFormClosed(object sender, FormClosedEventArgs e)
		{
			//Application.Exit();
		}
	}
}
