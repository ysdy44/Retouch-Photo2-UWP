using Retouch_Photo2.TestApp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.Tools.Elements
{
    /// <summary>
    /// Control of <see cref = "ViewModel.FillColor" />. 
    /// </summary>
    public sealed partial class FillColorEllipseControl : UserControl
    {
        //ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;

        //@Construct
        public FillColorEllipseControl()
        {
            this.InitializeComponent();

            this.RootGrid.Tapped += (s, e) =>
            {
                this.ViewModel.FillColorFlyout.ShowAt(this.RootGrid);
                this.ViewModel.FillColorPicker.Color = this.ViewModel.FillColor;
            };
        }
    }
}