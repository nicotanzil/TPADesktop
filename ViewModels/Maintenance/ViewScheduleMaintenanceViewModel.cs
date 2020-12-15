using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPA_Desktop_NT20_2.Models;

namespace TPA_Desktop_NT20_2.ViewModels.Maintenance
{
    class ViewScheduleMaintenanceViewModel : BaseViewModel
    {
        private Employee currentEmployee;
        private List<MaintenanceSchedule> listSchedules;  
        private ObservableCollection<MaintenanceSchedule> schedules;


        public ViewScheduleMaintenanceViewModel(Employee _employee)
        {
            CurrentEmployee = _employee; 
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                listSchedules = new List<MaintenanceSchedule>((from x in db.MaintenanceSchedules
                                                               select x).ToList());
                Schedules = new ObservableCollection<MaintenanceSchedule>(listSchedules);
            }
        }


        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }


        public ObservableCollection<MaintenanceSchedule> Schedules
        {
            get { return schedules; }
            set { schedules = value; OnPropertyChanged("Schedules"); }
        }


    }
}
