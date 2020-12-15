using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;

namespace TPA_Desktop_NT20_2.ViewModels.Customer.ATM
{
    class ATMTransferViewModel : BaseViewModel
    {
        #region Attributes
        private Account tempAccount; 
        private IndividualAccount account, targetAccount;
        private Decimal amount;
        private RelayCommand transferCommand; 
        private string message;

        #endregion

        public ATMTransferViewModel(IndividualAccount _account)
        {
            Account = _account;
            tempAccount = new Account();
            targetAccount = new IndividualAccount();

            targetAccount.Account = tempAccount; 
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


        public IndividualAccount TargetAccount
        {
            get { return targetAccount; }
            set { targetAccount = value; OnPropertyChanged("TargetAccount"); }
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


        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged("Message"); }
        }


        private bool CanExecute(object parameter)
        {
            return true; 
        }

        private void LoadAccount()
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                Account query = (from x in db.Accounts
                                 where x.DebitCardId == TargetAccount.Account.DebitCardId
                                 select x).FirstOrDefault();

                if (query != null)
                {
                    IndividualAccount temp = new IndividualAccount();
                    temp.Account = query;
                    TargetAccount = temp;
                }
            }
        }

        private void Transfer(object parameter)
        {
            LoadAccount();
            if (IsAccountExists(TargetAccount.Account.DebitCardId) && 
                Account.Account.DebitCardId != TargetAccount.Account.DebitCardId)
            {
                if (Amount > 0)
                {
                    if (ValidateAccountBalance(Account.Account.AccountId, Amount))
                    {
                        using (KongBuBankEntities db = new KongBuBankEntities())
                        {
                            //Add Transaction (Send)
                            Transaction sendTransaction = new Transaction();
                            sendTransaction.TransactionId = "TR" + IdFormat(Count("Transaction") + 1);
                            sendTransaction.AccountId = Account.Account.AccountId;
                            sendTransaction.RelatedAccountId = TargetAccount.Account.AccountId;
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

                            MessageBox.Show("Transfer to " + TargetAccount.Account.DebitCardId + " Success!", "Success");
                        }
                    }
                    else MessageBox.Show("Insufficient Balance! Minimum IDR20,000 Balance in account", "Error");
                }
                else MessageBox.Show("Invalid Amount!", "Error"); 
            }
            else MessageBox.Show("Invalid Account! Please input the correct card id", "Error"); 
        }

        private bool IsAccountExists(string debitId)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                Account query = (from x in db.Accounts
                                 where x.DebitCardId == debitId
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
