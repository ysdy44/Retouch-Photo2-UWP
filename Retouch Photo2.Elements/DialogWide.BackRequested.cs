using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    //@BackRequested
    public sealed partial class DialogWide : UserControl
    {

        /// <summary> Show the dialog. </summary>
        public void Show()
        {
            this._vsIsShow = true;
            this.VisualState = this.VisualState;//State
            this._PrimaryButton.Focus(FocusState.Pointer);

            BackRequestedExtension.DialogIsShow = true;
            BackRequestedExtension.Current.BackRequested += this.BackRequested;
            Window.Current.CoreWindow.KeyDown += this.CoreWindow_KeyDown;
        }
        /// <summary> Hide the dialog. </summary>
        public void Hide()
        {
            this._vsIsShow = false;
            this.VisualState = this.VisualState;//State

            BackRequestedExtension.DialogIsShow = false;
            BackRequestedExtension.Current.BackRequested -= this.BackRequested;
            Window.Current.CoreWindow.KeyDown -= this.CoreWindow_KeyDown;
        }

        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            this.Hide();
        }
        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs e)
        {
            switch (e.VirtualKey)
            {
                case VirtualKey.Escape:
                    this.Hide();
                    break;
            }
        }

    }
}