using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.Views.EmployeeGeneral;

namespace TPA_Desktop_NT20_2.ViewModels.Maintenance
{
    class MaintenanceViewModel : ApplicationViewModel
    {
        #region Attributes
        private Employee currentEmployee;
        #endregion

        public MaintenanceViewModel(Employee _employee)
        {
            Name = "Maintenance Team"; 
            CurrentEmployee = _employee; 
            SelectedViewModel = new ViewMaintenanceReportViewModel(CurrentEmployee); 
        }

        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee");  }
        }

        protected override void ChangeViewMethod(object parameter)
        {
            if(parameter.ToString() == "ViewReports")
            {
                //GOTO to Maintenance View Report
                SelectedViewModel = new ViewMaintenanceReportViewModel(CurrentEmployee); 
            }
            else if(parameter.ToString() == "ViewSchedules")
            {
                //GOTO to Schedule Maintenance View
                SelectedViewModel = new ViewScheduleMaintenanceViewModel(CurrentEmployee); 
            }
            else if(parameter.ToString() == "AddSchedules")
            {
                //GOTO to Add Maintenance Schedules
                SelectedViewModel = new AddMaintenanceScheduleViewModel(CurrentEmployee); 
            }
            else if(parameter.ToString() == "UpdateSchedules")
            {
                //GOTO to Update Maintenance Schedules
                SelectedViewModel = new UpdateMaintenanceScheduleViewModel(); 
            }
        }

        protected override void ReportItemMethod(object parameter)
        {
            ReportItemWindow reportWin = new ReportItemWindow(CurrentEmployee);
            reportWin.ShowDialog();
        }
    }
}
