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
using Android.Net;

namespace Repeat.Common
{
	public static class NetworkConnection
	{

		public static bool IsOnline(Context context)
		{
			ConnectivityManager connectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
			NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;

			bool isOnline = (activeConnection != null) && activeConnection.IsConnected;

			return isOnline;
			//if (isOnline)
			//{
			//	// type of connection
			//	NetworkInfo.State activeState = activeConnection.GetState();

			//	// Check for a WiFi connection
			//	NetworkInfo wifiInfo = connectivityManager.GetNetworkInfo(ConnectivityType.Wifi);
			//	if (wifiInfo.IsConnected)
			//	{
					
			//	}
			//	else
			//	{
					
			//	}

			//	// Check if roaming
			//	NetworkInfo mobileInfo = connectivityManager.GetNetworkInfo(ConnectivityType.Mobile);
			//	if (mobileInfo.IsRoaming && mobileInfo.IsConnected)
			//	{
					
			//	}
			//	else
			//	{
				
			//	}
			//}
			//else
			//{
			//	//no internet connection
			//	return false;
			//}
		}
	}
}