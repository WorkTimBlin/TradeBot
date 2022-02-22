using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot
{
	class ThroughProvider<T> : IProviderByParam<T>
	{
		public event Action<T> NewT;
		public void Subscribe(Param param, Action<T> handler)
		{
			NewT += handler;
		}

		public void Unsubscribe(Param param, Action<T> handler)
		{
			NewT -= handler;
		}
		public void OnNewN(T t)
		{
			NewT?.Invoke(t);
		}
	}
}
