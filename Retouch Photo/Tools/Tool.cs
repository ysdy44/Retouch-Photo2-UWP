using Microsoft.Graphics.Canvas;
using Retouch_Photo.Tools.Models;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo.Tools
{
    public abstract class Tool
    {
        public ToolType Type;
        public FrameworkElement Icon;
        public FrameworkElement WorkIcon;
        public ToolPage Page;
        

        //Operator
        public abstract void Start(Vector2 point);
        public abstract void Delta(Vector2 point);
        public abstract void Complete(Vector2 point);

        public abstract void Draw(CanvasDrawingSession ds);
        

        //@Override
        /// <summary> 当前页面成为活动页面 </summary>
        public abstract void ToolOnNavigatedTo();
        /// <summary> 当前页面不再成为活动页面 </summary>
        public abstract void ToolOnNavigatedFrom();
               

        //ToolList
        public static List<Tool> ToolList = new List<Tool>
        {
            Tool.CursorTool,
             Tool.ViewTool,
             Tool.FloodSetectTool,
             Tool.SelectionBrushTool,

             Tool.PaintBrushTool,
             Tool.WatercolorPenTool,
             Tool.PencilTool,
             Tool.EraseBrushTool,

             Tool.PenTool,
             Tool.RectangleTool,
             Tool.EllipseTool,
             Tool.GeometryTool,

             Tool.AcrylicTool,
             Tool.LineTool,
        };
        public static CursorTool CursorTool = new CursorTool();
        public static ViewTool ViewTool = new ViewTool();
        public static FloodSetectTool FloodSetectTool = new FloodSetectTool();
        public static SelectionBrushTool SelectionBrushTool = new SelectionBrushTool();

        public static PaintBrushTool PaintBrushTool = new PaintBrushTool();
        public static WatercolorPenTool WatercolorPenTool = new WatercolorPenTool();
        public static PencilTool PencilTool = new PencilTool();
        public static EraseBrushTool EraseBrushTool = new EraseBrushTool();

        public static PenTool PenTool = new PenTool();
        public static RectangleTool RectangleTool = new RectangleTool();
        public static EllipseTool EllipseTool = new EllipseTool();
        public static GeometryTool GeometryTool = new GeometryTool();

        public static AcrylicTool AcrylicTool = new AcrylicTool();
        public static LineTool LineTool = new LineTool();
    }
}
