using System;
using System.Collections.Generic;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Java.IO;
using MovieApp.Services;
using MovieApp.Models;

namespace MovieApp.Droid.Activities
{

	public class MovieListAdapter : BaseAdapter<MovieDetailsDTO>
	{
		private Activity _context;
		private List<MovieDetailsDTO> _movies;

		public MovieListAdapter(Activity context, List<MovieDetailsDTO> movies) 
		{
			this._context = context;
			this._movies = movies;
		}

		public override MovieDetailsDTO this[int position]
		{
			get
			{
				return this._movies[position];
			}
		}

		public override int Count
		{
			get
			{
				return this._movies.Count;
			}
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView;
			if (view == null)
			{
				view = this._context.LayoutInflater.Inflate(Resource.Layout.MovieListItem, null);
			}

			var movie = this._movies[position];
			view.FindViewById<TextView>(Resource.Id.title).Text = movie.info.Title;
			view.FindViewById<TextView>(Resource.Id.year).Text = movie.info.ReleaseDate.Year.ToString();
			view.FindViewById<TextView>(Resource.Id.cast).Text = movie.casts;

			var file = new File(movie.poster);
			var bmimg = BitmapFactory.DecodeFile(file.AbsolutePath);
			view.FindViewById<ImageView>(Resource.Id.picture).SetImageBitmap(bmimg);

			return view;
		}
	}
}
