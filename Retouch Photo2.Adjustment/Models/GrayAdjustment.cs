using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Pages;
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
        public static readonly GrayPage GrayPage = new GrayPage();

        //@Content
        public AdjustmentType Type => AdjustmentType.Gray;
        public FrameworkElement Icon { get; } = new GrayIcon();
        public Visibility PageVisibility => Visibility.Collapsed;
        public IAdjustmentPage Page => GrayAdjustment.GrayPage;
        public string Text { get; private set; }
        

        //@Construct
        /// <summary>
        /// Construct a Gray-adjustment.
        /// </summary>
        public GrayAdjustment()
        {
            this.Text = GrayAdjustment.GrayPage.Text;
        }


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