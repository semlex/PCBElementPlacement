using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCBElementPlacement
{
    struct ElementRank
    {
        public int index;
        public int value;
    }

    class PCB
    {
        int _width;
        int _height;
        List<CircuitElement> _elements;
        int[,] _plate;
        int[,] _connections;

        public PCB(int width, int height) {
            _width = width;
            _height = height;
            _elements = new List<CircuitElement>();
            _plate = new int[width, height];
            _connections = new int[0, 0];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++) {
                    _plate[i, j] = 0;
                }
            }
        }

        public bool CanPlaceElement(CircuitElement newElement)
        {
            if (newElement.X + newElement.Width > _width)
            {
                return false;
            }

            if (newElement.Y + newElement.Height > _height) {
                return false;
            }

            for (int i = newElement.X; i < newElement.X + newElement.Width; i++)
            {
                for (int j = newElement.Y; j < newElement.Y + newElement.Height; j++)
                {
                    if (_plate[i, j] == 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void AddElement(CircuitElement newElement)
        {
            _elements.Add(newElement);

            if (_connections.Length == 0)
            {
                if (_elements.Count == 2)
                {
                    _connections = new int[2, 2];

                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            _connections[i, j] = 0;
                        }
                    }
                }
            }
            else
            {
                int[,] connectionsCopy = new int[_connections.GetLength(0), _connections.GetLength(1)];
                Array.Copy(_connections, connectionsCopy, _connections.Length);

                _connections = new int[_elements.Count, _elements.Count];

                for (int i = 0; i < _connections.GetLength(0); i++)
                {
                    for (int j = 0; j < _connections.GetLength(1); j++)
                    {
                        _connections[i, j] = 0;
                    }
                }

                for (int i = 0; i < connectionsCopy.GetLength(0); i++)
                {
                    for (int j = 0; j < connectionsCopy.GetLength(1); j++)
                    {
                        _connections[i, j] = connectionsCopy[i, j];
                    }
                }
            }
        }

        public void PlaceElement(CircuitElement newElement)
        {
            for (int i = newElement.X; i < newElement.X + newElement.Width; i++)
            {
                for (int j = newElement.Y; j < newElement.Y + newElement.Height; j++)
                {
                    _plate[i, j] = 1;
                }
            }
        }

        public bool CanReplaceElements(int firstElemIndex, int secondElemIndex)
        {
            CircuitElement firstElem = _elements[firstElemIndex];
            CircuitElement secondElem = _elements[secondElemIndex];
            int[,] plateCopy = new int[_plate.GetLength(0), _plate.GetLength(1)];
            Array.Copy(_plate, plateCopy, _plate.Length);

            for (int i = firstElem.X; i < firstElem.X + firstElem.Width; i++) {
                for (int j = firstElem.Y; j < firstElem.Y + firstElem.Height; j++)
                {
                    _plate[i, j] = 0;
                }
            }

            for (int i = secondElem.X; i < secondElem.X + secondElem.Width; i++)
            {
                for (int j = secondElem.Y; j < secondElem.Y + secondElem.Height; j++)
                {
                    _plate[i, j] = 0;
                }
            }

            int tempX = firstElem.X;
            int tempY = firstElem.Y;

            firstElem.X = secondElem.X;
            firstElem.Y = secondElem.Y;

            secondElem.X = tempX;
            secondElem.Y = tempY;

            bool result1 = CanPlaceElement(firstElem);
            bool result2 = CanPlaceElement(secondElem);

            Array.Copy(plateCopy, _plate, _plate.Length);

            return result1 && result2;
        }

        public void ReplaceElements(int firstElemIndex, int secondElemIndex)
        {
            CircuitElement firstElem = _elements[firstElemIndex];
            CircuitElement secondElem = _elements[secondElemIndex];
            int[,] plateCopy = new int[_plate.GetLength(0), _plate.GetLength(1)];
            Array.Copy(_plate, plateCopy, _plate.Length);

            for (int i = firstElem.X; i < firstElem.X + firstElem.Width; i++)
            {
                for (int j = firstElem.Y; j < firstElem.Y + firstElem.Height; j++)
                {
                    _plate[i, j] = 0;
                }
            }

            for (int i = secondElem.X; i < secondElem.X + secondElem.Width; i++)
            {
                for (int j = secondElem.Y; j < secondElem.Y + secondElem.Height; j++)
                {
                    _plate[i, j] = 0;
                }
            }

            int tempX = firstElem.X;
            int tempY = firstElem.Y;

            firstElem.X = secondElem.X;
            firstElem.Y = secondElem.Y;

            secondElem.X = tempX;
            secondElem.Y = tempY;

            PlaceElement(firstElem);
            PlaceElement(secondElem);
        }

        public int GetElemIndexByCoords(int x, int y)
        {
            return _elements.FindIndex(e => x >= e.X && x < e.X + e.Width && y >= e.Y && y < e.Y + e.Height);
        }

        public void ConnectElements(int index1, int index2)
        {
            _connections[index1, index2] = 1;
            _connections[index2, index1] = 1;
        }

        public void DrawConnections(Graphics g, int cellSize)
        {
            Pen p = new Pen(Color.White);

            for (int i = 0; i < _connections.GetLength(0); i++)
            {
                for (int j = 0; j < _connections.GetLength(1); j++)
                {
                    if (_connections[i, j] == 1)
                    {
                        CircuitElement firstElem = _elements[i];
                        CircuitElement secondElem = _elements[j];

                        g.DrawLine(p, firstElem.X * cellSize + 10, firstElem.Y * cellSize + 10, secondElem.X * cellSize + 10, secondElem.Y * cellSize + 10);
                    }
                }
            }
        }

        private List<ElementRank> CalculateElementsRanks() {
            List<ElementRank> list = new List<ElementRank>();

            for (int i = 0; i < _connections.GetLength(0); i++)
            {
                
                ElementRank rank = new ElementRank();
                rank.index = i;
                rank.value = 0;

                for (int j = 0; j < _connections.GetLength(1); j++)
                {
                    rank.value++;
                }

                list.Add(rank);
            }

            list.Sort((a, b) => b.value - a.value);

            return list;
        }

        public int CalculateElemsConnectionsLength()
        {
            int result = 0;

            for (int i = 0; i < _connections.GetLength(0); i++)
            {
                CircuitElement element = _elements[i];
                for (int j = 0; j < _connections.GetLength(1); j++)
                {
                    if (_connections[i, j] == 1)
                    {
                        CircuitElement connectedElement = _elements[j];

                        result += Math.Abs(element.X - connectedElement.X) + Math.Abs(element.Y - connectedElement.Y);
                    }
                }
            }

            return result;
        }

        public void OptimalReplaceElements()
        {
            List<ElementRank> elementRanks = CalculateElementsRanks();

            for (int i = 0; i < elementRanks.Count; i++)
            {
                int firstElemIndex = elementRanks[i].index;
                for (int j = i; j < elementRanks.Count; j++)
                {
                    int secondElemIndex = elementRanks[j].index;

                    int connectionsLength1 = CalculateElemsConnectionsLength();

                    if (CanReplaceElements(firstElemIndex, secondElemIndex))
                    {
                        ReplaceElements(firstElemIndex, secondElemIndex);

                        int connectionsLength2 = CalculateElemsConnectionsLength();

                        if (connectionsLength1 <= connectionsLength2)
                        {
                            ReplaceElements(secondElemIndex, firstElemIndex);
                        } else
                        {
                            break;
                        }
                    }
                }
            }

        }

        public void Draw(Graphics g, int cellSize)
        {
            Pen p = new Pen(Color.DarkGray, 0.1f);

            for (int i = 0; i <= _width * cellSize; i += cellSize)
            {
                g.DrawLine(p, i, 0, i, _height * cellSize);
            }

            for (int i = 0; i <= _height * cellSize; i+=cellSize)
            {
                g.DrawLine(p, 0, i, _width * cellSize, i);
            }
            //for (int i = 0; i < _height * 25; i += 25)
            //{
            //    g.DrawLine(p, i, 0, i, _height * 25);
            //    g.DrawLine(p, 0, i, _height * 25, i);
            //}
            SolidBrush solidBrush;

            for (int x = 0; x < _width * cellSize; x += cellSize)
            {
                for (int y = 0; y < _height * cellSize; y += cellSize)
                {
                    solidBrush = new SolidBrush(Color.Black);
                    g.FillRectangle(solidBrush, x + 1, y + 1, cellSize - 1, cellSize - 1);
                }
            }

            _elements.ForEach(e => e.Draw(g, cellSize));
            DrawConnections(g, cellSize);
        }
    }
}
