namespace RedditVisualizer.Models
{
	public class RedditPost : Base
	{
		public string Kind { get; set; }

		public URLType PostType { get; set; }

		public string ReadableTime { get; set; }

		public bool CachedLocally { get; set; }

		public LinkData Data { get; set; }

		public enum URLType
		{
			Other,
			Image,
			GIF,
			SelfText
		}
	}
}
