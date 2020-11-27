using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TPA_Desktop_NT20_2.Models; 

namespace TPA_Desktop_NT20_2.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name");  }
        }

    }
}
