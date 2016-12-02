using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using MovieApp.iOS.Views;


namespace MovieApp.iOS.Controllers
{
	public class MovieListSource : UITableViewSource
	{
		public readonly NSString MovieListCellId = new NSString("MovieListCell");
		private List<MovieDetailsDTO> _movies;
		private Action<int> _onSelectedPerson;

		public MovieListSource(List<MovieDetailsDTO> movies, Action<int> onSelectedPerson)
		{
			this._movies = movies;
			this._onSelectedPerson = onSelectedPerson;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = (MovieCell)tableView.DequeueReusableCell(this.MovieListCellId);

			if (cell == null)
			{
				cell = new MovieCell((NSString)this.MovieListCellId);
			}

			int row = indexPath.Row;

			string titleDisp = this._movies[row].info.Title + " (" + this._movies[row].info.ReleaseDate.Year.ToString() + ")";
			string actorsDisp = this._movies[row].casts;
			string img = _movies[row].poster;
			MovieCellDTO dto = new MovieCellDTO(titleDisp, actorsDisp, img);
			cell.UpdateCell(dto);

			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return this._movies.Count;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);
			this._onSelectedPerson(indexPath.Row);
		}
	}
}
