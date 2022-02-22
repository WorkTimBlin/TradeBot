using QuikSharp;
using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using RansacBot.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot
{
	class TradeParams
	{
		public readonly string classCode;
		public readonly string secCode;
		public readonly string accountId;
		public readonly string clientCode;
		public TradeParams(string classCode, string secCode, string accountId, string clientCode)
		{
			this.classCode = classCode;
			this.secCode = secCode;
			this.accountId = accountId;
			this.clientCode = clientCode;
		}
	}
}
