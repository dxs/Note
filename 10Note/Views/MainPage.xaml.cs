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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace _10Note
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		NativeAdsManager nativeAdsManager;
		Workspace workspace;

        public MainPage()
        {
			workspace = new Workspace();
			this.InitializeComponent();
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

			//if sponsoredBy is not null show sponsoredBy textbox
			//var sponsoredBy = nativeAd.SponsoredBy;
			//if (!string.IsNullOrEmpty(sponsoredBy))
			//{
			//	SponsoredBy.Text = sponsoredBy;
			//	SponsoredBy.Visibility = Visibility.Visible;
			//}

			//if CallToAction is not null update Button
			//var callToAction = nativeAd.CallToAction;
			//if (!string.IsNullOrEmpty(callToAction))
			//{
			//	CallToAction.Content = callToAction;
			//	CallToAction.Visibility = Visibility.Visible;
			//}

			// Assets consists further information about Ad
			var assets = nativeAd.AdditionalAssets;

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

			// There might be multiple main images sent by the server
			//var mainImages = nativeAd.MainImages;
			//if (mainImages.Count > 0)
			//{
			//	var mainImage = mainImages[0];
			//	var bitmapImage = new BitmapImage();
			//	bitmapImage.UriSource = new Uri(mainImage.Url);
			//	MainImage.Source = bitmapImage;
			//	// Best view when using the Height and Width of the image given
			//	MainImage.Height = mainImage.Height;
			//	MainImage.Width = mainImage.Width;

			//	MainImageContainer.Visibility = Visibility.Visible;
			//}

			// It is required to show the AdIcon in your container
			NativeAdContainer.Children.Add(nativeAd.AdIcon);

			// Register any xaml framework element for clicks/impressions
			nativeAd.RegisterAdContainer(NativeAdContainer);
		}

		private void CommandBar_Closing(object sender, object e)
		{
			CommandBar cb = sender as CommandBar;
			if (cb != null) cb.Background.Opacity = 0.3;

		}

		private void CommandBar_Opening(object sender, object e)
		{
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
	}
}
