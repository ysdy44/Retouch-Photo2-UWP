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
    /// <see cref="IAdjustment"/>'s InvertAdjustment.
    /// </summary>
    public class InvertAdjustment : IAdjustment
    {

        //@Content
        public AdjustmentType Type => AdjustmentType.Invert;
        public Visibility PageVisibility => Visibility.Collapsed;

        public ICommand Edit { get; private set; }
        public ICommand Remove { get; private set; }

        public InvertAdjustment()
        {
            this.Edit = new AdjustmentCommand(() => AdjustmentCommand.Edit(this));
            this.Remove = new AdjustmentCommand(() => AdjustmentCommand.Remove(this));
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