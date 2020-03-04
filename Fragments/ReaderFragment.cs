using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace ShhhSMS.Fragments
{
    public class ReaderFragment : Fragment
    {
        private string _messageText;

        public ReaderFragment()
        {
            _messageText = "No Message";
        }

        public ReaderFragment(string messageText)
        {
            _messageText = messageText;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = inflater.Inflate(Resource.Layout.reader, container, false);

            var messageText = rootView.FindViewById<TextView>(Resource.Id.messageText);
            messageText.Text = _messageText;

            return rootView;
        }
    }
}