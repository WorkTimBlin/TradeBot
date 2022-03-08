
namespace RansacBot.UI.Components
{
	partial class CurfewControl
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
			this.controlNameLabel = new System.Windows.Forms.Label();
			this.closingTimePicker = new RansacBot.UI.Components.TimePicker();
			this.closingTimeLabel = new System.Windows.Forms.Label();
			this.openingTimePicker = new RansacBot.UI.Components.TimePicker();
			this.openingTimeLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// controlNameLabel
			// 
			this.controlNameLabel.AutoSize = true;
			this.controlNameLabel.Location = new System.Drawing.Point(4, 4);
			this.controlNameLabel.Name = "controlNameLabel";
			this.controlNameLabel.Size = new System.Drawing.Size(88, 15);
			this.controlNameLabel.TabIndex = 0;
			this.controlNameLabel.Text = "Curfew Control";
			// 
			// closingTimePicker
			// 
			this.closingTimePicker.Hours = 23;
			this.closingTimePicker.Location = new System.Drawing.Point(4, 41);
			this.closingTimePicker.Minutes = 23;
			this.closingTimePicker.Name = "closingTimePicker";
			this.closingTimePicker.Size = new System.Drawing.Size(102, 58);
			this.closingTimePicker.TabIndex = 1;
			// 
			// closingTimeLabel
			// 
			this.closingTimeLabel.AutoSize = true;
			this.closingTimeLabel.Location = new System.Drawing.Point(4, 23);
			this.closingTimeLabel.Name = "closingTimeLabel";
			this.closingTimeLabel.Size = new System.Drawing.Size(76, 15);
			this.closingTimeLabel.TabIndex = 2;
			this.closingTimeLabel.Text = "Closing Time";
			// 
			// openingTimePicker
			// 
			this.openingTimePicker.Hours = 23;
			this.openingTimePicker.Location = new System.Drawing.Point(112, 41);
			this.openingTimePicker.Minutes = 23;
			this.openingTimePicker.Name = "openingTimePicker";
			this.openingTimePicker.Size = new System.Drawing.Size(102, 58);
			this.openingTimePicker.TabIndex = 1;
			// 
			// openingTimeLabel
			// 
			this.openingTimeLabel.AutoSize = true;
			this.openingTimeLabel.Location = new System.Drawing.Point(112, 23);
			this.openingTimeLabel.Name = "openingTimeLabel";
			this.openingTimeLabel.Size = new System.Drawing.Size(82, 15);
			this.openingTimeLabel.TabIndex = 2;
			this.openingTimeLabel.Text = "Opening Time";
			// 
			// CurfewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.openingTimeLabel);
			this.Controls.Add(this.closingTimeLabel);
			this.Controls.Add(this.openingTimePicker);
			this.Controls.Add(this.closingTimePicker);
			this.Controls.Add(this.controlNameLabel);
			this.Name = "CurfewControl";
			this.Size = new System.Drawing.Size(212, 97);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label controlNameLabel;
		private TimePicker closingTimePicker;
		private System.Windows.Forms.Label closingTimeLabel;
		private TimePicker openingTimePicker;
		private System.Windows.Forms.Label openingTimeLabel;
	}
}
