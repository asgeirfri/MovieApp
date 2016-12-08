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
		private View _rootView;
		private LayoutInflater _inflater;
		private ViewGroup _container;

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
			//progressBar.Visibility = ViewStates.Invisible;
			_rootView = inflater.Inflate(Resource.Layout.TopMovies, container, false);
			return _rootView;
		}

		public async void GetTopMovies()
		{
			var progressBar = _rootView.FindViewById<ProgressBar>(Resource.Id.progressBar);
			var res = await _service.GetAllMovieInfo(false, "");
			var intent = new Intent(this.Context, typeof(MovieListActivity));
			intent.PutExtra("movies", JsonConvert.SerializeObject(res));
			progressBar.Visibility = ViewStates.Gone;
			this.StartActivity(intent);
		}
	}
}
