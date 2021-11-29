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


        public RansacObserver(in Vertexes vertexes, in MonkeyNFilter monkeyNFilter)
        {
            Vertexes = vertexes;
            MonkeyNFilter = monkeyNFilter;
        }
    }
}
