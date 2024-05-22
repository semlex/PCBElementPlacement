using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCBElementPlacement
{
    enum PCBAction { Add, Connect };

    public partial class Form1 : Form
    {

        const int cellSize = 25;

        PCBAction _pcbAction;
        PCB _pcb;

        int _selectedElement1;
        int _selectedElement2;

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            textBox1.Text = "0";
            _pcbAction = PCBAction.Add;
            _pcb = new PCB(15, 15);
            _selectedElement1 = -1;
            _selectedElement2 = -1;

            PrintGrid(pictureBox1);
        }

        private void PrintGrid(PictureBox pictureBox)
        {
            pictureBox.Image = null;
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics g = Graphics.FromImage(pictureBox.Image);

            _pcb.Draw(g, cellSize);

            //for (int i = 0; i < image.Length; i++)
            //{
            //    if (i > 0 && i % itemsInRow == 0)
            //    {
            //        x = 0;
            //        y += 25;
            //    }

            //    if (image[i] == 1)
            //    {
            //        solidBrush = new SolidBrush(Color.Black);
            //    }
            //    else
            //    {
            //        solidBrush = new SolidBrush(Color.White);
            //    }
            //    g.FillRectangle(solidBrush, x + 1, y + 1, 24, 24);

            //    x += 25;
            //}
        }

        private CircuitElement CreateNewCircuitElement(int x, int y)
        {
            switch (comboBox1.Text)
            {
                case "Резистор":
                    return new Resistor(x, y);
                case "Конденсатор":
                    return new Capacitor(x, y);
                case "Индуктор":
                    return new Inductor(x, y);
                default: return new Resistor(x, y);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            pictureBox2.Image = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Graphics g = Graphics.FromImage(pictureBox2.Image);

            switch (comboBox1.Text)
            {
                case "Резистор":
                    Resistor resistor = new Resistor(0, 0);
                    resistor.Draw(g, cellSize);
                    break;
                case "Конденсатор":
                    Capacitor capacitor = new Capacitor(0, 0);
                    capacitor.Draw(g, cellSize);
                    break;
                case "Индуктор":
                    Inductor inductor = new Inductor(0, 0);
                    inductor.Draw(g, cellSize);
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _pcbAction = PCBAction.Add;
            _selectedElement1 = -1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _pcbAction = PCBAction.Connect;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X / cellSize;
            int y = e.Y / cellSize;

            Graphics g = pictureBox1.CreateGraphics();

            if (_pcbAction == PCBAction.Add)
            {
                CircuitElement newElem = CreateNewCircuitElement(x, y);

                if (_pcb.CanPlaceElement(newElem) == true)
                {
                    _pcb.AddElement(newElem);
                    _pcb.PlaceElement(newElem);

                    newElem.Draw(g, cellSize);
                }
            }

            if (_pcbAction == PCBAction.Connect)
            {
                int index = _pcb.GetElemIndexByCoords(x, y);

                if (index != -1)
                {
                    if (_selectedElement1 == -1)
                    {
                        _selectedElement1 = index;
                    }
                    else
                    {
                        _selectedElement2 = index;
                        _pcb.ConnectElements(_selectedElement1, _selectedElement2);
                        _pcb.DrawConnections(g, cellSize);
                        textBox1.Text = $"{_pcb.CalculateElemsConnectionsLength()}";

                        _selectedElement1 = -1;
                        _selectedElement2 = -1;
                    }
                }


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();

            _pcb.OptimalReplaceElements();
            _pcb.Draw(g, cellSize);
            textBox1.Text = $"{_pcb.CalculateElemsConnectionsLength()}";
        }
    }
}
