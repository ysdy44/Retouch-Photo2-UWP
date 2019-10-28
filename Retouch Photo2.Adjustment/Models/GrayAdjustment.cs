using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s GrayAdjustment.
    /// </summary>
    public class GrayAdjustment : IAdjustment
    {

        public AdjustmentType Type => AdjustmentType.Gray;
        public FrameworkElement Icon { get; } = new GrayIcon();
        public Visibility PageVisibility => Visibility.Collapsed;


        public void Reset() { }
        public IAdjustment Clone()
        {
            return new GrayAdjustment();
        }
        public XElement Save()
        {
            return new XElement
            (
                "Gray"
            );
        }

        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new GrayscaleEffect
            {
                Source = image
            };
        }

    }
}