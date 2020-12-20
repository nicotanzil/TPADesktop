using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPA_Desktop_NT20_2.Models;

namespace TPA_Desktop_NT20_2.ViewModels.HRM
{
    class ViewEmployeeViewModel : BaseViewModel
    {
        #region Attributes
        private Employee currentEmployee;
        private List<Employee> listEmployees;
        private ObservableCollection<Employee> employees;
        #endregion

        public ViewEmployeeViewModel(Employee _employee)
        {
            CurrentEmployee = _employee; 
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                listEmployees = new List<Employee>((from x in db.Employees
                                                    select x).ToList());
                Employees = new ObservableCollection<Employee>(listEmployees); 
            }
        }

        public ObservableCollection<Employee> Employees
        {
            get { return employees; }
            set { employees = value; OnPropertyChanged("Employees"); }
        }


        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

    }
}
