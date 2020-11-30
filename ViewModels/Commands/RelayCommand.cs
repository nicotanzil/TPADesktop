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
        readonly Action<object> execute;
        readonly Predicate<object> canExecute;
        

        public RelayCommand(Action<object> _execute, Predicate<object> _canExecute)
        {
            //void, object
            this.execute = _execute;
            this.canExecute = _canExecute; 
        }

        public RelayCommand(Action<object> execute) : this(execute, null)
        {

        }


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);  
        }

        public void Execute(object parameter)
        {
            execute.Invoke(parameter); 
        }
    }
}
