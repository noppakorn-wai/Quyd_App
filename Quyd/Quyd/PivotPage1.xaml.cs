using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Quyd.Models;
using System.Threading.Tasks;

using Parse;
using System.Windows.Media.Imaging;

namespace Quyd
{
    public partial class PivotPage1 : PhoneApplicationPage
    {
        public PivotPage1()
        {
            InitializeComponent();
            loadComponentAsync();
            NavigationInTransition navigateInTransition = new NavigationInTransition();
            navigateInTransition.Backward = new SlideTransition { Mode = SlideTransitionMode.SlideLeftFadeIn };
            navigateInTransition.Forward = new SlideTransition { Mode = SlideTransitionMode.SlideRightFadeIn };

            NavigationOutTransition navigateOutTransition = new NavigationOutTransition();
            navigateOutTransition.Backward = new SlideTransition { Mode = SlideTransitionMode.SlideLeftFadeOut };
            navigateOutTransition.Forward = new SlideTransition { Mode = SlideTransitionMode.SlideRightFadeOut };
            TransitionService.SetNavigationInTransition(this, navigateInTransition);
            TransitionService.SetNavigationOutTransition(this, navigateOutTransition);
        }

        public async void loadComponentAsync()
        {
            var fb = new Facebook.FacebookClient();
            fb.AccessToken = ParseFacebookUtils.AccessToken;
            dynamic me = await fb.GetTaskAsync("me");
            UserProfile.usernameBox.Text = me.name;
            UserDetail.BoxMail.Text = me.email;
            UserDetail.BoxFacebook.NavigateUri = new Uri(me.link, UriKind.Absolute);
            dynamic photo = await fb.GetTaskAsync("me/picture?redirect=false");
            Uri uri = new Uri(photo.data.url, UriKind.Absolute);
            UserProfile.profilePictureBox.Source = new BitmapImage(uri);
            reloadAll();
        }
        public async void reloadAll()
        {
            await reloadUserPage();
            await reloadFeedPage();
        }

        public async Task reloadUserPage()
        {
            var fb = new Facebook.FacebookClient();
            fb.AccessToken = ParseFacebookUtils.AccessToken;
            var me = await fb.GetTaskAsync("me");

            var fbData = new Facebook.Client.GraphUser(me);

            PostList posts = new PostList();

            await posts.loadUserPostAsync(ParseUser.CurrentUser);

            if (posts.Size > 0)
            {
                UserPosts.Children.Clear();
            }

            foreach (var post in posts.posts)
            {
                var controlPost = new Quyd.Controls.ControlPost();
                controlPost.setLocation(post.Description);
                controlPost.setItems(post.postItems);
                UserPosts.Children.Add(controlPost);
            }
        }

        public async Task reloadFeedPage()
        {
            PostList posts = new PostList();


            await posts.loadFeedAsync(ParseUser.CurrentUser);

            if (posts.Size > 0)
            {
                FeedList.Children.Clear();
            }

            foreach (var post in posts.posts)
            {
                var controlFeed = new Quyd.Controls.ControlFeed();
                controlFeed.controlUserProfile.usernameBox.Text = post.PostBy.Get<string>("name");
                controlFeed.controlPost.setLocation(post.Description);
                controlFeed.controlPost.setItems(post.postItems);
                FeedList.Children.Add(controlFeed);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PagePost.xaml", UriKind.Relative));
        }
    }
}