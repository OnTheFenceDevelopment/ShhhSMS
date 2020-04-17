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

            public static int Scan_Public_Key_Request_Code = 1000;
            public static int Successful_Public_Key_Scan = 1001;
            public static int Unsuccessful_Public_Key_Scan = 1002;
            public static int Add_Contact_Request_Code = 1003;

    }
}