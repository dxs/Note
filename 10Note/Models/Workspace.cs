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
		DispatcherTimer AutoSaveTimer;
		private string WName = "default";
		private ObservableCollection<Note> noteCollection = new ObservableCollection<Note>();
		public ObservableCollection<Note> NoteCollection
		{
			get { return noteCollection; }
			set { noteCollection = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoteCollection))); }
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public Workspace() : this("default")
		{

		}

		public Workspace(string workspaceName)
		{
			WName = workspaceName;
			ApplicationData.Current.DataChanged += Current_DataChanged;
			Work();
		}

		private void Current_DataChanged(ApplicationData sender, object args)
		{
			AutoSaveTimer.Stop();
			Work();
		}

		private async void Work()
		{
			/*Try to Load*/
			await LoadWorkspace().ConfigureAwait(false);

			AutoSaveTimer = new DispatcherTimer
			{
				Interval = new TimeSpan(0, 0, 3)
			};
			AutoSaveTimer.Tick += async (e, o) => await SaveWorkspace().ConfigureAwait(false);
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
				NoteCollection = await SettingsStorageExtensions.ReadAsync<ObservableCollection<Note>>(ApplicationData.Current.RoamingFolder, WName);
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
				await SettingsStorageExtensions.SaveAsync<ObservableCollection<Note>>(ApplicationData.Current.RoamingFolder, WName, NoteCollection);
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
