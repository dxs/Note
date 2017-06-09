using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace _10Note.Helper
{
	public static class SettingsStorageExtensions
	{
		private const string fileExtension = ".json";

		public static bool IsRoamingStorageAvailable(this ApplicationData appData)
		{
			return (appData.RoamingStorageQuota == 0);
		}

		public static async Task SaveAsync<T>(this StorageFolder folder, string name, T content)
		{
			var file = await folder.CreateFileAsync(GetFileName(name), CreationCollisionOption.ReplaceExisting);
			var filecontent = await JSON.StringifyAsync(content);

			await FileIO.WriteTextAsync(file, filecontent);
		}

		public static async Task<T> ReadAsync<T>(this StorageFolder folder, string name)
		{
			if (!File.Exists(Path.Combine(folder.Path, GetFileName(name))))
			{
				return default(T);
			}

			var file = await folder.GetFileAsync($"{name}.json");
			var filecontent = await FileIO.ReadTextAsync(file);

			return await JSON.ToObjectAsync<T>(filecontent);
		}

		public static async Task SaveAsync<T>(this ApplicationDataContainer settings, string key, T value)
		{
			settings.Values[key] = await JSON.StringifyAsync(value);
		}

		public static async Task<T> ReadAsync<T>(this ApplicationDataContainer settings, string key)
		{
			object obj = null;

			if (settings.Values.TryGetValue(key, out obj))
			{
				return await JSON.ToObjectAsync<T>((string)obj);
			}
			return default(T);
		}

		private static string GetFileName(string name)
		{
			return string.Concat(name, fileExtension);
		}
	}
}