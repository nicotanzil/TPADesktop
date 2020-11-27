using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using TPA_Desktop_NT20_2.Models;

namespace TPA_Desktop_NT20_2.ViewModels.Commands
{
    public class RelayCommand : ICommand
    {

        Action<object> executeMethod;
        Func<object, bool> canExecuteMethod;

        public RelayCommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true; 
        }

        public void Execute(object parameter)
        {
            executeMethod(parameter); 
        }
    }
}
