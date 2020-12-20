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
using TPA_Desktop_NT20_2.ViewModels.HRM;

namespace TPA_Desktop_NT20_2.Views.HRM
{
    /// <summary>
    /// Interaction logic for HRMWindow.xaml
    /// </summary>
    public partial class HRMWindow : Window
    {
        public HRMWindow(Employee _employee)
        {
            InitializeComponent();

            DataContext = new HRMViewModel(_employee); 
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
