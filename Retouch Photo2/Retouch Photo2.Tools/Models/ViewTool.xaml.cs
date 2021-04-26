// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Retouch_Photo2.Tools.Models
{
    internal enum ViewMode
    {
        None,

        Radian,

        Scale,
    }

    /// <summary>
    /// <see cref="ITool"/>'s ViewTool.
    /// </summary>
    public partial class ViewTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;

        private string ScaleToStringConverter(float scale) => $"{this.ScaleToNumberConverter(scale)}%";
        private int ScaleToNumberConverter(float scale) => ViewScaleConverter.ScaleToNumber(scale);
        private double ScaleToValueConverter(float scale) => ViewScaleConverter.ScaleToValue(scale);

        private string RadianToStringConverter(float radian) => $"{this.RadianToNumberConverter(radian)}º";
        private int RadianToNumberConverter(float radian) => ViewRadianConverter.RadianToNumber(radian);
        private double RadianToValueConverter(float radian) => ViewRadianConverter.RadianToValue(radian);


        //@Content 
        public ToolType Type => ToolType.View;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }


        #region DependencyProperty


        /// <summary> Gets or sets the radian. </summary>
        public double Radian
        {
            get => (double)base.GetValue(RadianProperty);
            set => base.SetValue(RadianProperty, value);
        }
        /// <summary> Identifies the <see cref = "ViewTool.Radian" /> dependency property. </summary>
        public static readonly DependencyProperty RadianProperty = DependencyProperty.Register(nameof(Radian), typeof(double), typeof(ViewTool), new PropertyMetadata(0.0f, (sender, e) =>
        {
            ViewTool control = (ViewTool)sender;

            if (e.NewValue is double value)
            {
                control.ViewModel.SetCanvasTransformerRadian((float)value);//CanvasTransformer
                control.ViewModel.Invalidate(InvalidateMode.None);//Invalidate
            }
        }));


        /// <summary> Gets or sets the scale. </summary>
        public double Scale
        {
            get => (double)base.GetValue(ScaleProperty);
            set => base.SetValue(ScaleProperty, value);
        }
        /// <summary> Identifies the <see cref = "ViewTool.Scale" /> dependency property. </summary>
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(nameof(Scale), typeof(double), typeof(ViewTool), new PropertyMetadata(0.0f, (sender, e) =>
        {
            ViewTool control = (ViewTool)sender;

            if (e.NewValue is double value)
            {
                control.ViewModel.SetCanvasTransformerScale((float)value);//CanvasTransformer
                control.ViewModel.Invalidate(InvalidateMode.None);//Invalidate
            }
        }));


        /// <summary> Gets or sets <see cref = "ViewTool" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "ViewTool.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(ViewTool), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a ViewTool. 
        /// </summary>
        public ViewTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.RadianButton.Tapped += (s, e) => TouchbarExtension.Instance = this.RadianButton;
            this.ConstructRadianStoryboard();
            this.ConstructRadian1();
            this.ConstructRadian2();

            this.ScaleButton.Tapped += (s, e) => TouchbarExtension.Instance = this.ScaleButton;
            this.ConstructScaleStoryboard();
            this.ConstructScale1();
            this.ConstructScale2();
        }


        public void Started(Vector2 startingPoint, Vector2 point)
        {
            //Tip
            this.ViewModel.SetTipTextPosition();
            this.ViewModel.TipTextVisibility = Visibility.Visible;

            this.ViewModel.CanvasTransformer.CacheMove(startingPoint);
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

            CoreCursorExtension.Hand_Is = true;//CoreCursorType
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            //Tip
            this.ViewModel.SetTipTextPosition();

            this.ViewModel.CanvasTransformer.Move(point);
            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            //Tip
            this.ViewModel.TipTextVisibility = Visibility.Collapsed;

            if (isOutNodeDistance) this.ViewModel.CanvasTransformer.Move(point);
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate

            CoreCursorExtension.Hand_Is = false;//CoreCursorType
        }
        public void Clicke(Vector2 point) { }

        public void Cursor(Vector2 point) { }

        public void Draw(CanvasDrawingSession drawingSession) { }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            TouchbarExtension.Instance = null;
        }

    }


    public partial class ViewTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.RadianTextBlock.Text = resource.GetString("Tools_View_Radian");
            this.RadianClearToolTip.Content = resource.GetString("Tools_View_RadianClear");

            this.ScaleTextBlock.Text = resource.GetString("Tools_View_Scale");
            this.ScaleClearToolTip.Content = resource.GetString("Tools_View_ScaleClear");
        }


        private void ConstructRadianStoryboard()
        {
            // Binding own DependencyProperty to the Storyboard
            Storyboard.SetTarget(this.RadianKeyFrames, this);
            Storyboard.SetTargetProperty(this.RadianKeyFrames, "Radian");
            this.RadianStoryboard.Completed += (s, e) => this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            this.RadianClearButton.Tapped += (s, e) =>
            {
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                this.Radian = this.ViewModel.CanvasTransformer.Radian;
                this.RadianStoryboard.Begin();//Storyboard
            };
        }

        private void ConstructRadian1()
        {
            this.RadianPicker.Minimum = ViewRadianConverter.MinNumber;
            this.RadianPicker.Maximum = ViewRadianConverter.MaxNumber;
            this.RadianPicker.ValueChanged += (sender, value) =>
            {
                float radian = ViewRadianConverter.NumberToRadian((int)value);
                this.ViewModel.SetCanvasTransformerRadian(radian);//CanvasTransformer
                this.ViewModel.Invalidate();//Invalidate
            };
        }

        private void ConstructRadian2()
        {
            this.RadianSlider.Minimum = ViewRadianConverter.MinValue;
            this.RadianSlider.Maximum = ViewRadianConverter.MaxValue;
            this.RadianSlider.ValueChangeStarted += (sender, value) => this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            this.RadianSlider.ValueChangeDelta += (sender, value) =>
            {
                float radian = ViewRadianConverter.ValueToRadian(value);
                this.ViewModel.SetCanvasTransformerRadian(radian);//CanvasTransformer
                this.ViewModel.Invalidate(InvalidateMode.None);//Invalidate
            };
            this.RadianSlider.ValueChangeCompleted += (sender, value) => this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }


        private void ConstructScaleStoryboard()
        {
            //  Binding own DependencyProperty to the Storyboard
            Storyboard.SetTarget(this.ScaleKeyFrames, this);
            Storyboard.SetTargetProperty(this.ScaleKeyFrames, "Scale");
            this.ScaleStoryboard.Completed += (s, e) => this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            this.ScaleClearButton.Tapped += (s, e) =>
            {
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                this.Scale = this.ViewModel.CanvasTransformer.Scale;
                this.ScaleStoryboard.Begin();//Storyboard
            };
        }

        private void ConstructScale1()
        {
            this.ScalePicker.Minimum = ViewScaleConverter.MinNumber;
            this.ScalePicker.Maximum = ViewScaleConverter.MaxNumber;
            this.ScalePicker.ValueChanged += (sender, value) =>
            {
                float scale = ViewScaleConverter.NumberToScale((int)value);
                this.ViewModel.SetCanvasTransformerScale(scale);//CanvasTransformer
                this.ViewModel.Invalidate();//Invalidate
            };
        }

        private void ConstructScale2()
        {
            this.ScaleSlider.Minimum = ViewScaleConverter.MinValue;
            this.ScaleSlider.Maximum = ViewScaleConverter.MaxValue;
            this.ScaleSlider.ValueChangeStarted += (sender, value) => this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            this.ScaleSlider.ValueChangeDelta += (sender, value) =>
            {
                float scale = ViewScaleConverter.ValueToScale(value);
                this.ViewModel.SetCanvasTransformerScale(scale);//CanvasTransformer
                this.ViewModel.Invalidate(InvalidateMode.None);//Invalidate
            };
            this.ScaleSlider.ValueChangeCompleted += (sender, value) => this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }

    }
}