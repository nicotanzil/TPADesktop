using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;

namespace TPA_Desktop_NT20_2.ViewModels.Customer.ATM
{
    class ATMDepositViewModel : BaseViewModel
    {
        #region Attributes
        private IndividualAccount account;
        private Decimal amount;
        private RelayCommand depositCommand; 
        private string message;
        #endregion

        public ATMDepositViewModel(IndividualAccount _account)
        {
            Account = _account;
        }

        public IndividualAccount Account
        {
            get { return account; }
            set { account = value; OnPropertyChanged("Account"); }
        }

        public Decimal Amount
        {
            get { return amount; }
            set { amount = value; OnPropertyChanged("Amount"); }
        }


        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged("Message"); }
        }


        public RelayCommand DepositCommand
        {
            get
            {
                depositCommand = new RelayCommand(Deposit, CanExecute);
                return depositCommand;
            }
            set
            {
                depositCommand = value;
                OnPropertyChanged("DepositCommand");
            }
        }

        private bool CanExecute(object parameter)
        {
            return true; 
        }

        private void Deposit(object parameter)
        {
            if (Amount > 0)
            {
                using (KongBuBankEntities db = new KongBuBankEntities())
                {
                    //Add Transaction
                    Transaction transaction = new Transaction();
                    transaction.TransactionId = "TR" + IdFormat(Count("Transaction") + 1);
                    transaction.AccountId = Account.Account.AccountId;
                    transaction.PaymentTypeId = "PA001";
                    transaction.DebitCardId = Account.Account.DebitCardId;
                    transaction.TransactionDate = DateTime.Now;
                    transaction.Amount = Amount;
                    transaction.TransactionType = "Deposit";

                    //Update Balance
                    Account updateAccount = db.Accounts.Find(Account.Account.AccountId);
                    updateAccount.Balance += Amount;

                    db.Transactions.Add(transaction);
                    db.SaveChanges();

                    MessageBox.Show("Deposit Success!", "Success");
                }
            }
            else MessageBox.Show("Invalid Amount!", "Error"); 
        }
    }
}
