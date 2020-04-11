using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ShhhSMS
{
    public static class Constants
    {
        public static class Identifiers
        {
            public static string PublicKey => "public_key";
            public static string DeviceId => "deviceId";
            public static string ContactId => "contactId";
            public static string UserPassword => "user_pwd";
        }
    }
}