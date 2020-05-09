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