using Retouch_Photo2.ViewModels;
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

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Control of <see cref = "KeyboardViewModel.IsSquare" /> and <see cref = "KeyboardViewModel.IsCenter" />.
    /// </summary>
    public sealed partial class MoreCreateControl : UserControl
    {
        //@ViewModel
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;

        //@Content
        /// <summary> MoreCreateControl's _Flyout. </summary>
        public Flyout Flyout { get => this._Flyout; set => this.Flyout = value; }

        //@Construct
        public MoreCreateControl()
        {
            this.InitializeComponent(); 
        }
    }
}
