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
    public class ContactMaintenanceFragment : Fragment
    {
        private Button _addContact;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = inflater.Inflate(Resource.Layout.contact_maintenance, container, false);
            _addContact = rootView.FindViewById<Button>(Resource.Id.addContact);
            _addContact.Click += AddContact_Click;

            return rootView;
        }

        private void AddContact_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}