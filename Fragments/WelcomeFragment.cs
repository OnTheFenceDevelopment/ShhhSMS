﻿using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;

namespace ShhhSMS.Fragments
{
    public class WelcomeFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.welcome, container, false);
        }
    }
}