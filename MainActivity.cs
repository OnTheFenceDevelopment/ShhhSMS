using System;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using ShhhSMS.Fragments;
using ShhhSMS.Services;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace ShhhSMS
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    [IntentFilter(new[] { Android.Content.Intent.ActionSend }, Categories = new[] { Android.Content.Intent.CategoryDefault }, DataMimeType = @"text/plain")]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        FloatingActionButton fab;
        NavigationView navigationView;

        // TODO: Replace with IOC
        IEncryptionService encryptionService;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            navigationView.Menu.GetItem(0).SetChecked(true);

            var welcomeTransaction = SupportFragmentManager.BeginTransaction();
            welcomeTransaction.Add(Resource.Id.fragment_container, new WelcomeFragment(), "Welcome");
            welcomeTransaction.Commit();

            // Check for 'incoming' SMS message
            if (Intent.Action == Android.Content.Intent.ActionSend && Intent.Type == "text/plain")
            {
                var readerFragment = new ReaderFragment(Intent.GetStringExtra(Android.Content.Intent.ExtraText));
                PerformFragmentNavigation(readerFragment, "Reader");
            }

            encryptionService = new EncryptionService();
            if (await encryptionService.PasswordExists() == false)
            {
                PerformFragmentNavigation(new LoginFragment(), "Login");
                fab.Visibility = ViewStates.Invisible;
            }
        }

        public override void OnBackPressed()
        {
            var drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            var composeFragment = new ComposeFragment();
            composeFragment.OnCancel += ComposeFragment_OnCancel;

            PerformFragmentNavigation(composeFragment, "Compose");
            fab.Visibility = ViewStates.Invisible;
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_home)
            {
                PerformFragmentNavigation(new WelcomeFragment(), "Welcome");
                fab.Visibility = ViewStates.Visible;
            }
            else if (id == Resource.Id.nav_compose)
            {
                var composeFragment = new ComposeFragment();
                composeFragment.OnCancel += ComposeFragment_OnCancel;

                PerformFragmentNavigation(composeFragment, "Compose");
                fab.Visibility = ViewStates.Invisible;
            }
            else if (id == Resource.Id.nav_reader)
            {
                PerformFragmentNavigation(new ReaderFragment(), "Reader");
                fab.Visibility = ViewStates.Visible;
            }
            else if (id == Resource.Id.nav_help)
            {
                PerformFragmentNavigation(new HelpFragment(), "Help");
                fab.Visibility = ViewStates.Visible;
            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

        private void ComposeFragment_OnCancel(object sender, EventArgs e)
        {
            navigationView.Menu.GetItem(0).SetChecked(true);
            PerformFragmentNavigation(new WelcomeFragment(), "Welcome");
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void PerformFragmentNavigation(SupportFragment fragment, string fragmentTag)
        {
            var menuTransaction = SupportFragmentManager.BeginTransaction();

            menuTransaction.SetCustomAnimations(Resource.Animation.abc_slide_in_top, Resource.Animation.abc_fade_out);

            menuTransaction.Replace(Resource.Id.fragment_container, fragment, fragmentTag);
            menuTransaction.Commit();
        }
    }
}

