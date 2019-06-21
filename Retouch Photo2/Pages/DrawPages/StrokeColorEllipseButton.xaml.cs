using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Pages.DrawPages
{
    /// <summary>
    /// Button of <see cref = "SelectionViewModel.StrokeColor" />.
    /// </summary>
    public sealed partial class StrokeColorEllipseButton : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => Retouch_Photo2.App.ViewModel;
        SelectionViewModel SelectionViewModel => Retouch_Photo2.App.SelectionViewModel;

        //@Static
        static StrokeColorEllipseControl StrokeColorEllipseControl = new StrokeColorEllipseControl();

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