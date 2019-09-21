using System;
using System.Windows.Input;

namespace BillingManagement.Utils
{
    internal class RelayCommand : ICommand 
    {
        private Func<object, bool> _canExecute = (_) => true;
        private Action<object> _doExecute = (_) => { };
        private bool _previousCanExecute = false;
        public RelayCommand(Func<object, bool> canExecute, Action<object> execute )
        {
            _canExecute = canExecute;
            _doExecute = execute;
        }

        public RelayCommand(Action<object> execute)
        {
            _doExecute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            bool canExecute = _canExecute(parameter);

            if (_previousCanExecute == canExecute)
                return canExecute;
            
            _previousCanExecute = canExecute;

            if (CanExecuteChanged == null)
                return canExecute;

            CanExecuteChanged(this, new EventArgs());
            return canExecute;

        }

        public void Execute(object parameter)
        {
            _doExecute(parameter);
        }
    }
}
