using Retouch_Photo.Models;
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

    /// <summary>
    /// The converter of Angle and radian .
    /// </summary>
    public static class ViewConverter
    {

        /*         
        [1] y = x * π / 180

        [2] -π<y<π , -180<x<180
        [3] y=1 , x=-9

        [4] value is x , radian is y
        [5] radian = value * π / 180
        [6] value = radian * 180 / π
        */
        public static double RadianDefult = 0;
        public static double RadianMinimum = -180;
        public static double RadianMaximum = 180;

        public static double RadianToValue(float radian) => radian * 180.0 / Math.PI;
        public static float ValueToRadian(double value) => (float)(value * Math.PI / 180.0);


        /*         
        [1] y = b/(c-x)

        [2] b = 10 , c = 1
        [3] 0.1<y<10 , -99<x<0
        [4] y=1 , x=-9

        [5] value is x , scale is y
        [6] scale = 10 / (1 - value)
        [7] value = 1 - 10 / scale
        */
        public static readonly double ScaleDefult = -9;
        public static readonly double ScaleMinimum = -99;
        public static readonly double ScaleMaximum = 0;

        public static double ScaleToValue(float scale) => 1 - 10 / scale;
        public static float ValueToScale(double value) => (float)(10 / (1 - value));

    }

    public sealed partial class ViewPage : ToolPage
    {
        //ViewModel
        public DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;
        
        public ViewPage()
        {
            this.InitializeComponent();

            //Radian
            this.RadianFrame.Value = ViewConverter.RadianDefult;
            this.RadianSlider.Value = ViewConverter.RadianToValue(this.ViewModel.MatrixTransformer.Radian);
            this.RadianSlider.Maximum = ViewConverter.RadianMaximum;
            this.RadianSlider.Minimum = ViewConverter.RadianMinimum;
            this.RadianSlider.ValueChanged += RadianSlider_ValueChanged;

            // Scale
            this.ScaleFrame.Value = ViewConverter.ScaleDefult;
            this.ScaleSlider.Value = ViewConverter.ScaleToValue(this.ViewModel.MatrixTransformer.Scale);
            this.ScaleSlider.Maximum = ViewConverter.ScaleMaximum;
            this.ScaleSlider.Minimum = ViewConverter.ScaleMinimum;
            this.ScaleSlider.ValueChanged += ScaleSlider_ValueChanged;
        }

        //@Override
        public override void ToolOnNavigatedTo()//当前页面成为活动页面
        {
        }
        public override void ToolOnNavigatedFrom()//当前页面不再成为活动页面
        {
        }


        //Radian
        private void RadianButton_Tapped(object sender, TappedRoutedEventArgs e)=> this.RadianStoryboard.Begin();
        private void RadianSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.ViewModel.MatrixTransformer.Radian = ViewConverter.ValueToRadian(e.NewValue);
            this.ViewModel.Invalidate();
        }

        // Scale
        private void ScaleButton_Tapped(object sender, TappedRoutedEventArgs e)=>  this.ScaleStoryboard.Begin();
        private void ScaleSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.ViewModel.MatrixTransformer.Scale = ViewConverter.ValueToScale(e.NewValue);
            this.ViewModel.Invalidate();
        }
                    

    }
}
