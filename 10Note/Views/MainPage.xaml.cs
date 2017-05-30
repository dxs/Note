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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace _10Note
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		Workspace workspace;

        public MainPage()
        {
			workspace = new Workspace();
			//workspace.AddNote(new Note("Titre","Ceci est une note"));
			this.InitializeComponent();
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
			workspace.AddNote(new Note());
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
