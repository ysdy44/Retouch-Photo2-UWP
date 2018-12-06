using Retouch_Photo.ViewModels.ToolViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Retouch_Photo.Library.CanvasOperator;

namespace Retouch_Photo.Models
{

    public class Tool
    {
        public ToolType Type;

        public FrameworkElement Icon;
        public FrameworkElement WorkIcon;

        public FrameworkElement Page;
        public ToolViewModel ViewModel;
    }


    public enum ToolType
    {
        /// <summary>光标 </summary>
        Cursor,
        /// <summary>小手 </summary>
        View,
        /// <summary>魔棒 </summary>
        FloodSetect,
        /// <summary>选区笔刷 </summary>
        SelectionBrush,

        /// <summary>矩形选区 </summary>
        RectangularMarquee,
        /// <summary>椭圆选区 </summary>
        EllipticalMarquee,
        /// <summary>几何选区 </summary>
        PolygonMarquee,
        /// <summary>自由选区 </summary>
        FreeHandMarquee,

        /// <summary>画笔 </summary>
        PaintBrush,
        /// <summary>水笔 </summary>
        WatercolorPen,
        /// <summary>铅笔 </summary>
        Pencil,
        /// <summary>橡皮 </summary>
        EraseBrush,

        /// <summary>钢笔 </summary>
        Pen,
        /// <summary>矩形几何 </summary>
        Rectangle,
        /// <summary>圆形几何 </summary>
        Ellipse,
        /// <summary>几何 </summary>
        Geometry,

    }

}
