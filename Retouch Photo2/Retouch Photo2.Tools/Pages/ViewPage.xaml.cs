using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
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

        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsSelected;
        public bool IsSelected { private get; set; }


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
            Storyboard.SetTarget(this.RadianKeyFrames, this);
            Storyboard.SetTarget(this.ScaleKeyFrames, this);

            //Radian
            this.RadianTouchbarButton.Unit = "º";
            this.RadianClearButton.Tapped += (s, e) =>
            {
                this.Radian = this.ViewModel.CanvasTransformer.Radian;
                this.RadianStoryboard.Begin();
            };

            //Scale
            this.ScaleTouchbarButton.Unit = "%";
            this.ScaleClearButton.Tapped += (s, e) =>
            {
                this.Scale = this.ViewModel.CanvasTransformer.Scale;
                this.ScaleStoryboard.Begin();
            };
        }
    }
}