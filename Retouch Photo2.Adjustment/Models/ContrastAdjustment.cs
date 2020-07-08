using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
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
        //@Generic
        public static IAdjustmentPage GenericPage;// = new ContrastPage();
        
        //@Content
        public AdjustmentType Type => AdjustmentType.Contrast;
        public Visibility PageVisibility => Visibility.Visible;
        public IAdjustmentPage Page { get; } = ContrastAdjustment.GenericPage;
        public string Text => ContrastAdjustment.GenericPage.Text;


        /// <summary> Amount by which to adjust the contrast of the image. Default value 0,  -1 -> 1. </summary>
        public float Contrast = 0.0f;
        public float StartingContrast { get; private set; }
        public void CacheContrast() => this.StartingContrast = this.Contrast;


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