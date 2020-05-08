using Retouch_Photo2.Operates;
using System;
using System.Collections.Generic;
using System.IO;
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
            this.ContentControl.Content = new BackOneIcon();


            //   1.
            //     ToolTipService.SetToolTip(this.Button, "OOOOOO");


            //   2.
            ToolTip ToolTip = new ToolTip()
            {
                Content = "OOOOOO",
                Placement = PlacementMode.Right,
                Style = this.pToolTipStyle
            };

            ToolTipService.SetToolTip(this.Button, ToolTip);




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
