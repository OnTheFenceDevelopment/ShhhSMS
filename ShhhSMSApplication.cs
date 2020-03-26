using System;
using Android.App;
using Android.Runtime;

namespace ShhhSMS
{
    [Application]
    public class ShhhSMSApplication : Application
    {

        public ShhhSMSApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public async override void OnCreate()
        {
            base.OnCreate();

            var deviceId = await Xamarin.Essentials.SecureStorage.GetAsync("deviceId");

            if (deviceId == null)
            {
                deviceId = Guid.NewGuid().ToString();
                await Xamarin.Essentials.SecureStorage.SetAsync("deviceId", deviceId);
            }
        }

        public override void OnTerminate()
        {
            // TODO: Remove Password from Secure Storage

            base.OnTerminate();
        }
    }
}
