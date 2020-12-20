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
    class ManageLeavingPermitViewModel : BaseViewModel
    {
        #region Attributes
        private string employeeId;
        private Employee currentEmployee;
        private RelayCommand viewCommand, submitCommand;
        private DateTime leaveDate;
        private string reason;
        private List<LeavingPermit> listPermits;
        private ObservableCollection<LeavingPermit> permits;
        #endregion

        public ManageLeavingPermitViewModel()
        {
            LeaveDate = DateTime.Now;
            LoadLeavingPermit(); 
        }

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

        public DateTime LeaveDate
        {
            get { return leaveDate; }
            set { leaveDate = value; OnPropertyChanged("LeaveDate"); }
        }

        public string Reason
        {
            get { return reason; }
            set { reason = value; OnPropertyChanged("Reason"); }
        }

        public ObservableCollection<LeavingPermit> Permits
        {
            get { return permits; }
            set { permits = value; OnPropertyChanged("Permits"); }
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
                        LoadLeavingPermit(); 
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
            if (IsEmployeeValid(EmployeeId) && ValidateDate()) 
            {
                if (Reason != null)
                {
                    using (KongBuBankEntities db = new KongBuBankEntities())
                    {
                        LeavingPermit permit = new LeavingPermit();

                        permit.LeavingPermitId = "LP" + IdFormat(Count("LeavingPermit") + 1);
                        permit.EmployeeId = EmployeeId;
                        permit.LeaveDate = LeaveDate;
                        permit.Reason = Reason;

                        db.LeavingPermits.Add(permit);
                        db.SaveChanges();

                        MessageBox.Show("Leaving Permit has been Added!", "Success");
                        LeaveDate = DateTime.Now;
                        Reason = null; 

                        LoadLeavingPermit(); 
                    }
                }
                else MessageBox.Show("Reason must be filled!", "Error"); 
            }
        }

        private void LoadLeavingPermit()
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                listPermits = new List<LeavingPermit>((from x in db.LeavingPermits
                                                       where x.EmployeeId == EmployeeId
                                                       select x).ToList());
                Permits = new ObservableCollection<LeavingPermit>(listPermits);
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

        private bool ValidateDate()
        {
            LeaveDate = new DateTime(LeaveDate.Year, LeaveDate.Month, LeaveDate.Day);
            int gap = DateTime.Compare(LeaveDate, DateTime.Now);
            if (gap > 0) return true;
            else MessageBox.Show("Invalid Time Sequence!", "Error");
            return false;
        }
    }
}
