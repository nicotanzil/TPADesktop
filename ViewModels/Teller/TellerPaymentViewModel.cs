using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;

namespace TPA_Desktop_NT20_2.ViewModels.Teller
{
    class TellerPaymentViewModel : BaseViewModel
    {
        #region Attributes
        private string message;
        private IndividualAccount account;
        private RelayCommand viewCommand, paymentCommand;
        private Employee currentEmployee;
        private int amount;
        private CollectionView paymentTypes;
        private string paymentType;
        #endregion

        public TellerPaymentViewModel(Employee _employee)
        {
            Name = "TellerPayment";
            account = new IndividualAccount();
            CurrentEmployee = _employee; 
            IList<string> list = new List<string>();
            list.Add("Electricity");
            list.Add("Water");
            paymentTypes = new CollectionView(list); 
        }

        public IndividualAccount Account
        {
            get { return account; }
            set { account = value; OnPropertyChanged("Account"); }
        }

        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }


        public CollectionView PaymentTypes
        {
            get { return paymentTypes; }
        }

        public string PaymentType
        {
            get { return paymentType; }
            set { paymentType = value; OnPropertyChanged("PaymentType"); }
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

        public RelayCommand PaymentCommand
        {
            get
            {
                paymentCommand = new RelayCommand(Payment, CanExecute);
                return paymentCommand;
            }
            set
            {
                paymentCommand = value;
                OnPropertyChanged("PaymentCommand");
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

                Account.AccountId = id;
                Account.Name = name;
                Account.Balance = Convert.ToDouble(balance);
                Account.Email = email;
                Message = "Success!";
            }
        }

        private void Payment(object parameter)
        {
            LoadAccount(null); 
            if(IsAccountExists(Account.AccountId))
            {
                //Account exists
                if(Amount > 0)
                {
                    //Generate transaction data
                    int count = Count("Transaction");
                    string id = "TR" + IdFormat(count + 1);
                    Double amount = Amount;
                    string trType = PaymentType; 
                    string paymentTypeId = "PA001";
                    string debitCard = GetDebitCard(Account.AccountId);

                    Console.WriteLine(id + " " + amount + " " + trType + " " + paymentTypeId);

                    Console.WriteLine("Payment Type: " + PaymentType); 
                    //Validate combobox
                    if(PaymentType == null)
                    {
                        MessageBox.Show("Payment Type must be choosed!");
                        return; 
                    }

                    //Update account balance
                    if (UpdateBalance(Account.AccountId))
                    {
                        Console.WriteLine("Payment Success!");
                        //Create Transaction Log
                        AddTransaction(id, Account.AccountId, CurrentEmployee.EmployeeId, paymentTypeId, debitCard, amount, trType);
                        MessageBox.Show("Payment: " + PaymentType + " Success!", "Success");
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

        private void AddTransaction(string transactionId, string accountId, string employeeId, string paymentTypeId, string debitCardId, Double amount, string transactionType)
        {
            Console.WriteLine("INSERT INTO [Transaction] VALUES ('" + transactionId + "', '" + accountId + "', NULL, '" + employeeId + "', '" + paymentTypeId + "', '" + debitCardId + "', NULL, " + amount + ", GETDATE(), '" + transactionType + "')");
            Execute("INSERT INTO [Transaction] VALUES ('" + transactionId + "', '" + accountId + "', NULL, '" + employeeId + "', '" + paymentTypeId + "', '" + debitCardId + "', NULL, " + amount + ", GETDATE(), '" + transactionType + "')");
        }

        private bool UpdateBalance(String _id)
        {
            Double balance = Convert.ToDouble(GetData("SELECT * FROM Account WHERE AccountId = '" + _id + "'").Rows[0]["Balance"]);
            balance -= Amount;
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
