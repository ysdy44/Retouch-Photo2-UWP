﻿using Retouch_Photo.ViewModels;
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
using Windows.UI;
using Retouch_Photo.Library;

namespace Retouch_Photo.Pages.ToolPages
{
    public sealed partial class ToolRectanglePage : Page
    {
        //ViewModel
        public DrawViewModel ViewModel;

        public ToolRectanglePage()
        {
            this.InitializeComponent();

            //ViewModel
            this.ViewModel = App.ViewModel;
            this.ModeControl.Mode = this.ViewModel.MarqueeTool.MarqueeMode;
            this.ColorBrush.Color = this.ViewModel.Color;
            this.ColorPicker.Color = this.ViewModel.Color;
        }

        private void ModeControl_ModeChanged(MarqueeMode mode)
        {
            this.ViewModel.MarqueeTool.MarqueeMode = this.ModeControl.Mode;
        }
        private void ColorButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.ColorFlyout.ShowAt(this.ColorButton);
        }
        private void ColorPicker_ColorChange(object sender, Color value)
        {
            this.ViewModel.Color = value;
            this.ColorBrush.Color = value;
        }

    }
}