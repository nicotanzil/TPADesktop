using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;
using TPA_Desktop_NT20_2.Views.Customer.ATM;

namespace TPA_Desktop_NT20_2.ViewModels.Customer
{
    class ATMAuthenticationViewModel : BaseViewModel
    {
        #region Attributes
        private string message;
        private RelayCommand loginCommand;
        private Account tempAccount; 
        private IndividualAccount account;
        #endregion

        public ATMAuthenticationViewModel()
        {
            Name = "ATMAuthentication";
            tempAccount = new Account();
            account = new IndividualAccount();

            Account.Account = tempAccount; 
        }

        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged("Message");  }
        }

        public IndividualAccount Account
        {
            get { return account; }
            set { account = value; OnPropertyChanged("Account");  }
        }

        public RelayCommand LoginCommand
        {
            get
            {
                loginCommand = new RelayCommand(LoadData, CanExecute);
                return loginCommand;
            }
            set
            {
                loginCommand = value;
                OnPropertyChanged("LoginCommand");
            }
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

        private void LoadData(object parameter)
        {
            KongBuBankEntities db = KongBuBankEntities.Instance;
            Account query = (from x in db.Accounts
                                where x.DebitCardId == Account.Account.DebitCardId
                                where x.PIN == Account.Account.PIN
                                select x).FirstOrDefault();

            if(query == null)
            {
                //Account Not Found
                Message = "Account Not Found!";
                MessageBox.Show("Invalid Card / PIN", "Error"); 
            }
            else
            {
                IndividualAccount temp = new IndividualAccount();
                temp.Account = query;
                Account = temp;

                Console.WriteLine("Authentication Success: " + Account.Account.Name);
                Message = "Success!";

                RedirectPage(); 
            }
        }

        private void RedirectPage()
        {
            //GOTO ATM Menu
            ATMWindow atmWin = new ATMWindow(Account);
            atmWin.ShowDialog(); 
        }
    }
}
