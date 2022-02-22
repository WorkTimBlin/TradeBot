﻿
namespace RansacBot
{
	partial class FormBotPreview
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
			this.components = new System.ComponentModel.Container();
			this.showDemoContinousButton = new System.Windows.Forms.Button();
			this.showDemoAllAtOnseButton = new System.Windows.Forms.Button();
			this.firstOnly = new System.Windows.Forms.CheckBox();
			this.pause = new System.Windows.Forms.CheckBox();
			this.stop = new System.Windows.Forms.Button();
			this.plotView1 = new OxyPlot.WindowsForms.PlotView();
			this.numericUpDown_Speed = new System.Windows.Forms.NumericUpDown();
			this.buttonQuickWatch = new System.Windows.Forms.Button();
			this.numericUpDown_NSetter = new System.Windows.Forms.NumericUpDown();
			this.labelNSetter = new System.Windows.Forms.Label();
			this.labelSpeed = new System.Windows.Forms.Label();
			this.plotView2 = new OxyPlot.WindowsForms.PlotView();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.reloadQuik = new System.Windows.Forms.Button();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Speed)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_NSetter)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// showDemoContinousButton
			// 
			this.showDemoContinousButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.showDemoContinousButton.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.showDemoContinousButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.showDemoContinousButton.ForeColor = System.Drawing.SystemColors.ControlText;
			this.showDemoContinousButton.Location = new System.Drawing.Point(820, 146);
			this.showDemoContinousButton.Name = "showDemoContinousButton";
			this.showDemoContinousButton.Size = new System.Drawing.Size(93, 40);
			this.showDemoContinousButton.TabIndex = 1;
			this.showDemoContinousButton.Text = "show demo continous";
			this.showDemoContinousButton.UseVisualStyleBackColor = false;
			this.showDemoContinousButton.Click += new System.EventHandler(this.OnShowDemoProgressing);
			// 
			// showDemoAllAtOnseButton
			// 
			this.showDemoAllAtOnseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.showDemoAllAtOnseButton.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.showDemoAllAtOnseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.showDemoAllAtOnseButton.Location = new System.Drawing.Point(820, 192);
			this.showDemoAllAtOnseButton.Name = "showDemoAllAtOnseButton";
			this.showDemoAllAtOnseButton.Size = new System.Drawing.Size(93, 40);
			this.showDemoAllAtOnseButton.TabIndex = 1;
			this.showDemoAllAtOnseButton.Text = "show demo all at once";
			this.showDemoAllAtOnseButton.UseVisualStyleBackColor = false;
			this.showDemoAllAtOnseButton.Click += new System.EventHandler(this.OnShowDemoAllAtOnce);
			// 
			// firstOnly
			// 
			this.firstOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.firstOnly.Appearance = System.Windows.Forms.Appearance.Button;
			this.firstOnly.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.firstOnly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.firstOnly.Location = new System.Drawing.Point(820, 65);
			this.firstOnly.Name = "firstOnly";
			this.firstOnly.Size = new System.Drawing.Size(93, 75);
			this.firstOnly.TabIndex = 2;
			this.firstOnly.Text = "show only first builded versions of ransacs";
			this.firstOnly.UseVisualStyleBackColor = false;
			// 
			// pause
			// 
			this.pause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pause.Appearance = System.Windows.Forms.Appearance.Button;
			this.pause.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.pause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.pause.Location = new System.Drawing.Point(820, 238);
			this.pause.Name = "pause";
			this.pause.Size = new System.Drawing.Size(24, 24);
			this.pause.TabIndex = 3;
			this.pause.Text = "II";
			this.pause.UseVisualStyleBackColor = false;
			this.pause.CheckedChanged += new System.EventHandler(this.pause_CheckedChanged);
			// 
			// stop
			// 
			this.stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.stop.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.stop.Location = new System.Drawing.Point(850, 238);
			this.stop.Name = "stop";
			this.stop.Size = new System.Drawing.Size(48, 24);
			this.stop.TabIndex = 4;
			this.stop.Text = "stop";
			this.stop.UseVisualStyleBackColor = false;
			this.stop.Click += new System.EventHandler(this.stop_Click);
			// 
			// plotView1
			// 
			this.plotView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.plotView1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.plotView1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.plotView1.Location = new System.Drawing.Point(-3, 0);
			this.plotView1.MinimumSize = new System.Drawing.Size(600, 0);
			this.plotView1.Name = "plotView1";
			this.plotView1.PanCursor = System.Windows.Forms.Cursors.Hand;
			this.plotView1.Size = new System.Drawing.Size(652, 253);
			this.plotView1.TabIndex = 0;
			this.plotView1.Text = "plotView1";
			this.plotView1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
			this.plotView1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
			this.plotView1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
			// 
			// numericUpDown_Speed
			// 
			this.numericUpDown_Speed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDown_Speed.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.numericUpDown_Speed.Location = new System.Drawing.Point(820, 323);
			this.numericUpDown_Speed.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.numericUpDown_Speed.Name = "numericUpDown_Speed";
			this.numericUpDown_Speed.Size = new System.Drawing.Size(93, 23);
			this.numericUpDown_Speed.TabIndex = 7;
			this.numericUpDown_Speed.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
			// 
			// buttonQuickWatch
			// 
			this.buttonQuickWatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonQuickWatch.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.buttonQuickWatch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonQuickWatch.Location = new System.Drawing.Point(820, 503);
			this.buttonQuickWatch.Name = "buttonQuickWatch";
			this.buttonQuickWatch.Size = new System.Drawing.Size(93, 23);
			this.buttonQuickWatch.TabIndex = 8;
			this.buttonQuickWatch.Text = "QuickTicks";
			this.buttonQuickWatch.UseVisualStyleBackColor = false;
			this.buttonQuickWatch.Click += new System.EventHandler(this.buttonQuickWatch_Click);
			// 
			// numericUpDown_NSetter
			// 
			this.numericUpDown_NSetter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDown_NSetter.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDown_NSetter.Location = new System.Drawing.Point(820, 474);
			this.numericUpDown_NSetter.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDown_NSetter.Name = "numericUpDown_NSetter";
			this.numericUpDown_NSetter.ReadOnly = true;
			this.numericUpDown_NSetter.Size = new System.Drawing.Size(93, 23);
			this.numericUpDown_NSetter.TabIndex = 9;
			this.numericUpDown_NSetter.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// labelNSetter
			// 
			this.labelNSetter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelNSetter.AutoSize = true;
			this.labelNSetter.Location = new System.Drawing.Point(820, 456);
			this.labelNSetter.Name = "labelNSetter";
			this.labelNSetter.Size = new System.Drawing.Size(19, 15);
			this.labelNSetter.TabIndex = 10;
			this.labelNSetter.Text = "N:";
			// 
			// labelSpeed
			// 
			this.labelSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelSpeed.AutoSize = true;
			this.labelSpeed.Location = new System.Drawing.Point(820, 305);
			this.labelSpeed.Name = "labelSpeed";
			this.labelSpeed.Size = new System.Drawing.Size(42, 15);
			this.labelSpeed.TabIndex = 11;
			this.labelSpeed.Text = "Speed:";
			// 
			// plotView2
			// 
			this.plotView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.plotView2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.plotView2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.plotView2.Location = new System.Drawing.Point(0, -1);
			this.plotView2.MinimumSize = new System.Drawing.Size(600, 0);
			this.plotView2.Name = "plotView2";
			this.plotView2.PanCursor = System.Windows.Forms.Cursors.Hand;
			this.plotView2.Size = new System.Drawing.Size(649, 255);
			this.plotView2.TabIndex = 0;
			this.plotView2.Text = "plotView1";
			this.plotView2.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
			this.plotView2.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
			this.plotView2.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Cursor = System.Windows.Forms.Cursors.HSplit;
			this.splitContainer1.Location = new System.Drawing.Point(162, 12);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.plotView1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.plotView2);
			this.splitContainer1.Size = new System.Drawing.Size(652, 514);
			this.splitContainer1.SplitterDistance = 256;
			this.splitContainer1.TabIndex = 12;
			// 
			// reloadQuik
			// 
			this.reloadQuik.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.reloadQuik.BackColor = System.Drawing.Color.DimGray;
			this.reloadQuik.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.reloadQuik.Location = new System.Drawing.Point(820, 12);
			this.reloadQuik.Name = "reloadQuik";
			this.reloadQuik.Size = new System.Drawing.Size(75, 23);
			this.reloadQuik.TabIndex = 13;
			this.reloadQuik.Text = "Reload Quik";
			this.reloadQuik.UseVisualStyleBackColor = false;
			this.reloadQuik.Click += new System.EventHandler(this.reloadQuik_Click);
			// 
			// listBox1
			// 
			this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 15;
			this.listBox1.Location = new System.Drawing.Point(12, 12);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(141, 514);
			this.listBox1.TabIndex = 14;
			// 
			// timer1
			// 
			this.timer1.Interval = 500;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// FormBotPreview
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.ClientSize = new System.Drawing.Size(925, 538);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.reloadQuik);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.labelSpeed);
			this.Controls.Add(this.labelNSetter);
			this.Controls.Add(this.numericUpDown_NSetter);
			this.Controls.Add(this.buttonQuickWatch);
			this.Controls.Add(this.numericUpDown_Speed);
			this.Controls.Add(this.stop);
			this.Controls.Add(this.pause);
			this.Controls.Add(this.firstOnly);
			this.Controls.Add(this.showDemoAllAtOnseButton);
			this.Controls.Add(this.showDemoContinousButton);
			this.MinimumSize = new System.Drawing.Size(941, 577);
			this.Name = "FormBotPreview";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Speed)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_NSetter)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button showDemoContinousButton;
		private System.Windows.Forms.Button showDemoAllAtOnseButton;
		private System.Windows.Forms.CheckBox firstOnly;
		private System.Windows.Forms.CheckBox pause;
		private System.Windows.Forms.Button stop;
		private OxyPlot.WindowsForms.PlotView plotView1;
		private System.Windows.Forms.NumericUpDown numericUpDown_Speed;
		private System.Windows.Forms.Button buttonQuickWatch;
		private System.Windows.Forms.NumericUpDown numericUpDown_NSetter;
		private System.Windows.Forms.Label labelNSetter;
		private System.Windows.Forms.Label labelSpeed;
		private OxyPlot.WindowsForms.PlotView plotView2;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Button reloadQuik;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Timer timer1;
	}
}