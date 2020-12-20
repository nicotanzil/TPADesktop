using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPA_Desktop_NT20_2.Models;

namespace TPA_Desktop_NT20_2.ViewModels.HRM
{
    class ManageEmployeeDataViewModel : ApplicationViewModel
    {
        #region Attributes
        private Employee currentEmployee;
        #endregion

        public ManageEmployeeDataViewModel(Employee _employee)
        {
            CurrentEmployee = _employee;
            SelectedViewModel = new ViewEmployeeViewModel(CurrentEmployee);
        }

        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        protected override void ChangeViewMethod(object parameter)
        {
            if (parameter.ToString() == "ViewEmployee")
            {
                SelectedViewModel = new ViewEmployeeViewModel(CurrentEmployee); 
            }
            else if (parameter.ToString() == "InsertEmployee")
            {
                SelectedViewModel = new InsertEmployeeViewModel();
            }
            else if (parameter.ToString() == "UpdateEmployee")
            {
                SelectedViewModel = new UpdateEmployeeViewModel(); 
            }
            else if (parameter.ToString() == "RemoveEmployee")
            {
                SelectedViewModel = new RemoveEmployeeViewModel(); 
            }
            else if (parameter.ToString() == "ViewPosition")
            {
                SelectedViewModel = new ViewPositionViewModel(CurrentEmployee);
            }
            else if (parameter.ToString() == "ViewCandidate")
            {
                SelectedViewModel = new ViewCandidateViewModel(CurrentEmployee); 
            }
            else if (parameter.ToString() == "InsertCandidate")
            {
                SelectedViewModel = new InsertCandidateViewModel(); 
            }
            else if (parameter.ToString() == "RemoveCandidate")
            {
                SelectedViewModel = new RemoveCandidateViewModel(); 
            }
        }
    }
}
