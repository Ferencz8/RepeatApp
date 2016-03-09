﻿using System;
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

namespace Repeat
{
    [Activity(Label = "Repeat", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        DrawerLayout mDrawerLayout;
        List<string> notebookItems = new List<string>();
        SideMenuAdapter notebooksAdapter;
        ListView mLeftDrawer;
        ListView elements;
        Button addButton;
        Button menuButton;
        ActionBarDrawerToggle mDrawerToggle;


		protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.myDrawer);
            mLeftDrawer = FindViewById<ListView>(Resource.Id.leftListView);
            addButton = FindViewById<Button>(Resource.Id.addButton);
            elements = FindViewById<ListView>(Resource.Id.elements);
            menuButton = FindViewById<Button>(Resource.Id.menuButton);
            //var ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, Storage.GetItems().Select(n => n.Name).ToList());

            //notebookItems.Add("First Left Item");
            //notebookItems.Add("Second Left Item");
            var ListAdapter = new MyCustomBaseAdapter(this);

            elements.Adapter = ListAdapter;

            addButton.Click += delegate
            {
                StartActivity(typeof(NoteDetailsActivity));
            };
            menuButton.Click += delegate
            {
                mDrawerLayout.OpenDrawer(mLeftDrawer);
            };

			mLeftDrawer.ItemClick += listView_ItemClick;

            mDrawerToggle = new MyActionBarDrawerToggle(this, mDrawerLayout, Resource.Drawable.Icon, Resource.String.open_drawer, Resource.String.close_drawer);

			notebooksAdapter = new SideMenuAdapter(this);//, Android.Resource.Layout.SimpleListItem1, notebookItems);
            mLeftDrawer.Adapter = notebooksAdapter;
            mDrawerLayout.SetDrawerListener(mDrawerToggle);
        }

		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			//Get our item from the list adapter
			var item = this.notebooksAdapter.GetItemAtPosition(e.Position);

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
            var ListAdapter = new MyCustomBaseAdapter(this); // new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, Storage.GetItems().Select(n => n.Name).ToList());

            elements.Adapter = ListAdapter;

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }
    }
}

