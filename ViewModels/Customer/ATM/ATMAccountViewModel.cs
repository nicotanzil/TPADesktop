using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPA_Desktop_NT20_2.Models;

namespace TPA_Desktop_NT20_2.ViewModels.Customer.ATM
{
    class ATMAccountViewModel : BaseViewModel
    {
        private IndividualAccount account;

        public ATMAccountViewModel(IndividualAccount _account)
        {
            Account = _account; 
        }

        public IndividualAccount Account
        {
            get { return account; }
            set { account = value; OnPropertyChanged("Account"); }
        }
    }
}
