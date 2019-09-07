using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    public sealed partial class ViewRadianTouchbarSlider : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;

        //@Converter
        private int NumberConverter(float radian) => (int)ViewRadianConverter.RadianToNumber(radian);
        private double ValueConverter(float radian) => ViewRadianConverter.RadianToValue(radian);
        
        //@Construct
        public ViewRadianTouchbarSlider()
        {
            this.InitializeComponent();
            
            //Number
            this.TouchbarSlider.Unit = "º";
            this.TouchbarSlider.NumberMinimum = ViewRadianConverter.MinNumber;
            this.TouchbarSlider.NumberMaximum = ViewRadianConverter.MaxNumber;
            this.TouchbarSlider.NumberChange += (sender, number) =>
            {
                float radian = ViewRadianConverter.NumberToRadian(number);
                this.Change(radian);
            };

            //Value
            this.TouchbarSlider.Minimum = ViewRadianConverter.MinValue;
            this.TouchbarSlider.Maximum = ViewRadianConverter.MaxValue;
            this.TouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.TouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float radian = ViewRadianConverter.ValueToRadian(value);
                this.Change(radian);
            };
            this.TouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }

        public void Change(float radian)
        {
            //CanvasTransformer
            this.ViewModel.CanvasTransformer.Radian = radian;
            this.ViewModel.CanvasTransformer.ReloadMatrix();

            this.ViewModel.NotifyCanvasTransformerRadian();//Notify
            this.ViewModel.Invalidate();//Invalidate
        }

    }
}