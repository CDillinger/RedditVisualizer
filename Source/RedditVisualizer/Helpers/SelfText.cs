using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RedditVisualizer.Models;
using Newtonsoft.Json;

namespace RedditVisualizer.Helpers
{
	class SelfText
	{
		public static async Task<Tuple<List<RedditPost>, string, string>> FindControversialSelfTextAsync(string subreddit)
		{
			return FindSelfTextAsync(await Posts.FindControversialPostsAsync(subreddit));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindControversialSelfTextAsync(string subreddit, int countViewed, string after)
		{
			return FindSelfTextAsync(await Posts.FindControversialPostsAsync(subreddit, countViewed, after));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindHotSelfTextAsync(string subreddit)
		{
			return FindSelfTextAsync(await Posts.FindHotPostsAsync(subreddit));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindHotSelfTextAsync(string subreddit, int countViewed, string after)
		{
			return FindSelfTextAsync(await Posts.FindHotPostsAsync(subreddit, countViewed, after));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindNewSelfTextAsync(string subreddit)
		{
			return FindSelfTextAsync(await Posts.FindNewPostsAsync(subreddit));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindNewSelfTextAsync(string subreddit, int countViewed, string after)
		{
			return FindSelfTextAsync(await Posts.FindNewPostsAsync(subreddit, countViewed, after));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindRisingSelfTextAsync(string subreddit)
		{
			return FindSelfTextAsync(await Posts.FindRisingPostsAsync(subreddit));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindRisingSelfTextAsync(string subreddit, int countViewed, string after)
		{
			return FindSelfTextAsync(await Posts.FindRisingPostsAsync(subreddit, countViewed, after));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindTopSelfTextAsync(string subreddit)
		{
			return FindSelfTextAsync(await Posts.FindTopPostsAsync(subreddit));
		}

		public static async Task<Tuple<List<RedditPost>, string, string>> FindTopSelfTextAsync(string subreddit, int countViewed, string after)
		{
			return FindSelfTextAsync(await Posts.FindTopPostsAsync(subreddit, countViewed, after));
		}

		private static Tuple<List<RedditPost>, string, string> FindSelfTextAsync(Tuple<List<RedditPost>, string, string> OldPosts)
		{
			Tuple<List<RedditPost>, string, string> Posts = new Tuple<List<RedditPost>, string, string>(new List<RedditPost>(), OldPosts.Item2, OldPosts.Item3);

			foreach (RedditPost p in OldPosts.Item1)
				if (p.PostType == RedditPost.URLType.SelfText)
					Posts.Item1.Add(p);

			return Posts;
		}
	}
}
