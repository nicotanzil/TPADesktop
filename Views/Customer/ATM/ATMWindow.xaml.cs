using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TPA_Desktop_NT20_2.Models;
using TPA_Desktop_NT20_2.ViewModels.Customer.ATM;

namespace TPA_Desktop_NT20_2.Views.Customer.ATM
{
    /// <summary>
    /// Interaction logic for ATMWindow.xaml
    /// </summary>
    public partial class ATMWindow : Window
    {
        public ATMWindow(IndividualAccount account)
        {
            InitializeComponent();

            DataContext = new ATMViewModel(account); 
        }
    }
}
