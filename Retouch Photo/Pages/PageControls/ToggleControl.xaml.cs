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

namespace Retouch_Photo.Pages.PageControls
{
    public sealed partial class ToggleControl : UserControl
    {
        #region DependencyProperty


        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(ToggleControl), typeof(ToggleControl), new PropertyMetadata(false, (sender, e) =>
        {
            ToggleControl con = (ToggleControl)sender;

            if (e.NewValue is bool value)
            {
                con.Check(value);
            }
        }));


        public object CenterContent
        {
            get { return (object)GetValue(CenterContentProperty); }
            set { SetValue(CenterContentProperty, value); }
        }
        public static readonly DependencyProperty CenterContentProperty = DependencyProperty.Register(nameof(CenterContent), typeof(ToggleControl), typeof(ToggleControl), new PropertyMetadata(false, (sender, e) =>
        {
            ToggleControl con = (ToggleControl)sender;

            if (e.NewValue is object value)
            {
                con.ContentPresenter.Content = value;
            }
        }));


        #endregion

        //Delegate
        public delegate void CheckedChangedHandler(bool IsChecked);
        public event CheckedChangedHandler CheckedChanged = null;

        public ToggleControl()
        {
            this.InitializeComponent();
        }

        private void ContentPresenter_Tapped(object sender, TappedRoutedEventArgs e) => this.IsChecked = !this.IsChecked;


        public void Check(bool isChecked)
        {
            this.ContentPresenter.Background = IsChecked ? this.AccentColor : this.UnAccentColor;
            this.ContentPresenter.Foreground = IsChecked ? this.CheckColor : this.UnCheckColor;
            
            this.CheckedChanged?.Invoke(isChecked);
        }

    }
}
