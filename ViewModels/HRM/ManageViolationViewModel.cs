using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;

namespace TPA_Desktop_NT20_2.ViewModels.HRM
{
    class ManageViolationViewModel : BaseViewModel
    {
        #region Attributes
        private Employee currentEmployee;
        private string employeeId;
        private int severity;
        private int repetition;
        private int solution;
        private CollectionView rateSeverity;
        private CollectionView rateRepetition;
        private CollectionView rateSolution;
        private string description;
        private Decimal violationScore;

        private RelayCommand submitCommand;
        private RelayCommand checkCommand; 
        #endregion

        public ManageViolationViewModel()
        {
            currentEmployee = new Employee();

            IList<int> list = new List<int>();
            for (int i = 1; i <= 5; i++) list.Add(i);
            rateSeverity = new CollectionView(list);
            rateRepetition = new CollectionView(list);
            rateSolution = new CollectionView(list);

            Severity = 1;
            Repetition = 1;
            Solution = 1;
        }

        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }


        public Decimal ViolationScore
        {
            get { return violationScore; }
            set { violationScore = value; OnPropertyChanged("ViolationScore"); }
        }


        public string EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value; OnPropertyChanged("EmployeeId"); }
        }

        public int Severity
        {
            get { return severity; }
            set { severity = value; OnPropertyChanged("Severity"); }
        }


        public int Repetition
        {
            get { return repetition; }
            set { repetition = value; OnPropertyChanged("Repetition"); }
        }


        public int Solution
        {
            get { return solution; }
            set { solution = value; OnPropertyChanged("Solution"); }
        }


        public CollectionView RateSeverity
        {
            get { return rateSeverity; }
        }

        public CollectionView RateRepetition
        {
            get { return rateRepetition; }
        }

        public CollectionView RateSolution
        {
            get { return rateSolution; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged("Description"); }
        }


        public RelayCommand SubmitCommand
        {
            get
            {
                submitCommand = new RelayCommand(InsertViolation, CanExecute);
                return submitCommand;
            }
            set
            {
                submitCommand = value;
                OnPropertyChanged("SubmitCommand");
            }
        }

        public RelayCommand CheckCommand
        {
            get
            {
                checkCommand = new RelayCommand(CheckViolation, CanExecute);
                return checkCommand;
            }
            set
            {
                checkCommand = value;
                OnPropertyChanged("CheckCommand");
            }
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

        private void InsertViolation(object parameter)
        {
            if(ValidateEmployee(EmployeeId))
            {
                if (Description != null)
                {
                    using (KongBuBankEntities db = new KongBuBankEntities())
                    {
                        Employee employee = db.Employees.Find(EmployeeId);
                        int score = CalculateViolation();
                        employee.ViolationScore += score;

                        ViolationReport report = new ViolationReport();
                        report.ViolationId = "VR" + IdFormat(Count("ViolationReport") + 1);
                        report.EmployeeId = employee.EmployeeId;
                        report.Score = score;
                        report.Description = Description;
                        report.Date = DateTime.Now;
                        report.IsActive = true;

                        db.ViolationReports.Add(report); 
                        db.SaveChanges(); 

                        CurrentEmployee = employee;

                        MessageBox.Show("Violation Score Added! [" + CurrentEmployee.ViolationScore + "]", "Success");
                        
                        if(CurrentEmployee.ViolationScore > VIOLATIONTHRESHOLD)
                        {
                            MessageBox.Show("Violation Threshold Exceeded. Please send firing request to Manager!", "Warning");
                        }
                    }
                }
                else MessageBox.Show("Description must be filled!", "Error"); 
            }
        }

        private void CheckViolation(object parameter)
        {
            if(ValidateEmployee(EmployeeId))
            {
                using (KongBuBankEntities db = new KongBuBankEntities())
                {
                    Employee employee = (from x in db.Employees
                                         where x.EmployeeId == EmployeeId
                                         select x).First();

                    ViolationScore = employee.ViolationScore;
                    CurrentEmployee = employee; 

                }
            }
        }

        private int CalculateViolation()
        {
            int total = 0;
            total += Severity;
            total += Repetition * 2;
            total += Solution;
            return total; 
        }

        private bool ValidateEmployee(string _id)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                Employee employee = (from x in db.Employees
                                     where x.EmployeeId == _id
                                     select x).FirstOrDefault();
                if (employee != null)
                {
                    if (employee.IsActive)
                    {
                        return true; 
                    }
                    else MessageBox.Show("Employee not active!", "Error");
                }
                else MessageBox.Show("Employee not found!", "Error");
                return false; 
            }
        }
    }
}
