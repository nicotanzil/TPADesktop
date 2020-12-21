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
        private RelayCommand reportItemCommand; 

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
                changeViewCommand = new RelayCommand(ChangeViewMethod, CanExecute);
                return changeViewCommand; 
            }
            set
            {
                changeViewCommand = value;
                OnPropertyChanged("ChangeViewCommand"); 
            }
        }

        public RelayCommand ReportItemCommand
        {
            get
            {
                reportItemCommand = new RelayCommand(ReportItemMethod, CanExecute);
                return reportItemCommand;
            }
            set
            {
                reportItemCommand = value;
                OnPropertyChanged("ReportItemCommand");
            }
        }

        protected virtual void ChangeViewMethod(object parameter)
        {
            if(parameter.ToString() == "EmployeeAuthentication")
            {
                this.SelectedViewModel = new EmployeeAuthenticationViewModel(); 
            }
            else if(parameter.ToString() == "CustomerAuthentication")
            {
                this.SelectedViewModel = new CustomerAuthenticationViewModel(); 
            }
            else if(parameter.ToString() == "ManagerAuthentication")
            {
                this.SelectedViewModel = new ManagerAuthenticationViewModel(); 
            }
        }

        protected virtual void ReportItemMethod(object parameter)
        {

        } 

        protected bool CanExecute(object parameter)
        {
            return true; 
        }
    }
}
