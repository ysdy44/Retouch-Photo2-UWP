using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    //@BackRequested
    public sealed partial class DrawLayout : UserControl
    {

        private PhoneLayoutType PhoneType
        {
            set
            {
                PhoneLayoutType oldValue = this._vsPhoneType;
                PhoneLayoutType newValue = value;

                this._vsPhoneType = value;
                this.VisualState = this.VisualState;//State

                switch (newValue)
                {
                    case PhoneLayoutType.ShowLeft:
                    case PhoneLayoutType.ShowRight:
                        switch (oldValue)
                        {
                            case PhoneLayoutType.Hided:
                                this.Show();
                                break;
                        }
                        break;
                    case PhoneLayoutType.Hided:
                        switch (oldValue)
                        {
                            case PhoneLayoutType.ShowLeft:
                            case PhoneLayoutType.ShowRight:
                                this.Hide();
                                break;
                        }
                        break;
                }
            }
        }


        /// <summary> Show the dialog. </summary>
        private void Show()
        {
            BackRequestedExtension.LayoutIsShow = true;
            BackRequestedExtension.Current.BackRequested += this.BackRequested;
            Window.Current.CoreWindow.KeyDown += this.CoreWindow_KeyDown;
        }
        /// <summary> Hide the dialog. </summary>
        private void Hide()
        {
            BackRequestedExtension.LayoutIsShow = false;
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
            if (BackRequestedExtension.DialogIsShow) return;

            switch (e.VirtualKey)
            {
                case VirtualKey.Escape:
                    this.Hide();
                    break;
            }
        }

    }
}