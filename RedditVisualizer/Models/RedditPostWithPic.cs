namespace RedditVisualizer.Models
{
	public class RedditPostWithPic : RedditPost
	{
		public bool CachedLocally { get; set; }

		public string LocalPath { get; set; }
	}
}
