using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RansacBot.UI;
using RansacsRealTime;
using RansacBot.Trading;
using OxyPlot.Series;
using Accord.Math;
using Accord.Statistics.Distributions.Univariate;
using Accord.Statistics.Models.Regression.Linear;
using RansacBot.UI.Components;

namespace BotTests
{
	[TestClass]
	public class UITests
	{
		[TestMethod]
		public void RansacLevelUsageControlInit()
		{
			Assert.AreEqual(new RansacLevelUsageControl().SigmaType, Enum.GetValues<SigmaType>()[0]);
		}
	}
}
