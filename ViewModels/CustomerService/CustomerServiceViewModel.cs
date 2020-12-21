using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.Views.EmployeeGeneral;

namespace TPA_Desktop_NT20_2.ViewModels.CustomerService
{
    class CustomerServiceViewModel : ApplicationViewModel
    {
        #region Attributes
        private Employee currentEmployee;
        #endregion

        public CustomerServiceViewModel(Employee _employee)
        {
            Name = "Customer Service";
            CurrentEmployee = _employee;
        }

        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        protected override void ChangeViewMethod(object parameter)
        {
            if(parameter.ToString() == "CheckTransaction")
            {
                SelectedViewModel = new CheckTransactionViewModel(); 
            }
        }

        protected override void ReportItemMethod(object parameter)
        {
            ReportItemWindow reportWin = new ReportItemWindow(CurrentEmployee);
            reportWin.ShowDialog();
        }
    }
}
