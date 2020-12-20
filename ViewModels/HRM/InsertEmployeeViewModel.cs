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
    class InsertEmployeeViewModel : BaseViewModel
    {
        #region Attributes
        private string candidateId;
        private Candidate currentCandidate;
        private Employee currentEmployee;
        private RelayCommand viewCommand, insertCommand;
        #endregion

        public InsertEmployeeViewModel()
        {
            CurrentEmployee = new Employee(); 

        }

        public string CandidateId
        {
            get { return candidateId; }
            set { candidateId = value; OnPropertyChanged("CandidateId"); }
        }


        public Candidate CurrentCandidate
        {
            get { return currentCandidate; }
            set { currentCandidate = value; OnPropertyChanged("CurrentCandidate"); }
        }


        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
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

        public RelayCommand InsertCommand
        {
            get
            {
                insertCommand = new RelayCommand(Insert, CanExecute);
                return insertCommand;
            }
            set
            {
                insertCommand = value;
                OnPropertyChanged("InsertCommand");
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
                Candidate query = (from x in db.Candidates
                                   where x.CandidateId == CandidateId
                                   select x).FirstOrDefault();
                if (query == null)
                {
                    //Account not found
                    MessageBox.Show("Candidate Not Found!", "Invalid Data");
                }
                else
                {
                    CurrentCandidate = query;
                }
            }
        }

        private void Insert(object parameter)
        {
            if(ValidateCandidate(CandidateId) && ValidateDepartment(CurrentEmployee.DepartmentId))
            {
                if (CurrentEmployee.Email != null && CurrentEmployee.Password != null)
                {
                    if (CurrentEmployee.Salary > 0)
                    {
                        //Insert Employee
                        using (KongBuBankEntities db = new KongBuBankEntities())
                        {
                            Candidate candidate = db.Candidates.Find(CandidateId);
                            candidate.Status = "Accepted";

                            Department department = db.Departments.Find(CurrentEmployee.DepartmentId);
                            department.AvailablePosition--; 

                            Employee employee = new Employee();
                            employee.EmployeeId = "EM" + IdFormat(Count("Employee") + 1);
                            employee.CandidateId = candidate.CandidateId;
                            employee.Name = candidate.Name;
                            employee.Dob = candidate.Dob;
                            employee.DepartmentId = CurrentEmployee.DepartmentId;
                            employee.Email = CurrentEmployee.Email;
                            employee.Password = CurrentEmployee.Password;
                            employee.PerformanceScore = 0; 
                            employee.ViolationScore = 0;
                            employee.Salary = CurrentEmployee.Salary;
                            employee.IsActive = true;

                            db.Employees.Add(employee);
                            db.SaveChanges(); 

                            CandidateId = null;
                            CurrentEmployee = null;
                            MessageBox.Show("Employee Accepted!", "Success"); 
                        }
                    }
                    else MessageBox.Show("Invalid Salary", "Error");
                }
                else MessageBox.Show("Email and Password must be filled!", "Error");
            }
        }

        private bool ValidateCandidate(string _id)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                Candidate candidate = (from x in db.Candidates
                                       where x.CandidateId == _id
                                       select x).FirstOrDefault();

                if (candidate != null)
                {
                    if (candidate.Status == "Pending")
                    {
                        return true;
                    }
                    else MessageBox.Show("Invalid Candidate!", "Error");
                }
                else MessageBox.Show("Candidate not found!", "Error");
                return false; 
            }
        }

        private bool ValidateDepartment(string _id)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                Department department = (from x in db.Departments
                                         where x.DepartmentId == _id
                                         select x).FirstOrDefault();
                if (department != null)
                {
                    if (department.AvailablePosition > 0)
                    {
                        return true;
                    }
                    else MessageBox.Show("No available position in " + department.Name + " Department!", "Error");
                }
                else MessageBox.Show("Department not found!", "Error");
                return false; 
            }
        }

    }
}
