using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;

namespace TPA_Desktop_NT20_2.ViewModels.HRM
{
    class CreateViolationReportViewModel : BaseViewModel
    {
        #region Attributes
        private string employeeId;
        private RelayCommand submitCommand;
        private string description;

        private List<Employee> listEmployees; 
        private ObservableCollection<Employee> employees;
        #endregion

        public CreateViolationReportViewModel()
        {
            using(KongBuBankEntities db = new KongBuBankEntities())
            {
                listEmployees = new List<Employee>((from x in db.Employees
                                                    select x).ToList());

                Employees = new ObservableCollection<Employee>(listEmployees); 
            }
        }

        public string EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value; OnPropertyChanged("EmployeeId"); }
        }

        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged("Description"); }
        }


        public ObservableCollection<Employee> Employees
        {
            get { return employees; }
            set { employees = value; OnPropertyChanged("Employees"); }
        }


        public RelayCommand SubmitCommand
        {
            get
            {
                submitCommand = new RelayCommand(Submit, CanExecute);
                return submitCommand;
            }
            set
            {
                submitCommand = value;
                OnPropertyChanged("SubmitCommand");
            }
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

        private void Submit(object paramater)
        {
            if(ValidateEmployee(EmployeeId))
            {
                if(ValidateRequest(EmployeeId))
                {
                    if (Description != null)
                    {
                        using (KongBuBankEntities db = new KongBuBankEntities())
                        {
                            FiringRequest request = new FiringRequest();
                            request.RequestId = "FR" + IdFormat(Count("FiringRequest") + 1);
                            request.TargetEmployeeId = EmployeeId;
                            request.Description = Description;
                            request.RequestDate = DateTime.Now;
                            request.IsActive = true;

                            db.FiringRequests.Add(request);
                            db.SaveChanges();

                            MessageBox.Show("Firing Request Sent!", "Error");

                            EmployeeId = null; 
                            Description = null; 
                        }
                    }
                    else MessageBox.Show("Description must be filled!", "Error"); 
                }
            }
        }

        private bool ValidateRequest(string _id)
        {
            using(KongBuBankEntities db = new KongBuBankEntities())
            {
                FiringRequest request = (from x in db.FiringRequests
                                         where x.TargetEmployeeId == _id
                                         select x).FirstOrDefault(); 

                if (request != null)
                {
                    if (!request.IsActive)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Firing Request already exists!", "Error");
                        return false;
                    }
                }
                else return true; 
            }
        }

        private bool ValidateEmployee(string _id)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                Employee employee = (from x in db.Employees
                                     where x.EmployeeId == EmployeeId
                                     where x.IsActive == true
                                     select x).FirstOrDefault(); 
                if(employee != null)
                {
                    return true; 
                }else
                {
                    MessageBox.Show("Invalid employee ID", "Error");
                    return false; 
                }
            }
        }

    }
}
