using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacRealTime
{
    class RansacObserver
    {
        public Vertexes Vertexes { get; set; }
        public MonkeyNFilter MonkeyNFilter { get; set; }
        public int N { get;private set; }

        public RansacObserver(in Vertexes vertexes, in MonkeyNFilter monkeyNFilter, int n)
        {
            Vertexes = vertexes;
            MonkeyNFilter = monkeyNFilter;
            N = n;
        }
    }
}
