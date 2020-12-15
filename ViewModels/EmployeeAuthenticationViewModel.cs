using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;
using TPA_Desktop_NT20_2.Models.SQL;
using System.ComponentModel;
using System.Windows;
using TPA_Desktop_NT20_2.Views.Teller;
using TPA_Desktop_NT20_2.Views.Maintenance;

namespace TPA_Desktop_NT20_2.ViewModels
{
    public class EmployeeAuthenticationViewModel : BaseViewModel
    {
        #region attributes
        private string message;
        private Employee currentEmployee;
        private RelayCommand loginCommand;
        #endregion


        public EmployeeAuthenticationViewModel()
        {
            Name = "EmployeeAuthentication";
            currentEmployee = new Employee(); 
        }

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }


        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        public RelayCommand LoginCommand
        {
            get
            {
                loginCommand = new RelayCommand(LoadData, CanExecute);
                return loginCommand; 
            }
            set
            {
                loginCommand = value;
                OnPropertyChanged("LoginCommand"); 
            }
        }

        private bool CanExecute(object parameter)
        {
            return true; 
        }

        private void LoadData(object parameter)
        {
            using(KongBuBankEntities db = new KongBuBankEntities())
            {
                Employee query = (from x in db.Employees
                                  where x.Email == CurrentEmployee.Email
                                  where x.Password == CurrentEmployee.Password
                                  select x).FirstOrDefault();

                if(query != null)
                {
                    CurrentEmployee = query;
                    RedirectPage(query.Department.Name); 
                }
                else
                {
                    //Account not found
                    Message = "Invalid Email / Password!";
                    MessageBox.Show("Invalid Email / Password!", "Authentication Error");
                }
            }
        }

        private void RedirectPage(string department)
        {
            if (department == "Teller")
            {
                //GOTO teller page
                TellerWindow tellerWin = new TellerWindow(CurrentEmployee);
                tellerWin.ShowDialog();
            }
            else if (department == "Customer Service")
            {
                //GOTO customer service page
            }
            else if (department == "Maintenance Team")
            {
                //GOTO maintenance
                MaintenanceWindow maintenanceWin = new MaintenanceWindow(CurrentEmployee);
                maintenanceWin.ShowDialog(); 
            }
        }
    }
}
