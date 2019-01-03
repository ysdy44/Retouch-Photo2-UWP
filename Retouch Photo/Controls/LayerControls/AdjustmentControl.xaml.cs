using Retouch_Photo.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Controls.LayerControls
{
    public sealed partial class AdjustmentControl : UserControl
    {

        #region DependencyProperty

        public Adjustment Adjustment
        {
            get { return (Adjustment)GetValue(AdjustmentProperty); }
            set { SetValue(AdjustmentProperty, value); }
        }
        public static readonly DependencyProperty AdjustmentProperty = DependencyProperty.Register(nameof(Adjustment), typeof(Adjustment), typeof(AdjustmentControl), new PropertyMetadata(null));

        #endregion

        
        //Delegate
        public event AdjustmentHandler AdjustmentContext = null;
        public event AdjustmentHandler AdjustmentRemove = null;

         
        public AdjustmentControl()
        {
            this.InitializeComponent();
        }

        //Converter
        public Visibility TrueToVisibilityConverter(bool hasPage) => hasPage ? Visibility.Visible : Visibility.Collapsed;


        private void AdjustmentButton_Tapped(object sender, TappedRoutedEventArgs e) => this.AdjustmentContext?.Invoke(this.Adjustment);
        private void RemoveButton_Tapped(object sender, TappedRoutedEventArgs e) => this.AdjustmentRemove?.Invoke(this.Adjustment);

    }
}
