using Microsoft.Graphics.Canvas;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo.Adjustments
{
    /// <summary>
    /// 传递Adjustment的委托。
    /// </summary>
    public delegate void AdjustmentHandler(Adjustment adjustment);
    public delegate void AdjustmentsHandler(IEnumerable<Adjustment> adjustments);
    
    /// <summary>
    /// Adjustment: 调整。
    /// 给图层提供调整。
    /// </summary>
    public abstract class Adjustment
    {
        //delegate
        public delegate void VoidCall();
        public static event VoidCall InvalidateCall = null;
        public static void Invalidate() => Adjustment.InvalidateCall?.Invoke();


        public AdjustmentType Type;
        public FrameworkElement Icon;

        public AdjustmentItem Item;        
        /// <summary> 有无参数页面 </summary>
         public bool HasPage;

        /// <summary> 重置参数 </summary>
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
