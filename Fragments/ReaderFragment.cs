using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using ShhhSMS.Models;
using ShhhSMS.Services;
using System;
using System.Linq;

namespace ShhhSMS.Fragments
{
    public class ReaderFragment : Fragment
    {
        private string _messageText;
        private DecryptionPackage decryptionPackage;

        // TODO: Replace with IOC
        IEncryptionService encryptionService;
        IContactService contactService;

        public ReaderFragment()
        {
            encryptionService = new EncryptionService();

            _messageText = "No Message";
        }

        public ReaderFragment(string messageText)
        {
            encryptionService = new EncryptionService();
            _messageText = messageText;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var packetElements = _messageText.Split("|");
            contactService = new ContactService();
            var contacts = contactService.GetContacts();
            var sender = contacts.SingleOrDefault(x => x.Id == packetElements[0]);

            if (sender == null)
            {
                // TODO: If sender is null then user could be re-opening a message they sent - check contactId
            }
            else
            {
                var nonce = Convert.FromBase64String(packetElements[1]);
                var encryptedMessage = Convert.FromBase64String(packetElements[2]);
                var senderPublicKey = Convert.FromBase64String(sender.PublicKey);
                decryptionPackage = new DecryptionPackage(encryptedMessage, nonce, senderPublicKey);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = inflater.Inflate(Resource.Layout.reader, container, false);

            var messageText = rootView.FindViewById<TextView>(Resource.Id.messageText);

            if (decryptionPackage != null)
            {
                // Don't like this (get awaiter)
                messageText.Text = encryptionService.DecryptMessage(decryptionPackage).GetAwaiter().GetResult();
            }
            else
            {
                Toast.MakeText(Activity, "Unable to resolve message sender", ToastLength.Long).Show();
            }

            return rootView;
        }
    }
}