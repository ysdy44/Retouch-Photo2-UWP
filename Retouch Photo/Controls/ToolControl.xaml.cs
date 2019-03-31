using Retouch_Photo.Models;
using Retouch_Photo.Tools;
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
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;


        //delegate
        public delegate void IndexChangedHandler(int index);
        public static event IndexChangedHandler IndexChanged = null;
        public static void SetIndex(int index) => ToolControl.IndexChanged?.Invoke(index);
        

        public ToolControl()
        {
            this.InitializeComponent();
            this.ListBox.Loaded += (sender, e) => this.ListBox.ItemsSource =from tool in Tool.ToolList.Values select tool.Icon; 

            ToolControl.IndexChanged += (int index) =>
            {
                if (this.ListBox.Items == null) return;
             
                if (index < 0) return;
                if (index >= this.ListBox.Items.Count) return;

                this.ListBox.SelectedIndex = index;
            };

            this.ListBox.SelectionChanged+=(sender, e)=>
            {
                int index = this.ListBox.SelectedIndex;

                if (index < 0) return;
                if (index >= Tool.ToolList.Count) return;

                this.ViewModel.Tool = Tool.ToolList[(ToolType)index];
                this.ViewModel.Invalidate();
            };
            
        }
    }
}
