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
			Task.Run(() =>
			{
				this.Invoke((Action)(() => runButton.Enabled = false));
				foreach (var dataset in datasets)
				{
					ProcessDataset(dataset);
				}
				this.Invoke((Action)(() => runButton.Enabled = true));
			});
		}

		private void ProcessDataset(Dataset dataset)
		{
			this.Invoke(GetChangingStatusTo("Processing dataset " + dataset.DirName));
			IEnumerable<string> unparsedTicks = dataset.FilesWithoutHeaders.Concat();
			HystoryFromFileProcessor processor = GetProcessor(unparsedTicks);
			GetProcessorReady(processor);
			using StreamWriter writer = new(outputDirectoryTextBox.Text + '\\' + dataset.DirName + ".csv");
			Process(processor, writer.WriteLine);
		}

		private HystoryFromFileProcessor GetProcessor(IEnumerable<string> unparsedTicks)
		{
			return new(useFilterCheckbox.Checked, unparsedTicks, TicksParser.DamirStandart);
		}
		private void GetProcessorReady(HystoryFromFileProcessor processor)
		{
			this.Invoke(GetChangingStatusTo("Counting all ticks..."));
			processor.GetReady();
			this.Invoke(GetChangingStatusTo("Ticks counted"));
		}
		private void Process(HystoryFromFileProcessor processor, Action<string> output)
		{
			this.Invoke(GetChangingStatusTo("Building Trades..."));
			processor.ProgressChanged +=
			(processor) => this.Invoke(GetChangingProgress((int)processor.ProgressPromille));
			processor.ProcessFiles(output);
			this.Invoke(GetChangingStatusTo("Trades builded!"));
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
		

		private void ProcessFiles(string path, List<string> names, string outputFile, int period)
		{
			S2_ET_S2_DecisionMaker decisionMaker =
				new S2_ET_S2_DecisionMaker(useFilterCheckbox.Checked);

			HystoryTradingModule tradingModule =
				new(
					decisionMaker.TradeWithStopProvider,
					decisionMaker.ClosingProvider);


			FinishedTradesBuilder finishedTradesBuilder = new();
			tradingModule.TradeExecuted += finishedTradesBuilder.OnTradeOpend;
			tradingModule.TradeClosedOnPrice += finishedTradesBuilder.OnTradeClosedOnPrice;
			tradingModule.StopExecutedOnPrice += finishedTradesBuilder.OnTradeClosedOnPrice;


			StreamWriter writer = new(outputFile);

			finishedTradesBuilder.NewTradeFinished += (finishedTrade) =>
			{
				writer.WriteLine(finishedTrade.ToString());
			};


			HystoryQuikSimulator quikSimulator = HystoryQuikSimulator.Instance;
			finishedTradesBuilder.NewTick += quikSimulator.OnNewTick;
			quikSimulator.NewTick += decisionMaker.OnNewTick;

			names.Sort(FileNameComparer);
			IEnumerable<Tick> allTicks =
				new TicksLazySequentialParser(
					names.
					Select((name) => path + name).
					Select((fileName) => File.ReadLines(fileName).Skip(1)).
					Concat(), 
					TicksParser.DamirStandart);
			int numberOfTicks = allTicks.Count();
			this.Invoke((Action)(() => progressBar1.Maximum = numberOfTicks / period));
			this.Invoke((Action)(() => progressBar1.Value = 0));

			Task.Run(() =>
			{
				int count = 0;
				foreach (Tick tick in allTicks)
				{
					finishedTradesBuilder.OnNewTick(tick);
					if (count > period)
					{
						count = 0;
					}
					count++;
				}
				writer.Dispose();
			});
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
