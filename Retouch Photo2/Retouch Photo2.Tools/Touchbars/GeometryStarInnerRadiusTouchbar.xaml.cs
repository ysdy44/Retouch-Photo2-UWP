using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Touchbars
{
    public sealed partial class GeometryStarInnerRadiusTouchbar : UserControl, ITouchbar
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        public TouchbarType Type => TouchbarType.GeometryStarInnerRadius;
        public UIElement Self => this;

        //@Converter
        private int NumberConverter(float innerRadius) => (int)(innerRadius * 100d);
        private double ValueConverter(float innerRadius) => innerRadius * 100d;

        //@Construct
        public GeometryStarInnerRadiusTouchbar()
        {
            this.InitializeComponent();

            //Number
            this.TouchbarSlider.Unit = "%";
            this.TouchbarSlider.NumberMinimum = 0;
            this.TouchbarSlider.NumberMaximum = 100;
            this.TouchbarSlider.NumberChange += (sender, number) =>
            {
                float innerRadius = number / 100f;
                this.Change(innerRadius);
            };

            //Value
            this.TouchbarSlider.Minimum = 0d;
            this.TouchbarSlider.Maximum = 100d;
            this.TouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.TouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float innerRadius = (float)(value / 100d);
                this.Change(innerRadius);
            };
            this.TouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }

        private void Change(float innerRadius)
        {
            this.SelectionViewModel.GeometryStarInnerRadius = innerRadius;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryStarLayer geometryStarLayer)
                {
                    geometryStarLayer.InnerRadius = innerRadius;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }
    }
}