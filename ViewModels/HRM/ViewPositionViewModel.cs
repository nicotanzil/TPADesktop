using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPA_Desktop_NT20_2.Models;

namespace TPA_Desktop_NT20_2.ViewModels.HRM
{
    class ViewPositionViewModel : BaseViewModel
    {
        #region Attributes
        private Employee currentEmployee;
        private List<Department> listDepartments;
        private ObservableCollection<Department> departments;
        #endregion

        public ViewPositionViewModel(Employee _employee)
        {
            CurrentEmployee = _employee;
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                listDepartments = new List<Department>((from x in db.Departments
                                                      select x).ToList());
                Departments = new ObservableCollection<Department>(listDepartments);
            }
        }

        public ObservableCollection<Department> Departments
        {
            get { return departments; }
            set { departments = value; OnPropertyChanged("Departments"); }
        }


        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }
    }
}
