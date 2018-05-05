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
using System.IO;
using System.Windows.Threading;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;

namespace SubscribeRemind
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        string path = System.IO.Path.Combine(Environment.CurrentDirectory, "SavedSubscriptions.txt");
        public static int position = 0;
       
        private DispatcherTimer timer;
        public MainPage()
        {
            InitializeComponent();

            if (File.Exists(path))
            {
                Subscription temp = new Subscription();
        
                if (Subscription.AlreadyChecked == false)
                {
                    
                    temp.LoadSubscriptions();
                }
             
            }
           
             SubList.PopulateList();
            
            
             CountdownFunction(position);
            
           

            








        }

    
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Frame pageframe = null;
            DependencyObject currparent = VisualTreeHelper.GetParent(this);
            while (currparent != null && pageframe == null)
            {
                pageframe = currparent as Frame;
                currparent = VisualTreeHelper.GetParent(currparent);
            }

            if (pageframe != null)
            {
                pageframe.Source = new Uri("AddPage.xaml", UriKind.Relative);
            }


        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            Frame pageframe = null;
            DependencyObject currparent = VisualTreeHelper.GetParent(this);
            while (currparent != null && pageframe == null)
            {
                pageframe = currparent as Frame;
                currparent = VisualTreeHelper.GetParent(currparent);
            }

            if (pageframe != null)
            {
                pageframe.Source = new Uri("ViewSubPage.xaml", UriKind.Relative);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            //HERE remove and unsave subscription!
        }

        private void CycleButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            if (position >= SubList.subscriptions.Count)
            {
                position = 0;
            }
            else
            {
                position++;
            }
            
            CountdownFunction(position);
            

        }

        private void CountdownFunction(int position)
        {
           if(position == SubList.subscriptions.Count)
            {
                position = 0;
            }

            timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                //get subscripotion remind date from subscription may need to move creation place of remind date

                DateTime reminderDate = SubList.subscriptions[position].dateOfReminder;
                TimeSpan ts = reminderDate - DateTime.Now;
                this.countdownText.Text = SubList.subscriptions[position].name + ": " + string.Format("{0} Days, {1} Hours, {2} Minutes, {3} Seconds", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
            }, this.Dispatcher);
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Notifier notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.BottomRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });

            notifier.ShowWarning("HELLO TESTING!");
        }
    }
}
