using Retouch_Photo2.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Element
{
    public sealed partial class FillColorControl : UserControl
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        public FillColorControl()
        {
            this.InitializeComponent();
            
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
