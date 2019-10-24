using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Retouch_Photo2.Tools.Pages
{
    internal enum ViewMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Radian. </summary>
        Radian,

        /// <summary> Scale. </summary>
        Scale,
    }

    /// <summary>
    /// Page of <see cref = "ViewTool"/>.
    /// </summary>
    public sealed partial class ViewPage : Page, IToolPage
    {   
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@Content
        public FrameworkElement Self => this;
        public bool IsSelected { private get; set; }
        ViewMode _mode
        {
            set
            {
                switch (value)
                {
                    case ViewMode.None:
                        this.RadianTouchbarButton.IsSelected = false;
                        this.ScaleTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null;
                        break;
                    case ViewMode.Radian:
                        this.RadianTouchbarButton.IsSelected = true;
                        this.ScaleTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = this.RadianTouchbarSlider;
                        break;
                    case ViewMode.Scale:
                        this.RadianTouchbarButton.IsSelected = false;
                        this.ScaleTouchbarButton.IsSelected = true;
                        this.TipViewModel.TouchbarControl = this.ScaleTouchbarSlider;
                        break;
                }
            }
        }

        //@Converter
        private int RadianNumberConverter(float radian) => ViewRadianConverter.RadianToNumber(radian);
        private double RadianValueConverter(float radian) => ViewRadianConverter.RadianToValue(radian);

        private int ScaleNumberConverter(float scale) => ViewScaleConverter.ScaleToNumber(scale);
        private double ScaleValueConverter(float scale) => ViewScaleConverter.ScaleToValue(scale);

        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsSelected;


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
                con.ViewModel.SetCanvasTransformerRadian((float)value);//CanvasTransformer
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
                con.ViewModel.SetCanvasTransformerScale((float)value);//CanvasTransformer
            }
        }));


        #endregion


        //@Construct
        public ViewPage()
        {
            this.InitializeComponent();

            //Radian: 
            //  Binding own DependencyProperty to the Storyboard
            Storyboard.SetTarget(this.RadianKeyFrames, this);
            this.RadianStoryboard.Completed += (s, e) =>
            {
                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
            this.RadianClearButton.Tapped += (s, e) =>
            {
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                this.Radian = this.ViewModel.CanvasTransformer.Radian;
                this.RadianStoryboard.Begin();//Storyboard
            };

            //Scale: 
            //  Binding own DependencyProperty to the Storyboard
            Storyboard.SetTarget(this.ScaleKeyFrames, this);
            this.ScaleStoryboard.Completed += (s, e) =>
            {
                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
            this.ScaleClearButton.Tapped += (s, e) =>
            {
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                this.Scale = this.ViewModel.CanvasTransformer.Scale;
                this.ScaleStoryboard.Begin();//Storyboard
            };

            //Radian
            {
                //Button
                this.RadianTouchbarButton.Unit = "º";
                this.RadianTouchbarButton.Toggle += (s, value) =>
                {
                    if (value)
                        this._mode = ViewMode.Radian;
                    else
                        this._mode = ViewMode.None;
                };

                //Number
                this.RadianTouchbarSlider.Unit = "º";
                this.RadianTouchbarSlider.NumberMinimum = ViewRadianConverter.MinNumber;
                this.RadianTouchbarSlider.NumberMaximum = ViewRadianConverter.MaxNumber;
                this.RadianTouchbarSlider.NumberChange += (sender, number) =>
                {
                    float radian = ViewRadianConverter.NumberToRadian(number);
                    this.ViewModel.SetCanvasTransformerRadian(radian);//CanvasTransformer
                };

                //Value
                this.RadianTouchbarSlider.Minimum = ViewRadianConverter.MinValue;
                this.RadianTouchbarSlider.Maximum = ViewRadianConverter.MaxValue;
                this.RadianTouchbarSlider.ValueChangeStarted += (sender, value) => { };
                this.RadianTouchbarSlider.ValueChangeDelta += (sender, value) =>
                {
                    float radian = ViewRadianConverter.ValueToRadian(value);
                    this.ViewModel.SetCanvasTransformerRadian(radian);//CanvasTransformer
                };
                this.RadianTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            }

            //Scale
            {
                //Button
                this.ScaleTouchbarButton.Unit = "%";
                this.ScaleTouchbarButton.Toggle += (s, value) =>
                {
                    if (value)
                        this._mode = ViewMode.Scale;
                    else
                        this._mode = ViewMode.None;
                };

                //Number
                this.ScaleTouchbarSlider.Unit = "%";
                this.ScaleTouchbarSlider.NumberMinimum = ViewScaleConverter.MinNumber;
                this.ScaleTouchbarSlider.NumberMaximum = ViewScaleConverter.MaxNumber;
                this.ScaleTouchbarSlider.NumberChange += (sender, number) =>
                {
                    float scale = ViewScaleConverter.NumberToScale(number);
                    this.ViewModel.SetCanvasTransformerScale(scale);//CanvasTransformer
                };

                //Value
                this.ScaleTouchbarSlider.Minimum = ViewScaleConverter.MinValue;
                this.ScaleTouchbarSlider.Maximum = ViewScaleConverter.MaxValue;
                this.ScaleTouchbarSlider.ValueChangeStarted += (sender, value) => { };
                this.ScaleTouchbarSlider.ValueChangeDelta += (sender, value) =>
                {
                    float scale = ViewScaleConverter.ValueToScale(value);
                    this.ViewModel.SetCanvasTransformerScale(scale);//CanvasTransformer
                };
                this.ScaleTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            }
        }
        
        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this._mode = ViewMode.None;
        }
    }
}