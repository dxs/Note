using Note.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace _10Notes
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		NoteHandler Notes = new NoteHandler();

		public MainPage()
		{
			this.InitializeComponent();
			ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
			formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
			CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
			coreTitleBar.ExtendViewIntoTitleBar = true;
			SetupBackup();
		}

		private async void SetupBackup()
		{
			Notes = await Filer.DeserelizeDataFromJson();
			if (Notes.Notes.Count > 0)
				Notes.SelectedNote = Notes.Notes[0];

			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = new TimeSpan(0, 0, 5);
			timer.Tick += async (e, o) =>
			{
				await Filer.SerializeDataToJson(Notes);
			};
			timer.Start();
		}

		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int index = (sender as ListView).SelectedIndex;
			if (index < 0)
				index = 0;
			if (index < Notes.Notes.Count)
				Notes.SelectedNote = Notes.Notes[index];
			else
				Notes.SelectedNote = new ANote();
		}

		private void Button_Delete(object sender, RoutedEventArgs e)
		{
			try
			{
				Notes.Notes.Remove(Notes.SelectedNote);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			try
			{
				if (Notes.Notes.Count > 0)
					Notes.SelectedNote = Notes.Notes[0];
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		private void Button_Add(object sender, RoutedEventArgs e)
		{
			Notes.AddEmptyNote();
		}
	}
}