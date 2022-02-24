using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot
{
	static class IEnumerableExtention
	{
		public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<IEnumerable<TSource>> sources)
		{
			foreach (IEnumerable<TSource> source in sources)
			{
				foreach (TSource obj in source)
				{
					yield return obj;
				}
			}
		}
	}
}
