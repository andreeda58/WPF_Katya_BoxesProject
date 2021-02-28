using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using BL_Boxes.Events;
using BL_Boxes.Models;
using DS_Boxes;

namespace BL_Boxes
{
    public class Manager
    {
        static string path = "BoxesKatya.txt";
        static bool isNull;
        public event NewInfo NewInfoToShow;
        public event ConfirmAswerEventHandler confirmationCustomer;
        public event NewOrder orderBox;
        BST<DataX> bstMain;
        DoubleLinkedList<DataTime> queue_Time;
        DispatcherTimer _checkPeriod;
        TimeSpan _expitationDate;
        int _requirementsReOrderMinAmount;
        private double maxMultiSize = 1.5;
        bool alreadyRequested;
        int _maxItemsPerBox;

        public Manager(int maxItemsPerBox, int requirementsReOrderMinAmount, TimeSpan checkPeriod, TimeSpan expirationDate)
        {
            bstMain = new BST<DataX>();
            queue_Time = new DoubleLinkedList<DataTime>();

            _maxItemsPerBox = maxItemsPerBox;
            DataY.Renew_Max_Quantity(maxItemsPerBox);
            _requirementsReOrderMinAmount = requirementsReOrderMinAmount;
            DataY.Renew_requirementsReOrderMinAmount(requirementsReOrderMinAmount);

            _expitationDate = expirationDate;
            _checkPeriod = new DispatcherTimer();
            _checkPeriod.Interval = checkPeriod;
            _checkPeriod.Tick += _checkPeriod_Tick;
            _checkPeriod.Start();
        }

        private void _checkPeriod_Tick(object sender, EventArgs e)
        {
            if (queue_Time.First != null)
            {
                if (_expitationDate <= DateTime.Now - queue_Time.Last.Data.Last_Time)
                {
                   
                    DataY dataY = new DataY(queue_Time.Last.Data._y.SizeY, queue_Time.Last.Data._y.Quantity);
                    DataX dataX = new DataX(queue_Time.Last.Data._x.SizeX, dataY);

                    bstMain.FindValue(dataX, out dataX);
                    dataX.bstY.Remove(dataY);

                    if (dataX.bstY.IsRootEmpty())
                        bstMain.Remove(dataX);

                    queue_Time.RemoveLast();
                    NewInfoToShow.Invoke(new Info($"the box with the following characteristics: Width: {dataX.SizeX},Heigth: {dataY.SizeY}\nWAS DELETE!!!!! ",
                        $"Width: {dataX.SizeX},Heigth: {dataY.SizeY}"));
                }
            }
        }
        public void AddBoxes(double xsize, double ysize, int newquantity)
        {
            DataY newY = new DataY(ysize, newquantity);
            DataX newX = new DataX(xsize, newY);
            DataTime dataTime = new DataTime(newX, newY);

            DataX foundX;
            DataY foundY;

            if (bstMain.IsRootEmpty())// if the tree is empty
            {
                queue_Time.AddFirst(new DataTime(newX, newY));
                bstMain.Add(newX);
                newY.DateDoubleListNode = queue_Time.First;
            }
            else
            {
                if (bstMain.FindValue(newX, out foundX)) //if found  the X size
                {
                    //get the node X 
                    if (foundX.bstY.FindValue(newY, out foundY))  //if the node 'y' exits sum the new quantity 
                    {
                        if (foundY.Max_Quantity >= foundY.Quantity + newquantity)//case that the quantity is smaller of max quantity
                            foundY.Quantity += newquantity;
                        else
                            foundY.Quantity = foundY.Max_Quantity;
                    }
                    else
                    {
                        foundX.bstY.Add(newY);// add the newy node to the not  exist y.
                        queue_Time.AddFirst(dataTime);
                        newY.DateDoubleListNode = queue_Time.First;
                    }
                }
                else
                {
                    queue_Time.AddFirst(dataTime);
                    bstMain.Add(newX);
                    newY.DateDoubleListNode = queue_Time.First;
                }
            }
        }
        public string Infornation(double xsize, double ysize)
        {
            DataY newY = new DataY(ysize);
            DataX newX = new DataX(xsize, newY);
            string data = "";

            if (bstMain.FindValue(newX, out newX))
            {
                data += $"{newX.ToString()}\n";
                if (newX.bstY.FindValue(newY, out newY))
                {
                    data += $"{newY.ToString()}";
                }
                else
                    data += "The box searched with the following heigth size doesn't exist";

                return data;
            }
            else
                return "The box searched with the following base size doesn't exist";
        }
        public string Infornation(DataX X, DataY Y) => $" {X.ToString()}\n {Y.ToString()}";
        public bool Buy(double xsize, double ysize, int amount = 1)
        {
            int initialAmount = amount;
            bool exit = false;
            DataY dataY = new DataY(ysize, amount), initialDataY = dataY;
            DataX dataX = new DataX(xsize, dataY);
            List<DataX> pointsX = new List<DataX>();
            List<DataY> pointsY = new List<DataY>();

            if (!bstMain.FindValueOrClosest(dataX, out dataX) || dataX.SizeX > xsize * maxMultiSize)
            {
                if (dataX == null)
                    NewInfoToShow.Invoke(new Info("No match"));
                else
                    NewInfoToShow.Invoke(new Info("The x size is too large "));
                return false;
            }
            while (dataX.SizeX <= xsize * maxMultiSize && exit == false)
            {
                if (dataX.bstY.FindValueOrClosest(dataY, out dataY))
                {
                    while (dataY.SizeY <= ysize * maxMultiSize)
                    {
                        if (dataY.Quantity <= amount)
                        {
                            amount -= dataY.Quantity;
                            pointsX.Add(dataX);
                            pointsY.Add(dataY);

                            if (amount == 0) { exit = true; break; }

                            dataX.bstY.Find_Next_Size(dataY, out dataY);
                            if (dataY == null) { dataY = initialDataY; break; }
                        }
                        else
                        {
                            pointsX.Add(dataX);
                            pointsY.Add(dataY);
                            exit = true;
                            break;
                        }
                    }
                }
                if (exit == false)
                {
                    if (dataY == null)
                        dataY = initialDataY;

                    bstMain.Find_Next_Size(dataX, out dataX);
                    if (dataX == null) break;
                }
            }
            if (pointsX.Count == 0)
            {
                NewInfoToShow.Invoke(new Info("No match"));
                return false;
            }
            if (confirmationCustomer.Invoke(new ConfirmPurcharse(pointsX, pointsY)))
            {
                for (int i = 0; i < pointsY.Count; i++)
                {
                    if (pointsY[i].Quantity <= initialAmount)
                    {
                        initialAmount -= pointsY[i].Quantity;
                        queue_Time.RemoveNodeFromDoubleList(pointsY[i].DateDoubleListNode);

                        orderBox.Invoke(new OrderMoreBoxes(pointsX[i], pointsY[i]));

                        pointsX[i].bstY.Remove(pointsY[i]);

                        if (pointsX[i].bstY.IsRootEmpty())
                            bstMain.Remove(pointsX[i]);
                    }
                    else
                    {
                        pointsY[i].Quantity -= initialAmount;
                        queue_Time.ReorderNode(pointsY[i].DateDoubleListNode);
                        pointsY[i].DateDoubleListNode = queue_Time.First;

                        if (pointsY[i].Quantity <= _requirementsReOrderMinAmount && alreadyRequested == false)
                        {
                            alreadyRequested = true;
                            orderBox.Invoke(new OrderMoreBoxes(pointsX[i], pointsY[i]));
                        }
                    }
                }
                return true;
            }
            else
            {
                pointsX.Clear();
                pointsY.Clear();
                return false;
            }
        }
        public void ChangesMaxSearchPercentage(double percentage)
        {
            maxMultiSize = 1 + percentage;
        }
        public void ChangesMaxQuantity(int newMaxQuantity) => DataY.Renew_Max_Quantity(newMaxQuantity);

        public static Manager OnInit()
        {
            StreamReader sr = new StreamReader(File.Open(path, FileMode.OpenOrCreate));
            var stringData = sr.ReadToEnd();
            if (string.IsNullOrWhiteSpace(stringData))
            {
                isNull = true;
                return null;
            }
            else
                isNull = false;

            string[] splitString = stringData.Split('\n');
            var firstLine = splitString[0].Split(',');
            Manager newManager = new Manager(int.Parse(firstLine[0]), int.Parse(firstLine[1]), TimeSpan.Parse(firstLine[2]), TimeSpan.Parse(firstLine[3]));
            List<DataTime> newBoxList = new List<DataTime>();

            for (int i = 1; i < splitString.Length - 1; i++)
            {
                var newLine = splitString[i].Split(',');
                DataY newY = new DataY(double.Parse(newLine[1]), int.Parse(newLine[2]));
                DataX newX = new DataX(double.Parse(newLine[0]), newY);

                DateTime newDate = DateTime.Parse(newLine[3]);

                newBoxList.Add(new DataTime(newX, newY) { Last_Time = newDate });

                if (newManager.bstMain.FindValue(newX, out DataX data))
                    data.bstY.Add(newY);

                else
                    newManager.bstMain.Add(newX);


            }
            newBoxList.Sort((b1, b2) => -b1.Last_Time.CompareTo(b2.Last_Time));
            foreach (var item in newBoxList)
            {
                newManager.queue_Time.AddLast(item);
                item._y.DateDoubleListNode = newManager.queue_Time.Last;
            }

            return newManager;
        }
        public void OnExit()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{_maxItemsPerBox},{_requirementsReOrderMinAmount},{_checkPeriod.Interval},{_expitationDate}");

            bstMain.ScanInOrder(cw1 => cw1.bstY.ScanInOrder(cw2 => sb.AppendLine($"{cw1.ToString()},{cw2.ToString()}")));
            using (StreamWriter sw = new StreamWriter(File.Open(path, FileMode.Create)))
            {

                sw.Write(sb.ToString());
                sw.Close();
            }
        }
    }
}

