using Retouch_Photo.ViewModels;
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

namespace Retouch_Photo.Pages.ToolPages
{
    public sealed partial class ToolViewPage : Page
    {
        //ViewModel
        public DrawViewModel ViewModel;

        public ToolViewPage()
        {
            this.InitializeComponent();

            //ViewModel
            this.ViewModel = App.ViewModel;
            this.Slider.Value = this.ViewModel.Transformer.Radian*180.0/Math.PI;
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (this.ViewModel.CanvasControl == null) return;
         
            this.ViewModel.Transformer.Radian = (float)(e.NewValue/180.0*Math.PI);
            this.ViewModel.Invalidate(true);
        }
    }
}
