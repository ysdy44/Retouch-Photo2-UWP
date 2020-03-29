using Microsoft.Toolkit.Uwp.UI.Controls;
using Retouch_Photo2.Layers;
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

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// MainPagr's the only <see cref = "ViewModels.PhotoControl" />. 
    /// </summary>
    public sealed partial class PhotoControl : UserControl
    {

        //@VisualState
        PhotoSelectMode _vsSelectMode = PhotoSelectMode.None;
        public VisualState VisualState
        {
            get
            {
                switch (this._vsSelectMode)
                {
                    case PhotoSelectMode.None: return this.Normal;
                    case PhotoSelectMode.UnSelected: return this.UnSelected;
                    case PhotoSelectMode.Selected: return this.Selected;
                }
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        //@Content
        /// <summary> TextBlock's tittle. </summary>
        public String Titlle { set => this.TextBlock.Text = value; }
        /// <summary> ImageEx's source. </summary>
        public object ImageSource { set => this.ImageEx.Source = value; }

        /// <summary>
        /// Gets or sets the select-mode.
        /// </summary>
        public PhotoSelectMode SelectMode
        {
            get => this._vsSelectMode;
            set
            {
                this._vsSelectMode = value;
                this.VisualState = this.VisualState;//State
            }
        }

        //@Construct
        public PhotoControl()
        {
            this.InitializeComponent();
        }
    }
}