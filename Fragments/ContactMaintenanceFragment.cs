using System;

using AndroidX.Fragment.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;

namespace ShhhSMS.Fragments
{
    public class ContactMaintenanceFragment : Fragment
    {
        private Button _addContact;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
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
            OpenScanner();
        }

        private void OpenScanner()
        {
            // TODO: If scan is valid, open 'Add New Contact' Fragment
            var fragmentTransaction = ParentFragmentManager.BeginTransaction();

            // Remove fragment else it will crash as it is already added to backstack
            var prev = ParentFragmentManager.FindFragmentByTag("scanContactDialog");
            if (prev != null)
                fragmentTransaction.Remove(prev);

            // Create and show the dialog
            var newFragment = new ScanPublicKeyDialogFragment();
            newFragment.SetTargetFragment(this, Constants.Scan_Public_Key_Request_Code);

            //Add fragment
            newFragment.Show(fragmentTransaction, "scanContactDialog");
        }

        private void OpenAddContactDialog(string contactId, string publicKey)
        {
            var fragmentTransaction = ParentFragmentManager.BeginTransaction();

            // Remove fragment else it will crash as it is already added to backstack
            var prev = ParentFragmentManager.FindFragmentByTag("addContactDialog");
            if (prev != null)
                fragmentTransaction.Remove(prev);

            // Create and show the dialog
            var newFragment = new AddContactDialogFragment();
            var dataBundle = new Bundle();

            dataBundle.PutString("contactId", contactId);
            dataBundle.PutString("publicKey", publicKey);

            newFragment.Arguments = dataBundle;

            //Add fragment
            newFragment.Show(fragmentTransaction, "addContactDialog");
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == Constants.Scan_Public_Key_Request_Code)
            {
                // Ensure Dialog is removed from backstack etc
                var prev = ParentFragmentManager.FindFragmentByTag("scanContactDialog");
                var cunt = ChildFragmentManager.Fragments;
                if (prev != null)
                {
                    var fragmentTransaction = ParentFragmentManager.BeginTransaction();
                    fragmentTransaction.Remove(prev);
                    fragmentTransaction.Commit();
                }

                if (resultCode == Constants.Successful_Public_Key_Scan)
                {
                    // TODO: Pull Data elements and pass to Add Contact Dialog
                    var contactId = data.GetStringExtra("contactId");
                    var publicKey = data.GetStringExtra("publicKey");

                    OpenAddContactDialog(contactId, publicKey);
                }
                else if (resultCode == Constants.Unsuccessful_Public_Key_Scan)
                {
                    Activity.RunOnUiThread(() => Toast.MakeText(Activity, "Invalid Barcode - Please try again!", ToastLength.Long).Show());
                }
                else
                {
                    Activity.RunOnUiThread(() => Toast.MakeText(Activity, "Hmmm - something weird just happened, please try again!", ToastLength.Long).Show());
                }
            }
            else if (requestCode == Constants.Add_Contact_Request_Code)
            {

            }
        }
    }
}