using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;

namespace TPA_Desktop_NT20_2.ViewModels.CustomerService
{
    class CheckTransactionViewModel : BaseViewModel
    {
        #region Attributes
        private string period;
        DateTime dateLimit;
        private string accountId;

        private List<Transaction> listTransactions;
        private ObservableCollection<Transaction> transactions;

        private RelayCommand submitCommand; 
        #endregion

        public CheckTransactionViewModel()
        {
            dateLimit = new DateTime();
            dateLimit = DateTime.Now.AddDays(-90);
            Period = "Period " + dateLimit.Day + "." + dateLimit.Month + "." + dateLimit.Year + " - " +
                DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year;

            LoadDataGrid(AccountId);
        }

        public string Period
        {
            get { return period; }
            set { period = value; OnPropertyChanged("Period"); }
        }


        public string AccountId
        {
            get { return accountId; }
            set { accountId = value; OnPropertyChanged("AccountId"); }
        }


        public ObservableCollection<Transaction> Transactions
        {
            get { return transactions; }
            set { transactions = value; OnPropertyChanged("Transactions"); }
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

        public RelayCommand SubmitCommand
        {
            get
            {
                submitCommand = new RelayCommand(Submit, CanExecute);
                return submitCommand;
            }
            set
            {
                submitCommand = value;
                OnPropertyChanged("UpdateCommand");
            }
        }

        private void LoadDataGrid(string _accountId)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                listTransactions = ((from x in db.Transactions
                                     where x.AccountId == _accountId
                                     where x.TransactionDate >= dateLimit
                                     select x).ToList());

                Transactions = new ObservableCollection<Transaction>(listTransactions);
            }
        }

        private void Submit(object parameter)
        {
            if(ValidateAccount(AccountId))
            {
                LoadDataGrid(AccountId); 
            }
        }

        private bool ValidateAccount(string _id)
        {
            KongBuBankEntities db = KongBuBankEntities.Instance;
            Account account = db.Accounts.Find(_id);

            if (account != null)
            {
                if (account.IsActive)
                {
                    return true;
                }
                else MessageBox.Show("Account is inactive!", "Error");
            }
            else MessageBox.Show("Account not found!", "Error");
            return false; 
        }

    }
}
