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

namespace Retouch_Photo.Element
{
    public sealed partial class CompositeModeControl : UserControl
    {

        #region DependencyProperty

        public MarqueeCompositeMode Mode
        {
            get { return (MarqueeCompositeMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(MarqueeCompositeMode), typeof(CompositeModeControl), new PropertyMetadata(MarqueeCompositeMode.New, (sender, e) =>
        {
            CompositeModeControl con = (CompositeModeControl)sender;

            if (e.NewValue is MarqueeCompositeMode value)
            {
                con.Segmente(value);
            }
        }));

        #endregion

        //Delegate
        public delegate void ModeChangedHandler(MarqueeCompositeMode mode);
        public event ModeChangedHandler ModeChanged = null;

        public CompositeModeControl()
        {
            this.InitializeComponent(); 
        }

        private void NewSegmented_Tapped(object sender, TappedRoutedEventArgs e) => this.Mode=MarqueeCompositeMode.New;
        private void AddSegmented_Tapped(object sender, TappedRoutedEventArgs e) => this.Mode = MarqueeCompositeMode.Add;
        private void SubtractSegmented_Tapped(object sender, TappedRoutedEventArgs e) => this.Mode = MarqueeCompositeMode.Subtract;
        private void IntersectSegmented_Tapped(object sender, TappedRoutedEventArgs e) => this.Mode = MarqueeCompositeMode.Intersect;

        private void Segmente(MarqueeCompositeMode mode)
        {
            this.SegmenteColor(this.NewSegmented, mode == MarqueeCompositeMode.New);
            this.SegmenteColor(this.AddSegmented, mode == MarqueeCompositeMode.Add);
            this.SegmenteColor(this.SubtractSegmented, mode == MarqueeCompositeMode.Subtract);
            this.SegmenteColor(this.IntersectSegmented, mode == MarqueeCompositeMode.Intersect);

            this.ModeChanged?.Invoke(mode);
        }

        private void SegmenteColor(ContentPresenter control, bool IsChecked)
        {
            control.Background = IsChecked ? this.AccentColor : this.UnAccentColor;
            control.Foreground = IsChecked ? this.CheckColor : this.UnCheckColor;
        }



    }
}
