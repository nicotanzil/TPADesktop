using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;

namespace TPA_Desktop_NT20_2.ViewModels.Customer.ATM
{
    class ATMPaymentViewModel : BaseViewModel
    {
        #region Attributes
        private string message;
        private IndividualAccount account;
        private RelayCommand paymentCommand; 
        private Decimal amount;
        private CollectionView paymentTypes;
        private string paymentType;
        #endregion

        public ATMPaymentViewModel(IndividualAccount _account)
        {
            Name = "ATMPayment";
            account = new IndividualAccount(); 

            Account = _account;

            //ComboBox
            IList<string> list = new List<string>();
            list.Add("Electricity");
            list.Add("Water");
            paymentTypes = new CollectionView(list);
        }

        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged("Message"); }
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

        public CollectionView PaymentTypes
        {
            get { return paymentTypes; }
        }

        public string PaymentType
        {
            get { return paymentType; }
            set { paymentType = value; OnPropertyChanged("PaymentType"); }
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

        private void Payment(object parameter)
        {
            //Account exists
            if (Amount > 0)
            {
                if (ValidateAccountBalance(Account.Account.AccountId, Amount))
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

                        //Validate combobox
                        if (PaymentType == null)
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
