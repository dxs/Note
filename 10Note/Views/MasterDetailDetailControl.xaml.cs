using _10Note.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;

namespace _10Note.Views
{
    public sealed partial class MasterDetailDetailControl : UserControl
    {
        public SampleModel MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SampleModel; }
            set { SetValue(MasterMenuItemProperty, value); Setup(); }
        }

        public static DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem",typeof(SampleModel),typeof(MasterDetailDetailControl),new PropertyMetadata(null));

        public MasterDetailDetailControl()
        {
            InitializeComponent();
        }

        private async void Setup()
        {
            await LoadInkAsync();
        }

        private async Task SaveInkAsync()
        {
            if (DrawInkCanvas.InkPresenter.StrokeContainer.GetStrokes().Count > 0)
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(MasterMenuItem.ID.ToString() + ".gif",CreationCollisionOption.ReplaceExisting);

                if (null != file)
                {
                    using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        // This single method will get all the strokes and save them to the file
                        await DrawInkCanvas.InkPresenter.StrokeContainer.SaveAsync(stream);
                    }
                }
            }
        }

        private async Task LoadInkAsync()
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(MasterMenuItem.ID.ToString() + ".gif");
                if (null != file)
                {
                    using (var stream = await file.OpenSequentialReadAsync())
                    {
                        // Just like saving, it's only one method to load the ink into the canvas
                        await DrawInkCanvas.InkPresenter.StrokeContainer.LoadAsync(stream);
                    }
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("No File found");
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await SaveInkAsync();
        }

        private void ConvertTextButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
