using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Pages;
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
        public static readonly InvertPage InvertPage = new InvertPage();

        //@Content
        public AdjustmentType Type => AdjustmentType.Invert;
        public FrameworkElement Icon { get; } = new InvertIcon();
        public Visibility PageVisibility => Visibility.Collapsed;
        public IAdjustmentPage Page => InvertAdjustment.InvertPage;
        public string Text { get; private set; }


        //@Construct
        /// <summary>
        /// Initializes a Invert-adjustment.
        /// </summary>
        public InvertAdjustment()
        {
            this.Text = InvertAdjustment.InvertPage.Text;
        }


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