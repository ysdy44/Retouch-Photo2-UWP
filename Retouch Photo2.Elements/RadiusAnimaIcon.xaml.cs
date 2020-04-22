using Windows.Devices.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// The shadow icon of the control will also follow the animation, 
    /// if you change the width of the contents of the control.
    /// </summary>
    public sealed partial class RadiusAnimaIcon : UserControl
    {
        //@Delegate
        /// <summary> Occurs when a pointer enters the hit test area of this element. </summary>
        public event RoutedEventHandler Toggled;

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
        /// <summary> Identifies the <see cref = "RadiusAnimaIcon.ShadowOpacity" /> dependency property. </summary>
        public static readonly DependencyProperty ShadowOpacityProperty = DependencyProperty.Register(nameof(ShadowOpacity), typeof(double), typeof(RadiusAnimaIcon), new PropertyMetadata(0.3d));



        /// <summary> Sets or Gets the shadow color. </summary>
        public Color ShadowColor
        {
            get { return (Color)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }
        /// <summary> Identifies the <see cref = "RadiusAnimaIcon.ShadowColor" /> dependency property. </summary>
        public static readonly DependencyProperty ShadowColorProperty = DependencyProperty.Register(nameof(ShadowColor), typeof(Color), typeof(RadiusAnimaIcon), new PropertyMetadata(Colors.Black));


        #endregion
         

        //@Construct
        public RadiusAnimaIcon()
        {
            this.InitializeComponent();
            this.RootGrid.Tapped += (s, e) => this.Toggled?.Invoke(s, e);//Delegate
            this.RootGrid.PointerEntered += (s, e) =>
            {
                if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                {
                    this.Toggled?.Invoke(s, e);//Delegate
                }
            };
        }
    }
}