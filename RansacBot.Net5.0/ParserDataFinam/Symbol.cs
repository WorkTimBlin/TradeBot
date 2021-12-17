using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinamDataLoader
{
	internal class Symbol
	{
		public int Id { get; set; }
		public string SecCode { get; set; }
		public string Name { get; set; }
		public int MarketId { get; set; }
		public string MarketName { get; set; }
	}
}
