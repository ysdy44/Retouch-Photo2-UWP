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

namespace Retouch_Photo2.Elements.MainPages
{
    /// <summary>
    /// <see cref = "MainPage" /> Appbar's <see cref = "DuplicateControl" />.
    /// </summary>
    public sealed partial class DuplicateControl : UserControl
    {
        //@Content
        /// <summary> <see cref = "SaveControl" />'s OKButton. </summary>
        public Button OKButton   => this._OKButton.RootButton;  
        /// <summary> <see cref = "SaveControl" />'s CancelButton. </summary>
        public Button CancelButton => this._CancelButton.RootButton;

        //@Construct
        public DuplicateControl()
        {
            this.InitializeComponent();
        }
    }
}