using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_Boxes.Events
{
    public delegate void NewInfo(Info e);
    public class Info
    {
        public string ToShow { get; private set; }
        public string DeleteBoxeToShow { get; private set; }
        
        public Info(params string[] info)//multiple info to show
        {
            foreach (var item in info)
                ToShow += $"{item}";
        }

        public Info( string info,string boxDelete)//info//deleteinfotoshow
        {
            ToShow = info;
            DeleteBoxeToShow = boxDelete;
        }


        public Info( string info)//normal info
        {
            ToShow = info;
        }
    }

}
