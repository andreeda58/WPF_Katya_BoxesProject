using DS_Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_Boxes.Models
{
    public class DataX : IComparable<DataX>
    {
        private double _sizeX;
        internal BST<DataY> bstY;

        public double SizeX { get => _sizeX; set => _sizeX = value; }

        public DataX(double xSize, DataY newY)
        {
            _sizeX = xSize;

            if (bstY == null)
                bstY = new BST<DataY>();

            bstY.Add(newY);
        }
        public int CompareTo(DataX other) => this._sizeX.CompareTo(other._sizeX);
        public override string ToString() => $"{_sizeX}";

    }
}
