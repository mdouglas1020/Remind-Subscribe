using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace SubscribeRemind
{
    public class Subscription
    {


        public string name;
        public double amount;
        public int renewalType;
        public int reminderReq;
        public string firstCharge;
        public static bool alreadyChecked = false;
        public DateTime firstChargeDT = new DateTime();
        public DateTime dateOfReminder = new DateTime();
        public int deleted = 0;
        public Subscription()
        {

        }
        public Subscription(string name, double amount, int renewalType, int reminderReq, string firstCharge, int deleted)
        {
            this.name = name;
            this.amount = amount;
            this.renewalType = renewalType;
            this.reminderReq = reminderReq;
            this.firstCharge = firstCharge;
            firstChargeDT = DateTime.ParseExact(this.firstCharge, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            this.deleted = deleted;
        }
        public Subscription(string name, double amount, int renewalType, int reminderReq, string firstCharge)
        {
            this.name = name;
            this.amount = amount;
            this.renewalType = renewalType;
            this.reminderReq = reminderReq;
            this.firstCharge = firstCharge;
            firstChargeDT = DateTime.ParseExact(this.firstCharge, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
        public static bool AlreadyChecked
        {
            get
            {
                return alreadyChecked;
            }
        }
        public Boolean SaveSubscription(Subscription newSub)
        {
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "SavedSubscriptions.txt");
            try
            {
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    
                        string line = newSub.name + "," + newSub.amount.ToString() + "," + newSub.renewalType.ToString() + "," + newSub.reminderReq.ToString() + "," + newSub.firstCharge.ToString() + "," + newSub.deleted.ToString();
                        sw.WriteLine(line);
                  
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Boolean LoadSubscriptions()
        {
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "SavedSubscriptions.txt");
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line = "";
                    

                    while((line = sr.ReadLine()) != null)
                    {
                        String[] words = line.Split(',');

                        SubList.subscriptions.Add(new Subscription(words[0], double.Parse(words[1]), int.Parse(words[2]), int.Parse(words[3]), words[4], int.Parse(words[5])));


                    }
                   
                }
                
                alreadyChecked = true;
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

    }     
}
