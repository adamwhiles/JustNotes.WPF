using JustNotes.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JustNotes.WPF.ViewModel.Commands
{
    public class LoginCommand : ICommand
    {
        public LoginViewModel VM { get; set; }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public LoginCommand(LoginViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            User user = parameter as User;

            if (user == null) return false;
            if(string.IsNullOrEmpty(user.Email)) return false;
            if(string.IsNullOrEmpty(user.Password)) return false;
            return true;
        }

        public void Execute(object parameter)
        {
            VM.Login();
        }
    }
}
