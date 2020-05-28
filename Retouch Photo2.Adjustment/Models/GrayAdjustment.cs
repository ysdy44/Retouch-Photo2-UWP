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
        public static IAdjustmentGenericPage<GrayAdjustment> GenericPage;// = new GrayPage();
        
        //@Content
        public AdjustmentType Type => AdjustmentType.Gray;
        public Visibility PageVisibility => Visibility.Collapsed;
        public UIElement Page => GrayAdjustment.GenericPage.Self;
        public string Text => GrayAdjustment.GenericPage.Text;
        

        public void Reset() { }
        public void Follow() { }
        public void Close() { }


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