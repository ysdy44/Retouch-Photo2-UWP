using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using System.Xml.Linq;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s VignetteAdjustment.
    /// </summary>
    public class VignetteAdjustment : IAdjustment
    {

        public AdjustmentType Type => AdjustmentType.Vignette;
        public FrameworkElement Icon { get; } = new VignetteIcon();
        public Visibility PageVisibility => Visibility.Visible;


        /// <summary> Specifies the size of the vignette region as a percentage of the full image. </summary>
        public float Amount = 0.0f;
        /// <summary> Specifies how quickly the vignette color bleeds in over the region being faded. </summary>
        public float Curve = 0.0f;
        /// <summary> Specifies the color to fade toward. Default value black. </summary>
        public Color Color = Colors.Black;


        //@Construct
        /// <summary>
        /// Construct a vignette-adjustment.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public VignetteAdjustment(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a vignette-adjustment.
        /// </summary>
        public VignetteAdjustment()
        {
        }


        public void Reset()
        {
            this.Amount = 0.0f;
            this.Curve = 0.0f;
            this.Color = Colors.Black;
        }
        public IAdjustment Clone()
        {
            return new VignetteAdjustment
            {
                Amount = this.Amount,
                Curve = this.Curve,
                Color = this.Color,
            };
        }

        public XElement Save()
        {
            return new XElement
            (
                "Vignette",
                new XAttribute("Amount", this.Amount),
                new XAttribute("Curve", this.Curve),
                FanKit.Transformers.XML.SaveColor("Color", this.Color)
            );
        }
        public void Load(XElement element)
        {
            this.Amount = (float)element.Attribute("Amount");
            this.Curve = (float)element.Attribute("Curve");
            this.Color = FanKit.Transformers.XML.LoadColor(element.Element("Color"));
        }

        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new VignetteEffect
            {
                Amount = this.Amount,
                Curve = this.Curve,
                Color = this.Color,
                Source = image
            };
        }

    }
}