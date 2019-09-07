using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    public sealed partial class ViewScaleTouchbarSlider : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;

        //@Converter
        private int NumberConverter(float scale) => ViewScaleConverter.ScaleToNumber(scale);
        private double ValueConverter(float scale) => ViewScaleConverter.ScaleToValue(scale);

        //@Construct
        public ViewScaleTouchbarSlider()
        {
            this.InitializeComponent();

            //Number
            this.TouchbarSlider.Unit = "%";
            this.TouchbarSlider.NumberMinimum = ViewScaleConverter.MinNumber;
            this.TouchbarSlider.NumberMaximum = ViewScaleConverter.MaxNumber;
            this.TouchbarSlider.NumberChange += (sender, number) =>
            {
                float scale= ViewScaleConverter.NumberToScale(number);
                this.Change(scale);
            };

            //Value
            this.TouchbarSlider.Minimum = ViewScaleConverter.MinValue;
            this.TouchbarSlider.Maximum = ViewScaleConverter.MaxValue;
            this.TouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.TouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float scale = ViewScaleConverter.ValueToScale(value);
                this.Change(scale);
            };
            this.TouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }

        public void Change(float scale)
        {
            this.ViewModel.Text = scale.ToString();
            //CanvasTransformer
            this.ViewModel.CanvasTransformer.Scale = scale;
            this.ViewModel.CanvasTransformer.ReloadMatrix();

            this.ViewModel.NotifyCanvasTransformerScale();//Notify
            this.ViewModel.Invalidate();//Invalidate
        }

    }
}