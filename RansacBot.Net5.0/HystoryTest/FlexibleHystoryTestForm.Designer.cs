
namespace RansacBot.HystoryTest
{
	partial class FlexibleHystoryRunForm
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
			System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node0");
			this.inputFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.chooseHystoryFilePathButton = new System.Windows.Forms.Button();
			this.inputFileLabel = new System.Windows.Forms.Label();
			this.outputDirectoryTextBox = new System.Windows.Forms.TextBox();
			this.outputDirectoryLabel = new System.Windows.Forms.Label();
			this.chooseOutputDirectoryButton = new System.Windows.Forms.Button();
			this.runButton = new System.Windows.Forms.Button();
			this.outputFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// inputFileDialog
			// 
			this.inputFileDialog.FileName = "openFileDialog1";
			this.inputFileDialog.Filter = "txt files (*.txt)|*.txt|csv files (*.csv)|*.csv";
			this.inputFileDialog.Multiselect = true;
			this.inputFileDialog.Title = "Hystory Ticks File";
			// 
			// chooseHystoryFilePathButton
			// 
			this.chooseHystoryFilePathButton.Location = new System.Drawing.Point(713, 12);
			this.chooseHystoryFilePathButton.Name = "chooseHystoryFilePathButton";
			this.chooseHystoryFilePathButton.Size = new System.Drawing.Size(75, 23);
			this.chooseHystoryFilePathButton.TabIndex = 0;
			this.chooseHystoryFilePathButton.Text = "Find...";
			this.chooseHystoryFilePathButton.UseVisualStyleBackColor = true;
			this.chooseHystoryFilePathButton.Click += new System.EventHandler(this.chooseHystoryFilePathButton_Click);
			// 
			// inputFileLabel
			// 
			this.inputFileLabel.AutoSize = true;
			this.inputFileLabel.Location = new System.Drawing.Point(12, 15);
			this.inputFileLabel.Name = "inputFileLabel";
			this.inputFileLabel.Size = new System.Drawing.Size(98, 15);
			this.inputFileLabel.TabIndex = 2;
			this.inputFileLabel.Text = "Hystory Ticks File";
			// 
			// outputDirectoryTextBox
			// 
			this.outputDirectoryTextBox.Location = new System.Drawing.Point(116, 159);
			this.outputDirectoryTextBox.Name = "outputDirectoryTextBox";
			this.outputDirectoryTextBox.ReadOnly = true;
			this.outputDirectoryTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.outputDirectoryTextBox.Size = new System.Drawing.Size(591, 23);
			this.outputDirectoryTextBox.TabIndex = 1;
			// 
			// outputDirectoryLabel
			// 
			this.outputDirectoryLabel.AutoSize = true;
			this.outputDirectoryLabel.Location = new System.Drawing.Point(12, 162);
			this.outputDirectoryLabel.Name = "outputDirectoryLabel";
			this.outputDirectoryLabel.Size = new System.Drawing.Size(96, 15);
			this.outputDirectoryLabel.TabIndex = 2;
			this.outputDirectoryLabel.Text = "Output Directory";
			// 
			// chooseOutputDirectoryButton
			// 
			this.chooseOutputDirectoryButton.Location = new System.Drawing.Point(713, 158);
			this.chooseOutputDirectoryButton.Name = "chooseOutputDirectoryButton";
			this.chooseOutputDirectoryButton.Size = new System.Drawing.Size(75, 23);
			this.chooseOutputDirectoryButton.TabIndex = 0;
			this.chooseOutputDirectoryButton.Text = "Find...";
			this.chooseOutputDirectoryButton.UseVisualStyleBackColor = true;
			this.chooseOutputDirectoryButton.Click += new System.EventHandler(this.chooseOutputDirectoryButton_Click);
			// 
			// runButton
			// 
			this.runButton.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.runButton.Location = new System.Drawing.Point(41, 335);
			this.runButton.Name = "runButton";
			this.runButton.Size = new System.Drawing.Size(136, 67);
			this.runButton.TabIndex = 6;
			this.runButton.Text = "Run";
			this.runButton.UseVisualStyleBackColor = true;
			this.runButton.Click += new System.EventHandler(this.runButton_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(41, 415);
			this.progressBar1.Maximum = 2153;
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(747, 23);
			this.progressBar1.TabIndex = 7;
			// 
			// treeView1
			// 
			this.treeView1.Location = new System.Drawing.Point(117, 13);
			this.treeView1.Name = "treeView1";
			treeNode1.ForeColor = System.Drawing.Color.Red;
			treeNode1.Name = "Node0";
			treeNode1.Text = "Node0";
			this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
			this.treeView1.Size = new System.Drawing.Size(121, 97);
			this.treeView1.TabIndex = 8;
			// 
			// FlexibleHystoryRunForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.treeView1);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.runButton);
			this.Controls.Add(this.outputDirectoryLabel);
			this.Controls.Add(this.inputFileLabel);
			this.Controls.Add(this.outputDirectoryTextBox);
			this.Controls.Add(this.chooseOutputDirectoryButton);
			this.Controls.Add(this.chooseHystoryFilePathButton);
			this.Name = "FlexibleHystoryRunForm";
			this.Text = "FlexibleHystoryTest";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog inputFileDialog;
		private System.Windows.Forms.Button chooseHystoryFilePathButton;
		private System.Windows.Forms.Label inputFileLabel;
		private System.Windows.Forms.TextBox outputDirectoryTextBox;
		private System.Windows.Forms.Label outputDirectoryLabel;
		private System.Windows.Forms.Button chooseOutputDirectoryButton;
		private System.Windows.Forms.Button runButton;
		private System.Windows.Forms.FolderBrowserDialog outputFolderBrowserDialog;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.TreeView treeView1;
	}
}