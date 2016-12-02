using System;
using UIKit;
using CoreGraphics;

namespace MovieApp.iOS
{
	public class MovieInfoController : UIViewController
	{
		private const int HorizontalMargin = 20;
		private const int StartY = 80;
		private const int StepY = 50;
		private int _yCord;

		MovieDetailsDTO _movie;

		public MovieInfoController(MovieDetailsDTO dto)
		{
			this._movie = dto;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.Title = "Movie Info";
			this.View.BackgroundColor = UIColor.Purple;
			this._yCord = StartY;

			UILabel title = CreateLabel(_movie.info.Title + " (" + _movie.info.ReleaseDate.Year.ToString() + ")", HorizontalMargin, this.View.Bounds.Width, 50, 30);
			StepDown();

			string genreString = "";
			foreach (var genre in _movie.info.Genres)
			{
				genreString += genre.Name.ToString() + ", ";
			}
			genreString = genreString.Remove(genreString.Length - 2);
			UILabel details = CreateLabel(genreString, HorizontalMargin, this.View.Bounds.Width, 50, 12);
			StepDown();

			var posterImage = new UIImageView()
			{
				Frame = new CGRect(HorizontalMargin, this._yCord, 100, 130),
				Image = UIImage.FromFile(_movie.poster)

			};

			UILabel overview = CreateLabel(_movie.info.Overview, HorizontalMargin * 2 + 100, this.View.Bounds.Width - HorizontalMargin * 3 - 100, 130, 14);
			overview.LineBreakMode = UILineBreakMode.WordWrap;
			overview.Lines = 0;
			StepDown();

			this.View.AddSubview(title);
			this.View.AddSubview(details);
			this.View.AddSubview(posterImage);
			this.View.AddSubview(overview);

		}

		private UILabel CreateLabel(string text, int horizontalMargin, nfloat width, int height, nfloat fontSize)
		{
			var prompt = new UILabel()
			{
				Frame = new CGRect(horizontalMargin, this._yCord, width, height),
				Text = text,
				Font = UIFont.FromName("Arial", fontSize)
			};
			return prompt;
		}

		private void StepDown()
		{
			this._yCord += StepY;
		}
	}
}
