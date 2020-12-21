using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.Views.EmployeeGeneral;

namespace TPA_Desktop_NT20_2.ViewModels.HRM
{
    class HRMViewModel : ApplicationViewModel
    {
        #region Attributes
        private Employee currentEmployee;
        #endregion

        public HRMViewModel(Employee _employee)
        {
            Name = "HRM Team";
            CurrentEmployee = _employee;
            SelectedViewModel = new ManageEmployeeDataViewModel(CurrentEmployee);
        }

        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        protected override void ChangeViewMethod(object parameter)
        {
            if (parameter.ToString() == "EmployeeData")
            {
                SelectedViewModel = new ManageEmployeeDataViewModel(CurrentEmployee);
            }
            else if(parameter.ToString() == "Violation")
            {
                SelectedViewModel = new ManageViolationViewModel(); 
            }
            else if(parameter.ToString() == "ViolationRequest")
            {
                SelectedViewModel = new CreateViolationReportViewModel(); 
            }
            else if(parameter.ToString() == "SalaryRaise")
            {
                SelectedViewModel = new ManageSalaryRequestViewModel(); 
            }
            else if(parameter.ToString() == "LeavingPermit")
            {
                SelectedViewModel = new ManageLeavingPermitViewModel(); 
            }
        }

        protected override void ReportItemMethod(object parameter)
        {
            ReportItemWindow reportWin = new ReportItemWindow(CurrentEmployee);
            reportWin.ShowDialog();
        }
    }
}
