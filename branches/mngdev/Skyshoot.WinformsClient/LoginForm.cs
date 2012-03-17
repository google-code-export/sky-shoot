using System;
using System.Windows.Forms;

namespace SkyShoot.WinFormsClient
{
	public partial class LoginForm : Form
	{
		public LoginForm()
		{
			InitializeComponent();
		}

		public string UserName { get; private set; }
		public string Password { get; private set; }

		private void button1_OnClick(object sender, EventArgs e)
		{
			UserName = tbLogin.Text;
			Password = tbPassword.Text;
			DialogResult = DialogResult.OK;
		}

		private void btExit_OnClick(object sender, EventArgs e)
		{
			Application.Exit();
		}


	}
}
