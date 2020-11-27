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

namespace TPA_Desktop_NT20_2.ViewModels
{
    public class EmployeeAuthenticationViewModel : BaseViewModel,IDataErrorInfo
    {
        #region attributes
        private string message;
        private Employee employee;
        private RelayCommand loginCommand;
        private Employee currentEmployee;
        public string Error { get { return null;  } }
        #endregion


        public EmployeeAuthenticationViewModel()
        {
            Name = "EmployeeAuthentication";
            employee = new Employee(); 
        }

        public string this[string email] //TOFIX Error Message
        {
            get
            {
                string result = null; 

                switch(email)
                {
                    case "Email":
                        if (string.IsNullOrWhiteSpace(Employee.Email))
                            result = "Email must be filled!";
                        break; 
                }

                return result;
            }
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


        public Employee Employee
        {
            get { return employee; }
            set { employee = value; OnPropertyChanged("Employee"); }
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


        public DataTable GetData(string query)
        {
            DbManager db = new DbManager(); 
            DataTable dt = new DataTable();
            dt = db.Get(query);

            return dt;
        }

        private bool CanExecute(object parameter)
        {
            return true; 
        }

        private void LoadData(object parameter)
        {
            DataTable dt = GetData("SELECT * FROM Employee WHERE Email = '" + employee.Email + "' AND Password = '" + employee.Password + "'");
            if(dt.Rows.Count == 0)
            {
                //Account not found
                Message = "Invalid Email / Password!";
                MessageBox.Show("Invalid Email / Password!", "Authentication Error"); 
            }
            else if (dt.Rows.Count == 1) //Employee account is found
            {
                int index = 0;
                string id = dt.Rows[index]["employeeId"].ToString();
                string name = dt.Rows[index]["name"].ToString();
                DateTime dob = (DateTime)dt.Rows[index]["dob"];
                string departmentId = dt.Rows[index]["departmentId"].ToString();
                string email = dt.Rows[index]["email"].ToString();
                string password = dt.Rows[index]["password"].ToString();

                DataTable dtDep = GetData("SELECT * FROM Department WHERE DepartmentId = '" + departmentId + "'");
                Department department = new Department(departmentId, dtDep.Rows[0]["Name"].ToString());

                currentEmployee = new Employee(id, name, dob, email, password, department); 

                Console.WriteLine("Authentication success: " + currentEmployee.Name); 
                Message = "Success!";

                RedirectPage();
            }
        }

        private void RedirectPage()
        {
            DataTable dt = GetData("SELECT * FROM Department WHERE departmentId = '" + currentEmployee.Department.DepartmentId + "'");
            Console.WriteLine("Rows: " + dt.Rows.Count);
            if (dt.Rows.Count == 1)
            {
                if ((string)dt.Rows[0]["name"] == "Teller")
                {
                    //goto teller page
                    TellerWindow tellerWin = new TellerWindow(currentEmployee);
                    tellerWin.ShowDialog(); 
                }
            }
        }
    }
}
