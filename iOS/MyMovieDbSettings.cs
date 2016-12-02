using System;
using DM.MovieApi;
namespace MovieApp.iOS
{
	public class MyMovieDbSettings : IMovieDbSettings
	{
		public string ApiKey { get; }
		public string ApiUrl { get; }

		public MyMovieDbSettings()
		{
			this.ApiKey = "42b74a04acb09dd9097fa67f49678395";
			this.ApiUrl = "http://api.themoviedb.org/3/";
		}
	}
}