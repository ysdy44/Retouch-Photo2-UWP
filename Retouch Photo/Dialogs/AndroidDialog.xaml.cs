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

namespace Retouch_Photo.Dialogs
{
    public sealed partial class AndroidDialog : UserControl
    {

        #region DependencyProperty

        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }
        public static readonly DependencyProperty IsShowProperty = DependencyProperty.Register(nameof(IsShow), typeof(bool), typeof(AndroidDialog), new PropertyMetadata(false, (s, e) =>
        {
            AndroidDialog con = (AndroidDialog)s;

            if (e.NewValue is bool value)
            {
                if (value)
                {
                    con.RootGrid.Visibility = Visibility.Visible;
                    con.BackgroundGrid.Visibility = Visibility.Visible;
                }
                else
                {
                    con.RootGrid.Visibility = Visibility.Collapsed;
                    con.BackgroundGrid.Visibility = Visibility.Collapsed;
                }
            }
        }));

        #endregion

        public string Tittle { get => this.TittlePresenter.Text; set => this.TittlePresenter.Text = value; }
        public object CenterContent { get => this.ContentPresenter.Content; set => this.ContentPresenter.Content = value; }

        public AndroidDialog()
        {
            this.InitializeComponent();
        }

        private void CloseButton_Tapped(object sender, TappedRoutedEventArgs e) => this.IsShow = false;
    }
}
