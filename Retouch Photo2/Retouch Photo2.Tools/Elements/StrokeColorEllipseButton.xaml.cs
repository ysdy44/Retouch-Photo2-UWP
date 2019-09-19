using Retouch_Photo2.ViewModels;
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

        //@Static
        public static StrokeColorEllipseControl StrokeColorEllipseControl = new StrokeColorEllipseControl();

        //@Construct
        public StrokeColorEllipseButton()
        {
            this.InitializeComponent();

            this.RootGrid.Tapped += (s, e) =>
            {
                StrokeColorEllipseButton.StrokeColorEllipseControl.Flyout.ShowAt(this);
                StrokeColorEllipseButton.StrokeColorEllipseControl.ColorPicker.Color = this.SelectionViewModel.StrokeColor;
            };
        }
    }
}