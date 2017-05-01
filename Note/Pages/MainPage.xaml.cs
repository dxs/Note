using Note.Logic;
using System;
using System.Diagnostics;
using System.Numerics;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Note
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
			applyAcrylicAccent(MainGrid);
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

		#region BackDropBrush

		Compositor _compositor;
		SpriteVisual _hostSprite;

		private void applyAcrylicAccent(Panel e)
		{
			_compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

			_hostSprite = _compositor.CreateSpriteVisual();
			_hostSprite.Size = new Vector2((float)MainGrid.ActualWidth, (float)MainGrid.ActualHeight);

			ElementCompositionPreview.SetElementChildVisual(
				MainGrid, _hostSprite);
			_hostSprite.Brush = _compositor.CreateHostBackdropBrush();
		}

		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (_hostSprite != null)
				_hostSprite.Size = e.NewSize.ToVector2();
		}

		#endregion

		private void Button_Delete(object sender, RoutedEventArgs e)
		{
			try
			{
				Notes.Notes.Remove(Notes.SelectedNote);
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			try
			{
				if (Notes.Notes.Count > 0)
					Notes.SelectedNote = Notes.Notes[0];
			}
			catch(Exception ex)
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
