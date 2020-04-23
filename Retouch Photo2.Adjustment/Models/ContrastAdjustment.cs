using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Pages;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s ContrastAdjustment.
    /// </summary>
    public class ContrastAdjustment : IAdjustment
    {
        //@Static
        public static readonly ContrastPage ContrastPage = new ContrastPage();

        //@Content
        public AdjustmentType Type => AdjustmentType.Contrast;
        public FrameworkElement Icon { get; } = new ContrastIcon();
        public Visibility PageVisibility => Visibility.Visible;
        public IAdjustmentPage Page => ContrastAdjustment.ContrastPage;
        public string Text { get; private set; }

        /// <summary> Amount by which to adjust the contrast of the image. Default value 0,  -1 -> 1. </summary>
        public float Contrast = 0.0f;


        //@Construct
        /// <summary>
        /// Construct a contrast-adjustment.
        /// </summary>
        public ContrastAdjustment()
        {
            this.Text = ContrastAdjustment.ContrastPage.Text;
        }


        public void Reset()
        {
            this.Contrast = 0.0f;

            if (ContrastAdjustment.ContrastPage.Adjustment == this)
            {
                ContrastAdjustment.ContrastPage.Follow(this);
            }
        }
        public void Follow()
        {
            ContrastAdjustment.ContrastPage.Adjustment = this;
            ContrastAdjustment.ContrastPage.Follow(this);
        }
        public void Close()
        {
            ContrastAdjustment.ContrastPage.Adjustment = null;
        }

        
        public IAdjustment Clone()
        {
            return new ContrastAdjustment
            {
                Contrast = this.Contrast,
            };
        }


        public void SaveWith(XElement element)
        {
            element.Add(new XAttribute("Contrast", this.Contrast));
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