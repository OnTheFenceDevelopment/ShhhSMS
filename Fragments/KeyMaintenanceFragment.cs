using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AndroidX.Fragment.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ShhhSMS.Fragments
{
    public class KeyMaintenanceFragment : Fragment
    {
        public Button _sharePublicKey;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = inflater.Inflate(Resource.Layout.key_maintenance, container, false);

            _sharePublicKey = rootView.FindViewById<Button>(Resource.Id.sharePublicKey);
            _sharePublicKey.Click += SharePublicKey_Click;

            // Resolve and Wire up controls

            return rootView;
        }

        private void SharePublicKey_Click(object sender, EventArgs e)
        {
            FragmentTransaction ft = ParentFragmentManager.BeginTransaction();

            //Remove fragment else it will crash as it is already added to backstack
            Fragment prev = ParentFragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }

            ft.AddToBackStack(null);

            // Create and show the dialog.
            var newFragment = new SharePublicKeyDialogFragment();

            //Add fragment
            newFragment.Show(ft, "dialog");
        }
    }
}