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
        private Account tempAccount; 
        private IndividualAccount account;
        private RelayCommand viewCommand, paymentCommand;
        private Employee currentEmployee;
        private Decimal amount;
        private CollectionView paymentTypes;
        private string paymentType;
        #endregion

        public TellerPaymentViewModel(Employee _employee)
        {
            Name = "TellerPayment";
            tempAccount = new Account(); 
            account = new IndividualAccount();
            CurrentEmployee = _employee;

            Account.Account = tempAccount; 

            //ComboBox
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
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                Account query = (from x in db.Accounts
                                 where x.AccountId == Account.Account.AccountId
                                 select x).FirstOrDefault();
                if (query == null)
                {
                    //Account not found
                    Message = "Account Not Found!";
                    MessageBox.Show("Account Not Found!", "Error");
                }
                else
                {
                    IndividualAccount temp = new IndividualAccount();
                    temp.Account = query;
                    Account = temp;
                }
            }
        }

        private void Payment(object parameter)
        {
            LoadAccount(null); 
            if(IsAccountExists(Account.Account.AccountId))
            {
                //Account exists
                if(Amount > 0)
                {
                    if(ValidateAccountBalance(Account.Account.AccountId, Amount))
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

                            //Validate combobox
                            if(PaymentType == null)
                            {
                                MessageBox.Show("Payment Type must be choosed!", "Error");
                                return; 
                            }

                            transaction.TransactionType = "Payment " + PaymentType;

                            //Update Balance
                            Account updateAccount = db.Accounts.Find(Account.Account.AccountId);
                            updateAccount.Balance -= Amount;

                            db.Transactions.Add(transaction);
                            db.SaveChanges();

                            MessageBox.Show("Payment: " + paymentType + " Success!", "Success");
                            LoadAccount(null); 
                        }
                    }
                    else MessageBox.Show("Insufficient Balance! Minimum IDR20,000 Balance in account", "Error");
                }
                else MessageBox.Show("Invalid Amount!", "Error");
            }
            else MessageBox.Show("Account not found!", "Error");
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
