
namespace RansacBot.UI
{
	partial class FormBotOrHystoryTestChoice
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
			this.buttonBotDemo = new System.Windows.Forms.Button();
			this.buttonHystoryTest = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonBotDemo
			// 
			this.buttonBotDemo.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.buttonBotDemo.Location = new System.Drawing.Point(13, 13);
			this.buttonBotDemo.Name = "buttonBotDemo";
			this.buttonBotDemo.Size = new System.Drawing.Size(387, 425);
			this.buttonBotDemo.TabIndex = 0;
			this.buttonBotDemo.Text = "Bot Demo";
			this.buttonBotDemo.UseVisualStyleBackColor = true;
			this.buttonBotDemo.Click += new System.EventHandler(this.buttonBotDemo_Click);
			// 
			// buttonHystoryTest
			// 
			this.buttonHystoryTest.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.buttonHystoryTest.Location = new System.Drawing.Point(406, 13);
			this.buttonHystoryTest.Name = "buttonHystoryTest";
			this.buttonHystoryTest.Size = new System.Drawing.Size(382, 425);
			this.buttonHystoryTest.TabIndex = 0;
			this.buttonHystoryTest.Text = "Hystory Test";
			this.buttonHystoryTest.UseVisualStyleBackColor = true;
			this.buttonHystoryTest.Click += new System.EventHandler(this.buttonHystoryTest_Click);
			// 
			// FormBotOrHystoryTestChoice
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.buttonHystoryTest);
			this.Controls.Add(this.buttonBotDemo);
			this.Name = "FormBotOrHystoryTestChoice";
			this.Text = "FormBotOrHystoryTestChoice";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonBotDemo;
		private System.Windows.Forms.Button buttonHystoryTest;
	}
}