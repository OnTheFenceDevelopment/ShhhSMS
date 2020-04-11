using System;
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AlertDialog = Android.App.AlertDialog;
using ShhhSMS.Services;

namespace ShhhSMS.Fragments
{
    public class LoginFragment : Fragment
    {
        private Button loginCancel;
        private Button loginOK;
        private EditText userPassword;
        private TextView loginMessage;

        private string publicKey;
        private string deviceId;

        private string loginMessageText;

        // TODO: Replace with IOC
        EncryptionService encryptionService;

        public async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            encryptionService = new EncryptionService();

            if (await encryptionService.PublicKeyExists())
            {
                loginMessageText = "Public Key Does Not Exist";
            }
            else
            {
                loginMessageText = "Public Key Exists";
            }

            publicKey = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.Identifiers.PublicKey);
            deviceId = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.Identifiers.DeviceId);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = inflater.Inflate(Resource.Layout.generate_keys, container, false);

            loginCancel = rootView.FindViewById<Button>(Resource.Id.login_cancel);
            loginCancel.Click += LoginCancel_Click;

            loginOK = rootView.FindViewById<Button>(Resource.Id.login_ok);
            loginOK.Click += LoginOK_Click;
            loginOK.Enabled = false;

            userPassword = rootView.FindViewById<EditText>(Resource.Id.userPassword);
            userPassword.AfterTextChanged += UserPassword_AfterTextChanged;
            userPassword.RequestFocus();

            // TODO: Set message based on Key status (different for first and subsequent logins)
            loginMessage = rootView.FindViewById<TextView>(Resource.Id.loginMessage);
            loginMessage.Text = loginMessageText;

            return rootView;
        }

        private void UserPassword_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            loginOK.Enabled = !string.IsNullOrWhiteSpace(userPassword.Text);
        }

        private void NavigateToWelcome()
        {
            // TODO: Also need to Show the Floating Compose Button
            var welcomeTransaction = Activity.SupportFragmentManager.BeginTransaction();
            welcomeTransaction.SetCustomAnimations(Resource.Animation.abc_slide_in_top, Resource.Animation.abc_fade_out);
            welcomeTransaction.Replace(Resource.Id.fragment_container, new WelcomeFragment(), "Welcome");
            welcomeTransaction.Commit();
        }

        private async void LoginOK_Click(object sender, EventArgs e)
        {
            var passwordBytes = Sodium.GenericHash.Hash(userPassword.Text, deviceId, 32);
            var keyPair = Sodium.PublicKeyBox.GenerateKeyPair(passwordBytes);

            if (string.IsNullOrWhiteSpace(publicKey))
            {
                // No Public Key Present - Store 
                await encryptionService.SetPublicKey(Convert.ToBase64String(keyPair.PublicKey));
                await encryptionService.SetPassword(userPassword.Text);

                Toast.MakeText(Activity, "Encryption Keys Generated", ToastLength.Long).Show();
                NavigateToWelcome();
            }
            else
            {
                // Public Key Present - Generate and Check
                var generatedPublicKeyBase64 = Convert.ToBase64String(keyPair.PublicKey);

                if (publicKey.Equals(generatedPublicKeyBase64))
                {
                    Toast.MakeText(Activity, "Login Successful", ToastLength.Long).Show();
                    NavigateToWelcome();
                }
                else
                {
                    // TODO: Display Failure Alert
                    var builder = new AlertDialog.Builder(Activity);
                    builder.SetTitle("Login Failure");
                    builder.SetMessage("Incorrect password/passphrase - please try again.");
                    builder.SetPositiveButton("OK", (s, e) =>
                    {
                        builder.Dispose();
                    });

                    var dialog = builder.Create();
                    dialog.Show();
                }
            }
        }

        private void LoginCancel_Click(object sender, EventArgs e)
        {
            var builder = new AlertDialog.Builder(Activity);
            builder.SetTitle("Are you sure?");
            builder.SetMessage("Are you sure you want to exit?");
            builder.SetPositiveButton("Yes", (s, e) =>
            {
                Activity.Finish();
            });
            builder.SetNegativeButton("No", (s, e) =>
            {
                builder.Dispose();
                return;
            });

            var dialog = builder.Create();
            dialog.Show();
        }
    }
}
