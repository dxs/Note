using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Note.Logic
{
	public class ANote : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private DateTime lastModified;
		public DateTime LastModified
		{
			get { return lastModified; }
			set { lastModified = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastModified))); }
		}

		private string title = string.Empty;
		public string Title
		{
			get { return title; }
			set { title = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title))); }
		}

		public ANote()
		{
			LastModified = DateTime.Now;
		}		
	}

	public class TitleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			string v = (string)value;

			if (v == string.Empty)
				return "Sans titre";
			return v;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
