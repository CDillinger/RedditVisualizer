using System;
using System.Collections.Generic;
using System.Windows;
using System.Diagnostics;
using RedditVisualizer.Helpers;
using RedditVisualizer.Models;
using RedditVisualizer.ViewModels;

namespace RedditVisualizer.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindowViewModel ViewModel { get; private set; }

		public MainWindow()
		{
			InitializeComponent();

			DataContext = ViewModel = new MainWindowViewModel();
		}

		private async void GetPostsButton_Click(object sender, RoutedEventArgs e)
		{
			var posts = new Tuple<List<RedditPost>, string, string>(new List<RedditPost>(), "", "");

			switch (PostTypeComboBox.SelectedIndex)
			{
				case 0:
					switch (SortComboBox.SelectedIndex)
					{
						case 0:
							posts = await Posts.FindAllControversialPostsAsync(URLSuffixTextBox.Text);
							break;

						case 1:
							posts = await Posts.FindAllHotPostsAsync(URLSuffixTextBox.Text);
							break;

						case 2:
							posts = await Posts.FindAllNewPostsAsync(URLSuffixTextBox.Text);
							break;

						case 3:
						default:
							posts = await Posts.FindAllTopPostsAsync(URLSuffixTextBox.Text);
							break;
					}
					break;

				case 1:
					switch (SortComboBox.SelectedIndex)
					{
						case 0:
							posts = await Pics.FindAllControversialPicsAsync(URLSuffixTextBox.Text);
							break;

						case 1:
							posts = await Pics.FindAllHotPicsAsync(URLSuffixTextBox.Text);
							break;

						case 2:
							posts = await Pics.FindAllNewPicsAsync(URLSuffixTextBox.Text);
							break;

						case 3:
						default:
							posts = await Pics.FindAllTopPicsAsync(URLSuffixTextBox.Text);
							break;
					}
					break;
			}

			ViewModel.Posts = posts.Item1;
			ViewModel.FeaturedPost = posts.Item1[0];
			ViewModel.PreviousPageSuffix = posts.Item2;
			ViewModel.NextPageSuffix = posts.Item3;
			CheckPostStuff();
		}

		private void GoToButton_Click(object sender, RoutedEventArgs e)
		{
			Process.Start("http://www.reddit.com" + ViewModel.FeaturedPost.Data.Permalink);
		}

		private void PreviousImageButton_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.FeaturedPost = ViewModel.Posts[ViewModel.Posts.IndexOf(ViewModel.FeaturedPost) - 1];
			CheckPostStuff();
		}

		private void NextImageButton_Click(object sender, RoutedEventArgs e)
		{
			if (ViewModel.Posts.IndexOf(ViewModel.FeaturedPost) >= ViewModel.Posts.Count - 1)
			{
				NextImageButton.IsEnabled = false;
				NextImageButton.Content = "Fetching Posts...";

				MessageBox.Show("Sorry, this has not been implemented yet.");

			}
			else
			{
				ViewModel.FeaturedPost = ViewModel.Posts[ViewModel.Posts.IndexOf(ViewModel.FeaturedPost) + 1];
			}

			CheckPostStuff();
		}

		private void CheckPostStuff()
		{
			if (ViewModel.FeaturedPost != null)
				GoToButton.IsEnabled = true;
			else
				GoToButton.IsEnabled = false;

			if (ViewModel.Posts.IndexOf(ViewModel.FeaturedPost) < 1)
				PreviousImageButton.IsEnabled = false;
			else
				PreviousImageButton.IsEnabled = true;

			if (ViewModel.Posts.IndexOf(ViewModel.FeaturedPost) >= ViewModel.Posts.Count - 1)
				NextImageButton.Content = "Load More";
			else
				NextImageButton.IsEnabled = true;
		}
	}
}
