using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LR1_3kyrs_NOVIKOVA_BAI24.Helper
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;     // execute — делегат, который выполняется при нажатии
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);// Если canExecute == null — возвращаем true (кнопка всегда активна)
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
