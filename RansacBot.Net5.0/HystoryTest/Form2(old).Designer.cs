
namespace RansacBot.HystoryTest
{
	partial class Form2
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
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.goButton = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(12, 31);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(776, 23);
			this.textBox1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(77, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "Data file path";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(11, 75);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(776, 23);
			this.textBox2.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 57);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 15);
			this.label2.TabIndex = 1;
			this.label2.Text = "output folder path";
			// 
			// goButton
			// 
			this.goButton.Font = new System.Drawing.Font("Segoe UI", 90F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.goButton.Image = global::RansacBot.Properties.Resources._1;
			this.goButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.goButton.Location = new System.Drawing.Point(13, 143);
			this.goButton.Name = "goButton";
			this.goButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.goButton.Size = new System.Drawing.Size(470, 295);
			this.goButton.TabIndex = 2;
			this.goButton.Text = "Go";
			this.goButton.UseVisualStyleBackColor = true;
			this.goButton.Click += new System.EventHandler(this.goButton_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(13, 105);
			this.progressBar1.Maximum = 2153;
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(774, 32);
			this.progressBar1.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label3.Location = new System.Drawing.Point(489, 143);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(0, 89);
			this.label3.TabIndex = 4;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label4.Location = new System.Drawing.Point(489, 349);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(0, 89);
			this.label4.TabIndex = 4;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.goButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.textBox1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button goButton;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
	}
}