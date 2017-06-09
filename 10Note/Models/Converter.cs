using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace _10Note.Models
{
	public class TitleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			string tmp = value as string;
			if (tmp == string.Empty)
				return "No Title";
			else
				return tmp;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return (value as string);
		}
	}

	public class BodyConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			string tmp = value as string;
			if (tmp == string.Empty)
				return "No Body";
			else
				return tmp;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return (value as string);
		}
	}
}
