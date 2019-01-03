using Retouch_Photo.Models.Adjustments;
using Retouch_Photo.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo.Pages.AdjustmentPages
{
    public sealed partial class ContrastPage : Page
    {

        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;

        #region DependencyProperty

        public ContrastAdjustment ContrastAdjustment
        {
            get { return (ContrastAdjustment)GetValue(ContrastAdjustmentProperty); }
            set { SetValue(ContrastAdjustmentProperty, value); }
        }
        public static readonly DependencyProperty ContrastAdjustmentProperty = DependencyProperty.Register(nameof(ContrastAdjustment), typeof(ContrastAdjustment), typeof(ContrastAdjustment), new PropertyMetadata(null, (sender, e) =>
        {
            ContrastPage con = (ContrastPage)sender;

            if (e.NewValue is ContrastAdjustment adjustment)
            {
                con.ContrastSlider.Value = adjustment.Contrast * 100;
            }
        }));

        #endregion


        public ContrastPage()
        {
            this.InitializeComponent();
        }

        private void ContrastSlider_ValueChangeDelta(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.ContrastAdjustment.Contrast = (float)(e.NewValue / 100);
            this.ViewModel.Invalidate();
        }
    }
}

