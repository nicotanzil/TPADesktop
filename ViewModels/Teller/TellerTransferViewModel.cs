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
        private IndividualAccount account, targetAccount; 
        private RelayCommand viewCommand, transferCommand;
        private Employee currentEmployee;
        private int amount;
        #endregion


        public TellerTransferViewModel(Employee _employee)
        {
            Name = "TellerTransfer";
            account = new IndividualAccount();
            targetAccount = new IndividualAccount(); 
            CurrentEmployee = _employee; 
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

        public int Amount
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
            DataTable dt = GetData("SELECT * FROM Account WHERE AccountId = '" + Account.AccountId + "'");
            if (IsEmpty(dt))
            {
                //Account not found
                Message = "Account Not Found!";
                MessageBox.Show("Account Not Found!", "Invalid Data");
            }
            else
            {
                int index = 0;
                string id = dt.Rows[index]["AccountId"].ToString();
                string name = dt.Rows[index]["Name"].ToString();
                string balance = dt.Rows[index]["Balance"].ToString();
                string email = dt.Rows[index]["Email"].ToString();

                Account.AccountId = id;
                Account.Name = name;
                Account.Balance = Convert.ToDouble(balance);
                Account.Email = email;
                Message = "Success!";
            }

            dt = GetData("SELECT * FROM Account WHERE AccountId = '" + TargetAccount.AccountId + "'");
            if(IsEmpty(dt))
            {
                Message = "Transfer Account Not Found!";
                MessageBox.Show("Transfer Account Not Found!", "Invalid Data");
            }
            else
            {
                int index = 0;
                string id = dt.Rows[index]["AccountId"].ToString();
                string name = dt.Rows[index]["Name"].ToString();
                string balance = dt.Rows[index]["Balance"].ToString();
                string email = dt.Rows[index]["Email"].ToString();

                TargetAccount.AccountId = id;
                TargetAccount.Name = name;
                TargetAccount.Balance = Convert.ToDouble(balance);
                TargetAccount.Email = email;
                Message = "Success!";
            }
        }

        private void Transfer(object parameter)
        {
            LoadAccount(null); 
            if(IsAccountExists(Account.AccountId) && IsAccountExists(TargetAccount.AccountId))
            {
                if(Amount > 0)
                {
                    //Generate transaction data send
                    int count = Count("Transaction");
                    string sendId = "TR" + IdFormat(count + 1);
                    string sendTrType = "Send Transfer";
                    string sendPaymentTypeId = "PA001";
                    string debitCard = GetDebitCard(Account.AccountId);

                    //Generate transaction data receive
                    string recvId = "TR" + IdFormat(count + 2);
                    string recvTrType = "Receive Transfer";
                    string recvPaymentTypeId = "PA001";

                    if(UpdateBalance(Account.AccountId, (-1)*Amount))
                    {
                        //Update TargetAccount balance
                        UpdateBalance(TargetAccount.AccountId, Amount);
                        //Add Send Transaction
                        AddTransaction(sendId, Account.AccountId, targetAccount.AccountId, 
                            CurrentEmployee.EmployeeId, sendPaymentTypeId, debitCard, Amount, sendTrType);
                        //Add Receive Transaction
                        AddTransaction(recvId, TargetAccount.AccountId, Account.AccountId,
                            CurrentEmployee.EmployeeId, recvPaymentTypeId, Amount, recvTrType);
                        MessageBox.Show("Transfer Success!", "Success");
                    }
                    else MessageBox.Show("Insufficient Balance! Minimum IDR20,000 Balance in account", "Error");
                }
            }
            LoadAccount(null); 
        }

        private bool IsAccountExists(string accountId)
        {
            DataTable dt = GetData("SELECT * FROM Account WHERE AccountId = '" + account.AccountId + "'");
            if (IsEmpty(dt)) return false;
            return true;
        }

        private void AddTransaction(string transactionId, string accountId, string targetId, string employeeId, string paymentTypeId, string debitCardId, Double amount, string transactionType)
        {
            Console.WriteLine("INSERT INTO [Transaction] VALUES ('" + transactionId + "', '" + accountId + "', '" + targetId + "', '" + employeeId + "', '" + paymentTypeId + "', '" + debitCardId + "', NULL, " + amount + ", GETDATE(), '" + transactionType + "')");
            Execute("INSERT INTO [Transaction] VALUES ('" + transactionId + "', '" + accountId + "', '" + targetId + "', '" + employeeId + "', '" + paymentTypeId + "', '" + debitCardId + "', NULL, " + amount + ", GETDATE(), '" + transactionType + "')");
        }

        private void AddTransaction(string transactionId, string accountId, string targetId, string employeeId, string paymentTypeId, Double amount, string transactionType)
        {
            Console.WriteLine("INSERT INTO [Transaction] VALUES ('" + transactionId + "', '" + accountId + "', '" + targetId + "', '" + employeeId + "', '" + paymentTypeId + "', NULL, NULL, " + amount + ", GETDATE(), '" + transactionType + "')");
            Execute("INSERT INTO [Transaction] VALUES ('" + transactionId + "', '" + accountId + "', '" + targetId + "', '" + employeeId + "', '" + paymentTypeId + "', NULL, NULL, " + amount + ", GETDATE(), '" + transactionType + "')");
        }

        private bool UpdateBalance(String _id, Double amount)
        {
            Double balance = Convert.ToDouble(GetData("SELECT * FROM Account WHERE AccountId = '" + _id + "'").Rows[0]["Balance"]);
            balance += amount;
            if (balance > 20000)
            {
                Execute("UPDATE Account SET Balance = " + balance + " WHERE AccountId = '" + _id + "'");
                return true;
            }
            return false;
        }

        private string GetDebitCard(string _id)
        {
            return GetData("SELECT * FROM DebitCard WHERE AccountId = '" + _id + "'").Rows[0]["CardId"].ToString();
        }
    }
}
