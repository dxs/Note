using _10Note.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace _10Note.Models
{
	public class Workspace
	{
		public string WName = "default";
		public ObservableCollection<Note> NoteCollection = new ObservableCollection<Note>();

		public Workspace(string workspaceName = "default")
		{
			WName = workspaceName;
		}

		#region Collection Operation
		public void AddNote(Note _note)
		{
			NoteCollection.Insert(0, _note);
		}

		public void RemoveNote(int index)
		{
			NoteCollection.RemoveAt(index);
		}

		public void RemoveNote(Note _note)
		{
			NoteCollection.Remove(_note);
		}

		#endregion

		#region WorkSpace Operation
		public async Task<bool> LoadWorkspace()
		{
			try
			{
				NoteCollection = await SettingsStorageExtensions.ReadAsync<ObservableCollection<Note>>(ApplicationData.Current.LocalFolder, WName);
			}
			catch(Exception e)
			{
				return false;
			}
			return true;
		}

		public async Task<bool> SaveWorkspace()
		{
			try
			{
				await SettingsStorageExtensions.SaveAsync<ObservableCollection<Note>>(ApplicationData.Current.LocalFolder, WName, NoteCollection);
			}
			catch(Exception e)
			{
				return false;
			}
			return true;
		}
		#endregion

	}
}
