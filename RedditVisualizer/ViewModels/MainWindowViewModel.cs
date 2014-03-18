using System.Collections.Generic;
using RedditVisualizer.Models;

namespace RedditVisualizer.ViewModels
{
	public class MainWindowViewModel : Base
	{
		public List<RedditPost> Posts
		{
			get { return _posts; }
			set { SetField(ref _posts, value); }
		}
		private List<RedditPost> _posts;

		public string PreviousPageSuffix
		{
			get { return _previousPage; }
			set { SetField(ref _previousPage, value); }
		}
		private string _previousPage;

		public string NextPageSuffix
		{
			get { return _nextPage; }
			set { SetField(ref _nextPage, value); }
		}
		private string _nextPage;

		public RedditPost FeaturedPost
		{
			get { return _featuredPost; }
			set { SetField(ref _featuredPost, value); }
		}
		private RedditPost _featuredPost;
	}
}
