using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    public sealed partial class StrokeWidthControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Converter
        private int NumberConverter(float strokeWidth) => (int)(strokeWidth * 100.0f);
        private double ValueConverter(float strokeWidth) => strokeWidth;

        //@Construct
        public StrokeWidthControl()
        {
            this.InitializeComponent();

            //Number
            this.TouchbarSlider.Unit = "%";
            this.TouchbarSlider.NumberMinimum = 0;
            this.TouchbarSlider.NumberMaximum = 10000;
            this.TouchbarSlider.NumberChange += (sender, value) =>
            {
                float strokeWidth = value / 100f;
                this.Change(strokeWidth);
            };

            //Value
            this.TouchbarSlider.Minimum = 0d;
            this.TouchbarSlider.Maximum = 100d;
            this.TouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.TouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float strokeWidth = (float)value;
                this.Change(strokeWidth);
            };
            this.TouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }

        private void Change(float strokeWidth)
        {
            //Selection
            this.SelectionViewModel.StrokeWidth = strokeWidth;
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is IGeometryLayer geometryLayer)
                {
                    geometryLayer.StrokeWidth = strokeWidth;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }
    }
}
