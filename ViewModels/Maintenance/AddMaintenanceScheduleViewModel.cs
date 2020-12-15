using Syncfusion.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;

namespace TPA_Desktop_NT20_2.ViewModels.Maintenance
{
    class AddMaintenanceScheduleViewModel : BaseViewModel
    {
        #region Attributes
        private string message;
        private Employee currentEmployee;
        private MaintenanceSchedule schedule;
        private string reportId;
        private TimeSpan time;

        private RelayCommand submitCommand;
        #endregion

        public AddMaintenanceScheduleViewModel(Employee _employee)
        {
            CurrentEmployee = _employee;
        }

        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }


        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged("Message"); }
        }

        public MaintenanceSchedule Schedule
        {
            get { return schedule; }
            set { schedule = value; OnPropertyChanged("Schedule"); }
        }


        public RelayCommand SubmitCommand
        {
            get
            {
                submitCommand = new RelayCommand(SubmitSchedule, CanExecute);
                return submitCommand;
            }
            set
            {
                submitCommand = value;
                OnPropertyChanged("SubmitCommand");
            }
        }


        public string ReportId
        {
            get { return reportId; }
            set { reportId = value; OnPropertyChanged("ReportId"); }
        }


        public TimeSpan Time
        {
            get { return time; }
            set { time = value; OnPropertyChanged("Time"); }
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

        private void SubmitSchedule(object parameter)
        {
            Console.WriteLine("Report ID: " + ReportId);
            Console.WriteLine("Time: " + Time);
        }

    }
}
