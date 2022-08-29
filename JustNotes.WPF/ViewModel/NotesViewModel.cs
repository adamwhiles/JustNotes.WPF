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
		public DeleteCommand DeleteCommand { get; set; }

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
			DeleteCommand = new DeleteCommand(this);

			Notes = new ObservableCollection<Note>();

			IsVisible = Visibility.Collapsed;

			GetNotes();
        }
		
		public async void NewNote(string userId)
		{
			Note note = new Note() {
				UserId = App.UserId,
				CreatedDate = DateTime.Now,
				UpdatedDate = DateTime.Now,
				Title = "New Note " + DateTime.Now
			};

			await DBHelper.Insert(note);
			 await GetNotes();
		}

		public async 
		Task
GetNotes(bool initialRequest = false, Note currentNote = null)
		{
			var notes = (await DBHelper.Read<Note>()).Where(n => n.UserId == App.UserId);
			Notes.Clear();

			foreach(Note note in notes)
			{
				Notes.Add(note);
			}

			if(initialRequest)
			{
                this.selectedNote = Notes.FirstOrDefault();
                SelectedNoteChanged?.Invoke(this, EventArgs.Empty);
            }

			if(currentNote != null)
			{
				this.selectedNote = currentNote;
                SelectedNoteChanged?.Invoke(this, EventArgs.Empty);
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

        public async void StopEditing(Note note)
        {
            IsVisible = Visibility.Collapsed;
			await DBHelper.Update(note);
			await GetNotes(currentNote: Notes.Single(n => n.Id == note.Id));
        }

		public async void DeleteNote()
		{
			await DBHelper.Delete(selectedNote);
			await GetNotes();
		}

    }
}
