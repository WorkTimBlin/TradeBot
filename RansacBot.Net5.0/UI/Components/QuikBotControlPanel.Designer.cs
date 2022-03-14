
namespace RansacBot.UI.Components
{
	partial class QuikBotControlPanel
	{
		/// <summary> 
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором компонентов

		/// <summary> 
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.compensateClosedStopsCheckBox = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(3, 43);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(140, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "Close Positions Precent";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(149, 43);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(43, 23);
			this.numericUpDown1.TabIndex = 3;
			// 
			// compensateClosedStopsCheckBox
			// 
			this.compensateClosedStopsCheckBox.AutoSize = true;
			this.compensateClosedStopsCheckBox.Location = new System.Drawing.Point(3, 3);
			this.compensateClosedStopsCheckBox.Name = "compensateClosedStopsCheckBox";
			this.compensateClosedStopsCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.compensateClosedStopsCheckBox.Size = new System.Drawing.Size(161, 34);
			this.compensateClosedStopsCheckBox.TabIndex = 5;
			this.compensateClosedStopsCheckBox.Text = "Compensate closed stops\r\n with market order";
			this.compensateClosedStopsCheckBox.UseVisualStyleBackColor = true;
			// 
			// QuikBotControlPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.compensateClosedStopsCheckBox);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.button1);
			this.Name = "QuikBotControlPanel";
			this.Size = new System.Drawing.Size(196, 437);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.CheckBox compensateClosedStopsCheckBox;
	}
}
