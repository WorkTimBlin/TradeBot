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
	}
}
