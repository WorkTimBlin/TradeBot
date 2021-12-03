using Accord.Statistics.Models.Regression.Linear;
using System.Collections.Generic;
using System.Linq;

namespace RansacRealTime
{
	public class Ransac
	{
		#region Свойства

		public double Slope { get; private set; }
		public double Intercept { get; private set; }
		public double Sigma { get; private set; }
		public double ErrorTreshold { get; private set; }
		public int Length { get; private set; }
		public readonly int firstTickIndex;
		public readonly int firstBuildTickIndex;
		public int LastRebuildTickIndex { get; private set; }
		/// <summary>
		/// Index of tick next to last
		/// </summary>
		public int EndIndexTick { get { return firstTickIndex + Length; } }

		#endregion

		public delegate void RansacEventHandler(Ransac ransac);
		public event RansacEventHandler StopRansac;
		public event RansacEventHandler NeedRebuilding;

		/// <summary>
		/// Базовый конструктор с параметрами без проверок.
		/// </summary>
		/// <param name="firstTickIndex"></param>
		/// <param name="firstBuildTick"></param>
		/// <param name="lastRebuildTick"></param>
		/// <param name="length"></param>
		/// <param name="slope"></param>
		/// <param name="intercept"></param>
		/// <param name="sigma"></param>
		/// <param name="errorTreshold"></param>
		public Ransac(int firstTickIndex, int firstBuildTick, int lastRebuildTick, int length, float slope, float intercept, float sigma, float errorTreshold)
		{
            Slope = slope;
			Intercept = intercept;
			Sigma = sigma;
			ErrorTreshold = errorTreshold;
			this.firstTickIndex = firstTickIndex;
			Length = length;
			firstBuildTickIndex = firstBuildTick;
			LastRebuildTickIndex = lastRebuildTick;
		}

		/// <summary>
		/// Базовый конструктор, основанный на точках данных.
		/// </summary>
		/// <param name="ticks"></param>
		/// <param name="firstIndex"></param>
		/// <param name="typeSigma"></param>
		/// <param name="percentile"></param>
		public Ransac(in List<Tick> ticks, int firstIndex, TypeSigma typeSigma, double percentile = 90)
		{
			RansacComputator.Compute(ticks, typeSigma, percentile, out SimpleLinearRegression reg, out double errorTreshold, out double sigma);
			Slope = reg.Slope;
			Intercept = reg.Intercept;
			Length = ticks.Count;
			Sigma = sigma;
			firstTickIndex = firstIndex;
			ErrorTreshold = errorTreshold;
			firstBuildTickIndex = EndIndexTick - 1;
			LastRebuildTickIndex = firstBuildTickIndex;
		}

		/// <summary>
		/// Реакция на появление новой вершины, проверяем необходиомсть перестройки или окончания ранзака, 
		/// иначе просто добавляем вершину.
		/// </summary>
		/// <param name="tick">Новая вершина</param>
		public void OnNewVertex(Tick tick)
		{
			switch (GetActionTwoK(tick))
			{
				case Action.Stop:
					StopRansac?.Invoke(this);
					break;
				case Action.Rebuild:
					NeedRebuilding?.Invoke(this);
					break;
				case Action.Add:
					AddVertex(tick);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Проверка TwoK. Выпадение за сигму либо перестраивает ранзак либо пытается его перестроить.
		/// </summary>
		/// <param name="tick"></param>
		/// <returns></returns>
		private Action GetActionTwoK(Tick tick)
		{
			double difference = (tick.PRICE - Transform(tick.VERTEXINDEX)) * (Slope > 0 ? -1 : 1);

			if (difference > Sigma)
			{
				NeedRebuilding?.Invoke(this);
				difference = (tick.PRICE - Transform(tick.VERTEXINDEX)) * (Slope > 0 ? -1 : 1);

				if (difference > Sigma)
					return Action.Stop;
				else
					return Action.Nothing;
			}

			if (-difference > Sigma)
				return Action.Rebuild;

			return Action.Add;
		}

		/// <summary>
		/// Перестраивает ранзак и обновляет параметр LastIndexRebuild.
		/// </summary>
		/// <param name="ransac">Обновленный ранзак.</param>
		public void Rebuild(Ransac ransac)
		{
			Slope = ransac.Slope;
			Intercept = ransac.Intercept;
			Length = ransac.Length;
			ErrorTreshold = ransac.ErrorTreshold;
			Sigma = ransac.Sigma;
			LastRebuildTickIndex = EndIndexTick - 1;
		}

		/// <summary>
		/// Добавляет вершину в ранзак (увеличивает длину).
		/// </summary>
		/// <param name="tick"></param>
		private void AddVertex(Tick tick)
		{
			Length++;
		}

		/// <summary>
		/// Вычисляет значение ранзака в указанной точке.
		/// </summary>
		/// <param name="index">X - индекс тика по MonkeyN</param>
		/// <returns>Y = Slope*X + Intercept</returns>
		public double Transform(int index)
		{
			return Intercept + Slope * index;
		}

		/// <summary>
		/// Вычисляет значение ранзака в указанных точках.
		/// </summary>
		/// <param name="indexes">Список индексов.</param>
		/// <returns>Список: Y = Slope*X + Intercept.</returns>
		public double[] Transform(int[] indexes)
		{
			return indexes.Select(x => x * Slope + Intercept).ToArray();
		}

		public enum Action : byte
		{
			Add,
			Rebuild,
			Stop,
			Nothing
		}
	}
}
