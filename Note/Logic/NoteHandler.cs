using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note.Logic
{
	public class NoteHandler
	{
		public ObservableCollection<ANote> Notes = new ObservableCollection<ANote>();

		public NoteHandler()
		{

		}

		public void AddEmptyNote()
		{
			Notes.Insert(0, new ANote());
		}
	}
}
