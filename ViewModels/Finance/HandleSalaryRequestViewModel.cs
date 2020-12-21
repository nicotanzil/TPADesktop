using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;

namespace TPA_Desktop_NT20_2.ViewModels.Finance
{
    class HandleSalaryRequestViewModel : BaseViewModel
    {
        #region Attributes
        private List<SalaryRaiseRequest> listRequests;
        private ObservableCollection<SalaryRaiseRequest> salaryRaiseRequests;
        private string requestId;
        private bool selected;
        private RelayCommand updateCommand;
        #endregion

        public HandleSalaryRequestViewModel()
        {
            Selected = true; 
            LoadDataGrid();
        }

        public ObservableCollection<SalaryRaiseRequest> SalaryRaiseRequests
        {
            get { return salaryRaiseRequests; }
            set { salaryRaiseRequests = value; OnPropertyChanged("SalaryRaiseRequests"); }
        }

        private void LoadDataGrid()
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                listRequests = new List<SalaryRaiseRequest>((from x in db.SalaryRaiseRequests
                                                        where x.IsApproved == "Pending"
                                                        select x).ToList());

                SalaryRaiseRequests = new ObservableCollection<SalaryRaiseRequest>(listRequests);
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
                    SalaryRaiseRequest request = db.SalaryRaiseRequests.Find(RequestId);
                    if (Selected)
                    {
                        //Approve raise
                        Employee employee = db.Employees.Find(request.EmployeeId);
                        employee.Salary += request.Amount; 

                        request.IsApproved = "Accepted";

                        MessageBox.Show("Salary Raise Request Accepted!", "Success");
                    }
                    else
                    {
                        request.IsApproved = "Rejected"; 
                        MessageBox.Show("Salary Raise Request Rejected!", "Rejected");
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
                SalaryRaiseRequest request = db.SalaryRaiseRequests.Find(_requestId);
                if (request == null)
                {
                    MessageBox.Show("Salary Request not found!", "Error");
                    return false;
                }
                else
                {
                    if (request.IsApproved != "Pending")
                    {
                        MessageBox.Show("Salary Request already processed!", "Error");
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
