using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Note
{
	public class RichTextViewver : RichEditBox
	{
		public const string RichTextPropertyName = "RichText";

		public static readonly DependencyProperty RichTextProperty =
			DependencyProperty.Register(RichTextPropertyName,
										typeof(string),
										typeof(RichEditBox),
										new PropertyMetadata(
											new PropertyChangedCallback
												(RichTextPropertyChanged)));

		public RichTextViewver()
		{
			IsReadOnly = true;
			Background = new SolidColorBrush { Opacity = 0 };
			BorderThickness = new Thickness(0);
		}

		public string RichText
		{
			get { return (string)GetValue(RichTextProperty); }
			set { SetValue(RichTextProperty, value); }
		}

		private static void RichTextPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
		}
	}

}
