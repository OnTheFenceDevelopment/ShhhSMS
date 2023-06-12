using System;

using AndroidX.Fragment.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ShhhSMS.Services;

namespace ShhhSMS.Fragments
{
    public class KeyMaintenanceFragment : Fragment
    {
        public Button _sharePublicKey;

        // TODO: Replace with IOC
        IEncryptionService encryptionService;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            encryptionService = new EncryptionService();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = inflater.Inflate(Resource.Layout.key_maintenance, container, false);

            _sharePublicKey = rootView!.FindViewById<Button>(Resource.Id.sharePublicKey);
            _sharePublicKey!.Click += SharePublicKey_Click;

            return rootView;
        }

        private async void SharePublicKey_Click(object sender, EventArgs e)
        {
            var fragmentTransaction = ParentFragmentManager.BeginTransaction();

            // Remove fragment else it will crash as it is already added to backstack
            var prev = ParentFragmentManager.FindFragmentByTag("publicKeyDialog");

            if (prev != null)
            {
                fragmentTransaction.Remove(prev);
            }

            fragmentTransaction.AddToBackStack(null);

            // Create and show the dialog
            var newFragment = new SharePublicKeyDialogFragment(await encryptionService.GetQRCodeContent());

            //Add fragment
            newFragment.Show(fragmentTransaction, "publicKeyDialog");
        }
    }
}