using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.Models.SQL;

namespace TPA_Desktop_NT20_2.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        private string name;
        private KongBuBankEntities kongBuBank;

        public BaseViewModel()
        {
            kongBuBank = new KongBuBankEntities(); 
        }

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name");  }
        }


        public KongBuBankEntities KongBuBank
        {
            get { return kongBuBank; }
            set { kongBuBank = value; OnPropertyChanged("KongBuBank"); }
        }


        public bool IsEmpty(DataTable dt)
        {
            if (dt.Rows.Count == 0) return true;
            return false; 
        }

        public DataTable GetData(string query)
        {
            DbManager db = new DbManager();
            DataTable dt = new DataTable();
            dt = db.Get(query);

            return dt;
        }

        public void Execute(string query)
        {
            DbManager db = new DbManager();
            db.Execute(query);
        }

        public int Count(string tableName)
        {
            using(KongBuBankEntities db = new KongBuBankEntities())
            {
                if(tableName == "Transaction")
                {
                    return (from x in db.Transactions select x).Count(); 
                }
                else if(tableName == "MaintenanceReport")
                {
                    return (from x in db.MaintenanceReports select x).Count(); 
                }
                else
                {
                    return -1; 
                }
            }
        }

        public String IdFormat(int index)
        {
            return index < 10 ? "00" + index : index < 100 ? "0" + index : index.ToString(); 
        }
    }
}
