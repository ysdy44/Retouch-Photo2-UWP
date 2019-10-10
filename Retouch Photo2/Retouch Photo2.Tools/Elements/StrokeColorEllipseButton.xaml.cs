using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Button of <see cref = "SelectionViewModel.StrokeColor" />.
    /// </summary>
    public sealed partial class StrokeColorEllipseButton : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@Static
        public static StrokeColorEllipseControl StrokeColorEllipseControl = new StrokeColorEllipseControl();
        Flyout Flyout => StrokeColorEllipseButton.StrokeColorEllipseControl.Flyout;
        HSVColorPickers.ColorPicker ColorPicker => StrokeColorEllipseButton.StrokeColorEllipseControl.ColorPicker;

        //@Construct
        public StrokeColorEllipseButton()
        {
            this.InitializeComponent();

            this.RootGrid.Tapped += (s, e) =>
            {
                this.TipViewModel.TouchbarType = TouchbarType.None;//Touchbar

                if (this.Parent is FrameworkElement placementTarget)
                {
                    this.Flyout.ShowAt(placementTarget);
                }
                else
                {
                    this.Flyout.ShowAt(this);
                }

                this.ColorPicker.Color = this.SelectionViewModel.StrokeColor;
            };
        }
    }
}