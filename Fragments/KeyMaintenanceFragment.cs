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
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = inflater.Inflate(Resource.Layout.key_maintenance, container, false);

            // Resolve and Wire up controls

            return rootView;
        }
    }
}