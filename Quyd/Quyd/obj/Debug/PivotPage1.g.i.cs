﻿#pragma checksum "C:\Users\duckky\Desktop\Quyd_App\Quyd\Quyd\PivotPage1.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "41B895BEE92D1EB81BC9EAF7CB3D1BF2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.33440
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Quyd {
    
    
    public partial class PivotPage1 : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.PivotItem User;
        
        internal Microsoft.Phone.Controls.PivotItem Feeds;
        
        internal Microsoft.Phone.Controls.PivotItem Notifications;
        
        internal System.Windows.Controls.TextBlock notificationBox;
        
        internal Microsoft.Phone.Controls.PivotItem Search;
        
        internal Microsoft.Phone.Controls.PivotItem Store;
        
        internal System.Windows.Controls.Image AppLogo;
        
        internal System.Windows.Controls.TextBlock Appname;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Quyd;component/PivotPage1.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.User = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("User")));
            this.Feeds = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("Feeds")));
            this.Notifications = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("Notifications")));
            this.notificationBox = ((System.Windows.Controls.TextBlock)(this.FindName("notificationBox")));
            this.Search = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("Search")));
            this.Store = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("Store")));
            this.AppLogo = ((System.Windows.Controls.Image)(this.FindName("AppLogo")));
            this.Appname = ((System.Windows.Controls.TextBlock)(this.FindName("Appname")));
        }
    }
}

