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
using Retouch_Photo2.Elements;
using Retouch_Photo2.Elements.SettingPages;
using Retouch_Photo2.ViewModels;
using Windows.Storage;
using Windows.System;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ImagePage" />. 
    /// </summary>
    public sealed partial class ImagesPage : Page
    { 
        //@Construct
        public ImagesPage()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) =>
            {
                this.TextBlock.Text += "Loaded";
            };

            this.BackButton.Tapped += (s, e) => this.Frame.GoBack();
        }


        //The current page becomes the active page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.TextBlock.Text += "OnNavigatedTo";
        }

    }
}