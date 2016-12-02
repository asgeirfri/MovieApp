using System;
using System.Collections.Generic;
using System.Linq;
using MovieApp.iOS.Controllers;
using Foundation;
using UIKit;
using System.Drawing;

namespace MovieApp.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override UIWindow Window
		{
			get;
			set;
		}
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			this.Window = new UIWindow(UIScreen.MainScreen.Bounds);

			var movieSearchController = new MovieController();
			var movieSearchNavigationController = new UINavigationController(movieSearchController);

			var movieTopRatedController = new MovieTopRatedController();
			var movieTopRatedNavitationController = new UINavigationController(movieTopRatedController);

			var tabBarController = new TabBarController()
			{
				ViewControllers = new UIViewController[] { movieSearchNavigationController, movieTopRatedNavitationController }
			};

			this.Window.RootViewController = tabBarController;
			this.Window.MakeKeyAndVisible();
			return true;
		}
		private UICollectionViewFlowLayout CreateFlowLayout()
		{
			return new UICollectionViewFlowLayout()
			{
				MinimumLineSpacing = 5,
				MinimumInteritemSpacing = 5,
				ItemSize = new SizeF(80, 80)
			};
		}
	}
}
