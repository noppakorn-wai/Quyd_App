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

using Quyd.Resources;

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
            UserProfile.usernameBox.Text = ParseUser.CurrentUser.Get<string>("name");
            UserDetail.BoxMail.Text = ParseUser.CurrentUser.Email;
            UserDetail.BoxFacebook.NavigateUri = new Uri(me.link, UriKind.Absolute);
            UserDetail.BoxFacebook.TargetName = "_blank";
            dynamic photo = await fb.GetTaskAsync("me/picture?redirect=false");
            string facebookId = ParseUser.CurrentUser.Get<string>("facebookId");
            UserProfile.profilePictureBox.Source = new BitmapImage(new Uri("http://graph.facebook.com/" + facebookId + "/picture", UriKind.Absolute));

            Init();
        }

        public async void Init()
        {
            Store store = new Store();
            try
            {
                await store.loadAsync(ParseUser.CurrentUser);
            }
            catch (QuydException ex)
            {
                if (ex.Code == QuydException.ErrorCode.store_notFound)
                {
                    store = new Store(ParseUser.CurrentUser.ObjectId, new ParseGeoPoint(-1, 1), new List<string> { "000 000 0000" });
                }
            }

            if (store.Object.ObjectId == null)
            {
                await store.saveAsync();
            }

            reloadUserPage();
            reloadFeedPage(store);
            reloadStorePage(store);
        }

        public async void reloadUserPage()
        {
            PostList posts = new PostList();

            await posts.loadUserPostAsync(ParseUser.CurrentUser);

            if (posts.Size > 0)
            {
                UserPosts.Children.Clear();
            }
            else
            {
                UserLoad.Text = "ไม่มีข้อมูล";
            }

            foreach (var post in posts.posts)
            {
                var controlPost = new Quyd.Controls.ControlPost();
                controlPost.locationBox.Text = "Bangkok , Thailand";
                controlPost.setItems(post);
                controlPost.timeBox.Text = post.CreateAt.ToString();
                controlPost.BtnBid.Visibility = Visibility.Collapsed;
                UserPosts.Children.Add(controlPost);
            }
        }

        public async void reloadFeedPage(Store store)
        {
            FeedList.Children.Clear();

            PostList posts = new PostList();

            await posts.loadFeedAsync(ParseUser.CurrentUser);

            if (posts.Size > 0)
            {
                FeedList.Children.Clear();
            }
            else
            {
                FeedLoad.Text = "ไม่มีข้อมูล";
            }
            int i = 0;
            foreach (var post in posts.posts)
            {
                var controlFeed = new Quyd.Controls.ControlFeed();
                controlFeed.controlUserProfile.usernameBox.Text = post.PostBy.Get<string>("name");
                string facebookId = post.PostBy.Get<string>("facebookId");
                controlFeed.controlUserProfile.profilePictureBox.Source = new BitmapImage(new Uri("http://graph.facebook.com/" + facebookId + "/picture", UriKind.Absolute));
                controlFeed.controlPost.locationBox.Text = "Bangkok , Thailand";
                controlFeed.controlPost.BtnBid.Visibility = Visibility.Visible;
                controlFeed.controlPost.setItems(post);
                controlFeed.controlPost.setValue(post, store);
                controlFeed.controlPost.timeBox.Text = post.CreateAt.ToString();
                controlFeed.controlPost.parent = this;
                controlFeed.controlPost.num = i++;
                FeedList.Children.Add(controlFeed);
            }
        }

        public async void reloadStorePage(Store store)
        {
            ItemList itemList = await store.getStoreItemsAsync();
            if (itemList.Size > 0)
            {
                StackItemDetail.Children.Clear();
            }
            else
            {
                StoreItemsLoad.Text = "ไม่มีข้อมูล";
            }

            foreach (var item in itemList)
            {
                var controlItemDetail = new Quyd.Controls.ControlItemDetail(item);
                controlItemDetail.BoxItemName.Text = item.Name;
                controlItemDetail.BoxInfo.Text = item.Description;
                controlItemDetail.BoxValue.Text = (item as Priceable).Price.ToString();
                controlItemDetail.ImageIcon.Source = new BitmapImage(new Uri("/Resources/Images/" + item.Name + ".jpg", UriKind.Relative));

                StackItemDetail.Children.Add(controlItemDetail);
            }

            PostList posts = new PostList();
            await posts.loadStorePostAsync(store);

            if (posts.Size > 0)
            {
                StackPost.Children.Clear();
            }
            else
            {
                StorePostsLoad.Text = "ไม่มีข้อมูล";
            }

            foreach (var post in posts.posts)
            {
                var controlFeed = new Quyd.Controls.ControlFeed();
                controlFeed.controlUserProfile.usernameBox.Text = post.PostBy.Get<string>("name");
                string facebookId = post.PostBy.Get<string>("facebookId");
                controlFeed.controlUserProfile.profilePictureBox.Source = new BitmapImage(new Uri("http://graph.facebook.com/" + facebookId + "/picture", UriKind.Absolute));
                controlFeed.controlPost.locationBox.Text = "Bangkok , Thailand";
                controlFeed.controlPost.setItems(post);
                controlFeed.controlPost.timeBox.Text = post.CreateAt.ToString();
                controlFeed.controlPost.BtnBid.Visibility = Visibility.Collapsed;
                StackPost.Children.Add(controlFeed);
            }
        }

        public void bidEvent(int num, Post post)
        {
            FeedList.Children.RemoveAt(num);

            var controlFeed = new Quyd.Controls.ControlFeed();
            controlFeed.controlUserProfile.usernameBox.Text = post.PostBy.Get<string>("name");
            string facebookId = post.PostBy.Get<string>("facebookId");
            controlFeed.controlUserProfile.profilePictureBox.Source = new BitmapImage(new Uri("http://graph.facebook.com/" + facebookId + "/picture", UriKind.Absolute));
            controlFeed.controlPost.locationBox.Text = "Bangkok , Thailand";
            controlFeed.controlPost.setItems(post);
            controlFeed.controlPost.timeBox.Text = post.CreateAt.ToString();
            controlFeed.controlPost.BtnBid.Visibility = Visibility.Collapsed;
            StackPost.Children.Add(controlFeed);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PagePost.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            reloadUserPage();
        }
    }
}