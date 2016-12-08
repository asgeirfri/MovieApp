using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MovieApp.Droid.Activities;
using MovieApp.Models;
using MovieApp.Services;
using Newtonsoft.Json;

namespace MovieApp.Droid
{
	public class MovieSearchFragment : Fragment
	{
		private MovieService _service;

		public MovieSearchFragment()
		{
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			_service = new MovieService(new DroidPosterDownload());
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);
			var rootView = inflater.Inflate(Resource.Layout.MovieInput, container, false);

			// Get our UI controls from the loaded layout
			var movieEditText = rootView.FindViewById<EditText>(Resource.Id.movieEditText);
			var searchButton = rootView.FindViewById<Button>(Resource.Id.searchButton);
			var progressBar = rootView.FindViewById<ProgressBar>(Resource.Id.progressBar);
			progressBar.Visibility = ViewStates.Invisible;

			searchButton.Click += async (sender, e) =>
			{
				progressBar.Visibility = Android.Views.ViewStates.Visible;
				var manager = (InputMethodManager)this.Context.GetSystemService(Context.InputMethodService);
				manager.HideSoftInputFromWindow(movieEditText.WindowToken, 0);
				var res = await _service.GetAllMovieInfo(true, movieEditText.Text);

				var intent = new Intent(this.Context, typeof(MovieListActivity));
				intent.PutExtra("movies", JsonConvert.SerializeObject(res));
				progressBar.Visibility = ViewStates.Gone;
				this.StartActivity(intent);
			};

			return rootView;
		}

	}
}
