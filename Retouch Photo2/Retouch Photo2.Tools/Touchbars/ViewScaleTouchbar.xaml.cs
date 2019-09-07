using Retouch_Photo2.Elements;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Touchbars
{
    public sealed partial class ViewScaleTouchbar : UserControl, ITouchbar
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;

        public TouchbarType Type => TouchbarType.ViewScale;
        public UserControl Self => this;

        //@Converter
        private int NumberConverter(float scale) => ViewScaleConverter.ScaleToNumber(scale);
        private double ValueConverter(float scale) => ViewScaleConverter.ScaleToValue(scale);

        //@Construct
        public ViewScaleTouchbar()
        {
            this.InitializeComponent();

            //Number
            this.TouchbarSlider.Unit = "%";
            this.TouchbarSlider.NumberMinimum = ViewScaleConverter.MinNumber;
            this.TouchbarSlider.NumberMaximum = ViewScaleConverter.MaxNumber;
            this.TouchbarSlider.NumberChange += (sender, number) =>
            {
                float scale = ViewScaleConverter.NumberToScale(number);
                this.ViewModel.SetCanvasTransformerScale(scale);//CanvasTransformer
            };

            //Value
            this.TouchbarSlider.Minimum = ViewScaleConverter.MinValue;
            this.TouchbarSlider.Maximum = ViewScaleConverter.MaxValue;
            this.TouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.TouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float scale = ViewScaleConverter.ValueToScale(value);
                this.ViewModel.SetCanvasTransformerScale(scale);//CanvasTransformer
            };
            this.TouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }
    }
}