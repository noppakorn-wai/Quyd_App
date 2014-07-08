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
            generatePost();
        }

        public void generatePost()
        {
            PostList posts = new PostList();

            if (posts.size() > 0)
            {
                UserPosts.Children.Clear();
            }

            foreach(var post in posts.posts)
            {
                var controlPost = new Quyd.Controls.ControlPost();
                controlPost.Margin = new System.Windows.Thickness(5,5,5,0);
                controlPost.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                controlPost.Height = 125;
                controlPost.Width = 480;
                controlPost.setLocation("Test1");
                UserPosts.Children.Add(controlPost);
            }
        }
    }
}