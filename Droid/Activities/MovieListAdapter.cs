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
			//view.SetBackgroundColor(Color.DarkRed);
			var movie = this._movies[position];
			var title = view.FindViewById<TextView>(Resource.Id.title);
			var casts = view.FindViewById<TextView>(Resource.Id.cast);
			var image = view.FindViewById<ImageView>(Resource.Id.picture);

			SetTextAndLayout(title, movie.info.Title + " (" + movie.info.ReleaseDate.Year + ")");
			SetTextAndLayout(casts, movie.casts);

			var file = new File(movie.poster);
			var bmimg = BitmapFactory.DecodeFile(file.AbsolutePath);
			image.SetImageBitmap(bmimg);

			return view;
		}

		private void SetTextAndLayout(TextView view, String text)
		{
			view.Text = text;
		}
	}
}
