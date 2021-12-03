using System;

namespace RansacRealTime
{

	public delegate void NewTickHandler(Tick tick);

	public readonly struct Tick
	{
		/// <summary>
		/// ID тика из QUIK
		/// </summary>
		public readonly long ID;
		/// <summary>
		/// Порядковый номер вершины
		/// </summary>
		public readonly int VERTEXINDEX;
		public readonly double PRICE;


		/// <summary>
		/// полный конструктор
		/// </summary>
		/// <param name="id">ID тика.</param>
		/// <param name="index">Пользовательский индекс вершин MonkeyN.</param>
		/// <param name="price">Цена тика.</param>
		public Tick(long id, int index, double price)
		{
			ID = id;
			VERTEXINDEX = index;
			PRICE = price;
		}

		/// <summary>
		/// serialises the tick
		/// </summary>
		/// <returns>serialised tick in format ID;VERTEXINDEX;PRICE</returns>
		public override string ToString()
		{
			return ID.ToString() + ';' + VERTEXINDEX.ToString() + ';' + PRICE.ToString();
		}

		/// <summary>
		/// Parses given line
		/// </summary>
		/// <param name="line"></param>
		/// <returns>tick parsed from format ID;VERTEXINDEX;PRICE</returns>
		public static Tick StandartParse(string line)
		{
			return StandartParse(line.Split(';'));
		}

		public static Tick StandartParse(string[] fields)
		{
			return new(Convert.ToInt64(fields[0]), Convert.ToInt32(fields[1]), Convert.ToDouble(fields[2]));
		}
	}
}
