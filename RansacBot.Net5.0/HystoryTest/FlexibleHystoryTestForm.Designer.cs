
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
			this.inputFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.chooseHystoryFilePathButton = new System.Windows.Forms.Button();
			this.hystoryTicksFilePath = new System.Windows.Forms.TextBox();
			this.inputFileLabel = new System.Windows.Forms.Label();
			this.outputDirectoryTextBox = new System.Windows.Forms.TextBox();
			this.outputDirectoryLabel = new System.Windows.Forms.Label();
			this.chooseOutputDirectoryButton = new System.Windows.Forms.Button();
			this.deciderLabel = new System.Windows.Forms.Label();
			this.ticksLabel = new System.Windows.Forms.Label();
			this.fileLabel = new System.Windows.Forms.Label();
			this.monkeyNLabel = new System.Windows.Forms.Label();
			this.vertexesLabel = new System.Windows.Forms.Label();
			this.tradesLabel = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.stopPlacerRansacLevelUsageControl = new RansacBot.UI.Components.RansacLevelUsageControl();
			this.higherLowerFilterRansacLevelUsageControl = new RansacBot.UI.Components.RansacLevelUsageControl();
			this.label5 = new System.Windows.Forms.Label();
			this.closingRansacLevelUsageControl = new RansacBot.UI.Components.RansacLevelUsageControl();
			this.runButton = new System.Windows.Forms.Button();
			this.outputFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// inputFileDialog
			// 
			this.inputFileDialog.FileName = "openFileDialog1";
			this.inputFileDialog.Filter = "txt files (*.txt)|*.txt|csv files (*.csv)|*.csv";
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
			// hystoryTicksFilePath
			// 
			this.hystoryTicksFilePath.Location = new System.Drawing.Point(116, 12);
			this.hystoryTicksFilePath.Name = "hystoryTicksFilePath";
			this.hystoryTicksFilePath.ReadOnly = true;
			this.hystoryTicksFilePath.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.hystoryTicksFilePath.Size = new System.Drawing.Size(591, 23);
			this.hystoryTicksFilePath.TabIndex = 1;
			this.hystoryTicksFilePath.Text = "C:\\Users\\ir2\\Desktop\\1.txt";
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
			this.outputDirectoryTextBox.Location = new System.Drawing.Point(116, 41);
			this.outputDirectoryTextBox.Name = "outputDirectoryTextBox";
			this.outputDirectoryTextBox.ReadOnly = true;
			this.outputDirectoryTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.outputDirectoryTextBox.Size = new System.Drawing.Size(591, 23);
			this.outputDirectoryTextBox.TabIndex = 1;
			// 
			// outputDirectoryLabel
			// 
			this.outputDirectoryLabel.AutoSize = true;
			this.outputDirectoryLabel.Location = new System.Drawing.Point(12, 44);
			this.outputDirectoryLabel.Name = "outputDirectoryLabel";
			this.outputDirectoryLabel.Size = new System.Drawing.Size(96, 15);
			this.outputDirectoryLabel.TabIndex = 2;
			this.outputDirectoryLabel.Text = "Output Directory";
			// 
			// chooseOutputDirectoryButton
			// 
			this.chooseOutputDirectoryButton.Location = new System.Drawing.Point(713, 41);
			this.chooseOutputDirectoryButton.Name = "chooseOutputDirectoryButton";
			this.chooseOutputDirectoryButton.Size = new System.Drawing.Size(75, 23);
			this.chooseOutputDirectoryButton.TabIndex = 0;
			this.chooseOutputDirectoryButton.Text = "Find...";
			this.chooseOutputDirectoryButton.UseVisualStyleBackColor = true;
			this.chooseOutputDirectoryButton.Click += new System.EventHandler(this.chooseOutputDirectoryButton_Click);
			// 
			// deciderLabel
			// 
			this.deciderLabel.AutoSize = true;
			this.deciderLabel.Location = new System.Drawing.Point(41, 149);
			this.deciderLabel.Name = "deciderLabel";
			this.deciderLabel.Size = new System.Drawing.Size(99, 15);
			this.deciderLabel.TabIndex = 3;
			this.deciderLabel.Text = "InvertedNDecider";
			// 
			// ticksLabel
			// 
			this.ticksLabel.AutoSize = true;
			this.ticksLabel.Location = new System.Drawing.Point(106, 100);
			this.ticksLabel.Name = "ticksLabel";
			this.ticksLabel.Size = new System.Drawing.Size(33, 15);
			this.ticksLabel.TabIndex = 3;
			this.ticksLabel.Text = "Ticks";
			// 
			// fileLabel
			// 
			this.fileLabel.AutoSize = true;
			this.fileLabel.Location = new System.Drawing.Point(41, 85);
			this.fileLabel.Name = "fileLabel";
			this.fileLabel.Size = new System.Drawing.Size(25, 15);
			this.fileLabel.TabIndex = 3;
			this.fileLabel.Text = "File";
			// 
			// monkeyNLabel
			// 
			this.monkeyNLabel.AutoSize = true;
			this.monkeyNLabel.Location = new System.Drawing.Point(41, 115);
			this.monkeyNLabel.Name = "monkeyNLabel";
			this.monkeyNLabel.Size = new System.Drawing.Size(59, 15);
			this.monkeyNLabel.TabIndex = 3;
			this.monkeyNLabel.Text = "MonkeyN";
			// 
			// vertexesLabel
			// 
			this.vertexesLabel.AutoSize = true;
			this.vertexesLabel.Location = new System.Drawing.Point(106, 134);
			this.vertexesLabel.Name = "vertexesLabel";
			this.vertexesLabel.Size = new System.Drawing.Size(50, 15);
			this.vertexesLabel.TabIndex = 3;
			this.vertexesLabel.Text = "Vertexes";
			// 
			// tradesLabel
			// 
			this.tradesLabel.AutoSize = true;
			this.tradesLabel.Location = new System.Drawing.Point(137, 164);
			this.tradesLabel.Name = "tradesLabel";
			this.tradesLabel.Size = new System.Drawing.Size(40, 15);
			this.tradesLabel.TabIndex = 3;
			this.tradesLabel.Text = "Trades";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(41, 238);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(109, 15);
			this.label2.TabIndex = 3;
			this.label2.Text = "Higher-Lower Filter";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(41, 188);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(66, 15);
			this.label3.TabIndex = 3;
			this.label3.Text = "Stop Placer";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(137, 214);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(97, 15);
			this.label4.TabIndex = 3;
			this.label4.Text = "Trades with stops";
			// 
			// stopPlacerRansacLevelUsageControl
			// 
			this.stopPlacerRansacLevelUsageControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.stopPlacerRansacLevelUsageControl.Level = 2;
			this.stopPlacerRansacLevelUsageControl.Location = new System.Drawing.Point(156, 182);
			this.stopPlacerRansacLevelUsageControl.MaximumSize = new System.Drawing.Size(292, 29);
			this.stopPlacerRansacLevelUsageControl.MinimumSize = new System.Drawing.Size(292, 29);
			this.stopPlacerRansacLevelUsageControl.Name = "stopPlacerRansacLevelUsageControl";
			this.stopPlacerRansacLevelUsageControl.SigmaType = RansacsRealTime.SigmaType.Sigma;
			this.stopPlacerRansacLevelUsageControl.Size = new System.Drawing.Size(292, 29);
			this.stopPlacerRansacLevelUsageControl.TabIndex = 5;
			// 
			// higherLowerFilterRansacLevelUsageControl
			// 
			this.higherLowerFilterRansacLevelUsageControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.higherLowerFilterRansacLevelUsageControl.Level = 4;
			this.higherLowerFilterRansacLevelUsageControl.Location = new System.Drawing.Point(156, 232);
			this.higherLowerFilterRansacLevelUsageControl.MaximumSize = new System.Drawing.Size(292, 29);
			this.higherLowerFilterRansacLevelUsageControl.MinimumSize = new System.Drawing.Size(292, 29);
			this.higherLowerFilterRansacLevelUsageControl.Name = "higherLowerFilterRansacLevelUsageControl";
			this.higherLowerFilterRansacLevelUsageControl.SigmaType = RansacsRealTime.SigmaType.ErrorThreshold;
			this.higherLowerFilterRansacLevelUsageControl.Size = new System.Drawing.Size(292, 29);
			this.higherLowerFilterRansacLevelUsageControl.TabIndex = 5;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(41, 273);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(47, 15);
			this.label5.TabIndex = 3;
			this.label5.Text = "Closing";
			// 
			// closingRansacLevelUsageControl
			// 
			this.closingRansacLevelUsageControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.closingRansacLevelUsageControl.Level = 2;
			this.closingRansacLevelUsageControl.Location = new System.Drawing.Point(156, 267);
			this.closingRansacLevelUsageControl.MaximumSize = new System.Drawing.Size(292, 29);
			this.closingRansacLevelUsageControl.MinimumSize = new System.Drawing.Size(292, 29);
			this.closingRansacLevelUsageControl.Name = "closingRansacLevelUsageControl";
			this.closingRansacLevelUsageControl.SigmaType = RansacsRealTime.SigmaType.Sigma;
			this.closingRansacLevelUsageControl.Size = new System.Drawing.Size(292, 29);
			this.closingRansacLevelUsageControl.TabIndex = 5;
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
			// FlexibleHystoryRunForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.runButton);
			this.Controls.Add(this.closingRansacLevelUsageControl);
			this.Controls.Add(this.higherLowerFilterRansacLevelUsageControl);
			this.Controls.Add(this.stopPlacerRansacLevelUsageControl);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.tradesLabel);
			this.Controls.Add(this.vertexesLabel);
			this.Controls.Add(this.ticksLabel);
			this.Controls.Add(this.monkeyNLabel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.fileLabel);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.deciderLabel);
			this.Controls.Add(this.outputDirectoryLabel);
			this.Controls.Add(this.inputFileLabel);
			this.Controls.Add(this.outputDirectoryTextBox);
			this.Controls.Add(this.hystoryTicksFilePath);
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
		private System.Windows.Forms.TextBox hystoryTicksFilePath;
		private System.Windows.Forms.Label inputFileLabel;
		private System.Windows.Forms.TextBox outputDirectoryTextBox;
		private System.Windows.Forms.Label outputDirectoryLabel;
		private System.Windows.Forms.Button chooseOutputDirectoryButton;
		private System.Windows.Forms.Label deciderLabel;
		private System.Windows.Forms.Label ticksLabel;
		private System.Windows.Forms.Label fileLabel;
		private System.Windows.Forms.Label monkeyNLabel;
		private System.Windows.Forms.Label vertexesLabel;
		private System.Windows.Forms.Label tradesLabel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private UI.Components.RansacLevelUsageControl stopPlacerRansacLevelUsageControl;
		private UI.Components.RansacLevelUsageControl higherLowerFilterRansacLevelUsageControl;
		private System.Windows.Forms.Label label5;
		private UI.Components.RansacLevelUsageControl closingRansacLevelUsageControl;
		private System.Windows.Forms.Button runButton;
		private System.Windows.Forms.FolderBrowserDialog outputFolderBrowserDialog;
		private System.Windows.Forms.ProgressBar progressBar1;
	}
}