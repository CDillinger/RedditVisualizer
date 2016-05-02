using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RedditVisualizer.Models;
using Newtonsoft.Json;

namespace RedditVisualizer.Helpers
{
	public static class Pics
	{
		public static async Task<Tuple<List<RedditPost>, string, string>> FindControversialPicsAsync(string subreddit)
		{
			return FindPicsAsync(await Posts.FindControversialPostsAsync(subreddit));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindControversialPicsAsync(string subreddit, int countViewed, string after)
		{
			return FindPicsAsync(await Posts.FindControversialPostsAsync(subreddit, countViewed, after));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindHotPicsAsync(string subreddit)
		{
			return FindPicsAsync(await Posts.FindHotPostsAsync(subreddit));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindHotPicsAsync(string subreddit, int countViewed, string after)
		{
			return FindPicsAsync(await Posts.FindHotPostsAsync(subreddit, countViewed, after));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindNewPicsAsync(string subreddit)
		{
			return FindPicsAsync(await Posts.FindNewPostsAsync(subreddit));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindNewPicsAsync(string subreddit, int countViewed, string after)
		{
			return FindPicsAsync(await Posts.FindNewPostsAsync(subreddit, countViewed, after));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindRisingPicsAsync(string subreddit)
		{
			return FindPicsAsync(await Posts.FindRisingPostsAsync(subreddit));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindRisingPicsAsync(string subreddit, int countViewed, string after)
		{
			return FindPicsAsync(await Posts.FindRisingPostsAsync(subreddit, countViewed, after));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindTopPicsAsync(string subreddit)
		{
			return FindPicsAsync(await Posts.FindTopPostsAsync(subreddit));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindTopPicsAsync(string subreddit, int countViewed, string after)
		{
			return FindPicsAsync(await Posts.FindTopPostsAsync(subreddit, countViewed, after));
		}

		private static Tuple<List<RedditPost>, string, string> FindPicsAsync(Tuple<List<RedditPost>, string, string> OldPosts)
		{
			Tuple<List<RedditPost>, string, string> Posts = new Tuple<List<RedditPost>, string, string>(new List<RedditPost>(), OldPosts.Item2, OldPosts.Item3);

			foreach (RedditPost p in OldPosts.Item1)
				if (p.PostType == RedditPost.URLType.Image || p.PostType == RedditPost.URLType.GIF)
					Posts.Item1.Add(p);

			return Posts;
		}
	}
}
