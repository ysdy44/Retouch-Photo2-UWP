using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s InvertAdjustment.
    /// </summary>
    public class InvertAdjustment : IAdjustment
    {
        //@Static
        //@Generic
        public static string GenericText = "Invert";
        public static IAdjustmentPage GenericPage;// = new InvertPage();

        //@Content
        public AdjustmentType Type => AdjustmentType.Invert;
        public Visibility PageVisibility => Visibility.Collapsed;
        public IAdjustmentPage Page { get; } = InvertAdjustment.GenericPage;
        public string Text => InvertAdjustment.GenericText;


        public void Reset() { }
        public void Follow() { }
        public void Close() { }


        public IAdjustment Clone()
        {
            return new InvertAdjustment();
        }


        public void SaveWith(XElement element) { }
        public void Load(XElement element) { }


        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new InvertEffect
            {
                Source = image
            };
        }

    }
}