using JustNotes.WPF.Models;
using JustNotes.WPF.ViewModel.Commands;
using JustNotes.WPF.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JustNotes.WPF.ViewModel
{
    public class NotesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Note> Notes { get; set; }

		private Note selectedNote;

        public NewNoteCommand NewNoteCommand { get; set; }
        public EditCommand EditCommand { get; set; }
        public CommitEditCommand CommitEditCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

		public event EventHandler SelectedNoteChanged;

		public Note SelectedNote
		{
			get { return selectedNote; }
			set 
			{ 
				selectedNote = value;
				OnPropertyChanged("SelectedNote");
				SelectedNoteChanged?.Invoke(this, EventArgs.Empty);
			}
		}
		private Visibility isVisible;

		public Visibility IsVisible
		{
			get { return isVisible; }
			set 
			{ 
				isVisible = value;
				OnPropertyChanged("IsVisible");
			}
		}

		public NotesViewModel()
		{
			NewNoteCommand = new NewNoteCommand(this);
			EditCommand = new EditCommand(this);
			CommitEditCommand = new CommitEditCommand(this);

			Notes = new ObservableCollection<Note>();

			IsVisible = Visibility.Collapsed;

			GetNotes();
        }
		
		public void NewNote(int userId)
		{
			Note note = new Note() {
				UserId = userId,
				CreatedDate = DateTime.Now,
				UpdatedDate = DateTime.Now,
				Title = "New Note " + DateTime.Now
			};

			DBHelper.Insert(note);
			GetNotes();
		}

		private void GetNotes()
		{
			var notes = DBHelper.Read<Note>().Where(n => n.UserId == 1);
			Notes.Clear();

			foreach(Note note in notes)
			{
				Notes.Add(note);
			}
        }

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void StartEditing()
		{
			IsVisible = Visibility.Visible;
		}

        public void StopEditing(Note note)
        {
            IsVisible = Visibility.Collapsed;
			DBHelper.Update(note);
			GetNotes();
        }

    }
}
