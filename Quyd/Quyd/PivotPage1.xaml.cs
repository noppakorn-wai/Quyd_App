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
            UserDetail.BoxMail.Text = me.email;
            UserDetail.BoxFacebook.NavigateUri = new Uri(me.link, UriKind.Absolute);
            UserDetail.BoxFacebook.TargetName = "_blank";
            dynamic photo = await fb.GetTaskAsync("me/picture?redirect=false");
            string facebookId = ParseUser.CurrentUser.Get<string>("facebookId");
            UserProfile.profilePictureBox.Source = new BitmapImage(new Uri("http://graph.facebook.com/" + facebookId + "/picture", UriKind.Absolute));

            await reloadAll();
        }

        public async Task reloadAll()
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
            else
            {
                UserLoad.Text = "ไม่มีข้อมูล";
            }

            foreach (var post in posts.posts)
            {
                var controlPost = new Quyd.Controls.ControlPost();
                controlPost.locationBox.Text = "Bangkok , Thailand";
                controlPost.setItems(await post.getUserItem());
                controlPost.timeBox.Text = post.CreateAt.ToString();
                controlPost.BtnBid.Visibility = (await post.isBidable(ParseUser.CurrentUser))?Visibility.Visible:Visibility.Collapsed;
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
            else
            {
                FeedLoad.Text = "ไม่มีข้อมูล";
            }

            foreach (var post in posts.posts)
            {
                var controlFeed = new Quyd.Controls.ControlFeed();
                controlFeed.controlUserProfile.usernameBox.Text = post.PostBy.Get<string>("name");
                string facebookId = post.PostBy.Get<string>("facebookId");
                controlFeed.controlUserProfile.profilePictureBox.Source = new BitmapImage(new Uri("http://graph.facebook.com/" + facebookId + "/picture", UriKind.Absolute));
                controlFeed.controlPost.locationBox.Text += " (" + post.Location.Latitude + "," + post.Location.Longitude + ")";
                controlFeed.controlPost.BtnBid.Visibility = (await post.isBidable(ParseUser.CurrentUser)) ? Visibility.Visible : Visibility.Collapsed;
                controlFeed.controlPost.setItems(await post.getUserItem());
                controlFeed.controlPost.timeBox.Text = post.CreateAt.ToString();
                FeedList.Children.Add(controlFeed);
            }
        }

        public async Task reloadStorePage()
        {
            Store store = new Store();
            try
            {
                await store.loadAsync(ParseUser.CurrentUser);
            }
            catch(QuydException ex)
            {
                if(ex.Code == QuydException.ErrorCode.store_notFound)
                {
                    store = new Store(ParseUser.CurrentUser.ObjectId, new ParseGeoPoint(-1, 1), new List<string> { "000 000 0000" });
                }
            }

            if(store.Object.ObjectId == null)
            {
                await store.saveAsync();
            }

            if ((await store.getStoreItemsAsync()).Size > 0)
            {
                StackItemDetail.Children.Clear();
            }
            else
            {
                StoreItemsLoad.Text = "ไม่มีข้อมูล";
            }

            foreach (var item in (await store.getStoreItemsAsync()))
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
                controlFeed.controlPost.locationBox.Text += " (" + post.Location.Latitude + "," + post.Location.Longitude + ")";
                controlFeed.controlPost.setItems(await post.getUserItem());
                controlFeed.controlPost.timeBox.Text = post.CreateAt.ToString();
                controlFeed.controlPost.BtnBid.Visibility = (await post.isBidable(ParseUser.CurrentUser)) ? Visibility.Visible : Visibility.Collapsed;
                StackPost.Children.Add(controlFeed);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PagePost.xaml", UriKind.Relative));
        }

        protected void Page_InitComplete(object sender, EventArgs e)
        {
            reloadAll();
        }        
    }
}