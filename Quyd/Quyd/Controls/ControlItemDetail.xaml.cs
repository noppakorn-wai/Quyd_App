using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Parse;
using Quyd.Models;

namespace Quyd.Controls
{
    public partial class ControlItemDetail : UserControl
    {
        public Item item;
        private int value;

        public ControlItemDetail()
        {
            InitializeComponent();
            this.item = null;
        }

        public ControlItemDetail(Item item)
        {
            InitializeComponent();
            this.item = item;
        }

        private async void BoxValue_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int input = Convert.ToInt32(BoxValue.Text);

                if (item != null)
                {
                    (item as Priceable).Price = input;
                    await (item as StoreItem).saveAsync();
                }
            }
            catch (System.FormatException)
            {
                BoxValue.Text = value.ToString();
            }
        }

        private void BoxValue_GotFocus(object sender, RoutedEventArgs e)
        {
            value = Convert.ToInt32(BoxValue.Text);
        }
    }
}
