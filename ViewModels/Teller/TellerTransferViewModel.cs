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
    class TellerTransferViewModel : BaseViewModel
    {
        #region Attributes
        private string message;
        private Account tempAccount, tempAccountTarget; 
        private IndividualAccount account, targetAccount; 
        private RelayCommand viewCommand, transferCommand;
        private Employee currentEmployee;
        private Decimal amount;
        #endregion


        public TellerTransferViewModel(Employee _employee)
        {
            Name = "TellerTransfer";
            tempAccount = new Account();
            tempAccountTarget = new Account(); 
            account = new IndividualAccount();
            targetAccount = new IndividualAccount();
            CurrentEmployee = _employee;

            Account.Account = tempAccount;
            TargetAccount.Account = tempAccountTarget; 
        }

        public IndividualAccount Account
        {
            get { return account; }
            set { account = value; OnPropertyChanged("Account"); }
        }


        public IndividualAccount TargetAccount
        {
            get { return targetAccount; }
            set { targetAccount = value; OnPropertyChanged("TargetAccount"); }
        }


        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        public Decimal Amount
        {
            get { return amount; }
            set { amount = value; OnPropertyChanged("Amount"); }
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

        public RelayCommand TransferCommand
        {
            get
            {
                transferCommand = new RelayCommand(Transfer, CanExecute);
                return transferCommand;
            }
            set
            {
                transferCommand = value;
                OnPropertyChanged("TransferCommand");
            }
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

        private void LoadAccount(object parameter)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                Account query = (from x in db.Accounts
                                 where x.AccountId == Account.Account.AccountId
                                 select x).FirstOrDefault(); 
                if(query == null)
                {
                    Message = "Account Not Found!";
                    MessageBox.Show("Account Not Found!", "Error");
                    return; 
                }
                else
                {
                    IndividualAccount temp = new IndividualAccount();
                    temp.Account = query;
                    Account = temp; 
                }

                query = (from x in db.Accounts
                         where x.AccountId == TargetAccount.Account.AccountId
                         select x).FirstOrDefault();

                if (query == null)
                {
                    Message = "Account Not Found!";
                    MessageBox.Show("Account Not Found!", "Error");
                    return;
                }
                else
                {
                    IndividualAccount temp = new IndividualAccount();
                    temp.Account = query;
                    TargetAccount = temp;
                }
            }
        }

        private void Transfer(object parameter)
        {
            LoadAccount(null); 
            if (IsAccountExists(Account.Account.AccountId) && 
                IsAccountExists(TargetAccount.Account.AccountId) && 
                Account.Account.AccountId != TargetAccount.Account.AccountId)
            {
                if (Amount > 0)
                {
                    if(ValidateAccountBalance(Account.Account.AccountId, Amount))
                    {
                        using (KongBuBankEntities db = new KongBuBankEntities())
                        {
                            //Add Transaction (Send)
                            Transaction sendTransaction = new Transaction();
                            sendTransaction.TransactionId = "TR" + IdFormat(Count("Transaction") + 1);
                            sendTransaction.AccountId = Account.Account.AccountId;
                            sendTransaction.RelatedAccountId = TargetAccount.Account.AccountId;
                            sendTransaction.EmployeeId = CurrentEmployee.EmployeeId;
                            sendTransaction.PaymentTypeId = "PA001";
                            sendTransaction.DebitCardId = Account.Account.DebitCardId;
                            sendTransaction.TransactionDate = DateTime.Now;
                            sendTransaction.Amount = Amount;
                            sendTransaction.TransactionType = "Send Transfer";

                            //Add Transaction (Receive)
                            Transaction receiveTransaction = new Transaction();
                            receiveTransaction.TransactionId = "TR" + IdFormat(Count("Transaction") + 2);
                            receiveTransaction.AccountId = TargetAccount.Account.AccountId;
                            receiveTransaction.RelatedAccountId = Account.Account.AccountId;
                            receiveTransaction.EmployeeId = CurrentEmployee.EmployeeId;
                            receiveTransaction.PaymentTypeId = "PA001";
                            receiveTransaction.DebitCardId = TargetAccount.Account.DebitCardId;
                            receiveTransaction.TransactionDate = DateTime.Now;
                            receiveTransaction.Amount = Amount;
                            receiveTransaction.TransactionType = "Receive Transaction";

                            //Update Balance
                            Account updateAccount = db.Accounts.Find(TargetAccount.Account.AccountId);
                            updateAccount.Balance += Amount;

                            updateAccount = db.Accounts.Find(Account.Account.AccountId);
                            updateAccount.Balance -= Amount;

                            db.Transactions.Add(sendTransaction);
                            db.Transactions.Add(receiveTransaction);
                            db.SaveChanges();

                            MessageBox.Show("Transfer to " + TargetAccount.Account.AccountId + " Success!", "Success");
                            LoadAccount(null);
                        }
                    }
                    else MessageBox.Show("Insufficient Balance! Minimum IDR20,000 Balance in account", "Error");
                }
                else MessageBox.Show("Invalid Amount!", "Error"); 
            }
            else MessageBox.Show("Invalid Account!", "Error");
            LoadAccount(null); 
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
