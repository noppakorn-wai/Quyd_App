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
    public partial class PagePost : PhoneApplicationPage
    {
        //Post post;

        public PagePost()
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

            //loadData();
        }

        /*public async void loadData()
        {
            post = new Post(new ParseGeoPoint(-1, 1), ParseUser.CurrentUser);
            
            await post.getUserItem();

            StackItem.Children.Clear();

            foreach (var item in (await post.getUserItem()))
            {
                var controlItemDetail = new Quyd.Controls.ControlItemDetail();
                controlItemDetail.BoxItemName.Text = item.Name;
                controlItemDetail.BoxInfo.Text = item.Description;
                controlItemDetail.BoxValue.Text = (item as Quantifiable).Quantity.ToString();
                controlItemDetail.ImageIcon.Source = new BitmapImage(new Uri(item.Icon, UriKind.Absolute));
                StackItem.Children.Add(controlItemDetail);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await post.saveAsync();
            int i = 0;
            ItemList itemList = await post.getUserItem();
            foreach (var item in (itemList))
            {
                int q = Convert.ToInt32((StackItem.Children.ElementAt<UIElement>(i) as Quyd.Controls.ControlItemDetail).BoxValue.Text);
                if (q > 0)
                {
                    (item as PostItem).Quantity = q;
                    i++;
                    (item as PostItem).Post = post;
                    await (item as PostItem).saveAsync();
                }
            }
            
            NavigationService.GoBack();
        }*/
    }
}