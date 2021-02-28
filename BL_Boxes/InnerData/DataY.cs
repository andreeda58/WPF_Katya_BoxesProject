using DS_Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_Boxes.Models
{
    public class DataY : IComparable<DataY>
    {
        private double _sizeY;
        private static int _maxQuantity;
        private int _quantity;
        private static int _requirementsReOrderMinAmount;
        private DoubleLinkedList<DataTime>.Node _node;

        public int Quantity { get { return _quantity; } set { _quantity = value; } }
        public double SizeY { get { return _sizeY; } set { _sizeY = value; } }
        public DoubleLinkedList<DataTime>.Node DateDoubleListNode { get => _node; set => _node = value; }
        public int Max_Quantity { get => _maxQuantity; }
        public int RequirementsReOrderMinAmount { get => _requirementsReOrderMinAmount; }

        



        public DataY(double size, int quantity=0)
        {
            _sizeY = size;

            if (quantity > _maxQuantity)
                _quantity = _maxQuantity;
            else
                _quantity = quantity;
            
        }
        public int CompareTo(DataY other) => this._sizeY.CompareTo(other._sizeY);
        public override string ToString() => $"{_sizeY},{_quantity},{DateDoubleListNode.Data.Last_Time}";
        internal static void Renew_Max_Quantity(int newquantity = 30) => _maxQuantity = newquantity;
        internal static void Renew_requirementsReOrderMinAmount(int newAmount = 10) => _requirementsReOrderMinAmount = newAmount;
    }
}
