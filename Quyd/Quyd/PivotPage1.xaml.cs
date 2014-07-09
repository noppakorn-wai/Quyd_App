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
        }

        public async void loadComponentAsync()
        {
            //var fb = new Facebook.FacebookClient();
            //fb.AccessToken = ParseFacebookUtils.AccessToken;
            //var me = await fb.GetTaskAsync("me");

            //var fbData = new Facebook.Client.GraphUser(me);
            //UserProfile.usernameBox.Text = fbData.Name;
            var fb = new Facebook.FacebookClient();
            fb.AccessToken = ParseFacebookUtils.AccessToken;
            dynamic me = await fb.GetTaskAsync("me");
            UserProfile.usernameBox.Text = me.id;
            UserDetail.BoxMail.Text = me.email;
            dynamic photo = await fb.GetTaskAsync("me/picture?redirect=false");
            Uri uri = new Uri(photo.data.url, UriKind.Absolute);
            UserProfile.profilePictureBox.Source = new BitmapImage(uri);

            generatePost();
        }
        public async void generatePost()
        {
            var fb = new Facebook.FacebookClient();
            fb.AccessToken = ParseFacebookUtils.AccessToken;
            var me = await fb.GetTaskAsync("me");

            var fbData = new Facebook.Client.GraphUser(me);

            PostList posts = new PostList();

            await posts.loadAsync(ParseUser.CurrentUser);

            if (posts.Size > 0)
            {
                UserPosts.Children.Clear();
            }

            foreach(var post in posts.posts)
            {
                var controlPost = new Quyd.Controls.ControlPost();
                controlPost.setLocation("Test1");
                UserPosts.Children.Add(controlPost);
            }
        }
    }
}