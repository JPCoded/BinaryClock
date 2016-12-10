using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryClock
{
    class BinaryCircles
    {
        public BinaryCircle[] Circles { get; set; }
        private int CurrentPosition { get; set; }

        public BinaryCircle Next()
        {
            var current = Circles[CurrentPosition];
            CurrentPosition++;
            return current;
        }


        public void Reset()
        {
            CurrentPosition = 0;
        }
    }
}
