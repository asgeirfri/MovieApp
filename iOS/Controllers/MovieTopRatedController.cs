using System;
using UIKit;
using DM.MovieApi;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi.ApiResponse;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using MovieDownload;
using System.IO;
using CoreGraphics;

namespace MovieApp.iOS.Controllers
{
	public class MovieTopRatedController : UITableViewController
	{
		private bool shouldReload;
		private ImageDownloader _downloader;
		private List<MovieDetailsDTO> _movies;
		private UIActivityIndicatorView _spinner;
		private IApiMovieRequest _movieApi;

		public MovieTopRatedController()
		{
			shouldReload = true;
			this.Title = "Top Rated";
			this.TabBarItem = new UITabBarItem(UITabBarSystemItem.TopRated, 0);

			_downloader = new ImageDownloader(new StorageClient());
			MyMovieDbSettings settings = new MyMovieDbSettings();
			MovieDbFactory.RegisterSettings(settings);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.View.BackgroundColor = UIColor.Yellow;
			this.TableView.RowHeight = 60;

			_spinner = CreateSpinner();
			_movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;

			this.View.AddSubview(_spinner);
		}

		public override async void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			if (shouldReload)
			{
				_spinner.StartAnimating();
				ApiSearchResponse<MovieInfo> response = await _movieApi.GetTopRatedAsync();
				var results = new List<MovieDetailsDTO>();
				foreach (var res in response.Results)
				{
					var response2 = await _movieApi.GetCreditsAsync(res.Id);
					string localPath = "";
					if (res.PosterPath != null)
					{
						localPath = _downloader.LocalPathForFilename(res.PosterPath);
						var token = new CancellationToken();
						if (!File.Exists(localPath))
						{
							await _downloader.DownloadImage(res.PosterPath, localPath, token);
						}
					}

					string casts = "";
					for (int i = 0; i < 3; i++)
					{
						if (response2.Item.CastMembers.Count > i)
						{
							casts += response2.Item.CastMembers[i].Name + ", ";
						}
					}
					if (casts.Length > 2)
					{
						casts = casts.Remove(casts.Length - 2);
					}

					var resp = new MovieDetailsDTO(res, localPath, casts);
					results.Add(resp);
				}
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

		/*private async Task<List<MovieDetailsDTO>> PrepareData()
		{
			var movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
			ApiSearchResponse<MovieInfo> response = await movieApi.GetTopRatedAsync();
			var results = new List<MovieDetailsDTO>();
			foreach (var res in response.Results)
			{
				var response2 = await movieApi.GetCreditsAsync(res.Id);
				string localPath = "";
				if (res.PosterPath != null)
				{
					localPath = _downloader.LocalPathForFilename(res.PosterPath);
					var token = new CancellationToken();
					if (!File.Exists(localPath))
					{
						await _downloader.DownloadImage(res.PosterPath, localPath, token);
					}
				}

				string casts = "";
				for (int i = 0; i < 3; i++)
				{
					if (response2.Item.CastMembers.Count > i)
					{
						casts += response2.Item.CastMembers[i].Name + ", ";
					}
				}
				if (casts.Length > 2)
				{
					casts = casts.Remove(casts.Length - 2);
				}

				var resp = new MovieDetailsDTO(res, localPath, casts);
				results.Add(resp);
			}
			return results;
		}*/

		private UIActivityIndicatorView CreateSpinner()
		{
			UIActivityIndicatorView spinner = new UIActivityIndicatorView()
			{
				Frame = new CGRect(5, 5, this.View.Bounds.Width - 2 * 5, 50),
				Color = UIColor.Magenta
			};
			return spinner;
		}
	}
}
