using System;
using System.Windows.Input;

namespace StrawberryMusic.Command
{
    class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action<object> executeMethod;

        public RelayCommand(Action<object> _executeMethod)
        {
            executeMethod = _executeMethod;
        }

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
