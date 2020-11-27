using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using System.Windows.Input; 
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands; 

namespace TPA_Desktop_NT20_2.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;
        private RelayCommand changeViewCommand; 


        public ApplicationViewModel()
        {
            SelectedViewModel = new EmployeeAuthenticationViewModel(); 
        }

        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set { _selectedViewModel = value; OnPropertyChanged("SelectedViewModel");  }
        }

        public RelayCommand ChangeViewCommand
        {
            get
            {
                changeViewCommand = new RelayCommand(changeViewMethod, CanExecute);
                return changeViewCommand; 
            }
            set
            {
                changeViewCommand = value;
                OnPropertyChanged("ChangeViewCommand"); 
            }
        }

        protected virtual void changeViewMethod(object parameter)
        {
            if(parameter.ToString() == "EmployeeAuthentication")
            {
                this.SelectedViewModel = new EmployeeAuthenticationViewModel(); 
            }
            else if(parameter.ToString() == "CustomerAuthentication")
            {
                this.SelectedViewModel = new CustomerAuthenticationViewModel(); 
            }
        }

        protected bool CanExecute(object parameter)
        {
            return true; 
        }
    }
}
