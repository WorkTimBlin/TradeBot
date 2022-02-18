using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot
{
	abstract class AbstractProviderByParam<TIn, TOut> : IProviderByParam<TOut>
	{
		private readonly Dictionary<string, Action<TOut>> recievers = new();
		protected SequentialProvider<TIn> sequentialProvider;

		public AbstractProviderByParam()
		{
			sequentialProvider = new(OnObject);
		}

		protected void OnObject(TIn tin)
		{
			if (recievers.TryGetValue(GetKey(tin), out Action<TOut>? handler))
			{
				handler?.Invoke(GetTOut(tin));
			}
		}

		public void Subscribe(Param instrument, Action<TOut> handler)
		{
			Subscribe(instrument.classCode, instrument.secCode, handler);
		}
		public void Subscribe(string classCode, string secCode, Action<TOut> handler)
		{
			string key = classCode + secCode;
			if (recievers.ContainsKey(key))
			{
				recievers[key] += handler;
			}
			else
			{
				recievers.Add(key, handler);
			}
		}
		public void Unsubscribe(Param instrument, Action<TOut> handler)
		{
			Unsubscribe(instrument.classCode, instrument.secCode, handler);
		}
		public void Unsubscribe(string classCode, string secCode, Action<TOut> handler)
		{
			string key = classCode + secCode;
			if (recievers.ContainsKey(key))
			{
				recievers[key] -= handler ?? throw new Exception("tried to unsubscribe null");
				if (recievers[key] == null) recievers.Remove(key);
			}
		}

		protected abstract TOut GetTOut(TIn tin);
		protected abstract string GetKey(TIn tin);
	}
}
