using JustNotes.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JustNotes.WPF.ViewModel.Commands
{
    public class CommitEditCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public NotesViewModel VM { get; set; }

        public CommitEditCommand(NotesViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Note note = parameter as Note;
            if(note != null) VM.StopEditing(note);
        }
    }
}
