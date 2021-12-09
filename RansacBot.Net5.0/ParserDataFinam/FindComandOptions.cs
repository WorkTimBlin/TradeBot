namespace FinamDataLoader
{
	internal class FindCommandOptions
	{
		public string Ticker { get; set; }
		public string Name { get; set; }

		public FindCommandOptions(string ticker, string name)
		{
			Ticker = ticker;
			Name = name;
		}

		public bool IsValid()
		{
			return !(string.IsNullOrWhiteSpace(this.Ticker) && string.IsNullOrWhiteSpace(this.Name));
		}
	}
}

