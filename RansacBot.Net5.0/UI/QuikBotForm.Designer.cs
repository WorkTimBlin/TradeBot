
namespace RansacBot.UI
{
	partial class QuikBotForm
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.stopPlacingRansacTabPage = new System.Windows.Forms.TabPage();
			this.plotView1 = new OxyPlot.WindowsForms.PlotView();
			this.closingRansacTabPage = new System.Windows.Forms.TabPage();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.listBox2 = new System.Windows.Forms.ListBox();
			this.tradingHystoryLabel = new System.Windows.Forms.Label();
			this.stopsLabel = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.compensateClosedStopsCheckBox = new System.Windows.Forms.CheckBox();
			this.tabControl1.SuspendLayout();
			this.stopPlacingRansacTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.stopPlacingRansacTabPage);
			this.tabControl1.Controls.Add(this.closingRansacTabPage);
			this.tabControl1.Location = new System.Drawing.Point(264, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(361, 426);
			this.tabControl1.TabIndex = 0;
			// 
			// stopPlacingRansacTabPage
			// 
			this.stopPlacingRansacTabPage.Controls.Add(this.plotView1);
			this.stopPlacingRansacTabPage.Location = new System.Drawing.Point(4, 24);
			this.stopPlacingRansacTabPage.Name = "stopPlacingRansacTabPage";
			this.stopPlacingRansacTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.stopPlacingRansacTabPage.Size = new System.Drawing.Size(353, 398);
			this.stopPlacingRansacTabPage.TabIndex = 0;
			this.stopPlacingRansacTabPage.Text = "Stop Placing Ransac";
			this.stopPlacingRansacTabPage.UseVisualStyleBackColor = true;
			// 
			// plotView1
			// 
			this.plotView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.plotView1.Location = new System.Drawing.Point(6, 6);
			this.plotView1.Name = "plotView1";
			this.plotView1.PanCursor = System.Windows.Forms.Cursors.Hand;
			this.plotView1.Size = new System.Drawing.Size(341, 386);
			this.plotView1.TabIndex = 0;
			this.plotView1.Text = "plotView1";
			this.plotView1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
			this.plotView1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
			this.plotView1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
			// 
			// closingRansacTabPage
			// 
			this.closingRansacTabPage.Location = new System.Drawing.Point(4, 24);
			this.closingRansacTabPage.Name = "closingRansacTabPage";
			this.closingRansacTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.closingRansacTabPage.Size = new System.Drawing.Size(353, 398);
			this.closingRansacTabPage.TabIndex = 1;
			this.closingRansacTabPage.Text = "Closing Ransac";
			this.closingRansacTabPage.UseVisualStyleBackColor = true;
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 15;
			this.listBox1.Location = new System.Drawing.Point(138, 42);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(120, 394);
			this.listBox1.TabIndex = 2;
			// 
			// listBox2
			// 
			this.listBox2.FormattingEnabled = true;
			this.listBox2.ItemHeight = 15;
			this.listBox2.Location = new System.Drawing.Point(12, 42);
			this.listBox2.Name = "listBox2";
			this.listBox2.Size = new System.Drawing.Size(120, 394);
			this.listBox2.TabIndex = 2;
			// 
			// tradingHystoryLabel
			// 
			this.tradingHystoryLabel.AutoSize = true;
			this.tradingHystoryLabel.Location = new System.Drawing.Point(13, 13);
			this.tradingHystoryLabel.Name = "tradingHystoryLabel";
			this.tradingHystoryLabel.Size = new System.Drawing.Size(90, 15);
			this.tradingHystoryLabel.TabIndex = 3;
			this.tradingHystoryLabel.Text = "Trading Hystory";
			// 
			// stopsLabel
			// 
			this.stopsLabel.AutoSize = true;
			this.stopsLabel.Location = new System.Drawing.Point(138, 13);
			this.stopsLabel.Name = "stopsLabel";
			this.stopsLabel.Size = new System.Drawing.Size(115, 15);
			this.stopsLabel.TabIndex = 3;
			this.stopsLabel.Text = "Current Active Stops";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(632, 13);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(156, 41);
			this.button1.TabIndex = 1;
			this.button1.Text = "BotTradeButton";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// compensateClosedStopsCheckBox
			// 
			this.compensateClosedStopsCheckBox.AutoSize = true;
			this.compensateClosedStopsCheckBox.Location = new System.Drawing.Point(632, 61);
			this.compensateClosedStopsCheckBox.Name = "compensateClosedStopsCheckBox";
			this.compensateClosedStopsCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.compensateClosedStopsCheckBox.Size = new System.Drawing.Size(161, 34);
			this.compensateClosedStopsCheckBox.TabIndex = 4;
			this.compensateClosedStopsCheckBox.Text = "Compensate closed stops\r\n with market order";
			this.compensateClosedStopsCheckBox.UseVisualStyleBackColor = true;
			// 
			// QuikBotForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.compensateClosedStopsCheckBox);
			this.Controls.Add(this.stopsLabel);
			this.Controls.Add(this.tradingHystoryLabel);
			this.Controls.Add(this.listBox2);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.tabControl1);
			this.Name = "QuikBotForm";
			this.Text = "QuikBotForm";
			this.tabControl1.ResumeLayout(false);
			this.stopPlacingRansacTabPage.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage stopPlacingRansacTabPage;
		private OxyPlot.WindowsForms.PlotView plotView1;
		private System.Windows.Forms.TabPage closingRansacTabPage;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.ListBox listBox2;
		private System.Windows.Forms.Label tradingHystoryLabel;
		private System.Windows.Forms.Label stopsLabel;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox compensateClosedStopsCheckBox;
	}
}