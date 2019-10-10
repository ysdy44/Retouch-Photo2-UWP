using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Touchbars
{
    public sealed partial class GeometryTriangleCenterTouchbar : UserControl, ITouchbar
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        public TouchbarType Type => TouchbarType.GeometryTriangleCenter;
        public UIElement Self => this;

        //@Converter
        private int NumberConverter(float center) => (int)(center * 100d);
        private double ValueConverter(float center) => center * 100d;

        //@Construct
        public GeometryTriangleCenterTouchbar()
        {
            this.InitializeComponent();

            //Number
            this.TouchbarSlider.Unit = "%";
            this.TouchbarSlider.NumberMinimum = 0;
            this.TouchbarSlider.NumberMaximum = 100;
            this.TouchbarSlider.NumberChange += (sender, number) =>
            {
                float center = number / 100.0f;
                this.Change(center);
            };

            //Value
            this.TouchbarSlider.Minimum = 0d;
            this.TouchbarSlider.Maximum = 100d;
            this.TouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.TouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float center = (float)value / 100.0f;
                this.Change(center);
            };
            this.TouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }

        private void Change(float center)
        {
            if (center < 0.0f) center = 0.0f;
            if (center > 1.0f) center = 1.0f;

            this.SelectionViewModel.GeometryTriangleCenter = center;
            
            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryTriangleLayer   geometryTriangleLayer)
                {
                    geometryTriangleLayer.Center = center;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }
    }
}
