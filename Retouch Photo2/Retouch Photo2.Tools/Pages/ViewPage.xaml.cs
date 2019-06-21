using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Tips;
using System;
using Windows.UI.Xaml.Controls;


namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// <see cref="ViewTool"/>'s Page.
    /// </summary>
    public sealed partial class ViewPage : Page
    {
        //@ViewModel
        public ViewModel ViewModel => Retouch_Photo2.App.ViewModel;
        public TipViewModel TipViewModel => Retouch_Photo2.App.TipViewModel;
        
        //@Converter
        /*         
        [1] y = x * π / 180

        [2] -π<y<π , -180<x<180
        [3] y=1 , x=-9

        [4] value is x , radian is y
        [5] radian = value * π / 180
        [6] value = radian * 180 / π
        */
        private const double RadianDefult = 0;
        private const double RadianMinimum = -180;
        private const double RadianMaximum = 180;
        private double RadianToValue(float radian) => radian * 180.0 / Math.PI;
        private float ValueToRadian(double value) => (float)(value * Math.PI / 180.0);
        private string RadianToString(float radian) => ((int)((radian * 180.0 / Math.PI))).ToString()+ "º";


        /*         
        [1] y = b/(c-x)

        [2] b = 10 , c = 1
        [3] 0.1<y<10 , -99<x<0
        [4] y=1 , x=-9

        [5] value is x , scale is y
        [6] scale = 10 / (1 - value)
        [7] value = 1 - 10 / scale
        */
        private const double ScaleDefult = -9;
        private const double ScaleMinimum = -99;
        private const double ScaleMaximum = 0;
        private double ScaleToValue(float scale) => 1 - 10 / scale;
        private float ValueToScale(double value) => (float)(10 / (1 - value));
        private string ScaleToString(float scale) => ((int)(scale * 100)).ToString()+"%";


        //@Construct
        public ViewPage()
        {
            this.InitializeComponent();

            //Radian
            this.RadianFrame.Value = ViewPage.RadianDefult;
            this.RadianSlider.Maximum = ViewPage.RadianMaximum;
            this.RadianSlider.Minimum = ViewPage.RadianMinimum;
            this.RadianSlider.ValueChanged += (s, e) =>
            {
                float radian = this.ValueToRadian(e.NewValue);
                this.RadianTextBlock.SetValue(TextBlock.TextProperty, this.RadianToString(radian));//DependencyObject

                //CanvasTransformer
                this.ViewModel.CanvasTransformer.Radian = this.ValueToRadian(e.NewValue);
                this.ViewModel.CanvasTransformer.ReloadMatrix();

                this.ViewModel.Invalidate();//Invalidate
            };
            this.RadianButton.Tapped += (s, e) => this.RadianStoryboard.Begin();


            // Scale
            this.ScaleFrame.Value = ViewPage.ScaleDefult;
            this.ScaleSlider.Maximum = ViewPage.ScaleMaximum;
            this.ScaleSlider.Minimum = ViewPage.ScaleMinimum;
            this.ScaleSlider.ValueChanged += (s, e) =>
            {
                float scale = this.ValueToScale(e.NewValue);
                this.ScaleTextBlock.SetValue(TextBlock.TextProperty, this.ScaleToString(scale));//DependencyObject

                //CanvasTransformer
                this.ViewModel.CanvasTransformer.Scale = scale;
                this.ViewModel.CanvasTransformer.ReloadMatrix();

                this.ViewModel.Invalidate();//Invalidate
            };
            this.ScaleButton.Tapped += (s, e) => this.ScaleStoryboard.Begin();
        }
    }
}
