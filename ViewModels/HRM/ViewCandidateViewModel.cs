using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPA_Desktop_NT20_2.Models;

namespace TPA_Desktop_NT20_2.ViewModels.HRM
{
    class ViewCandidateViewModel : BaseViewModel
    {
        #region Attributes
        private Employee currentEmployee;
        private List<Candidate> listCandidates;
        private ObservableCollection<Candidate> candidates;
        #endregion

        public ViewCandidateViewModel(Employee _employee)
        {
            CurrentEmployee = _employee;
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                listCandidates = new List<Candidate>((from x in db.Candidates
                                                      where x.Status == "Pending"
                                                    select x).ToList());
                Candidates = new ObservableCollection<Candidate>(listCandidates);
            }
        }

        public ObservableCollection<Candidate> Candidates
        {
            get { return candidates; }
            set { candidates = value; OnPropertyChanged("Candidates"); }
        }


        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }
    }
}
