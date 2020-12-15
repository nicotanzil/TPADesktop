using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPA_Desktop_NT20_2.Models;

namespace TPA_Desktop_NT20_2.ViewModels.Customer.ATM
{
    class ATMViewModel : ApplicationViewModel
    {
        private IndividualAccount account;

        public IndividualAccount Account
        {
            get { return account; }
            set { account = value; OnPropertyChanged("Account"); }
        }

        public ATMViewModel(IndividualAccount _account)
        {
            Name = "ATM";
            Account = _account;
            SelectedViewModel = new ATMAccountViewModel(Account); 
        }

        protected override void ChangeViewMethod(object parameter)
        {
            UpdateBalance(); 
            if(parameter.ToString() == "ATMAccountView")
            {
                this.SelectedViewModel = new ATMAccountViewModel(Account); 
            }
            else if(parameter.ToString() == "ATMDeposit")
            {
                this.SelectedViewModel = new ATMDepositViewModel(Account); 
            }
            else if(parameter.ToString() == "ATMWithdraw")
            {
                this.SelectedViewModel = new ATMWithdrawViewModel(Account); 
            }
            else if(parameter.ToString() == "ATMTransfer")
            {
                this.SelectedViewModel = new ATMTransferViewModel(Account); 
            }
            else if(parameter.ToString() == "ATMPayment")
            {
                this.SelectedViewModel = new ATMPaymentViewModel(Account); 
            }
        }

        public void UpdateBalance()
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                Account query = (from x in db.Accounts
                                 where x.AccountId == Account.Account.AccountId
                                 select x).FirstOrDefault();

                Account.Account = query; 
            }
        }
    }
}
