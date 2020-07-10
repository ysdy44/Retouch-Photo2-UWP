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
        /// <summary> ContentBorder's Content. </summary>
        public UIElement CenterContent { get => this.ContentBorder.Child; set => this.ContentBorder.Child = value; }

        //@Construct
        /// <summary>
        /// Initializes a RadiusAnimaPanel. 
        /// </summary>
        public RadiusAnimaPanel()
        {
            this.InitializeComponent();
            this.ContentBorder.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.Frame.Value = e.NewSize.Width + 8;
                this.Storyboard.Begin();
            };
        }
    }
}