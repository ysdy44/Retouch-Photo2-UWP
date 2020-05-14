using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Pages;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s SaturationAdjustment.
    /// </summary>
    public class SaturationAdjustment : IAdjustment
    {
        //@Static
        public static readonly SaturationPage SaturationPage = new SaturationPage();

        //@Content
        public AdjustmentType Type => AdjustmentType.Saturation;
        public FrameworkElement Icon { get; } = new SaturationIcon();
        public Visibility PageVisibility => Visibility.Visible;
        public IAdjustmentPage Page => SaturationAdjustment.SaturationPage;
        public string Text { get; private set; }

        /// <summary> Gets or sets the saturation intensity for effect. </summary>
        public float Saturation = 1.0f;


        //@Construct
        /// <summary>
        /// Initializes a Saturation-adjustment.
        /// </summary>
        public SaturationAdjustment()
        {
            this.Text = SaturationAdjustment.SaturationPage.Text;
        }


        public void Reset()
        {
            this.Saturation = 1.0f;

            if (SaturationAdjustment.SaturationPage.Adjustment == this)
            {
                SaturationAdjustment.SaturationPage.Follow(this);
            }
        }
        public void Follow()
        {
            SaturationAdjustment.SaturationPage.Adjustment = this;
            SaturationAdjustment.SaturationPage.Follow(this);
        }
        public void Close()
        {
            SaturationAdjustment.SaturationPage.Adjustment = null;
        }


        public IAdjustment Clone()
        {
            return new SaturationAdjustment
            {
                Saturation = this.Saturation,
            };
        }


        public void SaveWith(XElement element)
        {
            element.Add(new XAttribute("Saturation", this.Saturation));
        }
        public void Load(XElement element)
        {
            this.Saturation = (float)element.Attribute("Saturation");
        }


        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new SaturationEffect
            {
                Saturation = this.Saturation,
                Source = image
            };
        }

    }
}