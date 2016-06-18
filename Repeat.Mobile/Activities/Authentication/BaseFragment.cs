using Android.App;

namespace Repeat.Activities.Authentication
{
	public class BaseFragment : Fragment
	{
		private bool removeFragment = false;

		public void RemoveThisFragment()
		{
			removeFragment = true;
			Fragment fragment = FragmentManager.FindFragmentByTag(Tag);
			FragmentManager fragmentManager = Activity.FragmentManager;
			FragmentTransaction fragmentTransaction = fragmentManager.BeginTransaction();

			fragmentTransaction.Remove(fragment);

			fragmentTransaction.SetTransition(FragmentTransit.FragmentFade);

			fragmentTransaction.Commit();
		}

		public void CreateNewFragment(Fragment newFragment)
		{
			Fragment fragment = FragmentManager.FindFragmentByTag(Tag);
			FragmentManager manager = Activity.FragmentManager;
			FragmentTransaction fragmentTransaction = manager.BeginTransaction();

			fragmentTransaction.Replace(fragment.Id, newFragment, newFragment.GetType().Name);
			fragmentTransaction.AddToBackStack(null);
			
			fragmentTransaction.SetTransition(FragmentTransit.FragmentFade);
			fragmentTransaction.Commit();
		}

		public void ShowExistingFragment(string existingFragmentTagName, bool clearFragmentTop)
		{

			Fragment fragment = FragmentManager.FindFragmentByTag(Tag);
			Fragment existingFragment = FragmentManager.FindFragmentByTag(existingFragmentTagName);

			FragmentManager manager = Activity.FragmentManager;
			FragmentTransaction fragmentTransaction = manager.BeginTransaction();


			if (clearFragmentTop)
			{
				int count = manager.BackStackEntryCount;

				for (int i = (count - 1); i > 0; i--)
				{
					FragmentManager.IBackStackEntry backStackEntry = manager.GetBackStackEntryAt(i);

					int id = backStackEntry.Id;
					string currentFragmentName = backStackEntry.Name;
					
					manager.PopBackStack();
				}
			}
			else
			{
				fragmentTransaction.Replace(fragment.Id, existingFragment);
			}

			fragmentTransaction.SetTransition(FragmentTransit.FragmentFade);
			fragmentTransaction.Commit();
		}

		public bool CheckIfFragmentExists(string existingFragmentTagName)
		{
			return FragmentManager.FindFragmentByTag(existingFragmentTagName)!=null ? true : false;
		}

		public override void OnDestroyView()
		{
			base.OnDestroyView();
			if (removeFragment)
			{
				Activity.Finish();
			}
		}
	}
}