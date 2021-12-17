
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
			this.button1 = new System.Windows.Forms.Button();
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
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(695, 192);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(93, 33);
			this.button1.TabIndex = 1;
			this.button1.Text = "show demo";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.OnShowDemo);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.plotView1);
			this.Name = "FormMain";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private OxyPlot.WindowsForms.PlotView plotView1;
		private System.Windows.Forms.Button button1;
	}
}