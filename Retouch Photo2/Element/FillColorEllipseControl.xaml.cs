using Retouch_Photo2.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Element
{
    public sealed partial class FillColorEllipseControl : UserControl
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        public FillColorEllipseControl()
        {
            this.InitializeComponent();
            
            this.ColorButton.Tapped += (s, e) =>
            {
                this.ColorFlyout.ShowAt(this.ColorButton);
                this.ColorPicker.Color = this.ViewModel.Color;
            };
            this.ColorPicker.ColorChange += (s, value) =>
            {
                this.ViewModel.Color = value;

                Layer layer = this.ViewModel.Layer;
                if (layer != null)
                {
                    layer.ColorChanged(value);
                    this.ViewModel.Invalidate();
                }
            };
        }
    }
}
