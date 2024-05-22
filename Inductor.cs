using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBElementPlacement
{
    class Inductor : CircuitElement
    {
        public Inductor(int x, int y) : base(3, 1, Color.Purple)
        {
            _x = x;
            _y = y;
        }
    }
}
