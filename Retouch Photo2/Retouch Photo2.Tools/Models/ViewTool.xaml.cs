using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// Enum of <see cref="ViewTool"/>.
    /// </summary>
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
    /// <see cref="ITool"/>'s ViewTool.
    /// </summary>
    public partial class ViewTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        
        //@TouchBar
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
        private int ScaleNumberConverter(float scale) => ViewScaleConverter.ScaleToNumber(scale);
        private double ScaleValueConverter(float scale) => ViewScaleConverter.ScaleToValue(scale);

        private int RadianNumberConverter(float radian) => ViewRadianConverter.RadianToNumber(radian);
        private double RadianValueConverter(float radian) => ViewRadianConverter.RadianToValue(radian);
        
        #region DependencyProperty


        /// <summary> Gets or sets radian. </summary>
        public double Radian
        {
            get { return (double)GetValue(RadianProperty); }
            set { SetValue(RadianProperty, value); }
        }
        /// <summary> Identifies the <see cref = "ViewTool.Radian" /> dependency property. </summary>
        public static readonly DependencyProperty RadianProperty = DependencyProperty.Register(nameof(Radian), typeof(double), typeof(ViewTool), new PropertyMetadata(0.0f, (sender, e) =>
        {
            ViewTool con = (ViewTool)sender;

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
        /// <summary> Identifies the <see cref = "ViewTool.Scale" /> dependency property. </summary>
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(nameof(Scale), typeof(double), typeof(ViewTool), new PropertyMetadata(0.0f, (sender, e) =>
        {
            ViewTool con = (ViewTool)sender;

            if (e.NewValue is double value)
            {
                con.ViewModel.SetCanvasTransformerScale((float)value);//CanvasTransformer
            }
        }));


        #endregion
        

        //@Construct
        public ViewTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructToolTip();

            this.ConstructRadianStoryboard();
            this.ConstructRadian();

            this.ConstructScaleStoryboard();
            this.ConstructScale();
        }


        private void ConstructRadianStoryboard()
        {
            // Binding own DependencyProperty to the Storyboard
            Storyboard.SetTarget(this.RadianKeyFrames, this);
            Storyboard.SetTargetProperty(this.RadianKeyFrames, "Radian");
            this.RadianStoryboard.Completed += (s, e) => this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            this.RadianClearButton.Click += (s, e) =>
            {
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                this.Radian = this.ViewModel.CanvasTransformer.Radian;
                this.RadianStoryboard.Begin();//Storyboard
            };
        }
        private void ConstructRadian()
        {
            //Button
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
            this.RadianTouchbarSlider.ValueChanged += (sender, value) =>
            {
                float radian = ViewRadianConverter.NumberToRadian((int)value);
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


        private void ConstructScaleStoryboard()
        {
            //  Binding own DependencyProperty to the Storyboard
            Storyboard.SetTarget(this.ScaleKeyFrames, this);
            Storyboard.SetTargetProperty(this.ScaleKeyFrames, "Scale");
            this.ScaleStoryboard.Completed += (s, e) => this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            this.ScaleClearButton.Click += (s, e) =>
            {
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                this.Scale = this.ViewModel.CanvasTransformer.Scale;
                this.ScaleStoryboard.Begin();//Storyboard
            };
        }
        private void ConstructScale()
        {
            //Button
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
            this.ScaleTouchbarSlider.ValueChanged += (sender, value) =>
            {
                float scale = ViewScaleConverter.NumberToScale((int)value);
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

        
        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this._mode = ViewMode.None;
        }

    }
    
    /// <summary>
    /// <see cref="ITool"/>'s ViewTool.
    /// </summary>
    public partial class ViewTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content =
                this.Title = resource.GetString("/Tools/View");

            this.RadianTouchbarButton.CenterContent = resource.GetString("/Tools/View_Radian");
            this.RadianClearToolTip.Content = resource.GetString("/Tools/View_RadianClear");

            this.ScaleTouchbarButton.CenterContent = resource.GetString("/Tools/View_Scale");
            this.ScaleClearToolTip.Content = resource.GetString("/Tools/View_ScaleClear");
        }

        //ToolTip
        private void ConstructToolTip()
        {
            this._button.ToolTip.Opened += (s, e) =>
            {
                if (this.IsSelected)
                {
                    this.RadianClearToolTip.IsOpen = true;
                    this.ScaleClearToolTip.IsOpen = true;
                }
            };
            this._button.ToolTip.Closed += (s, e) =>
            {
                this.RadianClearToolTip.IsOpen = false;
                this.ScaleClearToolTip.IsOpen = false;
            };
        }

        
        //@Content
        public ToolType Type => ToolType.View;
        public string Title { get; set; }
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => this._button.IsSelected; set => this._button.IsSelected = value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new ViewIcon();
        readonly ToolButton _button = new ToolButton(new ViewIcon());


        public void Started(Vector2 startingPoint, Vector2 point)
        {
            //Text
            this.ViewModel.SetTipTextPosition();
            this.ViewModel.TipTextVisibility = Visibility.Visible;

            this.ViewModel.CanvasTransformer.CacheMove(startingPoint);
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            //Text
            this.ViewModel.SetTipTextPosition();

            this.ViewModel.CanvasTransformer.Move(point);
            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            //Text
            this.ViewModel.TipTextVisibility = Visibility.Collapsed;

            if (isOutNodeDistance) this.ViewModel.CanvasTransformer.Move(point);
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        public void Clicke(Vector2 point) => this.TipViewModel.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) { }
                              
    }
}