using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Touchbars
{
    public sealed partial class AcrylicTintOpacityTouchbar : UserControl, ITouchbar
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        public TouchbarType Type => TouchbarType.AcrylicTintOpacity;
        public UserControl Self => this;

        //@Converter
        private int NumberConverter(float tintOpacity) => (int)(tintOpacity * 100d);
        private double ValueConverter(float tintOpacity) => tintOpacity * 100d;

        //@Construct
        public AcrylicTintOpacityTouchbar()
        {
            this.InitializeComponent();

            //Number
            this.TouchbarSlider.Unit = "%";
            this.TouchbarSlider.NumberMinimum = 0;
            this.TouchbarSlider.NumberMaximum = 90;
            this.TouchbarSlider.NumberChange += (sender, number) =>
            {
                float tintOpacity = number / 100f;
                this.Change(tintOpacity);
            };

            //Value
            this.TouchbarSlider.Minimum = 0d;
            this.TouchbarSlider.Maximum = 90d;
            this.TouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.TouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float tintOpacity = (float)(value / 100d);
                this.Change(tintOpacity);
            };
            this.TouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }

        private void Change(float opacity)
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