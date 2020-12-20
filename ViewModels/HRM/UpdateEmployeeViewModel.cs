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
    class UpdateEmployeeViewModel : BaseViewModel
    {
        #region Attributes
        private string employeeId;
        private Employee currentEmployee;
        private RelayCommand viewCommand, updateCommand; 
        private string newEmail;
        private string newPassword;
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


        public string NewEmail
        {
            get { return newEmail; }
            set { newEmail = value; OnPropertyChanged("NewEmail"); }
        }


        public string NewPassword
        {
            get { return newPassword; }
            set { newPassword = value; OnPropertyChanged("NewPassword"); }
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

        public RelayCommand UpdateCommand
        {
            get
            {
                updateCommand = new RelayCommand(Update, CanExecute);
                return updateCommand;
            }
            set
            {
                updateCommand = value;
                OnPropertyChanged("UpdateCommand");
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

        private void Update(object parameter)
        {
            if(ValidateEmployee(EmployeeId))
            {
                using (KongBuBankEntities db = new KongBuBankEntities())
                {
                    Employee employee = db.Employees.Find(EmployeeId);  

                    if(NewEmail != "")
                    {
                        employee.Email = NewEmail;
                        MessageBox.Show("Email Updated!", "Error");
                        CurrentEmployee = employee;
                    }
                    if(NewPassword != "")
                    {
                        employee.Password = NewPassword;
                        MessageBox.Show("Password Updated!", "Error");
                    }
                    db.SaveChanges(); 
                }
            }
        }

        private bool ValidateEmployee(string _id)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                Employee employee = (from x in db.Employees
                                     where x.EmployeeId == _id
                                     select x).FirstOrDefault();
                if (employee != null)
                {
                    return true;
                }
                else MessageBox.Show("Employee not found!", "Error");
                return false; 
            }
        }
    }
}
