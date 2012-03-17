using System;
using System.Windows.Forms;
using SkyShoot.Contracts.Session;

namespace SkyShoot.WinFormsClient
{
	public partial class CreateGameDialog : Form
	{
		#region Accessors

		public TileSet Tile 
		{
			get
			{
				TileSet res;
				return Enum.TryParse(cbMap.SelectedItem.ToString(), out res) ? res : TileSet.Snow;
			}
		}

		public int MaxPlayers
		{
			get { return (int) nmMaxPlayers.Value; }
		}

		public GameMode Mode
		{
			get
			{
				GameMode res;
				return Enum.TryParse(cbMode.SelectedItem.ToString(), out res) ? res:GameMode.Coop;
			}
		}

		#endregion

		public CreateGameDialog()
		{
			InitializeComponent();
		}

		private void CreateGameDialog_OnLoad(object sender, EventArgs e)
		{
			cbMap.Items.AddRange(Enum.GetNames(typeof(TileSet)));
			cbMode.Items.AddRange(Enum.GetNames(typeof(GameMode)));
			cbMap.SelectedIndex = 0;
			cbMode.SelectedIndex = 0;
		}
	}
}
