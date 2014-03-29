using System;
using System.Collections.Generic;
using System.Windows;
using System.Diagnostics;
using RedditVisualizer.Helpers;
using RedditVisualizer.Models;
using RedditVisualizer.ViewModels;
using System.Windows.Data;

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
			try
			{
				await ViewModel.LoadInitialPostsAsync(URLSuffixTextBox.Text, (MainWindowViewModel.PostSort)SortComboBox.SelectedIndex, (MainWindowViewModel.PostType)PostTypeComboBox.SelectedIndex);
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show("An Exception Has Occurred!\n\nException Message:\n" + ex.Message);
			}
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

		private void PredefinedSubsComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (URLSuffixTextBox != null)
			{
				switch (PredefinedSubsComboBox.SelectedIndex)
				{
					default:
					case 0:
						URLSuffixTextBox.Text = "r/all";
						break;

					case 1:
						URLSuffixTextBox.Text = "r/animalsbeingjerks";
						break;

					case 2:
						URLSuffixTextBox.Text = "r/carporn";
						break;

					case 3:
						URLSuffixTextBox.Text = "r/cats";
						break;

					case 4:
						URLSuffixTextBox.Text = "r/comics";
						break;

					case 5:
						URLSuffixTextBox.Text = "r/corgi";
						break;

					case 6:
						URLSuffixTextBox.Text = "r/aww";
						break;

					case 7:
						URLSuffixTextBox.Text = "r/dogpictures";
						break;

					case 8:
						URLSuffixTextBox.Text = "r/food";
						break;

					case 9:
						URLSuffixTextBox.Text = "r/foodporn";
						break;

					case 10:
						URLSuffixTextBox.Text = "r/funny";
						break;

					case 11:
						URLSuffixTextBox.Text = "r/historyporn";
						break;

					case 12:
						URLSuffixTextBox.Text = "r/jokes";
						break;

					case 13:
						URLSuffixTextBox.Text = "r/militaryporn";
						break;

					case 14:
						URLSuffixTextBox.Text = "r/pics";
						break;

					case 15:
						URLSuffixTextBox.Text = "r/tattoos";
						break;

					case 16:
						URLSuffixTextBox.Text = "r/wallpapers";
						break;

					case 17:
						URLSuffixTextBox.Text = "r/waterporn";
						break;
				}
			}
		}

		private void CustomURLCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			PredefinedSubsComboBox.Visibility = System.Windows.Visibility.Collapsed;
			URLTextBlock.Visibility = System.Windows.Visibility.Visible;
			URLSuffixTextBox.Visibility = System.Windows.Visibility.Visible;
		}

		private void CustomURLCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			PredefinedSubsComboBox.Visibility = System.Windows.Visibility.Visible;
			URLTextBlock.Visibility = System.Windows.Visibility.Collapsed;
			URLSuffixTextBox.Visibility = System.Windows.Visibility.Collapsed;
		}
	}
}
