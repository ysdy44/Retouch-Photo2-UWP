using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Adjustments
{
    /// <summary>
    /// Retouch_Photo2 Adjustments 's control.
    /// </summary>
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


        //@Delegate
        public event AdjustmentHandler Remove = null;
        public event AdjustmentHandler Edit = null;


        //@Construct
        public Control()
        {
            this.InitializeComponent();

            this.EditButton.Tapped += (s, e) => this.Edit?.Invoke(this.Adjustment);
            this.RemoveButton.Tapped += (s, e) => this.Remove?.Invoke(this.Adjustment); 
        }

        //Converter
        public Visibility AdjustmentVisibilityConverter(Adjustment adjustment)
        {
            if (adjustment == null) return Visibility.Collapsed;

            return adjustment.HasPage ? Visibility.Visible : Visibility.Collapsed;
        }
        public object AdjustmentNullConverter(Adjustment adjustment)
        {
            if (adjustment == null) return null;

            return adjustment.Icon;
        }


   }
}
