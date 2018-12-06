using Retouch_Photo.Library;
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
    public sealed partial class ToolFreeHandMarqueePage : Page
    {
        //ViewModel
        public DrawViewModel ViewModel;

        public ToolFreeHandMarqueePage()
        {
            this.InitializeComponent();

            //ViewModel
            this.ViewModel = App.ViewModel;
            this.CompositModeControl.Mode = this.ViewModel.MarqueeTool.CompositeMode;
        }

        private void CompositModeControl_ModeChanged(MarqueeCompositeMode mode) => this.ViewModel.MarqueeTool.CompositeMode = mode;

    }
}
