using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using Quyd.Models;

using Parse;

namespace Quyd.Controls
{
    public partial class ControlPost : UserControl
    {
        public ControlPost()
        {
            InitializeComponent();
        }

        private async void StackItem_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Post post = this.DataContext as Post;
                IEnumerable<PostItem> items = await post.getItemsAsync();
                if (items.Count<PostItem>() > 0)
                {
                    StackItem.Children.Clear();
                    foreach (PostItem item in items)
                    {
                        var controlItem = new Quyd.Controls.ControlItem();
                        controlItem.icon.Source = new BitmapImage(new Uri(item.Icon, UriKind.Absolute));
                        controlItem.quantity.Text = item.Quantity.ToString();
                        StackItem.Children.Add(controlItem);
                    }
                }
                else
                {
                    StackItemStatus.Text = "ไม่พบข้อมูล";
                }
            }
            catch(NullReferenceException)
            {

            }
        }
    }
}
