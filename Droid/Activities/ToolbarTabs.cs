using System;
using Android.App;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Widget;

namespace MovieApp.Droid
{
	using Android.Support.V4.App;

	public class ToolbarTabs
	{
		public static void Construct(FragmentActivity activity, Toolbar toolbar)
		{
			TopMovieFragment topMovieFragment = new TopMovieFragment();
			var fragments = new Fragment[]
			{
				new MovieSearchFragment(),
				topMovieFragment
			};

			var titles = CharSequence.ArrayFromStringArray(new[]
			{
				"Search",
				"Top Rated"
			});

			var viewPager = activity.FindViewById<ViewPager>(Resource.Id.viewpager);
			viewPager.Adapter = new TabsFragmentPageAdapter(activity.SupportFragmentManager, fragments, titles);

			var tabLayout = activity.FindViewById<TabLayout>(Resource.Id.sliding_tabs);
			tabLayout.SetupWithViewPager(viewPager);
			viewPager.PageSelected += (sender, args) =>
			{
				if (args.Position == 1)
				{
					topMovieFragment.GetTopMovies(activity);
				}
			};
			SetToolbar(activity, toolbar);
		}

		public static void SetToolbar(Activity activity, Toolbar toolbar)
		{
			activity.SetActionBar(toolbar);
			activity.ActionBar.Title = activity.GetString(Resource.String.ToolbarTitle);
		}
	}
}
