using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using MovieApp.Droid.Activities;
using MovieApp.Models;
using MovieApp.Services;
using Newtonsoft.Json;

namespace MovieApp.Droid
{
	public class TopMovieFragment : Fragment
	{
		private MovieService _service;

		public TopMovieFragment()
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
			var rootView = inflater.Inflate(Resource.Layout.TopMovies, container, false);

			// Get our UI controls from the loaded layout
			GetTopMovies(rootView);
			//progressBar.Visibility = ViewStates.Invisible;
			return rootView;
		}

		private async void GetTopMovies(View rootView)
		{
			var progressBar = rootView.FindViewById<ProgressBar>(Resource.Id.progressBar);
			var res = await _service.GetAllMovieInfo(false, "");
			var intent = new Intent(this.Context, typeof(MovieListActivity));
			intent.PutExtra("movies", JsonConvert.SerializeObject(res));
			progressBar.Visibility = ViewStates.Gone;
			this.StartActivity(intent);
		}

	}
}
