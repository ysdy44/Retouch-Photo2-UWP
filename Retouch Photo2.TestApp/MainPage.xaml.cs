﻿using Retouch_Photo2.Operates;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Retouch_Photo2.TestApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            ZipArchive archive;
            this.ContentControl.Content = new BackOneIcon();
        }

        private void AAA_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.ContentControl.IsEnabled = true;
        }
        private void BBB_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.ContentControl.IsEnabled = false;
        }
    }
}
