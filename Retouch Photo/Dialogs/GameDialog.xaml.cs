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
    public sealed partial class GameDialog : UserControl
    {
        #region DependencyProperty

        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }
        public static readonly DependencyProperty IsShowProperty = DependencyProperty.Register(nameof(IsShow), typeof(bool), typeof(GameDialog), new PropertyMetadata(false, (s, e) =>
        {
            GameDialog con = (GameDialog)s;

            if (e.NewValue is bool value)
            {
                if (value)
                {
                    con.RootGrid.Visibility = Visibility.Visible;
                    con.ContentGrid.Visibility = Visibility.Visible;
                }
                else
                {
                    con.RootGrid.Visibility = Visibility.Collapsed;
                    con.ContentGrid.Visibility = Visibility.Collapsed;
                }
            }
        }));

        #endregion

        public object Tittle { get => this.TittlePresenter.Content; set => this.TittlePresenter.Content = value; }
        public object CenterContent { get => this.ContentPresenter.Content; set => this.ContentPresenter.Content = value; }

        public GameDialog()
        {
            this.InitializeComponent();
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.IsShow = false;
        }
    }
}
