using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using RedditVisualizer.Models;
using Newtonsoft.Json;

namespace RedditVisualizer.Helpers
{
	public static class Posts
	{
		public static async Task<Tuple<List<RedditPost>, string, string>> FindControversialPostsAsync(string subreddit)
		{
			return await FindPostsAsync("http://www.reddit.com/r/" + subreddit + "/controversial.json");
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindControversialPostsAsync(string subreddit, int countViewed, string after)
		{
			return await FindPostsAsync(string.Format("http://www.reddit.com/r/{0}/controversial.json?count={1}&after={2}", subreddit, countViewed, after));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindHotPostsAsync(string subreddit)
		{
			return await FindPostsAsync("http://www.reddit.com/r/" + subreddit + "/hot.json");
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindHotPostsAsync(string subreddit, int countViewed, string after)
		{
			return await FindPostsAsync(string.Format("http://www.reddit.com/r/{0}/hot.json?count={1}&after={2}", subreddit, countViewed, after));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindNewPostsAsync(string subreddit)
		{
			return await FindPostsAsync("http://www.reddit.com/r/" + subreddit + "/new.json");
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindNewPostsAsync(string subreddit, int countViewed, string after)
		{
			return await FindPostsAsync(string.Format("http://www.reddit.com/r/{0}/new.json?count={1}&after={2}", subreddit, countViewed, after));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindRisingPostsAsync(string subreddit)
		{
			return await FindPostsAsync("http://www.reddit.com/r/" + subreddit + "/rising.json");
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindRisingPostsAsync(string subreddit, int countViewed, string after)
		{
			return await FindPostsAsync(string.Format("http://www.reddit.com/r/{0}/rising.json?count={1}&after={2}", subreddit, countViewed, after));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindTopPostsAsync(string subreddit)
		{
			return await FindPostsAsync("http://www.reddit.com/r/" + subreddit + "/top.json");
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindTopPostsAsync(string subreddit, int countViewed, string after)
		{
			return await FindPostsAsync(string.Format("http://www.reddit.com/r/{0}/top.json?count={1}&after={2}", subreddit, countViewed, after));
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
				p.Data.Title = StringConvert.XmlStringToNormal(p.Data.Title);
				p.Data.SelfText = StringConvert.XmlStringToNormal(p.Data.SelfText);

				try
				{
					DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
					p.ReadableTime = dt.AddSeconds(p.Data.CreationTimeStampUTC).ToLocalTime().ToString("g");

					if (p.Data.Domain.IndexOf("self.") == 0)
					{
						p.PostType = RedditPost.URLType.SelfText;
					}

					if (p.Data.URL.Contains("imgur.com") && !p.Data.URL.Contains("i.") && !p.Data.URL.Contains("/a/") && !p.Data.URL.Contains("/r/") && !p.Data.URL.Contains("/gallery/"))
					{
						string tempURL1 = p.Data.URL.Substring(0, p.Data.URL.IndexOf("imgur.com"));
						string tempURL2 = p.Data.URL.Substring(p.Data.URL.IndexOf("imgur.com"));
						p.Data.URL = string.Format("{0}i.{1}.png", tempURL1, tempURL2);
					}

					string extension = p.Data.URL.Substring(p.Data.URL.LastIndexOf('.'));
					if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
						p.PostType = RedditPost.URLType.Image;

					if (extension == ".gif")
						p.PostType = RedditPost.URLType.GIF;
				}

				catch
				{
					p.PostType = RedditPost.URLType.Other;
				}

				p.Data.NonCachedURL = p.Data.URL;
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
