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

using ZXing.Mobile;

namespace ShhhSMS.Fragments
{
    public class AddContactDialogFragment : DialogFragment
    {
        ZXingScannerFragment scanFragment;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = inflater.Inflate(Resource.Layout.add_contact_dialog, container, false);

            scanFragment = new ZXingScannerFragment();

            //FragmentManager.BeginTransaction().Replace(Resource.Id.fragment_container, scanFragment).Commit();

            return rootView;
        }
    }
}