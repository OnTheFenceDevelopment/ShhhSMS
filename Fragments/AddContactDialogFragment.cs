using Android.OS;
using Android.Views;

using AndroidX.Fragment.App;
using System;

namespace ShhhSMS.Fragments
{
    public class AddContactDialogFragment : DialogFragment
    {
        private Guid _contactId;
        private string _publicKey;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = inflater.Inflate(Resource.Layout.add_contact_dialog, container, false);

            _contactId = new Guid(Arguments.GetString("contactId"));
            _publicKey = Arguments.GetString("publicKey");

            return rootView;
        }
    }
}