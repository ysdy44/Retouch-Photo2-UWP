using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using System.Numerics;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s BrightnessAdjustment.
    /// </summary>
    public class BrightnessAdjustment : IAdjustment
    {

        public AdjustmentType Type => AdjustmentType.Brightness;
        public FrameworkElement Icon { get; } = new BrightnessIcon();
        public Visibility PageVisibility => Visibility.Visible;

        /// <summary> Interval 1.0->0.5 . </summary>
        public float WhiteLight = 1.0f;
        /// <summary> Interval 1.0->0.5 . </summary>
        public float WhiteDark = 1.0f;

        /// <summary> Interval 0.0->0.5 . </summary>
        public float BlackLight = 0.0f;
        /// <summary> Interval 0.0->0.5 . </summary>
        public float BlackDark = 0.0f;


        //@Construct
        /// <summary>
        /// Construct a brightness-adjustment.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public BrightnessAdjustment(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a brightness-adjustment.
        /// </summary>
        public BrightnessAdjustment()
        {
        }


        public void Reset()
        {
            this.WhiteLight = 1.0f;
            this.WhiteDark = 1.0f;
            this.BlackLight = 0.0f;
            this.BlackDark = 0.0f;
        }
        public IAdjustment Clone()
        {
            return new BrightnessAdjustment
            {
                WhiteLight = this.WhiteLight,
                WhiteDark = this.WhiteDark,
                BlackLight = this.BlackLight,
                BlackDark = this.BlackDark,
            };
        }

        public XElement Save()
        {
            return new XElement
             (
                "Brightness",
                new XAttribute("WhiteLight", this.WhiteLight),
                new XAttribute("WhiteDark", this.WhiteDark),
                new XAttribute("BlackLight", this.BlackLight),
                new XAttribute("BlackDark", this.BlackDark)
             );
        }
        public void Load(XElement element)
        {
            this.WhiteLight = (float)element.Attribute("WhiteLight");
            this.WhiteDark = (float)element.Attribute("WhiteDark");
            this.BlackLight = (float)element.Attribute("BlackLight");
            this.BlackDark = (float)element.Attribute("BlackDark");
        }

        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new BrightnessEffect
            {
                WhitePoint = new Vector2(this.WhiteLight, this.WhiteDark),
                BlackPoint = new Vector2(this.BlackDark, this.BlackLight),
                Source = image
            };
        }

    }
}