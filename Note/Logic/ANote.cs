using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Note.Logic
{
	[DataContract]
	public class ANote : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private DateTime lastModified;
		[DataMember]
		public DateTime LastModified
		{
			get { return lastModified; }
			set { lastModified = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastModified))); }
		}

		private string title = string.Empty;
		[DataMember]
		public string Title
		{
			get { return title; }
			set { title = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title))); }
		}

		private string body = string.Empty;
		[DataMember]
		public string Body
		{
			get { return body; }
			set { body = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Body))); }
		}
		
		public ANote()
		{
			LastModified = DateTime.Now;
		}		
	}

	public class TitlePaneConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			string v = (string)value;

			if (v == string.Empty)
				return "Sans titre";
			if (v.Count() > 30)
			{
				v = v.Substring(0, 27);
				v += "...";
			}
			return v;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return value as string;
		}
	}

	public class TitleContentConverter : IValueConverter
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
			return value as string;
		}
	}

	public class DateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			DateTime t = (DateTime)value;
			if (DateTime.Now.Subtract(t).TotalHours < 24.0)
				return "Aujourd'hui";
			if (DateTime.Now.Subtract(t).TotalHours < 48.0)
				return "Hier";
			return "Plus ancien";
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}

	public class BodyMaxSizeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			string text = (string)value;
			if (text.Count() > 30)
			{
				text = text.Substring(0, 27);
				text += "...";
			}
			return text;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
