using _10Note.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Advertising.WinRT.UI;
using _10Note.Helper;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using System.Numerics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace _10Note
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		readonly NativeAdsManager nativeAdsManager;
		readonly Workspace workspace;

        public MainPage()
        {
			workspace = new Workspace();
			this.InitializeComponent();
			applyAcrylicAccent(null);
			nativeAdsManager = new NativeAdsManager(ID.AppId, ID.MainAdBannerId);
			nativeAdsManager.RequestAd();
			nativeAdsManager.AdReady += NativeAd_OnAdReady;
			nativeAdsManager.ErrorOccurred += NativeAd_ErrorOccurred;
		}

		private void NativeAd_ErrorOccurred(object sender, AdErrorEventArgs e)
		{
			//TODO
		}

		private void NativeAd_OnAdReady(object sender, object e)
		{
			var codacy = sender as NativeAdsManager;
			NativeAd nativeAd = (NativeAd)e;
			TitleBox.Visibility = Visibility.Visible;
			TitleBox.Text = nativeAd.Title;

			//if description is not null show description textbox
			var description = nativeAd.Description;
			if (!string.IsNullOrEmpty(description))
			{
				DescriptionBox.Text = nativeAd.Description;
				DescriptionBox.Visibility = Visibility.Visible;
			}

			// Loading images
			var icon = nativeAd.IconImage;
			if (icon != null)
			{
				var bitmapImage = new BitmapImage();
				bitmapImage.UriSource = new Uri(nativeAd.IconImage.Url);
				IconImage.Source = bitmapImage;
				// Best view when using the Height and Width of the image given
				IconImage.Height = nativeAd.IconImage.Height;
				IconImage.Width = nativeAd.IconImage.Width;

				IconImageContainer.Visibility = Visibility.Visible;
			}

			// It is required to show the AdIcon in your container
			NativeAdContainer.Children.Add(nativeAd.AdIcon);

			// Register any xaml framework element for clicks/impressions
			nativeAd.RegisterAdContainer(NativeAdContainer);
		}

		private void CommandBar_Closing(object sender, object e)
		{
			e.ToString();
			CommandBar cb = sender as CommandBar;
			if (cb != null) cb.Background.Opacity = 0.3;

		}

		private void CommandBar_Opening(object sender, object e)
		{
			e.ToString();
			CommandBar cb = sender as CommandBar;
			if (cb != null) cb.Background.Opacity = 1.0;

		}

		private void Add_Click(object sender, RoutedEventArgs e)
		{
			workspace.AddNote(new Note("Title","Body"));
		}

		private void Remove_Click(object sender, RoutedEventArgs e)
		{
			workspace.RemoveNote(MasterViewList.SelectedItem as Note);
		}

		private async void Save_Click(object sender, RoutedEventArgs e)
		{
			await workspace.SaveWorkspace();
		}

		#region TextBlock Tab

		private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Tab)
			{
				var textBox = (TextBox)e.OriginalSource;
				var originalStartPosition = textBox.SelectionStart;

				// SelectionStart treats "\r\n" as a single character.
				// So if you've a TextBox with just the text "\r\n" and the cursor is at the end, SelectionStart is
				// - for a UWP-app: 1
				// - for a WPF-app: 2
				// => so for a UWP-app, we need to solve this:
				var startPosition = GetRealStartPositionTakingCareOfNewLines(originalStartPosition, textBox.Text);



				var beforeText = textBox.Text.Substring(0, startPosition);
				var afterText = textBox.Text.Substring(startPosition, textBox.Text.Length - startPosition);
				var tabSpaces = 4;
				var tab = new string(' ', tabSpaces);
				textBox.Text = beforeText + tab + afterText;
				textBox.SelectionStart = originalStartPosition + tabSpaces;

				e.Handled = true;
			}
		}

		private int GetRealStartPositionTakingCareOfNewLines(int startPosition, string text)
		{
			int newStartPosition = startPosition;
			int currentPosition = 0;
			bool previousWasReturn = false;
			foreach (var character in text)
			{
				if (character == '\n')
					if (previousWasReturn)
						newStartPosition++;
				if (newStartPosition <= currentPosition)
					break;

				if (character == '\r')
					previousWasReturn = true;
				else
					previousWasReturn = false;

				currentPosition++;
			}
			return newStartPosition;
		}


		#endregion

		#region ACRYLIC

		private void applyAcrylicAccent(Panel e)
		{
			_compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
			_hostSprite = _compositor.CreateSpriteVisual();
			_hostSprite.Size = new Vector2((float)TransGrid.ActualWidth, (float)TransGrid.ActualHeight);

			ElementCompositionPreview.SetElementChildVisual(
					TransGrid, _hostSprite);
			_hostSprite.Brush = _compositor.CreateHostBackdropBrush();
		}
		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (_hostSprite != null)
				_hostSprite.Size = e.NewSize.ToVector2();
		}
		Compositor _compositor;
		SpriteVisual _hostSprite;

		#endregion

	}
}
