// Core:              
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★★
using Retouch_Photo2.Brushs;
using Retouch_Photo2.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Stroke - ColorEllipse
    /// </summary>
    public sealed partial class StrokeColorEllipse : UserControl
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Static
        /// <summary>
        /// Displays the Stroke-color flyout relative to the specified element.
        /// </summary>
        public static Action<FrameworkElement> ShowAt;
        private static Flyout Flyout;
        private static Retouch_Photo2.Elements.ColorPicker2 ColorPicker = new Retouch_Photo2.Elements.ColorPicker2();


        //@Construct
        /// <summary>
        /// Initializes a StrokeColorEllipse. 
        /// </summary>
        public StrokeColorEllipse()
        {
            this.InitializeComponent();

            // Displays the flyout relative to the specified element.
            this.ColorEllipse.Tapped += (s, e) =>
            {
                if (StrokeColorEllipse.Flyout != null)
                {
                    this.ShowFlyout();
                }
            };

            if (StrokeColorEllipse.Flyout == null)
            {
                this.ConstructFlyout();

                StrokeColorEllipse.ShowAt += this.ShowFlyout;
            }
        }


        private void ShowFlyout()
        {
            switch (this.SelectionViewModel.Stroke.Type)
            {
                case BrushType.Color:
                    StrokeColorEllipse.ColorPicker.Color = this.SelectionViewModel.Stroke.Color;
                    break;
            }

            StrokeColorEllipse.Flyout.ShowAt(this.ColorEllipse);
        }
        private void ShowFlyout(FrameworkElement placementTarget)
        {
            switch (this.SelectionViewModel.Stroke.Type)
            {
                case BrushType.Color:
                    StrokeColorEllipse.ColorPicker.Color = this.SelectionViewModel.Stroke.Color;
                    break;
            }

            StrokeColorEllipse.Flyout.ShowAt(placementTarget);
        }


        private void ConstructFlyout()
        {
            StrokeColorEllipse.Flyout = new Flyout
            {
                Content = StrokeColorEllipse.ColorPicker,
                FlyoutPresenterStyle = this.FlyoutPresenterStyle
            };

            //@Focus
            // Before Flyout Showed, Don't let TextBox Got Focus.
            // After TextBox Gots focus, disable Shortcuts in SettingViewModel.
            if (StrokeColorEllipse.ColorPicker.HexPicker is TextBox textBox)
            {
                textBox.IsEnabled = false;
                StrokeColorEllipse.Flyout.Opened += (s, e) => textBox.IsEnabled = true;
                StrokeColorEllipse.Flyout.Closed += (s, e) => textBox.IsEnabled = false;
                textBox.GotFocus += (s, e) => this.SettingViewModel.KeyIsEnabled = false;
                textBox.LostFocus += (s, e) => this.SettingViewModel.KeyIsEnabled = true;
            }

            StrokeColorEllipse.ColorPicker.ColorChanged += (s, value) => this.MethodViewModel.MethodStrokeColorChanged(value);

            StrokeColorEllipse.ColorPicker.ColorChangeStarted += (s, value) => this.MethodViewModel.MethodStrokeColorChangeStarted(value);
            StrokeColorEllipse.ColorPicker.ColorChangeDelta += (s, value) => this.MethodViewModel.MethodStrokeColorChangeDelta(value);
            StrokeColorEllipse.ColorPicker.ColorChangeCompleted += (s, value) => this.MethodViewModel.MethodStrokeColorChangeCompleted(value);
        }

    }
}