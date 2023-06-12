
using System;
using AndroidX.Fragment.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ZXing.Common;
using ZXing;
using Android.Graphics;

namespace ShhhSMS.Fragments
{
    public class SharePublicKeyDialogFragment : DialogFragment
    {
        private ImageView _publicKeyQRCode;
        private string _barcodeContent;

        public SharePublicKeyDialogFragment(string barcodeContent)
        {
            _barcodeContent = barcodeContent;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = inflater.Inflate(Resource.Layout.share_public_key_dialog, container, false);

            _publicKeyQRCode = rootView!.FindViewById<ImageView>(Resource.Id.publicKeyQRCode);

            var bitmapMatrix = new MultiFormatWriter().encode(_barcodeContent, BarcodeFormat.QR_CODE, 660, 660);

            var width = bitmapMatrix.Width;
            var height = bitmapMatrix.Height;
            int[] pixelsImage = new int[width * height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (bitmapMatrix[j, i])
                        pixelsImage[i * width + j] = (int)Convert.ToInt64(0xff000000);
                    else
                        pixelsImage[i * width + j] = (int)Convert.ToInt64(0xffffffff);
                }
            }

            Bitmap bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            bitmap.SetPixels(pixelsImage, 0, width, 0, 0, width, height);
            _publicKeyQRCode!.SetImageBitmap(bitmap);

            return rootView;
        }
    }
}
