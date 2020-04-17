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

using ZXingScannerFragment = ShhhSMS.Models.ZXingScannerFragment;
using ZXing.Mobile;
using ZXing;

namespace ShhhSMS.Fragments
{
    public class ScanPublicKeyDialogFragment : DialogFragment
    {
        ZXingScannerFragment scanFragment;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var rootView = inflater.Inflate(Resource.Layout.scan_public_key_dialog, container, false);

            if (scanFragment == null)
            {
                scanFragment = new ZXingScannerFragment();

                ChildFragmentManager.BeginTransaction()
                    .Replace(Resource.Id.scanner_container, scanFragment)
                    .Commit();
            }

            return rootView;
        }

        public override void OnResume()
        {
            base.OnResume();
            Scan();
        }

        public override void OnPause()
        {
            scanFragment?.StopScanning();
            base.OnPause();
        }

        private void Scan()
        {
            var opts = new MobileBarcodeScanningOptions
            {
                PossibleFormats = new List<ZXing.BarcodeFormat> {
                    ZXing.BarcodeFormat.QR_CODE
                },
                CameraResolutionSelector = availableResolutions =>
                {

                    foreach (var ar in availableResolutions)
                    {
                        Console.WriteLine("Resolution: " + ar.Width + "x" + ar.Height);
                    }
                    return null;
                }
            };

            scanFragment.StartScanning(result =>
            {
                // Null result means scanning was cancelled
                if (result == null || string.IsNullOrEmpty(result.Text))
                {
                    Toast.MakeText(Activity, "Scanning Cancelled", ToastLength.Long).Show();
                    return;
                }

                if (IsValidContactScan(result) == false)
                {
                    TargetFragment.OnActivityResult(Constants.Scan_Public_Key_Request_Code, Constants.Unsuccessful_Public_Key_Scan, new Intent());
                    Dismiss();
                }

                // Otherwise, proceed with result
                var publicKeyElements = PublicKeyElements(result.Text);

                Activity.RunOnUiThread(() =>
                {                    
                    var resultBundle = new Bundle();
                    resultBundle.PutString("contactId", publicKeyElements[0]);
                    resultBundle.PutString("publicKey", publicKeyElements[1]);

                    var resultIntent = new Intent().PutExtras(resultBundle);

                    TargetFragment.OnActivityResult(Constants.Scan_Public_Key_Request_Code, Constants.Successful_Public_Key_Scan, resultIntent);

                    Dismiss();
                });

            }, opts);
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
            if (publicKeyBytes.Length != 32)
                return false;

            return true;
        }

        private string[] PublicKeyElements(string rawScanData)
        {
            var scanElements = rawScanData.Replace("{", string.Empty).Replace("}", string.Empty).Split("|");
            return new string[] { scanElements[0], scanElements[1] };
        }
    }
}