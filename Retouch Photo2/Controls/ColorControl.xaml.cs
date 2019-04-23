using Retouch_Photo2.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    public sealed partial class ColorControl : UserControl
    {

        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;


        #region DependencyProperty

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorControl), new PropertyMetadata(Colors.White));

        #endregion


        public ColorControl()
        {
            this.InitializeComponent();
        }


        private void ColorPicker_ColorChange(object sender, Color value)
        {
            this.ViewModel.Color = value;

            Layer layer = this.ViewModel.CurrentLayer;
            if (layer != null)
            {
                layer.ColorChanged(value);
                this.ViewModel.Invalidate();
            }
        }
    }
}
