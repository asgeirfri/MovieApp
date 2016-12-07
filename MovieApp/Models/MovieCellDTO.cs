using System;
namespace MovieApp.Models
{
	public class MovieCellDTO
	{
		public string title, actors, poster;
		public MovieCellDTO(string title, string actors, string poster)
		{
			this.title = title;
			this.actors = actors;
			this.poster = poster;
		}
	}
}
