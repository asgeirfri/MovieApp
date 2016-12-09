
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using MovieApp.Models;
using Newtonsoft.Json;

namespace MovieApp.Droid
{
	[Activity(Label = "MovieDetailsActivity")]
	public class MovieDetailsActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			this.SetContentView(Resource.Layout.MovieDetails);

			var jsonStr = this.Intent.GetStringExtra("movie");
			var movies = JsonConvert.DeserializeObject<MovieDetailsDTO>(jsonStr);

			var title = this.FindViewById<TextView>(Resource.Id.title);
			title.Text = movies.info.Title + " (" + movies.info.ReleaseDate.Year + ")";

			var detail = this.FindViewById<TextView>(Resource.Id.detail);
			detail.Text = movies.duration + " min";

			var overview = this.FindViewById<TextView>(Resource.Id.overview);
			overview.Text = movies.info.Overview;

			var image = this.FindViewById<ImageView>(Resource.Id.picture);
			var file = new File(movies.poster);
			var bmimg = BitmapFactory.DecodeFile(file.AbsolutePath);
			image.SetImageBitmap(bmimg);
		}
	}
}
