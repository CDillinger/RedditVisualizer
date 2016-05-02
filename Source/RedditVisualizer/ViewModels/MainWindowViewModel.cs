using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Win32;
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
			set
			{
				SetField(ref _featuredPost, value);
				PropertyChanged += FeaturedPost_PropertyChanged;
			}
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
			Pics,
			SelfText
		}

		#endregion

		#region Methods

		public void OpenFeatured()
		{
			Process.Start("http://www.reddit.com" + FeaturedPost.Data.Permalink);
		}

		public void OpenLink()
		{
			Process.Start(FeaturedPost.Data.NonCachedURL);
		}

		public async Task SaveImageAs()
		{
			var dialog = new SaveFileDialog
			{
				FileName = FeaturedPost.Data.NonCachedURL.Substring(FeaturedPost.Data.NonCachedURL.LastIndexOf('/') + 1)
			};
			switch (FeaturedPost.PostType)
			{
				case RedditPost.URLType.GIF:
					dialog.Filter = "GIF Image (*.gif) | *.gif";
					break;
				case RedditPost.URLType.Image:
					if (FeaturedPost.Data.NonCachedURL.EndsWith(".png"))
						dialog.Filter = "PNG Files (*.png) | *.png";
					else if (FeaturedPost.Data.NonCachedURL.EndsWith(".jpg") || FeaturedPost.Data.NonCachedURL.EndsWith(".jpg"))
						dialog.Filter = "JPG Files (*.jpg) | *.jpg";
					else
					{
						var extension = FeaturedPost.Data.NonCachedURL.Substring(FeaturedPost.Data.NonCachedURL.LastIndexOf('.') + 1);
						dialog.Filter = string.Format("{0} Files (*.{1}) | *.{1}", extension.ToUpper(), extension.ToLower());
						}
					break;
			}
			var success = dialog.ShowDialog();
			if (success == null || success == false)
				return;

			await Helpers.CacheImage.SaveImageAsync(FeaturedPost.Data.NonCachedURL, dialog.FileName);
		}

		private static async Task CacheImageLocally(RedditPost post)
		{
			if (post != null && (post.PostType == RedditPost.URLType.Image || post.PostType == RedditPost.URLType.GIF) && !post.CachedLocally)
			{
				string localPath = await CacheImage.CacheImageAsync(post.Data.URL);
				post.Data.URL = localPath;
				post.CachedLocally = true;
			}
		}

		public static async void FeaturedPost_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			RedditPost post = sender as RedditPost;
			await CacheImageLocally(post);
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
			await CheckPostStuff();

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
			await CheckPostStuff();

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

				case PostType.Pics:
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

				case PostType.SelfText:
					switch (sort)
					{
						case PostSort.Controversial:
							posts = await Helpers.SelfText.FindControversialSelfTextAsync(suffix);
							break;

						case PostSort.Hot:
							posts = await Helpers.SelfText.FindHotSelfTextAsync(suffix);
							break;

						case PostSort.New:
							posts = await Helpers.SelfText.FindNewSelfTextAsync(suffix);
							break;

						case PostSort.Rising:
							posts = await Helpers.SelfText.FindRisingSelfTextAsync(suffix);
							break;

						case PostSort.Top:
						default:
							posts = await Helpers.SelfText.FindTopSelfTextAsync(suffix);
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

				case PostType.Pics:
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

				case PostType.SelfText:
					switch (sort)
					{
						case PostSort.Controversial:
							posts = await Helpers.SelfText.FindControversialSelfTextAsync(suffix, countViewed, after);
							break;

						case PostSort.Hot:
							posts = await Helpers.SelfText.FindHotSelfTextAsync(suffix, countViewed, after);
							break;

						case PostSort.New:
							posts = await Helpers.SelfText.FindNewSelfTextAsync(suffix, countViewed, after);
							break;

						case PostSort.Rising:
							posts = await Helpers.SelfText.FindRisingSelfTextAsync(suffix, countViewed, after);
							break;

						case PostSort.Top:
						default:
							posts = await Helpers.SelfText.FindTopSelfTextAsync(suffix, countViewed, after);
							break;
					}
					break;
			}

			return posts;
		}

		public async void GoToPreviousPost()
		{
			FeaturedPost = Posts[Posts.IndexOf(FeaturedPost) - 1];
			await CheckPostStuff();
		}

		public async void GoToNextPost()
		{
			if (Posts.IndexOf(FeaturedPost) >= Posts.Count - 6 && !_loading)
			{
				_loading = true;
				await LoadMorePostsAsync(Suffix, Sort, Type);
			}

			if (Posts.IndexOf(FeaturedPost) >= Posts.Count - 1 && !_loading)
			{
				Window.NextImageButton.IsEnabled = false;
				Window.NextImageButton.Content = "Fetching Posts...";

				await LoadMorePostsAsync(Suffix, Sort, Type);
			}
			else
			{
				if (!_loading)
					FeaturedPost = Posts[Posts.IndexOf(FeaturedPost) + 1];
			}

			Window.NextImageButton.Content = "Next";
			await CheckPostStuff();
		}

		public async Task CheckPostStuff()
		{
			await CacheImageLocally(FeaturedPost);

			if (FeaturedPost != null)
			{
				Window.GoToButton.IsEnabled = true;
				Window.OpenLinkButton.IsEnabled = FeaturedPost.PostType != RedditPost.URLType.SelfText;
				Window.SaveImageButton.IsEnabled = FeaturedPost.PostType == RedditPost.URLType.Image || FeaturedPost.PostType == RedditPost.URLType.GIF;
			}
			else
				Window.GoToButton.IsEnabled = Window.OpenLinkButton.IsEnabled = Window.SaveImageButton.IsEnabled = false;

			Window.PreviousImageButton.IsEnabled = Posts.IndexOf(FeaturedPost) >= 1;

			if (Posts.IndexOf(FeaturedPost) >= Posts.Count - 1 && _loading)
			{
				Window.NextImageButton.IsEnabled = false;
				Window.NextImageButton.Content = "Fetching More Posts...";
			}
			else
				Window.NextImageButton.IsEnabled = true;

			switch (FeaturedPost.PostType)
			{
				case RedditPost.URLType.SelfText:
					Window.BodyTextBlock.Visibility = System.Windows.Visibility.Visible;
					Window.StillImage.Visibility = System.Windows.Visibility.Collapsed;
					Window.GIFImage.Visibility = System.Windows.Visibility.Collapsed;
					break;

				case RedditPost.URLType.Image:
					Window.BodyTextBlock.Visibility = System.Windows.Visibility.Collapsed;
					Window.StillImage.Visibility = System.Windows.Visibility.Visible;
					Window.GIFImage.Visibility = System.Windows.Visibility.Collapsed;
					break;

				case RedditPost.URLType.GIF:
					Window.BodyTextBlock.Visibility = System.Windows.Visibility.Collapsed;
					Window.StillImage.Visibility = System.Windows.Visibility.Collapsed;
					Window.GIFImage.Visibility = System.Windows.Visibility.Visible;
					break;

				case RedditPost.URLType.Other:
				default:
					Window.BodyTextBlock.Visibility = System.Windows.Visibility.Collapsed;
					Window.StillImage.Visibility = System.Windows.Visibility.Collapsed;
					Window.GIFImage.Visibility = System.Windows.Visibility.Collapsed;
					break;
			}
		}

		#endregion
	}
}
