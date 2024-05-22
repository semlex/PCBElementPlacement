using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PCBElementPlacement
{
    class CircuitElement
    {
        protected int _x;
        protected int _y;
        int _width;
        int _height;
        Color _color;

        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        public int Width
        {
            get
            {
                return _width;
            }
        }

        public int Height {
            get
            {
                return _height;
            }
        }

        public CircuitElement(int width, int height, Color color)
        {
            _width = width;
            _height = height;
            _color = color;
        }

        public void Draw(Graphics g, int cellSize)
        {
            SolidBrush solidBrush = new SolidBrush(_color);
            for (int i = _x * cellSize; i < (_x + _width) * cellSize; i += cellSize)
            {
                g.FillRectangle(solidBrush, i + 1, _y * cellSize + 1, cellSize - 1, cellSize - 1);
            }
        }
    }
}
