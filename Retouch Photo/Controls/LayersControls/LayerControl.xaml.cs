using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo.Controls.LayersControls
{
    public sealed partial class LayerControl : UserControl
    {

        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;


        #region DependencyProperty

        public Layer Layer
        {
            get { return (Layer)GetValue(LayerProperty); }
            set { SetValue(LayerProperty, value); }
        }
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(nameof(Layer), typeof(Layer), typeof(LayerControl), new PropertyMetadata(null, (sender, e) =>
        {
            LayerControl con = (LayerControl)sender;

            if (e.NewValue is Layer layer)
            {
                con.IsEnabled = true;
            }
            else
            {
                con.IsEnabled = false;
            }
        }));

        #endregion


        public LayerControl()
        {
            this.InitializeComponent();

            //Control
            this.Slider.ValueChanged += (object sender, RangeBaseValueChangedEventArgs e) => this.ViewModel.Invalidate();
            this.BlendControl.IndexChanged += (int index) => this.ViewModel.Invalidate();

            //Button
            this.AdjustmentButton.Tapped += (sender, e) => { };
            this.EffectButton.Tapped += (sender, e) => { };
            this.RemoveButton.Tapped += (sender, e) =>
            {
                this.ViewModel.RenderLayer.Remove(this.Layer);
                this.ViewModel.CurrentLayer = null;
                this.Layer = null;
            };
        }

    }
}
