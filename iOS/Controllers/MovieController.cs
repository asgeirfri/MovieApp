using System;
using DM.MovieApi;
using UIKit;
using CoreGraphics;
using DM.MovieApi.MovieDb.Movies;
using MovieApp.Services;

namespace MovieApp.iOS.Controllers
{
	public partial class MovieController : UIViewController
	{
		private MovieService _movieService;
		//private ImageDownloader _downloader;
		private const int HorizontalMargin = 20;
		private const int StartY = 80;
		private const int StepY = 50;
		private int _yCord;

		//protected ViewController(IntPtr handle) : base(handle)
		public MovieController()
		{
			// Note: this .ctor should not contain any initialization logic.
			this.Title = "Movie Search";
			this.TabBarItem = new UITabBarItem(UITabBarSystemItem.Search, 0);

			_movieService = new MovieService(new iOSPosterDownload());
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Perform any additional setup after loading the view, typically from a nib.
			this.View.BackgroundColor = UIColor.FromRGB(171,0,16);
			this._yCord = StartY;

			var prompt = CreatePrompt();
			StepDown();

			var movieField = CreateMovieField();
			StepDown();

			var searchButton = CreateSearchButton("Get movie");
			StepDown();

			var spinner = CreateSpinner();
			StepDown();

			searchButton.TouchUpInside += async (sender, e) =>
			{
				if (movieField.Text == "")
				{
					return;
				}
				movieField.ResignFirstResponder();
				searchButton.Enabled = false;
				spinner.StartAnimating();
				var results = await _movieService.GetAllMovieInfo(true, movieField.Text);
				this.NavigationController.PushViewController(new MovieListController(results), true);
				spinner.StopAnimating();
				searchButton.Enabled = true;
			};

			this.View.AddSubview(prompt);
			this.View.AddSubview(spinner);
			this.View.AddSubview(movieField);
			this.View.AddSubview(searchButton);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		private UILabel CreatePrompt()
		{
			var prompt = new UILabel()
			{
				Frame = new CGRect(HorizontalMargin, this._yCord, this.View.Bounds.Width, 50),
				Text = "Enter words in movie title:",
				TextColor = UIColor.White
			};
			return prompt;
		}

		private UITextField CreateMovieField()
		{
			var movieField = new UITextField()
			{
				Frame = new CGRect(HorizontalMargin, this._yCord, this.View.Bounds.Width - 2 * HorizontalMargin, 50),
				BorderStyle = UITextBorderStyle.RoundedRect,
				Placeholder = "Movie title",
			};
			return movieField;
		}

		private UIButton CreateSearchButton(string title)
		{
			var searchButton = UIButton.FromType(UIButtonType.RoundedRect);
			searchButton.Frame = new CGRect(HorizontalMargin, this._yCord, this.View.Bounds.Width - 2 * HorizontalMargin, 50);
			searchButton.SetTitle(title, UIControlState.Normal);
			searchButton.SetTitleColor(UIColor.White, UIControlState.Normal);
			searchButton.SetTitleColor(UIColor.Gray, UIControlState.Disabled);
			return searchButton;
		}

		private UIActivityIndicatorView CreateSpinner()
		{
			UIActivityIndicatorView spinner = new UIActivityIndicatorView()
			{
				Frame = new CGRect(HorizontalMargin, this._yCord, this.View.Bounds.Width - 2 * HorizontalMargin, 50),
				Color = UIColor.White
			};
			return spinner;
		}

		private void StepDown()
		{
			this._yCord += StepY;
		}
	}
}

