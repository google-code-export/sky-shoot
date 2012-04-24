using System;
using System.ServiceModel;
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
        private ISkyShootLogin _login;
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
                    using (OperationContextScope scope = new OperationContextScope((IContextChannel)_login))
                    {
                        var header = new MessageHeader<Guid>(_id);
                        var untyped = header.GetUntypedHeader("ID", "namespace");
                        if (OperationContext.Current.OutgoingMessageHeaders.FindHeader("ID", "namespace") == -1)
                        {
                            OperationContext.Current.OutgoingMessageHeaders.Add(untyped);
                        }
                    }
				    Level = _login.GameStart(_activeGame.GameId);
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
			_updatingThread.Abort();			
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
            // todo: !!!
            using (OperationContextScope scope = new OperationContextScope((IContextChannel)_service))
            {
                var header = new MessageHeader<Guid>(_id);
                var untyped = header.GetUntypedHeader("ID", "namespace");
                if (OperationContext.Current.OutgoingMessageHeaders.FindHeader("ID", "namespace") == -1)
                {
                    OperationContext.Current.OutgoingMessageHeaders.Add(untyped);
                }
                var players = _service.PlayerListUpdate();
                lstbPlayers.Items.AddRange(players);
            }
		}


		#endregion

		enum Modes
		{
			Choosing,
			Created,
			Joined
		}
		Modes _mode;
	    private Guid _id;
	    private string _name;

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

		public GameManagerForm(ISkyShootService service, ISkyShootLogin login, Guid id, string name)
		{
		    _name = name;
			_service = service;
		    _login = login;
		    _id = id;
			InitializeComponent();
		}

		private void RefreshClick(object sender, EventArgs e)
		{
            try
            {
                _list = _login.GetGameList();

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
                using (OperationContextScope scope = new OperationContextScope((IContextChannel)_login))
                {
                    var header = new MessageHeader<Guid>(_id);
                    var untyped = header.GetUntypedHeader("ID", "namespace");
                    if (OperationContext.Current.OutgoingMessageHeaders.FindHeader("ID", "namespace") == -1)
                    {
                        OperationContext.Current.OutgoingMessageHeaders.Add(untyped);
                    }
                    var header2 = new MessageHeader<string>(_name);
                    var untyped2 = header2.GetUntypedHeader("Name", "namespace");
                    if (OperationContext.Current.OutgoingMessageHeaders.FindHeader("Name", "namespace") == -1)
                    {
                        OperationContext.Current.OutgoingMessageHeaders.Add(untyped2);
                    }
                    _activeGame = _login.CreateGame(newGame.Mode, newGame.MaxPlayers,
                                                    newGame.Tile);
                }
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
            using (OperationContextScope scope = new OperationContextScope((IContextChannel)_login))
            {
                var header = new MessageHeader<Guid>(_id);
                var untyped = header.GetUntypedHeader("ID", "namespace");
                if (OperationContext.Current.OutgoingMessageHeaders.FindHeader("ID", "namespace") == -1)
                {
                    OperationContext.Current.OutgoingMessageHeaders.Add(untyped);
                }
                _login.LeaveGame();
            }
		    lstbGames.ClearSelected();
			RefreshClick(sender, e);
			_activeGame = null;
			Mode = Modes.Choosing;
		}

		private void btJoin_OnClick(object sender, EventArgs e)
		{
			if (_activeGame == null)
				return;
            using (OperationContextScope scope = new OperationContextScope((IContextChannel)_login))
            {
                var header = new MessageHeader<Guid>(_id);
                var untyped = header.GetUntypedHeader("ID", "namespace");
                if (OperationContext.Current.OutgoingMessageHeaders.FindHeader("ID", "namespace") == -1)
                {
                    OperationContext.Current.OutgoingMessageHeaders.Add(untyped);
                }
                var header2 = new MessageHeader<string>(_name);
                var untyped2 = header2.GetUntypedHeader("Name", "namespace");
                if (OperationContext.Current.OutgoingMessageHeaders.FindHeader("Name", "namespace") == -1)
                {
                    OperationContext.Current.OutgoingMessageHeaders.Add(untyped2);
                }
                _login.JoinGame(_activeGame);
            }
		    RefreshClick(sender, e);
			Mode = Modes.Joined;
		}


		private void BtExitClick(object sender, EventArgs e)
		{
			_updatingThread.Abort();
			Application.Exit();
		}
	}
}
