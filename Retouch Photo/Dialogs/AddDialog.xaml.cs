using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo.Dialogs
{
    public sealed partial class AddDialog : ContentDialog
    {
        BitmapSize SizeInPixels = new BitmapSize();

        #region Delegate

        /// <summary></summary>
        public delegate void AddSizeHandler(BitmapSize pixels);
        public event AddSizeHandler AddSize = null;

        #endregion

        public AddDialog()
        {
            this.InitializeComponent();
        }

        //Width Height
        private void WidthNumberPicker_ValueChange(object sender, int value) => this.SizeInPixels.Width = (uint)value;
        private void HeighNumberPicker_ValueChange(object sender, int value) => this.SizeInPixels.Height = (uint)value;

        //Cancel OK
        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) => this.Hide();
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
            this.AddSize?.Invoke(this.SizeInPixels);//Delegate
        }

    }
}
