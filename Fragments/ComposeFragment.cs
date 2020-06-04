using Android.OS;

using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using ShhhSMS.Models;
using ShhhSMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ShhhSMS.Fragments
{
    public class ComposeFragment : Fragment
    {
        private Button _composeSend;
        private Button _composeCancel;
        private EditText _messageText;
        private Spinner _contactSelector;

        private Contact _selectedContact;

        public event EventHandler OnCancel;

        // TODO: Replace with IOC
        IEncryptionService encryptionService;
        IContactService contactService;

        private List<Contact> _contacts;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            encryptionService = new EncryptionService();
            contactService = new ContactService();
            _contacts = contactService.GetContacts();
            _contacts.Insert(0, new Contact { Id = Guid.Empty.ToString(), Name = "Select Recipient", PublicKey = string.Empty });
        }

        private async void ComposeSend_Click(object sender, System.EventArgs e)
        {
            await SendSms(_messageText.Text);
            _messageText.Text = string.Empty;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var rootView = inflater.Inflate(Resource.Layout.compose, container, false);

            _composeSend = rootView.FindViewById<Button>(Resource.Id.composeSend);
            _composeSend.Click += ComposeSend_Click;
            _composeSend.Enabled = false;

            _composeCancel = rootView.FindViewById<Button>(Resource.Id.composeCancel);
            _composeCancel.Click += ComposeCancel_Click;

            _messageText = rootView.FindViewById<EditText>(Resource.Id.composeText);
            _messageText.AfterTextChanged += MessageText_AfterTextChanged;
            _messageText.RequestFocus();

            _contactSelector = rootView.FindViewById<Spinner>(Resource.Id.composeContactSelector);
            _contactSelector.ItemSelected += ContactSelector_ItemSelected;
            _contactSelector.Adapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleSpinnerDropDownItem, _contacts.Select(x => x.Name).ToList());

            return rootView;
        }

        private void ContactSelector_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            // Resolve Contact Record from list using selected Name
            _selectedContact = _contacts.SingleOrDefault(x => x.Name == _contactSelector.GetItemAtPosition(e.Position).ToString());

            SetSendButtonState();
        }

        private void MessageText_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            SetSendButtonState();
        }

        private void SetSendButtonState()
        {
            if (string.IsNullOrWhiteSpace(_messageText.Text) || (_selectedContact != null && _selectedContact.Id == Guid.Empty.ToString()))
            {
                _composeSend.Enabled = false;
            }
            else
            {
                _composeSend.Enabled = true;
            }
        }

        private void ComposeCancel_Click(object sender, EventArgs e)
        {
            OnCancel?.Invoke(this, EventArgs.Empty);
        }

        public async Task SendSms(string messageText)
        {
            try
            {
                // Need to Encrypt the message using recipients Public Key and pass the result to Sms
                var encryptedMessage = await encryptionService.EncryptMessage(messageText, _selectedContact.PublicKey);

                var message = new SmsMessage { Body = encryptedMessage.ToString() };

                await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException)
            {
                // TODO: Sms is not supported on this device. Display Error
            }
            catch (Exception)
            {
                // TODO: Other error has occurred. Display Error
            }
        }
    }
}