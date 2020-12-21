using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TPA_Desktop_NT20_2.Models;
using System.Windows.Input;
using TPA_Desktop_NT20_2.ViewModels.Commands;
using TPA_Desktop_NT20_2.ViewModels.Teller;
using System.Windows;
using TPA_Desktop_NT20_2.Views.EmployeeGeneral;

namespace TPA_Desktop_NT20_2.ViewModels.Teller
{

    public class TellerViewModel : ApplicationViewModel
    {
        #region Attributes
        private Employee employee;
        private string role;
        #endregion

        public Employee Employee
        {
            get { return employee; }
            set { employee = value; OnPropertyChanged("Employee");  }
        }

        public string Role
        {
            get { return role; }
            set { role = value; OnPropertyChanged("Role");  }
        }


        public TellerViewModel(Employee _employee)
        {
            Name = "Teller"; 
            Employee = _employee;
            SelectedViewModel = new TellerDepositViewModel(Employee); 
        }

        protected override void ChangeViewMethod(object parameter)
        {
            if(parameter.ToString() == "TellerDeposit")
            {
                this.SelectedViewModel = new TellerDepositViewModel(Employee); 
            }
            else if(parameter.ToString() == "TellerWithdraw")
            {
                this.SelectedViewModel = new TellerWithdrawViewModel(Employee); 
            }
            else if(parameter.ToString() == "TellerTransfer")
            {
                this.SelectedViewModel = new TellerTransferViewModel(Employee);
            }
            else if(parameter.ToString() == "TellerPayment")
            {
                this.SelectedViewModel = new TellerPaymentViewModel(Employee); 
            }
        }

        protected override void ReportItemMethod(object parameter)
        {
            ReportItemWindow reportWin = new ReportItemWindow(Employee);
            reportWin.ShowDialog(); 
        }
    }
}
