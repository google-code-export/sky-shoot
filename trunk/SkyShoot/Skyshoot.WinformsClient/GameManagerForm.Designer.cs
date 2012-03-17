namespace SkyShoot.WinFormsClient
{
	partial class GameManagerForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btRefresh = new System.Windows.Forms.Button();
			this.btCreate = new System.Windows.Forms.Button();
			this.btJoin = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.lstbGames = new System.Windows.Forms.ListBox();
			this.lstbPlayers = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lblStatus = new System.Windows.Forms.Label();
			this.btLeave = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btRefresh
			// 
			this.btRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btRefresh.Location = new System.Drawing.Point(259, 46);
			this.btRefresh.Name = "btRefresh";
			this.btRefresh.Size = new System.Drawing.Size(106, 28);
			this.btRefresh.TabIndex = 0;
			this.btRefresh.Text = "Refresh";
			this.btRefresh.UseVisualStyleBackColor = true;
			this.btRefresh.Click += new System.EventHandler(this.RefreshClick);
			// 
			// btCreate
			// 
			this.btCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btCreate.Location = new System.Drawing.Point(259, 12);
			this.btCreate.Name = "btCreate";
			this.btCreate.Size = new System.Drawing.Size(106, 28);
			this.btCreate.TabIndex = 1;
			this.btCreate.Text = "Create";
			this.btCreate.UseVisualStyleBackColor = true;
			this.btCreate.Click += new System.EventHandler(this.btCreate_OnClick);
			// 
			// btJoin
			// 
			this.btJoin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btJoin.Location = new System.Drawing.Point(259, 80);
			this.btJoin.Name = "btJoin";
			this.btJoin.Size = new System.Drawing.Size(106, 28);
			this.btJoin.TabIndex = 2;
			this.btJoin.Text = "Join";
			this.btJoin.UseVisualStyleBackColor = true;
			this.btJoin.Click += new System.EventHandler(this.btJoin_OnClick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Games list:";
			// 
			// lstbGames
			// 
			this.lstbGames.FormattingEnabled = true;
			this.lstbGames.Location = new System.Drawing.Point(15, 33);
			this.lstbGames.Name = "lstbGames";
			this.lstbGames.Size = new System.Drawing.Size(233, 160);
			this.lstbGames.TabIndex = 4;
			this.lstbGames.SelectedIndexChanged += new System.EventHandler(this.lstbGames_OnSelectedIndexChanged);
			// 
			// lstbPlayers
			// 
			this.lstbPlayers.FormattingEnabled = true;
			this.lstbPlayers.Location = new System.Drawing.Point(15, 224);
			this.lstbPlayers.Name = "lstbPlayers";
			this.lstbPlayers.Size = new System.Drawing.Size(233, 134);
			this.lstbPlayers.TabIndex = 6;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 203);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(44, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Players:";
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.Location = new System.Drawing.Point(256, 169);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(35, 13);
			this.lblStatus.TabIndex = 7;
			this.lblStatus.Text = "label3";
			// 
			// btLeave
			// 
			this.btLeave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btLeave.Location = new System.Drawing.Point(259, 114);
			this.btLeave.Name = "btLeave";
			this.btLeave.Size = new System.Drawing.Size(106, 28);
			this.btLeave.TabIndex = 8;
			this.btLeave.Text = "Leave";
			this.btLeave.UseVisualStyleBackColor = true;
			this.btLeave.Click += new System.EventHandler(this.btLeave_OnClick);
			// 
			// GameManagerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(377, 381);
			this.Controls.Add(this.btLeave);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.lstbPlayers);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lstbGames);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btJoin);
			this.Controls.Add(this.btCreate);
			this.Controls.Add(this.btRefresh);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GameManagerForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "GameManagerForm";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GameManagerForm_OnFormClosed);
			this.Load += new System.EventHandler(this.GameManagerForm_OnLoad);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btRefresh;
		private System.Windows.Forms.Button btCreate;
		private System.Windows.Forms.Button btJoin;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox lstbGames;
		private System.Windows.Forms.ListBox lstbPlayers;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Button btLeave;
	}
}