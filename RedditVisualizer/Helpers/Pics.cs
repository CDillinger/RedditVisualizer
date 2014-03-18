using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RedditVisualizer.Models;
using Newtonsoft.Json;

namespace RedditVisualizer.Helpers
{
	class Pics
	{
		public static async Task<Tuple<List<RedditPost>, string, string>> FindAllControversialPicsAsync(string subreddit)
		{
			return FindAllPicsAsync(await Posts.FindAllControversialPostsAsync(subreddit));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindAllHotPicsAsync(string subreddit)
		{
			return FindAllPicsAsync(await Posts.FindAllHotPostsAsync(subreddit));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindAllNewPicsAsync(string subreddit)
		{
			return FindAllPicsAsync(await Posts.FindAllNewPostsAsync(subreddit));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindAllTopPicsAsync(string subreddit)
		{
			return FindAllPicsAsync(await Posts.FindAllTopPostsAsync(subreddit));
		}

		private static Tuple<List<RedditPost>, string, string> FindAllPicsAsync(Tuple<List<RedditPost>, string, string> OldPosts)
		{
			Tuple<List<RedditPost>, string, string> Posts = new Tuple<List<RedditPost>, string, string>(new List<RedditPost>(), OldPosts.Item2, OldPosts.Item3);

			foreach (RedditPost p in OldPosts.Item1)
			{
				try
				{
					string extension = p.Data.URL.Substring(p.Data.URL.LastIndexOf('.'));
					if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
						Posts.Item1.Add(p);
				}
				catch { }
			}

			return Posts;
		}
	}
}
