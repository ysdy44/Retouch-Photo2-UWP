using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Pages.DrawPages
{
    /// <summary>
    /// Button of <see cref = "SelectionViewModel.FillColor" />.
    /// </summary>
    public sealed partial class FillColorEllipseButton : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Static
        static FillColorEllipseControl FillColorEllipseControl = new FillColorEllipseControl();

        //@Construct
        public FillColorEllipseButton()
        {
            this.InitializeComponent();

            this.RootGrid.Tapped += (s, e) =>
            {
                FillColorEllipseButton.FillColorEllipseControl.Flyout.ShowAt(this);
                FillColorEllipseButton.FillColorEllipseControl.ColorPicker.Color = this.SelectionViewModel.FillColor;
            };
        }
    }
}