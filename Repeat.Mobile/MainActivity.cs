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
using Repeat.Mobile.PCL.DAL;
using System.IO;
using SQLite.Net.Platform.XamarinAndroid;
using Repeat.Mobile.PCL.DependencyManagement;
using Repeat.Mobile.PCL.DAL.Repositories.Interfaces;
using Repeat.Mobile.Activities.Notebooks;
using Repeat.Mobile.Activities.Notes;
using Repeat.Mobile.Activities.SideMenu;

namespace Repeat.Mobile
{
	[Activity(Label = "Repeat", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		DrawerLayout drawerLayout;
		List<string> notebookItems = new List<string>();
		NotebooksAdapter notebooksAdapter;
		NotesAdapter notesAdapter;
		ListView notebooks;
		ListView notes;
		Button addNoteButton;
		Button menuButton;
		Button addNotebookButton;
		ActionBarDrawerToggle drawerToggle;
		LinearLayout leftSideMenu;

		int chosenNotebookId;

		protected override void OnCreate(Bundle bundle)
		{
			StartUp.Configure();

			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			drawerLayout = FindViewById<DrawerLayout>(Resource.Id.myDrawer);
			notebooks = FindViewById<ListView>(Resource.Id.notebooks);
			leftSideMenu  = FindViewById<LinearLayout>(Resource.Id.leftSideMenu);
			addNoteButton = FindViewById<Button>(Resource.Id.addButton);
			notes = FindViewById<ListView>(Resource.Id.notes);
			menuButton = FindViewById<Button>(Resource.Id.menuButton);
			addNotebookButton = FindViewById<Button>(Resource.Id.addNotebookButton);
			
			drawerToggle = new SideMenuDrawerToggle(this, drawerLayout, Resource.Drawable.Icon, Resource.String.open_drawer, Resource.String.close_drawer);
			drawerLayout.SetDrawerListener(drawerToggle);
			drawerLayout.CloseDrawer(leftSideMenu);

			notebooksAdapter = new NotebooksAdapter(this);//, Android.Resource.Layout.SimpleListItem1, notebookItems);
			notebooks.Adapter = notebooksAdapter;
			chosenNotebookId = notebooksAdapter.GetItemAtPosition(0).Id;

			notesAdapter = new NotesAdapter(this, chosenNotebookId);
			notes.Adapter = notesAdapter;

			addNoteButton.Click += delegate
			{
				StartNoteDetailsActivity();
			};
			menuButton.Click += delegate
			{
				//mDrawerLayout.OpenDrawer(mLeftDrawer);
				drawerLayout.OpenDrawer(leftSideMenu);
			};

			notebooks.ItemClick += listView_ItemClick;

			addNotebookButton.Click += delegate
			{

				var builder = new AlertDialog.Builder(this);
				builder.SetTitle("Add Notebook");
				EditText edit = new EditText(this);
				builder.SetView(edit);
				builder.SetPositiveButton("Save", (sender, args) => 
				{
					/* do stuff on OK */
					string notebookName = edit.Text;

					//TODO:: TempSolution maybe add one more layer for handling CRUD operations
					var repo = Kernel.Get<INotebooksRepository>();
					int rowschanged = repo.Add(new PCL.DAL.Entities.Notebook()
					{
						Name = notebookName,
					});
					if(rowschanged == 1)
					{
						chosenNotebookId = repo.GetByName(notebookName).Id;
					}
					notebooksAdapter.RefreshContent();
					notebooksAdapter.NotifyDataSetChanged();

					notesAdapter.RefreshContent(chosenNotebookId);
					notes.Adapter = notesAdapter;
				});
				builder.Show();
			};
		}

		private void StartNoteDetailsActivity()
		{
			Intent intent = new Intent(this, typeof(NoteDetailsActivity));
			Bundle notesBundle = new Bundle();
			notesBundle.PutInt("notebookId", chosenNotebookId);
			intent.PutExtras(notesBundle);
			StartActivity(intent, notesBundle);
		}

		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			//Get our item from the list adapter
			var item = notebooksAdapter.GetItemAtPosition(e.Position);

			chosenNotebookId = item.Id;

			notesAdapter.RefreshContent(chosenNotebookId);
			notes.Adapter = notesAdapter;
			//Make a toast with the item name just to show it was clicked
			Toast.MakeText(this, item.Name + " Clicked!", ToastLength.Short).Show();
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (drawerToggle.OnOptionsItemSelected(item))
			{
			}

			switch (item.ItemId)
			{
				case Resource.Id.menuButton:
					{
						drawerLayout.OpenDrawer(notebooks);
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

