using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;

namespace ShhhSMS.Fragments
{
    public class WelcomeFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.welcome, container, false);
        }
    }
}