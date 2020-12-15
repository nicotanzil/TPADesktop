using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPA_Desktop_NT20_2.ViewModels.Customer;

namespace TPA_Desktop_NT20_2.ViewModels
{
    class CustomerAuthenticationViewModel : ApplicationViewModel
    {

        public CustomerAuthenticationViewModel()
        {
            Name = "CustomerAuthentication";
            SelectedViewModel = new ATMAuthenticationViewModel(); 

        }

        protected override void ChangeViewMethod(object parameter)
        {
            if(parameter.ToString() == "ATMAuthentication")
            {
                this.SelectedViewModel = new ATMAuthenticationViewModel(); 
            }
            else if(parameter.ToString() == "QueueMachine")
            {
                this.SelectedViewModel = new QueueMachineViewModel();
            }
        }
    }
}
