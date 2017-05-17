using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Note.Logic
{
	public class NoteHandler : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[DataMember]
		public ObservableCollection<ANote> Notes = new ObservableCollection<ANote>();
		private ANote selectedNote = null;
		public ANote SelectedNote
		{
			get { return selectedNote; }
			set { selectedNote = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNote))); }
		}

		public NoteHandler()
		{

		}

		public void AddEmptyNote()
		{
			Notes.Insert(0, new ANote() { Title = "Sans titre", Body = string.Empty });
		}
	}
}
