using Accord.Statistics.Models.Regression.Linear;
using System.Collections.Generic;
using System.Linq;

namespace RansacRealTime
{
	public class Ransac
	{
		#region Свойства

		/// <summary>
		/// Угол наклона прямой. Параметр из уравнения прямой (Y = Slope * X + Intercept).
		/// </summary>
		public double Slope { get; private set; }
		/// <summary>
		/// Сдвиг прямой. Параметр из уравнения прямой (Y = Slope * X + Intercept).
		/// </summary>
		public double Intercept { get; private set; }
		/// <summary>
		/// Сигма (может быть вычислена по разному).
		/// </summary>
		public double Sigma { get; private set; }
		/// <summary>
		/// Один из типов сигм, используя эту величину рассчитываются инлайнеры.
		/// </summary>
		public double ErrorTreshold { get; private set; }
		/// <summary>
		/// Длина ранзака.
		/// </summary>
		public int Length { get; private set; }
		/// <summary>
		/// Индекс, первого входящего в ранзак, тика по MonkeyN.
		/// </summary>
		public int FirstIndexTick { get; private set; }
		/// <summary>
		/// Индекс тика по MonkeyN, в который был построен ранзак.
		/// </summary>
		public int FirstIndexBuild { get; private set; }
		/// <summary>
		/// Индекс тика по MonkeyN, в котором было последнее перестроение ранзака.
		/// </summary>
		public int LastIndexRebuild { get; private set; }
		/// <summary>
		/// Индекс тика по MonkeyN, который преодалел сигму и больше не попадает в ранзак.
		/// </summary>
		public int EndIndexTick { get { return FirstIndexTick + Length; } }

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
			FirstIndexTick = firstTickIndex;
			Length = length;
			FirstIndexBuild = firstBuildTick;
			LastIndexRebuild = lastRebuildTick;
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
			FirstIndexTick = firstIndex;
			ErrorTreshold = errorTreshold;
			FirstIndexBuild = EndIndexTick - 1;
			LastIndexRebuild = FirstIndexBuild;
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
			LastIndexRebuild = EndIndexTick - 1;
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
	}
}
