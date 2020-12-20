using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;

namespace TPA_Desktop_NT20_2.ViewModels.Maintenance
{
    class UpdateMaintenanceScheduleViewModel : BaseViewModel
    {
        #region Attributes
        private MaintenanceSchedule maintenanceSchedule;
        private string reportId;
        private bool selected; 
        private string description; 
        private RelayCommand updateCommand;
        #endregion

        public UpdateMaintenanceScheduleViewModel()
        {
            Selected = false; 
        }

        public string ReportId
        {
            get { return reportId; }
            set { reportId = value; OnPropertyChanged("ReportId");  }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; OnPropertyChanged("Selected"); }
        }

        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged("Description"); }
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

        public RelayCommand UpdateCommand
        {
            get
            {
                updateCommand = new RelayCommand(UpdateSchedule, CanExecute);
                return updateCommand;
            }
            set
            {
                updateCommand = value;
                OnPropertyChanged("UpdateCommand");
            }
        }

        private void UpdateSchedule(object parameter)
        {
            if(ValidateReportId(ReportId))
            {
                if (Description != null)
                {
                    using (KongBuBankEntities db = new KongBuBankEntities())
                    {
                        MaintenanceSchedule schedule = db.MaintenanceSchedules.Find(ReportId);
                        if (Selected) schedule.Status = "Pending";
                        else schedule.Status = "Finish";

                        db.SaveChanges();
                        MessageBox.Show("Maintenance Schedule Updated!", "Success"); 
                    }
                }
                else MessageBox.Show("Description must be filled!", "Error"); 
            }
        }

        private bool ValidateReportId(string _reportId)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                MaintenanceSchedule schedule = db.MaintenanceSchedules.Find(_reportId);
                if (schedule == null)
                {
                    MessageBox.Show("Maintenance Schedule Not Found!", "Error");
                    return false;
                }
                return true;
            }
        }

    }
}
