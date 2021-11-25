using Accord.Math;
using Accord.Statistics.Distributions.Univariate;
using Accord.Statistics.Models.Regression.Linear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RansacRealTime
{
	/// <summary>
	/// Класс предоставляет 1 единственный функционал (Compute) - выдает построенный ранзак с указанными параметрами.
	/// </summary>
	public static class RansacComputator
	{
		private static (int count, SimpleLinearRegression reg)[] Best;
		private static int MinSamples;
		private static double[] x;
		private static double[] y;
		private static readonly object locker = new();


		public static void Compute(List<Tick> ticks, TypeSigma typeSigma, double percentile,
			out SimpleLinearRegression bestReg, out double errorThreshold, out double sigma)
		{
			x = ticks.Select(n => (double)n.VERTEXINDEX).ToArray();
			y = ticks.Select(n => (double)n.PRICE).ToArray();

			int MaxEvaluations = ticks.Count > 100 ? 100 : ticks.Count;
			MinSamples = (int)(0.3 * ticks.Count) > 2 ? (int)(0.3 * ticks.Count) : 2;
			Best = new (int count, SimpleLinearRegression reg)[MaxEvaluations];

			double median = GetMedian(y);
			double error = GetErrorThreshold(y, median);
			errorThreshold = error;

			ParallelLoopResult result = Parallel.For(0, MaxEvaluations, new ParallelOptions { MaxDegreeOfParallelism = 4 }, Iteration);

			if (result.LowestBreakIteration == null)
			{
				bestReg = Array.Find(Best, x => x.count == Best.Max(x => x.count)).reg;
			}
			else
			{
				bestReg = Best[Convert.ToInt32(result.LowestBreakIteration.ToString())].reg;
			}

			double slope = bestReg.Slope;
			double intercept = bestReg.Intercept;

			sigma = typeSigma switch
			{
				TypeSigma.Sigma => Math.Sqrt(y.Zip(x.Select(x => x * slope + intercept), (a, b) => Math.Pow(a - b, 2)).Sum() / ticks.Count),
				TypeSigma.SigmaInliers => Math.Sqrt(
					y.Select(y => Math.Abs(y - median) <= error ? y : 0).Zip(x.Select(x => x * slope + intercept),
					(a, b) => a == 0 ? 0 : Math.Pow(a - b, 2)).Sum() / y.Count(y => Math.Abs(y - median) <= error)),
				TypeSigma.СonfidenceInterval => GetPercentileSigma(ticks, bestReg.Slope, bestReg.Intercept, error, percentile / 100.0),
				_ => error,
			};
		}

		/// <summary>
		/// Итерация - генерирует выборку из данных и строит ранзак, если он удовлетворяет условию поиска, то останавливает весь поиск. <br/>
		/// Иначе сравнивается с текущим лучшим ранзаком и итерация заканчивается.
		/// </summary>
		/// <param name="index">Индекс-номер итерации.</param>
		/// <param name="pls">Состояние потока</param>
		private static void Iteration(int index, ParallelLoopState pls)
		{
			int[] indexSamples = Vector.Sample(MinSamples, x.Length);
			double[] localx = x.Get(indexSamples);
			double[] localy = y.Get(indexSamples);

			SimpleLinearRegression reg = new OrdinaryLeastSquares().Learn(localx, localy);
			double[] outY = reg.Transform(localx);

			double localMedian = GetMedian(outY);
			double localError = GetErrorThreshold(outY, localMedian);
			int localInliers = outY.Count(a => Math.Abs(a - localMedian) <= localError);

			lock (locker)
			{
				if (localInliers > Best.Max(x => x.count))
				{
					Best[index] = (localInliers, reg);

					if (localInliers / (double)MinSamples >= 0.95)
						pls.Break();
				}
			}
		}
		/// <summary>
		/// Вычисляет доверительный интервал.
		/// </summary>
		/// <param name="ticks"></param>
		/// <param name="slope"></param>
		/// <param name="intercept"></param>
		/// <param name="error"></param>
		/// <param name="percent"></param>
		/// <returns></returns>
		private static double GetPercentileSigma(List<Tick> ticks, double slope, double intercept, double error, double percent)
		{
			var a = ticks.FindAll(x => Math.Abs(x.PRICE - (slope * x.VERTEXINDEX + intercept)) > error)
				.Select(x => Math.Abs(x.PRICE - (slope * x.VERTEXINDEX + intercept))).ToArray();

			if (a.Length == 0)
				return error;

			EmpiricalDistribution distribution = new(a);
			return distribution.InverseDistributionFunction(percent);
		}
		/// <summary>
		/// Вычисляет медиану.
		/// </summary>
		/// <param name="y"></param>
		/// <returns></returns>
		private static double GetMedian(double[] y)
		{
			y = y.OrderBy(x => x).ToArray();
			return y.Length % 2 == 0 ? (y[y.Length / 2] + y[y.Length / 2 - 1]) / 2 : y[y.Length / 2];
		}
		/// <summary>
		/// Вычисляет ошибку по инлайнерам.
		/// </summary>
		/// <param name="y"></param>
		/// <param name="median"></param>
		/// <returns></returns>
		private static double GetErrorThreshold(double[] y, double median)
		{
			y = y.Select(x => Math.Abs(x - median)).OrderBy(x => x).ToArray();
			return (y.Length % 2 == 0 ? (y[y.Length / 2 - 1] + y[y.Length / 2]) / 2.0 : y[y.Length / 2]) / 0.6744897501960817;
		}
	}
}
