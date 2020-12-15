using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPA_Desktop_NT20_2.Models;

namespace TPA_Desktop_NT20_2.ViewModels.Maintenance
{
    class ViewMaintenanceReportViewModel : BaseViewModel
    {
        #region Attributes
        private Employee currentEmployee;
        private List<MaintenanceReport> listReports;
        private ObservableCollection<MaintenanceReport> reports; 
        #endregion

        public ViewMaintenanceReportViewModel(Employee _employee)
        {
            CurrentEmployee = _employee;
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                listReports = new List<MaintenanceReport>((from x in db.MaintenanceReports
                                                       select x).ToList());
                Reports = new ObservableCollection<MaintenanceReport>(listReports);
            }
        }

        public ObservableCollection<MaintenanceReport> Reports
        {
            get { return reports; }
            set { reports = value; OnPropertyChanged("Reports"); }
        }

        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }
    }
}
