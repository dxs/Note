﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Note.Logic
{
	public class Filer
	{

		public static async Task<string> SerializeDataToJson(NoteHandler _note, string filename = "main")
		{
			try
			{
				var Folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Data",CreationCollisionOption.OpenIfExists);
				var file = await Folder.CreateFileAsync(filename + ".json", CreationCollisionOption.ReplaceExisting);
				var data = await file.OpenStreamForWriteAsync();

				using (StreamWriter r = new StreamWriter(data))
				{
					var serelizedfile = JsonConvert.SerializeObject(_note.Notes);
					r.Write(serelizedfile);

				}
				return filename;
			}
			catch (Exception e)
			{
				//Cannot find file
				return "";
			}
		}

		public static async Task<NoteHandler> DeserelizeDataFromJson(string fileName = "main")
		{
			try
			{
				var Note = new NoteHandler();
				var Folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
				var file = await Folder.GetFileAsync(fileName + ".json");
				var data = await file.OpenReadAsync();

				using (StreamReader r = new StreamReader(data.AsStream()))
				{
					string text = r.ReadToEnd();
					if (text == string.Empty)
						return Note;
					var a = JsonConvert.DeserializeObject<ObservableCollection<ANote>>(text);
					foreach (var i in a)
					{
						Note.Notes.Add(i);
					}
				}
				return Note;
			}
			catch (Exception e)
			{
				return new NoteHandler();
			}
		}

	}
}
