
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MovieApp.Services;
using MovieApp.Models;
using Newtonsoft.Json;

namespace MovieApp.Droid.Activities
{
	[Activity(Label = "Movie List")]
	public class MovieListActivity : ListActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			var jsonStr = this.Intent.GetStringExtra("movies");
			var movies = JsonConvert.DeserializeObject<List<MovieDetailsDTO>>(jsonStr);
			this.ListAdapter = new MovieListAdapter(this, movies);
		}
	}
}
