// Core:              ★★
// Referenced:   ★
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
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
        //@Generic
        public static string GenericText = "Vignette";
        public static IAdjustmentPage GenericPage;// = new VignettePage();

        //@Content
        public AdjustmentType Type => AdjustmentType.Vignette;
        public Visibility PageVisibility => Visibility.Visible;
        public IAdjustmentPage Page { get; } = VignetteAdjustment.GenericPage;
        public string Text => VignetteAdjustment.GenericText;


        /// <summary> Specifies the size of the vignette region as a percentage of the full image. </summary>
        public float Amount = 0.0f;
        public float StartingAmount { get; private set; }
        public void CacheAmount() => this.StartingAmount = this.Amount;

        /// <summary> Specifies how quickly the vignette color bleeds in over the region being faded. </summary>
        public float Curve = 0.0f;
        public float StartingCurve { get; private set; }
        public void CacheCurve() => this.StartingCurve = this.Curve;

        /// <summary> Specifies the color to fade toward. Default value black. </summary>
        public Color Color = Colors.Black;
        public Color StartingColor { get; private set; }
        public void CacheColor() => this.StartingColor = this.Color;


        public IAdjustment Clone()
        {
            return new VignetteAdjustment
            {
                Amount = this.Amount,
                Curve = this.Curve,
                Color = this.Color,
            };
        }


        public void SaveWith(XElement element)
        {
            element.Add(new XAttribute("Amount", this.Amount));
            element.Add(new XAttribute("Curve", this.Curve));
            element.Add(FanKit.Transformers.XML.SaveColor("Color", this.Color));
        }
        public void Load(XElement element)
        {
            if (element.Element("Amount") is XElement amount) this.Amount = (float)amount;
            if (element.Element("Curve") is XElement curve) this.Curve = (float)curve;
            if (element.Element("Color") is XElement color) this.Color = FanKit.Transformers.XML.LoadColor(color);
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