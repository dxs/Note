using System;
using Windows.UI.Xaml.Controls;

namespace _10Note.Models
{
    public class SampleModel
    {
        static Random r = new Random();
        public string Title { get; set; }
        public string Description { get; set; }
        public int ID { get; private set; }
        public Symbol Symbol { get; set; }

        public char SymbolAsChar
        {
            get { return (char)Symbol; }
        }

        public SampleModel()
        {
            GenID();
        }

        private void GenID()
        {
            ID = r.Next(1000, 9999999);
        }
    }
}
