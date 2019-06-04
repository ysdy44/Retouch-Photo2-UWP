using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Tools.Models;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Tools
{
    public abstract class Tool
    {
        public ToolType Type;
        public FrameworkElement Icon;
        public FrameworkElement WorkIcon;
        public ToolPage Page;


        public abstract void Start(Vector2 point);
        public abstract void Delta(Vector2 point);
        public abstract void Complete(Vector2 point);

        public abstract void Draw(CanvasDrawingSession ds);


        //@Override
        /// <summary> 当前页面成为活动页面 </summary>
        public virtual void ToolOnNavigatedTo() { }
        /// <summary> 当前页面不再成为活动页面 </summary>
        public virtual void ToolOnNavigatedFrom() { }


        //ToolList
        public static Dictionary<ToolType, Tool> ToolList = new Dictionary<ToolType, Tool>
        {
             {ToolType.Cursor, new CursorTool()},
             {ToolType.View, new ViewTool()},

             {ToolType.PaintBrush, new PaintBrushTool()},
             {ToolType.Pencil, new PencilTool()},

             {ToolType.Pen, new PenTool()},
             {ToolType.Rectangle, new RectangleTool()},
             {ToolType.Ellipse, new EllipseTool()},
             {ToolType.Geometry, new GeometryTool()},

             {ToolType.Acrylic, new AcrylicTool()},
             {ToolType.Line, new LineTool()},
        };
    }
}
