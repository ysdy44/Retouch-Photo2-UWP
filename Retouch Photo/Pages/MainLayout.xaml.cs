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

namespace Retouch_Photo.Pages
{
    public sealed partial class MainLayout : UserControl
    {

        public UIElement CenterContent { get => this.CenterBorder.Child; set => this.CenterBorder.Child = value; }
        public UIElement Appbar { get => this.AppbarGrid.Child; set => this.AppbarGrid.Child = value; }

        public MainLayout()
        {
            this.InitializeComponent();
        }

        // Appbar
        private void AppbarGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.AppbarRectangleFrameWidth.Value = e.NewSize.Width;
            this.AppbarRectangleStoryboard.Begin();
        }
    }
}
