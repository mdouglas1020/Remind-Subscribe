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
using Microsoft.Win32;
using System.IO;

namespace SubscribeRemind
{
    /// <summary>
    /// Interaction logic for AddPage.xaml
    /// </summary>
    public partial class AddPage : Page 
    {
       // SubList list = new SubList();
        public AddPage()
        {
            InitializeComponent();
          

        }

        private void Calendar_SelectedDatesChanged(object sender, RoutedEventArgs e)
        {
            firstChargeTextBox.Text = calender1.SelectedDate.Value.ToString("dd/MM/yyyy");
        }


        private void AddToList_Click(object sender, RoutedEventArgs e)
        {
            

            string name = SubscriptionNameTextBox.Text;
            int renewalType = RenewalTypeComboBox.SelectedIndex;
            string firstCharge = firstChargeTextBox.Text;
            Double amountCharge;
            Double.TryParse(amountTextBox.Text, out amountCharge);
            int reminderReq = reminderComboBox.SelectedIndex;
           
            if(name != "")
            {
                Subscription newSub = new Subscription(name, amountCharge, renewalType, reminderReq, firstCharge);
                SubList.subscriptions.Add(newSub);
               
                newSub.SaveSubscription(newSub);



                Frame pageFrame = null;
                DependencyObject currParent = VisualTreeHelper.GetParent(this);
                while (currParent != null && pageFrame == null)
                {
                    pageFrame = currParent as Frame;
                    currParent = VisualTreeHelper.GetParent(currParent);
                }

                if (pageFrame != null)
                {
                    pageFrame.Source = new Uri("MainPage.xaml", UriKind.Relative);
                }

            }
            else
            {
                MessageBox.Show("Sorry! At the minumum a name is required for your subscription to save!");
            }




        }
    }
}
