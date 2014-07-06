using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Quyd.Resources;

using Parse;
using Quyd.Model;

namespace Quyd
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

            login();
        }

        private void login()
        {
            try
            {
               
                //ParseUser user = await ParseFacebookUtils.LogInAsync(browser, null);
                //await ParseUser.LogInAsync("wai", "wai");
                // Login was successful.
                //status.Text += "login complete - User : " + ParseUser.CurrentUser.Username + "\n";
            }
            catch (ParseException ex)
            {
               
            }
            catch (Exception ex)
            {
                
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                IList<string> phones = new List<string>();
                phones.Add("12345678");
                phones.Add("12345679");
                ParseGeoPoint location = new ParseGeoPoint(-41, -10);
                Store new_store = new Store("testingStore", location, phones);
                await new_store.save();
            }
            catch (QuydException ex)
            {
                
            }
            catch (ParseException ex)
            {
                
            }
        }

        private void OnSessionStateChanged(object sender, Facebook.Client.Controls.SessionStateChangedEventArgs e)
        {

        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}