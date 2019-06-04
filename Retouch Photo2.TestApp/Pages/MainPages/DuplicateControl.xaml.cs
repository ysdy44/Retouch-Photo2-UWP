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

namespace Retouch_Photo2.TestApp.Pages.MainPages
{

    /// <summary>
    /// <see cref = "MainPage" /> Appbar's <see cref = "DuplicateControl" />.
    /// </summary>
    public sealed partial class DuplicateControl : UserControl
    {

        /// <summary> <see cref = "SaveControl" />'s OKButton. </summary>
        public Windows.UI.Xaml.Controls.Button OKButton { get => this._OKButton.RootButton; set => this._OKButton.RootButton = value; }
        /// <summary> <see cref = "SaveControl" />'s CancelButton. </summary>
        public Windows.UI.Xaml.Controls.Button CancelButton { get => this._CancelButton.RootButton; set => this._CancelButton.RootButton = value; }

        public DuplicateControl()
        {
            this.InitializeComponent();
        }
    }
}