using Retouch_Photo2.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Controls.LayersControls
{
    public sealed partial class LayerControl : UserControl
    {

        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;


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
            this.Slider.ValueChanged += (sender, e) => this.ViewModel.Invalidate();
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
