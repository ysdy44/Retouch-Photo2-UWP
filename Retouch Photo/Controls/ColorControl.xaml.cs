using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Controls
{
    public sealed partial class ColorControl : UserControl
    {

        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;


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
                layer.Invalidate();
                this.ViewModel.Invalidate();
            }
        }
    }
}
