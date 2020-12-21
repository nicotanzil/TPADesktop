using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Commands;
using TPA_Desktop_NT20_2.Views.ManagerFolder;

namespace TPA_Desktop_NT20_2.ViewModels
{
    class ManagerAuthenticationViewModel : BaseViewModel
    {
        #region attributes
        private string message;
        private Manager currentManager;
        private RelayCommand loginCommand;
        #endregion


        public ManagerAuthenticationViewModel()
        {
            Name = "ManagerAuthentication";
            CurrentManager = new Manager();
            CurrentManager.Email = "haopa@mail.com";
            CurrentManager.Password = "password"; 
        }

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }

        public Manager CurrentManager
        {
            get { return currentManager; }
            set { currentManager = value; OnPropertyChanged("CurrentManager"); }
        }

        public RelayCommand LoginCommand
        {
            get
            {
                loginCommand = new RelayCommand(LoadData, CanExecute);
                return loginCommand;
            }
            set
            {
                loginCommand = value;
                OnPropertyChanged("LoginCommand");
            }
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

        private void LoadData(object parameter)
        {
            KongBuBankEntities db = KongBuBankEntities.Instance; 
            Manager query = (from x in db.Managers
                                where x.Email == CurrentManager.Email
                                where x.Password == CurrentManager.Password
                                select x).FirstOrDefault();

            if (query != null)
            {
                CurrentManager = query;
                RedirectPage();
            }
            else
            {
                //Account not found
                Message = "Invalid Email / Password!";
                MessageBox.Show("Invalid Email / Password!", "Authentication Error");
            }
        }

        private void RedirectPage()
        {
            ManagerWindow win = new ManagerWindow(CurrentManager);
            win.ShowDialog(); 
        }
    }
}
