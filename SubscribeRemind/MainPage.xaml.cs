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
        public static int Position { get; set; }
        public static int pos;
        private DispatcherTimer testTimer = new DispatcherTimer();
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

            if(SubList.subscriptions.Count != 0)
            {
                CountdownFunction(0);
            }
            TimeSpan add = new TimeSpan(0, 0, 10);
            DateTime test = DateTime.Now + add;
      
            
          
            SubList.PopulateList();

            //SubList.subscriptions[0].dateOfReminder = test;


            

            testTimer.Tick += new EventHandler(TestTimer_Click);

            testTimer.Interval = new TimeSpan(0, 0, 1);
            testTimer.Start();








        }
        
        private void TestTimer_Click(object sender, EventArgs e)
        {
            DateTime now;
            now = DateTime.Now.Date;
            TimeSpan dif;
            foreach (Subscription sub in SubList.subscriptions.ToList())
            {
                TimeSpan zero = new TimeSpan(0,0,0);
                if(sub.dateOfReminder - DateTime.Now >=  zero)
                {
                     dif = sub.dateOfReminder - DateTime.Now;
                }
                else
                {
                    dif = new TimeSpan(0, 0, 0);
                }
               
                if(DateTime.Now.Date == sub.dateOfReminder.Date && dif.Seconds == 0)
                {
                    
                    Notifier notifier = new Notifier(cfg =>
                    {
                        // Sound alert, and remove feature possible removed int added
                        cfg.PositionProvider = new WindowPositionProvider(
                            parentWindow: Application.Current.MainWindow,
                            corner: Corner.BottomRight,
                            offsetX: 10,
                            offsetY: 10);

                        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                            notificationLifetime: TimeSpan.FromSeconds(250),
                            maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                        cfg.Dispatcher = Application.Current.Dispatcher;
                    });
                    string message = "ATTENTION! Here is your reminder for your " + sub.name + " " + "subscription!";
                    notifier.ShowWarning(message);
                    sub.deleted = 1;
                    SubList.subscriptions.Remove(sub);
                    System.Media.SystemSounds.Exclamation.Play();
                }
                
            }
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
            Frame pageframe = null;
            DependencyObject currparent = VisualTreeHelper.GetParent(this);
            while (currparent != null && pageframe == null)
            {
                pageframe = currparent as Frame;
                currparent = VisualTreeHelper.GetParent(currparent);
            }

            if (pageframe != null)
            {
                pageframe.Source = new Uri("RemovePage.xaml", UriKind.Relative);
            }
        }

        private void CycleButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            if (pos >= SubList.subscriptions.Count)
            {
                pos = 0;
            }
            else
            {
                pos++;
            }
            
            CountdownFunction(pos);
            

        }

        private void CountdownFunction(int position)
        {
           if(position >= SubList.subscriptions.Count)
            {
                position = 0;
            }

            timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                //get subscripotion remind date from subscription may need to move creation place of remind date

                DateTime reminderDate = SubList.subscriptions[position].dateOfReminder;
                TimeSpan ts = reminderDate - DateTime.Now;
                if(ts.Seconds <= 0 && reminderDate.DayOfYear == DateTime.Now.DayOfYear)
                {
                    
                    this.countdownText.Text = SubList.subscriptions[position].name + ": " + string.Format("0 Days, 0 Hours, 0 Minutes, 0 Seconds");
                    timer.Stop();
                }
                this.countdownText.Text = SubList.subscriptions[position].name + ": " + string.Format("{0} Days, {1} Hours, {2} Minutes, {3} Seconds", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
            }, this.Dispatcher);
           
        }

       
  
    }
}
