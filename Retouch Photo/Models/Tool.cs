using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models.Tools;
using Retouch_Photo.ViewModels;
using Retouch_Photo.ViewModels.ToolViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Models
{
    public abstract class ToolPage:Page
    {
        //@Override
        /// <summary> 当前页面成为活动页面 </summary>
        public abstract void ToolOnNavigatedTo();
        /// <summary> 当前页面不再成为活动页面 </summary>
        public abstract void ToolOnNavigatedFrom();
    }

    public abstract class Tool
    {
        public ToolType Type;

        public FrameworkElement Icon;
        public FrameworkElement WorkIcon;

        public ToolPage Page;

        public ToolViewModel ViewModel;

        public static List<Tool> ToolList = new List<Tool>
        {
             new CursorTool(),
             new ViewTool(),
             new FloodSetectTool(),
             new SelectionBrushTool(),

             new PaintBrushTool(),
             new WatercolorPenTool(),
             new PencilTool(),
             new EraseBrushTool(),

             new PenTool(),
             new RectangleTool(),
             new EllipseTool(),
             new GeometryTool(),

             new AcrylicTool(),
        };
    }

    public enum ToolType
    {
        /// <summary>光标</summary>
        Cursor,
        /// <summary>小手</summary>
        View,
        /// <summary>魔棒</summary>
        FloodSetect,
        /// <summary>选区笔刷</summary>
        SelectionBrush,
        
        /// <summary>画笔</summary>
        PaintBrush,
        /// <summary>水笔</summary>
        WatercolorPen,
        /// <summary>铅笔</summary>
        Pencil,
        /// <summary>橡皮</summary>
        EraseBrush,

        /// <summary>钢笔</summary>
        Pen,
        /// <summary>矩形几何</summary>
        Rectangle,
        /// <summary>圆形几何</summary>
        Ellipse,
        /// <summary>几何</summary>
        Geometry,


        /// <summary>亚克力</summary>
        Acrylic,
    }

}
