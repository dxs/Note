using Newtonsoft.Json;
using System.Threading.Tasks;

namespace _10Note.Helper
{
	public static class JSON
    {
		public static async Task<T> ToObjectAsync<T>(string value)
		{
			return await Task.Run<T>(() =>
			{
				return JsonConvert.DeserializeObject<T>(value);
			});
		}

		public static async Task<string> StringifyAsync(object value)
		{
			return await Task.Run<string>(() =>
			{
				return JsonConvert.SerializeObject(value);
			});
		}
	}
}
