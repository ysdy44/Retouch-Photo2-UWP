using Microsoft.Graphics.Canvas;
using System.Collections.Generic;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.Storage;
using System.Threading.Tasks;

namespace Retouch_Photo.Adjustments
{
    public delegate void AdjustmentHandler(Adjustment adjustment);
    public delegate void AdjustmentsHandler(IEnumerable<Adjustment> adjustments);

    /// <summary> Adjust Layers. </summary>
    public abstract class Adjustment
    {
        //static
        public static Action Invalidate;

        public AdjustmentType Type;
        public FrameworkElement Icon;

        public AdjustmentItem Item;
        public bool HasPage;

        public abstract void Reset();
        public abstract ICanvasImage GetRender(ICanvasImage image);

        //@static
        public static ICanvasImage Render(List<Adjustment> adjustments, ICanvasImage image)
        {
            if (adjustments == null) return image;
            if (adjustments.Count == 0) return image;
            if (adjustments.Count == 1) return adjustments.Single().GetRender(image);

            foreach (var item in adjustments)
            {
                image = item.GetRender(image);
            }
            return image;
        }
    }
}
