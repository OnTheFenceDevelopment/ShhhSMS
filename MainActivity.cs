using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using Google.Android.Material.FloatingActionButton;

using Google.Android.Material.Navigation;
using ShhhSMS.Fragments;
using ShhhSMS.Services;
using System;
using SupportFragment = AndroidX.Fragment.App.Fragment;

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

            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
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

            var incomingSMSContent = string.Empty;

            // Check for 'incoming' SMS message
            if (Intent.Action == Android.Content.Intent.ActionSend && Intent.Type == "text/plain")
            {
                incomingSMSContent = Intent.GetStringExtra(Android.Content.Intent.ExtraText);
            }

            encryptionService = new EncryptionService();
            if (await encryptionService.PasswordExists() == false)
            {
                PerformFragmentNavigation(new LoginFragment(), "Login");
                fab.Visibility = ViewStates.Invisible;
            }
            else if (string.IsNullOrEmpty(incomingSMSContent))
            {
                var readerFragment = new ReaderFragment(incomingSMSContent);
                PerformFragmentNavigation(readerFragment, "Reader");
            }
        }

        public override void OnBackPressed()
        {
            var drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
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
            else if (id == Resource.Id.nav_key_maintenance)
            {
                PerformFragmentNavigation(new KeyMaintenanceFragment(), "Key Maintenance");
                fab.Visibility = ViewStates.Invisible;
            }
            else if (id == Resource.Id.nav_contact_management)
            {
                PerformFragmentNavigation(new ContactMaintenanceFragment(), "Contacts");
                fab.Visibility = ViewStates.Invisible;
            }
            else if (id == Resource.Id.nav_logout)
            {
                if (encryptionService == null)
                    encryptionService = new EncryptionService();

                encryptionService.ClearPassword();

                Finish();
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

            menuTransaction.AddToBackStack(null);

            menuTransaction.Commit();
        }
    }
}

