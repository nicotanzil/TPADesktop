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
using TPA_Desktop_NT20_2.ViewModels.EmployeeGeneral;

namespace TPA_Desktop_NT20_2.Views.EmployeeGeneral
{
    /// <summary>
    /// Interaction logic for ReportItemWindow.xaml
    /// </summary>
    public partial class ReportItemWindow : Window
    {
        public ReportItemWindow(Employee _employee)
        {
            InitializeComponent();

            DataContext = new ReportItemViewModel(_employee, this); 
        }
    }
}
