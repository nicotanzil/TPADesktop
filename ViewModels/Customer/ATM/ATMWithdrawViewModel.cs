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
    class ATMWithdrawViewModel : BaseViewModel
    {
        #region Attributes
        private IndividualAccount account;
        private Decimal amount;
        private RelayCommand withdrawCommand;
        private string message;
        #endregion

        public ATMWithdrawViewModel(IndividualAccount _account)
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


        public RelayCommand WithdrawCommand
        {
            get
            {
                withdrawCommand = new RelayCommand(Withdraw, CanExecute);
                return withdrawCommand;
            }
            set
            {
                withdrawCommand = value;
                OnPropertyChanged("WithdrawCommand");
            }
        }


        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged("Message"); }
        }

        public bool CanExecute(object parameter)
        {
            return true; 
        }

        private void Withdraw(object parameter)
        {
            if (Amount > 0)
            {
                if(ValidateAccountBalance(Account.Account.AccountId, Amount))
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
                        transaction.TransactionType = "Withdraw";

                        //Update Balance
                        Account updateAccount = db.Accounts.Find(Account.Account.AccountId);
                        updateAccount.Balance -= Amount;

                        db.Transactions.Add(transaction);
                        db.SaveChanges();

                        MessageBox.Show("Withdraw Success!", "Success");
                    }
                }
                else MessageBox.Show("Insufficient Balance! Minimum IDR20,000 Balance in account", "Error");
            }
            else MessageBox.Show("Invalid Amount!", "Error"); 
        }

        private bool ValidateAccountBalance(string _id, Decimal amount)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                Account checkAccount = db.Accounts.Find(_id);
                return checkAccount.Balance - amount < 20000 ? false : true; 
            }
        }
    }
}
