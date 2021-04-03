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
    /// <see cref="IAdjustment"/>'s GrayAdjustment.
    /// </summary>
    public class GrayAdjustment : IAdjustment
    {
        //@Static
        //@Generic
        public static string GenericText = "Gray";
        public static ControlTemplate GenericIcon;
        public static IAdjustmentPage GenericPage;// = new GrayPage();
        
        //@Content
        public AdjustmentType Type => AdjustmentType.Gray;
        public Visibility PageVisibility => Visibility.Collapsed;
        public IAdjustmentPage Page => GrayAdjustment.GenericPage;
        public ControlTemplate Icon => GrayAdjustment.GenericIcon;
        public string Title => GrayAdjustment.GenericText;

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