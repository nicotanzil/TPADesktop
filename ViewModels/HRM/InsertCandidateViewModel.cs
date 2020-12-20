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
    class InsertCandidateViewModel : BaseViewModel
    {
        #region Attributes
        private string candidateName;
        private RelayCommand registerCommand; 

        #region StartTime 
        private DateTime dob;
        private int minute;
        private int hour;
        #endregion

        private CollectionView minuteTypes;
        private CollectionView hourTypes;
        #endregion

        public InsertCandidateViewModel()
        {
            #region Date List
            IList<int> minuteList = new List<int>();
            IList<int> hourList = new List<int>();
            for (int i = 0; i < 60; i++) minuteList.Add(i);
            for (int i = 0; i < 24; i++) hourList.Add(i);
            minuteTypes = new CollectionView(minuteList);
            hourTypes = new CollectionView(hourList);

            dob = DateTime.Now;
            #endregion
        }

        public string CandidateName
        {
            get { return candidateName; }
            set { candidateName = value; OnPropertyChanged("CandidateName"); }
        }

        public DateTime Dob
        {
            get { return dob; }
            set { dob = value; OnPropertyChanged("Dob"); }
        }

        public int Minute
        {
            get { return minute; }
            set { minute = value; OnPropertyChanged("Minute"); }
        }

        public int Hour
        {
            get { return hour; }
            set { hour = value; OnPropertyChanged("Hour"); }
        }
        public CollectionView MinuteTypes
        {
            get { return minuteTypes; }
        }

        public CollectionView HourTypes
        {
            get { return hourTypes; }
        }

        public RelayCommand RegisterCommand
        {
            get
            {
                registerCommand = new RelayCommand(RegisterCandidate, CanExecute);
                return registerCommand;
            }
            set
            {
                registerCommand = value;
                OnPropertyChanged("RegisterCommand");
            }
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

        private void RegisterCandidate(object parameter)
        {
            if(ValidateDateTime())
            {
                if (CandidateName != null)
                {
                    using (KongBuBankEntities db = new KongBuBankEntities())
                    {
                        Candidate candidate = new Candidate();
                        candidate.CandidateId = "CA" + IdFormat(Count("Candidate") + 1);
                        candidate.Name = CandidateName;
                        candidate.Dob = Dob;
                        candidate.RegistrationDate = DateTime.Now;
                        candidate.Status = "Pending";

                        db.Candidates.Add(candidate);
                        db.SaveChanges();

                        MessageBox.Show("Candidate Insert Success!", "Error");
                        CandidateName = null;
                        Dob = DateTime.Now; 
                    }
                }
                else MessageBox.Show("Name must be filled!", "Error"); 
            }
        }

        private bool ValidateDateTime()
        {
            Dob = new DateTime(Dob.Year, Dob.Month, Dob.Day);
            int gap = DateTime.Compare(DateTime.Now, Dob);
            if (gap > 0) return true;
            else MessageBox.Show("Invalid Time Sequence!", "Error");
            return false;
        }
    }
}
