using Retouch_Photo.Library;
using Retouch_Photo.Models;
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

namespace Retouch_Photo.Controls
{
    public sealed partial class BottombarControl : UserControl
    {


        #region DependencyProperty


        public Tool Tool
        {
            get { return (Tool)GetValue(ToolsProperty); }
            set { SetValue(ToolsProperty, value); }
        }
        public static readonly DependencyProperty ToolsProperty = DependencyProperty.Register(nameof(Tool), typeof(Tool), typeof(BottombarControl), new PropertyMetadata(null, (sender, e) =>
        {
            BottombarControl con = (BottombarControl)sender;

            if (e.NewValue is Tool tool)
            {
                con.Frame.Content = tool.Page;
            }
        }));


        #endregion

        public BottombarControl()
        {
            this.InitializeComponent();
        }
    }
}
