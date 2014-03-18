using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using RedditVisualizer.Models;
using Newtonsoft.Json;

namespace RedditVisualizer.Helpers
{
	class Posts
	{
		public static async Task<Tuple<List<RedditPost>, string, string>> FindAllControversialPostsAsync(string subreddit)
		{
			return await FindAllPostsAsync(subreddit, "/controversial.json");
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindAllHotPostsAsync(string subreddit)
		{
			return await FindAllPostsAsync(subreddit, "/hot.json");
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindAllNewPostsAsync(string subreddit)
		{
			return await FindAllPostsAsync(subreddit, "/new.json");
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindAllTopPostsAsync(string subreddit)
		{
			return await FindAllPostsAsync(subreddit, "/top.json");
		}

		private static async Task<Tuple<List<RedditPost>, string, string>> FindAllPostsAsync(string subreddit, string suffix)
		{
			string response = await GetResponse("http://www.reddit.com/" + subreddit + suffix);

			string before = "", after = "";
			string beforeSubstring = response.Substring(response.IndexOf("\"before\": ") + 10);
			if (beforeSubstring.Substring(0, 4) != "null")
			{
				beforeSubstring = beforeSubstring.Substring(1);
				before = beforeSubstring.Substring(0, beforeSubstring.IndexOf('"'));
			}

			string afterSubstring = response.Substring(response.IndexOf("\"after\": ") + 9);
			if (afterSubstring.Substring(0, 4) != "null")
			{
				afterSubstring = afterSubstring.Substring(1);
				after = afterSubstring.Substring(0, afterSubstring.IndexOf('"'));
			}

			response = response.Substring(response.IndexOf('['));
			response = response.Substring(0, response.LastIndexOf(']') + 1);

			List<RedditPost> posts = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<RedditPost>>(response));
			foreach (RedditPost p in posts)
			{
				while (p.Data.Title.Contains("&quot;"))
				{
					string part1 = p.Data.Title.Substring(0, p.Data.Title.IndexOf("&quot;"));
					string part2 = p.Data.Title.Substring(p.Data.Title.IndexOf("&quot;") + 6);
					p.Data.Title = part1 + '"'.ToString() + part2;
				}
			}

			return new Tuple<List<RedditPost>, string, string>(posts, before, after);
		}

		private static async Task<string> GetResponse(string url)
		{
			var httpClient = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, url);
			request.Headers.Add("KeepAlive", "true");
			request.Headers.Add("User-Agent", "TestySHIT");

			var response = await httpClient.SendAsync(request);
			return await response.Content.ReadAsStringAsync();
		}
	}
}
