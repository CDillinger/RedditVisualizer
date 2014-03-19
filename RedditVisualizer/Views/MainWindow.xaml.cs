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

			DataContext = ViewModel = new MainWindowViewModel(this);
		}

		private async void GetPostsButton_Click(object sender, RoutedEventArgs e)
		{
			await ViewModel.LoadInitialPostsAsync(URLSuffixTextBox.Text, (MainWindowViewModel.PostSort)SortComboBox.SelectedIndex, (MainWindowViewModel.PostType)PostTypeComboBox.SelectedIndex);
		}

		private void GoToButton_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.OpenFeatured();
		}

		private void PreviousImageButton_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.GoToPreviousPost();
		}

		private void NextImageButton_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.GoToNextPost();
		}
	}
}
