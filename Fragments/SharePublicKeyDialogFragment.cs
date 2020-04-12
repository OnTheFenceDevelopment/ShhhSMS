
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
    public class SharePublicKeyDialogFragment : DialogFragment
    {

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = inflater.Inflate(Resource.Layout.share_public_key_dialog, container, false);
            return rootView;
        }
    }
}
