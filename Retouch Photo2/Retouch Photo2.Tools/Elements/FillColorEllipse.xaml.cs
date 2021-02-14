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
    /// Fill - ColorEllipse
    /// </summary>
    public sealed partial class FillColorEllipse : UserControl
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Static
        /// <summary>
        /// Displays the fill-color flyout relative to the specified element.
        /// </summary>
        public static Action<FrameworkElement> ShowAt;
        private static Flyout Flyout;
        private static Retouch_Photo2.Elements.ColorPicker2 ColorPicker = new Retouch_Photo2.Elements.ColorPicker2();


        //@Construct
        /// <summary>
        /// Initializes a FillColorEllipse. 
        /// </summary>
        public FillColorEllipse()
        {
            this.InitializeComponent();

            // Displays the flyout relative to the specified element.
            this.ColorEllipse.Tapped += (s, e) =>
            {
                if (FillColorEllipse.Flyout != null)
                {
                    this.ShowFlyout();
                }
            };

            if (FillColorEllipse.Flyout == null)
            {
                this.ConstructFlyout();

                FillColorEllipse.ShowAt += this.ShowFlyout;
            }
        }


        private void ShowFlyout()
        {
            switch (this.SelectionViewModel.Fill.Type)
            {
                case BrushType.Color:
                    FillColorEllipse.ColorPicker.Color = this.SelectionViewModel.Fill.Color;
                    break;
            }

            FillColorEllipse.Flyout.ShowAt(this.ColorEllipse);
        }
        private void ShowFlyout(FrameworkElement placementTarget)
        {
            switch (this.SelectionViewModel.Fill.Type)
            {
                case BrushType.Color:
                    FillColorEllipse.ColorPicker.Color = this.SelectionViewModel.Fill.Color;
                    break;
            }

            FillColorEllipse.Flyout.ShowAt(placementTarget);
        }


        private void ConstructFlyout()
        {
            FillColorEllipse.Flyout = new Flyout
            {
                Content = FillColorEllipse.ColorPicker,
                FlyoutPresenterStyle=this.FlyoutPresenterStyle
            };            

            //@Focus
            // Before Flyout Showed, Don't let TextBox Got Focus.
            // After TextBox Gots focus, disable Shortcuts in SettingViewModel.
            if (FillColorEllipse.ColorPicker.HexPicker is TextBox textBox)
            {
                textBox.IsEnabled = false;
                FillColorEllipse.Flyout.Opened += (s, e) => textBox.IsEnabled = true;
                FillColorEllipse.Flyout.Closed += (s, e) => textBox.IsEnabled = false;
                textBox.GotFocus += (s, e) => this.SettingViewModel.UnRegisteKey();
                textBox.LostFocus += (s, e) => this.SettingViewModel.RegisteKey();
            }

            FillColorEllipse.ColorPicker.ColorChanged += (s, value) => this.MethodViewModel.MethodFillColorChanged(value);

            FillColorEllipse.ColorPicker.ColorChangeStarted += (s, value) => this.MethodViewModel.MethodFillColorChangeStarted(value);
            FillColorEllipse.ColorPicker.ColorChangeDelta += (s, value) => this.MethodViewModel.MethodFillColorChangeDelta(value);
            FillColorEllipse.ColorPicker.ColorChangeCompleted += (s, value) => this.MethodViewModel.MethodFillColorChangeCompleted(value);
        }

    }
}