using System;
using System.Security;
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

            Password = new SecureString();
        }

        public override void OnTerminate()
        {
            if (Password != null)
            {
                // TODO: Should the Activity do this?
                Password.Dispose();
                Password = null;
            }

            base.OnTerminate();
        }

        public bool LoginRequired => Password.Length == 0;

        public SecureString Password { get; private set; }
    }
}
