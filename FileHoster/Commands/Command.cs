using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace FileHoster.Commands
{
    public sealed class Command : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;
        public Command(Action<object> execute,Func<object,bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public void Execute(object arg)
        {
            _execute.Invoke(arg);
        }
        public bool CanExecute(object arg)
        {
            return _canExecute.Invoke(arg);
        }
    }
}
