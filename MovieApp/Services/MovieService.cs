using System;
using System.Collections.Generic;
using MovieApp.Models;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi.ApiResponse;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using DM.MovieApi;

namespace MovieApp.Services
{
	/*
	 * MovieService is a gateway to the MovieDB API 
	 */
	public class MovieService
	{
		private IPosterDownload _downloader;
		private IApiMovieRequest _movieApi;

		public MovieService(IPosterDownload downloader)
		{
			MyMovieDbSettings settings = new MyMovieDbSettings();
			MovieDbFactory.RegisterSettings(settings);

			_downloader = downloader;
			_movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
		}

		/*
		 * Parameter search, true if it's a search, false if it's getting top movies.
		 * Parameter query, search query if it's a movie search.
		 * 
		 * This function calls gets the list of movies requested and downloads a poster or them as well.
		 * 
		 * Returns a list of MoviedDetailsDTO containing all necesary info about movies.
		 */
		public async Task<List<MovieDetailsDTO>> GetAllMovieInfo(bool search, string query)
		{
			ApiSearchResponse<MovieInfo> response;
			var results = new List<MovieDetailsDTO>();

			if (search)
			{
				response = await _movieApi.SearchByTitleAsync(query);
			}
			else
			{
				response = await _movieApi.GetTopRatedAsync();
			}

			foreach (var res in response.Results)
			{
				var durationResponse = await _movieApi.FindByIdAsync(res.Id);
				var creditResponse = await _movieApi.GetCreditsAsync(res.Id);
				string localPath = "";
				string casts = "";
				var duration = "";

				if (_downloader != null)
				{
					localPath = await _downloader.DownloadImage(res.PosterPath);
				}

				if (creditResponse.Item != null)
				{
					for (int i = 0; i < 3; i++)
					{
						if (creditResponse.Item.CastMembers.Count > i)
						{
							casts += creditResponse.Item.CastMembers[i].Name + ", ";
						}
					}
					if (casts.Length > 2)
					{
						casts = casts.Remove(casts.Length - 2);
					}
				}

				if (durationResponse.Item != null)
				{
					duration = durationResponse.Item.Runtime.ToString();
				}

				var resp = new MovieDetailsDTO(res, localPath, casts, duration);
				results.Add(resp);
			}
			return results;
		}
	}
}
