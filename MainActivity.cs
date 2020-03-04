using System;
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

namespace ShhhSMS
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        FloatingActionButton fab;

        protected override void OnCreate(Bundle savedInstanceState)
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

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            var welcomeTransaction = SupportFragmentManager.BeginTransaction();
            welcomeTransaction.Add(Resource.Id.fragment_container, new WelcomeFragment(), "Welcome");
            welcomeTransaction.Commit();
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
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
            var menuTransaction = SupportFragmentManager.BeginTransaction();
            menuTransaction.Replace(Resource.Id.fragment_container, new ComposeFragment(), "Compose");
            menuTransaction.Commit();

            fab.Visibility = ViewStates.Invisible;
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_home)
            {
                var welcomeTransaction = SupportFragmentManager.BeginTransaction();
                welcomeTransaction.Replace(Resource.Id.fragment_container, new WelcomeFragment(), "Welcome");
                welcomeTransaction.Commit();

                fab.Visibility = ViewStates.Visible;
            }
            else if (id == Resource.Id.nav_compose)
            {
                var menuTransaction = SupportFragmentManager.BeginTransaction();

                var composeFragment = new ComposeFragment();
                composeFragment.OnCancel += ComposeFragment_OnCancel;

                menuTransaction.Replace(Resource.Id.fragment_container, composeFragment, "Compose");
                menuTransaction.Commit();

                fab.Visibility = ViewStates.Invisible;
            }
            else if (id == Resource.Id.nav_reader)
            {
                var menuTransaction = SupportFragmentManager.BeginTransaction();
                menuTransaction.Replace(Resource.Id.fragment_container, new ReaderFragment(), "Reader");
                menuTransaction.Commit();

                fab.Visibility = ViewStates.Visible;
            }
            else if (id == Resource.Id.nav_help)
            {
                var menuTransaction = SupportFragmentManager.BeginTransaction();
                menuTransaction.Replace(Resource.Id.fragment_container, new HelpFragment(), "Help");
                menuTransaction.Commit();

                fab.Visibility = ViewStates.Visible;
            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

        private void ComposeFragment_OnCancel(object sender, EventArgs e)
        {
            // TODO: Need to Set the Selected Menu Item
            var welcomeTransaction = SupportFragmentManager.BeginTransaction();
            welcomeTransaction.Replace(Resource.Id.fragment_container, new WelcomeFragment(), "Welcome");
            welcomeTransaction.Commit();

            fab.Visibility = ViewStates.Visible;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

