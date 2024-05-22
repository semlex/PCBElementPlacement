using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PCBElementPlacement
{
    class Resistor : CircuitElement
    {
        public Resistor(int x, int y) : base(2, 1, Color.Red) {
            _x = x;
            _y = y;
        }
    }
}
