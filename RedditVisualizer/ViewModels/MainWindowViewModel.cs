using System;
using System.Collections.Generic;
using System.Diagnostics;
using RedditVisualizer.Helpers;
using RedditVisualizer.Models;
using RedditVisualizer.Views;
using System.Threading.Tasks;

namespace RedditVisualizer.ViewModels
{
	public class MainWindowViewModel : Base
	{
		public MainWindowViewModel(MainWindow window)
		{
			Window = window;
		}

		public MainWindow Window
		{
			get { return _window; }
			set { SetField(ref _window, value); }
		}
		private MainWindow _window;

		#region Properties

		public List<RedditPost> Posts
		{
			get { return _posts; }
			set { SetField(ref _posts, value); }
		}
		private List<RedditPost> _posts;

		public int CountViewed
		{
			get { return _countViewed; }
			set { SetField(ref _countViewed, value); }
		}
		private int _countViewed;

		public string BeforePost
		{
			get { return _beforePost; }
			set { SetField(ref _beforePost, value); }
		}
		private string _beforePost;

		public string AfterPost
		{
			get { return _afterPost; }
			set { SetField(ref _afterPost, value); }
		}
		private string _afterPost;

		public RedditPost FeaturedPost
		{
			get { return _featuredPost; }
			set { SetField(ref _featuredPost, value); }
		}
		private RedditPost _featuredPost;

		public string Suffix
		{
			get { return _suffix; }
			set { SetField(ref _suffix, value); }
		}
		private string _suffix;

		public PostSort Sort
		{
			get { return _sort; }
			set { SetField(ref _sort, value); }
		}
		private PostSort _sort;

		public PostType Type
		{
			get { return _type; }
			set { SetField(ref _type, value); }
		}
		private PostType _type;

		private bool _loading = false;

		#endregion

		#region Enums

		public enum PostSort
		{
			Hot,
			New,
			Rising,
			Controversial,
			Top
		}

		public enum PostType
		{
			All,
			PicsOnly
		}

		#endregion

		#region Methods

		public void OpenFeatured()
		{
			Process.Start("http://www.reddit.com" + FeaturedPost.Data.Permalink);
		}

		public async Task<bool> LoadInitialPostsAsync(string suffix, PostSort sort, PostType type)
		{
			Suffix = suffix;
			Sort = sort;
			Type = type;

			Tuple<List<RedditPost>, string, string> posts = await LoadPostsAsync(Suffix, Sort, Type);

			Posts = posts.Item1;
			FeaturedPost = posts.Item1[0];
			BeforePost = posts.Item2;
			AfterPost = posts.Item3;
			CheckPostStuff();

			CountViewed = 25;

			return true;
		}

		public async Task<bool> LoadMorePostsAsync(string suffix, PostSort sort, PostType type)
		{
			Tuple<List<RedditPost>, string, string> posts = await LoadPostsAsync(suffix, sort, type, CountViewed, AfterPost);

			foreach (RedditPost p in posts.Item1)
			{
				Posts.Add(p);
			}
			BeforePost = posts.Item2;
			AfterPost = posts.Item3;
			CheckPostStuff();

			CountViewed += 25;

			_loading = false;

			return true;
		}

		public async Task<Tuple<List<RedditPost>, string, string>> LoadPostsAsync(string suffix, PostSort sort, PostType type)
		{
			var posts = new Tuple<List<RedditPost>, string, string>(new List<RedditPost>(), "", "");

			switch (type)
			{
				case PostType.All:
					switch (sort)
					{
						case PostSort.Controversial:
							posts = await Helpers.Posts.FindControversialPostsAsync(suffix);
							break;

						case PostSort.Hot:
							posts = await Helpers.Posts.FindHotPostsAsync(suffix);
							break;

						case PostSort.New:
							posts = await Helpers.Posts.FindNewPostsAsync(suffix);
							break;

						case PostSort.Rising:
							posts = await Helpers.Posts.FindRisingPostsAsync(suffix);
							break;

						case PostSort.Top:
						default:
							posts = await Helpers.Posts.FindTopPostsAsync(suffix);
							break;
					}
					break;

				case PostType.PicsOnly:
					switch (sort)
					{
						case PostSort.Controversial:
							posts = await Helpers.Pics.FindControversialPicsAsync(suffix);
							break;

						case PostSort.Hot:
							posts = await Helpers.Pics.FindHotPicsAsync(suffix);
							break;

						case PostSort.New:
							posts = await Helpers.Pics.FindNewPicsAsync(suffix);
							break;

						case PostSort.Rising:
							posts = await Helpers.Pics.FindRisingPicsAsync(suffix);
							break;

						case PostSort.Top:
						default:
							posts = await Helpers.Pics.FindTopPicsAsync(suffix);
							break;
					}
					break;
			}

			return posts;
		}

		public async Task<Tuple<List<RedditPost>, string, string>> LoadPostsAsync(string suffix, PostSort sort, PostType type, int countViewed, string after)
		{
			var posts = new Tuple<List<RedditPost>, string, string>(new List<RedditPost>(), "", "");

			switch (type)
			{
				case PostType.All:
					switch (sort)
					{
						case PostSort.Controversial:
							posts = await Helpers.Posts.FindControversialPostsAsync(suffix, countViewed, after);
							break;

						case PostSort.Hot:
							posts = await Helpers.Posts.FindHotPostsAsync(suffix, countViewed, after);
							break;

						case PostSort.New:
							posts = await Helpers.Posts.FindNewPostsAsync(suffix, countViewed, after);
							break;

						case PostSort.Rising:
							posts = await Helpers.Posts.FindRisingPostsAsync(suffix, countViewed, after);
							break;

						case PostSort.Top:
						default:
							posts = await Helpers.Posts.FindTopPostsAsync(suffix, countViewed, after);
							break;
					}
					break;

				case PostType.PicsOnly:
					switch (sort)
					{
						case PostSort.Controversial:
							posts = await Helpers.Pics.FindControversialPicsAsync(suffix, countViewed, after);
							break;

						case PostSort.Hot:
							posts = await Helpers.Pics.FindHotPicsAsync(suffix, countViewed, after);
							break;

						case PostSort.New:
							posts = await Helpers.Pics.FindNewPicsAsync(suffix, countViewed, after);
							break;

						case PostSort.Rising:
							posts = await Helpers.Pics.FindRisingPicsAsync(suffix, countViewed, after);
							break;

						case PostSort.Top:
						default:
							posts = await Helpers.Pics.FindTopPicsAsync(suffix, countViewed, after);
							break;
					}
					break;
			}

			return posts;
		}

		public void GoToPreviousPost()
		{
			FeaturedPost = Posts[Posts.IndexOf(FeaturedPost) - 1];
			CheckPostStuff();
		}

		public async void GoToNextPost()
		{
			if (Posts.IndexOf(FeaturedPost) >= Posts.Count - 6 && _loading == false)
			{
				_loading = true;
				await LoadMorePostsAsync(Suffix, Sort, Type);
			}

			if (Posts.IndexOf(FeaturedPost) >= Posts.Count - 1 && _loading == false)
			{
				Window.NextImageButton.IsEnabled = false;
				Window.NextImageButton.Content = "Fetching Posts...";

				await LoadMorePostsAsync(Suffix, Sort, Type);
			}
			else
				FeaturedPost = Posts[Posts.IndexOf(FeaturedPost) + 1];

			Window.NextImageButton.Content = "Next";
			CheckPostStuff();
		}

		public void CheckPostStuff()
		{
			if (FeaturedPost != null)
				Window.GoToButton.IsEnabled = true;
			else
				Window.GoToButton.IsEnabled = false;

			if (Posts.IndexOf(FeaturedPost) < 1)
				Window.PreviousImageButton.IsEnabled = false;
			else
				Window.PreviousImageButton.IsEnabled = true;

			if (Posts.IndexOf(FeaturedPost) >= Posts.Count - 1 && _loading == true)
			{
				Window.NextImageButton.IsEnabled = false;
				Window.NextImageButton.Content = "Fetching More Posts...";
			}
			else
				Window.NextImageButton.IsEnabled = true;
		}

		#endregion
	}
}
