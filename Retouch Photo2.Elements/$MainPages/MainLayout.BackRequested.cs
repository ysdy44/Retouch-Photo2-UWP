using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    //@BackRequested
    public sealed partial class MainLayout : UserControl
    {

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public MainPageState State
        {
            get => this._vsState;
            set
            {
                MainPageState oldValue = this._vsState;
                MainPageState newValue = value;

                this._vsState = value;
                this.VisualState = this.VisualState;//State

                switch (newValue)
                {
                    case MainPageState.None:
                    case MainPageState.Initial:
                    case MainPageState.Main:
                        switch (oldValue)
                        {
                            case MainPageState.Pictures:
                            case MainPageState.Rename:
                            case MainPageState.Delete:
                            case MainPageState.Duplicate:
                                this.Show();
                                break;
                        }
                        break;
                    case MainPageState.Pictures:
                    case MainPageState.Rename:
                    case MainPageState.Delete:
                    case MainPageState.Duplicate:
                        switch (oldValue)
                        {
                            case MainPageState.None:
                            case MainPageState.Initial:
                            case MainPageState.Main:
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