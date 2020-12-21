using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPA_Desktop_NT20_2.Models;

namespace TPA_Desktop_NT20_2.ViewModels.ManagerFolder
{
    class ManagerViewModel : ApplicationViewModel
    {
        #region Attributes
        private Manager currentManager;
        #endregion

        public ManagerViewModel(Manager _manager)
        {
            Name = "Manager"; 
            CurrentManager = _manager;
            SelectedViewModel = new ViewFiringRequestViewModel(); 
        }

        public Manager CurrentManager
        {
            get { return currentManager; }
            set { currentManager = value; OnPropertyChanged("Manager"); }
        }

        protected override void ChangeViewMethod(object parameter)
        {
            if(parameter.ToString() == "FiringRequest")
            {
                SelectedViewModel = new ViewFiringRequestViewModel(); 
            }
        }
    }
}
