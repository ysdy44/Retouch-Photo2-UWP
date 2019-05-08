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

namespace Retouch_Photo2.Pages.MainPages
{
    public sealed partial class FolderDialog : ContentDialog
    {
        String name = "Untitled";

        #region Delegate

        /// <summary></summary>
        public delegate void FolderNameHandler(String name);
        public event FolderNameHandler FolderName = null;

        #endregion

        public FolderDialog()
        {
            this.InitializeComponent();
        }

        //Cancel OK
        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) => this.Hide();
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
            this.FolderName?.Invoke(this.name);//Delegate
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.name = this.TextBox.Text;
        }
    }
}
