using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    public sealed partial class AcrylicBlurAmountTouchbarSlider : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Converter
        private int NumberConverter(float blurAmount) => (int)blurAmount;
        private double ValueConverter(float blurAmount) => blurAmount ;
         
        //@Construct
        public AcrylicBlurAmountTouchbarSlider()
        {
            this.InitializeComponent();

            //Number
            this.TouchbarSlider.Unit = "dp";
            this.TouchbarSlider.NumberMinimum = 10;
            this.TouchbarSlider.NumberMaximum = 100;
            this.TouchbarSlider.NumberChange += (sender, number) =>
            {
                float amount = number;
                this.Change(amount);
            };

            //Value
            this.TouchbarSlider.Minimum = 10d;
            this.TouchbarSlider.Maximum = 100d;
            this.TouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.TouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float amount = (float)value;
            this.Change(amount);
            };
            this.TouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }

        private void Change(float amount)
        {
            if (amount < 10.0f) amount = 10.0f;
            if (amount > 100.0f) amount = 100.0f;

            this.SelectionViewModel.AcrylicBlurAmount = amount;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is AcrylicLayer acrylicLayer)
                {
                    acrylicLayer.BlurAmount = amount;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }
    }
}