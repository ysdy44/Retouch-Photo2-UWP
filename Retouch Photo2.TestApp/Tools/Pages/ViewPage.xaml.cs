using Retouch_Photo2.TestApp.Tools.Models;
using Retouch_Photo2.TestApp.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2.TestApp.Tools.Pages
{ 
    /// <summary>
    /// <see cref="ViewTool"/>'s Page.
    /// </summary>
    public sealed partial class ViewPage : Page
    {
        //ViewModel
        public ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;

        //@Converter
        /*         
        [1] y = x * π / 180

        [2] -π<y<π , -180<x<180
        [3] y=1 , x=-9

        [4] value is x , radian is y
        [5] radian = value * π / 180
        [6] value = radian * 180 / π
        */
        public const double RadianDefult = 0;
        public const double RadianMinimum = -180;
        public const double RadianMaximum = 180;
        private double RadianToValue(float radian) => radian * 180.0 / Math.PI;
        private float ValueToRadian(double value) => (float)(value * Math.PI / 180.0);


        /*         
        [1] y = b/(c-x)

        [2] b = 10 , c = 1
        [3] 0.1<y<10 , -99<x<0
        [4] y=1 , x=-9

        [5] value is x , scale is y
        [6] scale = 10 / (1 - value)
        [7] value = 1 - 10 / scale
        */
        public const double ScaleDefult = -9;
        public const double ScaleMinimum = -99;
        public const double ScaleMaximum = 0;
        private double ScaleToValue(float scale) => 1 - 10 / scale;
        private float ValueToScale(double value) => (float)(10 / (1 - value));
        

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
                this.ViewModel.CanvasTransformer.Radian = this.ValueToRadian(e.NewValue);
                this.ViewModel.CanvasTransformer.ReloadMatrix();
                this.ViewModel.Invalidate();
            };
            this.RadianButton.Tapped += (s, e) => this.RadianStoryboard.Begin();


            // Scale
            this.ScaleFrame.Value = ViewPage.ScaleDefult;
            this.ScaleSlider.Maximum = ViewPage.ScaleMaximum;
            this.ScaleSlider.Minimum = ViewPage.ScaleMinimum;
            this.ScaleSlider.ValueChanged += (s, e) =>
            {
                this.ViewModel.CanvasTransformer.Scale = this.ValueToScale(e.NewValue);
                this.ViewModel.CanvasTransformer.ReloadMatrix();
                this.ViewModel.Invalidate();
            };
            this.ScaleButton.Tapped += (s, e) => this.ScaleStoryboard.Begin();
        }


        //The current page becomes the active page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Radian 
            this.RadianSlider.Value = this.RadianToValue(this.ViewModel.CanvasTransformer.Radian);

            // Scale 
            this.ScaleSlider.Value = this.ScaleToValue(this.ViewModel.CanvasTransformer.Scale);
        }
        //The current page no longer becomes an active page
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }
    }
}
