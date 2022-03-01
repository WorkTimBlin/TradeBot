
namespace RansacBot.UI.Components
{
	partial class RansacLevelUsageControl
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
			this.sigmaTypeLabel = new System.Windows.Forms.Label();
			this.sigmaTypeComboBox = new System.Windows.Forms.ComboBox();
			this.levelLabel = new System.Windows.Forms.Label();
			this.levelNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.nameLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.levelNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// sigmaTypeLabel
			// 
			this.sigmaTypeLabel.AutoSize = true;
			this.sigmaTypeLabel.Location = new System.Drawing.Point(3, 19);
			this.sigmaTypeLabel.Name = "sigmaTypeLabel";
			this.sigmaTypeLabel.Size = new System.Drawing.Size(67, 15);
			this.sigmaTypeLabel.TabIndex = 0;
			this.sigmaTypeLabel.Text = "Sigma Type";
			// 
			// sigmaTypeComboBox
			// 
			this.sigmaTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.sigmaTypeComboBox.FormattingEnabled = true;
			this.sigmaTypeComboBox.Location = new System.Drawing.Point(76, 16);
			this.sigmaTypeComboBox.Name = "sigmaTypeComboBox";
			this.sigmaTypeComboBox.Size = new System.Drawing.Size(130, 23);
			this.sigmaTypeComboBox.TabIndex = 1;
			// 
			// levelLabel
			// 
			this.levelLabel.AutoSize = true;
			this.levelLabel.Location = new System.Drawing.Point(212, 19);
			this.levelLabel.Name = "levelLabel";
			this.levelLabel.Size = new System.Drawing.Size(34, 15);
			this.levelLabel.TabIndex = 0;
			this.levelLabel.Text = "Level";
			// 
			// levelNumericUpDown
			// 
			this.levelNumericUpDown.Location = new System.Drawing.Point(250, 16);
			this.levelNumericUpDown.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.levelNumericUpDown.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.levelNumericUpDown.Name = "levelNumericUpDown";
			this.levelNumericUpDown.Size = new System.Drawing.Size(37, 23);
			this.levelNumericUpDown.TabIndex = 2;
			this.levelNumericUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new System.Drawing.Point(4, 4);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(44, 15);
			this.nameLabel.TabIndex = 3;
			this.nameLabel.Text = "Ransac";
			// 
			// RansacLevelUsageControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.levelNumericUpDown);
			this.Controls.Add(this.sigmaTypeComboBox);
			this.Controls.Add(this.levelLabel);
			this.Controls.Add(this.sigmaTypeLabel);
			this.MaximumSize = new System.Drawing.Size(292, 42);
			this.MinimumSize = new System.Drawing.Size(292, 42);
			this.Name = "RansacLevelUsageControl";
			this.Size = new System.Drawing.Size(290, 40);
			((System.ComponentModel.ISupportInitialize)(this.levelNumericUpDown)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label sigmaTypeLabel;
		private System.Windows.Forms.ComboBox sigmaTypeComboBox;
		private System.Windows.Forms.Label levelLabel;
		private System.Windows.Forms.NumericUpDown levelNumericUpDown;
		private System.Windows.Forms.Label nameLabel;
	}
}
