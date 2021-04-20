using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;

namespace Retouch_Photo2.Elements
{
    //@BackRequested
    public sealed partial class MainLayout : UserControl
    {

        #region DependencyProperty

        
        /// <summary> Gets or set the state for <see cref="ApplicationViewTitleBar"/>. </summary>
        public bool IsAccent
        {
            get => (bool)base.GetValue(IsAccentProperty);
            set => SetValue(IsAccentProperty, value);
        }
        /// <summary> Identifies the <see cref = "ApplicationTitleBarExtension.IsAccent" /> dependency property. </summary>
        public static readonly DependencyProperty IsAccentProperty = DependencyProperty.Register(nameof(IsAccent), typeof(bool), typeof(ApplicationTitleBarExtension), new PropertyMetadata(false));


        /// <summary> Gets or set the state. </summary>
        public MainPageState State
        {
            get => (MainPageState)base.GetValue(TitleBarColorProperty);
            set => SetValue(TitleBarColorProperty, value);
        }
        /// <summary> Identifies the <see cref = "MainLayout.State" /> dependency property. </summary>
        public static readonly DependencyProperty TitleBarColorProperty = DependencyProperty.Register(nameof(State), typeof(MainPageState), typeof(MainLayout), new PropertyMetadata(MainPageState.None, (sender, e) =>
        {
            MainLayout control = (MainLayout)sender;

            if (e.NewValue is MainPageState value && e.OldValue is MainPageState oldValue)
            {
                switch (value)
                {
                    case MainPageState.Pictures:
                    case MainPageState.Rename:
                    case MainPageState.Delete:
                    case MainPageState.Duplicate:
                        switch (oldValue)
                        {
                            case MainPageState.Main:
                                control.IsAccent = true;
                                control.Show(value);
                                break;
                        }
                        break;
                    case MainPageState.Main:
                        control.IsAccent = false;
                        control.Hide();
                        break;
                    default:
                        break;
                }
            }
        }));


        #endregion


        /// <summary>
        /// Show the page state.
        /// </summary>
        /// <param name="state"> The state. </param>
        public void Show(MainPageState state)
        {
            this._vsState = state;
            this.VisualState = this.VisualState;//State

            BackRequestedExtension.LayoutIsShow = true;
            BackRequestedExtension.Current.BackRequested += this.BackRequested;
            Window.Current.CoreWindow.KeyDown += this.CoreWindow_KeyDown;
        }
        /// <summary> Hide the page. </summary>
        public void Hide()
        {
            this._vsState = MainPageState.Main;
            this.VisualState = this.VisualState;//State

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