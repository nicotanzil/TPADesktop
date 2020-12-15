using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;

namespace TPA_Desktop_NT20_2.ViewModels.EmployeeGeneral
{
    class ReportItemViewModel : BaseViewModel
    {
        #region Attributes
        private string itemId;
        private string description;
        private Employee currentEmployee;
        private RelayCommand submitCommand, backCommand;
        private Window win; 
        #endregion

        public ReportItemViewModel(Employee _employee, Window _win)
        {
            currentEmployee = new Employee(); 
            CurrentEmployee = _employee;
            win = _win; 
        }


        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }


        public RelayCommand SubmitCommand
        {
            get 
            {
                submitCommand = new RelayCommand(SubmitReport, CanExecute); 
                return submitCommand; 
            }
            set 
            { 
                submitCommand = value; 
                OnPropertyChanged("SubmitCommand"); 
            }
        }

        public RelayCommand BackCommand
        {
            get
            {
                backCommand = new RelayCommand(CloseWindow, CanExecute);
                return backCommand;
            }
            set
            {
                backCommand = value; 
                OnPropertyChanged("BackCommand");
            }
        }

        private bool CanExecute(object parameter)
        {
            return true; 
        }

        public string ItemId
        {
            get { return itemId; }
            set { itemId = value; OnPropertyChanged("ItemId"); }
        }


        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged("Description"); }
        }

        private void SubmitReport(object parameter)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                if (ItemId != null && Description != null)
                {
                    if (IsItemExists(ItemId))
                    {
                        //Add Maintenance Report
                        MaintenanceReport report = new MaintenanceReport();
                        report.ReportId = "MR" + IdFormat(Count("MaintenanceReport") + 1);
                        report.EmployeeId = CurrentEmployee.EmployeeId;
                        report.ItemId = ItemId;
                        report.ReportDate = DateTime.Now;
                        report.Status = false;
                        report.Description = Description;
                        Console.WriteLine("Employee ID: " + report.EmployeeId);
                        Console.WriteLine("Item ID: " + report.ItemId); 

                        db.MaintenanceReports.Add(report);
                        db.SaveChanges();

                        MessageBox.Show("Report Submitted!", "Success");
                        CloseWindow(null);
                    }
                    else MessageBox.Show("Item ID is not exists!", "Error"); 
                }
                else MessageBox.Show("Item Id and Description must be filled!", "Error"); 
            }
        }

        private bool IsItemExists(string itemId)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                Item item = (from x in db.Items
                             where x.ItemId == itemId
                             select x).FirstOrDefault();
                return item != null ? true : false; 
            }
        }

        private void CloseWindow(object parameter)
        {
            win.Close(); 
        }

    }
}
