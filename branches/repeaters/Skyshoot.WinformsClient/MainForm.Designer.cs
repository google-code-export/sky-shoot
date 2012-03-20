﻿namespace SkyShoot.WinFormsClient
{
	partial class MainForm
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
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this._pnCanvas = new System.Windows.Forms.Panel();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 452);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(640, 22);
			this.statusStrip1.TabIndex = 3;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.AutoSize = false;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(200, 17);
			this.toolStripStatusLabel1.Text = "Ready";
			this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _pnCanvas
			// 
			this._pnCanvas.Cursor = System.Windows.Forms.Cursors.Cross;
			this._pnCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
			this._pnCanvas.Location = new System.Drawing.Point(0, 0);
			this._pnCanvas.Name = "_pnCanvas";
			this._pnCanvas.Size = new System.Drawing.Size(640, 452);
			this._pnCanvas.TabIndex = 4;
			this._pnCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_OnPaint);
			this._pnCanvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnCanvas_OnClick);
			this._pnCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnCanvas_OnMouseMove);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(640, 474);
			this.Controls.Add(this._pnCanvas);
			this.Controls.Add(this.statusStrip1);
			this.Name = "MainForm";
			this.Text = "SkyShoot Game";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_OnFormClosing);
			this.Load += new System.EventHandler(this.MainForm_OnLoad);
			//this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(MainForm_KeyPress);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_OnKeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_OnKeyUp);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.Panel _pnCanvas;
	}
}

