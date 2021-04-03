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
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s HueRotationAdjustment.
    /// </summary>
    public class HueRotationAdjustment : IAdjustment
    {
        //@Static
        //@Generic
        public static string GenericText = "HueRotation";
        public static ControlTemplate GenericIcon;
        public static IAdjustmentPage GenericPage;// = new HueRotationPage();

        //@Content
        public AdjustmentType Type => AdjustmentType.HueRotation;
        public Visibility PageVisibility => Visibility.Visible;
        public IAdjustmentPage Page => HueRotationAdjustment.GenericPage;
        public ControlTemplate Icon => HueRotationAdjustment.GenericIcon;
        public string Title => HueRotationAdjustment.GenericText; 
        
        public ICommand Edit { get; private set; }
        public ICommand Remove { get; private set; }

        public HueRotationAdjustment()
        {
            this.Edit = new AdjustmentCommand(() => AdjustmentCommand.Edit(this));
            this.Remove = new AdjustmentCommand(() => AdjustmentCommand.Remove(this));
        }


        /// <summary> Angle to rotate the hue, in radians. Default value 0, range 0 to 2*pi. </summary>
        public float Angle = 0.0f;
        public float StartingAngle { get; private set; }
        public void CacheAngle() => this.StartingAngle = this.Angle;

               
        public IAdjustment Clone()
        {
            return new HueRotationAdjustment
            {
                Angle = this.Angle,
            };
        }


        public void SaveWith(XElement element)
        {
            element.Add(new XAttribute("Angle", this.Angle));
        }
        public void Load(XElement element)
        {
            if (element.Attribute("Angle") is XAttribute angle) this.Angle = (float)angle;
        }


        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new HueRotationEffect
            {
                Angle = this.Angle,
                Source = image
            };
        }

    }
}