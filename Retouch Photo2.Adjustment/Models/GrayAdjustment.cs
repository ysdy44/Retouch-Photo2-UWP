// Core:              ★★
// Referenced:   ★
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s GrayAdjustment.
    /// </summary>
    public class GrayAdjustment : IAdjustment
    {

        //@Content
        public AdjustmentType Type => AdjustmentType.Gray;
        public Visibility PageVisibility => Visibility.Collapsed;

        public ICommand Edit { get; private set; }
        public ICommand Remove { get; private set; }

        public GrayAdjustment()
        {
            this.Edit = new AdjustmentCommand(() => AdjustmentCommand.Edit(this));
            this.Remove = new AdjustmentCommand(() => AdjustmentCommand.Remove(this));
        }


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