using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10Note.Models
{
	public class Note : INotifyPropertyChanged
	{
		private string title = string.Empty;
		public string Title
		{
			get { return title; }
			set { title = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title))); }
		}

		private string body = string.Empty;
		public string Body
		{
			get { return body; }
			set { body = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Body))); }
		}

		public Note(string _title, string _body)
		{
			Title = _title;
			Body = _body;
		}

		public Note() : this(string.Empty,string.Empty)
		{

		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
