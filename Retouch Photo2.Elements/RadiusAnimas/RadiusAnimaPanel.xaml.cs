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