using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL_Boxes;
using BL_Boxes.Events;

namespace WPF_Katya_BoxesProject
{
   
    public partial class MainWindow : Window
    {    
        Manager  manager;
        bool success;

        public MainWindow()
        {
            InitializeComponent();
            manager = new Manager(40,10,new TimeSpan(1,0,0),new TimeSpan(1,0,0));
            manager.confirmationCustomer += Manager_confirmationCustomer;
            manager.orderBox += Manager_NewOrder;
            manager.NewInfoToShow += Manager_NewInfoToShow;
        }

        private void Manager_NewInfoToShow(InfoToCustomer e) => MessageBox.Show(e.ToShow);
        public void Manager_NewOrder(OrderMoreBoxes e)=> MessageBox.Show($"Ordering new boxes for the storage with the following measures: Heigth:{e.Y.SizeY}, Width:{e.X.SizeX}");
        public bool Manager_confirmationCustomer(ConfirmPurcharse e)
        {
            MessageBoxResult result =  MessageBox.Show( e.showToCustumer,"Check Out", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                return true;
            else
                return false;
        }
        private void button_add_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_Xsize.Text!="" && textBox_Ysize.Text!="" && textBox_amount.Text!="")
            {
                manager.AddBoxes(double.Parse(textBox_Xsize.Text), double.Parse(textBox_Ysize.Text), int.Parse(textBox_amount.Text));
                ResetTextBoxes();
                textBlock_Result.Text = "The box has been added";
            }
            else
                textBlock_Result.Text = "Please fill all the text boxes";
           
        }
        private void button_buy_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_Xsize.Text != "" && textBox_Ysize.Text != "" && textBox_amount.Text != "")
            {
                success=manager.Buy(double.Parse(textBox_Xsize.Text), double.Parse(textBox_Ysize.Text), int.Parse(textBox_amount.Text));
                ResetTextBoxes();
                if (success)
                    textBlock_Result.Text = "The purchase has been executed";
                else
                    textBlock_Result.Text = "The purchase hasn't been executed";
            }
            else
                textBlock_Result.Text = "Please fill all the text boxes";

        }
        private void button_info_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_Xsize.Text != "" && textBox_Ysize.Text != "")
            {
                textBlock_Result.Text = manager.Info(double.Parse(textBox_Xsize.Text), double.Parse(textBox_Ysize.Text));
            }
            else
            textBlock_Result.Text = "Please fill all the text boxes";
        }
        private void button_Showall_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void ResetTextBoxes()
        {
            textBox_Xsize.Text = "";
            textBox_Ysize.Text = "";
            textBox_amount.Text = "";
        }
        private void OnlyNumbers(object sender, KeyEventArgs e)
        {
           
            //if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            //    e.Handled = false;
            //else
            //    e.Handled = true;
        }

    }
   
}
