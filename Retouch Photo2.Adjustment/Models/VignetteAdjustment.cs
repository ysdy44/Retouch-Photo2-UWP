using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Pages;
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
        //@Static
        public static readonly VignettePage VignettePage = new VignettePage();

        //@Content
        public AdjustmentType Type => AdjustmentType.Vignette;
        public FrameworkElement Icon { get; } = new VignetteIcon();
        public Visibility PageVisibility => Visibility.Visible;
        public IAdjustmentPage Page => VignetteAdjustment.VignettePage;
        public string Text { get; private set; }

        /// <summary> Specifies the size of the vignette region as a percentage of the full image. </summary>
        public float Amount = 0.0f;
        /// <summary> Specifies how quickly the vignette color bleeds in over the region being faded. </summary>
        public float Curve = 0.0f;
        /// <summary> Specifies the color to fade toward. Default value black. </summary>
        public Color Color = Colors.Black;


        //@Construct
        /// <summary>
        /// Construct a Vignette-adjustment.
        /// </summary>
        public VignetteAdjustment()
        {
            this.Text = VignetteAdjustment.VignettePage.Text;
        }


        public void Reset()
        {
            this.Amount = 0.0f;
            this.Curve = 0.0f;
            this.Color = Colors.Black;

            if (VignetteAdjustment.VignettePage.Adjustment == this)
            {
                VignetteAdjustment.VignettePage.Follow(this);
            }
        }
        public void Follow()
        {
            VignetteAdjustment.VignettePage.Adjustment = this;
            VignetteAdjustment.VignettePage.Follow(this);
        }
        public void Close()
        {
            VignetteAdjustment.VignettePage.Adjustment = null;
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