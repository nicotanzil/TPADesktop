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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using TPA_Desktop_NT20_2.Models.SQL; 


namespace TPA_Desktop_NT20_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable = DbManager.Get("SELECT * FROM Account WHERE name='Nico'");
            string name = dataTable.Rows[0]["name"].ToString();
            string balance = dataTable.Rows[0]["balance"].ToString();
            LabelOne.Content = name + " " + balance; 
        }
    }
}
