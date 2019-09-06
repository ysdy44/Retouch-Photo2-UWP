using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages.ViewPages
{
    public sealed partial class RadianTouchbarSlider : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
      
        //@Converter
        private double RadianToValue(float radian) => ViewConverter.RadianToValue(radian);

        //@Construct
        public RadianTouchbarSlider()
        {
            this.InitializeComponent();

            this.TouchbarSlider.Minimum = ViewConverter.RadianMinimum;
            this.TouchbarSlider.Maximum = ViewConverter.RadianMaximum;
            this.TouchbarSlider.Value = ViewConverter.RadianToValue(this.ViewModel.CanvasTransformer.Radian);
            this.TouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.TouchbarSlider.ValueChangeDelta += (sender, value) => this.ChangeRadian(value);
            this.TouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            this.TouchbarSlider.ValueChange += (sender, value) => this.ChangeRadian(value);
        }

        private void ChangeRadian(double value)
        {
            //CanvasTransformer
            this.ViewModel.CanvasTransformer.Radian = ViewConverter.ValueToRadian(value);
            this.ViewModel.CanvasTransformer.ReloadMatrix();

            this.ViewModel.NotifyCanvasTransformerRadian();//Notify
            this.ViewModel.Invalidate();//Invalidate
        }

    }
}