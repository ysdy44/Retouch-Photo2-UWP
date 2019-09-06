using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages.ViewPages
{
    public sealed partial class ScaleTouchbarSlider : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@Converter
        private double ScaleToValue(float scale) => ViewConverter.ScaleToValue(scale);

        //@Construct
        public ScaleTouchbarSlider()
        {
            this.InitializeComponent();
            
            this.TouchbarSlider.Minimum = ViewConverter.ScaleMinimum;
            this.TouchbarSlider.Maximum = ViewConverter.ScaleMaximum;
            this.TouchbarSlider.Value = ViewConverter.ScaleToValue(this.ViewModel.CanvasTransformer.Scale);
            this.TouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.TouchbarSlider.ValueChangeDelta += (sender, value) => this.ChangeScale(value);
            this.TouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            this.TouchbarSlider.ValueChange += (sender, value) => this.ChangeScale(value);
        }

        private void ChangeScale(double value)
        {
            //CanvasTransformer
            this.ViewModel.CanvasTransformer.Scale = ViewConverter.ValueToScale(value);
            this.ViewModel.CanvasTransformer.ReloadMatrix();

            this.ViewModel.NotifyCanvasTransformerScale();//Notify
            this.ViewModel.Invalidate();//Invalidate
        }
    }
}