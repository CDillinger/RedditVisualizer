using Newtonsoft.Json;

namespace RedditVisualizer.Models
{
	public class LinkData
	{
		[JsonProperty("name")]
		public string ID { get; set; }

		public string SelfText { get; set; }

		public string Title { get; set; }

		public string Author { get; set; }

		public string Thumbnail { get; set; }

		public string Permalink { get; set; }

		public string URL { get; set; }

		public string NonCachedURL { get; set; }

		public string Domain { get; set; }

		[JsonProperty("created")]
		public double CreationTimeStamp { get; set; }

		[JsonProperty("created_utc")]
		public double CreationTimeStampUTC { get; set; }

		[JsonProperty("over_18")]
		public bool NSFW { get; set; }

		public int Score { get; set; }

		[JsonProperty("num_comments")]
		public int CommentCount { get; set; }
	}
}
