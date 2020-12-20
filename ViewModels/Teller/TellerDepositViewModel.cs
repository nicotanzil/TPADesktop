using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;

namespace TPA_Desktop_NT20_2.ViewModels.Teller
{
    public class TellerDepositViewModel : BaseViewModel
    {
        #region Attributes
        private string message;
        private Account tempAccount; 
        private IndividualAccount account;
        private RelayCommand viewCommand, depositCommand;
        private Employee currentEmployee;
        private Decimal amount; 
        #endregion

        public TellerDepositViewModel(Employee _employee)
        {
            Name = "TellerDeposit";
            tempAccount = new Account(); 
            account = new IndividualAccount();
            CurrentEmployee = _employee;

            Account.Account = tempAccount; 
        }

        public IndividualAccount Account
        {
            get { return account; }
            set { account = value; OnPropertyChanged("Account"); }
        }

        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee");  }
        }

        public Decimal Amount
        {
            get { return amount; }
            set { amount = value; OnPropertyChanged("Amount");  }
        }


        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }

        public RelayCommand ViewCommand
        {
            get
            {
                viewCommand = new RelayCommand(LoadAccount, CanExecute);
                return viewCommand;
            }
            set
            {
                viewCommand = value;
                OnPropertyChanged("ViewCommand");
            }
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

        private void LoadAccount(object parameter)
        {
            using(KongBuBankEntities db = new KongBuBankEntities())
            {
                Account query = (from x in db.Accounts 
                                 where x.AccountId == Account.Account.AccountId 
                                 select x).FirstOrDefault(); 
                if(query == null)
                {
                    //Account not found
                    Message = "Account Not Found!";
                    MessageBox.Show(Message, "Invalid Data");
                }
                else
                {
                    IndividualAccount temp = new IndividualAccount();
                    temp.Account = query;
                    Account = temp;
                }
            }
        }

        private void Deposit(object parameter)
        {
            //Transaction
            if (IsAccountExists(Account.Account.AccountId))
            {
                if (Amount > 0)
                {
                    using (KongBuBankEntities db = new KongBuBankEntities())
                    {
                        //Add Transaction
                        Transaction transaction = new Transaction();
                        transaction.TransactionId = "TR" + IdFormat(Count("Transaction") + 1);
                        transaction.AccountId = Account.Account.AccountId;
                        transaction.EmployeeId = CurrentEmployee.EmployeeId;
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
                        LoadAccount(null);
                    }
                }
                else MessageBox.Show("Invalid amount!", "Error"); 
            }
            else MessageBox.Show("Account not found!", "Error"); 
        }

        private bool IsAccountExists(string accountId)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                Account query = (from x in db.Accounts
                                 where x.AccountId == accountId
                                 select x).FirstOrDefault();
                return query != null ? true : false;
            }
        }
    }
}
