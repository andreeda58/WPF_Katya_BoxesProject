
using BL_Boxes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_Boxes.Events
{
    public delegate bool ConfirmAswerEventHandler(ConfirmPurcharse e);
    public class ConfirmPurcharse : EventArgs
    {
        public string showToCustumer { get; private set; }
       
        public ConfirmPurcharse(List<DataX> listX, List<DataY> ListY)
        {
            StringBuilder toShow;
            toShow = new StringBuilder();
            for (int i = 0; i < ListY.Count; i++)
                toShow.Append($"{i+1}) base size: {listX[i].ToString()}\nHeigth size: {ListY[i].SizeY}, amount: {ListY[i].Quantity}\n\n");
            
            showToCustumer = toShow.ToString();
        }
    }
}

