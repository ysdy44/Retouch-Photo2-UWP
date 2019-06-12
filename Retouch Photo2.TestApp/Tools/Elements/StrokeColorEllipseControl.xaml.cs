using Retouch_Photo2.TestApp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.Tools.Elements
{
    /// <summary>
    /// Control of <see cref = "ViewModel.StrokeColor" />. 
    /// </summary>
    public sealed partial class StrokeColorEllipseControl : UserControl
    {
        //ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;

        //@Construct
        public StrokeColorEllipseControl()
        {
            this.InitializeComponent();

            this.RootGrid.Tapped += (s, e) =>
            {
                this.ViewModel.StrokeColorFlyout.ShowAt(this.RootGrid);
                this.ViewModel.StrokeColorPicker.Color = this.ViewModel.StrokeColor;
            };
        }
    }
}