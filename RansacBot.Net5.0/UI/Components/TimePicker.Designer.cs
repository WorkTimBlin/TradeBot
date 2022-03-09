
namespace RansacBot.UI.Components
{
	partial class TimePicker
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
			this.hoursNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.hoursLabel = new System.Windows.Forms.Label();
			this.minutesNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.minutesLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.hoursNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.minutesNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// hoursNumericUpDown
			// 
			this.hoursNumericUpDown.Location = new System.Drawing.Point(59, 3);
			this.hoursNumericUpDown.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
			this.hoursNumericUpDown.Name = "hoursNumericUpDown";
			this.hoursNumericUpDown.Size = new System.Drawing.Size(40, 23);
			this.hoursNumericUpDown.TabIndex = 0;
			// 
			// hoursLabel
			// 
			this.hoursLabel.AutoSize = true;
			this.hoursLabel.Location = new System.Drawing.Point(3, 5);
			this.hoursLabel.Name = "hoursLabel";
			this.hoursLabel.Size = new System.Drawing.Size(39, 15);
			this.hoursLabel.TabIndex = 1;
			this.hoursLabel.Text = "Hours";
			// 
			// minutesNumericUpDown
			// 
			this.minutesNumericUpDown.Location = new System.Drawing.Point(59, 32);
			this.minutesNumericUpDown.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
			this.minutesNumericUpDown.Name = "minutesNumericUpDown";
			this.minutesNumericUpDown.Size = new System.Drawing.Size(40, 23);
			this.minutesNumericUpDown.TabIndex = 0;
			// 
			// minutesLabel
			// 
			this.minutesLabel.AutoSize = true;
			this.minutesLabel.Location = new System.Drawing.Point(3, 34);
			this.minutesLabel.Name = "minutesLabel";
			this.minutesLabel.Size = new System.Drawing.Size(50, 15);
			this.minutesLabel.TabIndex = 1;
			this.minutesLabel.Text = "Minutes";
			// 
			// TimePicker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.minutesLabel);
			this.Controls.Add(this.minutesNumericUpDown);
			this.Controls.Add(this.hoursLabel);
			this.Controls.Add(this.hoursNumericUpDown);
			this.Name = "TimePicker";
			this.Size = new System.Drawing.Size(102, 58);
			((System.ComponentModel.ISupportInitialize)(this.hoursNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.minutesNumericUpDown)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NumericUpDown hoursNumericUpDown;
		private System.Windows.Forms.Label hoursLabel;
		private System.Windows.Forms.NumericUpDown minutesNumericUpDown;
		private System.Windows.Forms.Label minutesLabel;
	}
}
