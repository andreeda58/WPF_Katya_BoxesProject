using BL_Boxes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_Boxes.Models
{
    public class DataTime
    {
        public DataX _x;
        public DataY _y;
        DateTime _last_Time;
        public DateTime Last_Time { get => _last_Time; set => _last_Time = value; }

        public DataTime(DataX x, DataY y)
        {
            _x = x;
            _y = y;
            Last_Time = DateTime.Now;
        }
        public override string ToString() => $"{_x},{_y}";
    }
}
