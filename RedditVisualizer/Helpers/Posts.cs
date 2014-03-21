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
		public static async Task<Tuple<List<RedditPost>, string, string>> FindControversialPostsAsync(string subreddit)
		{
			return await FindPostsAsync("http://www.reddit.com/" + subreddit + "/controversial.json");
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindControversialPostsAsync(string subreddit, int countViewed, string after)
		{
			return await FindPostsAsync(string.Format("http://www.reddit.com/{0}/controversial.json?count={1}&after={2}", subreddit, countViewed, after));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindHotPostsAsync(string subreddit)
		{
			return await FindPostsAsync("http://www.reddit.com/" + subreddit + "/hot.json");
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindHotPostsAsync(string subreddit, int countViewed, string after)
		{
			return await FindPostsAsync(string.Format("http://www.reddit.com/{0}/hot.json?count={1}&after={2}", subreddit, countViewed, after));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindNewPostsAsync(string subreddit)
		{
			return await FindPostsAsync("http://www.reddit.com/" + subreddit + "/new.json");
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindNewPostsAsync(string subreddit, int countViewed, string after)
		{
			return await FindPostsAsync(string.Format("http://www.reddit.com/{0}/new.json?count={1}&after={2}", subreddit, countViewed, after));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindRisingPostsAsync(string subreddit)
		{
			return await FindPostsAsync("http://www.reddit.com/" + subreddit + "/rising.json");
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindRisingPostsAsync(string subreddit, int countViewed, string after)
		{
			return await FindPostsAsync(string.Format("http://www.reddit.com/{0}/rising.json?count={1}&after={2}", subreddit, countViewed, after));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindTopPostsAsync(string subreddit)
		{
			return await FindPostsAsync("http://www.reddit.com/" + subreddit + "/top.json");
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindTopPostsAsync(string subreddit, int countViewed, string after)
		{
			return await FindPostsAsync(string.Format("http://www.reddit.com/{0}/top.json?count={1}&after={2}", subreddit, countViewed, after));
		}

		private static async Task<Tuple<List<RedditPost>, string, string>> FindPostsAsync(string url)
		{
			string response = await GetResponse(url);

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

				try
				{
					string extension = p.Data.URL.Substring(p.Data.URL.LastIndexOf('.'));
					if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
						p.URLisImage = true;
					else
						p.URLisImage = false;
				}

				catch
				{
					p.URLisImage = false;
				}
			}

			return new Tuple<List<RedditPost>, string, string>(posts, before, after);
		}

		private static async Task<string> GetResponse(string url)
		{
			var httpClient = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, url);
			request.Headers.Add("KeepAlive", "true");
			request.Headers.Add("User-Agent", "RedditVisualizer");

			var response = await httpClient.SendAsync(request);
			return await response.Content.ReadAsStringAsync();
		}
	}
}
