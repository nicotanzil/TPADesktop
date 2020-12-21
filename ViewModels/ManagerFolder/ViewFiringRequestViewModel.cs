using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;

namespace TPA_Desktop_NT20_2.ViewModels.ManagerFolder
{
    class ViewFiringRequestViewModel : BaseViewModel
    {
        #region Attributes
        private List<FiringRequest> listRequests;
        private ObservableCollection<FiringRequest> firingRequests;
        private string requestId;
        private bool selected;
        private RelayCommand updateCommand;
        #endregion

        public ViewFiringRequestViewModel()
        {
            LoadDataGrid(); 
        }

        public ObservableCollection<FiringRequest> FiringRequests
        {
            get { return firingRequests; }
            set { firingRequests = value; OnPropertyChanged("FiringRequests"); }
        }

        private void LoadDataGrid()
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                listRequests = new List<FiringRequest>((from x in db.FiringRequests
                                                        where x.IsActive == true
                                                        select x).ToList());

                FiringRequests = new ObservableCollection<FiringRequest>(listRequests);
            }
        }


        public string RequestId
        {
            get { return requestId; }
            set { requestId = value; OnPropertyChanged("RequestId"); }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; OnPropertyChanged("Selected"); }
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

        public RelayCommand UpdateCommand
        {
            get
            {
                updateCommand = new RelayCommand(UpdateRequest, CanExecute);
                return updateCommand;
            }
            set
            {
                updateCommand = value;
                OnPropertyChanged("UpdateCommand");
            }
        }

        private void UpdateRequest(object parameter)
        {
            if (ValidateRequestId(RequestId))
            {
                using (KongBuBankEntities db = new KongBuBankEntities())
                {
                    FiringRequest request = db.FiringRequests.Find(RequestId);
                    if(Selected)
                    {
                        //Fire the employee
                        Employee employee = db.Employees.Find(request.TargetEmployeeId);
                        employee.IsActive = false;
                        employee.ViolationScore = 0; 

                        request.IsActive = false;

                        MessageBox.Show("Employee Fired!!!", "Success"); 
                    }
                    else
                    {
                        request.IsActive = false;
                        MessageBox.Show("Firing Request Removed!", "Success"); 
                    }
                    RequestId = null; 
                    db.SaveChanges();
                    LoadDataGrid(); 
                }
            }
        }

        private bool ValidateRequestId(string _requestId)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                FiringRequest request = db.FiringRequests.Find(_requestId); 
                if(request == null)
                {
                    MessageBox.Show("Firing Request not found!", "Error");
                    return false; 
                }
                else
                {
                    if(!request.IsActive)
                    {
                        MessageBox.Show("Firing Request is inactive!", "Error");
                        return false; 
                    }
                }
                return true; 
            }
        }
    }
}
