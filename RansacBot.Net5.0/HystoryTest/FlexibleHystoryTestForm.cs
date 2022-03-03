using RansacBot.Assemblies;
using RansacBot.Trading;
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

namespace RansacBot.HystoryTest
{
	public partial class FlexibleHystoryTestForm : Form
	{
		DateTime startDateTime;
		List<Dataset> datasets = new();
		public FlexibleHystoryTestForm()
		{
			InitializeComponent();
		}

		private void chooseHystoryFilePathButton_Click(object sender, EventArgs e)
		{
			inputFileDialog.ShowDialog();
			//hystoryTicksFilePath.Text = inputFileDialog.FileName;
			List<string> names = inputFileDialog.FileNames.Select((s) => s.Substring(s.LastIndexOf('\\') + 1)).ToList();
			names.Sort(FileNameComparer);
			datasets.Add(new(
				inputFileDialog.FileName.Substring(0, inputFileDialog.FileName.LastIndexOf('\\') + 1),
				names));
			UpdateTreeView();
		}
		private void clearInputFilesButton_Click(object sender, EventArgs e)
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
		private void chooseOutputDirectoryButton_Click(object sender, EventArgs e)
		{
			outputFolderBrowserDialog.ShowDialog();
			outputDirectoryTextBox.Text = outputFolderBrowserDialog.SelectedPath;
			if(outputDirectoryTextBox.Text.Length > 0) runButton.Enabled = true;
		}


		//private Dictionary<SigmaType, int> GetEnoughRansacsCascades()
		//{
		//	List<RansacLevelUsageControl> usedRansacLevels = new()
		//	{
		//		//stopPlacerRansacLevelUsageControl,
		//		//higherLowerFilterRansacLevelUsageControl,
		//		//closingRansacLevelUsageControl
		//	};
		//	return GetEnoughRansacsCascades(usedRansacLevels);
		//}
		//private Dictionary<SigmaType, int> GetEnoughRansacsCascades(
		//	List<RansacLevelUsageControl> usedRansacLevels)
		//{
		//	Dictionary<SigmaType, int> levels = new();
		//	foreach(RansacLevelUsageControl control in usedRansacLevels)
		//	{
		//		if (levels.ContainsKey(control.SigmaType))
		//		{
		//			if(control.Level > levels[control.SigmaType])
		//			{
		//				levels[control.SigmaType] = control.Level;
		//			}
		//		}
		//		else
		//		{
		//			levels.Add(control.SigmaType, control.Level);
		//		}
		//	}
		//	return levels;
		//}


		private void runButton_Click(object sender, EventArgs e)
		{
			this.Invoke((Action)(() => runButton.Enabled = false));
			Task.Run(() =>
			{
				foreach (var dataset in datasets)
				{
					ProcessDataset(dataset);
				}
			}).ContinueWith((task) => this.Invoke((Action)(() => runButton.Enabled = true)));
		}

		private void ProcessDataset(Dataset dataset)
		{
			this.Invoke(GetChangingStatusTo("Processing dataset " + dataset.DirName));
			IEnumerable<string> unparsedTicks = dataset.FilesWithoutHeaders.Concat();
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

		private FinishedTradesFromUnparsedTicks GetNewProcessor(IEnumerable<string> unparsedTicks)
		{
			return new(useFilterCheckbox.Checked, unparsedTicks, TicksParser.DamirStandart);
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

		private Action GetChangingStatusTo(string status)
		{
			return () => 
			statusRichTextBox.Lines =
			statusRichTextBox.Lines.Append(DateTime.Now.ToString() + ' ' + status).ToArray();
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
		public class Dataset
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
	}
}
