using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using Repeat.Mobile.PCL.DependencyManagement;
using Repeat.Mobile.PCL.DAL.Repositories.Interfaces;
using Repeat.Mobile.Activities.Notebooks;
using Repeat.Mobile.Activities.Notes;
using Repeat.Mobile.Activities.SideMenu;
using Repeat.Mobile.PCL.DAL.Entities;
using Repeat.Mobile.Sync;
using Repeat.Mobile.PCL.Logging;
using Repeat.AppLayer;
using Repeat.Mobile.PCL.Common;
using Repeat.Common;

namespace Repeat.Mobile
{
	[Activity(Label = "Repeat", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : ActivityBase
	{
		DrawerLayout drawerLayout;
		List<string> notebookItems = new List<string>();
		NotebooksAdapter notebooksAdapter;
		NotesAdapter notesAdapter;
		ListView notebooks;
		ListView notes;
		Button addNoteButton;
		Button menuButton;
		Button syncButton;
		Button addNotebookButton;
		ActionBarDrawerToggle drawerToggle;
		LinearLayout leftSideMenu;

		Guid chosenNotebookId;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			drawerLayout = FindViewById<DrawerLayout>(Resource.Id.myDrawer);
			notebooks = FindViewById<ListView>(Resource.Id.notebooks);
			leftSideMenu = FindViewById<LinearLayout>(Resource.Id.leftSideMenu);
			addNoteButton = FindViewById<Button>(Resource.Id.addButton);
			notes = FindViewById<ListView>(Resource.Id.notes);
			menuButton = FindViewById<Button>(Resource.Id.menuButton);
			syncButton = FindViewById<Button>(Resource.Id.syncButton);
			addNotebookButton = FindViewById<Button>(Resource.Id.addNotebookButton);

			drawerToggle = new SideMenuDrawerToggle(this, drawerLayout, Resource.Drawable.Icon, Resource.String.open_drawer, Resource.String.close_drawer);
			drawerLayout.SetDrawerListener(drawerToggle);
			drawerLayout.CloseDrawer(leftSideMenu);

			notebooksAdapter = new NotebooksAdapter(this);
			notebooks.Adapter = notebooksAdapter;
			chosenNotebookId = Guid.Parse(notebooksAdapter.GetItemAtPosition(0).Id);

			notesAdapter = new NotesAdapter(this, chosenNotebookId);
			notes.Adapter = notesAdapter;

			addNoteButton.Click += AddNoteButton_Click;

			menuButton.Click += MenuButton_Click;

			syncButton.Click += SyncButton_Click;

			addNotebookButton.Click += AddNotebookButton_Click;

			notebooks.ItemClick += Notebooks_ItemClick; ;

			notes.ItemClick += Notes_ItemClick;
		}



		private void AddNoteButton_Click(object sender, EventArgs e)
		{
			//StartNoteDetailsActivity
			Intent intent = new Intent(this, typeof(NoteDetailsActivity));
			Bundle notesBundle = new Bundle();
			notesBundle.PutString("notebookId", chosenNotebookId.ToString());
			notesBundle.PutString("action", "ADD");
			intent.PutExtras(notesBundle);
			StartActivity(intent, notesBundle);
		}

		private void MenuButton_Click(object sender, EventArgs e)
		{
			//mDrawerLayout.OpenDrawer(mLeftDrawer);
			drawerLayout.OpenDrawer(leftSideMenu);
		}

		private void SyncButton_Click(object sender, EventArgs e)
		{
			if (NetworkConnection.IsOnline(this))
			{

				Kernel.Get<ILog>().Info(Guid.Empty, "Sync button clicked!");

				syncButton.Enabled = false;

				Toast.MakeText(this, "Sync Started!", ToastLength.Short).Show();

				Syncronizer.CreateSyncher().StartSynching(DbSyncStarted, DbSyncEnded);
			}
			else
			{
				Toast.MakeText(this, "Not internet connection!", ToastLength.Short).Show();
			}
		}

		private void DbSyncStarted()
		{
			RunOnUiThread(() => Toast.MakeText(this, "Db Sync started", ToastLength.Long));
		}

		private void DbSyncEnded()
		{
			RunOnUiThread(() =>
			{
				RefreshNotesListContent();
				Toast.MakeText(this, "Db Sync ended", ToastLength.Long);
				syncButton.Enabled = true;
			});			
		}

		private void AddNotebookButton_Click(object sender, EventArgs e)
		{
			var builder = new AlertDialog.Builder(this);
			builder.SetTitle("Add Notebook");
			EditText edit = new EditText(this);
			builder.SetView(edit);
			builder.SetPositiveButton("Save", (alertDialogSender, args) =>
			{
				/* do stuff on OK */
				string notebookName = edit.Text;

				//TODO:: TempSolution maybe add one more layer for handling CRUD operations
				var repo = Kernel.Get<INotebooksRepository>();
				int rowschanged = repo.Add(new Notebook()
				{
					Id = Guid.NewGuid().ToString(),
					Name = notebookName,
					CreatedDate = DateTime.UtcNow,
					ModifiedDate = DateTime.UtcNow,
				});
				if (rowschanged == 1)
				{
					chosenNotebookId = Guid.Parse(repo.GetByName(notebookName).Id);
				}
				notebooksAdapter.RefreshContent();
				notebooksAdapter.NotifyDataSetChanged();

				RefreshNotesListContent();
			});
			builder.Show();
		}

		private void Notes_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			Note noteClicked = notesAdapter.GetItemAtPosition(e.Position);

			//StartNoteDetailsActivity
			Intent intent = new Intent(this, typeof(NoteDetailsActivity));
			Bundle notesBundle = new Bundle();
			notesBundle.PutString("notebookId", chosenNotebookId.ToString());
			notesBundle.PutString("action", "EDIT");
			notesBundle.PutString("note", ObjectConverter.ToJSON(noteClicked));
			intent.PutExtras(notesBundle);
			StartActivity(intent, notesBundle);
		}

		private void Notebooks_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			//Get our item from the list adapter
			var item = notebooksAdapter.GetItemAtPosition(e.Position);

			chosenNotebookId = Guid.Parse(item.Id);

			RefreshNotesListContent();
			//Make a toast with the item name just to show it was clicked
			Toast.MakeText(this, item.Name + " Chosen!", ToastLength.Short).Show();
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
			RefreshNotesListContent();
		}

		//protected override void OnDestroy()
		//{
		//    base.OnDestroy();
		//}

		//protected override void OnStop()
		//{
		//    base.OnStop();
		//}

		protected override void OnPause()
		{
			base.OnPause();
		}



		private void RefreshNotesListContent()
		{
			notesAdapter.RefreshContent(chosenNotebookId);
			notes.Adapter = notesAdapter;
		}

		protected override void FinishedInitializing()
		{

		}
	}
}

