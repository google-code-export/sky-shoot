namespace SkyShoot.WinFormsClient
{
	partial class CreateGameDialog
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
			this.btOk = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.nmMaxPlayers = new System.Windows.Forms.NumericUpDown();
			this.cbMap = new System.Windows.Forms.ComboBox();
			this.cbMode = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.nmMaxPlayers)).BeginInit();
			this.SuspendLayout();
			// 
			// btOk
			// 
			this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOk.Location = new System.Drawing.Point(15, 108);
			this.btOk.Name = "btOk";
			this.btOk.Size = new System.Drawing.Size(75, 23);
			this.btOk.TabIndex = 0;
			this.btOk.Text = "Ok";
			this.btOk.UseVisualStyleBackColor = true;
			// 
			// btCancel
			// 
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Location = new System.Drawing.Point(129, 108);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(75, 23);
			this.btCancel.TabIndex = 1;
			this.btCancel.Text = "Cancel";
			this.btCancel.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(66, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Max players:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 36);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(31, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Map:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 63);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(37, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Mode:";
			// 
			// nmMaxPlayers
			// 
			this.nmMaxPlayers.Location = new System.Drawing.Point(84, 7);
			this.nmMaxPlayers.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
			this.nmMaxPlayers.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nmMaxPlayers.Name = "nmMaxPlayers";
			this.nmMaxPlayers.Size = new System.Drawing.Size(120, 20);
			this.nmMaxPlayers.TabIndex = 5;
			this.nmMaxPlayers.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// cbMap
			// 
			this.cbMap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbMap.FormattingEnabled = true;
			this.cbMap.Location = new System.Drawing.Point(84, 33);
			this.cbMap.Name = "cbMap";
			this.cbMap.Size = new System.Drawing.Size(121, 21);
			this.cbMap.TabIndex = 6;
			// 
			// cbMode
			// 
			this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbMode.Enabled = false;
			this.cbMode.FormattingEnabled = true;
			this.cbMode.Location = new System.Drawing.Point(84, 60);
			this.cbMode.Name = "cbMode";
			this.cbMode.Size = new System.Drawing.Size(121, 21);
			this.cbMode.TabIndex = 7;
			// 
			// CreateGameDialog
			// 
			this.AcceptButton = this.btOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(218, 147);
			this.Controls.Add(this.cbMode);
			this.Controls.Add(this.cbMap);
			this.Controls.Add(this.nmMaxPlayers);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.btOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CreateGameDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "CreateGameDialog";
			this.Load += new System.EventHandler(this.CreateGameDialog_OnLoad);
			((System.ComponentModel.ISupportInitialize)(this.nmMaxPlayers)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btOk;
		private System.Windows.Forms.Button btCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown nmMaxPlayers;
		private System.Windows.Forms.ComboBox cbMap;
		private System.Windows.Forms.ComboBox cbMode;
	}
}