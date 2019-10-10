using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Button of <see cref = "SelectionViewModel.FillColor" />.
    /// </summary>
    public sealed partial class FillColorEllipseButton : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@Static
        public static FillColorEllipseControl FillColorEllipseControl = new FillColorEllipseControl();
        Flyout Flyout => FillColorEllipseButton.FillColorEllipseControl.Flyout;
        HSVColorPickers.ColorPicker ColorPicker => FillColorEllipseButton.FillColorEllipseControl.ColorPicker;

        //@Construct
        public FillColorEllipseButton()
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

                this.ColorPicker.Color = this.SelectionViewModel.FillColor;
            };
        }
    }
}