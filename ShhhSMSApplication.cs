using System;
using System.Threading.Tasks;
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

            await InitialiseDeviceId();
            await InitialiseContactId();
        }

        private async Task InitialiseDeviceId()
        {
            var deviceId = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.Identifiers.DeviceId);

            if (deviceId == null)
            {
                deviceId = Guid.NewGuid().ToString();
                await Xamarin.Essentials.SecureStorage.SetAsync(Constants.Identifiers.DeviceId, deviceId);
            }
        }

        private async Task InitialiseContactId()
        {
            var contactId = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.Identifiers.ContactId);

            if (contactId == null)
            {
                contactId = Guid.NewGuid().ToString();
                await Xamarin.Essentials.SecureStorage.SetAsync(Constants.Identifiers.ContactId, contactId);
            }
        }

        public override void OnTerminate()
        {
            // TODO: Remove Password from Secure Storage

            base.OnTerminate();
        }
    }
}
