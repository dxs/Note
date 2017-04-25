using Microsoft.Graphics.Canvas.Effects;
using Note.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
			_compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
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
			Notes.SelectedNote = Notes.Notes[(sender as ListView).SelectedIndex];
		}

		#region BackDropBrush

		Compositor _compositor;
		SpriteVisual _hostSprite;

		private void applyAcrylicAccent(Panel e)
		{
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

		private void Button_Add(object sender, RoutedEventArgs e)
		{
			Notes.AddEmptyNote();
		}
	}
}
