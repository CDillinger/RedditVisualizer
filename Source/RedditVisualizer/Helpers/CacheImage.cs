using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RedditVisualizer.Helpers
{
	class CacheImage
	{
		public static async Task<string> CacheImageAsync(string url)
		{
			string localFileName = Path.GetTempFileName();
			await SaveImageAsync(url, localFileName);

			return localFileName;
		}

		public static async Task<bool> SaveImageAsync(string url, string localPath)
		{
			byte[] data = await GetImageAsync(url);
			Stream stream = File.Create(localPath);
			stream.Write(data, 0, data.Length);
			stream.Close();

			return true;
		}

		private static async Task<byte[]> GetImageAsync(string url)
		{
			var httpClient = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, url);
			request.Headers.Add("KeepAlive", "true");
			request.Headers.Add("User-Agent", "RedditVisualizer");

			var response = await httpClient.SendAsync(request);
			return await response.Content.ReadAsByteArrayAsync();
		}
	}
}
