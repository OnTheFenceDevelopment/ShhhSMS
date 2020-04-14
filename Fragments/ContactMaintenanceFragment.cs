using System;

using AndroidX.Fragment.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ZXing.Mobile;
using ZXing;

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

        private async void AddContact_Click(object sender, EventArgs e)
        {
            var scanner = new MobileBarcodeScanner();
            scanner.TopText = "Scan Contact QR Code";
            scanner.CancelButtonText = "Cancel";

            var result = await scanner.Scan();

            if (IsValidContactScan(result))
            {
                // TODO: If scan is valid, open 'Add New Contact' Fragment
                var foo = 1;
            }
            else
            {
                Activity.RunOnUiThread(() => Toast.MakeText(Activity, "Invalid Scan - Please Try Again", ToastLength.Long).Show());
            }
        }

        private bool IsValidContactScan(Result scannedData)
        {
            Guid contactId;

            // First make sure that it was a QR Code that was scanned
            if (scannedData.BarcodeFormat != BarcodeFormat.QR_CODE)
                return false;
            
            var scanElements = scannedData.Text.Split("|");

            // Ensure correct number of elements
            if (scanElements.Length != 2)
                return false;

            // Ensure ContactId is a valid Guid
            if (Guid.TryParse(scanElements[0], out contactId) == false)
                return false;

            // Ensure decoded PublicKey is a valid byte array of the correct length
            var publicKeyBytes = Convert.FromBase64String(scanElements[1]);
            if(publicKeyBytes.Length != 32)
                return false;

            return true;
        }
    }
}