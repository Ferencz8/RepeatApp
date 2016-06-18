using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Repeat.Mobile.Activities.Notebooks;
using Android.Support.V7.App;
using Android.Support.V4.Widget;

namespace Repeat.Mobile.Activities.SideMenu
{
    public class SideMenuDrawerToggle : ActionBarDrawerToggle
    {
        Activity mActivity;
		ListView leftListView;

        public SideMenuDrawerToggle(Activity activity, DrawerLayout drawerLayout, int openDrawerDesc, int closeDrawerDesc)//int imageResource, 
			: base(activity, drawerLayout, openDrawerDesc, closeDrawerDesc)
        {
            mActivity = activity;
        }

        public override void OnDrawerOpened(View drawerView)
        {
            int drawerType = (int)drawerView.Tag;

            if (drawerType == 0)
            {
				var drawerList = drawerView as ListView;
				if(drawerList != null)
				{
					drawerList.Adapter = new NotebooksAdapter(mActivity);
				}
                //Left Drawer
                base.OnDrawerOpened(drawerView);
                mActivity.ActionBar.Title = "Notebooks";
            }
        }

        public override void OnDrawerClosed(View drawerView)
        {
            int drawerType = (int)drawerView.Tag;

            if (drawerType == 0)
            {
                //Left Drawer
                base.OnDrawerClosed(drawerView);
                mActivity.ActionBar.Title = "Notebooks";
            }
        }

        public override void OnDrawerSlide(View drawerView, float slideOffset)
        {
            int drawerType = (int)drawerView.Tag;

            if (drawerType == 0)
            {
                //Left Drawer
                base.OnDrawerSlide(drawerView, slideOffset);
            }
        }
    }
}