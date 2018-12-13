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
    public sealed partial class ToolEllipticalMarqueePage : Page
    {
        //ViewModel
        public DrawViewModel ViewModel;

        public ToolEllipticalMarqueePage()
        {
            this.InitializeComponent();

            //ViewModel
            this.ViewModel = App.ViewModel;
            //[临时删掉]    this.CompositModeControl.Mode = this.ViewModel.MarqueeTool.CompositeMode;
            //[临时删掉]     this.ModeControl.Mode = this.ViewModel.MarqueeTool.MarqueeMode;
        }

        private void CompositModeControl_ModeChanged(MarqueeCompositeMode mode) { }  //[临时删掉]  => this.ViewModel.MarqueeTool.CompositeMode = mode;
        private void ModeControl_ModeChanged(MarqueeMode mode) { }  //[临时删掉]  => this.ViewModel.MarqueeTool.MarqueeMode = mode;

    }
}
