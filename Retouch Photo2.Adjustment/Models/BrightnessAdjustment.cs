using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Pages;
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
        //@Static
        public static readonly BrightnessPage BrightnessPage = new BrightnessPage();

        //@Content
        public AdjustmentType Type => AdjustmentType.Brightness;
        public FrameworkElement Icon { get; } = new BrightnessIcon();
        public Visibility PageVisibility => Visibility.Visible;
        public IAdjustmentPage Page => BrightnessAdjustment.BrightnessPage;
        public string Text { get; private set; }

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
        /// Initializes a brightness-adjustment.
        /// </summary>
        public BrightnessAdjustment()
        {
            this.Text = BrightnessAdjustment.BrightnessPage.Text;
        }


        public void Reset()
        {
            this.WhiteLight = 1.0f;
            this.WhiteDark = 1.0f;
            this.BlackLight = 0.0f;
            this.BlackDark = 0.0f;

            if (BrightnessAdjustment.BrightnessPage.Adjustment==this)
            {
                BrightnessAdjustment.BrightnessPage.Follow(this);
            }
        }
        public void Follow()
        {
            BrightnessAdjustment.BrightnessPage.Adjustment = this;
            BrightnessAdjustment.BrightnessPage.Follow(this);
        }
        public void Close()
        {
            BrightnessAdjustment.BrightnessPage.Adjustment = null;
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


        public void SaveWith(XElement element)
        {
            element.Add(new XAttribute("WhiteLight", this.WhiteLight));
            element.Add(new XAttribute("WhiteDark", this.WhiteDark));
            element.Add(new XAttribute("BlackLight", this.BlackLight));
            element.Add(new XAttribute("BlackDark", this.BlackDark));
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