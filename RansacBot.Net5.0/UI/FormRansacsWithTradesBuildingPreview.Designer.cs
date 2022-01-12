
namespace RansacBot
{
    partial class FormRansacsWithTradesBuildingPreview
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
			this.ShowDemoContinous = new System.Windows.Forms.Button();
			this.ShowDemoAllAtOnse = new System.Windows.Forms.Button();
			this.firstOnly = new System.Windows.Forms.CheckBox();
			this.pause = new System.Windows.Forms.CheckBox();
			this.stop = new System.Windows.Forms.Button();
			this.plotView1 = new OxyPlot.WindowsForms.PlotView();
			this.sigmaType = new System.Windows.Forms.ComboBox();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.buttonQuickWatch = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// ShowDemoContinous
			// 
			this.ShowDemoContinous.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ShowDemoContinous.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.ShowDemoContinous.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.ShowDemoContinous.ForeColor = System.Drawing.SystemColors.ControlText;
			this.ShowDemoContinous.Location = new System.Drawing.Point(695, 146);
			this.ShowDemoContinous.Name = "ShowDemoContinous";
			this.ShowDemoContinous.Size = new System.Drawing.Size(93, 40);
			this.ShowDemoContinous.TabIndex = 1;
			this.ShowDemoContinous.Text = "show demo continous";
			this.ShowDemoContinous.UseVisualStyleBackColor = false;
			this.ShowDemoContinous.Click += new System.EventHandler(this.OnShowDemoProgressing);
			// 
			// ShowDemoAllAtOnse
			// 
			this.ShowDemoAllAtOnse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ShowDemoAllAtOnse.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.ShowDemoAllAtOnse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.ShowDemoAllAtOnse.Location = new System.Drawing.Point(695, 192);
			this.ShowDemoAllAtOnse.Name = "ShowDemoAllAtOnse";
			this.ShowDemoAllAtOnse.Size = new System.Drawing.Size(93, 40);
			this.ShowDemoAllAtOnse.TabIndex = 1;
			this.ShowDemoAllAtOnse.Text = "show demo all at once";
			this.ShowDemoAllAtOnse.UseVisualStyleBackColor = false;
			this.ShowDemoAllAtOnse.Click += new System.EventHandler(this.OnShowDemoAllAtOnce);
			// 
			// firstOnly
			// 
			this.firstOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.firstOnly.Appearance = System.Windows.Forms.Appearance.Button;
			this.firstOnly.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.firstOnly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.firstOnly.Location = new System.Drawing.Point(695, 65);
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
			this.pause.Location = new System.Drawing.Point(695, 238);
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
			this.stop.Location = new System.Drawing.Point(725, 238);
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
			this.plotView1.Location = new System.Drawing.Point(12, 12);
			this.plotView1.MinimumSize = new System.Drawing.Size(677, 426);
			this.plotView1.Name = "plotView1";
			this.plotView1.PanCursor = System.Windows.Forms.Cursors.Hand;
			this.plotView1.Size = new System.Drawing.Size(677, 426);
			this.plotView1.TabIndex = 0;
			this.plotView1.Text = "plotView1";
			this.plotView1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
			this.plotView1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
			this.plotView1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
			// 
			// sigmaType
			// 
			this.sigmaType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.sigmaType.BackColor = System.Drawing.SystemColors.Control;
			this.sigmaType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.sigmaType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.sigmaType.FormattingEnabled = true;
			this.sigmaType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.sigmaType.Location = new System.Drawing.Point(695, 268);
			this.sigmaType.Name = "sigmaType";
			this.sigmaType.Size = new System.Drawing.Size(93, 23);
			this.sigmaType.TabIndex = 5;
			this.sigmaType.Tag = "Sigma Type";
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.numericUpDown1.Location = new System.Drawing.Point(695, 297);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(93, 23);
			this.numericUpDown1.TabIndex = 7;
			this.numericUpDown1.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
			// 
			// buttonQuickWatch
			// 
			this.buttonQuickWatch.Location = new System.Drawing.Point(695, 326);
			this.buttonQuickWatch.Name = "buttonQuickWatch";
			this.buttonQuickWatch.Size = new System.Drawing.Size(93, 23);
			this.buttonQuickWatch.TabIndex = 8;
			this.buttonQuickWatch.Text = "QuickTicks";
			this.buttonQuickWatch.UseVisualStyleBackColor = true;
			this.buttonQuickWatch.Click += new System.EventHandler(this.buttonQuickWatch_Click);
			// 
			// FormRansacsWithTradesBuildingPreview
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.buttonQuickWatch);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.plotView1);
			this.Controls.Add(this.sigmaType);
			this.Controls.Add(this.stop);
			this.Controls.Add(this.pause);
			this.Controls.Add(this.firstOnly);
			this.Controls.Add(this.ShowDemoAllAtOnse);
			this.Controls.Add(this.ShowDemoContinous);
			this.MinimumSize = new System.Drawing.Size(816, 489);
			this.Name = "FormRansacsWithTradesBuildingPreview";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button ShowDemoContinous;
        private System.Windows.Forms.Button ShowDemoAllAtOnse;
        private System.Windows.Forms.CheckBox firstOnly;
        private System.Windows.Forms.CheckBox pause;
        private System.Windows.Forms.Button stop;
        private OxyPlot.WindowsForms.PlotView plotView1;
		private System.Windows.Forms.ComboBox sigmaType;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Button buttonQuickWatch;
	}
}