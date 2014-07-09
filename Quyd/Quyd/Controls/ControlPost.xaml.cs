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

namespace Quyd.Controls
{
    public partial class ControlPost : UserControl
    {
        public ControlPost()
        {
            InitializeComponent();
        }

        public void setLocation(string location)
        {
            locationBox.Text = location;
        }

        public void setItems(ItemList itemList)
        {
            StackItem.Children.Clear();
            foreach (Item item in itemList)
            {
                var controlItem = new Quyd.Controls.ControlItem();
                controlItem.quantity.Text = (item as Quantifiable).Quantity.ToString();
                StackItem.Children.Add(controlItem);
            }
        }
    }
}
