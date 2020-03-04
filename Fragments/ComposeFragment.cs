using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ShhhSMS.Fragments
{
    public class ComposeFragment : Fragment
    {
        private Button _composeSend;
        private Button _composeCancel;
        private EditText _messageText;

        public event EventHandler OnCancel;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        private async void ComposeSend_Click(object sender, System.EventArgs e)
        {
            // TODO: Recipient Number Selection
            await SendSms(_messageText.Text, "07791652684");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var rootView = inflater.Inflate(Resource.Layout.compose, container, false);

            _composeSend = rootView.FindViewById<Button>(Resource.Id.composeSend);
            _composeSend.Click += ComposeSend_Click;

            _composeCancel = rootView.FindViewById<Button>(Resource.Id.composeCancel);
            _composeCancel.Click += ComposeCancel_Click;

            _messageText = rootView.FindViewById<EditText>(Resource.Id.composeText);
            
            return rootView;
        }

        private void ComposeCancel_Click(object sender, EventArgs e)
        {
            // TODO: Need to Replace Self with Welcome Fragment
            OnCancel?.Invoke(this, EventArgs.Empty);
        }

        public async Task SendSms(string messageText, string recipient)
        {
            try
            {
                var message = new SmsMessage(messageText, new[] { recipient });
                await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                var dsc = 1;
                // Sms is not supported on this device.
            }
            catch (Exception ex)
            {
                var foo = 1;
                // Other error has occurred.
            }
        }
    }
}