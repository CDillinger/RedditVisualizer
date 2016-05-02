using System;
using System.Windows;
using System.Windows.Input;
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
			try
			{
				await ViewModel.LoadInitialPostsAsync(URLSuffixTextBox.Text, (MainWindowViewModel.PostSort)SortComboBox.SelectedIndex, (MainWindowViewModel.PostType)PostTypeComboBox.SelectedIndex);
			}
			catch (Exception ex)
			{
				MessageBox.Show("An Exception Has Occurred!\n\nException Message:\n" + ex.Message);
			}
		}

		private void GoToButton_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.OpenFeatured();
		}

		private async void SaveImageButton_OnClickButton_Click(object sender, RoutedEventArgs e)
		{
			await ViewModel.SaveImageAs();
		}

		private void OpenLinkButton_OnClickButton_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.OpenLink();
		}

		private void PreviousImageButton_Click(object sender, RoutedEventArgs e)
		{

			if (ViewModel.FeaturedPost == null)
				return;
			ViewModel.GoToPreviousPost();
		}

		private void NextImageButton_Click(object sender, RoutedEventArgs e)
		{
			if (ViewModel.FeaturedPost == null)
				return;
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
						URLSuffixTextBox.Text = "all";
						break;

					case 1:
						URLSuffixTextBox.Text = "animalsbeingjerks";
						break;

					case 2:
						URLSuffixTextBox.Text = "carporn";
						break;

					case 3:
						URLSuffixTextBox.Text = "cats";
						break;

					case 4:
						URLSuffixTextBox.Text = "comics";
						break;

					case 5:
						URLSuffixTextBox.Text = "corgi";
						break;

					case 6:
						URLSuffixTextBox.Text = "aww";
						break;

					case 7:
						URLSuffixTextBox.Text = "dogpictures";
						break;

					case 8:
						URLSuffixTextBox.Text = "food";
						break;

					case 9:
						URLSuffixTextBox.Text = "foodporn";
						break;

					case 10:
						URLSuffixTextBox.Text = "funny";
						break;

					case 11:
						URLSuffixTextBox.Text = "historyporn";
						break;

					case 12:
						URLSuffixTextBox.Text = "jokes";
						break;

					case 13:
						URLSuffixTextBox.Text = "militaryporn";
						break;

					case 14:
						URLSuffixTextBox.Text = "pics";
						break;

					case 15:
						URLSuffixTextBox.Text = "tattoos";
						break;

					case 16:
						URLSuffixTextBox.Text = "wallpapers";
						break;

					case 17:
						URLSuffixTextBox.Text = "waterporn";
						break;
				}
			}
		}

		private void CustomURLCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			PredefinedSubsComboBox.Visibility = Visibility.Collapsed;
			URLTextBlock.Visibility = Visibility.Visible;
			URLSuffixTextBox.Visibility = Visibility.Visible;
		}

		private void CustomURLCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			PredefinedSubsComboBox.Visibility = Visibility.Visible;
			URLTextBlock.Visibility = Visibility.Collapsed;
			URLSuffixTextBox.Visibility = Visibility.Collapsed;
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (!PostTypeComboBox.IsFocused && !PredefinedSubsComboBox.IsFocused && !SortComboBox.IsFocused)
			{
				switch (e.Key)
				{
					case Key.Left:
						e.Handled = true;
						if (ViewModel.FeaturedPost == null || ViewModel.Posts.IndexOf(ViewModel.FeaturedPost) < 1)
							break;
						ViewModel.GoToPreviousPost();
						PreviousImageButton.Focus();
						break;

					case Key.Right:
						e.Handled = true;
						if (ViewModel.FeaturedPost == null)
							break;
						ViewModel.GoToNextPost();
						NextImageButton.Focus();
						break;

					default:
						break;
				}
			}
		}

		private void Window_MouseMove(object sender, MouseEventArgs e)
		{
			if (PreviousImageButton.IsMouseOver && ViewModel.Posts != null && ViewModel.FeaturedPost != null && ViewModel.Posts.IndexOf(ViewModel.FeaturedPost) > 0)
			{
				LeftArrowImage.Opacity = 0.8;
				RightArrowImage.Opacity = 0.4;
			}

			else if (NextImageButton.IsMouseOver && ViewModel.Posts != null && ViewModel.FeaturedPost != null && ViewModel.Posts.IndexOf(ViewModel.FeaturedPost) <= ViewModel.Posts.Count - 1)
			{
				LeftArrowImage.Opacity = 0.4;
				RightArrowImage.Opacity = 0.8;
			}

			else
			{
				LeftArrowImage.Opacity = 0.4;
				RightArrowImage.Opacity = 0.4;
			}
		}

		private void PreviousImageButton_OnGotFocus(object sender, RoutedEventArgs e)
		{
			LeftArrowImage.Opacity = 0.8;
			RightArrowImage.Opacity = 0.4;
		}

		private void PreviousImageButton_OnLostFocus(object sender, RoutedEventArgs e)
		{
			LeftArrowImage.Opacity = 0.4;
		}

		private void NextImageButton_OnGotFocus(object sender, RoutedEventArgs e)
		{
			LeftArrowImage.Opacity = 0.4;
			RightArrowImage.Opacity = 0.8;
		}

		private void NextImageButton_OnLostFocus(object sender, RoutedEventArgs e)
		{
			RightArrowImage.Opacity = 0.4;
		}
	}
}
