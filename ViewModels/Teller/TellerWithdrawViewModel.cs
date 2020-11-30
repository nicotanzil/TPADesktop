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
    class TellerWithdrawViewModel : BaseViewModel
    {
        #region Attributes
        private string message;
        private IndividualAccount account, currentAccount;
        private RelayCommand viewCommand, withdrawCommand;
        private Employee currentEmployee;
        private int amount;
        #endregion

        public TellerWithdrawViewModel(Employee _employee)
        {
            Name = "TellerWithdraw";
            account = new IndividualAccount();
            currentAccount = new IndividualAccount();
            CurrentEmployee = _employee; 
        }

        public IndividualAccount Account
        {
            get { return account; }
            set { account = value; OnPropertyChanged("Account"); }
        }

        public IndividualAccount CurrentAccount
        {
            get { return currentAccount; }
            set { currentAccount = value; OnPropertyChanged("CurrentAccount"); }
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

        private bool CanExecute(object parameter)
        {
            return true;
        }

        private void LoadAccount(object parameter)
        {
            DataTable dt = GetData("SELECT * FROM Account WHERE AccountId = '" + account.AccountId + "'");
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

                currentAccount.AccountId = id;
                currentAccount.Name = name;
                currentAccount.Balance = Convert.ToDouble(balance);
                currentAccount.Email = email;
                Message = "Success!";
            }
        }

        private bool IsAccountExists(string accountId)
        {
            DataTable dt = GetData("SELECT * FROM Account WHERE AccountId = '" + account.AccountId + "'");
            if (IsEmpty(dt)) return false;
            return true;
        }

        private void Withdraw(object parameter)
        {
            //Transaction
            LoadAccount(null);
            if (IsAccountExists(CurrentAccount.AccountId))
            {
                //Account exist
                if (Amount > 0)
                {
                    //Create Transaction Log
                    int count = Count("Transaction");
                    string id = "TR" + IdFormat(count + 1);
                    Double amount = Amount;
                    string trType = "Withdraw";
                    string paymentTypeId = "PA001";

                    Console.WriteLine(id + " " + amount + " " + trType + " " + paymentTypeId);
                    AddTransaction(id, CurrentAccount.AccountId, CurrentEmployee.EmployeeId, paymentTypeId, amount, trType);

                    //Update account balance
                    if (UpdateBalance(CurrentAccount.AccountId))
                    {
                        Console.WriteLine("Withdraw Success!");
                        MessageBox.Show("Withdraw Success!", "Success");
                    }
                    else MessageBox.Show("Insufficient Balance! Minimum IDR20,000 Balance in account", "Error"); 
                }
            }
            LoadAccount(null);
        }

        private void AddTransaction(string transactionId, string accountId, string employeeId, string paymentTypeId, Double amount, string transactionType)
        {
            Console.WriteLine("INSERT INTO [Transaction] VALUES ('" + transactionId + "', '" + accountId + "', '" + employeeId + "', '" + paymentTypeId + "', GETDATE(), " + amount + ", '" + transactionType + "')");
            Execute("INSERT INTO [Transaction] VALUES ('" + transactionId + "', '" + accountId + "', '" + employeeId + "', '" + paymentTypeId + "', " + amount + ", GETDATE(), '" + transactionType + "')");
        }

        private bool UpdateBalance(String _id)
        {
            Double balance = Convert.ToDouble(GetData("SELECT * FROM Account WHERE AccountId = '" + _id + "'").Rows[0]["Balance"]);
            balance -= Amount;
            if(balance > 20000)
            {
                Execute("UPDATE Account SET Balance = " + balance + " WHERE AccountId = '" + _id + "'");
                return true; 
            }
            return false; 
        }
    }
}
