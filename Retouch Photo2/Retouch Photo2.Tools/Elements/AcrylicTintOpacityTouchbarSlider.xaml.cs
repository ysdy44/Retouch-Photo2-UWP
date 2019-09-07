using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    public sealed partial class AcrylicTintOpacityTouchbarSlider : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Converter
        private int NumberConverter(float tintOpacity) => (int)(tintOpacity * 100d);
        private double ValueConverter(float tintOpacity) => tintOpacity * 100d;

        //@Construct
        public AcrylicTintOpacityTouchbarSlider()
        {
            this.InitializeComponent();

            //Number
            this.TouchbarSlider.Unit = "%";
            this.TouchbarSlider.NumberMinimum = 0;
            this.TouchbarSlider.NumberMaximum = 90;
            this.TouchbarSlider.NumberChange += (sender, number) => this.ValueChange(number / 100f);

            //Value
            this.TouchbarSlider.Minimum = 0d;
            this.TouchbarSlider.Maximum = 90d;
            this.TouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.TouchbarSlider.ValueChangeDelta += (sender, value) => this.ValueChange((float)(value / 100.0d));
            this.TouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }

        private void ValueChange(float opacity)
        { 
            this.SelectionViewModel.AcrylicTintOpacity = opacity;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is AcrylicLayer acrylicLayer)
                {
                    acrylicLayer.TintOpacity = opacity;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }
    }
}