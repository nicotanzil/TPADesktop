using Syncfusion.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;

namespace TPA_Desktop_NT20_2.ViewModels.Maintenance
{
    class AddMaintenanceScheduleViewModel : BaseViewModel
    {
        #region Attributes
        private Employee currentEmployee;
        private MaintenanceSchedule schedule;
        private string reportId;
        private RelayCommand submitCommand;
        #region StartTime 
        private DateTime startTime;
        private string startDate;
        private int startMinute;
        private int startHour;
        #endregion

        #region EndTime
        private DateTime endTime;
        private string endDate;
        private int endMinute;
        private int endHour;
        #endregion

        private CollectionView minuteStartTypes;
        private CollectionView hourStartTypes;

        private CollectionView minuteEndTypes;
        private CollectionView hourEndTypes;

        private Decimal estimateCost;
        private string description;
        #endregion


        public AddMaintenanceScheduleViewModel(Employee _employee)
        {
            CurrentEmployee = _employee;
            #region Date List
            IList<int> minuteList = new List<int>();
            IList<int> hourList = new List<int>();
            for (int i = 0; i < 60; i++) minuteList.Add(i);
            for (int i = 0; i < 24; i++) hourList.Add(i);
            minuteStartTypes = new CollectionView(minuteList);
            hourStartTypes = new CollectionView(hourList);

            minuteEndTypes = new CollectionView(minuteList);
            hourEndTypes = new CollectionView(hourList);

            StartTime = DateTime.Now;
            EndTime = DateTime.Now; 
            #endregion
        }
        #region Encap
        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        public MaintenanceSchedule Schedule
        {
            get { return schedule; }
            set { schedule = value; OnPropertyChanged("Schedule"); }
        }

        public RelayCommand SubmitCommand
        {
            get
            {
                submitCommand = new RelayCommand(SubmitSchedule, CanExecute);
                return submitCommand;
            }
            set
            {
                submitCommand = value;
                OnPropertyChanged("SubmitCommand");
            }
        }

        public string ReportId
        {
            get { return reportId; }
            set { reportId = value; OnPropertyChanged("ReportId"); }
        }

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; OnPropertyChanged("StartTime"); }
        }

        public int StartMinute
        {
            get { return startMinute; }
            set { startMinute = value; OnPropertyChanged("StartMinute"); }
        }

        public int StartHour
        {
            get { return startHour; }
            set { startHour = value; OnPropertyChanged("StartHour"); }
        }

        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; OnPropertyChanged("EndTime"); }
        }

        public int EndMinute
        {
            get { return endMinute; }
            set { endMinute = value; OnPropertyChanged("EndMinute"); }
        }

        public int EndHour
        {
            get { return endHour; }
            set { endHour = value; OnPropertyChanged("EndHour"); }
        }

        public CollectionView MinuteStartTypes
        {
            get { return minuteStartTypes; }
        }

        public CollectionView HourStartTypes
        {
            get { return hourStartTypes; }
        }

        public CollectionView MinuteEndTypes
        {
            get { return minuteEndTypes; }
        }

        public CollectionView HourEndTypes
        {
            get { return hourEndTypes; }
        }

        public Decimal EstimateCost
        {
            get { return estimateCost; }
            set { estimateCost = value; OnPropertyChanged("EstimateCost"); }
        }

        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged("Description"); }
        }
        #endregion

        private bool CanExecute(object parameter)
        {
            return true;
        }

        private void SubmitSchedule(object parameter)
        {
             if(ValidateDateTime())
            {
                if (ValidateReportId(ReportId))
                {
                    if (EstimateCost >= 0)
                    {
                        if (Description != null)
                        {
                            using (KongBuBankEntities db = new KongBuBankEntities())
                            {
                                //Create new MaintenanceSchedule
                                MaintenanceSchedule schedule = new MaintenanceSchedule();
                                schedule.ReportId = ReportId;
                                schedule.Status = "Pending"; 
                                schedule.StartDate = StartTime;
                                schedule.EndDate = EndTime;
                                schedule.EstimateCost = EstimateCost;
                                schedule.Description = Description;

                                //Update MaintenanceReport Status
                                MaintenanceReport report = db.MaintenanceReports.Find(schedule.ReportId);
                                report.Status = true;

                                db.MaintenanceSchedules.Add(schedule);

                                Console.WriteLine(schedule.ReportId);
                                Console.WriteLine(schedule.StartDate); 

                                db.SaveChanges();

                                MessageBox.Show("Maintenance Schedule Set Success!", "Success"); 
                            }  
                        }
                        else MessageBox.Show("Description must be filled!", "Error"); 
                    }
                    else MessageBox.Show("Cost Amount Invalid!", "Error"); 
                }
            }
        }

        private bool ValidateDateTime()
        {
            StartTime = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, StartHour, StartMinute, 0);
            EndTime = new DateTime(EndTime.Year, EndTime.Month, EndTime.Day, EndHour, EndMinute, 0);
            int gap = DateTime.Compare(EndTime, StartTime);
            if (gap > 0) return true; 
            else MessageBox.Show("Invalid Time Sequence!", "Error");
            return false; 
        }

        private bool ValidateReportId(string _reportId)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                MaintenanceReport report = db.MaintenanceReports.Find(_reportId);
                if (report == null)
                {
                    MessageBox.Show("Maintenance Report Not Found!", "Error");
                    return false; 
                }
                if (report.Status)
                {
                    MessageBox.Show("Maintenance Report has already been scheduled!", "Error");
                    return false; 
                }
                return true; 
            }
        }

    }
}
