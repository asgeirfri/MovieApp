using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;
using MovieApp.Services;
using Newtonsoft.Json;
using MovieApp.Models;

namespace MovieApp.Droid.Activities
{
	//[Activity(Label = "MovieApp.Droid")]
	[Activity(Label = "Movie Search", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : Activity
	{
		private MovieService _service;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			_service = new MovieService(new DroidPosterDownload());

			// Set our view from the "main" layout resource
			this.SetContentView(Resource.Layout.Main);

			// Get our UI controls from the loaded layout
			var movieEditText = this.FindViewById<EditText>(Resource.Id.movieEditText);
			//var movieTextView = this.FindViewById<TextView>(Resource.Id.movieTextView);
			var searchButton = this.FindViewById<Button>(Resource.Id.searchButton);
			var progressBar = this.FindViewById<ProgressBar>(Resource.Id.progressBar);
			progressBar.Visibility = ViewStates.Invisible;

			searchButton.Click += async (sender, e) =>
			{
				progressBar.Visibility = Android.Views.ViewStates.Visible;
				var manager = (InputMethodManager)this.GetSystemService(InputMethodService);
				manager.HideSoftInputFromWindow(movieEditText.WindowToken, 0);
				var res = await _service.GetAllMovieInfo(true, movieEditText.Text);

				var intent = new Intent(this, typeof(MovieListActivity));
				intent.PutExtra("movies", JsonConvert.SerializeObject(res));
				progressBar.Visibility = Android.Views.ViewStates.Gone;
				this.StartActivity(intent);
			};
		}


	}
}
