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

namespace Quyd
{
    public partial class PivotPage1 : PhoneApplicationPage
    {
        public PivotPage1()
        {
            InitializeComponent();
            loadComponentAsync();
        }

        public void loadComponentAsync()
        {
            UserProfile.usernameBox.Text = ParseUser.CurrentUser.Get<string>("name");
            FeedsProfile.usernameBox.Text = ParseUser.CurrentUser.Get<string>("name");
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