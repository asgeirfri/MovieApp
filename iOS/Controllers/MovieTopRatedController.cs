using UIKit;
using System.Collections.Generic;
using CoreGraphics;
using MovieApp.Models;
using MovieApp.Services;

namespace MovieApp.iOS.Controllers
{
	public class MovieTopRatedController : UITableViewController
	{
		private bool shouldReload;
		private List<MovieDetailsDTO> _movies;
		private UIActivityIndicatorView _spinner;
		private MovieService _service;

		public MovieTopRatedController()
		{
			shouldReload = true;
			this.Title = "Top Rated";
			this.TabBarItem = new UITabBarItem(UITabBarSystemItem.TopRated, 0);

			_service = new MovieService(new iOSPosterDownload());
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.View.BackgroundColor = UIColor.FromRGB(171, 0, 16);
			this.TableView.RowHeight = 60;

			_spinner = CreateSpinner();

			this.View.AddSubview(_spinner);
		}

		public override async void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			if (shouldReload)
			{
				_spinner.StartAnimating();
				var results = await _service.GetAllMovieInfo(false, "");
				_movies = results;
				_spinner.StopAnimating();
				this.TableView.Source = new MovieListSource(this._movies, OnSelectedMovie);
				this.TableView.ReloadData();

				this.TableView.Source = new MovieListSource(this._movies, OnSelectedMovie);
			}
			shouldReload = true;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			if (shouldReload)
			{
				_movies.Clear();
				this.TableView.ReloadData();
			}
		}

		private void OnSelectedMovie(int row)
		{
			shouldReload = false;
			this.NavigationController.PushViewController(new MovieInfoController(_movies[row]), true);
		}

		private UIActivityIndicatorView CreateSpinner()
		{
			UIActivityIndicatorView spinner = new UIActivityIndicatorView()
			{
				Frame = new CGRect(5, 5, this.View.Bounds.Width - 2 * 5, 50),
				Color = UIColor.White
			};
			return spinner;
		}
	}
}
