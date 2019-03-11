using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Adjustments
{
    public sealed partial class Control : UserControl
    {
        #region DependencyProperty

        public Adjustment Adjustment
        {
            get { return (Adjustment)GetValue(AdjustmentProperty); }
            set { SetValue(AdjustmentProperty, value); }
        }
        public static readonly DependencyProperty AdjustmentProperty = DependencyProperty.Register(nameof(Adjustment), typeof(Adjustment), typeof(Control), new PropertyMetadata(null));

        #endregion


        //Delegate
        public event AdjustmentHandler Remove = null;
        public event AdjustmentHandler Edit = null;


        public Control()
        {
            this.InitializeComponent();
        }

        //Converter
        public Visibility AdjustmentVisibilityConverter(Adjustment adjustment)
        {
            if (adjustment == null) return Visibility.Collapsed;

            return adjustment.HasPage? Visibility.Visible: Visibility.Collapsed;
        }
        public object AdjustmentNullConverter(Adjustment adjustment)
        {
            if (adjustment == null) return null;

            return adjustment.Icon;
        }


        
        private void AdjustmentButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Edit?.Invoke(this.Adjustment);
        private void RemoveButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Remove?.Invoke(this.Adjustment);


    }
}
