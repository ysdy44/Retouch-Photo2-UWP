using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo.Dialogs
{
    public sealed partial class WelcomeContentDialog : ContentDialog
    {

        private ApplicationViewTitleBar TitleBar = ApplicationView.GetForCurrentView().TitleBar;
        public ElementTheme TitleRequestedTheme
        {
            set
            {
                    this.LightToggleButton.IsChecked = value == ElementTheme.Light;
                    this.DarkToggleButton.IsChecked = value == ElementTheme.Dark;
                    this.TitleBar.ButtonInactiveBackgroundColor =
                    this.TitleBar.ButtonBackgroundColor =
                    this.TitleBar.InactiveBackgroundColor =
                    this.TitleBar.BackgroundColor =
                    value == ElementTheme.Light ?
                    Color.FromArgb(255, 238, 238, 238) :
                    Color.FromArgb(255, 0, 0, 0);
            }
        }
        public ElementTheme AppRequestedTheme
        {
            set
            {
                if (Window.Current.Content is FrameworkElement frameworkElement)
                {
                    frameworkElement.RequestedTheme = value;
                }
            }
        }

        public WelcomeContentDialog()
        {
            this.InitializeComponent();
        }

        private void DarkToggleButton_Tapped(object sender, TappedRoutedEventArgs e) => this.TitleRequestedTheme = this.AppRequestedTheme = ElementTheme.Dark;
        private void LightToggleButton_Tapped(object sender, TappedRoutedEventArgs e) => this.TitleRequestedTheme = this.AppRequestedTheme = ElementTheme.Light;

        private void CloseButton_Click(object sender, RoutedEventArgs e) => this.Hide();

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                this.TitleRequestedTheme = frameworkElement.RequestedTheme;
            }
        }

    }
}
