using Retouch_Photo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo.Pages.ToolPages
{ 

    public sealed partial class ToolViewPage : Page
    {
        //ViewModel
        public DrawViewModel ViewModel;

        #region Radian & Scale       

        /*         
        [1] y = x * π / 180

        [2] -π<y<π , -180<x<180
        [3] y=1 , x=-9

        [4] value is x , radian is y
        [5] radian = value * π / 180
        [6] value = radian * 180 / π
        */
        public readonly double RadianDefult = 0;
        public readonly double RadianMinimum = -180;
        public readonly double RadianMaximum = 180;

        public double RadianToValue(float radian) => radian * 180.0 / Math.PI;
        public float ValueToRadian(double value) => (float)(value * Math.PI / 180.0);


        /*         
        [1] y = b/(c-x)

        [2] b = 10 , c = 1
        [3] 0.1<y<10 , -99<x<0
        [4] y=1 , x=-9

        [5] value is x , scale is y
        [6] scale = 10 / (1 - value)
        [7] value = 1 - 10 / scale
        */
        public readonly double ScaleDefult = -9;
        public readonly double ScaleMinimum = -99;
        public readonly double ScaleMaximum = 0;

        public double ScaleToValue(float scale) => 1 - 10 / scale;
        public float ValueToScale(double value) => (float)(10 / (1 - value));
  

        #endregion


        public ToolViewPage()
        {
            this.InitializeComponent();

            //ViewModel
            this.ViewModel = App.ViewModel;

            //Radian
            this.RadianFrame.Value = this.RadianDefult;
            this.RadianSlider.Value = this.RadianToValue(this.ViewModel.MatrixTransformer.Radian);
            this.RadianSlider.Maximum = this.RadianMaximum;
            this.RadianSlider.Minimum = this.RadianMinimum;
            this.RadianSlider.ValueChanged += RadianSlider_ValueChanged;

            // Scale
            this.ScaleFrame.Value = this.ScaleDefult;
            this.ScaleSlider.Value = this.ScaleToValue(this.ViewModel.MatrixTransformer.Scale);
            this.ScaleSlider.Maximum = this.ScaleMaximum;
            this.ScaleSlider.Minimum = this.ScaleMinimum;
            this.ScaleSlider.ValueChanged += ScaleSlider_ValueChanged;
        }


        //Radian
        private void RadianButton_Tapped(object sender, TappedRoutedEventArgs e)=> this.RadianStoryboard.Begin();
        private void RadianSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (this.ViewModel == null) return;
            if (this.ViewModel.CanvasControl == null) return;

            this.ViewModel.MatrixTransformer.Radian = this.ValueToRadian(e.NewValue);
            this.ViewModel.Invalidate();
        }

        // Scale
        private void ScaleButton_Tapped(object sender, TappedRoutedEventArgs e)=>  this.ScaleStoryboard.Begin();
        private void ScaleSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (this.ViewModel == null) return;
            if (this.ViewModel.CanvasControl == null) return;

            this.ViewModel.MatrixTransformer.Scale = this.ValueToScale(e.NewValue);
            this.ViewModel.Invalidate();
        }
                    

    }
}
