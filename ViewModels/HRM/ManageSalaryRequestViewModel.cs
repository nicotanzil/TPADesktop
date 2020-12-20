using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;

namespace TPA_Desktop_NT20_2.ViewModels.HRM
{
    class ManageSalaryRequestViewModel : BaseViewModel
    {
        #region Attributes
        private string employeeId;
        private Employee currentEmployee;
        private RelayCommand viewCommand, submitCommand;
        private Decimal raiseAmount;
        #endregion

        public string EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value; OnPropertyChanged("EmployeeId"); }
        }
        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }


        public Decimal RaiseAmount
        {
            get { return raiseAmount; }
            set { raiseAmount = value; OnPropertyChanged("RaiseAmount"); }
        }


        public RelayCommand ViewCommand
        {
            get
            {
                viewCommand = new RelayCommand(LoadAccount, CanExecute);
                return viewCommand;
            }
            set
            {
                viewCommand = value;
                OnPropertyChanged("ViewCommand");
            }
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

        private void LoadAccount(object parameter)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                Employee query = (from x in db.Employees
                                  where x.EmployeeId == EmployeeId
                                  select x).FirstOrDefault();
                if (query != null)
                {
                    if (query.IsActive)
                    {
                        CurrentEmployee = query;
                    }
                    else MessageBox.Show("Employee is inactive!", "Inavlid Data"); 
                }
                else
                {
                    MessageBox.Show("Employee Not Found!", "Invalid Data");
                }
            }
        }

        private void Submit(object parameter)
        {
            if(IsEmployeeValid(EmployeeId))
            {
                if(RaiseAmount > 0)
                {
                    using (KongBuBankEntities db = new KongBuBankEntities())
                    {
                        SalaryRaiseRequest request = new SalaryRaiseRequest();
                        request.RequestId = "SR" + IdFormat(Count("SalaryRaiseRequest") + 1);
                        request.EmployeeId = EmployeeId;
                        request.Amount = RaiseAmount;
                        request.IsApproved = "Pending";
                        request.RequestDate = DateTime.Now;

                        db.SalaryRaiseRequests.Add(request);
                        db.SaveChanges();

                        MessageBox.Show("Salary Raise Request Sent!", "Success");
                        EmployeeId = null;
                        RaiseAmount = 0;
                    }
                }
            }
        }

        private bool IsEmployeeValid(string _id)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                Employee employee = db.Employees.Find(EmployeeId);

                if (employee != null)
                {
                    if (employee.IsActive)
                    {
                        return true;
                    }
                    else MessageBox.Show("Employee is inactive!", "Error");
                }
                else MessageBox.Show("Employee not found!", "Error");
                return false;
            }
        }

    }
}
