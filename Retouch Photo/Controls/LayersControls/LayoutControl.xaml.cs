using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Controls.LayersControls
{
    public sealed partial class LayoutControl : UserControl
    {


        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;


        #region DependencyProperty

        public  Layer Layer
        {
            get { return (Layer)GetValue(LayerProperty); }
            set { SetValue(LayerProperty, value); }
        }
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(nameof(Layer), typeof(Layer), typeof(LayoutControl), new PropertyMetadata(null, (sender,e) =>
        {
            LayoutControl con = (LayoutControl)sender;

            if (e.NewValue is Layer layer)
            {
                con.CheckBox.IsChecked = layer.IsVisual;
                con.SecondBorder.Child = layer.Icon;                
                con.TextBlock.Text = layer.Name;
            }
        }));

        #endregion

        //Delegate
        public delegate void FlyoutShowHandler(UserControl control,Layer layer,bool isShow);
        public event FlyoutShowHandler FlyoutShow = null;


        public LayoutControl()
        {
            this.InitializeComponent();

            //Flyout
            this.BackgroundGrid.Holding += (sender, e) => this.FlyoutShow?.Invoke(this, this.Layer, true);
            this.BackgroundGrid.RightTapped += (sender, e) => this.FlyoutShow?.Invoke(this, this.Layer, true);
            this.BackgroundGrid.Tapped += (sender, e) => this.FlyoutShow?.Invoke(this, this.Layer, false);
        }
               

        private void CheckBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;

            if (this.Layer == null) return;

            this.Layer.IsVisual = this.CheckBox.IsChecked??false;
            this.ViewModel.Invalidate();
        }
        
    }
}
