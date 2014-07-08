using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Quyd.Model;

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

        public async void loadComponentAsync()
        {
            //Notification Section
           /* NotificationSet notifications = new NotificationSet();
            await notifications.loadUnread();
            if (notifications.size() > 0)
            {
                notificationBox.Text = notifications.get(0).ToString();
            }
            else
            {
                notificationBox.Text = "No new notification";
            }
            */
            //User section
            //var username = ParseUser.CurrentUser.Get<string>("name");
            //usernameBox.Text = username;//.Split('.')[0];
            //+ParseUser.CurrentUser.Get<string>("profilePicture")
            //Uri uri = new Uri("http://graph.facebook.com/" + username + "/picture", UriKind.Absolute);
            //profilePicture.Source = new System.Windows.Media.Imaging.BitmapImage(uri);
        }

    }
}