using System;
using System.Collections.Generic;

namespace MovieApp.iOS
{
	public class MovieDetailsDTO
	{
		public DM.MovieApi.MovieDb.Movies.MovieInfo info;
		public string poster;
		public string casts;
		public string duration;

		public MovieDetailsDTO(DM.MovieApi.MovieDb.Movies.MovieInfo info, string poster, string casts, string duration)
		{
			this.info = info;
			this.poster = poster;
			this.casts = casts;
			this.duration = duration;
		}
	}
}
