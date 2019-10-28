using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s ContrastAdjustment.
    /// </summary>
    public class ContrastAdjustment : IAdjustment
    {

        public AdjustmentType Type => AdjustmentType.Contrast;
        public FrameworkElement Icon { get; } = new ContrastIcon();
        public Visibility PageVisibility => Visibility.Visible;

        /// <summary> Amount by which to adjust the contrast of the image. Default value 0,  -1 -> 1. </summary>
        public float Contrast = 0.0f;


        //@Construct
        /// <summary>
        /// Construct a contrast-adjustment.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public ContrastAdjustment(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a contrast-adjustment.
        /// </summary>
        public ContrastAdjustment()
        {
        }


        public void Reset()
        {
            this.Contrast = 0.0f;
        }
        public IAdjustment Clone()
        {
            return new ContrastAdjustment
            {
                Contrast = this.Contrast,
            };
        }

        public XElement Save()
        {
            return new XElement
            (
                "Contrast",
                new XAttribute("Contrast", this.Contrast)
            );
        }
        public void Load(XElement element)
        {
            this.Contrast = (float)element.Attribute("Contrast");
        }


        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new ContrastEffect
            {
                Contrast = this.Contrast,
                Source = image
            };
        }

    }
}