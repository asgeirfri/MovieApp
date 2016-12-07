using System;
using UIKit;

namespace MovieApp.iOS.Controllers
{
	public class TabBarController : UITabBarController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.TabBar.BackgroundColor = UIColor.LightGray;
			this.TabBar.TintColor = UIColor.Red;

			this.SelectedIndex = 0;
		}

	}
}
