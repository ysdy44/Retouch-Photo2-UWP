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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Retouch_Photo.Controls.AppbarControls
{
    public sealed partial class AppbarSaveControl : UserControl
    {

        //Delegate

        public event TappedEventHandler OKButtonTapped;
        public event TappedEventHandler CancelButtonTapped;

        public AppbarSaveControl()
        {
            this.InitializeComponent();
        }


        private void OKButton_Tapped(object sender, TappedRoutedEventArgs e) => this.OKButtonTapped?.Invoke(sender, e);
        private void CancelButton_Tapped(object sender, TappedRoutedEventArgs e) => this.CancelButtonTapped?.Invoke(sender, e);

    }
}
