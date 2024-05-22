using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace PCBElementPlacement.Tests
{
    [TestClass]
    public class PCBElementPlacemenTests
    {
        [TestMethod]
        public void CanPlaceElement_x_51_width_50_false_expected()
        {
            PCB pcb = new PCB(50, 50);
            CircuitElement elem = new CircuitElement(51, 50, Color.Red);
            elem.X = 0;
            elem.Y = 0;

            bool result = pcb.CanPlaceElement(elem);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanPlaceElement_x_50_width_51_true_expected()
        {
            PCB pcb = new PCB(51, 50);
            CircuitElement elem = new CircuitElement(50, 50, Color.Red);
            elem.X = 0;
            elem.Y = 0;

            bool result = pcb.CanPlaceElement(elem);

            Assert.IsTrue(result);
        }
        [TestMethod]
        public void CanPlaceElement_y_51_height_50_false_expected()
        {
            PCB pcb = new PCB(50, 50);
            CircuitElement elem = new CircuitElement(50, 51, Color.Red);
            elem.X = 0;
            elem.Y = 0;

            bool result = pcb.CanPlaceElement(elem);

            Assert.IsFalse(result);
        }
        [TestMethod]
        public void CanPlaceElement_y_50_height_51_true_expected()
        {
            PCB pcb = new PCB(50, 51);
            CircuitElement elem = new CircuitElement(50, 50, Color.Red);
            elem.X = 0;
            elem.Y = 0;

            bool result = pcb.CanPlaceElement(elem);

            Assert.IsTrue(result);
        }
    }
}
