using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Linq;
using System.Collections.Generic;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using Repeat.DAL;
using System.IO;
using SQLite.Net.Platform.XamarinAndroid;

namespace Repeat
{
	[Activity(Label = "Repeat", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		DrawerLayout mDrawerLayout;
		List<string> notebookItems = new List<string>();
		NotebooksAdapter notebooksAdapter;
		ListView mLeftDrawer;
		ListView notes;
		Button addButton;
		Button menuButton;
		ActionBarDrawerToggle mDrawerToggle;


		int chosenNotebookId;

		protected override void OnCreate(Bundle bundle)
		{
			StartUp.Configure();

			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.myDrawer);
			mLeftDrawer = FindViewById<ListView>(Resource.Id.leftListView);
			addButton = FindViewById<Button>(Resource.Id.addButton);
			notes = FindViewById<ListView>(Resource.Id.notes);
			menuButton = FindViewById<Button>(Resource.Id.menuButton);

			
			mDrawerToggle = new SideMenuDrawerToggle(this, mDrawerLayout, Resource.Drawable.Icon, Resource.String.open_drawer, Resource.String.close_drawer);
			mDrawerLayout.SetDrawerListener(mDrawerToggle);

			notebooksAdapter = new NotebooksAdapter(this);//, Android.Resource.Layout.SimpleListItem1, notebookItems);
			mLeftDrawer.Adapter = notebooksAdapter;
			chosenNotebookId = notebooksAdapter.GetItemAtPosition(0).Id;

			notes.Adapter = new NotesAdapter(this, chosenNotebookId);

			addButton.Click += delegate
			{
				// StartActivity(typeof(NoteDetailsActivity));
				Intent intent = new Intent(this, typeof(NoteDetailsActivity));
				Bundle notesBundle = new Bundle();
				notesBundle.PutInt("notebookId", chosenNotebookId);
				intent.PutExtras(notesBundle);
				StartActivity(intent,notesBundle);
			};
			menuButton.Click += delegate
			{
				mDrawerLayout.OpenDrawer(mLeftDrawer);
			};

			mLeftDrawer.ItemClick += listView_ItemClick;
		}

		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			//Get our item from the list adapter
			var item = notebooksAdapter.GetItemAtPosition(e.Position);

			chosenNotebookId = item.Id;

			//Make a toast with the item name just to show it was clicked
			Toast.MakeText(this, item.Name + " Clicked!", ToastLength.Short).Show();
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (mDrawerToggle.OnOptionsItemSelected(item))
			{
			}

			switch (item.ItemId)
			{
				case Resource.Id.menuButton:
					{
						mDrawerLayout.OpenDrawer(mLeftDrawer);
					}

					return true;

				default:
					return base.OnOptionsItemSelected(item);
			}
		}

		protected override void OnResume()
		{
			base.OnResume();
			notes.Adapter = new NotesAdapter(this, chosenNotebookId); // new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, Storage.GetItems().Select(n => n.Name).ToList());
		}

		//protected override void OnDestroy()
		//{
		//    base.OnDestroy();
		//}

		//protected override void OnStop()
		//{
		//    base.OnStop();
		//}

		//protected override void OnPause()
		//{
		//    base.OnPause();
		//}
	}
}

