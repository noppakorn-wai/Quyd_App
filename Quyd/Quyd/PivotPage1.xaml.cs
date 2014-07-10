using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Parse;
using Quyd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

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
            var fb = new Facebook.FacebookClient();
            fb.AccessToken = ParseFacebookUtils.AccessToken;
            dynamic me = await fb.GetTaskAsync("me");
            UserProfile.usernameBox.Text = ParseUser.CurrentUser.Get<string>("name");
            UserDetail.BoxMail.Text = me.email;
            UserDetail.BoxFacebook.NavigateUri = new Uri(me.link, UriKind.Absolute);
            dynamic photo = await fb.GetTaskAsync("me/picture?redirect=false");
            string facebookId = ParseUser.CurrentUser.Get<string>("facebookId");
            UserProfile.profilePictureBox.Source = new BitmapImage(new Uri("http://graph.facebook.com/" + facebookId + "/picture", UriKind.Absolute));

            reloadAll();
        }

        public async void reloadAll()
        {
            await reloadUserPage();
            await reloadFeedPage();
            await reloadStorePage();
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
                string facebookId = post.PostBy.Get<string>("facebookId");
                controlFeed.controlUserProfile.profilePictureBox.Source = new BitmapImage(new Uri("http://graph.facebook.com/" + facebookId + "/picture", UriKind.Absolute));
                controlFeed.controlPost.setLocation(post.PostBy.Get<string>("name"));
                controlFeed.controlPost.setItems(post.postItems);
                FeedList.Children.Add(controlFeed);
            }
        }

        public async Task reloadStorePage()
        {
            Store store = new Store();
            await store.loadAsync(ParseUser.CurrentUser);

            if (store.storeItems.Size > 0)
            {
                StackItemDetail.Children.Clear();
            }

            foreach (var item in store.storeItems)
            {
                var controlItemDetail = new Quyd.Controls.ControlItemDetail();
                controlItemDetail.BoxItemName.Text = item.Name;
                controlItemDetail.BoxInfo.Text = item.Description;
                StackItemDetail.Children.Add(controlItemDetail);
            }

            PostList posts = new PostList();
            await posts.loadStorePostAsync(store);

            if (posts.Size > 0)
            {
                StackPost.Children.Clear();
            }

            foreach (var post in posts.posts)
            {
                var controlFeed = new Quyd.Controls.ControlFeed();
                controlFeed.controlUserProfile.usernameBox.Text = post.PostBy.Get<string>("name");
                string facebookId = post.PostBy.Get<string>("facebookId");
                controlFeed.controlUserProfile.profilePictureBox.Source = new BitmapImage(new Uri("http://graph.facebook.com/" + facebookId + "/picture", UriKind.Absolute));
                controlFeed.controlPost.setLocation(post.PostBy.Get<string>("name"));
                controlFeed.controlPost.setItems(post.postItems);
                StackPost.Children.Add(controlFeed);
            }
        }
    }
}