using RansacBot.Assemblies;
using RansacBot.HystoryTest;
using RansacBot.Trading;
using RansacBot.Trading.Filters;
using RansacBot.Trading.Hystory;
using RansacBot.Trading.Hystory.Infrastructure;
using RansacsRealTime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RansacBot.UI
{
	public partial class FlexibleHystoryTestForm : Form
	{
		DateTime startDateTime;
		List<Dataset> datasets = new();
		List<Control> adjustmentControls;
		RansacObservingParameters closingRansac;
		RansacObservingParameters filterRansac;
		RansacObservingParameters stopsRansac;

		CurfewFiltersDialog filtersDialog = new();
		List<(TimeSpan closingTime, TimeSpan openingTime)> filterTimes;
		
		public FlexibleHystoryTestForm()
		{
			InitializeComponent();
			adjustmentControls = new()
			{
				runButton,
				runFilesSeparatlyButton,
				addDatasetButton,
				addDatasetFromFolderButton,
				clearDatasetsButton,
				useFilterCheckbox,
				findOutputDirectoryButton, 
				closerRansacLevelUsageControl,
				filterRansacLevelUsageControl,
				stopsPlacingRansacLevelUsageControl
			};
		}

		private void addDatasetButton_Click(object sender, EventArgs e)
		{
			if (inputFileDialog.ShowDialog() != DialogResult.OK) return;
			//hystoryTicksFilePath.Text = inputFileDialog.FileName;
			List<string> names = inputFileDialog.FileNames.Select((s) => s.Substring(s.LastIndexOf('\\') + 1)).ToList();
			names.Sort(FileNameComparer);
			datasets.Add(new(
				inputFileDialog.FileName.Substring(0, inputFileDialog.FileName.LastIndexOf('\\') + 1),
				names));
			UpdateTreeView();
		}
		private void addDatasetFromFolderButton_Click(object sender, EventArgs e)
		{
			if (inputFolderBrowserDialog.ShowDialog() != DialogResult.OK) return;
			List<string> names = 
				Directory.GetFiles(inputFolderBrowserDialog.SelectedPath).
				Select((s) => s.Substring(s.LastIndexOf('\\') + 1)).ToList();
			if(!names.TrueForAll(s => s.EndsWith(".txt")))
			{
				GetChangingStatusTo("Error : there are files that doesn't end with .txt")();
				return;
			}
			try { names.Sort(FileNameComparer); }
			catch
			{
				GetChangingStatusTo("Error during files sort")();
				return;
			}
			datasets.Add(new(
				inputFolderBrowserDialog.SelectedPath + '\\',
				names));
			UpdateTreeView();
		}
		private void clearDatasetsButton_Click(object sender, EventArgs e)
		{
			datasets.Clear();
			UpdateTreeView();
		}
		private void UpdateTreeView()
		{
			inputFilesTreeView.Nodes.Clear();
			foreach(var dataset in datasets)
			{
				inputFilesTreeView.Nodes.Add(
					new TreeNode(
						dataset.path,
						dataset.fileNames.Select((s) => new TreeNode(s)).ToArray()
						));
			}
		}
		private void findOutputDirectoryButton_Click(object sender, EventArgs e)
		{
			if (outputFolderBrowserDialog.ShowDialog() != DialogResult.OK) return;
			outputDirectoryTextBox.Text = outputFolderBrowserDialog.SelectedPath;
			if (outputDirectoryTextBox.Text.Length > 0)
			{
				runButton.Enabled = true;
				runFilesSeparatlyButton.Enabled = true;
			}
		}


		private void runButton_Click(object sender, EventArgs e)
		{
			RunProcessingForAllDatasets(ProcessDataset);
		}
		private void runFilesSeparatlyButton_Click(object sender, EventArgs e)
		{
			RunProcessingForAllDatasets(ProcessSingleFilesFromDataset);
		}



		Task RunProcessingForAllDatasets(Action<Dataset> Process)
		{
			closingRansac = closerRansacLevelUsageControl.Parameters;
			filterRansac = filterRansacLevelUsageControl.Parameters;
			stopsRansac = stopsPlacingRansacLevelUsageControl.Parameters;
			this.Invoke((Action)BlockAllAdjustmentControls);
			return Task.Run(() =>
			{
				ProcessAllDatasets(Process);
			}).ContinueWith((task) => this.Invoke((Action)UnblockAllAdjustmentControls));
		}
		void ProcessAllDatasets(Action<Dataset> Process)
		{
			foreach(Dataset dataset in datasets)
			{
				Process(dataset);
			}
		}
		private void ProcessDataset(Dataset dataset)
		{
			this.Invoke(GetChangingStatusTo("Processing dataset " + dataset.DirName));
			DateTime currentTickTime = new();
			IEnumerable<string> unparsedTicks = dataset.FilesWithoutHeaders.Concat().Select(str =>
			{
				currentTickTime = new(Convert.ToInt64(str.Split(',')[1] + "0000000"));
				return str;
			});
			FinishedTradesFromUnparsedTicks processor = GetNewProcessor(unparsedTicks);
			GetProcessorReady(processor);
			startDateTime = DateTime.Now;
			string outputFileName = outputDirectoryTextBox.Text + '\\' + dataset.DirName + ".csv";
			processor.ProgressChanged += UpdateProgressBarFromProcessor;
			using (StreamWriter writer = new(outputFileName))
			{
				this.Invoke(GetChangingStatusTo("Building Trades..."));
				foreach(string trade in Process(processor))
				{
					writer.WriteLine(trade);
				}
				this.Invoke(GetChangingStatusTo("Trades builded!"));
			}
			processor.ProgressChanged -= UpdateProgressBarFromProcessor;
		}
		private void ProcessSingleFilesFromDataset(Dataset dataset)
		{
			this.Invoke(GetChangingStatusTo("Processing dataset " + dataset.DirName));
			string outputDirName = outputDirectoryTextBox.Text + '\\' + dataset.DirName + "_Deals";
			if (Directory.Exists(outputDirName) && Directory.GetFiles(outputDirName).Length > 0)
			{
				this.Invoke(
					GetChangingStatusTo(
						"Output folder " + dataset.DirName + " already exists. Please ensure there's no such folder and try again."));
				return;
			}
			Directory.CreateDirectory(outputDirName);
			while (!Directory.Exists(outputDirName)) ;
			foreach((string filename, IEnumerable<string> tickStrings) file in Enumerable.Zip(dataset.fileNames, dataset.FilesWithoutHeaders))
			{
				this.Invoke(GetChangingStatusTo("Processing file " + file.filename));
				string outputFileName = outputDirName + '\\' + file.filename.Replace(".txt", ".csv");
				FinishedTradesFromUnparsedTicks processor = GetNewProcessor(file.tickStrings);
				GetProcessorReady(processor);
				startDateTime = DateTime.Now;
				processor.ProgressChanged += UpdateProgressBarFromProcessor;
				using (StreamWriter writer = new(outputFileName))
				{
					this.Invoke(GetChangingStatusTo("Building Trades..."));
					foreach (string trade in Process(processor))
					{
						writer.WriteLine(trade);
					}
					this.Invoke(GetChangingStatusTo("Trades builded!"));
				}
				processor.ProgressChanged -= UpdateProgressBarFromProcessor;
			}
		}

		private FinishedTradesFromUnparsedTicks GetNewProcessor(IEnumerable<string> unparsedTicks)
		{
			ITickWithDateTimeParcer parcer = TicksWithDateTimeParser.DamirStandart;
			TicksDateTimeExtractor timeExtractor = new(unparsedTicks.Select(parcer.ParseTick));
			return new(timeExtractor, 
				new Closer_Filter_StopDecisionMaker(
					closingRansac, 
					filterRansac, 
					stopsRansac, 
					useFilterCheckbox.Checked,
					100,
					filterTimes.Select(times => 
						(Func<CurfewTimeTradeFilter>)
						(() => new CurfewTimeTradeFilter(times.closingTime, times.openingTime, timeExtractor.GetLastTickTime))
						)
					)
				);
		}
		private void GetProcessorReady(FinishedTradesFromUnparsedTicks processor)
		{
			this.Invoke(GetChangingStatusTo("Counting all ticks..."));
			processor.GetReady();
			this.Invoke(GetChangingStatusTo("Ticks counted"));
		}
		private IEnumerable<string> Process(FinishedTradesFromUnparsedTicks processor)
		{
			return processor.GetAllFinishedTrades().Select(trade => trade.ToString());
		}
		private void UpdateProgressBarFromProcessor(FinishedTradesFromUnparsedTicks finishedTrades)
		{
			this.Invoke(GetChangingProgress((int)finishedTrades.ProgressPromille));
			this.Invoke(new Action(() => 
			remainingTimeLabel.Text = "Approximate Remaining Time: " + 
			((DateTime.Now - startDateTime) * 
			((1000 - finishedTrades.ProgressPromille) / finishedTrades.ProgressPromille)).ToString(@"hh\:mm\:ss")));
		}

		private void BlockAllAdjustmentControls()
		{
			foreach (Control control in adjustmentControls)
			{
				control.Enabled = false;
			}
		}
		private void UnblockAllAdjustmentControls()
		{
			foreach (Control control in adjustmentControls)
			{
				control.Enabled = true;
			}
		}
		private Action GetChangingStatusTo(string status)
		{
			return () =>
			{
				string newStatus = DateTime.Now.ToString() + ' ' + status + '\n';
				statusRichTextBox.AppendText(newStatus);
				statusRichTextBox.ScrollToCaret();
			};
		}
		private Action GetChangingProgress(int progress)
		{
			return () => progressBar1.Value = progress;
		}
		
		int FileNameComparer(string s1, string s2)
		{
			int n1 = NumberOfFile(s1);
			int n2 = NumberOfFile(s2);
			return n1 > n2 ? 1 : n1 < n2 ? -1 : 0; 
		}
		int NumberOfFile(string s1)
		{
			return Convert.ToInt32(s1.Substring(0, s1.LastIndexOf('.')).Substring(s1.LastIndexOf('_') + 1));
		}
		class Dataset
		{
			public readonly string path;
			public readonly IEnumerable<string> fileNames;
			public IEnumerable<string> FullNames { get => fileNames.Select(filename => path + filename); }
			public IEnumerable<IEnumerable<string>> Files { get => FullNames.Select(File.ReadLines); }
			public IEnumerable<IEnumerable<string>> FilesWithoutHeaders { get => Files.Select(file => file.Skip(1)); }
			public string DirName { get => path.Split('\\', StringSplitOptions.RemoveEmptyEntries)[^1]; }
			public Dataset(string path, IEnumerable<string> fileNames)
			{
				this.path = path;
				this.fileNames = fileNames;
			}
		}

		private void curfewFiltersButton_Click(object sender, EventArgs e)
		{
			if (filtersDialog.ShowDialog() != DialogResult.OK) return;
			filterTimes = filtersDialog.AllFilterTimes;
		}
	}
}
