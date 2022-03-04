
namespace RansacBot.HystoryTest
{
	partial class FlexibleHystoryTestForm
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
			this.inputFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.addDatasetButton = new System.Windows.Forms.Button();
			this.inputFileLabel = new System.Windows.Forms.Label();
			this.outputDirectoryTextBox = new System.Windows.Forms.TextBox();
			this.outputDirectoryLabel = new System.Windows.Forms.Label();
			this.findOutputDirectoryButton = new System.Windows.Forms.Button();
			this.runButton = new System.Windows.Forms.Button();
			this.outputFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.inputFilesTreeView = new System.Windows.Forms.TreeView();
			this.clearDatasetsButton = new System.Windows.Forms.Button();
			this.useFilterCheckbox = new System.Windows.Forms.CheckBox();
			this.statusRichTextBox = new System.Windows.Forms.RichTextBox();
			this.closerRansacLevelUsageControl = new RansacBot.UI.Components.RansacLevelUsageControl();
			this.remainingTimeLabel = new System.Windows.Forms.Label();
			this.filterRansacLevelUsageControl = new RansacBot.UI.Components.RansacLevelUsageControl();
			this.stopsPlacingRansacLevelUsageControl = new RansacBot.UI.Components.RansacLevelUsageControl();
			this.runFilesSeparatlyButton = new System.Windows.Forms.Button();
			this.addDatasetFromFolderButton = new System.Windows.Forms.Button();
			this.inputFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.SuspendLayout();
			// 
			// inputFileDialog
			// 
			this.inputFileDialog.FileName = "openFileDialog1";
			this.inputFileDialog.Filter = "txt files (*.txt)|*.txt|csv files (*.csv)|*.csv";
			this.inputFileDialog.Multiselect = true;
			this.inputFileDialog.Title = "Hystory Ticks File";
			// 
			// addDatasetButton
			// 
			this.addDatasetButton.Location = new System.Drawing.Point(477, 15);
			this.addDatasetButton.Name = "addDatasetButton";
			this.addDatasetButton.Size = new System.Drawing.Size(75, 23);
			this.addDatasetButton.TabIndex = 0;
			this.addDatasetButton.Text = "Add...";
			this.addDatasetButton.UseVisualStyleBackColor = true;
			this.addDatasetButton.Click += new System.EventHandler(this.addDatasetButton_Click);
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
			this.outputDirectoryTextBox.Location = new System.Drawing.Point(117, 118);
			this.outputDirectoryTextBox.Name = "outputDirectoryTextBox";
			this.outputDirectoryTextBox.ReadOnly = true;
			this.outputDirectoryTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.outputDirectoryTextBox.Size = new System.Drawing.Size(591, 23);
			this.outputDirectoryTextBox.TabIndex = 1;
			// 
			// outputDirectoryLabel
			// 
			this.outputDirectoryLabel.AutoSize = true;
			this.outputDirectoryLabel.Location = new System.Drawing.Point(12, 121);
			this.outputDirectoryLabel.Name = "outputDirectoryLabel";
			this.outputDirectoryLabel.Size = new System.Drawing.Size(96, 15);
			this.outputDirectoryLabel.TabIndex = 2;
			this.outputDirectoryLabel.Text = "Output Directory";
			// 
			// findOutputDirectoryButton
			// 
			this.findOutputDirectoryButton.Location = new System.Drawing.Point(714, 118);
			this.findOutputDirectoryButton.Name = "findOutputDirectoryButton";
			this.findOutputDirectoryButton.Size = new System.Drawing.Size(75, 23);
			this.findOutputDirectoryButton.TabIndex = 0;
			this.findOutputDirectoryButton.Text = "Find...";
			this.findOutputDirectoryButton.UseVisualStyleBackColor = true;
			this.findOutputDirectoryButton.Click += new System.EventHandler(this.findOutputDirectoryButton_Click);
			// 
			// runButton
			// 
			this.runButton.Enabled = false;
			this.runButton.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.runButton.Location = new System.Drawing.Point(12, 332);
			this.runButton.Name = "runButton";
			this.runButton.Size = new System.Drawing.Size(136, 67);
			this.runButton.TabIndex = 6;
			this.runButton.Text = "Run";
			this.runButton.UseVisualStyleBackColor = true;
			this.runButton.Click += new System.EventHandler(this.runButton_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(12, 415);
			this.progressBar1.Maximum = 1000;
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(776, 23);
			this.progressBar1.TabIndex = 7;
			// 
			// inputFilesTreeView
			// 
			this.inputFilesTreeView.Cursor = System.Windows.Forms.Cursors.Default;
			this.inputFilesTreeView.Location = new System.Drawing.Point(117, 15);
			this.inputFilesTreeView.Name = "inputFilesTreeView";
			this.inputFilesTreeView.Size = new System.Drawing.Size(354, 97);
			this.inputFilesTreeView.TabIndex = 8;
			// 
			// clearDatasetsButton
			// 
			this.clearDatasetsButton.Location = new System.Drawing.Point(477, 44);
			this.clearDatasetsButton.Name = "clearDatasetsButton";
			this.clearDatasetsButton.Size = new System.Drawing.Size(75, 23);
			this.clearDatasetsButton.TabIndex = 0;
			this.clearDatasetsButton.Text = "Clear";
			this.clearDatasetsButton.UseVisualStyleBackColor = true;
			this.clearDatasetsButton.Click += new System.EventHandler(this.clearDatasetsButton_Click);
			// 
			// useFilterCheckbox
			// 
			this.useFilterCheckbox.AutoSize = true;
			this.useFilterCheckbox.Checked = true;
			this.useFilterCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.useFilterCheckbox.Location = new System.Drawing.Point(307, 209);
			this.useFilterCheckbox.Name = "useFilterCheckbox";
			this.useFilterCheckbox.Size = new System.Drawing.Size(164, 19);
			this.useFilterCheckbox.TabIndex = 9;
			this.useFilterCheckbox.Text = "Use ET4 higher-lower filter";
			this.useFilterCheckbox.UseVisualStyleBackColor = true;
			// 
			// statusRichTextBox
			// 
			this.statusRichTextBox.Location = new System.Drawing.Point(477, 147);
			this.statusRichTextBox.Name = "statusRichTextBox";
			this.statusRichTextBox.Size = new System.Drawing.Size(311, 234);
			this.statusRichTextBox.TabIndex = 10;
			this.statusRichTextBox.Text = "";
			// 
			// closerRansacLevelUsageControl
			// 
			this.closerRansacLevelUsageControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.closerRansacLevelUsageControl.LabelText = "Closing Ransac";
			this.closerRansacLevelUsageControl.Level = 2;
			this.closerRansacLevelUsageControl.Location = new System.Drawing.Point(13, 147);
			this.closerRansacLevelUsageControl.Name = "closerRansacLevelUsageControl";
			this.closerRansacLevelUsageControl.SigmaType = RansacsRealTime.SigmaType.Sigma;
			this.closerRansacLevelUsageControl.Size = new System.Drawing.Size(289, 42);
			this.closerRansacLevelUsageControl.TabIndex = 11;
			// 
			// remainingTimeLabel
			// 
			this.remainingTimeLabel.AutoSize = true;
			this.remainingTimeLabel.Location = new System.Drawing.Point(475, 384);
			this.remainingTimeLabel.Name = "remainingTimeLabel";
			this.remainingTimeLabel.Size = new System.Drawing.Size(10, 15);
			this.remainingTimeLabel.TabIndex = 13;
			this.remainingTimeLabel.Text = "|";
			// 
			// filterRansacLevelUsageControl
			// 
			this.filterRansacLevelUsageControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.filterRansacLevelUsageControl.LabelText = "Filter Ransac";
			this.filterRansacLevelUsageControl.Level = 4;
			this.filterRansacLevelUsageControl.Location = new System.Drawing.Point(12, 195);
			this.filterRansacLevelUsageControl.Name = "filterRansacLevelUsageControl";
			this.filterRansacLevelUsageControl.SigmaType = RansacsRealTime.SigmaType.ErrorThreshold;
			this.filterRansacLevelUsageControl.Size = new System.Drawing.Size(289, 42);
			this.filterRansacLevelUsageControl.TabIndex = 11;
			// 
			// stopsPlacingRansacLevelUsageControl
			// 
			this.stopsPlacingRansacLevelUsageControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.stopsPlacingRansacLevelUsageControl.LabelText = "Stops Ransac";
			this.stopsPlacingRansacLevelUsageControl.Level = 2;
			this.stopsPlacingRansacLevelUsageControl.Location = new System.Drawing.Point(12, 243);
			this.stopsPlacingRansacLevelUsageControl.Name = "stopsPlacingRansacLevelUsageControl";
			this.stopsPlacingRansacLevelUsageControl.SigmaType = RansacsRealTime.SigmaType.Sigma;
			this.stopsPlacingRansacLevelUsageControl.Size = new System.Drawing.Size(289, 42);
			this.stopsPlacingRansacLevelUsageControl.TabIndex = 11;
			// 
			// runFilesSeparatlyButton
			// 
			this.runFilesSeparatlyButton.Enabled = false;
			this.runFilesSeparatlyButton.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.runFilesSeparatlyButton.Location = new System.Drawing.Point(154, 332);
			this.runFilesSeparatlyButton.Name = "runFilesSeparatlyButton";
			this.runFilesSeparatlyButton.Size = new System.Drawing.Size(315, 67);
			this.runFilesSeparatlyButton.TabIndex = 6;
			this.runFilesSeparatlyButton.Text = "Run Files Separatly";
			this.runFilesSeparatlyButton.UseVisualStyleBackColor = true;
			this.runFilesSeparatlyButton.Click += new System.EventHandler(this.runFilesSeparatlyButton_Click);
			// 
			// addDatasetFromFolderButton
			// 
			this.addDatasetFromFolderButton.Location = new System.Drawing.Point(558, 15);
			this.addDatasetFromFolderButton.Name = "addDatasetFromFolderButton";
			this.addDatasetFromFolderButton.Size = new System.Drawing.Size(112, 23);
			this.addDatasetFromFolderButton.TabIndex = 0;
			this.addDatasetFromFolderButton.Text = "Add folder...";
			this.addDatasetFromFolderButton.UseVisualStyleBackColor = true;
			this.addDatasetFromFolderButton.Click += new System.EventHandler(this.addDatasetFromFolderButton_Click);
			// 
			// FlexibleHystoryTestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.remainingTimeLabel);
			this.Controls.Add(this.stopsPlacingRansacLevelUsageControl);
			this.Controls.Add(this.filterRansacLevelUsageControl);
			this.Controls.Add(this.closerRansacLevelUsageControl);
			this.Controls.Add(this.statusRichTextBox);
			this.Controls.Add(this.useFilterCheckbox);
			this.Controls.Add(this.inputFilesTreeView);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.runFilesSeparatlyButton);
			this.Controls.Add(this.runButton);
			this.Controls.Add(this.outputDirectoryLabel);
			this.Controls.Add(this.inputFileLabel);
			this.Controls.Add(this.outputDirectoryTextBox);
			this.Controls.Add(this.findOutputDirectoryButton);
			this.Controls.Add(this.clearDatasetsButton);
			this.Controls.Add(this.addDatasetFromFolderButton);
			this.Controls.Add(this.addDatasetButton);
			this.Name = "FlexibleHystoryTestForm";
			this.Text = "Bot Hystory Test";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog inputFileDialog;
		private System.Windows.Forms.Button addDatasetButton;
		private System.Windows.Forms.Label inputFileLabel;
		private System.Windows.Forms.TextBox outputDirectoryTextBox;
		private System.Windows.Forms.Label outputDirectoryLabel;
		private System.Windows.Forms.Button findOutputDirectoryButton;
		private System.Windows.Forms.Button runButton;
		private System.Windows.Forms.FolderBrowserDialog outputFolderBrowserDialog;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.TreeView inputFilesTreeView;
		private System.Windows.Forms.Button clearDatasetsButton;
		private System.Windows.Forms.CheckBox useFilterCheckbox;
		private System.Windows.Forms.RichTextBox statusRichTextBox;
		private UI.Components.RansacLevelUsageControl closerRansacLevelUsageControl;
		private System.Windows.Forms.Label remainingTimeLabel;
		private UI.Components.RansacLevelUsageControl filterRansacLevelUsageControl;
		private UI.Components.RansacLevelUsageControl stopsPlacingRansacLevelUsageControl;
		private System.Windows.Forms.Button runFilesSeparatlyButton;
		private System.Windows.Forms.Button addDatasetFromFolderButton;
		private System.Windows.Forms.FolderBrowserDialog inputFolderBrowserDialog;
	}
}