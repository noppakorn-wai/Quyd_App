using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

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
    }
}
