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
    class RemoveCandidateViewModel : BaseViewModel
    {
        #region Attributes
        private string candidateId;
        private Candidate currentCandidate;
        private RelayCommand viewCommand;
        private RelayCommand removeCommand; 
        #endregion

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

        public RelayCommand RemoveCommand
        {
            get
            {
                removeCommand = new RelayCommand(Remove, CanExecute);
                return removeCommand;
            }
            set
            {
                removeCommand = value;
                OnPropertyChanged("RemoveCommand");
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

        private void Remove(object parameter)
        {
            if (IsCandidateValid(CandidateId))
            {
                using (KongBuBankEntities db = new KongBuBankEntities())
                {
                    Candidate candidate = db.Candidates.Find(CurrentCandidate.CandidateId);
                    candidate.Status = "Rejected";

                    db.SaveChanges();
                    MessageBox.Show("Candidate: " + candidate.CandidateId + " Removed!");
                    CurrentCandidate = null; 
                }
            }
            else MessageBox.Show("Candidate not found!", "Error"); 
        }

        private bool IsCandidateValid(string _id)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                Candidate query = (from x in db.Candidates
                                   where x.CandidateId == _id
                                   select x).FirstOrDefault();
                if(query == null)
                {
                    MessageBox.Show("Candidate not found!", "Error");
                    return false;
                }
                else if(query.Status == "Rejected")
                {
                    MessageBox.Show("Candidate already removed!", "Error");
                    return false; 
                }
                else if(query.Status == "Accepted")
                {
                    MessageBox.Show("Candidate already accepted!", "Error"); 
                }
                return true; 
            }
        }

    }
}
