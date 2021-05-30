// Core:              ★★
// Referenced:   ★
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s ColorMatchAdjustment.
    /// </summary>
    public class ColorMatchAdjustment : IAdjustment
    {

        //@Content
        public AdjustmentType Type => AdjustmentType.ColorMatch;
        public Visibility PageVisibility => Visibility.Visible;

        public ICommand Edit { get; private set; }
        public ICommand Remove { get; private set; }

        public ColorMatchAdjustment()
        {
            this.Edit = new AdjustmentCommand(() => AdjustmentCommand.Edit(this));
            this.Remove = new AdjustmentCommand(() => AdjustmentCommand.Remove(this));
        }

        /// <summary> Source color. </summary>
        public Color SourceColor = Colors.White;
        public Color StartingSourceColor { get; private set; }
        public void CacheSourceColor() => this.StartingSourceColor = this.SourceColor;

        /// <summary> Destination color. </summary>
        public Color DestinationColor = Color.FromArgb(0, 0, 0, 0);
        public Color StartingDestinationColor { get; private set; }
        public void CacheDestinationColor() => this.StartingDestinationColor = this.DestinationColor;

        public IAdjustment Clone()
        {
            return new ColorMatchAdjustment
            {
                SourceColor = this.SourceColor,
                DestinationColor = this.DestinationColor
            };
        }


        public void SaveWith(XElement element)
        {
            element.Add(FanKit.Transformers.XML.SaveColor("SourceColor", this.SourceColor));
            element.Add(FanKit.Transformers.XML.SaveColor("DestinationColor", this.DestinationColor));
        }
        public void Load(XElement element)
        {
            if (element.Element("SourceColor") is XElement sourceColor) this.SourceColor = FanKit.Transformers.XML.LoadColor(sourceColor);
            if (element.Element("DestinationColor") is XElement destinationColor) this.DestinationColor = FanKit.Transformers.XML.LoadColor(destinationColor);
        }


        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new ColorMatrixEffect
            {
                Source = image,
                ColorMatrix = AdjustmentExtensions.GetColorMatching(this.SourceColor, this.DestinationColor)
            };
        }

    }
}