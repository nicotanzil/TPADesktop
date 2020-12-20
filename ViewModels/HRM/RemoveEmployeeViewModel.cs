using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;

namespace TPA_Desktop_NT20_2.ViewModels.HRM
{
    class RemoveEmployeeViewModel : BaseViewModel
    {
        #region Attributes
        private string employeeId;
        private Employee currentEmployee;
        private RelayCommand viewCommand, removeCommand; 
        #endregion

        public string EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value; OnPropertyChanged("EmployeeId"); }
        }

        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        public RelayCommand ViewCommand
        {
            get
            {
                viewCommand = new RelayCommand(LoadAccount, CanExecute);
                return viewCommand;
            }
            set
            {
                viewCommand = value;
                OnPropertyChanged("ViewCommand");
            }
        }

        public RelayCommand RemoveCommand
        {
            get
            {
                removeCommand = new RelayCommand(Remove, CanExecute);
                return removeCommand;
            }
            set
            {
                removeCommand = value;
                OnPropertyChanged("RemoveCommand");
            }
        }
        private bool CanExecute(object parameter)
        {
            return true;
        }

        private void LoadAccount(object parameter)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                Employee query = (from x in db.Employees
                                  where x.EmployeeId == EmployeeId
                                  select x).FirstOrDefault();
                if (query == null)
                {
                    //Account not found
                    MessageBox.Show("Employee Not Found!", "Invalid Data");
                }
                else
                {
                    CurrentEmployee = query;
                }
            }
        }

        private void Remove(object parameter)
        {
            if(IsEmployeeValid(EmployeeId))
            {
                using (KongBuBankEntities db = new KongBuBankEntities())
                {
                    Employee employee = (from x in db.Employees
                                         where x.EmployeeId == EmployeeId
                                         select x).First();
                    db.Employees.Remove(employee);

                    Department department = db.Departments.Find(employee.DepartmentId);
                    department.AvailablePosition++; 

                    db.SaveChanges();
                    MessageBox.Show("Employee Removed!", "Success"); 
                }
            }
        }

        private bool IsEmployeeValid(string _id)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                Employee employee = db.Employees.Find(EmployeeId);

                if (employee != null)
                {
                    if (!employee.IsActive)
                    {
                        return true;
                    }
                    else MessageBox.Show("Employee still active!", "Error"); 
                }
                else MessageBox.Show("Employee not found!", "Error");
                return false; 
            }
        }
    }
}
