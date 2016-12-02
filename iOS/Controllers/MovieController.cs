using System;
using DM.MovieApi;
using UIKit;
using CoreGraphics;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi.ApiResponse;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using MovieDownload;
using System.IO;
using System.Threading;

namespace MovieApp.iOS.Controllers
{
	public partial class MovieController : UIViewController
	{
		private ImageDownloader _downloader;
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

			MyMovieDbSettings settings = new MyMovieDbSettings();
			MovieDbFactory.RegisterSettings(settings);
			_downloader = new ImageDownloader(new StorageClient());
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
				var movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
				spinner.StartAnimating();
				ApiSearchResponse<MovieInfo> response = await movieApi.SearchByTitleAsync(movieField.Text);
				var results = new List<MovieDetailsDTO>();
				foreach (var res in response.Results)
				{
					var durationResponse = await movieApi.FindByIdAsync(res.Id);
					var creditResponse = await movieApi.GetCreditsAsync(res.Id);
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
					if (creditResponse.Item != null)
					{
						for (int i = 0; i < 3; i++)
						{
							if (creditResponse.Item.CastMembers.Count > i)
							{
								casts += creditResponse.Item.CastMembers[i].Name + ", ";
							}
						}
						if (casts.Length > 2)
						{
							casts = casts.Remove(casts.Length - 2);
						}
					}
					var duration = "";
					if (durationResponse.Item != null)
					{
						duration = durationResponse.Item.Runtime.ToString();
					}

					var resp = new MovieDetailsDTO(res, localPath, casts, duration);
					results.Add(resp);
				}
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
	/*private async Task<List<MovieDetailsDTO>> PrepareData(string query)
	{
		var movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
		ApiSearchResponse<MovieInfo> response = await movieApi.SearchByTitleAsync(query);
		var results = new List<MovieDetailsDTO>();
		foreach (var res in response.Results)
		{
			var creditResponse = await movieApi.GetCreditsAsync(res.Id);
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
				if (creditResponse.Item.CastMembers.Count > i)
				{
					casts += creditResponse.Item.CastMembers[i].Name + ", ";
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

