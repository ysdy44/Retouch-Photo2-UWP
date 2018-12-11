using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo.Controls
{
    public sealed partial class ToolControl : UserControl
    {
        //ViewModel
        public DrawViewModel ViewModel;

        public ToolControl()
        {
            this.InitializeComponent();

            //ViewModel
            this.ViewModel = App.ViewModel;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = this.ListBox.SelectedIndex;

            this.ViewModel.Tool = this.ViewModel.Tools[index];
            this.ViewModel.Invalidate();
        }


    }
}
