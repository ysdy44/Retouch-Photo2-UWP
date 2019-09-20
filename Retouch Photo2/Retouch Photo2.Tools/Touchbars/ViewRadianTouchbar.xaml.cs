using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Touchbars
{
    public sealed partial class ViewRadianTouchbar : UserControl, ITouchbar
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;

        public TouchbarType Type => TouchbarType.ViewRadian;
        public UIElement Self => this;

        //@Converter
        private int NumberConverter(float radian) => (int)ViewRadianConverter.RadianToNumber(radian);
        private double ValueConverter(float radian) => ViewRadianConverter.RadianToValue(radian);

        //@Construct
        public ViewRadianTouchbar()
        {
            this.InitializeComponent();

            //Number
            this.TouchbarSlider.Unit = "º";
            this.TouchbarSlider.NumberMinimum = ViewRadianConverter.MinNumber;
            this.TouchbarSlider.NumberMaximum = ViewRadianConverter.MaxNumber;
            this.TouchbarSlider.NumberChange += (sender, number) =>
            {
                float radian = ViewRadianConverter.NumberToRadian(number);
                this.ViewModel.SetCanvasTransformerRadian(radian);//CanvasTransformer
            };

            //Value
            this.TouchbarSlider.Minimum = ViewRadianConverter.MinValue;
            this.TouchbarSlider.Maximum = ViewRadianConverter.MaxValue;
            this.TouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.TouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float radian = ViewRadianConverter.ValueToRadian(value);
                this.ViewModel.SetCanvasTransformerRadian(radian);//CanvasTransformer
            };
            this.TouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }
    }
}