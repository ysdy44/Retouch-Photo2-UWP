using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s InvertAdjustment.
    /// </summary>
    public class InvertAdjustment : IAdjustment
    {

        public AdjustmentType Type => AdjustmentType.Invert;
        public FrameworkElement Icon { get; } = new InvertIcon();
        public Visibility PageVisibility => Visibility.Visible;


        public void Reset() { }
        public IAdjustment Clone()
        {
            return new InvertAdjustment();
        }
        public XElement Save()
        {
            return new XElement
            (
                "Invert"
            );
        }

        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new InvertEffect
            {
                Source = image
            };
        }

    }
}