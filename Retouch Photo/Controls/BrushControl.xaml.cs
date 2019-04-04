using Retouch_Photo.Models;
using Retouch_Photo.Models.Layers;
using Retouch_Photo.ViewModels;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Controls
{

    public sealed partial class BrushControl : UserControl
    {
        //ViewModel
        public DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;


        #region DependencyProperty

        public Layer Layer
        {
            get { return (Layer)GetValue(LayerProperty); }
            set { SetValue(LayerProperty, value); }
        }
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(nameof(Layer), typeof(Layer), typeof(BrushControl), new PropertyMetadata(null, (sender, e) =>
        {
            BrushControl con = (BrushControl)sender;

            if (e.NewValue is Layer value)
            {
                if (value is GeometryLayer geometryLayer)
                {



                }
            }
        }));

        #endregion


        public BrushControl()
        {
            this.InitializeComponent();
            this.ReserveButton.Tapped += (s, e) =>  this.OperatorControl.Reserve();
            this.RemoveButton.Tapped += (s, e) =>
            {
                this.SetControl(Colors.Transparent, 0, false);
                this.OperatorControl.Remove();
            };

            //Delegate
            this.OperatorControl.OffsetChanged += (offset) => this.NumberControl.Value = (int)(offset * 100);
            this.OperatorControl.StopChanged += (stop, isEnabled) => this.SetControl(stop.Color, (int)(stop.Position * 100), isEnabled);

            //Color            
            this.ColorPicker.ColorChange += (s, color) => this.SetColor(color);
            this.StrawPicker.ColorChange += (s, color) => this.SetColor(color);
            this.ColorButton.Tapped += (s, e) =>
            {
                this.ColorFlyout.ShowAt(this.ColorButton);
                this.ColorPicker.Color = this.SolidColorBrush.Color;
            };

            //Offset            
            this.NumberControl.ValueChange += (s, value) =>this.OperatorControl.SetOffset((float)value / 100.0f);
        }


        private void SetColor(Color color)
        {
            this.SolidColorBrush.Color = color;
            this.OperatorControl.SetColor(color);
        }
        private void SetControl(Color color, int offset, bool isEnabled)
        {
            this.SolidColorBrush.Color = color;
            this.NumberControl.Value = offset;
            this.RemoveButton.IsEnabled = this.NumberControl.IsEnabled = isEnabled;
        }
    }
}
