using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBElementPlacement
{
    class Capacitor : CircuitElement
    {
        public Capacitor(int x, int y) : base(1, 1, Color.Orange)
        {
            _x = x;
            _y = y;
        }
    }
}
