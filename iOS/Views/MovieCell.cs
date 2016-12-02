using System;
using Foundation;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MovieApp.iOS.Views
{
	public class MovieCell : UITableViewCell	
	{
		private UILabel _titleLabel, _actorsLabel;
		private UIImageView _posterImage;
		public MovieCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
		{
			this._titleLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 16f),
				TextColor = UIColor.FromRGB(60, 15, 100)
			};

			this._actorsLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 10f),
				TextColor = UIColor.Brown
			};

			this._posterImage = new UIImageView();

			this.ContentView.AddSubviews(new UIView[] { this._titleLabel, this._actorsLabel, this._posterImage });
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			this._posterImage.Frame = new CGRect(5, 5, 40, 50);
			this._titleLabel.Frame = new CGRect(55, 5, this.ContentView.Bounds.Width - 55, 25);
			this._actorsLabel.Frame = new CGRect(55, 30, this.ContentView.Bounds.Width - 55, 20);
		}

		public void UpdateCell(MovieCellDTO dto)
		{
			this._titleLabel.Text = dto.title;
			this._actorsLabel.Text = dto.actors;
			if (File.Exists(dto.poster))
			{
				this._posterImage.Image = UIImage.FromFile(dto.poster);
			}

			this.Accessory = UITableViewCellAccessory.DisclosureIndicator;
		}
	}
}
