﻿using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using ZXing;
using ZXing.Mobile;

namespace ShhhSMS.Models
{
    /// <summary>
    /// The 'official' version of ZXingScannerFragment switches out the AndroidX.Fragment.App and Android.Support.App based on the API version of the OS on the device
    /// I wanted/needed to use the AndroidX version regardless - hence the separate class (copied from the source but with the #if __ANDROID_29__ conditional statement removed)
    /// Original Source at: https://github.com/Redth/ZXing.Net.Mobile/blob/master/ZXing.Net.Mobile/Android/ZXingScannerFragment.android.cs 
    /// </summary>
    public class ZXingScannerFragment : Fragment, IZXingScanner<View>, IScannerView
    {
        private ZXingSurfaceView scanner;
        private ZxingOverlayView zxingOverlay;
        private Action<Result> scanCallback;

        public ZXingScannerFragment()
        {
            UseCustomOverlayView = false;
        }

        FrameLayout frame;

        public override View OnCreateView(LayoutInflater layoutInflater, ViewGroup viewGroup, Bundle bundle)
        {
            frame = (FrameLayout)layoutInflater.Inflate(ZXing.Net.Mobile.Resource.Layout.zxingscannerfragmentlayout, viewGroup, false);

            var layoutParams = GetChildLayoutParams();

            try
            {
                scanner = new ZXingSurfaceView(Activity, ScanningOptions);

                frame.AddView(scanner, layoutParams);

                if (!UseCustomOverlayView)
                {
                    zxingOverlay = new ZxingOverlayView(Activity);
                    zxingOverlay.TopText = TopText ?? "";
                    zxingOverlay.BottomText = BottomText ?? "";

                    frame.AddView(zxingOverlay, layoutParams);
                }
                else if (CustomOverlayView != null)
                {
                    frame.AddView(CustomOverlayView, layoutParams);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Surface View Failed: " + ex);
            }

            Android.Util.Log.Debug(MobileBarcodeScanner.TAG, "ZXingScannerFragment->OnResume exit");

            return frame;
        }

        public override void OnStart()
        {
            base.OnStart();
            // won't be 0 if OnCreateView has been called before.
            if (frame.ChildCount == 0)
            {
                var layoutParams = GetChildLayoutParams();
                // reattach scanner and overlay views.
                frame.AddView(scanner, layoutParams);

                if (!UseCustomOverlayView)
                    frame.AddView(zxingOverlay, layoutParams);
                else if (CustomOverlayView != null)
                    frame.AddView(CustomOverlayView, layoutParams);
            }
        }

        public override void OnStop()
        {
            if (scanner != null)
            {
                scanner.StopScanning();
                frame.RemoveView(scanner);
            }

            if (!UseCustomOverlayView)
                frame.RemoveView(zxingOverlay);
            else if (CustomOverlayView != null)
                frame.RemoveView(CustomOverlayView);

            base.OnStop();
        }

        LinearLayout.LayoutParams GetChildLayoutParams()
        {
            var layoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            layoutParams.Weight = 1;
            return layoutParams;
        }

        public View CustomOverlayView { get; set; }
        public bool UseCustomOverlayView { get; set; }
        public MobileBarcodeScanningOptions ScanningOptions { get; set; }
        public string TopText { get; set; }
        public string BottomText { get; set; }
        public void Torch(bool on) => scanner?.Torch(on);
        public void AutoFocus() => scanner?.AutoFocus();
        public void AutoFocus(int x, int y) => scanner?.AutoFocus(x, y);

        public void StartScanning(Action<Result> scanResultHandler, MobileBarcodeScanningOptions options = null)
        {
            ScanningOptions = options;
            scanCallback = scanResultHandler;

            if (scanner == null)
                return;

            Scan();
        }

        void Scan() => scanner?.StartScanning(scanCallback, ScanningOptions);
        public void StopScanning() => scanner?.StopScanning();
        public void PauseAnalysis() => scanner?.PauseAnalysis();
        public void ResumeAnalysis() => scanner?.ResumeAnalysis();
        public void ToggleTorch() => scanner?.ToggleTorch();
        public bool IsTorchOn => scanner?.IsTorchOn ?? false;
        public bool IsAnalyzing => scanner?.IsAnalyzing ?? false;
        public bool HasTorch => scanner?.HasTorch ?? false;
    }
}