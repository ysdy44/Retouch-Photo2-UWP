using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// Represents the control that a drawer can be folded.
    /// </summary>
    public abstract partial class Expander : UserControl
    {

        //@Static
        /// <summary>
        /// A canvas, covered by a lot of <see cref="Expander"/>.
        /// </summary>
        public static readonly Canvas OverlayCanvas = new Canvas();
        /// <summary>
        /// True if lightweight elimination is enabled for <see cref="Expander.OverlayCanvas"/>.
        /// </summary>
        public static bool IsOverlayDismiss
        {
            set
            {
                if (value)
                    Expander.OverlayCanvas.Background = new SolidColorBrush(Colors.Transparent);
                else
                    Expander.OverlayCanvas.Background = null;
            }
        }


    }
}