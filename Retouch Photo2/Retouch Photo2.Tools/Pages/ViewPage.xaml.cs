using Retouch_Photo2.Tools.Elements;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Tips;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "ViewTool"/>.
    /// </summary>
    public sealed partial class ViewPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@Converter
        private int RadianNumberConverter(float radian) => ViewRadianConverter.RadianToNumber(radian);
        private int ScaleNumberConverter(float scale) => ViewScaleConverter.ScaleToNumber(scale);

        //@Touchbar
        ViewRadianTouchbarSlider _radianTouchbarSlider { get; } = new ViewRadianTouchbarSlider();
        ViewScaleTouchbarSlider _scaleTouchbarSlider { get; } = new ViewScaleTouchbarSlider();

        /// <summary> Type of ViewPage. </summary>
        public ViewToolType Type
        {
            set
            {
                switch (value)
                {
                    case ViewToolType.None:
                        {
                            this.RadianTouchbarButton.IsChecked = false;
                            this.ScaleTouchbarButton.IsChecked = false;
                            this.TipViewModel.Touchbar = null;//Touchbar
                        }
                        break;
                    case ViewToolType.Radian:
                        {
                            this.RadianTouchbarButton.IsChecked = true;
                            this.ScaleTouchbarButton.IsChecked = false;
                            this.TipViewModel.Touchbar = this._radianTouchbarSlider;//Touchbar
                        }
                        break;
                    case ViewToolType.Scale:
                        {
                            this.RadianTouchbarButton.IsChecked = false;
                            this.ScaleTouchbarButton.IsChecked = true;
                            this.TipViewModel.Touchbar = this._scaleTouchbarSlider;//Touchbar
                        }
                        break;
                }
            }
        }

        #region DependencyProperty
        
        /// <summary> Gets or sets radian. </summary>
        public double Radian
        {
            get { return (double)GetValue(RadianProperty); }
            set { SetValue(RadianProperty, value); }
        }
        /// <summary> Identifies the <see cref = "ViewPage.Radian" /> dependency property. </summary>
        public static readonly DependencyProperty RadianProperty = DependencyProperty.Register(nameof(Radian), typeof(double), typeof(ViewPage), new PropertyMetadata(0.0f, (sender, e) =>
        {
            ViewPage con = (ViewPage)sender;

            if (e.NewValue is double value)
            {
                con._radianTouchbarSlider.Change((float)value);
            }
        }));

        /// <summary> Gets or sets scale. </summary>
        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }
        /// <summary> Identifies the <see cref = "ViewPage.Scale" /> dependency property. </summary>
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(nameof(Scale), typeof(double), typeof(ViewPage), new PropertyMetadata(0.0f, (sender, e) =>
        {
            ViewPage con = (ViewPage)sender;

            if (e.NewValue is double value)
            {
                con._scaleTouchbarSlider.Change((float)value);
            }
        }));

        #endregion
        //@Construct
        public ViewPage()
        {
            this.InitializeComponent();
            
            //Radian
            this.RadianTouchbarButton.Tapped2 += (s, isChecked) =>
            {
                if (isChecked) this.Type = ViewToolType.None;
                else this.Type = ViewToolType.Radian;
            };
            this.RadianClearButton.Tapped += (s, e) =>
            {
                Storyboard.SetTarget(this.RadianKeyFrames, this);
                this.Radian = this.ViewModel.CanvasTransformer.Radian;
                this.RadianStoryboard.Begin();
            };

            //Scale
            this.ScaleTouchbarButton.Tapped2 += (s, isChecked) =>
            {
                if (isChecked) this.Type = ViewToolType.None;
                else this.Type = ViewToolType.Scale;
            };
            this.ScaleClearButton.Tapped += (s, e) =>
            {
                Storyboard.SetTarget(this.ScaleKeyFrames, this);
                this.Scale = this.ViewModel.CanvasTransformer.Radian;
                this.ScaleStoryboard.Begin();
            };
        }
    }
}