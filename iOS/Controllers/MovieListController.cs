using System;
using System.Collections.Generic;
using MovieApp.Models;
using UIKit;

namespace MovieApp.iOS.Controllers
{
	public class MovieListController : UITableViewController
	{
		private List<MovieDetailsDTO> _movies;

		public MovieListController(List<MovieDetailsDTO> movies)
		{
			_movies = movies;
		}

		public override void ViewDidLoad()
		{
			this.View.BackgroundColor = UIColor.FromRGB(171, 0, 16);
			this.Title = "Movie List";
			this.TableView.RowHeight = 60;


			this.TableView.Source = new MovieListSource(this._movies, OnSelectedMovie);
		}

		private void OnSelectedMovie(int row)
		{
			this.NavigationController.PushViewController(new MovieInfoController(_movies[row]), true);
		}
	}
}
