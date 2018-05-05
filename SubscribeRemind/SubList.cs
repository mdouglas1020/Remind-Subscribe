using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SubscribeRemind
{
    public static class SubList
    {

        public static List<Subscription> subscriptions = new List<Subscription>();
        public static Boolean canAdd = false;
     

        public static void PopulateList()
        {
            List<SubListString> items = new List<SubListString>();
            foreach (Subscription sub in SubList.subscriptions)
            {

                DateTime dateOfRenewal = new DateTime();
                DateTime dateOfReminder = new DateTime();

                string renewalString = "";
                string reminderString = "";

                switch (sub.renewalType)
                {
                    case 0:
                        {

                            renewalString = "Daily";
                            break;
                        }
                    case 1:
                        {
                            DateTime today = DateTime.Now;

                            dateOfRenewal = sub.firstChargeDT.AddMonths(1);


                            renewalString = "Monthly";
                            break;
                        }
                    case 2:
                        {
                            DateTime today = DateTime.Now;

                            dateOfRenewal = sub.firstChargeDT.AddYears(1);

                            renewalString = "Yearly";
                            break;
                        }
                }


                switch (sub.reminderReq)
                {
                    case 0: //24 hours before
                        {

                            DateTime today = DateTime.Now;
                            TimeSpan req = new TimeSpan(0, 24, 0, 0);

                            switch (sub.renewalType)
                            {
                                case 0: // Daily billing
                                    {
                                        TimeSpan duration = new TimeSpan(1, 0, 0, 0);
                                        dateOfRenewal = sub.firstChargeDT + duration;
                                        dateOfReminder = dateOfRenewal - req;
                                        sub.dateOfReminder = dateOfReminder;
                                        break;
                                    }
                                case 1: // Monthly billing
                                    {

                                        dateOfRenewal = sub.firstChargeDT.AddMonths(1);
                                        dateOfReminder = dateOfRenewal - req;
                                        sub.dateOfReminder = dateOfReminder;
                                        break;
                                    }
                                case 2: //yearly building
                                    {

                                        dateOfRenewal = sub.firstChargeDT.AddYears(1);
                                        dateOfReminder = dateOfRenewal - req;
                                        sub.dateOfReminder = dateOfReminder;
                                        break;
                                    }

                            }



                            reminderString = "24 Hours";
                            break;
                        }
                    case 1: // 48 Hours before 
                        {

                            DateTime today = DateTime.Now;
                            TimeSpan req = new TimeSpan(0, 48, 0, 0);

                            switch (sub.renewalType)
                            {
                                case 0: // DONT THINK THIS CASE IS POSSIBLE TEST TOMORROW
                                    {

                                        break;
                                    }
                                case 1: //Monthly billing 48 hours before
                                    {
                                        dateOfRenewal = sub.firstChargeDT.AddMonths(1);
                                        dateOfReminder = dateOfRenewal - req;
                                        sub.dateOfReminder = dateOfReminder;
                                        break;
                                    }
                                case 2: //Yearly billing 48 hours before
                                    {
                                        dateOfRenewal = sub.firstChargeDT.AddYears(1);
                                        dateOfReminder = dateOfRenewal - req;
                                        sub.dateOfReminder = dateOfReminder;
                                        break;
                                    }
                            }
                            reminderString = "48 Hours";
                            break;
                        }
                    case 2: //One week reminder
                        {
                            TimeSpan req = new TimeSpan(0, 168, 0, 0);


                            switch (sub.renewalType)
                            {
                                case 0: // daily Not possible
                                    {

                                        break;
                                    }
                                case 1: // Monthly billing one week reminder
                                    {
                                        dateOfRenewal = sub.firstChargeDT.AddMonths(1);
                                        dateOfReminder = dateOfRenewal - req;
                                        sub.dateOfReminder = dateOfReminder;
                                        break;
                                    }
                                case 2: // Yearly billing, one week before
                                    {
                                        dateOfRenewal = sub.firstChargeDT.AddYears(1);
                                        dateOfReminder = dateOfRenewal - req;
                                        sub.dateOfReminder = dateOfReminder;
                                        break;
                                    }
                            }
                            reminderString = "One Week";
                            break;
                        }
                }


                items.Add(new SubListString() { Company = sub.name, ReminderType = reminderString, RenewalType = renewalString, Amount = "$" + sub.amount.ToString(), FirstCharge = sub.firstCharge.ToString(), NextCharge = dateOfRenewal.ToString("dd/MM/yyyy"), ReminderDate = dateOfReminder.ToString("dd/MM/yyyy") });


                
                

            }
          
        }
    }
}