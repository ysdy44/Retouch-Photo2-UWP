using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    //@BackRequested
    public sealed partial class DrawLayout : UserControl
    {

        /// <summary>
        /// Show the phone layout.
        /// </summary>
        /// <param name="type"> The type. </param>
        private void ShowPhone(PhoneLayoutType type)
        {
            if (this._vsPhoneType != PhoneLayoutType.Hided) return;

            //Phone
            {
                this._vsPhoneType = type;
                this.VisualState = this.VisualState;//State
            }

            BackRequestedExtension.LayoutIsShow = true;
            BackRequestedExtension.Current.BackRequested += this.BackRequested;
            Window.Current.CoreWindow.KeyDown += this.CoreWindow_KeyDown;
        }

        /// <summary>
        /// Show writable layout.
        /// </summary>
        /// <param name="icon"> The icon. </param>
        /// <param name="title"> The title. </param>
        /// <param name="content"> The content. </param>
        public void ShowWritable(ControlTemplate icon, string title, object content)
        {
            if (this.IsWritable) return;

            //Writable
            {
                this.WritableIconContentControl.Template = icon;
                this.WritableTextBlock.Text = title;
                this.WritableContentPresenter.Content = content;

                this.IsWritable = true;
                this.WritableVisualState = this.WritableVisualState;//State

                this._vsIsFullScreen = true;
                this.VisualStateCore = this.VisualState;//State
            }

            BackRequestedExtension.LayoutIsShow = true;
            BackRequestedExtension.Current.BackRequested += this.BackRequested;
            Window.Current.CoreWindow.KeyDown += this.CoreWindow_KeyDown;
        }

        /// <summary> Hide the layout. </summary>
        public void Hide()
        {
            //Phone
            if (this._vsPhoneType != PhoneLayoutType.Hided)
            {
                this._vsPhoneType = PhoneLayoutType.Hided;
                this.VisualState = this.VisualState;//State
            }

            //Writable
            if (this.IsWritable)
            {
                this.WritableIconContentControl.Template = null;
                this.WritableTextBlock.Text = string.Empty;
                this.WritableContentPresenter.Content = null;

                this.IsWritable = false;
                this.WritableVisualState = this.WritableVisualState;//State

                this._vsIsFullScreen = false;
                this.VisualState = this.VisualState;//State
            }

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