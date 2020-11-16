using System;
using System.ComponentModel; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPA_Desktop_NT20_2.Models
{
    class Account : INotifyPropertyChanged
    {
        private string accountId;

        public String AccountId
        {
            get { return accountId; }
            set 
            { 
                accountId = value;
                OnPropertyChanged("AccountId"); 
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set 
            {
                name = value;
                OnPropertyChanged("Name"); 
            }
        }

        private int balance;

        public int Balance
        {
            get { return balance; }
            set 
            {
                balance = value;
                OnPropertyChanged("Balance"); 
            }
        }

        private DateTime dob;


        public DateTime Dob
        {
            get { return dob; }
            set 
            {
                dob = value;
                OnPropertyChanged("Dob"); 
            }
        }

        private void OnPropertyChanged(string property)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property)); 
            }
        }     

        public event PropertyChangedEventHandler PropertyChanged;


    }
}
