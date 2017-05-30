using _10Note.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace _10Note.Models
{
	public class Workspace : INotifyPropertyChanged
	{
		public string WName = "default";
		private ObservableCollection<Note> noteCollection = new ObservableCollection<Note>();
		public ObservableCollection<Note> NoteCollection
		{
			get { return noteCollection; }
			set { noteCollection = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoteCollection))); }
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public Workspace(string workspaceName = "default")
		{
			WName = workspaceName;

			Work();
		}

		private async void Work()
		{
			/*Try to Load*/
			bool loaded = await LoadWorkspace();

			DispatcherTimer AutoSaveTimer = new DispatcherTimer
			{
				Interval = new TimeSpan(0, 0, 3)
			};
			AutoSaveTimer.Tick += async (e, o) => await SaveWorkspace();
			AutoSaveTimer.Start();
		}

		#region Collection Operation
		public void AddNote(Note _note)
		{
			if (NoteCollection.Count <= 0)
				NoteCollection.Add(_note);
			else
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
				if (NoteCollection == null)
					NoteCollection = new ObservableCollection<Note>();
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
