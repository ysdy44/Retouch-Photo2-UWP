using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s GrayAdjustment.
    /// </summary>
    public class GrayAdjustment : IAdjustment
    {
        //@Static
        //@Generic
        public static string GenericText = "Gray";
        public static IAdjustmentPage GenericPage;// = new GrayPage();
        
        //@Content
        public AdjustmentType Type => AdjustmentType.Gray;
        public Visibility PageVisibility => Visibility.Collapsed;
        public IAdjustmentPage Page { get; } = GrayAdjustment.GenericPage;
        public string Text => GrayAdjustment.GenericText;


        public IAdjustment Clone()
        {
            return new GrayAdjustment();
        }


        public void SaveWith(XElement element) { }
        public void Load(XElement element) { }


        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new GrayscaleEffect
            {
                Source = image
            };
        }

    }
}