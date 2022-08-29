using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JustNotes.WPF.ViewModel.Commands
{
    public class DeleteCommand : ICommand
    {
        public NotesViewModel VM { get; set; }

        public event EventHandler CanExecuteChanged;

        public DeleteCommand(NotesViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.DeleteNote();
        }
    }
}
