using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.Views.EmployeeGeneral;

namespace TPA_Desktop_NT20_2.ViewModels.Finance
{
    class FinanceViewModel : ApplicationViewModel
    {
        #region Attributes
        private Employee currentEmployee;
        #endregion

        public FinanceViewModel(Employee _employee)
        {
            Name = "Finance Team";
            CurrentEmployee = _employee;
            SelectedViewModel = new HandleSalaryRequestViewModel(); 
        }

        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        protected override void ChangeViewMethod(object parameter)
        {
            if (parameter.ToString() == "SalaryRequest")
            {
                SelectedViewModel = new HandleSalaryRequestViewModel(); 
            }
        }

        protected override void ReportItemMethod(object parameter)
        {
            ReportItemWindow reportWin = new ReportItemWindow(CurrentEmployee);
            reportWin.ShowDialog();
        }
    }
}
