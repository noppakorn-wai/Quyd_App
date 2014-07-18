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
using Quyd.Models;
using Facebook;

namespace Quyd
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            NavigationInTransition navigateInTransition = new NavigationInTransition();
            navigateInTransition.Backward = new SlideTransition { Mode = SlideTransitionMode.SlideLeftFadeIn };
            navigateInTransition.Forward = new SlideTransition { Mode = SlideTransitionMode.SlideRightFadeIn };

            NavigationOutTransition navigateOutTransition = new NavigationOutTransition();
            navigateOutTransition.Backward = new SlideTransition { Mode = SlideTransitionMode.SlideLeftFadeOut };
            navigateOutTransition.Forward = new SlideTransition { Mode = SlideTransitionMode.SlideRightFadeOut };
            TransitionService.SetNavigationInTransition(this, navigateInTransition);
            TransitionService.SetNavigationOutTransition(this, navigateOutTransition);
        }

        private async void loginFace_Click(object sender, RoutedEventArgs e)
        {
            loginGrid.Visibility = Visibility.Collapsed;
            browserGrid.Visibility = Visibility.Visible;
            try
            {
                var permissionRequests = new[] { "email", "public_profile", "user_friends" };
                ParseUser user = await ParseFacebookUtils.LogInAsync(browser, permissionRequests);

                loginGrid.Visibility = Visibility.Visible;
                browserGrid.Visibility = Visibility.Collapsed;
                loginFace.Visibility = Visibility.Collapsed;

                var fb = new Facebook.FacebookClient();
                fb.AccessToken = ParseFacebookUtils.AccessToken;
                var me = await fb.GetTaskAsync("me");

                var fbData = new Facebook.Client.GraphUser(me);

                user = ParseUser.CurrentUser;

                user["name"] = fbData.Name;
                user["facebookId"] = fbData.Id;

                dynamic parameters = new System.Dynamic.ExpandoObject();
                parameters.access_token = ParseFacebookUtils.AccessToken;
                parameters.fields = "email";
                dynamic result = await fb.GetTaskAsync("me", parameters);
                user["email"] = result.email;

                await user.SaveAsync();
                
                NavigationService.Navigate(new Uri("/PivotPage1.xaml", UriKind.Relative));
                NavigationService.RemoveBackEntry();
            }
            catch(Exception)
            {
                loginGrid.Visibility = Visibility.Visible;
                browserGrid.Visibility = Visibility.Collapsed;
                loginFace.Visibility = Visibility.Visible;
            }
        }

        private void browser_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            loginGrid.Visibility = Visibility.Visible;
            browserGrid.Visibility = Visibility.Collapsed;
            loginFace.Visibility = Visibility.Visible;
        }
    }
}