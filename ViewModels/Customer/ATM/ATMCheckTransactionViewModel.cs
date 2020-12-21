using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPA_Desktop_NT20_2.Models;

namespace TPA_Desktop_NT20_2.ViewModels.Customer.ATM
{
    class ATMCheckTransactionViewModel : BaseViewModel
    {
        #region Attributes
        private string period;
        private IndividualAccount account;
        DateTime dateLimit;

        private List<Transaction> listTransactions; 
        private ObservableCollection<Transaction> transactions;

        #endregion

        public ATMCheckTransactionViewModel(IndividualAccount _account)
        {
            account = _account;
            dateLimit = new DateTime(); 
            dateLimit = DateTime.Now.AddDays(-30);
            Period = "Period " + dateLimit.Day + "." + dateLimit.Month + "." + dateLimit.Year + " - " +
                DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year;

            LoadDataGrid(); 
        }

        public string Period
        {
            get { return period; }
            set { period = value; OnPropertyChanged("Period"); }
        }

        public IndividualAccount Account
        {
            get { return account; }
            set { account = value; OnPropertyChanged("Account"); }
        }

        public ObservableCollection<Transaction> Transactions
        {
            get { return transactions; }
            set { transactions = value; OnPropertyChanged("Transactions"); }
        }

        private void LoadDataGrid()
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                listTransactions = ((from x in db.Transactions
                                     where x.AccountId == account.Account.AccountId
                                     where x.TransactionDate >= dateLimit
                                     select x).ToList());

                Transactions = new ObservableCollection<Transaction>(listTransactions); 
            }
        }

    }
}
