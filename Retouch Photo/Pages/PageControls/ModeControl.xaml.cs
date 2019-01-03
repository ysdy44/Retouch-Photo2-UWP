using Retouch_Photo.Library;
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
    public sealed partial class ModeControl : UserControl
    {

        #region DependencyProperty


        public MarqueeMode Mode
        {
            get { return (MarqueeMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(MarqueeMode), typeof(ModeControl), new PropertyMetadata(MarqueeMode.None, (sender, e) =>
        {
            ModeControl con = (ModeControl)sender;

            if (e.NewValue is MarqueeMode value)
            {
                con.Segmente(value);
            }
        }));



        #endregion

        //Delegate
        public delegate void ModeChangedHandler(MarqueeMode mode);
        public event ModeChangedHandler ModeChanged = null;

        public ModeControl()
        {
            this.InitializeComponent();
        }

        private void NoneSegmented_Tapped(object sender, TappedRoutedEventArgs e) => this.Segmented_Tapped(MarqueeMode.None);
        private void SquareSegmented_Tapped(object sender, TappedRoutedEventArgs e) => this.Segmented_Tapped(MarqueeMode.Square);
        private void CenterSegmented_Tapped(object sender, TappedRoutedEventArgs e) => this.Segmented_Tapped(MarqueeMode.Center);
        private void SquareAndCenterSegmented_Tapped(object sender, TappedRoutedEventArgs e) => this.Segmented_Tapped(MarqueeMode.SquareAndCenter);
        private void Segmented_Tapped(MarqueeMode mode)
        {
            this.Mode = mode;
            this.ModeChanged?.Invoke(mode);
        }


        private void Segmente(MarqueeMode mode)
        {
            this.SegmenteColor(this.NoneSegmented, mode == MarqueeMode.None);
            this.SegmenteColor(this.SquareSegmented, mode == MarqueeMode.Square);
            this.SegmenteColor(this.CenterSegmented, mode == MarqueeMode.Center);
            this.SegmenteColor(this.SquareAndCenterSegmented, mode == MarqueeMode.SquareAndCenter);
        }
        private void SegmenteColor(ContentPresenter control, bool IsChecked)
        {
            control.Background = IsChecked ? this.AccentColor : this.UnAccentColor;
            control.Foreground = IsChecked ? this.CheckColor : this.UnCheckColor;
        }
    }
}
