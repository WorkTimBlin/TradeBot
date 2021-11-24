namespace RansacRealTime
{

	public delegate void NewTickHandler(Tick tick);

	/// <summary>
	/// Тик не из QUIK.
	/// </summary>
	public readonly struct Tick
	{
		/// <summary>
		/// ID тика из  QUIK.
		/// </summary>
		public readonly long ID { get; }
		/// <summary>
		/// Порядковый индекс вершины из MonkeyN.
		/// </summary>
		public readonly int VERTEXINDEX { get; }
		/// <summary>
		/// Цена тика.
		/// </summary>
		public readonly double PRICE { get; }


		/// <summary>
		/// Базовый конструктор
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
	}
}
