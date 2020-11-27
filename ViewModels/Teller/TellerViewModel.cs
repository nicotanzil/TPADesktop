using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TPA_Desktop_NT20_2.Models;
using System.Windows.Input;
using TPA_Desktop_NT20_2.ViewModels.Commands;
using TPA_Desktop_NT20_2.ViewModels.Teller; 


namespace TPA_Desktop_NT20_2.ViewModels.Teller
{

    public class TellerViewModel : ApplicationViewModel
    {

        private Employee employee;
        private string role;

        public Employee Employee
        {
            get { return employee; }
            set { employee = value; OnPropertyChanged("Employee");  }
        }

        public string Role
        {
            get { return role; }
            set { role = value; OnPropertyChanged("Role");  }
        }


        public TellerViewModel(Employee _employee)
        {
            Name = "Teller"; 
            Employee = _employee;
            Console.WriteLine("Teller: " + Employee.Name); 
            SelectedViewModel = new TellerDepositViewModel(); 
        }

        protected override void changeViewMethod(object parameter)
        {
            if(parameter.ToString() == "TellerDeposit")
            {
                this.SelectedViewModel = new TellerDepositViewModel(); 
            }
            else if(parameter.ToString() == "TellerWithdraw")
            {
                this.SelectedViewModel = new TellerWithdrawViewModel(); 
            }
        }
    }
}
