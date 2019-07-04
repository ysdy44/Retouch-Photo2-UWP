using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// The shadow panel of the control will also follow the animation, 
    /// if you change the width of the contents of the control.
    /// </summary>
    public sealed partial class RadiusAnimaPanel : UserControl
    {
        //@Content
        /// <summary> ContentPresenter's Content. </summary>
        public object CenterContent { get => this.ContentPresenter.Content; set => this.ContentPresenter.Content = value; }

        #region DependencyProperty


        /// <summary> Sets or Gets the shadow opacity. </summary>
        public double ShadowOpacity
        {
            get { return (double)GetValue(ShadowOpacityProperty); }
            set { SetValue(ShadowOpacityProperty, value); }
        }
        /// <summary> Identifies the <see cref = "RadiusAnimaPanel.ShadowOpacity" /> dependency property. </summary>
        public static readonly DependencyProperty ShadowOpacityProperty = DependencyProperty.Register(nameof(ShadowOpacity), typeof(double), typeof(RadiusAnimaPanel), new PropertyMetadata(0.3d));



        /// <summary> Sets or Gets the shadow color. </summary>
        public Color ShadowColor
        {
            get { return (Color)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }
        /// <summary> Identifies the <see cref = "RadiusAnimaPanel.ShadowColor" /> dependency property. </summary>
        public static readonly DependencyProperty ShadowColorProperty = DependencyProperty.Register(nameof(ShadowColor), typeof(Color), typeof(RadiusAnimaPanel), new PropertyMetadata(Colors.Black));
        

        #endregion

        //@Construct
        public RadiusAnimaPanel()
        {
            this.InitializeComponent();
            this.ContentPresenter.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.Frame.Value = e.NewSize.Width;
                this.Storyboard.Begin();
            };
        }
    }
}