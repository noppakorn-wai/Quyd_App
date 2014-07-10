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
        private Item item;

        public ControlItemDetail()
        {
            InitializeComponent();
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
                (item as Priceable).Price = input;
                await item.saveAsync();
            }
            catch (System.FormatException)
            {
               
                BoxValue.Text = (item as Priceable).Price.ToString();
            }
        }
    }
}
