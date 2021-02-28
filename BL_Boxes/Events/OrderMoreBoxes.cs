
using BL_Boxes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_Boxes.Events
{
    public delegate void NewOrder(OrderMoreBoxes e);
    public class OrderMoreBoxes
    {
        public DataX X { get; private set; }
        public DataY Y { get; private set; }
        public OrderMoreBoxes(DataX xToOrder, DataY yToOrder)
        {
            X = xToOrder;
            Y = yToOrder;
        }
        public override string ToString()
        {
            string[] splitString = Y.ToString().Split(',');
            return $"Ordering new boxes with the following measurements :\nWidth: {X.SizeX},Heigth: {splitString[0]}";
        }
    }
}
