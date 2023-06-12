using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using AlertDialog = Android.App.AlertDialog;
using System;
using ShhhSMS.Services;
using ShhhSMS.Models;

namespace ShhhSMS.Fragments
{
    public class AddContactDialogFragment : DialogFragment
    {
        private Guid _contactId;
        private string _publicKey;

        private Button _cancel;
        private Button _save;

        private EditText _newContactName;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = inflater.Inflate(Resource.Layout.add_contact_dialog, container, false);

            _contactId = new Guid(Arguments.GetString("contactId"));
            _publicKey = Arguments.GetString("publicKey");

            _newContactName = rootView!.FindViewById<EditText>(Resource.Id.newContactName);
            _newContactName!.TextChanged += NewContactName_TextChanged;

            _cancel = rootView.FindViewById<Button>(Resource.Id.newContactCancel);
            _cancel!.Click += Cancel_Click;

            _save = rootView.FindViewById<Button>(Resource.Id.newContactSave);
            _save!.Click += Save_Click;

            return rootView;
        }

        private void NewContactName_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            _save.Enabled = string.IsNullOrWhiteSpace(_newContactName.Text) == false;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            // Read Existing Contact 'store'
            var contactService = new ContactService();
            var contact = new Contact { Id = _contactId.ToString(), Name = _newContactName.Text, PublicKey = _publicKey };

            var success = contactService.SaveContact(contact);

            if (success)
                Toast.MakeText(Activity, "Save Successful", ToastLength.Long)!.Show();
            else
                Toast.MakeText(Activity, "Save Failed..!!", ToastLength.Long)!.Show();

            Dismiss();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            var builder = new AlertDialog.Builder(Activity);
            builder.SetTitle("Are you sure?");
            builder.SetMessage("Cancel adding this Contact?");
            builder.SetPositiveButton("Yes", (s, evt) =>
            {
                Dismiss();
            });
            builder.SetNegativeButton("No", (s, evt) =>
            {
                builder.Dispose();
                return;
            });

            var dialog = builder.Create();
            dialog!.Show();
        }
    }
}