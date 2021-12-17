
namespace RansacBot
{
	partial class FormMain
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
			this.plotView1 = new OxyPlot.WindowsForms.PlotView();
			this.ShowDemoContinous = new System.Windows.Forms.Button();
			this.ShowDemoAllAtOnse = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// plotView1
			// 
			this.plotView1.Location = new System.Drawing.Point(12, 12);
			this.plotView1.Name = "plotView1";
			this.plotView1.PanCursor = System.Windows.Forms.Cursors.Hand;
			this.plotView1.Size = new System.Drawing.Size(607, 426);
			this.plotView1.TabIndex = 0;
			this.plotView1.Text = "plotView1";
			this.plotView1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
			this.plotView1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
			this.plotView1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
			// 
			// ShowDemoContinous
			// 
			this.ShowDemoContinous.Location = new System.Drawing.Point(695, 146);
			this.ShowDemoContinous.Name = "ShowDemoContinous";
			this.ShowDemoContinous.Size = new System.Drawing.Size(93, 40);
			this.ShowDemoContinous.TabIndex = 1;
			this.ShowDemoContinous.Text = "show demo continous";
			this.ShowDemoContinous.UseVisualStyleBackColor = true;
			this.ShowDemoContinous.Click += new System.EventHandler(this.OnShowDemoProgressing);
			// 
			// ShowDemoAllAtOnse
			// 
			this.ShowDemoAllAtOnse.Location = new System.Drawing.Point(695, 192);
			this.ShowDemoAllAtOnse.Name = "ShowDemoAllAtOnse";
			this.ShowDemoAllAtOnse.Size = new System.Drawing.Size(93, 40);
			this.ShowDemoAllAtOnse.TabIndex = 1;
			this.ShowDemoAllAtOnse.Text = "show demo all at once";
			this.ShowDemoAllAtOnse.UseVisualStyleBackColor = true;
			this.ShowDemoAllAtOnse.Click += new System.EventHandler(this.OnShowDemoProgressing);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.ShowDemoAllAtOnse);
			this.Controls.Add(this.ShowDemoContinous);
			this.Controls.Add(this.plotView1);
			this.Name = "FormMain";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private OxyPlot.WindowsForms.PlotView plotView1;
		private System.Windows.Forms.Button ShowDemoContinous;
		private System.Windows.Forms.Button ShowDemoAllAtOnse;
	}
}