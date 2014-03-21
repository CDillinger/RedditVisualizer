using Newtonsoft.Json;

namespace RedditVisualizer.Models
{
	public class RedditPost : Base
	{
		public bool CachedLocally = false;

		public bool URLisImage { get; set; }

		public string Kind { get; set; }

		public LinkData Data { get; set; }

		public class LinkData
		{
			[JsonProperty("name")]
			public string ID { get; set; }

			public string Title { get; set; }

			public string Author { get; set; }

			public string Thumbnail { get; set; }

			public string Permalink { get; set; }

			public string URL { get; set; }

			[JsonProperty("created")]
			public float CreationTimeStamp { get; set; }

			[JsonProperty("created_utc")]
			public float CreationTimeStampUTC { get; set; }

			[JsonProperty("over_18")]
			public bool NSFW { get; set; }

			public int Score { get; set; }

			[JsonProperty("ups")]
			public int UpVotes { get; set; }

			[JsonProperty("downs")]
			public int DownVotes { get; set; }

			[JsonProperty("num_comments")]
			public int CommentCount { get; set; }
		}
	}
}
