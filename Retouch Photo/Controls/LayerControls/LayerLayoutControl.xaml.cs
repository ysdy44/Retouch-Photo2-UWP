using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Controls.LayerControls
{
    public sealed partial class LayerLayoutControl : UserControl
    {


        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;


        #region DependencyProperty

        public Layer Layer
        {
            get { return (Layer)GetValue(LayerProperty); }
            set { SetValue(LayerProperty, value); }
        }
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(nameof(Layer), typeof(Layer), typeof(LayerLayoutControl), new PropertyMetadata(null, (sender,e) =>
        {
            LayerLayoutControl con = (LayerLayoutControl)sender;

            if (e.NewValue is Layer layer)
            {
                con.CheckBox.IsChecked = layer.IsVisual;

                layer.CanvasControl = con.CanvasControl;
                layer.CanvasControl .Draw+=(sender2, args) => layer.ThumbnailDraw(sender2, args.DrawingSession, sender2.Size);

                con.TextBlock.Text = layer.Name;
            }
        }));

        #endregion

        //Delegate
        public delegate void FlyoutShowHandler(UserControl control,Layer layer,bool isShow);
        public event FlyoutShowHandler FlyoutShow = null;


        public LayerLayoutControl()
        {
            this.InitializeComponent();
        }


        //Flyout
        private void Grid_Holding(object sender, HoldingRoutedEventArgs e) => this.FlyoutShow?.Invoke(this, this.Layer,true);
        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e) => this.FlyoutShow?.Invoke(this, this.Layer,true);
        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)=> this.FlyoutShow?.Invoke(this, this.Layer,false);
        



        private void CheckBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;

            if (this.Layer == null) return;

            this.Layer.IsVisual = this.CheckBox.IsChecked??false;
            this.ViewModel.Invalidate();
        }
        

    }
}
