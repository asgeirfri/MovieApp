using System;

using Android.App;
using Android.Runtime;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;

namespace MovieApp.Droid.Activities
{
	[Activity(Label = "Movie Search", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true)]
	public class MainActivity : FragmentActivity
	{

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			this.SetContentView(Resource.Layout.Main);

			var toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
			ToolbarTabs.Construct(this, toolbar);
		}


	}
}
