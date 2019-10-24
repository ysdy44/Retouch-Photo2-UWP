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
    public sealed partial class PhotoControl : UserControl
    {

        //@VisualState
        bool? _vsSelectMode = null;
        public VisualState VisualState
        {
            get
            {
                switch (this._vsSelectMode)
                {
                    case null: return this.Normal;
                    case false: return this.UnSelected;
                    case true: return this.Selected;
                }
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        //@Construct
        public PhotoControl(Photo photo)
        {
            this.InitializeComponent();

            this.TextBlock.Text = photo.Name;
            this.ImageEx.Source = photo.Uri;

            this.RootGrid.Tapped += (s, e) =>
            {
                Photo.ItemClick?.Invoke(this.BackgroundGrid, photo);//Delegate
            };
        }

        public void SetSelectMode(bool? value)
        {
            this._vsSelectMode = value;
            this.VisualState = this.VisualState;//State
        }

    }
}