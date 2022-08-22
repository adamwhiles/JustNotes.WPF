using JustNotes.WPF.Models;
using JustNotes.WPF.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustNotes.WPF.ViewModel
{
    public class NotesViewModel
    {
        public ObservableCollection<Note> Notes { get; set; }

		private Note selectedNote;

		public Note SelectedNote
		{
			get { return selectedNote; }
			set 
			{ 
				selectedNote = value; 
				//TODO: get note details
			}
		}

		public NewNoteCommand NewNoteCommand { get; set; }

		public NotesViewModel()
		{
			NewNoteCommand = new NewNoteCommand(this);
		}
		

	}
}
