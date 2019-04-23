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
 
namespace Retouch_Photo2.Element
{
    public sealed partial class LoadingControl : UserControl
    {
        public string Text{ set => this.TextBlock.Text = value; get => this.TextBlock.Text; }

        public LoadingControl()
        {
            this.InitializeComponent(); 
        }
    }
}
