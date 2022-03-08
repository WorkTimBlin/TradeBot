
namespace RansacBot.UI
{
	partial class CurfewFiltersDialog
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
			this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.addButton = new System.Windows.Forms.Button();
			this.clearButton = new System.Windows.Forms.Button();
			this.removeButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// flowLayoutPanel
			// 
			this.flowLayoutPanel.AutoScroll = true;
			this.flowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.flowLayoutPanel.Location = new System.Drawing.Point(12, 12);
			this.flowLayoutPanel.Name = "flowLayoutPanel";
			this.flowLayoutPanel.Size = new System.Drawing.Size(262, 426);
			this.flowLayoutPanel.TabIndex = 0;
			// 
			// addButton
			// 
			this.addButton.Location = new System.Drawing.Point(281, 12);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(166, 41);
			this.addButton.TabIndex = 1;
			this.addButton.Text = "Add Curfew Filter";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// clearButton
			// 
			this.clearButton.Location = new System.Drawing.Point(281, 106);
			this.clearButton.Name = "clearButton";
			this.clearButton.Size = new System.Drawing.Size(166, 41);
			this.clearButton.TabIndex = 1;
			this.clearButton.Text = "Clear Curfew Filters";
			this.clearButton.UseVisualStyleBackColor = true;
			this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
			// 
			// removeButton
			// 
			this.removeButton.Location = new System.Drawing.Point(281, 59);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(166, 41);
			this.removeButton.TabIndex = 1;
			this.removeButton.Text = "Remove Last Curfew Filter";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(281, 414);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(81, 23);
			this.okButton.TabIndex = 2;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(368, 414);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(79, 23);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// CurfewFiltersDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(459, 450);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.clearButton);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.flowLayoutPanel);
			this.Name = "CurfewFiltersDialog";
			this.Text = "CurfewFiltersDialog";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button clearButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
	}
}