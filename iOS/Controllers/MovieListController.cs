using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DM.MovieApi;
using DM.MovieApi.MovieDb.Movies;
using MovieDownload;
using UIKit;

namespace MovieApp.iOS.Controllers
{
	public class MovieListController : UITableViewController
	{
		private List<MovieDetailsDTO> _movies;
		//private ImageDownloader _downloader;

		public MovieListController(List<MovieDetailsDTO> movies)
		{
			//_downloader = new ImageDownloader(new StorageClient());
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

		/*private async Task<List<MovieDetailsDTO>> PrepareData(IReadOnlyList<DM.MovieApi.MovieDb.Movies.MovieInfo> response, IApiMovieRequest movieApi)
		{
			var results = new List<MovieDetailsDTO>();
			foreach (var res in response)
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
	}
}
